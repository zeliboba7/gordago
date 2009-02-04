using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Gordago.Chart;

namespace Gordago.Stock {
	public class SymbolListForm : DataGrid {

		private DataTable _tbl;

		public SymbolListForm() {
			this.Location = new System.Drawing.Point(0, 0);
			this.Dock = DockStyle.Left;
			this.BorderStyle = BorderStyle.None;
			this.RowHeadersVisible = false;

//			this.View = View.Details;
//			this.FullRowSelect = true;
//			this.GridLines = true;
//			this.MultiSelect = false;
//			this.Columns.Add(Language.Dictionary.GetString(18,1,"Инструменты"), 55, HorizontalAlignment.Left);
//			this.Columns.Add("  Bid  ", 55, HorizontalAlignment.Left);
//			this.Columns.Add("  Ask  ", 55, HorizontalAlignment.Left);
//			this.UpdateSymbol();

			_tbl = new DataTable("symbols");
			_tbl.Columns.Add("sname", typeof(string));
			_tbl.Columns.Add("bid", typeof(string));
			_tbl.Columns.Add("ask", typeof(string));

			CurrencyManager cm = (CurrencyManager)FormMain.MainForm.BindingContext[_tbl];
			DataGridTableStyle gts = new DataGridTableStyle(cm);
			foreach (DataGridColumnStyle dcts in gts.GridColumnStyles){
				dcts.NullText = "";
				dcts.ReadOnly = true;
				switch(dcts.MappingName){
					case "sname":
						dcts.HeaderText = Language.Dictionary.GetString(18,1);
						break;
					case "bid":
						dcts.HeaderText = "Bid";
						break;
					case "ask":
						dcts.HeaderText = "Ask";
						break;
				}
			}
			
			DataGridColumnStyle cts = gts.GridColumnStyles["sname"];

			gts.ReadOnly = true;
			gts.RowHeadersVisible = false;
			this.CaptionVisible = false;
			this.ReadOnly = true;
			this.TableStyles.Clear();
			this.TableStyles.Add(gts);
			this.DataSource = _tbl.DefaultView;
			this._tbl.DefaultView.AllowEdit = false;

			UpdateSymbol();
			this.Size = new System.Drawing.Size(170, 300);
		}

		#region protected override void OnResize(EventArgs e) 
		protected override void OnResize(EventArgs e) {
			base.OnResize (e);
			DataGridTableStyle gts = this.TableStyles[0];
			foreach (DataGridColumnStyle dcts in gts.GridColumnStyles){
				switch(dcts.MappingName){
					case "sname":
						dcts.Width = this.ClientSize.Width - 110;
						break;
					default:
						dcts.Width = 55;
						break;
				}
			}
		}
		#endregion


		#region protected override void OnDoubleClick(EventArgs e) 
		protected override void OnDoubleClick(EventArgs e) {
			base.OnDoubleClick (e);

//			if (!this._enabled) return;
//
//			if (this.SelectedItems.Count <= 0) return;
//			string txt = this.SelectedItems[0].Text;
//
//			this.Cursor = Cursors.WaitCursor;
//			Symbol symbol = StockEngine.Symbols[txt];
//			symbol.LoadData();
//
//			ChartForm cf = new ChartForm(symbol);
//			cf.MdiParent = this.Parent as Form;
//			cf.Show();
//			this.Cursor = Cursors.Default;
		}
		#endregion

		public void UpdateSymbol(){

			foreach(Symbol symbol in StockEngine.Symbols.Symbols){
				DataRow srow = null;
				foreach (DataRow row in this._tbl.Rows){
					if (symbol.Name == (string)row["sname"]){
						srow = row;
					}
				}
				if (srow == null){
					srow = _tbl.NewRow();
					srow["sname"] = symbol.Name;
					this._tbl.Rows.Add(srow);
				}
			}
			//this.row
//			bool lb = false;
//			foreach (DataRowView rowv in this._tbl.DefaultView){
//				if (lb){
//					lb = false;
//				}else{
//					lb = true;
//				}
//			}
		}

		#region public void ClearSelectedItem()
		public void ClearSelectedItem(){
//			foreach (ListViewItem lvi in this.Items){
//				lvi.Selected = false;
//			}
		}
		#endregion

		protected override void OnClick(EventArgs e) {
			base.OnClick (e);
			this.Select(this.CurrentCell.RowNumber);
//			if (!this._enabled) return;
//			if (this.SelectedItems.Count < 1) return;
//
//			Form frm = FormMain.MainForm.ActiveMdiChild;
//			if (frm == null) return;
//
//			if (frm is WorkForm){
//				WorkForm wf = frm as WorkForm;
//				
//				FormMain.MainForm.SETesterPanel.SetSymbol(StockEngine.Symbols.GetSymbol(this.SelectedItems[0].Text));
//
//			}
		}

		#region private void ShowWarning(string sname)
		private void ShowWarning(string sname){
			string str = Language.Dictionary.GetString(13,10,"По %%symbol%% отсутствует минутная история.");
			str = str.Replace("%%symbol%%", sname);
			MessageBox.Show(str, OverStart.MessageCaption);
		}
		#endregion

		#region internal void SetSymbolTested(SymbolTested[] st)
		internal void SetSymbolTested(SymbolTested[] st){
			//			
			//			if (st == null) st = new SymbolTested[]{};
			//
			//			foreach (ListViewItem lvi in this.Items){
			//				if (lvi.ForeColor != Color.Black){
			//					lvi.ForeColor = Color.Black;
			//					lvi.Font = new Font(lvi.Font.FontFamily, lvi.Font.Size);
			//				}
			//				foreach (SymbolTested smbt in st){
			//					if (lvi.Text == smbt.Symbol.Name){
			//						lvi.ForeColor = Color.Red;
			//						lvi.Font = new Font(lvi.Font.FontFamily, lvi.Font.Size, FontStyle.Bold);
			//						break;
			//					}
			//				}
			//			}
		}
		#endregion

		#region internal void SetSymbolPrice(Symbol symbol)
		internal void SetSymbolPrice(Symbol symbol){
//			foreach (ListViewItem lvi in this.Items){
//				if (lvi.Text == symbol.Name){
//					lvi.SubItems[1].Text = symbol.Current.Bid.ToString();
//					lvi.SubItems[2].Text = symbol.Current.Ask.ToString();
//
//					foreach (History hst in StockEngine.Historyes.Historyes){
//						if (hst.Symbol.Name == symbol.Name){
//							hst.AddTick(symbol.Current.Time, symbol.Current.Bid, 1);
//						}
//					}
//					if (symbol.CurrentPriceStatus == Symbol.PriceStatus.UP){
//						lvi.ForeColor = Color.Blue;
//					}else if (symbol.CurrentPriceStatus == Symbol.PriceStatus.DOWN){
//						lvi.ForeColor = Color.Red;
//					}else{
//					}
//					break;
//				}
//			}
		}
		#endregion

		#region internal void SymchronizedSymbols()
		internal void SymchronizedSymbols(){
//			foreach(Symbol symbol in StockEngine.Symbols.Symbols){
//				bool find = false;
//				foreach (ListViewItem lvi in this.Items){
//					if (lvi.Text == symbol.Name){
//						lvi.SubItems[1].Text = symbol.Current.Bid.ToString();
//						lvi.SubItems[2].Text = symbol.Current.Ask.ToString();
//						find = true;
//						break;
//					}
//				}
//				if (!find){
//					ListViewItem lvi = new ListViewItem(symbol.Name);
//					lvi.SubItems.Add(symbol.Current.Bid.ToString());
//					lvi.SubItems.Add(symbol.Current.Ask.ToString());
//					this.Items.Add(lvi);
//				}
//			}
		}
		#endregion

		#region public void SetGlobalStatus(GlobalStatus status)
		public void SetGlobalStatus(GlobalStatus status){
		}
		#endregion
	}
}
