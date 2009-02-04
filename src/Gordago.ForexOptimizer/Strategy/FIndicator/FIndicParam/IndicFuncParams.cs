/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections;
using Gordago.Docs;

namespace Gordago.Strategy.FIndicator.FIndicParam {
	/// <summary>
	/// Коллекция параметров
	/// </summary>
	public class IndicFuncParams {

		private IndicFuncParam[] _ifparams;

		public IndicFuncParams() {
			_ifparams = new IndicFuncParam[]{};
		}

		public void Add(IndicFuncParam ifParam){
			ArrayList al = new ArrayList(_ifparams);
			al.Add(ifParam);
			_ifparams = new IndicFuncParam[al.Count];
			al.CopyTo(_ifparams);
		}

		public IndicFuncParam[] Params{
			get{return this._ifparams;}
		}

		public IndicFuncParam this[int index]{
			get{return this._ifparams[index] as IndicFuncParam;}
		}

		public void CopyTo(IndicFuncParams prs){
			int cnt = this.Params.Length;
			for (int i=0;i<cnt;i++){
				IndicFuncParam ifpFrom = this[i] as IndicFuncParam;
				IndicFuncParam ifpTo = prs[i] as IndicFuncParam;
				ifpFrom.CopyTo(ifpTo);
			}
		}
		public string GetChartString() {
			Cursit.Text.StringCreater sc = new Cursit.Text.StringCreater();
			for (int i=0;i<this.Params.Length;i++){
				IndicFuncParam ifp = this[i];
				sc.AppendString(ifp.ToString());
			}
			return sc.GetString(", ");
		}

	}

}
