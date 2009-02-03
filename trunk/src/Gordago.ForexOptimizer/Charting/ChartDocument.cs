/**
* @version $Id: ChartDocument.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.FO.Charting
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Windows.Forms;
  using Gordago.Docking;
  using Gordago.Trader;
  using Gordago.Trader.Charting;
  using Gordago.Trader.Charting.Figures;
  using System.IO;
  using Gordago.Core;
  using Gordago.FO.Instruments;
  using System.Drawing;

  public partial class ChartDocument : TabbedDocument, ITabbedDocument {

    private ISymbol _symbol;
    private string _guid;

    public ChartDocument(ISymbol symbol) {
      InitializeComponent();

      _guid = Guid.NewGuid().ToString();
      _symbol = symbol;
      this.SetKey(new ChartDocumentKey());
      this.InitializeChart();
    }

    public ChartDocument() {
      InitializeComponent();
      this.SetKey(new ChartDocumentKey());
    }

    #region public string GUID
    public string GUID {
      get { return _guid; }
    }
    #endregion

    #region public ChartControl ChartControl
    public ChartControl ChartControl {
      get { return _chartControl; }
    }
    #endregion

    #region private void InitializeChart()
    private void InitializeChart() {
      ChartBox cbox = new ChartBox();
      cbox.Figures.Add(new FigureBars());
      this._chartControl.ChartPanels.Add(cbox);
      _chartControl.Symbol = _symbol;
      this.UpdateText();
    }
    #endregion

    #region private void UpdateText()
    private void UpdateText() {
      if (_symbol == null) {
        this.Close();
      }
      this.Text = this.TabText = this._symbol.Name + ", " + _chartControl.TimeFrame.Name;
    }
    #endregion

    #region public void LoadProperties(FileInfo file)
    public void LoadProperties(FileInfo file) {
      EasyProperties ps = new EasyProperties();
      ps.Load(file);
      _guid = ps.GetValue<string>("GUID", Guid.NewGuid().ToString());
      _symbol = Global.Quotes[ps.GetValue<string>("Symbol", "")];
      if (_symbol == null)
        throw new Exception();

      this.InitializeChart();
    }
    #endregion

    #region public void SaveProperties(FileInfo file)
    public void SaveProperties(FileInfo file) {
      EasyProperties ps = new EasyProperties();
      ps.SetValue<string>("GUID", _guid);
      ps.SetValue<string>("Symbol", _symbol.Name);
      ps.Save(file);
    }
    #endregion

    #region private void _chartControl_DragOver(object sender, DragEventArgs e)
    private void _chartControl_DragOver(object sender, DragEventArgs e) {
      if (e.Data.GetDataPresent(typeof(IndicatorItem))) {
        e.Effect = DragDropEffects.Move;
      } else
        e.Effect = DragDropEffects.None;
    }
    #endregion

    private void _chartControl_DragDrop(object sender, DragEventArgs e) {
      if (!e.Data.GetDataPresent(typeof(IndicatorItem)))
        return;
      ChartBox cbox = this._chartControl.GetChartPanel(new Point(e.X, e.Y)) as ChartBox;
      if (cbox == null)
        return;
      IndicatorItem indicatorItem = e.Data.GetData(typeof(IndicatorItem)) as IndicatorItem;

      FigureIndicator figure = new FigureIndicator(indicatorItem.IndicatorType);
      cbox.Figures.Add(figure);
      Global.MainForm.Properties.SetPropertiesObject(figure);
    }
  }
}