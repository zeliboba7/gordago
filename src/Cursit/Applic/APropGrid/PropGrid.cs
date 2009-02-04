using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Cursit.Applic.APropGrid {
	public class PropGrid : System.Windows.Forms.UserControl{
		private System.ComponentModel.Container components = null;
		private PropGridRowCollection _items;
		private int _splitter = -1;

		public PropGrid() {
			this._items = new PropGridRowCollection();

			InitializeComponent();
			this.AutoScroll = true;
		}

		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			components = new System.ComponentModel.Container();
		}
		#endregion

		#region protected override void CreateHandle() 
		protected override void CreateHandle() {
			base.CreateHandle ();
			this._items.SelectedItemChanged += new PropGridRowsEventHandler(PGR_SelectedItem);
			this._items.SplitterChanged += new PropGridRowsEventHandler(this.PGR_SplitterChanged);
			this.BackColor = Color.White;
		}
		#endregion

		#region private void PGR_SelectedItem(object seneder, System.EventArgs e)
		private void PGR_SelectedItem(object seneder, System.EventArgs e){
			this.Refresh();
		}
		#endregion

		#region private void PGR_SplitterChanged(object sender, EventArgs e)
		private void PGR_SplitterChanged(object sender, EventArgs e){
			this._splitter = ((PropGridRowPrimitive)sender).Splitter;
			this.Refresh();
		}
		#endregion

		#region private int Add(PropGridRowPrimitive pgrp)
		private int Add(PropGridRowPrimitive pgrp){
			pgrp.Location = new Point(0, this._items.Height);

			if (this._splitter > -1)
				pgrp.Splitter = this._splitter;
			this.Controls.Add(pgrp);
			this._items.Add(pgrp);
			pgrp.Width = this.ClientRectangle.Width;
			return pgrp.Id;
		}
		#endregion

		#region public int Add(string caption)
		public int Add(string caption){
			PropGridRowPrimitive pgrp = new PropGridRowCaption(caption);
			return this.Add(pgrp);
		}
		#endregion

    #region public int Add(PropGridValue Value)
    public int Add(PropGridValue Value){
			PropGridRowPrimitive pgrp = null;
			switch(Value.ValueType){
				case PropGridValueType.Color:
					pgrp = new PropGridRowColor(Value as PropGridValueColor);
					break;
				case PropGridValueType.Text:
					pgrp = new PropGridRowText(Value as PropGridValueText);
					break;
				case PropGridValueType.Number:
					pgrp = new PropGridRowNumber(Value as PropGridValueNumber);
					break;
				case PropGridValueType.Float:
					pgrp = new PropGridRowNumber(Value as PropGridValueFloat);
					break;
				case PropGridValueType.ArrayStrings:
					pgrp = new PropGridRowArrString(Value as PropGridValueArrString);
					break;
				case PropGridValueType.Multi:
					pgrp = new PropGridRowMulti(Value as PropGridValueMulti);
					break;
				case PropGridValueType.Period:
					pgrp = new PropGridRowPeriod(Value as PropGridValuePeriod);
					break;
			}
			return this.Add(pgrp);
    }
    #endregion

    #region protected override void OnResize(EventArgs e)
    protected override void OnResize(EventArgs e) {
			base.OnResize (e);
			int cnt = _items.Count;
			for (int i=0;i<cnt;i++){
				_items[i].Width = this.ClientRectangle.Width;
			}
    }
    #endregion

    public void Clear(){
			this._items.Clear();
			this.Controls.Clear();
		}

		protected override void OnLostFocus(EventArgs e) {
			base.OnLostFocus (e);
			this._items.UnSelectAllRow();
		}

		public int Count{
			get{return this._items.Count;}
		}

		public PropGridValue this[int index]{
			get{
				return _items[index].PropGridValue;
			}
		}

	}
}
