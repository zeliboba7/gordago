/**
* @version $Id: EasyPropertiesNode.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Core
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Xml;
  using System.ComponentModel;
  using System.Reflection;
  using System.Collections;
  using System.Diagnostics;

  /// <summary>
  /// Read/Write property
  /// </summary>
  public class EasyPropertiesNode {

    #region class PropertyKey
    class PropertyKey {

      private readonly int _hashCode;
      private readonly string _name;

      public PropertyKey(string propertyName) {
        _hashCode = propertyName.GetHashCode();
        _name = propertyName;
      }

      #region public string Name
      public string Name {
        get { return _name; }
      }
      #endregion

      #region public override int GetHashCode()
      public override int GetHashCode() {
        return _hashCode;
      }
      #endregion

      #region public override bool Equals(object obj)
      public override bool Equals(object obj) {
        if (!(obj is PropertyKey))
          return false;

        PropertyKey key = (PropertyKey)obj;

        return key._hashCode == _hashCode;
      }
      #endregion
    }
    #endregion

    #region public struct Property
    public struct Property {
      private readonly string _name;
      private readonly object _value;

      public Property(string name, object value) {
        _name = name;
        _value = value;
      }

      public string Name {
        get { return this._name; }
      }

      public object Value {
        get { return this._value; }
      }

      public override string ToString() {
        return string.Format("name={0}, value={1}", this.Name, this.Value);
      }
    }
    #endregion

    public static readonly string XN_PROPERTY = "Pp";
    public static readonly string XN_VALUES = "Vv";
    public static readonly string XN_ARRAY = "Ar";
    public static readonly string XN_ELEMENT = "El";
    public static readonly string XN_AT_NAME = "n";
    public static readonly string XN_AT_TYPE = "t";
    public static readonly string XN_AT_VALUE = "v";
    public static readonly string XN_INDEX = "index";

    private readonly Dictionary<PropertyKey, EasyPropertiesNode> _childProperties = new Dictionary<PropertyKey, EasyPropertiesNode>();
    private readonly Dictionary<PropertyKey, object> _values = new Dictionary<PropertyKey, object>();

    private readonly string _name;
    private readonly EasyPropertiesNode _parent;
    private readonly EasyProperties _root;

    #region public EasyProperties(string name, EasyProperties parent)
    public EasyPropertiesNode(string name, EasyPropertiesNode parent) {
      //Debug.WriteLine(string.Format("Property={0}, Parent={1}",name, parent));
      _name = name;
      _parent = parent;
      _root = this.GetRoot(parent);
      if (_root == null)
        throw (new ArgumentException("EasyProperties root not found.", "parent"));
    }
    #endregion

    #region public EasyProperties Parent
    public EasyPropertiesNode Parent {
      get { return this._parent; }
    }
    #endregion

    #region public string Name
    public string Name {
      get { return this._name; }
    }
    #endregion

    #region public EasyProperties this[string name]
    public EasyPropertiesNode this[string name] {
      get {
        EasyPropertiesNode ps = null;
        PropertyKey pkey = new PropertyKey(name);
        _childProperties.TryGetValue(pkey, out ps);
        if (ps != null)
          return ps;
        ps = new EasyPropertiesNode(name, this);
        _childProperties.Add(pkey, ps);
        return ps;
      }
    }
    #endregion

    #region public bool ContainsNode(string name)
    public bool ContainsNode(string name) {
      EasyPropertiesNode ps = null;
      PropertyKey pkey = new PropertyKey(name);
      _childProperties.TryGetValue(pkey, out ps);
      return ps != null;
    }
    #endregion

    public bool ContainsProperty(string propertyName) {

      PropertyKey pkey = new PropertyKey(propertyName);
      return _values.ContainsKey(pkey);
    }

    #region public void Clear()
    public void Clear() {
      _childProperties.Clear();
      _values.Clear();
    }
    #endregion

    #region public void Remove(string name)
    public void Remove(string name) {
      PropertyKey pkey = new PropertyKey(name);
      _values.Remove(pkey);
      _childProperties.Remove(pkey);
    }
    #endregion

    #region public EasyPropertiesNode[] GetChildProperties()
    public EasyPropertiesNode[] GetChildProperties() {
      List<EasyPropertiesNode> list = new List<EasyPropertiesNode>();
      foreach (EasyPropertiesNode properties in _childProperties.Values) {
        list.Add(properties);
      }
      return list.ToArray();
    }
    #endregion

    #region public Property[] GetValues()
    public Property[] GetValues() {
      List<Property> list = new List<Property>();
      foreach (PropertyKey key in _values.Keys) {

        list.Add(new Property(key.Name, _values[key]));
      }
      return list.ToArray();
    }
    #endregion

    #region private EasyProperties GetRoot(EasyPropertiesNode parent)
    private EasyProperties GetRoot(EasyPropertiesNode parent) {

      if (parent == null) {
        return this as EasyProperties;
      }
      if (parent.Parent == null)
        return parent as EasyProperties;
      return GetRoot(parent.Parent) as EasyProperties;
    }
    #endregion

    #region public T GetValue<T>(string propertyName, T defaultValue)
    public T GetValue<T>(string propertyName, T defaultValue) {
      object o = null;
      PropertyKey pkey = new PropertyKey(propertyName);
      _values.TryGetValue(pkey, out o);
      if (o == null) {
        _values.Add(pkey, defaultValue);
        return defaultValue;
      }
      return (T)o;
    }
    #endregion

    #region public void SetValue<T>(string propertyName, T value)
    public void SetValue<T>(string propertyName, T value) {
      T oldValue = default(T);

      PropertyKey pkey = new PropertyKey(propertyName);

      if (value == null) {
        _values.Remove(pkey);
        return;
      }

      if (!_values.ContainsKey(pkey)) {
        _values.Add(pkey, value);
      } else {
        oldValue = GetValue<T>(propertyName, value);
        _values[pkey] = value;
      }
      Debug.WriteLine(string.Format("Name={0},Value={1}", propertyName, value));
      _root.OnPropertyChanged(new EasyPropertiesChangedEventArgs(this, oldValue, value));
    }
    #endregion

    #region internal void Read(XmlTextReader reader, EasyProperties.TypeCollection types)
    internal void Read(XmlTextReader reader, EasyProperties.TypeCollection types) {
      if (reader.IsEmptyElement) {
        return;
      }

      while (reader.Read()) {
        switch (reader.NodeType) {
          case XmlNodeType.EndElement:
            if (reader.LocalName == XN_PROPERTY)
              return;
            break;
          case XmlNodeType.Element:
            string propertyName = reader.LocalName;
            if (propertyName == XN_PROPERTY) {

              string name = reader.GetAttribute(XN_AT_NAME);
              if (name == EasyProperties.PXmlRootPropertiesName && this is EasyProperties)
                continue;
              EasyPropertiesNode ps = new EasyPropertiesNode(name, this);
              _childProperties.Add(new PropertyKey(name), ps);
              ps.Read(reader, types);
            } else if (propertyName == XN_VALUES) {
              this.ReadValues(reader, types);
            }
            break;
        }
      }
    }
    #endregion

    #region private void ReadValues(XmlTextReader reader, EasyProperties.TypeCollection types)
    private void ReadValues(XmlTextReader reader, EasyProperties.TypeCollection types) {
      if (reader.IsEmptyElement)
        return;

      while (reader.Read()) {
        switch (reader.NodeType) {
          case XmlNodeType.EndElement:
            if (reader.LocalName == XN_VALUES)
              return;
            break;
          case XmlNodeType.Element:
            if (reader.LocalName == XN_ELEMENT) {

              string pname = reader.GetAttribute(XN_AT_NAME);
              int typeIndex = Convert.ToInt32( reader.GetAttribute(XN_AT_TYPE));

              Type type = types[typeIndex];

              object value = null;

              if (type.IsArray) {
                value = this.ReadArray(reader, type);
              } else {
                value = this.ReadValue(reader, type);
              }
              this.SetValue<object>(pname, value);
            }
            break;
        }
      }
    }
    #endregion

    #region private Array ReadArray(XmlTextReader reader, Type type)
    private Array ReadArray(XmlTextReader reader, Type type) {
      ArrayList l = new ArrayList();

      Type elType = type.GetElementType();
      if (reader.IsEmptyElement)
        return l.ToArray(elType);

      while (reader.Read()) {
        switch (reader.NodeType) {
          case XmlNodeType.EndElement:
            if (reader.LocalName == XN_ELEMENT)
              return l.ToArray(elType);
            break;
          case XmlNodeType.Element:
            if (reader.LocalName == XN_ARRAY) 
              l.Add(this.ReadValue(reader, elType));
            break;
        }
      }
      return l.ToArray(elType);
    }
    #endregion

    #region private object ReadValue(XmlTextReader reader, Type type)
    private object ReadValue(XmlTextReader reader, Type type) {
      object value = null;
      TypeConverter c = TypeDescriptor.GetConverter(type);

      if (c.CanConvertTo(typeof(System.Byte[]))) {
        int base64len = 0;
        byte[] base64 = new byte[1000000];
        do {
          base64len += reader.ReadBase64(base64, base64len, 50);

        } while (reader.Name == XN_ELEMENT);
        byte[] bytes = new byte[base64len];
        Array.Copy(base64, bytes, base64len);

        value = c.ConvertFrom(bytes);
      } else {
        string sval = reader.GetAttribute(XN_AT_VALUE);
        value = c.ConvertFrom(sval);
      }
      return value;
    }
    #endregion

    #region internal void Write(XmlTextWriter writer, EasyProperties.TypeCollection types)
    internal void Write(XmlTextWriter writer, EasyProperties.TypeCollection types) {
      writer.WriteStartElement(XN_PROPERTY);
      writer.WriteAttributeString(XN_AT_NAME, this.Name);

      this.WriteValues(writer, types);
      this.WriteChildNodes(writer, types);

      writer.WriteEndElement();
    }
    #endregion

    #region private void WriteChildNodes(XmlTextWriter writer, EasyProperties.TypeCollection types)
    private void WriteChildNodes(XmlTextWriter writer, EasyProperties.TypeCollection types) {
      EasyPropertiesNode[] childProperties = this.GetChildProperties();
      foreach (EasyPropertiesNode ps in childProperties) {
        ps.Write(writer, types);
      }
    }
    #endregion

    #region private void WriteValues(XmlTextWriter writer, EasyProperties.TypeCollection types)
    private void WriteValues(XmlTextWriter writer, EasyProperties.TypeCollection types) {
      Property[] values = this.GetValues();
      if (values.Length == 0)
        return;
      writer.WriteStartElement(XN_VALUES);
      foreach (Property value in values) {
        writer.WriteStartElement(XN_ELEMENT);
        writer.WriteAttributeString(XN_AT_NAME, value.Name);
        this.WriteValue(writer, value, types);
        writer.WriteEndElement();
      }
      writer.WriteEndElement();
    }
    #endregion

    #region private void WriteValue(XmlTextWriter writer, Property property, EasyProperties.TypeCollection types)
    private void WriteValue(XmlTextWriter writer, Property property, EasyProperties.TypeCollection types) {
      object val = property.Value;

      if (val == null)
        return;

      Type type = val.GetType();

      writer.WriteAttributeString(XN_AT_TYPE, types.GetTypeId(type).ToString());

      if (val is Array || val is ArrayList) {
//        writer.WriteStartElement(XN_ARRAY);
        foreach (object o in (IEnumerable)val) {
//          writer.WriteStartElement(XN_ELEMENT);
          writer.WriteStartElement(XN_ARRAY);
          this.WriteValue(writer, o);
          writer.WriteEndElement();
          //          writer.WriteEndElement();
        }
//        writer.WriteEndElement();
      } else {
        this.WriteValue(writer, val);
      }
    }
    #endregion

    #region private void WriteValue(XmlTextWriter writer, object value)
    private void WriteValue(XmlTextWriter writer, object value) {
      TypeConverter c = TypeDescriptor.GetConverter(value.GetType());
      if (c.CanConvertTo(typeof(System.Byte[]))) {
        byte[] bs = c.ConvertTo(value, typeof(System.Byte[])) as byte[];
        writer.WriteBase64(bs, 0, bs.Length);
      } else {
        writer.WriteAttributeString(XN_AT_VALUE, value.ToString());
      }
    }
    #endregion

    #region public override string ToString()
    public override string ToString() {
      return this.Name;
    }
    #endregion
  }
}
