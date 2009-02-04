/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections;
using System.IO;

namespace Language {
	/// <summary>
	/// Summary description for Language.
	/// </summary>
	public class LanguageData {
		
		private string _name = "";
		
		private string _thisLangName = "";

		private string _fileName = "";

		private int nextId;
		
		private ArrayList langGroups;
		
		public LanguageData(){
			langGroups = new ArrayList();
			this.nextId = 1;
		}

		#region public string Name
		/// <summary>
		/// Наименование по Анлийски
		/// </summary>
		public string Name{
			get{return this._name;}
			set{this._name = value;}
		}
		#endregion

		#region public string ThisLangName
		/// <summary>
		/// Наименование на этом языке
		/// </summary>
		public string ThisLangName{
			get{return this._thisLangName;}
			set{this._thisLangName = value;}
		}
		#endregion

		public string FileName{
			get{return this._fileName;}
			set{this._fileName = value;}
		}

		#region public LangGroup this[int index]
		public LangGroup this[int index]{
			get{return (LangGroup)langGroups[index];}
		}
		#endregion

		#region public LangGroup this[string GroupName]
		public LangGroup this[string GroupName]{
			get{
				for (int i=0;i<this.Count;i++){
					LangGroup lngg = (LangGroup)langGroups[i];
					if (GroupName == lngg.Name){
						return lngg;
					}
				}
				LangGroup lngg1 = null;
				return lngg1;
			}
		}
		#endregion


		/// <summary>
		/// Создание новой группы в этой коллекции.
		/// </summary>
		private int Add(LangGroup lngGroup){
			return this.langGroups.Add(lngGroup);
		}

		/// <summary>
		/// Создание новой группы
		/// </summary>
		public LangGroup CreateNewLangGroup(string groupName){
			LangGroup lgrp = new LangGroup(this.nextId++, groupName);
			Add(lgrp);
			return lgrp;
		}

		#region public bool RemoveFromId(int id)
		public bool RemoveFromId(int id){
			int index = this.GetLangGroupIndexFromId(id);
			if (index < 0) return false;
			this.langGroups.RemoveAt(index);
			return true;
		}
		#endregion

		public LangGroup GetLangGroupFromId(int id){
			return this[GetLangGroupIndexFromId(id)];
		}

		public int GetLangGroupIndexFromId(int id){
			int cnt = this.Count;
			for (int i=0; i<cnt; i++){
				LangGroup lgrp = (LangGroup)this.langGroups[i];
				if (lgrp.Id == id) return i;
			}
			return -1;
		}

		public string GetDictionaryString(int GroupId, int ValueId){
			int cnt = this.Count;
			for (int i=0;i<cnt;i++){
				LangGroup lgrp = (LangGroup)this.langGroups[i];
				if (lgrp.Id == GroupId){
					LangValue lv = lgrp.GetLangValueFromId(ValueId);
					if (lv == null) return "Lang Item not Found";
					return lv.Text;
				}
			}
			return "Error in LangDict";
		}

		/// <summary>
		/// Кол-во групп
		/// </summary>
		public int Count {
			get{return langGroups.Count;}
		}

		/// <summary>
		/// Загрузка данных из файла
		/// </summary>
		/// <param name="FileName"></param>
		public void Load(string fileName){
			this.FileName = fileName;

			FileStream fs = new FileStream(_fileName, FileMode.Open);
			StreamReader sr = new StreamReader(fs);
			sr.BaseStream.Seek(0, SeekOrigin.Begin);
			
			string ss = sr.ReadLine();
			string[] ssa = ss.Split('|');
			this.Name = ssa[0];
			this.ThisLangName = ssa[1];
			this.nextId = Convert.ToInt16(ssa[2]);

			string bs = "";
			string sval = "";

			LangGroup lngg = new LangGroup();
			while(sr.Peek()>-1){
				string s = sr.ReadLine();
				if (s.Length > 2){
					bs = s.Substring(0,1);
					sval = s.Substring(1, s.Length-1);
					
					if (bs == "."){
						lngg = new LangGroup(sval);
						this.Add(lngg);
					}else if (bs == "\t"){
						lngg.ImportItem(sval);
					}
				}
			}
			fs.Close();
		}

		/// <summary>
		/// Сохранение данных в файл
		/// </summary>
		public void Save(){
			if (this.FileName == ""){ 
				return;
			}
			if (File.Exists(FileName))
				File.Delete(FileName);

			FileStream fs = new FileStream(_fileName, FileMode.CreateNew);

			StreamWriter sw = new StreamWriter(fs);
			string capt = Name.Trim() + "|" + ThisLangName + "|" + Convert.ToString(nextId);
			sw.WriteLine(capt);

			int cnt = this.langGroups.Count;
			for (int i=0;i<cnt;i++){
				LangGroup lgrp = (LangGroup)this.langGroups[i];
				sw.WriteLine("." + lgrp.ToString());
				int cnt1 = lgrp.Count;
				for (int ii=0;ii<cnt1;ii++){
					sw.WriteLine("\t" + lgrp[ii]);
				}
			}
			sw.Flush();
			fs.Close();
		}


	}
}

