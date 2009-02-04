/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Gordago.Strategy {

  

	/// <summary>
	/// Элемент текстового поля
	/// </summary>
	public abstract class TextBoxObjElement {

		private object _element;
		private int _left, _top, _width, _height;

		private bool _isunderline;
		private Pen _underlinePen;


		public TextBoxObjElement(object element) {
			this._element = element;
			_isunderline = false;
			_underlinePen = new Pen(Color.Red, 3);
		}

		#region Public propertyes
		protected object Elmnt{
			get{return _element;}
		}

		public virtual int Width{
			get{return _width;}
			set{_width = value;}
		}
		public virtual int Height{
			get{return this._height;}
			set{this._height = value;}
		}
		public virtual int Top{
			get{return this._top;}
			set{this._top = value;}
		}
		public virtual int Left{
			get{return this._left;}
			set{this._left = value;}
		}
		#endregion

		#region public bool IsUnderLine - Подчеркивается ли этот элемент
		/// <summary>
		/// Подчеркивается ли этот элемент
		/// </summary>
		public bool IsUnderLine{
			get{return _isunderline;}
			set{_isunderline = value;}
		}
	#endregion

    #region public Pen UnderLinePen
    /// <summary>
		/// Карандаш подчеркивание элемента
		/// </summary>
		public Pen UnderLinePen{
			get{return this._underlinePen;}
			set{_underlinePen = value;}
    }
    #endregion

    #region internal void Paint(PaintEventArgs e)
    internal void Paint(PaintEventArgs e){
			this.DrawElement(e);
			if (this.IsUnderLine)
				this.DrawUnderLine(e);
    }
    #endregion

    protected abstract void DrawElement(PaintEventArgs e);

    #region protected virtual void DrawUnderLine(PaintEventArgs e)
    protected virtual void DrawUnderLine(PaintEventArgs e){
			int x1 = this.Left;
			int x2 = this.Left + this.Width + 2;
			int y = this.Top+this.Height+1;
			e.Graphics.DrawLine(this.UnderLinePen, x1,y, x2,y);
    }
    #endregion
  }
}
