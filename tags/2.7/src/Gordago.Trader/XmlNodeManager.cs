/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Drawing;

namespace Gordago {
  public class XmlNodeManager {
    private XmlNode _node;
    private static long UNIXTICKS = new DateTime(1970, 1, 1, 0, 0, 0, 0).Ticks;

    public XmlNodeManager(XmlNode node) {
      _node = node;
    }

    #region public XmlNode XmlNode
    public XmlNode XmlNode {
      get { return this._node; }
    }
    #endregion

    #region public static string ConvertFloatToString(float value)
    /// <summary>
    /// ConvertFloatToString
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ConvertFloatToString(float value) {
      string dpoint = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
      string str = Convert.ToString(value);
      string svalue = str.Replace(dpoint, ".");
      return svalue;
    }
    #endregion

    #region public static float ConvertStringToFloat(string value)
    public static float ConvertStringToFloat(string value) {
      string dpoint = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
      value = value.Replace(".", dpoint);
      return Convert.ToSingle(value);
    }
    #endregion

    #region public void SetAttribute(string name, string value)
    public void SetAttribute(string name, string value) {
      _node.Attributes.Append(_node.OwnerDocument.CreateAttribute(name));
      _node.Attributes[name].Value = value;
    }
    #endregion

    #region public void SetAttribute(string name, Font value)
    public void SetAttribute(string name, Font value) {
      // throw (new Exception("void SetAttribute(string, Font) "));
    }
    #endregion

    #region public void SetAttribute(string name, bool value)
    public void SetAttribute(string name, bool value) {
      this.SetAttribute(name, value.ToString());
    }
    #endregion

    #region public void SetAttribute(string name, int value)
    public void SetAttribute(string name, int value) {
      this.SetAttribute(name, value.ToString());
    }
    #endregion

    #region public void SetAttribute(string name, DateTime value)
    public void SetAttribute(string name, DateTime value) {
      uint utime = (UInt32)((value.Ticks - UNIXTICKS) / 10000000L);
      SetAttribute(name, Convert.ToString(utime));
    }
    #endregion

    #region public void SetAttribute(string name, float value)
    public void SetAttribute(string name, float value) {
      this.SetAttribute(name, ConvertFloatToString(value));
    }
    #endregion

    #region public void SetAttribute(string name, Color value)
    public void SetAttribute(string name, Color value) {
      this.SetAttribute(name, value.ToArgb().ToString());
    }
    #endregion

    #region public string GetAttributeString(string name, string defaultValue)
    public string GetAttributeString(string name, string defaultValue) {
      XmlAttribute attr = _node.Attributes[name];
      if(attr == null || attr.Value == string.Empty) {
        return defaultValue;
      }
      return attr.Value;
    }
    #endregion

    #region public bool GetAttributeBoolean(string name, bool defaultValue)
    public bool GetAttributeBoolean(string name, bool defaultValue) {
      string ret = GetAttributeString(name, "");
      if (ret == "")
        return defaultValue;
      return Convert.ToBoolean(ret);
    }
    #endregion

    #region public DateTime GetAttributeDateTime(string name, DateTime defaultValue)
    public DateTime GetAttributeDateTime(string name, DateTime defaultValue) {
      string ret = GetAttributeString(name, "");
      if(ret == "")
        return defaultValue;
      uint utime = Convert.ToUInt32(ret);
      long time = UNIXTICKS + utime * 10000000L;

      return new DateTime(time);
    }
    #endregion

    #region public float GetAttributeFloat(string name, float defaultValue)
    public float GetAttributeFloat(string name, float defaultValue) {
      string ret = GetAttributeString(name, "");
      if(ret == "")
        return defaultValue;

      return ConvertStringToFloat(ret);
    }
    #endregion

    #region public Color GetAttributeColor(string name, Color defaultValue)
    public Color GetAttributeColor(string name, Color defaultValue) {
      string ret = GetAttributeString(name, "");
      if (ret == "")
        return defaultValue;
      return Color.FromArgb(Convert.ToInt32(ret));
    }
    #endregion

    #region public int GetAttributeInt32(string name, int defaultValue)
    public int GetAttributeInt32(string name, int defaultValue) {
      string ret = this.GetAttributeString(name, "");
      if (ret == "")
        return defaultValue;

      return Convert.ToInt32(ret);
    }
    #endregion

    public Font GetAttributeFont(string name, Font defaultValue) {
      return defaultValue;
    }
  }
}
