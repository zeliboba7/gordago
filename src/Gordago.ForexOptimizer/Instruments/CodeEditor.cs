/**
* @version $Id: CodeEditor.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.FO.Instruments
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using ICSharpCode.TextEditor;
  using System.IO;

  class CodeEditor : TextEditorControl {

    private readonly FileInfo _file;

    public CodeEditor(FileInfo file) {
      _file = file;
      this.InitializeComponent();
    }

    #region private void InitializeComponent()
    private void InitializeComponent() {
      this.SuspendLayout();
      // 
      // textAreaPanel
      // 
      this.textAreaPanel.Size = new System.Drawing.Size(416, 367);
      // 
      // CodeEditor
      // 
      this.Name = "CodeEditor";
      this.Size = new System.Drawing.Size(416, 367);
      this.ResumeLayout(false);
    }
    #endregion
  }
}
