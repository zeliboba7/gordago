using System;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;

using Gordago.Analysis.Chart;

namespace Gordago.PlugIn.Clock {

  public class ClockPlugIn:PlugInModule, IPlugIn {

    private const string CLOCKS_FIGURE_NAME = "ClockFigure";

    private IMainForm _mainForm;
    private ToolStripMenuItem _mniClock, _mniClockAdd, _mniClockRemove, _mniClockAbout;
    private ILangDictionary _langItems;

    #region public ToolStripMenuItem MenuItem
    /// <summary>
    /// Пункт в главном меню, раздел "Сервис"
    /// Если null, то отсутствует.
    /// </summary>
    public ToolStripMenuItem MenuItem {
      get { return _mniClock; ; }
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

    private CountryTime[] _country;

    public override bool OnLoad(IMainForm mainForm) {
      _mainForm = mainForm;

      mainForm.ChartActivate += new EventHandler(this._mainForm_ChartActivate);

      if(mainForm.Lang == "rus") {
        _langItems = new LangDictRussian();
      } else {
        _langItems = new LangDictEnglish();
      }

      _mniClock = new ToolStripMenuItem(_langItems.MenuItem);

      _mniClockAdd = new ToolStripMenuItem(_langItems.SubMenuItemAdd);
      _mniClockAdd.Click += new EventHandler(_mniClockAdd_Click);

      _mniClockRemove = new ToolStripMenuItem(_langItems.SubMenuItemRemove);
      _mniClockRemove.Click += new EventHandler(_mniClockRemove_Click);

      _mniClockAbout = new ToolStripMenuItem(_langItems.SubMenuItemAbout);
      _mniClockAbout.Click += new EventHandler(_mniClockAbout_Click);

      _mniClock.DropDownItems.Add(_mniClockAdd);
      _mniClock.DropDownItems.Add(_mniClockRemove);
      _mniClock.DropDownItems.Add(_mniClockAbout);
      

      int currentDelta = Convert.ToInt32((DateTime.Now.Ticks - DateTime.UtcNow.Ticks) / 10000000L / 60);

      _country = new CountryTime[]{
        new CountryTime("Current", currentDelta),
        new CountryTime("GMT+0", 0),
        new CountryTime("New York", -4*60),
        new CountryTime("Tokyo", +9*60)
      };

      return true;
    }

    #region private void _mainForm_ChartActivate(object sender, EventArgs e)
    private void _mainForm_ChartActivate(object sender, EventArgs e) {
      this.RefreshStatus();
    }
    #endregion

    #region private void _mniClockAdd_Click(object sender, EventArgs e)
    private void _mniClockAdd_Click(object sender, EventArgs e) {

      int x = 10, y = 20;

      for(int i = 0; i < _country.Length; i++) {
        string name = CLOCKS_FIGURE_NAME + "_" + _country[i].Name;

        ClockFigure figure = new ClockFigure(name, _country[i]);

        int x1 = x + i * (figure.ClockWidth + 10);
        figure.SetLocation(x1, y);

        _mainForm.ActiveChart.ChartBoxes[0].Figures.Add(figure);
      }
      this.RefreshStatus();
    }
    #endregion

    #region private void _mniClockRemove_Click(object sender, EventArgs e)
    private void _mniClockRemove_Click(object sender, EventArgs e) {
      ChartFigureList figures = _mainForm.ActiveChart.ChartBoxes[0].Figures;
      for(int i = 0; i < figures.Count; i++) {
        if(figures[i] is ClockFigure) {
          figures.Remove(figures[i]);
          i--;
        }
      }

      this.RefreshStatus();
    }
    #endregion

    #region private void _mniClockAbout_Click(object sender, EventArgs e)
    private void _mniClockAbout_Click(object sender, EventArgs e) {
      string link = "";
      if(_mainForm.Lang == "rus") {
        link = "http://forum.gordago.ru/showthread.php?t=5";
      } else {
        link = "http://forum.gordago.com/showthread.php?t=5";
      }

      try {
        ProcessStartInfo psi = new ProcessStartInfo("iexplore", link);
        psi.WorkingDirectory = "C:\\";
        Process.Start(psi);
      } catch { }
    }
    #endregion

    #region private void RefreshStatus()
    private void RefreshStatus() {
      bool flag = true;
      if(this._mainForm.ActiveChart == null) {
        flag = false;
      } else {
        ChartFigureList figures = _mainForm.ActiveChart.ChartBoxes[0].Figures;
        for(int i = 0; i < figures.Count; i++) {
          if(figures[i] is ClockFigure) {
            flag = false;
            break;
          }
        }
      }
      _mniClockAdd.Enabled = flag;
      _mniClockRemove.Enabled = !flag;
    }
    #endregion
  }
}
