/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

using Cursit.Applic.AConfig;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;

namespace Gordago.Analysis.Chart {
	public class ChartStyleManager {
		private List<ChartStyle> _styles;
		private ConfigValue _cfg;
    private string _dir;

    private static string DIR = "\\resources\\ch_style";
    private static string EXT = "gcs";

		public ChartStyleManager() {
      _styles = new List<ChartStyle>();
      _cfg = Config.Users["Chart"];

      _dir = Application.StartupPath + DIR;

      Cursit.Utils.FileEngine.CheckDir(_dir + "\\tmp.tmp");

      string[] files = Directory.GetFiles(_dir, "*."+EXT);

      for(int i = 0; i < files.Length; i++) {
        ChartStyle cs = Load(files[i]);
        if(cs != null)
          _styles.Add(cs);
      }

      #region "Default"
      if(this["Default"] == null) {
        ChartStyle cs = new ChartStyle();
        cs.Name = "Default";
        _styles.Add(cs);
        this.Save(cs, false);
      }
      #endregion

      #region "The Pink Symphony"
      if(this["The Pink Symphony"] == null) {

        ChartStyle cs = new ChartStyle();
        cs.Name = "The Pink Symphony";
        cs.BackColor = Color.FromArgb(255, 236, 239);
        cs.ScaleForeColor = Color.FromArgb(68, 68, 68);
        cs.GridColor = Color.FromArgb(80, 98, 98, 98);
        cs.BorderColor = Color.FromArgb(98, 98, 98);
        cs.BarColor = Color.Black;
        cs.BarDownColor = Color.FromArgb(125, 125, 255);
        cs.BarUpColor = Color.FromArgb(255, 157, 206);
        _styles.Add(cs);
        this.Save(cs, false);
      }
      #endregion

      #region "Treble clef"
      if(this["Treble clef"] == null) {
        ChartStyle cs = new ChartStyle();
        cs.Name = "Treble clef";
        cs.BackColor = Color.Honeydew;
        cs.ScaleForeColor = Color.FromArgb(40, 68, 68);
        cs.GridColor = Color.FromArgb(80, Color.Teal);
        cs.BorderColor = Color.Teal;
        cs.BarColor = Color.SaddleBrown;
        cs.BarUpColor = Color.CornflowerBlue;
        cs.BarDownColor = Color.RosyBrown;
        _styles.Add(cs);
        this.Save(cs, false);
      }
      #endregion

      #region "Duet"
      if(this["Duet"] == null) {
        ChartStyle cs = new ChartStyle();
        cs.Name = "Duet";
        cs.BackColor = Color.FromArgb(210, 210, 210);
        cs.ScaleForeColor = Color.FromArgb(74, 0, 130);
        cs.GridColor = Color.FromArgb(80, 55, 55, 55);
        cs.BorderColor = Color.FromArgb(55, 55, 55);
        cs.BarColor = Color.DarkSlateGray;
        cs.BarUpColor = Color.FromArgb(255, 130, 192);
        cs.BarDownColor = Color.FromArgb(75, 75, 75);
        _styles.Add(cs);
        this.Save(cs, false);
      }
      #endregion

      #region "MetaTrader"
      if(this["MetaTrader"] == null) {
        ChartStyle cs = new ChartStyle();
        cs.Name = "MetaTrader";
        cs.BackColor = Color.Black;
        cs.BorderColor = Color.FromArgb(105, 105, 105);
        cs.GridColor = Color.FromArgb(105, 105, 105);
        cs.ScaleForeColor = Color.White;
        cs.BarColor = Color.FromArgb(0, 255, 0);
        cs.BarDownColor = Color.Black;
        cs.BarUpColor = Color.White;
        _styles.Add(cs);
        this.Save(cs, false);
      }
      #endregion

      if (this["Gordago"] == null) {
        ChartStyle cs = new ChartStyle();
        cs.Name = "Gordago";
        cs.BackColor = Color.FromArgb(244, 245, 255);
        cs.BorderColor = Color.FromArgb(105, 105, 105);
        cs.GridColor = Color.FromArgb(105, 105, 105);
        cs.ScaleForeColor = Color.Black;
        cs.BarColor = Color.FromArgb(42, 42, 42);
        cs.BarDownColor = Color.FromArgb(128, 255, 128);
        cs.BarUpColor = Color.FromArgb(255, 251, 204);

        cs.SellTradeColor = Color.FromArgb(-29556);
        cs.BuyTradeColor = Color.FromArgb(-6967297);
        cs.StopTradeColor = Color.FromArgb(-8388608);
        cs.LimitTradeColor = Color.FromArgb(-15045594);

        cs.SellOrderColor = Color.FromArgb(-29556);
        cs.BuyOrderColor = Color.FromArgb(-6967297);
        cs.StopOrderColor = Color.FromArgb(-8388608);
        cs.LimitOrderColor = Color.FromArgb(-15045594);

        _styles.Add(cs);
        this.Save(cs, false);
      }

    }

    #region public ChartStyle this[int index]
    public ChartStyle this[int index]{
      get { return this._styles[index]; }
    }
    #endregion

    #region public ChartStyle this[string name]
    public ChartStyle this[string name] {
      get {
        for(int i = 0; i < _styles.Count; i++) {
          if (_styles[i].Name.ToUpper() == name.ToUpper())
            return _styles[i];
        }
        return null;
      }
    }
    #endregion

    #region public int Count
    public int Count {
      get { return this._styles.Count; }
    }
    #endregion

    #region public ChartStyle[] GetStyles()
    public ChartStyle[] GetStyles() {
      return _styles.ToArray();
    }
    #endregion

    #region public ChartStyle Load(string filename)
    public ChartStyle Load(string filename) {
      ChartStyle cs = new ChartStyle();
      try {
        XmlDocument doc = new XmlDocument();
        doc.Load(filename);

        XmlNode node = doc["ChartStyle"];
        if(node == null) return null;

        cs.Name = Cursit.Utils.FileEngine.GetFileNameFromPath(filename).Replace("." + EXT, "");

        foreach(XmlNode childnode in node.ChildNodes) {
          if(childnode.Name == "Color") {
            string name = LoadAttribute(childnode, "Name", "");
            string value = childnode.InnerText;
            switch(name) {
              case "BackColor":
                cs.BackColor = Color.FromArgb(Convert.ToInt32(value));
                break;
              case "BarColor":
                cs.BarColor = Color.FromArgb(Convert.ToInt32(value));
                break;
              case "BarDownColor":
                cs.BarDownColor = Color.FromArgb(Convert.ToInt32(value));
                break;
              case "BarUpColor":
                cs.BarUpColor = Color.FromArgb(Convert.ToInt32(value));
                break;
              case "BorderColor":
                cs.BorderColor = Color.FromArgb(Convert.ToInt32(value));
                break;
              case "GridColor":
                cs.GridColor = Color.FromArgb(Convert.ToInt32(value));
                break;
              case "ScaleForeColor":
                cs.ScaleForeColor = Color.FromArgb(Convert.ToInt32(value));
                break;
              case "SellTradeColor":
                cs.SellTradeColor = Color.FromArgb(Convert.ToInt32(value));
                break;
              case "SellOrderColor":
                cs.SellOrderColor = Color.FromArgb(Convert.ToInt32(value));
                break;
              case "BuyTradeColor":
                cs.BuyTradeColor = Color.FromArgb(Convert.ToInt32(value));
                break;
              case "BuyOrderColor":
                cs.BuyOrderColor = Color.FromArgb(Convert.ToInt32(value));
                break;
              case "StopTradeColor":
                cs.StopTradeColor = Color.FromArgb(Convert.ToInt32(value));
                break;
              case "StopOrderColor":
                cs.StopOrderColor = Color.FromArgb(Convert.ToInt32(value));
                break;
              case "LimitTradeColor":
                cs.LimitTradeColor = Color.FromArgb(Convert.ToInt32(value));
                break;
              case "LimitOrderColor":
                cs.LimitOrderColor = Color.FromArgb(Convert.ToInt32(value));
                break;
            }
          }
        }
      } catch {
        return null;
      }
      return cs;
    }
    #endregion

    #region private static string LoadAttribute(XmlNode node, string name, string defval)
    private static string LoadAttribute(XmlNode node, string name, string defval) {
      XmlAttribute attr = node.Attributes[name];
      if(attr == null || attr.Value == string.Empty) {
        return defval;
      }
      return attr.Value;
    }
    #endregion

    #region public void Save(ChartStyle cs)
    public void Save(ChartStyle cs) {
      this.Save(cs, true);
    }
    #endregion

    #region public void Save(ChartStyle cs, bool isReplace)
    public void Save(ChartStyle cs, bool isReplace) {

      XmlDocument doc = new XmlDocument();
      doc.LoadXml("<ChartStyle Version=\"1.0\"></ChartStyle>");

      CreateCSNode(doc, "BackColor", cs.BackColor);
      CreateCSNode(doc, "BorderColor", cs.BorderColor);
      CreateCSNode(doc, "GridColor", cs.GridColor);
      CreateCSNode(doc, "ScaleForeColor", cs.ScaleForeColor);
      CreateCSNode(doc, "BarColor", cs.BarColor);
      CreateCSNode(doc, "BarDownColor", cs.BarDownColor);
      CreateCSNode(doc, "BarUpColor", cs.BarUpColor);

      CreateCSNode(doc, "SellTradeColor", cs.SellTradeColor);
      CreateCSNode(doc, "BuyTradeColor", cs.BuyTradeColor);
      CreateCSNode(doc, "StopTradeColor", cs.StopTradeColor);
      CreateCSNode(doc, "LimitTradeColor", cs.LimitTradeColor);

      CreateCSNode(doc, "BuyOrderColor", cs.BuyOrderColor);
      CreateCSNode(doc, "SellOrderColor", cs.SellOrderColor);
      CreateCSNode(doc, "StopOrderColor", cs.StopOrderColor);
      CreateCSNode(doc, "LimitOrderColor", cs.LimitOrderColor);

      string filename = Application.StartupPath + DIR +"\\"+ cs.Name + "." + EXT;
      if(File.Exists(filename)) {
        if(!isReplace)
          return;
        File.Delete(filename);
      }
      doc.Save(filename);
    }
    #endregion

    #region private Assembly OnAssemblyResolveEventHandler(object sender, ResolveEventArgs args)
    //private Assembly OnAssemblyResolveEventHandler(object sender, ResolveEventArgs args) {
    //  return typeof(ChartStyle).Assembly;
    //}
    #endregion

    #region private XmlNode CreateCSNode(XmlDocument doc, string name, Color value)
    private XmlNode CreateCSNode(XmlDocument doc, string name, Color value) {
      XmlNode node = doc.CreateElement("Color");
      doc.DocumentElement.AppendChild(node);
      SetValueAttrNode(node, "Name", name);
      node.InnerText = value.ToArgb().ToString();
      return node;
    }
    #endregion

    #region private static void SetValueAttrNode(XmlNode node, string name, string value)
    private static void SetValueAttrNode(XmlNode node, string name, string value) {
      node.Attributes.Append(node.OwnerDocument.CreateAttribute(name));
      node.Attributes[name].Value = value;
    }
    #endregion

		#region public ChartStyle GetChartStyle(string name)
		public ChartStyle GetChartStyle(string name){
			foreach (ChartStyle style in this._styles){
				if (style.Name.ToLower() == name.ToLower())
					return style;
			}
			return this._styles[0];
		}
		#endregion

		#region public void SetDefaultStyle(ChartStyle chartstyle)
		public void SetDefaultStyle(ChartStyle chartstyle){
			_cfg["StyleName"].SetValue(chartstyle.Name);
		}
		#endregion

		#region public ChartStyle GetDefaultStyle()
		public ChartStyle GetDefaultStyle(){
      string sname = _cfg["StyleName", "Gordago"];
			return this.GetChartStyle(sname);
		}
		#endregion

    #region public void Delete(string name)
    public void Delete(string name) {
      List<ChartStyle> list = new List<ChartStyle>();
      for(int i = 0; i < this._styles.Count; i++) {
        if(this._styles[i].Name != name) {
          list.Add(_styles[i]);
        }
      }
      _styles.Clear();
      _styles.AddRange(list.ToArray());
    }
    #endregion

    #region public void Add(ChartStyle cs)
    public void Add(ChartStyle cs) {
      _styles.Add(cs);
    }
    #endregion
  }
}
