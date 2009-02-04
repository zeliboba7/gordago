/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.IO;
using System.Collections;

namespace Gordago.WebUpdate {
  /// <summary>
	/// Основная задача:
	///   прочитать описатель о фалах текущей проги и новой версии;
	///   выдать список фалов, которые необходимо заменить.
	/// </summary>
	public class UpdateSynchronizeListFile {

		private UpdateFileInfo[] _ulNew;
		private UpdateFileInfo[] _ulCurrent;
		private UpdateFileInfo[] _ulDelete;

		private string[] _filesDownload;

		public UpdateSynchronizeListFile(string fulNew, string fulCurrent) {
			this._ulNew = UpdateList(fulNew);
			this._ulCurrent = UpdateList(fulCurrent);

			_ulDelete = new UpdateFileInfo[]{};
			_filesDownload = new string[]{};
			if (_ulNew.Length < 1 || _ulCurrent.Length < 1) return ;

			ArrayList al = new ArrayList();

			foreach (UpdateFileInfo ufiNew in this.UpdateListNew){
				bool isfind = false;
				foreach (UpdateFileInfo ufiCur in this.UpdateListCurrent){
					if (ufiNew.FileName == ufiCur.FileName ){
						isfind = true;
						if(ufiNew.Version > ufiCur.Version || (ufiNew.IsCommerciy && Update.IsUpdateFromDemo))
							al.Add(ufiNew.FileName);
					}
				}
				if (!isfind)
					al.Add(ufiNew.FileName);
			}
			if (al.Count > 0)
				this._filesDownload = (string[])al.ToArray(typeof(string));
		}

		#region public UpdateFileInfo[] UpdateListNew
		public UpdateFileInfo[] UpdateListNew{
			get{return this._ulNew;}
		}
		#endregion

		#region public UpdateFileInfo[] UpdateListCurrent
		public UpdateFileInfo[] UpdateListCurrent{
			get{return this._ulCurrent;}
		}
		#endregion

		#region public string[] FilesDownload
		/// <summary>
		/// Список файлов для закачки
		/// </summary>
		public string[] FilesDownload{
			get{return this._filesDownload;}
		}
		#endregion

		#region public UpdateFileInfo[] UpdateListDelete - Список файлов для удаления
		/// <summary>
		/// Список файлов для удаления
		/// </summary>
		public UpdateFileInfo[] UpdateListDelete{
			get{return this._ulDelete;}
		}
		#endregion

		#region public static UpdateFileInfo[] UpdateList(string filename)
		public static UpdateFileInfo[] UpdateList(string filename) {
			UpdateFileInfo[] ufis = new UpdateFileInfo[]{};

			FileStream fs = new FileStream(filename, FileMode.Open);
			TextReader tr = new StreamReader(fs);
			ArrayList al = new ArrayList();
			while(tr.Peek() > -1){
				string s = tr.ReadLine().Trim();
				if (s.Length > 3){
					string[] sa = s.Split(new char[]{','});
					if (sa.Length >= 2){
						string flname = sa[0];
						string action = flname.Substring(0,1);
						flname = flname.Substring(1, flname.Length-1);
						int version = Convert.ToInt32(sa[1]);
						bool iscommercy = false;
						if (sa.Length >= 3 && sa[2] == "d")
							iscommercy = true;
							
						al.Add(new UpdateFileInfo(flname, version, action, iscommercy));
					}
				}
			}
			fs.Close();
			if (al.Count > 0)
				ufis = (UpdateFileInfo[])al.ToArray(typeof(UpdateFileInfo));
			return ufis;
		}
		#endregion

	}
}
