/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections;

namespace Language {
	/// <summary>
	/// Группа.
	/// </summary>
	public class LangGroup {
		
		public string Name;
		private int nextId;
		public int Id;

		private ArrayList langVals;

		/// <summary>
		/// Импорт новой группы из строки.
		/// </summary>
		public LangGroup(string importString){
			string[] sa = importString.Split('|');
			Id = Convert.ToInt16(sa[0]);
			Name = sa[1];
			nextId = Convert.ToInt16(sa[2]);
			langVals = new ArrayList();
		}
		public LangGroup(){
			this.langVals = new ArrayList();
		}

		public LangGroup(int id, string Name) {
			this.Name = Name;
			this.Id = id;
			this.nextId = 1;
			langVals = new ArrayList();
		}

		public void CreateNewLangValue(string Text){
			LangValue lv = new LangValue(this.nextId++, Text);
			this.langVals.Add(lv);
		}

		public bool RemoveFromId(int id){
			int index = this.GetLangValueIndexFromId(id);
			if (index < 0) return false;
			this.langVals.RemoveAt(index);
			return true;
		}

		public int Count{
			get{return this.langVals.Count;}
		}

		public LangValue this[int index]{
			get{return (LangValue)langVals[index];}
		}

		public LangValue GetLangValueFromId(int id){
			int index = this.GetLangValueIndexFromId(id);
			if (index < 0) return null;
			return this[index];
		}

		public int GetLangValueIndexFromId(int id){
			int cnt = this.langVals.Count;
			for (int i=0; i<cnt; i++){
				LangValue lgv = (LangValue)langVals[i];
				if (lgv.Id == id) return i;
			}
			return -1;
		}

		/// <summary>
		/// Разбор строки текста.
		/// </summary>
		/// <param name="sitem"></param>
		public void ImportItem(string sitem){
			string[] sa = sitem.Split('|');
			LangValue lv = new LangValue(Convert.ToInt16(sa[0]), sa[1]);
			this.langVals.Add(lv);
		}

		public override string ToString() {
			return Convert.ToString(Id) + "|" + Name.Trim() + "|" + Convert.ToString(this.nextId);
		}

	}
}
