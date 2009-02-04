/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using

using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using Cursit;

#endregion
namespace Gordago.Strategy {
	public class TextBoxObject: Control {

		#region public Events
		/// <summary>
		/// Событие - на пересчет позиций элементов в редакторе, возникает при 
		/// добавление или удаление элемента
		/// </summary>
		public event EventHandler ReCalculPosition;
		
		/// <summary>
		/// Тоже самое что и ReCalculPosition
		/// </summary>
		public event EventHandler ElementsChanged;

		/// <summary>
		/// Событие на изменение позиции
		/// </summary>
		public event EventHandler PositionChanged;

		/// <summary>
		/// Изменение высоты
		/// </summary>
		public event EventHandler HeighChanged;

		#endregion

		#region private propertyes
		private TextBoxObjElement[] _elements;
		private int _leftout, _rightout;
		private int _pos;
		private int _caretPositionX;
		private bool _isOutUnderLine = false;
		#endregion

		public const int HEIGHT_ROW = 22;

		#region public TextBoxObject():base() - constructor
		public TextBoxObject():base() {
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.Font = new Font("Microsoft Sans Serif", 12);

			this._elements = new TextBoxObjElement[]{};
			this.Height = HEIGHT_ROW;
			this._leftout = 5;
			this._rightout = 10;
			this.Width = _leftout + _rightout;
			this.Position = 0;
			this.BackColor = Color.White;
			this.Cursor = Cursors.IBeam;
		}
		#endregion

		#region public TextBoxObjElement[] Elements
		public TextBoxObjElement[] Elements{
			get{return _elements;}
		}
		#endregion

		#region public int Position
		public int Position{
			get{return _pos;}
			set{
				if (value > this.Elements.Length)
					value = this.Elements.Length;
				if (value < 0)
					value = 0;
				_pos = value;
				this.SetCaretPosition();
				if (this.PositionChanged != null)
					this.PositionChanged(this, new EventArgs());
			}
		}
		#endregion

		#region public int CaretPositionX
		public int CaretPositionX{
			get{return this._caretPositionX;}
		}
		#endregion

		#region public new int Height
		/// <summary>
		/// Изменения высоты строки + событие HeighChanged
		/// </summary>
		public new int Height{
			get{return base.Height;}
			set{
				if (value < HEIGHT_ROW) value = HEIGHT_ROW;
				if (base.Height == value) return;
				base.Height = value;
				if (this.HeighChanged != null)
					this.HeighChanged(this, new EventArgs());
			}
		}
		#endregion

		#region public object[] GetElementsFromType() - массив строк и контролов
		/// <summary>
		/// Массив элементов по типу. Т.е. char[] = string =>> массив строк и контролов
		/// </summary>
		public object[] GetElementsFromType(){
			if (this._elements.Length == 0) return new object[0];
			ArrayList objs = new ArrayList();
			ArrayList chars = null;
			foreach(TextBoxObjElement el in this._elements){
				if (el is TextBoxObjElementChar){
					if (chars == null)
						chars =  new ArrayList();
					chars.Add((el as TextBoxObjElementChar).Element);
				}else if(el is TextBoxObjElementCtrl){
					if (chars != null){
						char[] chs = new char[chars.Count];
						chars.CopyTo(chs);
						string s = new string(chs);
						objs.Add(s);
						chars = null;
					}
					objs.Add((el as TextBoxObjElementCtrl).Element);
				}
			}
			if (chars != null){
				char[] chs = new char[chars.Count];
				chars.CopyTo(chs);
				string s = new string(chs);
				objs.Add(s);
			}
			object[] obja = new object[objs.Count];
			objs.CopyTo(obja);
			return obja;
		}
		#endregion

		#region public void Add() - добавление элемента
		#region public void Add(object[] objs)
		public void Add(object[] objs){
			foreach(object obj in objs){
				if (obj is char){
					this.Add((char)obj);
				}else if (obj is string){
					this.Add((string)obj);
				}else if (obj is Control){
					this.Add(obj as Control);
				}
			}
		}
		#endregion
		#region public void Add(string str)
		public void Add(string str){
			this.Add(str.ToCharArray());
		}
		#endregion
		#region public void Add(char ch)
		public void Add(char ch){
			this.Add(new char[]{ch});
		}
		#endregion
		#region public void Add(char[] chars)
		public void Add(char[] chars){
			TextBoxObjElement[] tboels = new TextBoxObjElement[chars.Length];
			int i=0;
			foreach(char ch in chars){
				tboels[i++] = new TextBoxObjElementChar(ch);
			}
			this.Add(tboels);
		}
		#endregion
		#region public void Add(Control control)
		public void Add(Control control){
			TextBoxObjElementCtrl tboCtrl = new TextBoxObjElementCtrl(control);
			this.Add(tboCtrl);
			this.Controls.Add(control);
			tboCtrl.Resize += new EventHandler(this.Ctrl_Resize);
			control.MouseDown += new MouseEventHandler(this.Control_MouseDown);
		}
		#endregion
		#region public void Add(TextBoxObjElement tboElement)
		public void Add(TextBoxObjElement tboElement){
			this.Add(new TextBoxObjElement[]{tboElement});
		}
		#endregion
		#region public void Add(TextBoxObjElement[] tboElements)
		public void Add(TextBoxObjElement[] tboElements){
			int cntf = tboElements.Length;
			int cntt = this._elements.Length;
			int cnt = cntf+cntt;
			TextBoxObjElement[] tboels = new TextBoxObjElement[cnt];
			Array.Copy(this._elements, 0, tboels, 0, cntt);
			Array.Copy(tboElements, 0,tboels, cntt, cntf);
			this._elements = tboels;
			this.OnReCalculPositionElements();
			if (this.ElementsChanged != null)
				this.ElementsChanged(this, new EventArgs());

		}
		#endregion
		#endregion

		#region public void Insert() - вставка элемента
		#region public void Insert(int position, char ch)
		public void Insert(int position, char ch){
			this.Insert(position, new char[]{ch});
		}
		#endregion

		#region public void Insert(int position, char[] chars)
		public void Insert(int position, char[] chars){
			TextBoxObjElement[] tboels = new TextBoxObjElement[chars.Length];
			int i=0;
			foreach(char ch in chars){
				tboels[i++] = new TextBoxObjElementChar(ch);
			}
			this.Insert(position, tboels);
		}
		#endregion

		#region public void Insert(int position, Control control)
		public void Insert(int position, Control control){
			TextBoxObjElementCtrl tboCtrl = new TextBoxObjElementCtrl(control);
			this.Controls.Add(control);
			tboCtrl.Resize += new EventHandler(this.Ctrl_Resize);
			control.MouseDown += new MouseEventHandler(this.Control_MouseDown);
			this.Insert(position, tboCtrl);
		}
		#endregion
		#region public void Insert(int position, TextBoxObjElement tboElement)
		public void Insert(int position, TextBoxObjElement tboElement){
			this.Insert(position, new TextBoxObjElement[]{tboElement});;
		}
		#endregion
		#region public void Insert(int position, TextBoxObjElement[] tboElements)
		public void Insert(int position, TextBoxObjElement[] tboElements){
			ArrayList els = new ArrayList(this.Elements);
			els.InsertRange(position, tboElements);
			TextBoxObjElement[] elsa = new TextBoxObjElement[els.Count];
			els.CopyTo(elsa);
			this._elements = elsa;
			this.OnReCalculPositionElements();
			if (this.ElementsChanged != null)
				this.ElementsChanged(this, new EventArgs());

		}
		#endregion
		#endregion

		#region public RemoveAt() - удаление элемента
		public void RemoveAt(int index){
			if (index >= this.Elements.Length) return;
			if (index < 0) return;

			TextBoxObjElement el = this.Elements[index];
			if (el is TextBoxObjElementCtrl){
				TextBoxObjElementCtrl ctrl = el as TextBoxObjElementCtrl;
				ctrl.Element.MouseDown -= new MouseEventHandler(this.Control_MouseDown);
				ctrl.Resize -= new EventHandler(this.Ctrl_Resize);
				this.Controls.Remove(ctrl.Element);
			}
			ArrayList tboels = new ArrayList(this._elements);
			tboels.RemoveAt(index);
			TextBoxObjElement[] tboelsa = new TextBoxObjElement[tboels.Count];
			tboels.CopyTo(tboelsa);
			this._elements = tboelsa;
			this.OnReCalculPositionElements();

			if (this.ElementsChanged != null)
				this.ElementsChanged(this, new EventArgs());

		}
		#endregion

		#region private void Ctrl_Resize(object sender, EventArgs e)
		private void Ctrl_Resize(object sender, EventArgs e){
			this.OnReCalculPositionElements();
			this.SetCaretPosition();
		}
		#endregion

		#region private void ReCalculPositionElements() - Пересчет позиции элементов, Установка ширины строки
		/// <summary>
		/// Пересчет позиции элементов. Установка ширины строки
		/// </summary>
		private void ReCalculPositionElements(){

			int y = 0;
			foreach(TextBoxObjElement el in this._elements){
				y = Math.Max(y, el.Height);
			}

			this.Height = y;
			
			int x = this._leftout;

			foreach(TextBoxObjElement tboel in this._elements){
				tboel.Left = x;
				x += tboel.Width;
				tboel.Top = y - tboel.Height;
			}
			this.Width = x + this._rightout;
		}
		#endregion

		#region protected virtual void OnReCalculPositionElements()
		protected virtual void OnReCalculPositionElements(){
			this.ReCalculPositionElements();
			if (this.ReCalculPosition != null){
				this.ReCalculPosition(this, new EventArgs());
			}
			this._isOutUnderLine = false;
			this.Invalidate();
		}
		#endregion

		#region protected override void OnPaint(PaintEventArgs e)
		protected override void OnPaint(PaintEventArgs e) {
			base.OnPaint (e);
			foreach(TextBoxObjElement el in this.Elements){
				el.Paint(e);
			}

			if (this._isOutUnderLine && this.Elements.Length > 0){
				TextBoxObjElement el = this.Elements[this.Elements.Length-1];

				int x1 = el.Left + el.Width + 2;
				int x2 = x1 + 10;
				int y = el.Top + el.Height+1;
				e.Graphics.DrawLine(new Pen(Color.Red, 3), x1,y, x2,y);
			}
		}
		#endregion

		#region private void MouseDown() - событие, обработка клика мыши
		private void Control_MouseDown(object sender, MouseEventArgs e){
			//Control ctrl = sender as Control;
			//this.SetCaretPosition(ctrl.Left + e.X);
		}
		protected override void OnMouseDown(MouseEventArgs e) {
			base.OnMouseDown (e);
			this.Focus();
			this.SetCaretPosition(e.X);
		}
		#endregion

		#region public void SetCaretPosition(int x)
		public void SetCaretPosition(int x){
			if (x <= this._leftout){
				this.Position = 0;
			}else if(x >= this.Width-this._rightout){
				this.Position = this.Elements.Length;
			}else{
				int i=0;
				foreach (TextBoxObjElement el in this._elements){
					int x1= el.Left;
					int x2 = el.Left+el.Width;

					if (x >= x1 && x <= x2){

						if (x <= x1+(x2-x1)/2){
							this.Position = i;
						}else
							this.Position = i+1;
					}
					i++;
				}
			}
			this.SetCaretPosition();
		}
		#endregion 
		
		#region public void SetCaretPosition()
		public void SetCaretPosition(){
			int x = this._leftout;
			if (this.Position == 0){
			}else if (this.Position >= this.Elements.Length){
				x = this.Width - this._rightout;
			}else{
				x = this.Elements[this.Position].Left;
			}
			this._caretPositionX = x;

			/* определение позиции и высоты курсора */
			int hc = this.Font.Height;
			int cntel = this.Elements.Length;
			if (cntel > 0){
				TextBoxObjElement el = null;
				if (this.Position > 0){
					el = this.Elements[cntel-1];
				}else{
					el = this.Elements[0];
				}
				hc = el.Height;
			}


			Cursit.Win32API.CreateCaretApi(this.Handle, 1, hc);
			Cursit.Win32API.ShowCaretApi(this.Handle);
			Cursit.Win32API.SetCaretPosApi(x+2, 2);
		}
		#endregion

		#region override OnGotFocus & OnLostFocus
		protected override void OnGotFocus(EventArgs e) {
			base.OnGotFocus (e);
			this.Position = this.Position;
		}
		protected override void OnLostFocus(EventArgs e) {
			base.OnLostFocus (e);
			Cursit.Win32API.DestroyCaretApi();
		}
		#endregion

		#region public override bool PreProcessMessage(ref Message msg)
		public override bool PreProcessMessage(ref Message msg) {
			#region over comment
			//			int d = 0;
			//			d++;
			//			if (Keys.Alt == Control.ModifierKeys)
			//				return base.PreProcessMessage (ref msg);
			//			
			//			Keys keyDataArg;
			//			if ( ( msg.Msg == WMsg.WM_KEYDOWN || msg.Msg == WMsg.WM_CHAR || 
			//				msg.Msg == WMsg.WM_KEYUP ) &&
			//				this.GetKeyDataFromWParam( (int)msg.WParam, out keyDataArg ) ){
			//				
			//				if ( msg.Msg == WMsg.WM_KEYDOWN ){
			//					System.Diagnostics.Debug.WriteLine("WM_KEYDOWN");
			//				}
			//				if ( msg.Msg == WMsg.WM_CHAR ){
			//					System.Diagnostics.Debug.WriteLine("WM_CHAR");
			//				}
			//				if ( msg.Msg == WMsg.WM_KEYUP ){
			//					System.Diagnostics.Debug.WriteLine("WM_KEYUP");
			//				}
			//			}
			#endregion

			if ( msg.Msg == WMsg.WM_CHAR ){
				if ((int)msg.WParam > 31){
					this.Insert(this.Position, (char)msg.WParam);
					OnReCalculPositionElements();
					this.Position++;
					this.Invalidate();
					return base.PreProcessMessage (ref msg);
				}
			}
			#region Обработка клавиш управления
			if ( msg.Msg == WMsg.WM_KEYDOWN ){
				switch ((int)msg.WParam){
					case ProcessMsg.VK_RIGHT:
						this.Position++;
						return false;
					case ProcessMsg.VK_LEFT:
						this.Position--;
						return false;
					case ProcessMsg.VK_HOME:
						this.Position = 0;
						return false;
					case ProcessMsg.VK_END:
						this.Position = this.Elements.Length;
						return false;
					case ProcessMsg.VK_DEL:
						this.RemoveAt(this.Position);
						this.Refresh();
						return false;
					case ProcessMsg.VK_BACK:
						this.RemoveAt(this.Position-1);
						this.Position--;
						//this.SetCaretPosition();
						this.Refresh();
						return false;
				}
			}
			#endregion
			return base.PreProcessMessage (ref msg);
		}
		#endregion

		#region protected virtual bool CheckDragDropType(DragEventArgs drgevent) - Проверка обьекта на возможность вхождения по драгдропу
		/// <summary>
		/// Проверка обьекта на возможность вхождения по драгдропу
		/// </summary>
		/// <param name="obj">кидаемый объект по драгдропу</param>
		/// <returns>истина - этот объект может быть добавлен в этот компонент </returns>
		protected virtual bool CheckDragDropType(DragEventArgs drgevent){
			return false;
		}
		#endregion

		#region public void SetCaretPositionToEnd()
		public void SetCaretPositionToEnd(){
			this.Position = this.Elements.Length;
		}
		#endregion

		#region public void UnSetUnderLineAll() - Отменение лини подчеркивания на всех элементах
		/// <summary>
		/// Отменение лини подчеркивания на всех элементах
		/// </summary>
		public void UnSetUnderLineAll(){
			foreach (TextBoxObjElement el in this.Elements)
				el.IsUnderLine = false;
			this._isOutUnderLine = false;
		}
		#endregion

		#region public bool SetUnderLine(int indexElement)
		public bool SetUnderLine(int indexElement){
			if (indexElement >= this.Elements.Length){
				this._isOutUnderLine = true;
				return true;
			}
			this.Elements[indexElement].IsUnderLine = true;
			this.Invalidate();
			return true;
		}
		#endregion

		#region public bool SetUnderLineAllElement()
		public bool SetUnderLineAllElement(){
			foreach (TextBoxObjElement el in this.Elements)
				el.IsUnderLine = true;
			this.Invalidate();
			return true;
		}
		#endregion
	}
}
