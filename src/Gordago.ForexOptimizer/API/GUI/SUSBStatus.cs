/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Gordago.Strategy {
	public class SUSBStatus : System.Windows.Forms.UserControl {
		private System.ComponentModel.Container components = null;

		private Image _imgsellin, _imgsellout, _imgbuyin, _imgbuyout, _imgsellc, _imgbuyc;

		private SUSBStatusType _sellstatus, _buystatus;

		public SUSBStatus() {
			InitializeComponent();
			_sellstatus = _buystatus = SUSBStatusType.Empty;
		}

		public SUSBStatusType SellStatus{
			get{return this._sellstatus;}
		}

		public SUSBStatusType BuyStatus{
			get{return this._buystatus;}
		}

		public void InitImage(){
			_imgsellin = GordagoImages.Images.GetImage("sell", "strg");
			_imgsellout = GordagoImages.Images.GetImage("sellExit", "strg");
			_imgbuyin = GordagoImages.Images.GetImage("buy", "strg");
			_imgbuyout = GordagoImages.Images.GetImage("buyExit", "strg");
			_imgbuyout = GordagoImages.Images.GetImage("buy", "strg");
			_imgsellc =  GordagoImages.Images.GetImage("sellc", "strg");
			_imgbuyc =  GordagoImages.Images.GetImage("buyc", "strg");
		}

		public void SetSellStatus(SUSBStatusType status){
			this._sellstatus = status;
			this.Refresh();
		}

		public void SetBuyStatus(SUSBStatusType status){
			this._buystatus = status;
			this.Refresh();
		}

		protected override void OnPaint(PaintEventArgs e) {
			base.OnPaint (e);
			if (_sellstatus != SUSBStatusType.Empty){
				Image imgs = null;
				switch(_sellstatus){
					case SUSBStatusType.In:
						imgs = _imgsellin;
						break;
					case SUSBStatusType.Out:
//						imgs = _imgsellout;
//						break;
					case SUSBStatusType.Starting:
						imgs = _imgsellc;
						break;
				}
				e.Graphics.DrawImage(imgs, 0, 0); 
			}
			if (_buystatus != SUSBStatusType.Empty){
				Image imgb = null;
				switch(_buystatus){
					case SUSBStatusType.In:
						imgb = _imgbuyin;
						break;
					case SUSBStatusType.Out:
//						imgb = _imgbuyout;
//						break;
					case SUSBStatusType.Starting:
						imgb = _imgbuyc;
						break;
				}
				e.Graphics.DrawImage(imgb, 18, 0);
			}
		}


		#region protected override void Dispose( bool disposing )
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if(components != null) {
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			// 
			// SUSBStatus
			// 
			this.Name = "SUSBStatus";
			this.Size = new System.Drawing.Size(40, 24);

		}
		#endregion
	}

	public enum SUSBStatusType{
		Empty, 
		Starting,
		In, 
		Out
	}
}
