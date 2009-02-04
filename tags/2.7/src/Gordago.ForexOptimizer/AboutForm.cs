/**
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Gordago {
  public class AboutForm : System.Windows.Forms.Form {
    private System.Windows.Forms.Button _btnOk;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.LinkLabel _lnklblSupportMail;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.LinkLabel _lnkWeb;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label _lblVersion;
    private System.Windows.Forms.Label _lblWarning;
    private System.Windows.Forms.Label _lblComment;
    private System.ComponentModel.Container components = null;

    public AboutForm() {
      InitializeComponent();
      string version = System.Windows.Forms.Application.ProductVersion;
      string[] sa = version.Split(new char[] { '.' });
      this._lblVersion.Text = "Version: " + sa[0] + "." + sa[1] + " Build " + sa[2];
#if DEMO
			this._lblVersion.Text = this._lblVersion.Text + "\n DEMO";
#endif
      this._lblComment.Text = Language.Dictionary.GetString(20, 1);
      this._lblWarning.Text = Language.Dictionary.GetString(20, 2);
      this.Text = Language.Dictionary.GetString(20, 3, "О программе...");
    }
    protected override void Dispose(bool disposing) {
      if (disposing) {
        if (components != null) {
          components.Dispose();
        }
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this._btnOk = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this._lblComment = new System.Windows.Forms.Label();
      this._lnklblSupportMail = new System.Windows.Forms.LinkLabel();
      this.label3 = new System.Windows.Forms.Label();
      this._lnkWeb = new System.Windows.Forms.LinkLabel();
      this.label4 = new System.Windows.Forms.Label();
      this._lblVersion = new System.Windows.Forms.Label();
      this._lblWarning = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // _btnOk
      // 
      this._btnOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this._btnOk.Location = new System.Drawing.Point(481, 264);
      this._btnOk.Name = "_btnOk";
      this._btnOk.Size = new System.Drawing.Size(75, 23);
      this._btnOk.TabIndex = 0;
      this._btnOk.Text = "OK";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.label1.Location = new System.Drawing.Point(248, 32);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(192, 20);
      this.label1.TabIndex = 1;
      this.label1.Text = "Gordago Software Ltd.";
      // 
      // _lblComment
      // 
      this._lblComment.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this._lblComment.Location = new System.Drawing.Point(192, 64);
      this._lblComment.Name = "_lblComment";
      this._lblComment.Size = new System.Drawing.Size(368, 32);
      this._lblComment.TabIndex = 1;
      // 
      // _lnklblSupportMail
      // 
      this._lnklblSupportMail.AutoSize = true;
      this._lnklblSupportMail.Location = new System.Drawing.Point(200, 104);
      this._lnklblSupportMail.Name = "_lnklblSupportMail";
      this._lnklblSupportMail.Size = new System.Drawing.Size(115, 13);
      this._lnklblSupportMail.TabIndex = 2;
      this._lnklblSupportMail.TabStop = true;
      this._lnklblSupportMail.Text = "support@gordago.com";
      this._lnklblSupportMail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._lnklblSupportMail_LinkClicked);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.label3.Location = new System.Drawing.Point(192, 136);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(302, 17);
      this.label3.TabIndex = 1;
      this.label3.Text = "Copyright © 2004-2007 Gordago Software Ltd.";
      // 
      // _lnkWeb
      // 
      this._lnkWeb.AutoSize = true;
      this._lnkWeb.Location = new System.Drawing.Point(200, 168);
      this._lnkWeb.Name = "_lnkWeb";
      this._lnkWeb.Size = new System.Drawing.Size(127, 13);
      this._lnkWeb.TabIndex = 2;
      this._lnkWeb.TabStop = true;
      this._lnkWeb.Text = "http://www.gordago.com";
      this._lnkWeb.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._lnkWeb_LinkClicked);
      // 
      // label4
      // 
      this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.label4.Location = new System.Drawing.Point(8, 176);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(144, 16);
      this.label4.TabIndex = 1;
      this.label4.Text = "Forex Optimizer TT";
      // 
      // _lblVersion
      // 
      this._lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this._lblVersion.Location = new System.Drawing.Point(16, 200);
      this._lblVersion.Name = "_lblVersion";
      this._lblVersion.Size = new System.Drawing.Size(128, 40);
      this._lblVersion.TabIndex = 1;
      this._lblVersion.Text = "Version:";
      this._lblVersion.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // _lblWarning
      // 
      this._lblWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this._lblWarning.Location = new System.Drawing.Point(192, 192);
      this._lblWarning.Name = "_lblWarning";
      this._lblWarning.Size = new System.Drawing.Size(368, 72);
      this._lblWarning.TabIndex = 1;
      this._lblWarning.Text = "Внимание! Данный продукт ";
      // 
      // AboutForm
      // 
      this.AcceptButton = this._btnOk;
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.CancelButton = this._btnOk;
      this.ClientSize = new System.Drawing.Size(562, 290);
      this.Controls.Add(this._lnklblSupportMail);
      this.Controls.Add(this.label1);
      this.Controls.Add(this._btnOk);
      this.Controls.Add(this._lblComment);
      this.Controls.Add(this.label3);
      this.Controls.Add(this._lnkWeb);
      this.Controls.Add(this.label4);
      this.Controls.Add(this._lblVersion);
      this.Controls.Add(this._lblWarning);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Name = "AboutForm";
      this.ShowInTaskbar = false;
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "О программе...";
      this.DragDrop += new System.Windows.Forms.DragEventHandler(this.AboutForm_DragDrop);
      this.ResumeLayout(false);
      this.PerformLayout();

    }
    #endregion

    protected override void OnPaint(PaintEventArgs e) {
      base.OnPaint(e);
      e.Graphics.DrawLine(new Pen(Color.Black, 1), 170, 20, 170, this.Height - 40);
    }

    private void _lnklblSupportMail_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e) {
      System.Diagnostics.Process.Start("mailto:" + this._lnklblSupportMail.Text);
    }

    private void _lnkWeb_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e) {
      System.Diagnostics.Process.Start(this._lnkWeb.Text);
    }

    private void AboutForm_DragDrop(object sender, System.Windows.Forms.DragEventArgs e) {

    }

  }
}
