/**
* @version $Id: AddLanguageForm.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LangEditor.AppList
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Data;
  using System.Drawing;
  using System.Text;
  using System.Windows.Forms;
  using System.Globalization;
  using Gordago.Core;

  partial class AddLanguageForm : Form {

    private readonly AppItem _appItem;
    
    public AddLanguageForm(AppItem appItem) {
      InitializeComponent();
      _appItem = appItem;
    }

    #region public LanguageInfo SelectedLanguageInfo
    public LanguageInfo SelectedLanguageInfo {
      get { return _cmbLanguages.SelectedItem as LanguageInfo; }
    }
    #endregion

    #region private static int CompareDinosByLanguageInfo(LanguageInfo li1, LanguageInfo li2)
    private static int CompareDinosByLanguageInfo(LanguageInfo li1, LanguageInfo li2) {
      return li1.ToString().CompareTo(li2.ToString());
    }
    #endregion

    #region protected override void OnLoad(EventArgs e)
    protected override void OnLoad(EventArgs e) {
      base.OnLoad(e);

      CultureInfo[] cis = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
      List<LanguageInfo> list = new List<LanguageInfo>();
      LanguageInfo[] langs = _appItem.Language.ReLoad();

      foreach (CultureInfo ci in cis) {

        LanguageInfo addedLI = new LanguageInfo(ci.Name, ci.DisplayName, ci.EnglishName);
        foreach (LanguageInfo li in langs) {
          if (li.ToString().Equals(addedLI.ToString())) {
            addedLI = null;
            break;
          }
        }

        if (addedLI == null)
          continue;

        list.Add(addedLI);
      }
      list.Sort(CompareDinosByLanguageInfo);
      _cmbLanguages.Items.AddRange(list.ToArray());

      _lblApplication.Text = string.Format(Global.Languages["AppList"]["Application ID: {0}"], _appItem.Language.AppId);
    }
    #endregion

    #region private void _btnOK_Click(object sender, EventArgs e)
    private void _btnOK_Click(object sender, EventArgs e) {
      if (this.SelectedLanguageInfo == null)
        return;
      this.DialogResult = DialogResult.OK;
      this.Close();
    }
    #endregion

    #region private void _btnCancel_Click(object sender, EventArgs e)
    private void _btnCancel_Click(object sender, EventArgs e) {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }
    #endregion

  }
}