using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Win32;

using Gordago.Analysis;

namespace Gordago.PlugIn.MetaQuotesHistory {
  public class MQHPlugIn: PlugInModule, IPlugIn {
    private IMainForm _mainForm;
    private ToolStripMenuItem _mniMQH, _mniMQHImport, _mniMQHConvertToTicks;

    private static ILangDictionary _langItems;
    private bool _restartProgram = false;

    private MQHSymbolManager _mqhSymbols;

    public const string KEY_REESTR_APP_PATH = @"Software\MetaQuotes Software\MetaTrader 4\Settings";
    
    private static string _mqHistoryDir;

    #region public static TimeFrame[] MQH_TIMEFRAMES
    /// <summary>
    /// MetaQuotes Timeframe.
    /// </summary>
    public static TimeFrame[] MQH_TIMEFRAMES = 
      new TimeFrame[] {
        new TimeFrame("M1", 60),
        new TimeFrame("M5", 300), 
        new TimeFrame("M15", 900),
        new TimeFrame("M30", 1800),
        new TimeFrame("H1", 3600),
        new TimeFrame("H4", 14400),
        new TimeFrame("D1", 86400),
        new TimeFrame("W1", 604800),
        new TimeFrame("MN", 2592000)
    };
    #endregion

    #region public static string MQHistoryDir
    public static string MQHistoryDir {
      get { return _mqHistoryDir; }
    }
    #endregion

    #region public ToolStripMenuItem MenuItem
    /// <summary>
    /// Пункт в главном меню, раздел "Сервис"
    /// Если null, то отсутствует.
    /// </summary>
    public ToolStripMenuItem MenuItem {
      get { return _mniMQH; }
    }
    #endregion

    #region public ToolStrip Toolbar
    /// <summary>
    /// Панель инструментов. 
    /// Если null, то отсутствует.
    /// </summary>
    public ToolStrip Toolbar {
      get { return null; }
    }
    #endregion

    #region internal static ILangDictionary LanguageDictionary
    internal static ILangDictionary LanguageDictionary {
      get { return _langItems; }
    }
    #endregion

    public override bool OnLoad(IMainForm mainForm) {
      _mainForm = mainForm;

      mainForm.ChartActivate += new EventHandler(this._mainForm_ChartActivate);

      if(mainForm.Lang == "rus") {
        _langItems = new LangDictRussian();
      } else {
        _langItems = new LangDictEnglish();
      }

      _mniMQH = new ToolStripMenuItem(_langItems.MenuItem);

      _mniMQHImport = new ToolStripMenuItem(_langItems.SubMenuItemImport);
      _mniMQHImport.Click += new EventHandler(_mniMQHImport_Click);

      _mniMQHConvertToTicks = new ToolStripMenuItem(_langItems.SubMenuItemConvertToTicks);
      _mniMQHConvertToTicks.Click += new EventHandler(this._mniMQHConvertToTicks_Click);

      _mniMQH.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
        _mniMQHImport,
      _mniMQHConvertToTicks}
      );

      _mqHistoryDir = Application.StartupPath + "\\plugin\\mqhistory";

      this.LoadMQHistory();

      return true;
    }

    #region private void LoadMQHistory()
    private void LoadMQHistory() {
      _mqhSymbols = new MQHSymbolManager(_mainForm.Symbols, _mqHistoryDir);
    }
    #endregion

    #region private void _mainForm_ChartActivate(object sender, EventArgs e)
    private void _mainForm_ChartActivate(object sender, EventArgs e) {
      this.RefreshStatus();
    }
    #endregion

    private void _mniMQHImport_Click(object sender, EventArgs e) {
      string path = GetMetaTrader4Path();
      if(System.IO.Directory.Exists(path)) {
        path += "\\history";
      }

      FolderBrowserDialog fbd = new FolderBrowserDialog();
      fbd.SelectedPath = path;
      if(fbd.ShowDialog() != DialogResult.OK)
        return;
      string[] files = Directory.GetFiles(fbd.SelectedPath, "*.hst");
      if(files.Length == 0) {
        MessageBox.Show(_langItems.WarningImportFileNotFound, "MetaQuotesHistory");
        return;
      }
      _mainForm.Cursor = Cursors.WaitCursor;
      foreach(string file in files) {
        string[] sa = file.Split(new char[] { '\\' });
        string filename = sa[sa.Length - 1];

        string destFile = _mqHistoryDir + "\\" + filename;

        try {
          if(File.Exists(destFile)) {
            File.Delete(destFile);
          }
          File.Copy(file, destFile);
        } catch {}
      }
      LoadMQHistory();
      _mainForm.Cursor = Cursors.Default;
    }

    #region private void _mniMQHAdd_Click(object sender, EventArgs e)
    private void _mniMQHAdd_Click(object sender, EventArgs e) {

      if(_restartProgram) {
        MessageBox.Show(_langItems.RestartProgram, "MetaQuotesHistory");
        return;
      }

      MQHErrors error = this.CheckTimeFrames();
      if(error == MQHErrors.RestartProgram) {
        _restartProgram = true;
        MessageBox.Show(_langItems.RestartProgram, "MetaQuotesHistory");
      } else {

        MQHSymbol mqhSymbol = _mqhSymbols.GetSymbol(_mainForm.ActiveChart.Symbol.Name);
        _mainForm.ActiveChart.SetSymbol(mqhSymbol);
      }
      this.RefreshStatus();
    }
    #endregion

    private void _mniMQHRemove_Click(object sender, EventArgs e) {
      ISymbol symbol = _mainForm.Symbols.GetSymbol(_mainForm.ActiveChart.Symbol.Name);
      _mainForm.ActiveChart.SetSymbol(symbol);
      this.RefreshStatus();
    }

    private void _mniMQHConvertToTicks_Click(object sender, EventArgs e) {
      ConvertToTicksForm form = new ConvertToTicksForm();
      form.ShowDialog();
    }

    #region private void RefreshStatus()
    private void RefreshStatus() {
      if(_restartProgram) {
        _mniMQH.Enabled = false;
      } 
    }
    #endregion

    #region private MQHErrors CheckTimeFrames()
    /// <summary>
    /// Проверка на наличие необходимых для работы ТаймФреймов.
    /// Если необходимых периодов не окажеться, пользователю будет 
    /// предложено добавить их.
    /// После добавления новых периодов, необходимо перезапустить программу.
    /// </summary>
    /// <returns> </returns>
    private MQHErrors CheckTimeFrames() {
      bool addnewtf = false;
      for(int i = 0; i < MQH_TIMEFRAMES.Length; i++) {
        TimeFrame tf = _mainForm.TimeFrameManager.GetTimeFrame(MQH_TIMEFRAMES[i].Second);
        if(tf == null) {
          if(!addnewtf) {
            if(MessageBox.Show(string.Format(_langItems.WarningNewTimeFrame, MQH_TIMEFRAMES[i].Name), "MyPlagIn", MessageBoxButtons.YesNo) == DialogResult.No)
              return MQHErrors.TimeFrameNotFound;
            addnewtf = true;
          }
          _mainForm.TimeFrameManager.AddTimeFrame(MQH_TIMEFRAMES[i]);
        }
      }
      if(addnewtf)
        return MQHErrors.RestartProgram;
      return MQHErrors.NONE;
    }
    #endregion

    #region public static string GetMetaTrader4Path()
    public static string GetMetaTrader4Path() {
      string app_path = "";
      RegistryKey regKey = null;
      try {
        regKey = Registry.CurrentUser.OpenSubKey(KEY_REESTR_APP_PATH);
        if(regKey != null) {
          String[] valueNames = regKey.GetValueNames();
          foreach(String valueName in valueNames) {
            if(valueName == "ProgramPath") {
              object obj = regKey.GetValue(valueName);
              string strkey = obj as string;
              string[] sa = strkey.Split(new char[] { '\\' });
              if(sa.Length >= 2) {
                string[] sat = new string[sa.Length - 1];
                for(int i = 0; i < sa.Length - 1; i++) {
                  sat[i] = sa[i];
                }
                app_path = String.Join("\\", sat);
              }
            }
          }
        }
        if(app_path.Length <= 2 || !System.IO.Directory.Exists(app_path))
          app_path = "";

      } finally {
        if(regKey != null) {
          regKey.Close();
        }
      }
      return app_path;
    }
    #endregion

  }
}
