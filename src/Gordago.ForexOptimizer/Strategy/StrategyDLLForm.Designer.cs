namespace Gordago.Strategy {
  partial class StrategyDLLForm {
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
      _destroy = true;
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StrategyDLLForm));
        this._lstStrategy = new System.Windows.Forms.ListBox();
        this._propGrid = new System.Windows.Forms.PropertyGrid();
        this.splitContainer1 = new System.Windows.Forms.SplitContainer();
        this._lblStrategy = new System.Windows.Forms.Label();
        this.panel1 = new System.Windows.Forms.Panel();
        this._lblProp = new System.Windows.Forms.Label();
        this.splitContainer1.Panel1.SuspendLayout();
        this.splitContainer1.Panel2.SuspendLayout();
        this.splitContainer1.SuspendLayout();
        this.panel1.SuspendLayout();
        this.SuspendLayout();
        // 
        // _lstStrategy
        // 
        this._lstStrategy.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this._lstStrategy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this._lstStrategy.FormattingEnabled = true;
        this._lstStrategy.Location = new System.Drawing.Point(0, 22);
        this._lstStrategy.Name = "_lstStrategy";
        this._lstStrategy.Size = new System.Drawing.Size(314, 67);
        this._lstStrategy.TabIndex = 0;
        this._lstStrategy.SelectedIndexChanged += new System.EventHandler(this._lstStrategy_SelectedIndexChanged);
        // 
        // _propGrid
        // 
        this._propGrid.Dock = System.Windows.Forms.DockStyle.Fill;
        this._propGrid.HelpVisible = false;
        this._propGrid.Location = new System.Drawing.Point(0, 0);
        this._propGrid.Name = "_propGrid";
        this._propGrid.Size = new System.Drawing.Size(312, 177);
        this._propGrid.TabIndex = 1;
        this._propGrid.ToolbarVisible = false;
        // 
        // splitContainer1
        // 
        this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.splitContainer1.Location = new System.Drawing.Point(0, 0);
        this.splitContainer1.Name = "splitContainer1";
        this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
        // 
        // splitContainer1.Panel1
        // 
        this.splitContainer1.Panel1.Controls.Add(this._lblStrategy);
        this.splitContainer1.Panel1.Controls.Add(this._lstStrategy);
        // 
        // splitContainer1.Panel2
        // 
        this.splitContainer1.Panel2.Controls.Add(this.panel1);
        this.splitContainer1.Panel2.Controls.Add(this._lblProp);
        this.splitContainer1.Size = new System.Drawing.Size(314, 293);
        this.splitContainer1.SplitterDistance = 90;
        this.splitContainer1.TabIndex = 2;
        // 
        // _lblStrategy
        // 
        this._lblStrategy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this._lblStrategy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(236)))), ((int)(((byte)(242)))));
        this._lblStrategy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this._lblStrategy.Location = new System.Drawing.Point(0, 2);
        this._lblStrategy.Name = "_lblStrategy";
        this._lblStrategy.Size = new System.Drawing.Size(314, 21);
        this._lblStrategy.TabIndex = 2;
        this._lblStrategy.Text = "Strategy";
        this._lblStrategy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // panel1
        // 
        this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.panel1.Controls.Add(this._propGrid);
        this.panel1.Location = new System.Drawing.Point(0, 20);
        this.panel1.Name = "panel1";
        this.panel1.Size = new System.Drawing.Size(314, 179);
        this.panel1.TabIndex = 3;
        // 
        // _lblProp
        // 
        this._lblProp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this._lblProp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(236)))), ((int)(((byte)(242)))));
        this._lblProp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this._lblProp.Location = new System.Drawing.Point(0, 0);
        this._lblProp.Name = "_lblProp";
        this._lblProp.Size = new System.Drawing.Size(314, 21);
        this._lblProp.TabIndex = 2;
        this._lblProp.Text = "Properties";
        this._lblProp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // StrategyDLLForm
        // 
        this.BackColor = System.Drawing.Color.White;
        this.ClientSize = new System.Drawing.Size(314, 293);
        this.Controls.Add(this.splitContainer1);
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.Name = "StrategyDLLForm";
        this.ShowInTaskbar = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
        this.Text = "DLL Strategy";
        this.splitContainer1.Panel1.ResumeLayout(false);
        this.splitContainer1.Panel2.ResumeLayout(false);
        this.splitContainer1.ResumeLayout(false);
        this.panel1.ResumeLayout(false);
        this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListBox _lstStrategy;
    private System.Windows.Forms.PropertyGrid _propGrid;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.Label _lblProp;
    private System.Windows.Forms.Label _lblStrategy;
    private System.Windows.Forms.Panel panel1;
  }
}
