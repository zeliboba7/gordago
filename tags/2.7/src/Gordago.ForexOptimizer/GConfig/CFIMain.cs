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

using Language;

namespace Gordago.GConfig {
	public class CFIMain : ConfigFormItem {
		private System.Windows.Forms.Label label1;
		private System.ComponentModel.Container components = null;

		public CFIMain() {
			InitializeComponent();

			this.Text = Dictionary.GetString(9,1);
		}

		#region Component Designer generated code
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if(components != null) {
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		private void InitializeComponent() {
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(136, 160);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(162, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Общие настройки программы";
			// 
			// CFIMain
			// 
			this.Controls.Add(this.label1);
			this.Name = "CFIMain";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
