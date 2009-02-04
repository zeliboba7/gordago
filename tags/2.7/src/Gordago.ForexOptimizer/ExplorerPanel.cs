/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;

using Gordago.Analysis;
using Gordago.Strategy.FIndicator;
using Gordago.Strategy.FIndicator.FIndicParam;
using Gordago.Analysis.Chart;
using Cursit.Applic.APropGrid;
using Gordago.LstObject;
using Gordago.Strategy;
using Language;
#endregion

namespace Gordago {
  public partial class ExplorerPanel : UserControl {

    private IndicatorInsertBoxType _iiBoxType;
    private IndicatorGUI _currentIndicator;
    private Parameter[] _currentParameters;
    private ChartFigureIndicator _currentFigure;

    private ChartForm _chartForm = null;

    public ExplorerPanel() {
      InitializeComponent();

      _iiBoxType = IndicatorInsertBoxType.Empty;
      this._lblInsertIndicator.Text = Language.Dictionary.GetString(9, 3, "Вставить");
      this._lblIndicators.Text = Dictionary.GetString(9, 1, "Indicators");

      ImageList imglstTB = GordagoImages.Images.CreateImageListFromGroup("toolbox");
      string[] files = Directory.GetFiles(Application.StartupPath + "\\indicators\\images", "*.gif");
      foreach (string file in files) {
        string imagename = Cursit.Utils.FileEngine.GetFileNameFromPath(file);
        GordagoImages.Images.CreateImage(file, imagename, "Indicators");
        try {
          Image image = Image.FromFile(file);
          if (image != null)
            _imageListIndicator.Images.Add(imagename, image);
        } catch { }

      }
      int imgindx = GordagoImages.Images.GetIndexGroup("i.gif", "Indicators");

      ArrayList al = new ArrayList();
      foreach (Indicator indic in GordagoMain.IndicatorManager.Indicators) {
        bool find = false;
        for (int i = 0; i < al.Count; i++) {
          GroupIndicator gi = al[i] as GroupIndicator;
          if (gi.Name == indic.GroupName) {
            gi.Add(indic);
            find = true;
            break;
          }
        }
        if (!find) {
          al.Add(new GroupIndicator(indic.GroupName, indic));
        }
      }

      GroupIndicator[] gis = (GroupIndicator[])al.ToArray(typeof(GroupIndicator));

      GroupIndicator giempty = null;
      foreach (GroupIndicator gi in gis) {
        if (gi.Name == "") {
          giempty = gi;
        } else {
          TreeNode nnode = new TreeNode(gi.Name);
          nnode.ImageKey = "i.gif";
          nnode.SelectedImageKey = "i.gif";

          this.AddIndicatorsInNode(nnode, gi);
          this._trvIndicators.Nodes.Add(nnode);
        }
      }
      if (giempty != null) {
        foreach (Indicator indic in giempty.Indicators) {
          TreeNode tcnode = new TreeNode(indic.Name);
          tcnode.ImageKey = indic.ImageFileName;
          tcnode.SelectedImageKey = indic.ImageFileName;
          _trvIndicators.Nodes.Add(tcnode);
        }
      }
    }
    

    #region public IndicatorInsertBoxType IIBoxType
    public IndicatorInsertBoxType IIBoxType {
      get { return this._iiBoxType; }
      set {
        _iiBoxType = value;
        this.ClearIndicatorProperty();
      }
    }
    #endregion

    #region private void AddIndicatorsInNode(TreeNode node, GroupIndicator gi)
    private void AddIndicatorsInNode(TreeNode node, GroupIndicator gi) {
      foreach (Indicator indic in gi.Indicators) {
        int imgindex = GordagoImages.Images.GetIndexGroup(indic.ImageFileName, "Indicators");
        TreeNode tcnode = new TreeNode(indic.Name, imgindex, imgindex);
        node.Nodes.Add(tcnode);
      }
    }
    #endregion

    #region public void ClearIndicatorProperty()
    public void ClearIndicatorProperty() {
      this._pgIndic.Clear();
      this._lstIndicatorInsert.Items.Clear();
      _pngPGChartObject.Visible = false;
      _pgChartObject.SelectedObject = null;
    }
    #endregion

    #region public void ViewIndicatorProperty(IndicatorGUI indicator, Parameter[] parameters)
    /// <summary>
    /// Редактирование свойств индикатора, из внешнего источника
    /// </summary>
    public void ViewIndicatorProperty(IndicatorGUI indicator, Parameter[] parameters, ChartFigureIndicator figure) {
      UnSelectAllRowInViewIndicators();
      this._currentIndicator = indicator;
      _currentParameters = parameters;
      _currentFigure = figure;
      this.SetPropGridFromIndic();

    }
    #endregion

    #region public void ViewCharObjectPanel()
    public void ViewCharObjectPanel() {
      if (this._chartForm == null)
        return;
      _pngPGChartObject.Visible = true;
      this._treeViewChartObject.Nodes.Clear();

      IChartBox[] boxes = this._chartForm.ChartManager.ChartBoxes;
      for (int i = 0; i < boxes.Length; i++) {
        IChartBox cbox = boxes[i];
        TreeNode nodeBox = new TreeNode("Chart " + i.ToString());
        bool ladd = false;
        for (int k = 0; k < cbox.Figures.Count; k++) {
          ChartFigure figure = cbox.Figures[k];
          if (figure is ChartObject) {
            if (!ladd) {
              _treeViewChartObject.Nodes.Add(nodeBox);
              nodeBox.Expand();
              ladd = true;
            }
            ChartObject cobject = figure as ChartObject;
            TreeNode nodeCO = new TreeNode(cobject.TypeName + ": " + cobject.Name);
            nodeBox.Nodes.Add(nodeCO);
            nodeCO.Name = cobject.Id;
          }
        }
      }
    }
    #endregion

    #region internal void ViewChartObjectProperty(ChartObject chartObject, ChartForm chartForm)
    internal void ViewChartObjectProperty(ChartObject chartObject, ChartForm chartForm) {
      _chartForm = chartForm;
      this.ViewCharObjectPanel();
      this._splitPropChartObject.Panel1Collapsed = false;
      _pgChartObject.SelectedObject = chartObject;

      this.SelectChartObjectInNode(this._treeViewChartObject.Nodes, chartObject);
    }
    #endregion

    #region internal void ViewChartPanelProperty(ChartPanel chartPanel, ChartForm chartForm)
    internal void ViewChartPanelProperty(ChartPanel chartPanel, ChartForm chartForm) {
      if (chartPanel == null && _pgChartObject.SelectedObject is ChartPanelProperties) {
        this._pgChartObject.SelectedObject = null;
        this.ClearIndicatorProperty();
      } else if (chartPanel != null) {
        _chartForm = chartForm;
        _pngPGChartObject.Visible = true;
        this._splitPropChartObject.Panel1Collapsed = true;
        this._treeViewChartObject.Nodes.Clear();
        _pgChartObject.SelectedObject = chartPanel.Properties;
      }
    }
    #endregion

    #region private void SelectChartObjectInNode(TreeNodeCollection nodes, ChartObject cobject)
    private void SelectChartObjectInNode(TreeNodeCollection nodes, ChartObject cobject) {
      for (int i = 0; i < nodes.Count; i++) {
        TreeNode node = nodes[i];
        if (cobject.Id == node.Name) {
          node.Parent.Expand();
          node.TreeView.SelectedNode = node;
          node.TreeView.Refresh();
          return;
        } else {
          if (node.Nodes.Count > 0)
            this.SelectChartObjectInNode(node.Nodes, cobject);
        }
      }
    }
    #endregion

    #region public void UnSelectAllRowInViewIndicators()
    public void UnSelectAllRowInViewIndicators() {
      this.ClearIndicatorProperty();
    }
    #endregion

    #region private void SetPropGridFromIndic()
    private void SetPropGridFromIndic() {
      foreach (Form form in GordagoMain.MainForm.MdiChildren) {
        if (form is ChartForm) {
          ChartForm cform = form as ChartForm;
          if (cform.ChartManager.SelectedFigure != null) {
            if (this._currentIndicator.Parent != cform) {
              cform.ChartManager.SelectedFigure = null;
              cform.Invalidate();
            }
          }
        }
      }

      this.ClearIndicatorProperty();
      if (GordagoMain.MainForm.ActiveMdiChild is EditorForm)
        this._iiBoxType = IndicatorInsertBoxType.Strategy;
      else if (GordagoMain.MainForm.ActiveMdiChild is ChartForm)
        this._iiBoxType = IndicatorInsertBoxType.Chart;
      else if (GordagoMain.MainForm.ActiveMdiChild is CustomIndicatorForm)
        this._iiBoxType = IndicatorInsertBoxType.Strategy;
      else
        return;

      _pgIndic.Visible = false;

      IndicatorGUI indic = _currentIndicator;

      _pgIndic.Add(indic.Indicator.Name);

      foreach (IndicFuncParam ifp in indic.Params.Params) {
        if (this._iiBoxType != IndicatorInsertBoxType.Chart && ifp.Parameter is ParameterColor) {
        } else {
          if (ifp.Name == "__Shift" && _currentFigure != null && _iiBoxType == IndicatorInsertBoxType.Chart)
            ifp.Value = _currentFigure.Shift;
          AddPropGridRow(ifp);
        }
      }
      if (this._iiBoxType == IndicatorInsertBoxType.Chart) {
        ListObjItem item = new ListObjItem(indic.Indicator.Name, 0, indic);
        this._lstIndicatorInsert.Items.Add(item);
      } else
        foreach (IndicFunction indf in indic.IndicFunctions) {
          ListObjItem item = new ListObjItem(indf.Function.Name, 0, indf);
          this._lstIndicatorInsert.Items.Add(item);
        }
      _pgIndic.Visible = true;
    }
    #endregion

    #region private void AddPropGridRow(IndicFuncParam ifp)
    private void AddPropGridRow(IndicFuncParam ifp) {
      PropGridValue pgv = ifp.GetPropGridValue(_iiBoxType);

      pgv.ValueObject = ifp;
      _pgIndic.Add(pgv);
      pgv.ValueChanged += new EventHandler(this.PropGridValue_ValueChanged);
    }
    #endregion

    #region private void PropGridValue_ValueChanged(object sender, EventArgs e)
    /// <summary>
    /// Сохранение параметров из грида в индикатор и его функции
    /// </summary>
    private void PropGridValue_ValueChanged(object sender, EventArgs e) {
      PropGridValue pgv = sender as PropGridValue;
      if (pgv.ValueObject == null)
        return;
      IndicFuncParam ifp = pgv.ValueObject as IndicFuncParam;
      if (ifp is IndicFuncParamEnum) {
        string[] vals = (string[])pgv.Value;
        ((IndicFuncParamEnum)ifp).Text = vals[0];
        ifp.SetOptimizerValue(pgv.Value);
      } else if (ifp is IndicFuncParamVector) {
        string[] vals = (string[])pgv.Value;
        string[] valsc = new string[vals.Length];
        int i = 0;
        foreach (string val in vals) {
          valsc[i++] = ((IndicFuncParamVector)ifp).GetIndicFNameFromDes(val);
        }
        ifp.SetOptimizerValue(valsc);
        ifp.Value = valsc[0];
      } else if (ifp is IndicFuncParamNumber && this._iiBoxType == IndicatorInsertBoxType.Strategy) {
        if (pgv is PropGridValuePeriod) {
          PropGridValuePeriod pgvPeriod = pgv as PropGridValuePeriod;
          ifp.SetOptimizerValue(new int[]{Convert.ToInt32(pgvPeriod.ValueBegin),
																					 Convert.ToInt32(pgvPeriod.ValueEnd)});
          (ifp as IndicFuncParamNumber).Step = Convert.ToInt32(pgvPeriod.Step);
          ifp.Value = Convert.ToInt32(pgvPeriod.ValueBegin);
        } else
          ifp.Value = pgv.Value;

      } else {
        ifp.Value = pgv.Value;
      }
      ifp.Parameter.Value = ifp.Value;
      ifp.Refresh();


      if (GordagoMain.MainForm.ActiveMdiChild is Gordago.Analysis.Chart.ChartForm) {
        ChartForm cform = GordagoMain.MainForm.ActiveMdiChild as ChartForm;
        if (cform.ChartManager.SelectedFigure != null && cform.ChartManager.SelectedFigure is ChartFigureIndicator) {
          ChartFigureIndicator cfindic = cform.ChartManager.SelectedFigure as ChartFigureIndicator;
          if (cform.ChartManager.Analyzer.Cache.Count >= 128)
            cform.ChartManager.Analyzer.Cache.Clear();
          cfindic.Compute();
          if (ifp.Parameter.Name == "__Shift")
            cfindic.Shift = (int)ifp.Value;
          cform.Refresh();
        }
      }
    }
    #endregion

    #region private void _trvIndicators_AfterSelect(object sender, TreeViewEventArgs e)
    private void _trvIndicators_AfterSelect(object sender, TreeViewEventArgs e) {
      this.SetPropFromTreeView(e.Node);
    }
    #endregion

    #region private void _trvIndicators_Click(object sender, EventArgs e)
    private void _trvIndicators_Click(object sender, EventArgs e) {
      this.SetPropFromTreeView(this._trvIndicators.SelectedNode);
    }
    #endregion

    #region private void SetPropFromTreeView(TreeNode node)
    private void SetPropFromTreeView(TreeNode node) {
      if (node == null) return;
      Indicator selindic = null;
      foreach (Indicator indicator in GordagoMain.IndicatorManager.Indicators) {
        if (indicator.Name == node.Text) {
          selindic = indicator;
          break;
        }
      }
      if (selindic == null) {
        this.ClearIndicatorProperty();
        return;
      }
      MainForm fm = GordagoMain.MainForm;
      if (fm.ActiveMdiChild is EditorForm) {
        (fm.ActiveMdiChild as EditorForm).UnSetSelectStatusIndicBox();
      }
      IndicatorGUI indic = new IndicatorGUI(selindic, selindic.GetParameters(), this);

      this._currentIndicator = indic;
      this.SetPropGridFromIndic();
    }
    #endregion

    #region private void _pgChartObject_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
    private void _pgChartObject_PropertyValueChanged(object s, PropertyValueChangedEventArgs e) {
      if (this._pgChartObject.SelectedObject is ChartObject) {
        _chartForm.Refresh();
      }
    }
    #endregion

    #region private void _treeViewChartObject_AfterSelect(object sender, TreeViewEventArgs e)
    private void _treeViewChartObject_AfterSelect(object sender, TreeViewEventArgs e) {
      if (this._treeViewChartObject.SelectedNode == null || this._chartForm == null) return;
      string name = this._treeViewChartObject.SelectedNode.Name;

      ChartObject cobject = null;

      for (int i = 0; i < this._chartForm.ChartManager.ChartBoxes.Length; i++) {
        IChartBox cbox = this._chartForm.ChartManager.ChartBoxes[i];
        for (int k = 0; k < cbox.Figures.Count; k++) {
          ChartFigure figure = cbox.Figures[k];
          if (figure.Id == name) {
            cobject = figure as ChartObject;
            break;
          }
        }
      }
      if (cobject == null)
        return;
      _chartForm.ChartManager.SelectedFigure = cobject;
    }
  }
    #endregion

    #region public class GroupIndicator
  public class GroupIndicator {
    private string _name;
    private Indicator[] _indicators;
    public GroupIndicator(string name, Indicator indicator) {
      _name = name;
      _indicators = new Indicator[] { indicator };
    }
    #region public string Name
    public string Name {
      get { return this._name; }
    }
    #endregion

    public Indicator[] Indicators {
      get { return this._indicators; }
    }

    public void Add(Indicator indicator) {
      ArrayList al = new ArrayList(this._indicators);
      al.Add(indicator);
      _indicators = (Indicator[])al.ToArray(typeof(Indicator));
    }
  }
  #endregion

    #region public enum IndicatorInsertBoxType
  /// <summary>
  /// Тип редактирумого
  /// </summary>
  public enum IndicatorInsertBoxType {
    /// <summary>
    /// Нет возможности отображать свойства
    /// </summary>
    Empty,
    /// <summary>
    /// Отображать свойства для  редактора стратегий
    /// </summary>
    Strategy,
    /// <summary>
    /// Отображать свойства для графика
    /// </summary>
    Chart
  }
  #endregion

}
