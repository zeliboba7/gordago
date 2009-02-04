/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Drawing;
using System.Reflection;

namespace Gordago.Analysis.Chart {
  class ChartTemplates {

    private const string EXT = "gtp";

    private string[] _templates;
    private ChartForm _cform;

    private string _directory;
    private ChartObjectManager _chartObjectManager;
    private ChartPanelManager _chartPanelManager;

    public ChartTemplates(ChartForm cform) {
      _cform = cform;
      _templates = new string[0];
      _directory = Application.StartupPath + "\\templates";
      _chartObjectManager = GordagoMain.MainForm.ChartObjectManager;
      _chartPanelManager = GordagoMain.MainForm.ChartPanelManager;
    }

    #region public string[] Templates
    public string[] Templates {
      get { return this._templates;}
    }
    #endregion

    #region public void Refresh(ToolStripMenuItem mniTemplate)
    public void Refresh(ToolStripMenuItem mniTemplate) {
      this.Refresh();
      ToolStripMenuItem mniTRemoveTemplate = null;
      for(int i = 0; i < mniTemplate.DropDownItems.Count; i++) {
        ToolStripItem tsi = mniTemplate.DropDownItems[i];

        if(tsi.Name == "_mniTRemoveTemplate") {
          mniTRemoveTemplate = tsi as ToolStripMenuItem;

          mniTRemoveTemplate.Visible = _templates.Length != 0;
          mniTRemoveTemplate.DropDownItems.Clear();
        } else if(tsi.Name == "_mniTSeparator") {
          tsi.Visible = _templates.Length != 0;
        } else if(tsi.Name == "_mniTLoadTemplate") {
        } else if(tsi.Name == "_mniTSaveTemplate") {
        } else {
          mniTemplate.DropDownItems.Remove(tsi);
          i--;
        }
      }


      for(int k = 0; k < _templates.Length; k++) {
        ToolStripMenuItem mniFileRemove = new ToolStripMenuItem(_templates[k]);
        mniFileRemove.Name = _templates[k];
        mniFileRemove.Click += new EventHandler(this._mniRemoveFile_Click);
        mniTRemoveTemplate.DropDownItems.Add(mniFileRemove);

        ToolStripMenuItem mniFileChoise = new ToolStripMenuItem(_templates[k]);
        mniFileChoise.Name = _templates[k];
        mniFileChoise.Click += new EventHandler(this._mniChoiseFile_Click);
        mniTemplate.DropDownItems.Add(mniFileChoise);
      }
    }
    #endregion

    #region private void _mniRemoveFile_Click(object sender, EventArgs e)
    private void _mniRemoveFile_Click(object sender, EventArgs e) {
      ToolStripMenuItem mni = sender as ToolStripMenuItem;
      if(mni == null)
        return;
      string file = GetFileFromMenuItem(mni.Name);
      try {
        if(File.Exists(file)) 
          File.Delete(file);
      } catch { }
      this.Refresh();
    }
    #endregion

    #region private void _mniChoiseFile_Click(object sender, EventArgs e)
    private void _mniChoiseFile_Click(object sender, EventArgs e) {
      ToolStripMenuItem mni = sender as ToolStripMenuItem;
      if(mni == null)
        return;
      string file = GetFileFromMenuItem(mni.Name);
      this.Load(file);
    }
    #endregion

    #region private string GetFileFromMenuItem(string textMenu)
    private string GetFileFromMenuItem(string textMenu) {
      return _directory + "\\" + textMenu + "." + EXT;;
    }
    #endregion

    #region public void Load()
    public void Load() {
      OpenFileDialog ofd = new OpenFileDialog();
      ofd.InitialDirectory = _directory;
      ofd.Filter = "Gordago template (*.gtp)|*.gtp";
      ofd.Multiselect = false;
      if(ofd.ShowDialog() != DialogResult.OK)
        return;
      this.Load(ofd.FileName);
    }
    #endregion

    #region public void Load(string filename)
    public void Load(string filename) {
      try {
        string infile = _directory + "\\" + Cursit.Utils.FileEngine.GetFileNameFromPath(filename);
        if(infile != filename) 
          File.Copy(filename, infile);
        _cform.ClearTemplate();

        XmlDocument doc = new XmlDocument();
        if(!System.IO.File.Exists(filename)) return;
        doc.Load(filename);
        MainForm mform = GordagoMain.MainForm;

        XmlNode node = doc["Template"];
        if(node == null) return;

        foreach(XmlNode childnode in node.ChildNodes) {
          if(childnode.Name == "Chart") {
            LoadFromNode(_cform, childnode);
            break;
          }
        }
        _cform.Invalidate();
      } catch { }
    }
    #endregion

    #region public void Refresh()
    public void Refresh() {
      Cursit.Utils.FileEngine.CheckDir(_directory + "\\tmp.tmp");

      string[] files = Directory.GetFiles(_directory, "*." + EXT);

      _templates = new string[files.Length];

      for(int i = 0; i < files.Length; i++) {
        string file = Cursit.Utils.FileEngine.GetFileNameFromPath(files[i]);
        _templates[i] = file.Substring(0, file.Length - 4);
      }
    }
    #endregion

    #region public void Save()
    public void Save() {
      SaveFileDialog sfd = new SaveFileDialog();

      sfd.InitialDirectory = _directory;
      sfd.Filter = "Gordago template (*.gtp)|*.gtp";
      if(sfd.ShowDialog() != DialogResult.OK)
        return;

      string filename = sfd.FileName;
      this.Save(filename);
    }
    #endregion

    #region public void Save(string filename)
    public void Save(string filename) {
      if(File.Exists(filename)) {
        File.Delete(filename);
      }

      string xmlstr = string.Format("<Template Version=\"1.0\"></Template>");
      XmlDocument doc = new XmlDocument();
      doc.LoadXml(xmlstr);
      XmlNode node = doc.CreateElement("Chart");
      CreateNodeForSave(_cform, node);
      doc.DocumentElement.AppendChild(node);
      doc.Save(filename);
      this.Refresh();
    }
    #endregion

    #region public void LoadFromNode(ChartForm cform, XmlNode node)
    public void LoadFromNode(ChartForm cform, XmlNode node) {
      TimeFrame tf = TimeFrameManager.TimeFrames.GetTimeFrame(Convert.ToInt32(MainFormManager.LoadAttribute(node, "TF", "60")));
      if(tf == null)
        tf = TimeFrameManager.TimeFrames[0];

      cform.ChartManager.SetTimeFrame(tf);
      cform.ChartManager.SetZoom((ChartZoom)Convert.ToInt32(MainFormManager.LoadAttribute(node, "Zoom", "3")));
      cform.ChartManager.SetBarType((ChartFigureBarType)Convert.ToInt32(MainFormManager.LoadAttribute(node, "BarType", "0")));
      cform.ChartManager.SetGridVisible(Convert.ToBoolean(MainFormManager.LoadAttribute(node, "Grid", "True")));
      cform.ChartManager.SetChartShift(Convert.ToBoolean(MainFormManager.LoadAttribute(node, "ChartShift", "False")));
      cform.ChartManager.SetPeriodSeparators(Convert.ToBoolean(MainFormManager.LoadAttribute(node, "PerSeps", "False")));
      cform.ChartAutoScroll = Convert.ToBoolean(MainFormManager.LoadAttribute(node, "AutoScroll", "True"));
      
      int pos = Convert.ToInt32(MainFormManager.LoadAttribute(node, "Pos", "0"));
      cform.TmpPostion = pos;
      cform.ChartManager.SetPosition(pos);

      cform.MdiParent = GordagoMain.MainForm;
      bool percentError = false;
      bool firstBox = true;
      foreach(XmlNode childnode in node.ChildNodes) {
        if(childnode.Name == "Box") {
          int numbox = 0;
          if(!firstBox)
            numbox = cform.ChartManager.CreateChartBox();
          else
            firstBox = false;
          float percent = MainFormManager.CnvStringToFloat(MainFormManager.LoadAttribute(childnode, "Percent", "0"));

          foreach(XmlNode nodeindic in childnode.ChildNodes) {
            if(nodeindic.Name == "Indicator") {
              #region LoadIndicator
              string typename = MainFormManager.LoadAttribute(nodeindic, "Type", "");
              int shift = Convert.ToInt32(MainFormManager.LoadAttribute(nodeindic, "Shift", "0"));
              string indname = MainFormManager.LoadAttribute(nodeindic, "Name", "");
              Indicator indicator = GordagoMain.IndicatorManager.GetIndicator(typename);

              if(indicator != null) {
                Parameter[] parameters = indicator.GetParameters();
                foreach(XmlNode nodeprm in nodeindic)
                  SetParameter(nodeprm, parameters);
                Random rnd = new Random();
                string name = indname;
                if (indname == "")
                  name = "__indicator_" + indicator.Name + rnd.Next(1, 1000);

                cform.AddIndicator(numbox, name, indicator, parameters, shift);
              }
              #endregion
            } else if(nodeindic.Name == "Object") {
              #region LoadObject
              XmlNodeManager nodeManager = new XmlNodeManager(nodeindic);
              string typename = nodeManager.GetAttributeString("Type", "");
              string name = nodeManager.GetAttributeString("Name", "");

              #region совмеситмость с предыдущей версией
              switch(typename) {
                case "VerticalLine":
                  typename = typeof(Gordago.Analysis.Chart.ChartObjectVerticalLine).FullName;
                  //DateTime time = MainFormManager.LoadAttribute(nodeindic, "Time", DateTime.Now);
                  //if (numbox == 0)
                  //  cform.ChartManager.AddFigure(new ChartObjectVertLine(name, time), numbox);
                  break;
                case "HorizontalLine":
                  //float price = MainFormManager.CnvStringToFloat(MainFormManager.LoadAttribute(nodeindic, "Price", ""));
                  //cform.ChartManager.AddFigure(new ChartObjectHorsLine(name, price), numbox);
                  typename = typeof(Gordago.Analysis.Chart.ChartObjectHorizontalLine).FullName;
                  break;
                case "TrendLine":
                  typename = typeof(Gordago.Analysis.Chart.ChartObjectTrendLine).FullName;
                  break;
                case "Fibo":
                  typename = typeof(Gordago.Analysis.Chart.ChartObjectFibo).FullName;
                  break;
                case "FiboFan":
                  typename = typeof(Gordago.Analysis.Chart.ChartObjectFiboFan).FullName;
                  break;
                case "FiboTimeZone":
                  typename = typeof(Gordago.Analysis.Chart.ChartObjectFiboTimeZones).FullName;
                  break;
                case "CycleLines":
                  typename = typeof(Gordago.Analysis.Chart.ChartObjectCycleLines).FullName;
                  //DateTime time1 = MainFormManager.LoadAttribute(nodeindic, "Time1", DateTime.Now);
                  //DateTime time2 = MainFormManager.LoadAttribute(nodeindic, "Time2", DateTime.Now);
                  //float price1 = MainFormManager.CnvStringToFloat(MainFormManager.LoadAttribute(nodeindic, "Price1", ""));
                  //float price2 = MainFormManager.CnvStringToFloat(MainFormManager.LoadAttribute(nodeindic, "Price2", ""));
                  //if(typename == "TrendLine") {
                  //  cform.ChartManager.AddFigure(new ChartObjectTrendLine(name, time1, price1, time2, price2), numbox);
                  //} else if(typename == "Fibo") {
                  //  cform.ChartManager.AddFigure(new ChartObjectFiboLine(name, time1, price1, time2, price2), numbox);
                  //} else if(typename == "FiboFan") {
                  //  cform.ChartManager.AddFigure(new ChartObjectFiboFan(name, time1, price1, time2, price2), numbox);
                  //} else if(typename == "FiboTimeZone") {
                  //  cform.ChartManager.AddFigure(new ChartObjectFiboTimeZone(name, time1, price1, time2, price2), numbox);
                  //} else if(typename == "CycleLines") {
                  //  cform.ChartManager.AddFigure(new ChartObjectCycleLines(name, time1, price1, time2, price2), numbox);
                  //}

                  break;
              }
              #endregion
              ChartObject chartObject = this._chartObjectManager.Create(typename, name);
              if(chartObject != null) {
                chartObject.LoadTemplate(nodeManager);
                cform.ChartManager.ChartBoxes[numbox].Figures.Add(chartObject);
              }
              #endregion
            }
          }
          if(percent == 0) { 
            percentError = true; 
          } else { 
            cform.ChartManager.ChartBoxes[numbox].PercentHeight = percent; 
          }
        } else if (childnode.Name == "Panel") {
          XmlNodeManager nodeManager = new XmlNodeManager(childnode);
          string typename = nodeManager.GetAttributeString("Type", "");
          ChartPanel chartPanel = this._chartPanelManager.Create(typename, Guid.NewGuid().ToString("N"));
          if (chartPanel != null) {
            cform.ChartManager.ChartPanels.Add(chartPanel);
            chartPanel.LoadTemplate(nodeManager);
          }
        }
      }
      if(percentError) {
        float pc = 100F / cform.ChartManager.ChartBoxes.Length;

        for(int i = 0; i < cform.ChartManager.ChartBoxes.Length; i++) {
          cform.ChartManager.ChartBoxes[i].PercentHeight = pc;
        }
      }
    }
    #endregion

    #region private static void SetParameter(XmlNode node, Parameter[] parameters)
    private static void SetParameter(XmlNode node, Parameter[] parameters) {
      string name = MainFormManager.LoadAttribute(node, "Name", "");
      Parameter prm = null;
      foreach(Parameter fprm in parameters) {
        if(fprm.Name == name) {
          prm = fprm;
          break;
        }
      }
      if(prm == null) return;

      try {
        switch(node.Name) {
          case "Enum":
            if(!(prm is ParameterEnum)) return;
            ParameterEnum prmenum = prm as ParameterEnum;
            prmenum.Value = Convert.ToInt32(node.InnerText);
            return;
          case "Float":
            if(!(prm is ParameterFloat)) return;
            ParameterFloat prmfloat = prm as ParameterFloat;
            prmfloat.Value = Convert.ToSingle(node.InnerText);
            return;
          case "Int":
            if(!(prm is ParameterInteger)) return;
            ParameterInteger prmint = prm as ParameterInteger;
            prmint.Value = Convert.ToInt32(node.InnerText);
            return;
          case "Vector":
            if(!(prm is ParameterVector)) return;
            ParameterVector prmvector = prm as ParameterVector;
            prmvector.Value = node.InnerText;
            return;
          case "Color":
            if(!(prm is ParameterColor)) return;
            ParameterColor prmcolor = prm as ParameterColor;
            prmcolor.Value = Color.FromArgb(Convert.ToInt32(node.InnerText));
            return;
        }
      } catch {
      }
    }
    #endregion

    #region public static void CreateNodeForSave(ChartForm cform, XmlNode node)
    public static void CreateNodeForSave(ChartForm cform, XmlNode node) {
      XmlDocument doc = node.OwnerDocument;

      MainFormManager.SetValueAttrNode(node, "Symbol", cform.Symbol.Name);
      MainFormManager.SetValueAttrNode(node, "TF", cform.ChartManager.Bars.TimeFrame.Second.ToString());
      MainFormManager.SetValueAttrNode(node, "Pos", cform.ChartManager.Position.ToString());
      MainFormManager.SetValueAttrNode(node, "Zoom", ((int)cform.ChartManager.Zoom).ToString());
      MainFormManager.SetValueAttrNode(node, "BarType", ((int)cform.ChartManager.BarType).ToString());
      MainFormManager.SetValueAttrNode(node, "Grid", cform.ChartManager.GridVisible.ToString());
      MainFormManager.SetValueAttrNode(node, "PerSeps", cform.ChartManager.PeriodSeparators.ToString());
      MainFormManager.SetValueAttrNode(node, "ChartShift", cform.ChartManager.ChartShift.ToString());
      MainFormManager.SetValueAttrNode(node, "AutoScroll", cform.ChartAutoScroll.ToString());

      for(int i = 0; i < cform.ChartManager.ChartBoxes.Length; i++) {
        XmlNode nodebox = doc.CreateElement("Box");

        MainFormManager.SetValueAttrNode(nodebox, "Percent", MainFormManager.CnvFloatToString(Convert.ToSingle(Math.Round(cform.ChartManager.ChartBoxes[i].PercentHeight, 2))));

        node.AppendChild(nodebox);
        for(int index = 0; index < cform.ChartManager.ChartBoxes[i].Figures.Count; index++) {
          ChartFigure figure = cform.ChartManager.ChartBoxes[i].Figures[index];
          if(figure is ChartFigureIndicator) {
            SaveIndicator(nodebox, figure as ChartFigureIndicator);
          } else if(figure is ChartObject) {
            ChartObject cobject = figure as ChartObject;
            if(cobject.COPoints.IsCreate)
              SaveChartObject(nodebox, figure as ChartObject);
          }
        }
      }
      for (int i = 0; i < cform.ChartManager.ChartPanels.Count; i++) {
        ChartPanel chartPanel = cform.ChartManager.ChartPanels[i];
        XmlNode nodeBox = doc.CreateElement("Panel");
        node.AppendChild(nodeBox);

        XmlNodeManager nodeManager = new XmlNodeManager(nodeBox);
        chartPanel.SaveTemplate(nodeManager);
      }
    }
    #endregion

    #region private static void SaveIndicator(XmlNode nodebox, ChartFigureIndicator findic)
    private static void SaveIndicator(XmlNode nodebox, ChartFigureIndicator findic) {
      XmlNode node = nodebox.OwnerDocument.CreateElement("Indicator");
      nodebox.AppendChild(node);

      MainFormManager.SetValueAttrNode(node, "Name", findic.Name);

      Indicator indicator = findic.Indicator;
      MainFormManager.SetValueAttrNode(node, "Type", indicator.GetType().FullName);

      MainFormManager.SetValueAttrNode(node, "Shift", findic.Shift.ToString());

      for(int i = 0; i < findic.Parameters.Length; i++) {
        SaveParameter(node, findic.Parameters[i]);
      }
    }
    #endregion

    #region private static void SaveParameter(XmlNode nodeindic, Parameter parameter)
    private static void SaveParameter(XmlNode nodeindic, Parameter parameter) {
      string prmtype = "Unknow";

      if(parameter is ParameterEnum) {
        prmtype = "Enum";
      } else if(parameter is ParameterFloat) {
        prmtype = "Float";
      } else if(parameter is ParameterInteger) {
        prmtype = "Int";
      } else if(parameter is ParameterVector) {
        prmtype = "Vector";
      } else if(parameter is ParameterColor) {
        prmtype = "Color";
      }
      XmlNode node = nodeindic.OwnerDocument.CreateElement(prmtype);
      nodeindic.AppendChild(node);

      MainFormManager.SetValueAttrNode(node, "Name", parameter.Name);
      node.InnerText = parameter.ToSaveString();
    }
    #endregion

    #region private static void SaveChartObject(XmlNode nodebox, ChartObject fObject)
    private static void SaveChartObject(XmlNode nodebox, ChartObject fObject) {
      XmlNode node = nodebox.OwnerDocument.CreateElement("Object");
      nodebox.AppendChild(node);

      XmlNodeManager nodeManager = new XmlNodeManager(node);
      nodeManager.SetAttribute("Name", fObject.Name);
      nodeManager.SetAttribute("Type", fObject.GetType().FullName);
      fObject.SaveTemplate(nodeManager);
    }
    #endregion
  }
}
