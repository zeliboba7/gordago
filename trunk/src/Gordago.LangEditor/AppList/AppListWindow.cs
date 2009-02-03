/**
* @version $Id: AppListWindow.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LangEditor.AppList
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Drawing;
  using System.Data;
  using System.Text;
  using System.Windows.Forms;
  using Gordago.Docking;
  using Gordago.Core;

  public partial class AppListWindow : DockWindowPanel {

    #region class AppTreeNode : TreeNode
    class AppTreeNode : TreeNode {

      private readonly AppItem _appItem;

      public AppTreeNode(AppItem item)
        : base(item.Language.AppId) {
        _appItem = item;
      }

      #region public AppItem AppItem
      public AppItem AppItem {
        get { return _appItem; }
      }
      #endregion
    }
    #endregion

    #region class LanguageTreeNode:TreeNode
    class LanguageTreeNode : TreeNode {

      private readonly LanguageInfo _languageInfo;
      public LanguageTreeNode(LanguageInfo li)
        : base(li.DisplayName) {
        _languageInfo = li;
      }

      public LanguageInfo LanguageInfo {
        get { return _languageInfo; }
      }
    }
    #endregion

    public AppListWindow() {
      InitializeComponent();
    }

    #region protected override void OnLoad(EventArgs e)
    protected override void OnLoad(EventArgs e) {
      this.TabText = this.Text = Global.Languages["Menu"]["Applications"];
      this._mniAddLanguage.Text = Global.Languages["AppList"]["Add Language"];
      this._mniEditLanguage.Text = Global.Languages["AppList"]["Edit"];
      this.UpdateList();
      base.OnLoad(e);
    }
    #endregion

    #region public void UpdateList()
    public void UpdateList() {
      _treeView.Nodes.Clear();
      _treeView.BeginUpdate();

      foreach (AppItem item in Global.MainForm.AppList) {
        AppTreeNode node = new AppTreeNode(item);
        foreach (LanguageInfo li in item.Language.ReLoad()) {
          node.Nodes.Add(new LanguageTreeNode(li));
        }
        _treeView.Nodes.Add(node);
      }
      _treeView.EndUpdate();
    }
    #endregion

    #region private void _contextMenuStrip_Opening(object sender, CancelEventArgs e)
    private void _contextMenuStrip_Opening(object sender, CancelEventArgs e) {
      AppTreeNode appNode = this._treeView.SelectedNode as AppTreeNode;
      LanguageTreeNode lngNode = this._treeView.SelectedNode as LanguageTreeNode;

      if (appNode != null) {
        _mniAddLanguage.Visible = true;
        _mniEditLanguage.Visible = false;
      }else if (lngNode != null){
        _mniAddLanguage.Visible = false;
        _mniEditLanguage.Visible = true;
      } else {
        e.Cancel = true;
        return;
      }
    }
    #endregion

    #region private void _mniAddLanguage_Click(object sender, EventArgs e)
    private void _mniAddLanguage_Click(object sender, EventArgs e) {
      AppTreeNode node = this._treeView.SelectedNode as AppTreeNode;

      AddLanguageForm form = new AddLanguageForm(node.AppItem);
      if (form.ShowDialog() == DialogResult.Cancel)
        return;

      LanguageEditorDocument editor = Global.DockManager.ShowLanguageEditorDocument(node.AppItem);
      editor.SetLanguageInfo(form.SelectedLanguageInfo);
      this.UpdateList();
    }
    #endregion

    #region private void _mniEditLanguage_Click(object sender, EventArgs e)
    private void _mniEditLanguage_Click(object sender, EventArgs e) {
      this.EditLang();
    }
    #endregion

    #region private void EditLang()
    private void EditLang() {

      LanguageTreeNode lngNode = this._treeView.SelectedNode as LanguageTreeNode;
      if (lngNode == null)
        return;

      AppTreeNode appNode = lngNode.Parent as AppTreeNode;
      if (appNode == null)
        return;

      LanguageEditorDocument editor = Global.DockManager.ShowLanguageEditorDocument(appNode.AppItem);
      editor.SetLanguageInfo(lngNode.LanguageInfo);
    }
    #endregion

    #region private void _treeView_DoubleClick(object sender, EventArgs e)
    private void _treeView_DoubleClick(object sender, EventArgs e) {
      this.EditLang();
    }
    #endregion
  }
}
