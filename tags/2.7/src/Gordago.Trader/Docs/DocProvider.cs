/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Xml;
using System.Collections;

namespace Gordago.Docs {
	public class DocProvider {


		public static DocProviderIndic[] LoadFromXML(string filename) {
			XmlDocument doc = new XmlDocument();
			doc.Load(filename);
			XmlNode node = doc["Provider"];
			if ( node == null )
				throw new ArgumentOutOfRangeException("Error in XML: Provider not found");

			ArrayList al = new ArrayList();  
			foreach (XmlNode temp in node.ChildNodes){
				if ( temp.Name == "Indicator" ){
					XmlAttribute attr = temp.Attributes["Name"];
					if ( attr == null || attr.Value == string.Empty )
						throw new ArgumentOutOfRangeException("Error in XML: Indicator Name not found");

					DocProviderIndic dpindic = new DocProviderIndic(attr.Value);
					foreach (XmlNode temp1 in temp.ChildNodes){
						if (temp1.Name == "Function"){
							dpindic.Add(new DocProviderFunc(dpindic, temp1)); 
						}else if (temp1.Name == "DefIndicator"){
							dpindic.DefineIndicator = temp1.InnerText;
						}
					}
					al.Add(dpindic);
				}
			}
			return (DocProviderIndic[])al.ToArray(typeof(DocProviderIndic));
		}
	}
}
