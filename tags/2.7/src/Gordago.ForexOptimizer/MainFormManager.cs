/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using
using System;
using System.Xml;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;

using Gordago.Analysis.Chart;
using Gordago.Strategy;
using System.Collections;

using Gordago.Analysis;
using Gordago.Windows.Forms;
using Cursit.Table;
#endregion

namespace Gordago {
  internal class MainFormManager {

    #region class TableProp 
    class TableProp {
      private string _name;
      private string _widths;

      public TableProp(TableControl table) {
        _name = table.Name;
        _widths = table.Columns.GetConfigWidths();
      }

      public TableProp(string name, string widths) {
        _name = name;
        _widths = widths;
      }

      #region public string Name
      public string Name {
        get { return _name; }
      }
      #endregion

      #region public string Widths
      public string Widths {
        get { return this._widths; }
      }
      #endregion
    }
    #endregion

    private const string MFILENAME = "gso.xml";
    private static long UNIXTICKS = new DateTime(1970, 1, 1, 0, 0, 0, 0).Ticks;

    #region public static void Save()
    public static void Save(DockManager dockManager) {
      MainForm mform = GordagoMain.MainForm;

      XmlDocument doc = dockManager.SaveToXml();

      foreach(Form form in mform.MdiChildren){
        if(form is ChartForm) {
          SaveChartForm(form as ChartForm, doc);
        } else if (form is EditorForm) {
          SaveStrategyForm(form as EditorForm, doc);
        } else if(form is StrategyDLLForm) {
          SaveDLLStrategyForm(form as StrategyDLLForm, doc);
        }
      }
      doc.Save(Application.StartupPath + "\\" + MFILENAME);
    }
    #endregion

    #region private static XmlNode CreateFormNode (string name, Form form, XmlDocument doc)
    private static XmlNode CreateFormNode (string name, Form form, XmlDocument doc) {
      XmlNode node = doc.CreateElement(name);

      string state = "";
      switch(form.WindowState) {
        case FormWindowState.Maximized:
          state = "max";
          break;
        case FormWindowState.Minimized:
          state = "min";
          break;
        case FormWindowState.Normal:
          state = "normal";
          break;
      }

      SetValueAttrNode(node, "State", state);
      SetValueAttrNode(node, "Left", form.Location.X.ToString());
      SetValueAttrNode(node, "Top", form.Location.Y.ToString());
      SetValueAttrNode(node, "Width", form.Size.Width.ToString());
      SetValueAttrNode(node, "Height", form.Size.Height.ToString());

      doc.DocumentElement.AppendChild(node);
      return node;
    }
    #endregion

    #region private static void SaveStrategyForm(EditorForm wform, XmlDocument doc)
    private static void SaveStrategyForm(EditorForm wform, XmlDocument doc) {
      XmlNode node = CreateFormNode("Strategy", wform, doc);
      SetValueAttrNode(node, "File", wform.FileName);
    }
    #endregion

    #region private static void SaveChartForm(ChartForm cform, XmlDocument doc)
    private static void SaveChartForm(ChartForm cform, XmlDocument doc) {
      XmlNode node = CreateFormNode("Chart", cform, doc);
      ChartTemplates.CreateNodeForSave(cform, node);
    }
    #endregion

    #region private static void SaveDLLStrategyForm(StrategyDLLForm dform, XmlDocument doc)
    private static void SaveDLLStrategyForm(StrategyDLLForm dform, XmlDocument doc) {
      XmlNode node = CreateFormNode("StrategyDLL", dform, doc);
      SetValueAttrNode(node, "File", dform.FileName);
    }
    #endregion

    #region public static void SetValueAttrNode(XmlNode node, string name, string value)
    public static void SetValueAttrNode(XmlNode node, string name, string value) {
      node.Attributes.Append(node.OwnerDocument.CreateAttribute(name));
      node.Attributes[name].Value = value;
    }
    #endregion

    #region public static void SetValueAttrNode(XmlNode node, string name, DateTime time)
    public static void SetValueAttrNode(XmlNode node, string name, DateTime time) {
      uint utime = (UInt32)((time.Ticks - UNIXTICKS) / 10000000L);
      SetValueAttrNode(node, name, Convert.ToString(utime));
    }
    #endregion

    #region public static void Load()
    public static void Load(DockManager dockManager) {
      XmlDocument doc = new XmlDocument();
      string filename = Application.StartupPath + "\\" + MFILENAME;
      if (!System.IO.File.Exists(filename))
        return;
      doc.Load(filename);
      MainForm mform = GordagoMain.MainForm;

      XmlNode node = doc["Gordago"];
      if (node == null)
        return;

      dockManager.Load(doc);

      foreach (XmlNode childnode in node.ChildNodes) {
        if (childnode.Name == "Strategy") {
          LoadStrategy(childnode);
        } else if (childnode.Name == "Chart") {
          LoadChart(childnode);
        } else if (childnode.Name == "StrategyDLL") {
          LoadStrategyDLL(childnode);
        }
      }
    }
    #endregion

    #region private static void LoadStrategy(XmlNode node)
    private static void LoadStrategy(XmlNode node) {
      MainForm mform = GordagoMain.MainForm;
      string file = LoadAttribute(node, "File", "");
      if(file == "" || !System.IO.File.Exists(file)) return;

      EditorForm wform = (EditorForm)mform.StrategyManager.OpenFromFile(file);
      LoadSettingForm(node, wform);
      wform.SetBounds(wform.Location.X, wform.Location.Y, wform.Width, wform.Height);
      wform.Visible = true;
    }

    private static int Rectangle(int p, int p_2, int p_3, int p_4) {
      throw new Exception("The method or operation is not implemented.");
    }
    #endregion

    #region private static void LoadStrategyDLL(XmlNode node)
    private static void LoadStrategyDLL(XmlNode node) {
      MainForm mform = GordagoMain.MainForm;
      string file = LoadAttribute(node, "File", "");
      if(file == "" || !System.IO.File.Exists(file)) return;

      StrategyDLLForm dform = (StrategyDLLForm)mform.StrategyManager.OpenFromFile(file);
      LoadSettingForm(node, dform);
      dform.SetBounds(dform.Location.X, dform.Location.Y, dform.Width, dform.Height);
      dform.Visible = true;
    }
    #endregion

    #region private static void LoadChart(XmlNode node)
    private static void LoadChart(XmlNode node) {
      MainForm mform = GordagoMain.MainForm;

      ISymbol symbol = GordagoMain.SymbolEngine.GetSymbol(LoadAttribute(node, "Symbol", "")); 
      if (symbol == null) return;

      ChartForm cform = GordagoMain.MainForm.ChartShowNewForm(symbol);
      LoadSettingForm(node, cform);
      cform.Templates.LoadFromNode(cform, node);
      cform.LoadSettingsCompleate();
    }
    #endregion

    #region public static void LoadSettingForm(XmlNode node, Form form)
    public static void LoadSettingForm(XmlNode node, Form form) {
      string state = LoadAttribute(node, "State", "max");
      switch(state) {
        case "max":
          form.WindowState = FormWindowState.Maximized;
          break;
        case "min":
          form.WindowState = FormWindowState.Minimized;
          break;
        case "normal":
          form.WindowState = FormWindowState.Normal;
          break;
      }

      int left = Convert.ToInt32(LoadAttribute(node, "Left", "10"));
      int top = Convert.ToInt32(LoadAttribute(node, "Top", "10"));
      int width = Convert.ToInt32(LoadAttribute(node, "Width", "410"));
      int height = Convert.ToInt32(LoadAttribute(node, "Height", "310"));
      form.Location = new System.Drawing.Point(left, top);
      form.SetBounds(left, top, width, height);
    }
    #endregion

    #region public static string LoadAttribute(XmlNode node, string name, string defval)
    public static string LoadAttribute(XmlNode node, string name, string defval) {
      XmlAttribute attr = node.Attributes[name];
      if(attr == null || attr.Value == string.Empty) {
        return defval;
      }
      return attr.Value;
    }
    #endregion

    #region public static DateTime LoadAttribute(XmlNode node, string name, DateTime defTime)
    public static DateTime LoadAttribute(XmlNode node, string name, DateTime defTime) {
      string ret = LoadAttribute(node, name, "");
      if(ret == "")
        return defTime;
      uint utime = Convert.ToUInt32(ret);
      long time = UNIXTICKS + utime * 10000000L;

      return new DateTime(time);
    }
    #endregion

    #region public static float CnvStringToFloat(string str)
    public static float CnvStringToFloat(string str) {
      string dpoint = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
      str = str.Replace(".", dpoint);
      str = str.Replace(",", dpoint);
      return Convert.ToSingle(str);
    }
    #endregion

    #region public static string CnvFloatToString(float val)
    public static string CnvFloatToString(float val) {
      string dpoint = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
      string str = Convert.ToString(val);
      return str.Replace(dpoint, ".");
    }
    #endregion

  }
}
