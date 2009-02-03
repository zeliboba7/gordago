namespace Gordago.FO.Instruments {
  partial class CodeEditorDocument {
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

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CodeEditorDocument));
      this._textEditor = new ICSharpCode.TextEditor.TextEditorControl();
      this.imageList1 = new System.Windows.Forms.ImageList(this.components);
      this.SuspendLayout();
      // 
      // _textEditor
      // 
      this._textEditor.Dock = System.Windows.Forms.DockStyle.Fill;
      this._textEditor.Location = new System.Drawing.Point(0, 0);
      this._textEditor.Name = "_textEditor";
      this._textEditor.ShowEOLMarkers = true;
      this._textEditor.ShowInvalidLines = false;
      this._textEditor.ShowSpaces = true;
      this._textEditor.ShowTabs = true;
      this._textEditor.ShowVRuler = true;
      this._textEditor.Size = new System.Drawing.Size(292, 266);
      this._textEditor.TabIndent = 2;
      this._textEditor.TabIndex = 0;
      // 
      // imageList1
      // 
      this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
      this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
      this.imageList1.Images.SetKeyName(0, "Icons.16x16.Class.png");
      this.imageList1.Images.SetKeyName(1, "Icons.16x16.Method.png");
      this.imageList1.Images.SetKeyName(2, "Icons.16x16.Property.png");
      this.imageList1.Images.SetKeyName(3, "Icons.16x16.Field.png");
      this.imageList1.Images.SetKeyName(4, "Icons.16x16.Enum.png");
      this.imageList1.Images.SetKeyName(5, "Icons.16x16.NameSpace.png");
      this.imageList1.Images.SetKeyName(6, "Icons.16x16.Event.png");
      // 
      // CodeEditorDocument
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(292, 266);
      this.Controls.Add(this._textEditor);
      this.Name = "CodeEditorDocument";
      this.TabText = "CodeEditorDocument";
      this.Text = "CodeEditorDocument";
      this.ResumeLayout(false);

    }

    #endregion

    private ICSharpCode.TextEditor.TextEditorControl _textEditor;
    private System.Windows.Forms.ImageList imageList1;
  }
}