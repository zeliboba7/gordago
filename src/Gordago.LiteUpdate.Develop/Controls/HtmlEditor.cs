/**
* @version $Id: HtmlEditor.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate.Develop.Controls
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Drawing;
  using System.Data;
  using System.Text;
  using System.Windows.Forms;
  using System.IO;

  public partial class HtmlEditor : UserControl {
    public event EventHandler UserTextChanged;

    private bool _initialized = false;
    private bool _textChanged = true;

    public HtmlEditor() {
      InitializeComponent();
    }

    #region public string TextHtml
    public string TextHtml {
      get { return _codeEditor.Text; }
      set {
        _codeEditor.Text = value;
        _textChanged = true;
        _initialized = true;
        this.Update();
      }
    }
    #endregion

    #region private void UpdateBrowser()
    private void UpdateBrowser() {
      if (!_textChanged)
        return;
      _webBrowser.DocumentText = this.TextHtml;
      _textChanged = false;
    }
    #endregion

    #region private void _tbcMain_SelectedIndexChanged(object sender, EventArgs e)
    private void _tbcMain_SelectedIndexChanged(object sender, EventArgs e) {
      if (_tbcMain.SelectedIndex == 1)
        this.UpdateBrowser();
    }
    #endregion

    #region private void _codeEditor_TextChanged(object sender, EventArgs e)
    private void _codeEditor_TextChanged(object sender, EventArgs e) {
      _textChanged = true;
      if (!_initialized)
        return;
      this.OnUserTextChanged(EventArgs.Empty);
    }
    #endregion

    #region protected virtual void OnUserTextChanged(EventArgs e)
    protected virtual void OnUserTextChanged(EventArgs e) {
      if (UserTextChanged != null)
        UserTextChanged(this, e);
    }
    #endregion
  }
}
