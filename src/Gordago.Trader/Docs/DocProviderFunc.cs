/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Xml;
using System.Collections;

namespace Gordago.Docs {
	public class DocProviderFunc {
		
		private string _typefunc;
		private bool _noconvert;
		private DocProviderParam[] _params;
		private string _replstring;
		private string _funcRetType = "double";
		private DocProviderIndic _provindic;

		internal DocProviderFunc(DocProviderIndic parent, XmlNode node) {
			this._provindic = parent;
			XmlAttribute attr = node.Attributes["Type"];
			if ( attr == null || attr.Value == string.Empty )
				throw new ArgumentOutOfRangeException("Error in XML: Function Type not found");
			_typefunc = attr.Value;

			attr = node.Attributes["MTFunc"];
			if ( attr == null || attr.Value == string.Empty ){
				//				throw new ArgumentOutOfRangeException("Error in XML: Function MTFunc not found");
				_replstring = "";
			}else{
				_replstring = attr.Value;
			}


			attr = node.Attributes["FuncRetType"];
			if ( attr != null && attr.Value != string.Empty ){
				_funcRetType = attr.Value;
			}

			attr = node.Attributes["NoConverted"];
			_noconvert = false;
			if (attr != null && attr.Value != string.Empty){
				_noconvert = bool.Parse(attr.Value);
			}

			ArrayList al = new ArrayList();
			foreach (XmlNode tnode in node.ChildNodes){
				if (tnode.Name == "Parameter"){
					al.Add(new DocProviderParam(tnode));
				}
			}
			this._params = (DocProviderParam[])al.ToArray(typeof(DocProviderParam));
		}

		#region public DocProviderIndic ProvIndicator
		public DocProviderIndic ProvIndicator{
			get{return this._provindic;}
		}
		#endregion 

		#region public string ReplString
		public string ReplString{
			get{return _replstring;}
		}
		#endregion

		#region public string TypeFunc
		public string TypeFunc{
			get{return this._typefunc;}
		}
		#endregion
		
		#region public DocProviderParam[] Params
		public DocProviderParam[] Params{
			get{return this._params;}
		}
		#endregion

		#region public string FuncRetType
		public string FuncRetType{
			get{return this._funcRetType;}
		}
		#endregion

		#region public bool NoConverted
		public bool NoConverted{
			get{return this._noconvert;}
		}
		#endregion
	}
}
