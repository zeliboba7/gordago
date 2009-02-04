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
	///  ласс дл€ расчета дублирующих параметров. 
	/// ’ранит в себе ключ параметра, и сам описатель на него.
	/// </summary>
	public class CalculateDoubleParam {
				
		private System.Collections.Hashtable _doubleParam;
				
		public CalculateDoubleParam(DocIndicator docI) {

			System.Collections.Hashtable _hashTable = new Hashtable();
			this._doubleParam = new Hashtable();

						
			foreach(DocFunction docF in docI.Functions) {
				foreach(DocParameter docp in docF.Parameters){

					// уникальный ключ параметра, дл€ его идентификации
					string keyval = CreateKey(docp);
					// если данный параметр дубликат, то увеличение кол-ва дубликатов на 1
					// иначе, создание новой записи в таблице дубликатов
					if (_hashTable.ContainsKey(keyval)){
						DocParamItem dpi = (DocParamItem)_hashTable[keyval];
						int val = dpi.Count;
						_hashTable[keyval] = new DocParamItem(docp, val+1);
					}else{
						_hashTable.Add(keyval, new DocParamItem(docp, 1));
					}
				}
			}
								
			// ≈сли в таблице наход€тс€ дубликаты параметров, то перенос их в основную таблицу
			IDictionaryEnumerator idenum = _hashTable.GetEnumerator();
			while ( idenum.MoveNext() ){
				DocParamItem dpi = (DocParamItem)idenum.Value;
				if (dpi.Count > 1)
					this._doubleParam.Add(idenum.Key, idenum.Value);
			}				
		}
		public static string CreateKey(DocParameter docp){
			return docp.Name.Trim() + docp.Default.ToString().Trim();
		}
		/// <summary>
		/// ѕроверка параметра, на вхождение его в дубликате
		/// </summary>
		public bool IsDoubleParam(DocParameter docp){
			string key = CreateKey(docp);
			IDictionaryEnumerator idenum = this._doubleParam.GetEnumerator();
			while(idenum.MoveNext()){
				string s = (string)idenum.Key;
				if (s == key) return true;
			}
			return false;
		}

		public IDictionaryEnumerator GetEnumerator(){
			return this._doubleParam.GetEnumerator();
		}
		public int Count{
			get{return this._doubleParam.Count;}
		}
		/// <summary>
		/// ≈сть ли в функции параметры, неотнос€щихс€ к общим
		/// </summary>
		public bool IntoNonDoubleParam(DocParameter[] docParams){
			foreach(DocParameter docp in docParams){
				if (!this.IsDoubleParam(docp)) return true;
			}
			return false;
		}
		internal struct DocParamItem{
			public DocParameter DocParameter;
			public int Count;
			public DocParamItem(DocParameter docParameter, int count){
				this.DocParameter = docParameter;
				this.Count = count;
			}
		}

	}
}
