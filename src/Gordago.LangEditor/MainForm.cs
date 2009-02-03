/**
* @version $Id: MainForm.cs 4 2009-02-03 13:20:59Z AKuzmin $
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
  using Gordago.LangEditor.AppList;

  partial class MainForm : Form {

    private readonly AppListManager _appListManager;

    public MainForm() {
      Global.MainForm = this;
      _appListManager = new AppListManager();

      Global.DockManager = new Gordago.LangEditor.Docking.LEDockManager();
      Global.DockManager.Dock = DockStyle.Fill;
      this.Controls.Add(Global.DockManager);

      InitializeComponent();
    }

    #region public AppListManager AppList
    public AppListManager AppList {
      get { return _appListManager; }
    }
    #endregion

    #region protected override void OnLoad(EventArgs e)
    protected override void OnLoad(EventArgs e) {

      Global.DockManager.Load();

      Global.DockManager.ViewMenuItem = _mniView;
      Global.DockManager.FileMenuItem = _mniFile;
      Global.DockManager.HelpMenuItem = _mniHelp;
      base.OnLoad(e);
    }
    #endregion

    #region protected override void OnClosing(CancelEventArgs e)
    protected override void OnClosing(CancelEventArgs e) {
      Global.DockManager.Save();
      base.OnClosing(e);
    }
    #endregion
  }
}