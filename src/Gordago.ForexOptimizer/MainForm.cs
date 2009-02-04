/**
* @version $Id: MainForm.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.FO
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Data;
  using System.Drawing;
  using System.Text;
  using System.Windows.Forms;
  using Gordago.FO.Charting;
  using Gordago.FO.Instruments;

  partial class MainForm : Form {

    private readonly TablesManager _tablesManager;
    private readonly HelpManager _helpManager;
    private readonly ChartManager _chartManager;
    private readonly PropertiesManager _properties;
    private readonly FilesManager _filesManager;
    private readonly BuildManager _buildManager;

    public MainForm() {
      Global.MainForm = this;

      Global.DockManager = new ExtDockManager();
      Global.DockManager.Dock = DockStyle.Fill;
      this.Controls.Add(Global.DockManager);

      InitializeComponent();

      _helpManager = new HelpManager();
      _tablesManager = new TablesManager();
      _chartManager = new ChartManager();
      _properties = new PropertiesManager();
      _filesManager = new FilesManager();
      _buildManager = new BuildManager();
    }

    #region public TablesManager TablesManager
    public TablesManager TablesManager {
      get { return _tablesManager; }
    }
    #endregion

    #region public ChartManager ChartManager
    public ChartManager ChartManager {
      get { return _chartManager; }
    }
    #endregion

    #region public PropertiesManager Properties
    public PropertiesManager Properties {
      get { return _properties; }
    }
    #endregion

    #region public FilesManager FilesManager
    public FilesManager FilesManager {
      get { return _filesManager; }
    }
    #endregion

    #region public BuildManager BuildManager
    public BuildManager BuildManager {
      get { return _buildManager; }
    }
    #endregion

    #region protected override void OnLoad(EventArgs e)
    protected override void OnLoad(EventArgs e) {

      _helpManager.HelpMenuItem = _mniHelp;
      _chartManager.ViewMenuItem = _mniChart;

      _filesManager.FileMenuItem = _mniFile;
      _buildManager.BuildMenuItem = _mniBuild;
      Global.DockManager.ViewMenuItem = _mniView;

      _filesManager.ToolStrip = _tsStandart;

      _filesManager.UpdateIDE();
      Global.DockManager.Load();
      Global.UpdateManager.LoadUpdateManager();
      base.OnLoad(e);
    }
    #endregion

    #region protected override void OnClosing(CancelEventArgs e)
    protected override void OnClosing(CancelEventArgs e) {
      Global.DockManager.Save();
      Global.UpdateManager.SaveSettings();

      base.OnClosing(e);
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