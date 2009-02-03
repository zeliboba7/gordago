/**
* @version $Id: LanguageMenuManager.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Core
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Windows.Forms;

  public class LanguageMenuManager {

    #region class LIToolStripMenuItem: ToolStripMenuItem
    class LIToolStripMenuItem: ToolStripMenuItem {
      private readonly LanguageInfo _li;

      public LIToolStripMenuItem(LanguageInfo li):base(li.DisplayName) {
        _li = li;
      }

      public LanguageInfo LanguageInfo {
        get { return _li; }
      }
    }
    #endregion

    public event EventHandler LanguageChanged;
    
    private readonly ToolStripMenuItem _mniLanguages;
    private string _selectedLanguage;

    public LanguageMenuManager(LanguageManager languageManager, ToolStripMenuItem languagesMenuItem) {
      _mniLanguages = languagesMenuItem;
      _mniLanguages.Text = "Languages";
      _selectedLanguage = languageManager.Code;

      LanguageInfo[] lis = languageManager.ReLoad();
      foreach (LanguageInfo li in lis) {
        LIToolStripMenuItem mni = new LIToolStripMenuItem(li);
        mni.Click += new EventHandler(mni_Click);
        _mniLanguages.DropDownItems.Add(mni);
      }
      this.UpdateChecked();
    }

    #region public string SelectedLanguage
    public string SelectedLanguage {
      get { return _selectedLanguage; }
    }
    #endregion

    #region protected virtual void OnLanguageChanged(EventArgs e)
    protected virtual void OnLanguageChanged(EventArgs e){
      if (this.LanguageChanged != null) {
        this.LanguageChanged(this, e);
      }
    }
    #endregion

    #region private void mni_Click(object sender, EventArgs e)
    private void mni_Click(object sender, EventArgs e) {
      LIToolStripMenuItem mni = sender as LIToolStripMenuItem;
      if (mni == null)
        return;
      _selectedLanguage = mni.LanguageInfo.Code;
      this.UpdateChecked();
      this.OnLanguageChanged(EventArgs.Empty);
    }
    #endregion

    #region private void UpdateChecked()
    private void UpdateChecked() {
      foreach (ToolStripMenuItem item in _mniLanguages.DropDownItems) {
        LIToolStripMenuItem mni = item as LIToolStripMenuItem;
        if (mni == null)
          continue;
        mni.Checked = mni.LanguageInfo.Code.Equals(_selectedLanguage);
      }
    }
    #endregion

    #region public void SelectLanguage(string code)
    public void SelectLanguage(string code) {
      _selectedLanguage = code;
      this.UpdateChecked();
    }
    #endregion
  }
}
