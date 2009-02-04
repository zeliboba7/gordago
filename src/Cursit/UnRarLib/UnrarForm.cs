/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Cursit.UnRarLib {
	public class UnrarForm : System.Windows.Forms.Form {
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label _lblReady;
		private System.Windows.Forms.Button _btnCancel;

		private Unrar unrar;
		private string[] _files;
		private System.Windows.Forms.ProgressBar _pgsUnRar;

		public UnrarForm() {
			InitializeComponent();
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this._pgsUnRar = new System.Windows.Forms.ProgressBar();
			this._btnCancel = new System.Windows.Forms.Button();
			this._lblReady = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// _pgsUnRar
			// 
			this._pgsUnRar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this._pgsUnRar.Location = new System.Drawing.Point(6, 30);
			this._pgsUnRar.Name = "_pgsUnRar";
			this._pgsUnRar.Size = new System.Drawing.Size(474, 20);
			this._pgsUnRar.TabIndex = 7;
			// 
			// _btnCancel
			// 
			this._btnCancel.Location = new System.Drawing.Point(486, 30);
			this._btnCancel.Name = "_btnCancel";
			this._btnCancel.TabIndex = 8;
			this._btnCancel.Text = "Cancel";
			// 
			// _lblReady
			// 
			this._lblReady.AutoSize = true;
			this._lblReady.Location = new System.Drawing.Point(10, 10);
			this._lblReady.Name = "_lblReady";
			this._lblReady.Size = new System.Drawing.Size(23, 16);
			this._lblReady.TabIndex = 9;
			this._lblReady.Text = "File";
			// 
			// UnrarForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(484, 58);
			this.Controls.Add(this._lblReady);
			this.Controls.Add(this._btnCancel);
			this.Controls.Add(this._pgsUnRar);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "UnrarForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Extract files";
			this.ResumeLayout(false);

		}
		#endregion

		#region private void ShowProgress(string filename, int total, int current)
		delegate void ShowProgressDelegate(string filename, int total, int current);

		private void ShowProgress(string filename, int total, int current){
			try{
				if (!this._lblReady.InvokeRequired){
					this._lblReady.Text = filename;
					this._pgsUnRar.Maximum = total;
					this._pgsUnRar.Value = current;
				}else{
					ShowProgressDelegate showdel = new ShowProgressDelegate(ShowProgress);
					BeginInvoke(showdel, new object[]{filename, total, current});
				}
			}catch(Exception){}
		}
		#endregion

		public void StartExtract(string[] files){
			_files = files;
			ExtractFilesDelegate exdel = new ExtractFilesDelegate(this.ExtractFiles);
			exdel.BeginInvoke(null, null);
		}

		private delegate void ExtractFilesDelegate();

		private void ExtractFiles(){
			foreach (string file in this._files){
				this.ExtractFile(file);
			}
			this.Close();
		}
		private void ExtractFile(string filename){
			try {
				string directory = Path.GetDirectoryName(filename);
				unrar=new Unrar();
				AttachHandlers(unrar);

				unrar.DestinationPath=directory;
				unrar.Open(filename, Unrar.OpenMode.Extract);

				while(unrar.ReadHeader()) {
					this.ShowProgress(unrar.CurrentFile.FileName, 100, 0);
					unrar.Extract();
				}
			} catch(Exception) {
				this.ShowProgress("Erron in unrar", 100, 0);
			} finally {
				this.ShowProgress("Complete", 0,0);
				if(this.unrar!=null)
					unrar.Close();
			}
		}

		private void AttachHandlers(Unrar unrar) {
			unrar.ExtractionProgress+=new ExtractionProgressHandler(unrar_ExtractionProgress);
		}

		private void unrar_ExtractionProgress(object sender, ExtractionProgressEventArgs e) {
			this.ShowProgress(e.FileName, 100, (int)e.PercentComplete);
		}
	}
}