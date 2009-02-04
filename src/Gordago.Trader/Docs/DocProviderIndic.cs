/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections;

namespace Gordago.Docs {
	public class DocProviderIndic {

		private string _name;
		private DocProviderFunc[] _provfuncs;
		private string _define="";


		internal DocProviderIndic(string name) {
			this._name = name;
			_provfuncs = new DocProviderFunc[]{};
		}

		public DocProviderFunc[] ProvFuncs{
			get{return this._provfuncs;}
		}

		#region public string DefineIndicator
		/// <summary>
		/// Индикатор в MQL4
		/// </summary>
		public string DefineIndicator{
			get{return this._define;}
			set{this._define = value;}
		}
		#endregion

		#region internal void Add(DocProviderFunc provfunc)
		internal void Add(DocProviderFunc provfunc){
			ArrayList al = new ArrayList(this._provfuncs);
			al.Add(provfunc);
			this._provfuncs = (DocProviderFunc[])al.ToArray(typeof(DocProviderFunc));
		}
		#endregion

		#region public DocProviderFunc GetProvFunction(string typefunc)
		public DocProviderFunc GetProvFunction(string typefunc){
			foreach (DocProviderFunc dpfunc in this._provfuncs){
				if (dpfunc.TypeFunc == typefunc){
					return dpfunc;
				}
			}
			return null;
		}
		#endregion
	}
}
