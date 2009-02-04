/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Xml;

namespace Gordago.Docs {
	public class DocProviderParam {
		private string _mtname;
		private int _gsoindex;
		private DocParameterType _prmtype;

		internal DocProviderParam(XmlNode node) {
			XmlAttribute attr = node.Attributes["MT"];
			if ( attr == null || attr.Value == string.Empty )
				throw new ArgumentOutOfRangeException();
			_mtname = attr.Value;

			attr = node.Attributes["GSO"];
			if ( attr == null || attr.Value == string.Empty )
				throw new ArgumentOutOfRangeException();
			_gsoindex = Convert.ToInt32(attr.Value);

			attr = node.Attributes["Type"];
			if ( attr == null || attr.Value == string.Empty )
				throw new ArgumentOutOfRangeException();
			switch(attr.Value){
				case "Enum":
					this._prmtype = DocParameterType.Enum;
					break;
				case "Integer":
					this._prmtype = DocParameterType.Integer;
					break;
				case "Vector":
					this._prmtype = DocParameterType.Vector;
					break;
				case "Float":
					this._prmtype = DocParameterType.Float;
					break;
				default:
					throw new ArgumentOutOfRangeException("Error in XML: ParamType");
			}
		}

		public string MTName{
			get{return this._mtname;}
		}

		public int GSOIndex{
			get{return this._gsoindex;}
		}

		public DocParameterType PrmType{
			get{return this._prmtype;}
		}
	}
}

