/**
* @version $Id: LanguageEditorDocument.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LangEditor
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Data;
  using System.Drawing;
  using System.Text;
  using System.Windows.Forms;
  using Gordago.Docking;
  using Gordago.LangEditor.AppList;
  using Gordago.Core;

  partial class LanguageEditorDocument : TabbedDocument {

    private readonly AppItem _appItem;

    #region class PhraseGroupItem : ListViewItem
    class PhraseGroupItem : ListViewItem {
      private readonly LanguageManager.PhraseGroup _group;
      public PhraseGroupItem(LanguageManager.PhraseGroup group)
        : base(group.Name) {
        _group = group;
      }

      public LanguageManager.PhraseGroup PhraseGroup {
        get { return _group; }
      }
    }
    #endregion

    private readonly DataTable _table = new DataTable(); 

    public LanguageEditorDocument(AppItem appItem) {
      _appItem = appItem;

      InitializeComponent();
      this.SetKey(new EditorDocumentKey(appItem));

      _table.Columns.Add(new DataColumn("nn", typeof(int)));
      _table.Columns.Add(new DataColumn("lang", typeof(string)));
      _table.Columns.Add(new DataColumn("group", typeof(string)));
      _table.Columns.Add(new DataColumn("phrase_name", typeof(string)));
      _table.Columns.Add(new DataColumn("phrase_value", typeof(string)));

      _dataGridView.DataSource = _table;
    }

    #region protected override void Dispose(bool disposing)
    protected override void Dispose(bool disposing) {
      this.SaveValues();

      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }
    #endregion

    #region private void _txtDisplayName_TextChanged(object sender, EventArgs e)
    private void _txtDisplayName_TextChanged(object sender, EventArgs e) {
    }
    #endregion

    #region protected override void OnLoad(EventArgs e)
    protected override void OnLoad(EventArgs e) {
      base.OnLoad(e);
      _columnLstGroup.Text = Global.Languages["LED"].Phrase("ColGroup", "Group");
      _lblDisplayName.Text = Global.Languages["LED"].Phrase("lblDisplayName", "Display Name");
      _lblEnglishName.Text = Global.Languages["LED"].Phrase("lblEnglishName", "English Name");

      _dgvColNN.HeaderText = Global.Languages["LED"].Phrase("ColNN", "NN");
      _dgvColNN.HeaderText = Global.Languages["LED"].Phrase("ColPhrase", "Phrase");

      _lblCode.Text = Global.Languages["LED"].Phrase("lblCode", "Code");
    }
    #endregion

    #region private void SaveValues()
    private void SaveValues() {
      if (_table.Rows.Count == 0)
        return;
      bool changed = false;
      LanguageManager.PhraseGroup pgroup = null;
      foreach (DataRow row in _table.Rows) {
        string code = (string)row["lang"];
        string group = (string)row["group"];
        string phraseName = (string)row["phrase_name"];
        string phraseValue = (string)row["phrase_value"];

        if (!code.Equals(_appItem.Language.Code))
          return;
        if (pgroup == null)
          pgroup = _appItem.Language[group];


        string oldPhraseValue = pgroup[phraseName];
        if (!phraseValue.Equals(oldPhraseValue))
          changed = true;

        pgroup.SetPhrase(phraseName, phraseValue);
      }
      if (changed)
        _appItem.Language.Save();
    }
    #endregion

    #region public void SetLanguageInfo(LanguageInfo languageInfo)
    public void SetLanguageInfo(LanguageInfo languageInfo) {

      this.SaveValues();
      _table.Rows.Clear();

      _appItem.Language.Select(languageInfo.Code);

      this.Text = this.TabText = String.Format(Global.Languages["LED"].Phrase("TabText", "{0} {1}"), _appItem.AppId, languageInfo.Code);
      _txtDisplayName.Text = _appItem.Language.DisplayName;
      _txtEnglishName.Text = _appItem.Language.EnglishName;
      _txtCode.Text = _appItem.Language.Code;

      _lstGroup.Items.Clear();

      LanguageManager.PhraseGroup[] groups = _appItem.Language.GetGroups();
      foreach (LanguageManager.PhraseGroup group in groups) 
        _lstGroup.Items.Add(new PhraseGroupItem(group));
      
    }
    #endregion

    #region private void _lstGroup_SelectedIndexChanged(object sender, EventArgs e)
    private void _lstGroup_SelectedIndexChanged(object sender, EventArgs e) {
      this.UpdateTable();
    }
    #endregion

    #region private void UpdateTable()
    private void UpdateTable() {
      if (this._lstGroup.SelectedItems.Count != 1)
        return;
      this.SaveValues();

      LanguageManager.PhraseGroup group = (this._lstGroup.SelectedItems[0] as PhraseGroupItem).PhraseGroup;
      _table.Rows.Clear();

      int nn = 1;
      foreach (LanguageManager.Phrase phrase in group.GetPhrases()) {
        DataRow row = _table.NewRow();
        row["nn"] = nn++;
        row["lang"] = _appItem.Language.Code;
        row["group"] = group.Name;
        row["phrase_name"] = phrase.Name;
        row["phrase_value"] = phrase.Value;
        _table.Rows.Add(row);
      }
    }
    #endregion
  }
}