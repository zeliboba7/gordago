using System;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

using Gordago;
using Gordago.PlugIn;
using Gordago.Analysis;
using Gordago.Analysis.Chart;

namespace MyPlugIn {

  public class PlugIn: PlugInModule, IPlugIn {

    private IMainForm _mainForm;
    private ToolStripMenuItem _mniMyPlugIn, _mniMyPlugInAdd, _mniMyPlugInRemove, _mniMyPlugInSettings;
    private ToolStrip _tsMyPlugIn;
    private ToolStripButton _mnbMyPlugInAddOnChart;

    private ILangDictionary _langItems;
    private bool _restartProgram = false;
    private LimitAnalyzerManager _analyzer;

    /// <summary>
    /// Необходимые для работы ТаймФреймы. 
    /// Значения в секундах.
    /// 3600 - 1 часовой
    /// 14400 - 4 часовой
    /// 86400 - дневной
    /// </summary>
    public readonly TimeFrame[] WORK_TIMEFRAMES = new TimeFrame[] {
      new TimeFrame("M1", 3600),
      new TimeFrame("M4", 14400), 
      new TimeFrame("D1", 86400)
    };

    #region public ToolStripMenuItem MenuItem
    /// <summary>
    /// Пункт в главном меню, раздел "Сервис"
    /// Если null, то отсутствует.
    /// </summary>
    public ToolStripMenuItem MenuItem {
      get { return _mniMyPlugIn; }
    }
    #endregion

    #region public ToolStrip Toolbar
    /// <summary>
    /// Панель инструментов. 
    /// Если null, то отсутствует.
    /// </summary>
    public ToolStrip Toolbar {
      get { return _tsMyPlugIn; }
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

      _mniMyPlugIn = new ToolStripMenuItem(_langItems.MenuItem);

      _mniMyPlugInAdd = new ToolStripMenuItem(_langItems.SubMenuItemMyPlugInAdd);
      _mniMyPlugInAdd.Click += new EventHandler(_mniMyPlugInAdd_Click);

      _mniMyPlugInRemove = new ToolStripMenuItem(_langItems.SubMenuItemMyPlugInRemove);
      _mniMyPlugInRemove.Click += new EventHandler(_mniMyPlugInRemove_Click);

      _mniMyPlugInSettings = new ToolStripMenuItem(_langItems.SubMenuItemMyPlugInSettings);
      _mniMyPlugInSettings.Click += new EventHandler(_mniMyPlugInSettings_Click);

      _mniMyPlugIn.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
        _mniMyPlugInAdd,
        _mniMyPlugInRemove,
        _mniMyPlugInSettings});

      _mnbMyPlugInAddOnChart = new ToolStripButton();
      _mnbMyPlugInAddOnChart.Name = "_mnbMyPlugInAddOnChart";
      _mnbMyPlugInAddOnChart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      _mnbMyPlugInAddOnChart.Image = global::MyPlugIn.Properties.Resources.MyPlugIn;
      _mnbMyPlugInAddOnChart.ImageTransparentColor = System.Drawing.Color.Magenta;
      _mnbMyPlugInAddOnChart.Click += new System.EventHandler(_mniMyPlugInAdd_Click);

      _tsMyPlugIn = new ToolStrip();
      _tsMyPlugIn.Items.Add(_mnbMyPlugInAddOnChart);

      _analyzer = new LimitAnalyzerManager(mainForm.IndicatorManager, mainForm.Symbols);
      
      return true;
    }

    #region public LimitAnalyzerManager Analyzer
    /// <summary>
    /// Анализатор для рассчета функций индикаторов.
    /// </summary>
    public LimitAnalyzerManager Analyzer {
      get { return this._analyzer; }
    }
    #endregion

    #region private void _mainForm_ChartActivate(object sender, EventArgs e)
    private void _mainForm_ChartActivate(object sender, EventArgs e) {
      this.RefreshStatus();
    }
    #endregion

    #region private void _mniMyPlugInSettings_Click(object sender, EventArgs e)
    private void _mniMyPlugInSettings_Click(object sender, EventArgs e) {
      MyPlugInSettingsForm form = new MyPlugInSettingsForm();
      form.ShowDialog();
    }
    #endregion

    private void _mniMyPlugInAdd_Click(object sender, EventArgs e) {

      MyPlugInErrors error = this.CheckTimeFrames();
      if(error == MyPlugInErrors.TimeFrameNotFound) {
        return;
      } else if(error == MyPlugInErrors.RestartProgram) {
        _restartProgram = true;
        MessageBox.Show(_langItems.RestartProgram, "MyPlugIn");
      } else {
        _mainForm.ActiveChart.ChartBoxes[0].Figures.Add(new MyFigure("MyFigureSample", _analyzer));
      }
      this.RefreshStatus();
    }

    private void _mniMyPlugInRemove_Click(object sender, EventArgs e) {
      _mainForm.ActiveChart.ChartBoxes[0].Figures.RemoveAt("MyFigureSample");
      this.RefreshStatus();
    }

    private void RefreshStatus() {
      bool flag = true;
      if(this._mainForm.ActiveChart == null) {
        flag = false;
      } else {
        flag = _mainForm.ActiveChart.ChartBoxes[0].Figures.GetFigure("MyFigureSample") == null;
      }
      if(_restartProgram) flag = false;

      _mniMyPlugInAdd.Enabled =
        _mnbMyPlugInAddOnChart.Enabled = flag;
      _mniMyPlugInRemove.Enabled = !flag;
    }
    
    /// <summary>
    /// Проверка на наличие необходимых для работы ТаймФреймов.
    /// Если необходимых периодов не окажеться, пользователю будет 
    /// предложено добавить их.
    /// После добавления новых периодов, необходимо перезапустить программу.
    /// </summary>
    /// <returns> </returns>
    private MyPlugInErrors CheckTimeFrames() {
      bool addnewtf = false;
      for(int i = 0; i < WORK_TIMEFRAMES.Length; i++) {
        TimeFrame tf = _mainForm.TimeFrameManager.GetTimeFrame(WORK_TIMEFRAMES[i].Second);
        if(tf == null) {
          if(!addnewtf) {
            if(MessageBox.Show(string.Format(_langItems.WarningNewTimeFrame, WORK_TIMEFRAMES[i].Name), "MyPlugIn", MessageBoxButtons.YesNo) == DialogResult.No)
              return MyPlugInErrors.TimeFrameNotFound;
            addnewtf = true;
          }
          _mainForm.TimeFrameManager.AddTimeFrame(tf);
        }
      }
      if(addnewtf)
        return MyPlugInErrors.RestartProgram;
      return MyPlugInErrors.NONE;
    }

  }
}
