namespace Gordago.API {
  partial class MainLeftPanel {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if(disposing && (components != null)) {
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
      this.components = new System.ComponentModel.Container();
      this._symbolsPanel = new Gordago.Stock.SymbolsPanel();
      this._handOperate = new Gordago.API.HandOperate();
      this._tableLayout = new System.Windows.Forms.TableLayoutPanel();
      this._tableLayout.SuspendLayout();
      this.SuspendLayout();
      // 
      // _symbolsPanel
      // 
      this._symbolsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this._symbolsPanel.Location = new System.Drawing.Point(3, 3);
      this._symbolsPanel.Name = "_symbolsPanel";
      this._symbolsPanel.Size = new System.Drawing.Size(176, 149);
      this._symbolsPanel.TabIndex = 0;
      // 
      // _handOperate
      // 
      this._handOperate.BackColor = System.Drawing.Color.White;
      this._handOperate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._handOperate.Dock = System.Windows.Forms.DockStyle.Fill;
      this._handOperate.Location = new System.Drawing.Point(3, 158);
      this._handOperate.Name = "_handOperate";
      this._handOperate.Size = new System.Drawing.Size(176, 246);
      this._handOperate.TabIndex = 0;
      // 
      // _tableLayout
      // 
      this._tableLayout.ColumnCount = 1;
      this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this._tableLayout.Controls.Add(this._symbolsPanel, 0, 0);
      this._tableLayout.Controls.Add(this._handOperate, 0, 1);
      this._tableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
      this._tableLayout.Location = new System.Drawing.Point(0, 0);
      this._tableLayout.Name = "_tableLayout";
      this._tableLayout.RowCount = 2;
      this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 252F));
      this._tableLayout.Size = new System.Drawing.Size(182, 407);
      this._tableLayout.TabIndex = 1;
      // 
      // MainLeftPanel
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.White;
      this.Controls.Add(this._tableLayout);
      this.Name = "MainLeftPanel";
      this.Size = new System.Drawing.Size(182, 407);
      this._tableLayout.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private HandOperate _handOperate;
    private Gordago.Stock.SymbolsPanel _symbolsPanel;
    private System.Windows.Forms.TableLayoutPanel _tableLayout;
  }
}
