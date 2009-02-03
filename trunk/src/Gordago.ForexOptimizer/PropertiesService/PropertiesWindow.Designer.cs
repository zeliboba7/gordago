namespace Gordago.FO {
  partial class PropertiesWindow {
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
      this._propertyGrid = new System.Windows.Forms.PropertyGrid();
      this.SuspendLayout();
      // 
      // _propertyGrid
      // 
      this._propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
      this._propertyGrid.Location = new System.Drawing.Point(0, 0);
      this._propertyGrid.Name = "_propertyGrid";
      this._propertyGrid.Size = new System.Drawing.Size(235, 287);
      this._propertyGrid.TabIndex = 0;
      this._propertyGrid.ToolbarVisible = false;
      // 
      // PropertiesWindow
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this._propertyGrid);
      this.Name = "PropertiesWindow";
      this.Size = new System.Drawing.Size(235, 287);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.PropertyGrid _propertyGrid;
  }
}
