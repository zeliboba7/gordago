/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Cursit.Applic.AConfig;

namespace Gordago.Strategy {
	class SEditorRowCompile : System.Windows.Forms.UserControl {
		private System.ComponentModel.Container components = null;
		private SEditorRowCompileStatus _status;
		private SEditorRow _serow;

		public SEditorRowCompile(SEditorRow serow) {
			InitializeComponent();
			_status = SEditorRowCompileStatus.Empty;
			_serow = serow;
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
			// StrategySetRow
			// 
			this.Name = "StrategySetRow";
			this.Size = new System.Drawing.Size(32, 32);

		}
		#endregion

		#region public SEditorRowCompileStatus Status

		public SEditorRowCompileStatus Status{
			get{return _status;}
			set{_status = value;}
		}
		#endregion

		#region protected override void OnPaint(PaintEventArgs e)
		protected override void OnPaint(PaintEventArgs e) {
			Graphics g = e.Graphics;
			Image img = null;
			switch (_status){
				case SEditorRowCompileStatus.Empty:
					return;
				case SEditorRowCompileStatus.Compile:
					img = SEditorRow.ImageOk;
					break;
				case SEditorRowCompileStatus.Stop:
					img = SEditorRow.ImageNo;
					break;
				case SEditorRowCompileStatus.Question:
					img = SEditorRow.ImageMb;
					break;
			}
			int x = this.Width/2 - img.Width/2;
			int y = this.Height/2 - img.Height/2;
			g.DrawImage(img, x, y, img.Width, img.Height);
		}
		#endregion

		#region protected override void OnClick(EventArgs e)
		protected override void OnClick(EventArgs e) {
			base.OnClick (e);
			_serow.TextBoxObject.Focus();
		}
		#endregion

	}

	#region public enum SEditorRowCompileStatus
	/// <summary>
	/// Статус строки редактора
	/// </summary>
	public enum SEditorRowCompileStatus{
		/// <summary>
		/// Строка пуста, или не готова к использованию
		/// </summary>
		Empty,
		/// <summary>
		/// Запрещена для использования
		/// </summary>
		Stop,
		/// <summary>
		/// Запрещена для использования, и под вопросом
		/// </summary>
		Question, 
		/// <summary>
		/// Готова к использованию
		/// </summary>
		Compile
	}
	#endregion
}
