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

using Cursit.Utils;

namespace Gordago.Strategy {
	public class SEConfig : System.Windows.Forms.UserControl {
		private System.Windows.Forms.ComboBox _cmbsound;
		private System.Windows.Forms.Button _btnplay;
		private System.Windows.Forms.GroupBox _gbxSound;
		private System.Windows.Forms.GroupBox _gbxName;
		private System.Windows.Forms.TextBox _txtname;
		private System.Windows.Forms.GroupBox _gbxDescription;
		private System.Windows.Forms.RichTextBox _txtdesc;
		private System.ComponentModel.Container components = null;

		public SEConfig() {
			InitializeComponent();

			string[] files = System.IO.Directory.GetFiles(Application.StartupPath + "\\sound", "*.wav");
			this._cmbsound.Items.AddRange(Cursit.Utils.FileEngine.GetDisplayFiles(files));
			if (_cmbsound.Items.Count > 0)
				this._cmbsound.SelectedIndex = 0;
    }

    #region public string CName
    public string CName{
			get{return this._txtname.Text;}
			set{this._txtname.Text = value;}
    }
    #endregion

    #region public string CDescription
    public string CDescription{
			get{return this._txtdesc.Text;}
			set{this._txtdesc.Text = value;}
    }
    #endregion

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
			this._cmbsound = new System.Windows.Forms.ComboBox();
			this._btnplay = new System.Windows.Forms.Button();
			this._gbxSound = new System.Windows.Forms.GroupBox();
			this._gbxName = new System.Windows.Forms.GroupBox();
			this._txtname = new System.Windows.Forms.TextBox();
			this._gbxDescription = new System.Windows.Forms.GroupBox();
			this._txtdesc = new System.Windows.Forms.RichTextBox();
			this._gbxSound.SuspendLayout();
			this._gbxName.SuspendLayout();
			this._gbxDescription.SuspendLayout();
			this.SuspendLayout();
			// 
			// _cmbsound
			// 
			this._cmbsound.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._cmbsound.Location = new System.Drawing.Point(8, 16);
			this._cmbsound.Name = "_cmbsound";
			this._cmbsound.Size = new System.Drawing.Size(165, 21);
			this._cmbsound.TabIndex = 1;
			// 
			// _btnplay
			// 
			this._btnplay.Location = new System.Drawing.Point(184, 16);
			this._btnplay.Name = "_btnplay";
			this._btnplay.TabIndex = 2;
			this._btnplay.Text = "Play";
			this._btnplay.Click += new System.EventHandler(this._btnplay_Click);
			// 
			// _gbxSound
			// 
			this._gbxSound.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this._gbxSound.Controls.Add(this._btnplay);
			this._gbxSound.Controls.Add(this._cmbsound);
			this._gbxSound.Location = new System.Drawing.Point(8, 160);
			this._gbxSound.Name = "_gbxSound";
			this._gbxSound.Size = new System.Drawing.Size(440, 48);
			this._gbxSound.TabIndex = 3;
			this._gbxSound.TabStop = false;
			this._gbxSound.Text = "Sound";
			// 
			// _gbxName
			// 
			this._gbxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this._gbxName.Controls.Add(this._txtname);
			this._gbxName.Location = new System.Drawing.Point(8, 8);
			this._gbxName.Name = "_gbxName";
			this._gbxName.Size = new System.Drawing.Size(440, 48);
			this._gbxName.TabIndex = 4;
			this._gbxName.TabStop = false;
			this._gbxName.Text = "Name";
			// 
			// _txtname
			// 
			this._txtname.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this._txtname.Location = new System.Drawing.Point(8, 16);
			this._txtname.Name = "_txtname";
			this._txtname.Size = new System.Drawing.Size(424, 20);
			this._txtname.TabIndex = 0;
			this._txtname.Text = "";
			// 
			// _gbxDescription
			// 
			this._gbxDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this._gbxDescription.Controls.Add(this._txtdesc);
			this._gbxDescription.Location = new System.Drawing.Point(8, 56);
			this._gbxDescription.Name = "_gbxDescription";
			this._gbxDescription.Size = new System.Drawing.Size(440, 104);
			this._gbxDescription.TabIndex = 5;
			this._gbxDescription.TabStop = false;
			this._gbxDescription.Text = "Description";
			// 
			// _txtdesc
			// 
			this._txtdesc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this._txtdesc.Location = new System.Drawing.Point(8, 16);
			this._txtdesc.Name = "_txtdesc";
			this._txtdesc.Size = new System.Drawing.Size(424, 80);
			this._txtdesc.TabIndex = 0;
			this._txtdesc.Text = "";
			// 
			// SEConfig
			// 
			this.Controls.Add(this._gbxDescription);
			this.Controls.Add(this._gbxName);
			this.Controls.Add(this._gbxSound);
			this.Name = "SEConfig";
			this.Size = new System.Drawing.Size(456, 216);
			this._gbxSound.ResumeLayout(false);
			this._gbxName.ResumeLayout(false);
			this._gbxDescription.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region public void SetSoundFileName(string filename)
		public void SetSoundFileName(string filename){
			foreach (object obj in this._cmbsound.Items){
				FileEngine.DisplayFile dfile = (FileEngine.DisplayFile)obj;
				if (filename == dfile.DisplayName){
					this._cmbsound.SelectedItem = dfile;
				}
			}
		}
		#endregion


		public string GetSoundFileName(){
			if (this._cmbsound.SelectedItem == null)
				return "";
			Cursit.Utils.FileEngine.DisplayFile df = this._cmbsound.SelectedItem as FileEngine.DisplayFile;
			return df.DisplayName;
		}

		#region private void _btnplay_Click(object sender, System.EventArgs e) 
		private void _btnplay_Click(object sender, System.EventArgs e) {
			this.PlaySound();
		}
		#endregion

		#region public void PlaySound()
		public void PlaySound(){
			if (this._cmbsound.SelectedItem == null)
				return;
			try{
				Cursit.Utils.FileEngine.DisplayFile dfile = (Cursit.Utils.FileEngine.DisplayFile)this._cmbsound.SelectedItem;
				Cursit.Win32API.PlaySoundFile(dfile.FileName);
			}catch{}
		}
		#endregion
	}
}
