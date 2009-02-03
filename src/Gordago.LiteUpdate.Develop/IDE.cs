/**
* @version $Id: IDE.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate.Develop
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Data;
  using System.Drawing;
  using System.Text;
  using System.Windows.Forms;
  using System.IO;
  using WeifenLuo.WinFormsUI.Docking;
  using Gordago.LiteUpdate.Develop.Updates;

  public partial class IDE : Form{

    private readonly HelpManager _helpManager ;

    public IDE():base() {
      Global.MainForm = this;

      _helpManager = new HelpManager();

      Global.DockManager = new Gordago.LiteUpdate.Develop.Docking.LUDockManager();
      Global.DockManager.Dock = DockStyle.Fill;
      this.Controls.Add(Global.DockManager);

      InitializeComponent();
      Global.DockManager.ViewMenuItem = _mniView;

      _projectManager.ToolStrip = _tsStandart;
      _helpManager.HelpMenuItem = _mniHelp;
      Global.DockManager.ToolStrip = _tsStandart;

      Global.Builder.StartBuild += new Gordago.LiteUpdate.Develop.Projects.BuilderEventHandler(Builder_StartBuild);
      Global.Builder.StopBuild += new Gordago.LiteUpdate.Develop.Projects.BuilderEventHandler(Builder_StopBuild);
      Global.Builder.Progress += new Gordago.LiteUpdate.Develop.Projects.BuilderProgressEventHandler(Builder_Progress);
      this.Icon = global::Gordago.LiteUpdate.Develop.Properties.Resources.app;
    }

    #region public Gordago.LiteUpdate.Develop.Projects.ProjectManager ProjectManager
    public Gordago.LiteUpdate.Develop.Projects.ProjectManager ProjectManager {
      get { return _projectManager; }
    }
    #endregion

    #region protected override void OnLoad(EventArgs e)
    protected override void OnLoad(EventArgs e) {
      base.OnLoad(e);
      this._projectManager.Load();
      Global.DockManager.Load();
      Global.DockManager.ShowStartPageDocument();
      Global.UpdateManager.LoadUpdateManager();
    }
    #endregion

    #region protected override void OnClosing(CancelEventArgs e)
    protected override void OnClosing(CancelEventArgs e) {
      if (_projectManager.Project != null && !_projectManager.Project.IsSaved) {
        switch (MessageBox.Show("Save changes to the project?", "Gordago LiteUpdate Develop", MessageBoxButtons.YesNoCancel)) {
          case DialogResult.Cancel:
            e.Cancel = true;
            return;
          case DialogResult.Yes:
            _projectManager.SaveProject();
            break;
        }
      }
      _projectManager.CloseProject();

      Global.DockManager.Save();
      Global.UpdateManager.SaveSettings();
      base.OnClosing(e);
    }
    #endregion

    #region private void Builder_StartBuild(object sender, Gordago.LiteUpdate.Develop.Projects.BuilderEventArgs e)
    private void Builder_StartBuild(object sender, Gordago.LiteUpdate.Develop.Projects.BuilderEventArgs e) {
      if (this.InvokeRequired) {
        this.Invoke(new Gordago.LiteUpdate.Develop.Projects.BuilderEventHandler(Builder_StartBuild), sender, e);
      } else {
        OutputWindow outputWindow = Global.DockManager.ShowOutputWindow();
        outputWindow.TextBox.Clear();
        _statusLabel.Text = string.Format("Build {0}", e.Project.Name);
        _statusProgressBar.Visible = true;
      }
    }
    #endregion

    #region private void Builder_StopBuild(object sender, Gordago.LiteUpdate.Develop.Projects.BuilderEventArgs e)
    private void Builder_StopBuild(object sender, Gordago.LiteUpdate.Develop.Projects.BuilderEventArgs e) {
      if (this.InvokeRequired) {
        this.Invoke(new Gordago.LiteUpdate.Develop.Projects.BuilderEventHandler(Builder_StopBuild), sender, e);
      } else {
        _statusLabel.Text = "Ready";
        _statusProgressBar.Visible = false;
      }
    }
    #endregion

    #region private void Builder_Progress(object sender, Gordago.LiteUpdate.Develop.Projects.BuilderProgressEventArgs e)
    private void Builder_Progress(object sender, Gordago.LiteUpdate.Develop.Projects.BuilderProgressEventArgs e) {
      if (this.InvokeRequired) {
        this.Invoke(new Gordago.LiteUpdate.Develop.Projects.BuilderProgressEventHandler(Builder_Progress), sender, e);
      } else {
        if (e.Total == 0)
          return;

        _statusProgressBar.Maximum = (int)e.Total;
        _statusProgressBar.Value = (int)e.Current;
      }
    }
    #endregion

    #region public static IAppAction GetAppMenuItem(ToolStripMenuItem menu, AppAction action)
    public static IAppAction GetAppMenuItem(ToolStripMenuItem menu, AppAction action) {
      foreach (ToolStripMenuItem item in menu.DropDownItems) {
        IAppAction appItem = item as IAppAction;
        if (appItem != null && appItem.Action == action)
          return appItem;
      }
      return null;
    }
    #endregion

    #region public static IAppAction GetAppToolStripButtom(ToolStrip toolStrip, AppAction action)
    public static IAppAction GetAppToolStripButtom(ToolStrip toolStrip, AppAction action) {
      foreach (ToolStripItem item in toolStrip.Items) {
        AppToolStripButton buttom = item as AppToolStripButton;
        if (buttom != null && buttom.Action == action) 
          return buttom;
      }
      return null;
    }
    #endregion
  }
}