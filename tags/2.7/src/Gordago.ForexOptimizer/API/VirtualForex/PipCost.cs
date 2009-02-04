/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace Gordago.API.VirtualForex {

  /// <summary>
  /// Цена символа, необходимо для 
  /// </summary>
  class SymbolsProperty {

    private const string FILENAME = "symbols.xml";

    public static void Load(OnlineRateList onlineRates) {
      XmlDocument doc = new XmlDocument();
      string filename = Application.StartupPath + "\\" + FILENAME;
      if(!System.IO.File.Exists(filename)) return;
      doc.Load(filename);
      XmlNode node = doc["PipCost"];
      if(node == null) return;

      foreach(XmlNode childnode in node.ChildNodes) {
        try {
          if(childnode.Name == "Symbol") {
            string sName = LoadAttribute(childnode, "Name", "Unknow");
            int spread = Convert.ToInt32(LoadAttribute(childnode, "Spread", "3"));
            float pipCost = CnvStringToFloat(LoadAttribute(childnode, "Cost", "1.0"));

            OnlineRate onlineRate = (OnlineRate)onlineRates.GetOnlineRate(sName);
            if (onlineRate != null) {
              onlineRate.SetPipCost(pipCost);
              onlineRate.SetSpread(spread);
            }
          }
        } catch { }
      }
    }

    #region public static void Save(IOnlineRateList onlineRates)
    public static void Save(IOnlineRateList onlineRates) {
      string xmlstr = "<PipCost Version=\"1.0\"></PipCost>";

      XmlDocument doc = new XmlDocument();
      doc.LoadXml(xmlstr);
      for (int i = 0; i < onlineRates.Count; i++) {
        IOnlineRate onlineRate = onlineRates[i];
        XmlNode node = doc.CreateElement("Symbol");
        int spread =Convert.ToInt32( (onlineRate.BuyRate - onlineRate.SellRate) * SymbolManager.GetDelimiter(onlineRate.Symbol.DecimalDigits));
        SetValueAttrNode(node, "Name", onlineRate.Symbol.Name);
        SetValueAttrNode(node, "Spread", Convert.ToString(spread));
        SetValueAttrNode(node, "Cost", CnvFloatToString(onlineRate.PipCost));
        doc.DocumentElement.AppendChild(node);
      }

      doc.Save(Application.StartupPath + "\\" + FILENAME);
    }
    #endregion

    #region private static void SetValueAttrNode(XmlNode node, string name, string value)
    private static void SetValueAttrNode(XmlNode node, string name, string value) {
      node.Attributes.Append(node.OwnerDocument.CreateAttribute(name));
      node.Attributes[name].Value = value;
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

    #region private static float CnvStringToFloat(string str)
    private static float CnvStringToFloat(string str) {
      string dpoint = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
      str = str.Replace(".", dpoint);
      str = str.Replace(",", dpoint);
      return Convert.ToSingle(str);
    }
    #endregion

    #region private static string CnvFloatToString(float val)
    private static string CnvFloatToString(float val) {
      string dpoint = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
      string str = Convert.ToString(val);
      return str.Replace(dpoint, ".");
    }
    #endregion
  }
}
