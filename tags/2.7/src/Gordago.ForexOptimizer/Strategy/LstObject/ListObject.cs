/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Diagnostics;
using System.Collections;

namespace Gordago.LstObject {
	
	/// <summary>
	/// Изначально разработан для использования в просмотре индикаторов
	/// </summary>
	public class ListObject: ListView {
		protected ColumnHeader header;
		
		/// <summary>
		/// Хранение объектов в items
		/// </summary>
		private ArrayList alist;

		private System.ComponentModel.Container components = null;
		public ListObject(): base() {
			InitializeComponent();
			this.header =  new ColumnHeader();
			this.header.Text = "GSO name";
			this.header.TextAlign = HorizontalAlignment.Center;
			this.header.Width = this.Width;

			base.AllowDrop = false;
			
			this.Columns.Add(this.header);
			this.View = View.Details;
			this.MultiSelect = false;
			alist = new ArrayList();
		}

		/// <summary>
		/// Подпись заголовка
		/// </summary>
		[Category("Appearance")]
		public String HeaderText {
			get{return this.header.Text;}
			set{this.header.Text = value;}
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			components = new System.ComponentModel.Container();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}
		#endregion

		public void SetHeaderWidth(){
			this.Columns[0].Width= this.ClientSize.Width-5;
		}

    protected override void OnResize(EventArgs e) {
      if(Columns.Count == 1)
        this.SetHeaderWidth();
      base.OnResize(e);
    }

		#region DragAndDrop
		protected override void OnDragOver(DragEventArgs drgevent) {
			drgevent.Effect = DragDropEffects.Move;
			base.OnDragOver (drgevent);
		}

			
		protected override void OnItemDrag(ItemDragEventArgs e) {
			if (base.AllowDrop)
				base.DoDragDrop(GetDataForDragDrop(), DragDropEffects.Move);
			base.OnItemDrag (e);
		}

		private object GetDataForDragDrop() {
			DragItemData data = new DragItemData(this);

			foreach(ListObjItem item in this.SelectedItems) {
				data.DragItems.Add(item.OverObject);
			}

			return data.DragItems[0];
		}

		#endregion
	}
}
