﻿namespace Gordago.FO.Instruments {
  partial class IndicatorsWindow {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this._treeView = new System.Windows.Forms.TreeView();
      this.SuspendLayout();
      // 
      // _treeView
      // 
      this._treeView.Dock = System.Windows.Forms.DockStyle.Fill;
      this._treeView.FullRowSelect = true;
      this._treeView.Location = new System.Drawing.Point(0, 0);
      this._treeView.Name = "_treeView";
      this._treeView.Size = new System.Drawing.Size(211, 347);
      this._treeView.TabIndex = 0;
      this._treeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this._treeView_ItemDrag);
      // 
      // IndicatorsWindow
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(211, 347);
      this.Controls.Add(this._treeView);
      this.Name = "IndicatorsWindow";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TreeView _treeView;
  }
}
