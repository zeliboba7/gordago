/**
* @version $Id: EasyProperties.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Core
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.IO;
  using System.Xml;
  using System.Threading;
  using System.Globalization;

  public class EasyProperties : EasyPropertiesNode {

    #region internal class TypeCollection
    internal class TypeCollection {
      private readonly List<Type> _values = new List<Type>();

      #region public int Count
      public int Count {
        get { return _values.Count; }
      }
      #endregion

      #region public Type this[int index]
      public Type this[int index] {
        get { return _values[index]; }
      }
      #endregion

      #region public int GetTypeId(Type type)
      public int GetTypeId(Type type) {
        
        int id = _values.IndexOf(type);
        if (id > -1)
          return id;

        _values.Add(type);
        return this.GetTypeId(type);
      }
      #endregion

      public void Add(Type type) {
        int index = _values.IndexOf(type);
        if (index > -1)
          throw(new ArgumentException("type already exist", "type"));
        _values.Add(type);
      }
    }
    #endregion

    public event EasyPropertiesChangedEventHandler PropertyChanged;
    private readonly string PXmlRootName = "PropertiesService";
    private readonly string PXmlVersion = "1.0";
    internal static readonly string PXmlRootPropertiesName = "RootProperties";

    public EasyProperties() : base(PXmlRootPropertiesName, null) { }

    #region internal protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
    internal protected virtual void OnPropertyChanged(EasyPropertiesChangedEventArgs e) {
      if (this.PropertyChanged != null)
        this.PropertyChanged(this, e);
    }
    #endregion

    #region private void FillTypeCollection(EasyPropertiesNode node, TypeCollection types)
    private void FillTypeCollection(EasyPropertiesNode node, TypeCollection types) {
      EasyPropertiesNode.Property[] values = node.GetValues();
      foreach (EasyPropertiesNode.Property value in values) {
        types.GetTypeId(value.Value.GetType());
      }

      EasyPropertiesNode[] childProperties = node.GetChildProperties();
      foreach (EasyPropertiesNode childProp in childProperties) {
        this.FillTypeCollection(childProp, types);
      }
    }
    #endregion

    #region public bool Load(FileInfo file)
    public bool Load(FileInfo file) {

      if (file == null)
        throw (new ArgumentNullException("file"));

      if (!file.Exists)
        return false;

      CultureInfo savedCulture = Thread.CurrentThread.CurrentCulture;
      Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

      try {
        lock (typeof(EasyProperties)) {
          using (XmlTextReader reader = new XmlTextReader(file.FullName)) {
            TypeCollection types = new TypeCollection();

            bool inTree = false;
            bool fBreak = false;
            while (reader.Read() && !fBreak) {
              switch (reader.NodeType) {
                case XmlNodeType.EndElement:
                  if (reader.LocalName == "Types")
                    fBreak = true;
                  break;
                case XmlNodeType.Element:
                  string propertyName = reader.LocalName;
                  if (propertyName == "Types") {
                    inTree = true;
                  }
                  if (inTree && propertyName == EasyPropertiesNode.XN_ELEMENT) {
                    string typeName = reader.GetAttribute(EasyPropertiesNode.XN_AT_TYPE);
                    types.Add(Type.GetType(typeName, true));
                  }
                  break;
              }
            }

            base.Read(reader, types);
          }
        }
        return true;
      } catch (Exception ex) {
        throw (new Exception("Error loading properties: " + ex.Message + "\nSettings have been restored to default values.", ex));
      } finally {
        Thread.CurrentThread.CurrentCulture = savedCulture;
      }
    }
    #endregion

    #region public void Save(FileInfo file)
    public void Save(FileInfo file) {

      lock (typeof(EasyProperties)) {
        file.Directory.Create();
        CultureInfo savedCulture = Thread.CurrentThread.CurrentCulture;
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        try {
          using (XmlTextWriter writer = new XmlTextWriter(file.FullName, Encoding.UTF8)) {
            writer.Formatting = Formatting.Indented;
            writer.WriteStartElement(PXmlRootName);
            writer.WriteAttributeString("version", PXmlVersion);

            TypeCollection types = new TypeCollection();
            this.FillTypeCollection(this, types);
            writer.WriteStartElement("Types");
            for (int i = 0; i < types.Count; i++) {
              Type type = types[i];
              writer.WriteStartElement(EasyPropertiesNode.XN_ELEMENT);
              writer.WriteAttributeString(EasyPropertiesNode.XN_INDEX, i.ToString());
              string typeName = string.Format("{0}, {1}", type.FullName, type.Assembly);
              writer.WriteAttributeString(EasyPropertiesNode.XN_AT_TYPE, typeName);
              writer.WriteEndElement();
            }
            writer.WriteEndElement();

            base.Write(writer, types);

            writer.WriteEndElement(); // PXmlRootName
          }
        } finally {
          Thread.CurrentThread.CurrentCulture = savedCulture;
        }
      }
    }
    #endregion
  }
}
