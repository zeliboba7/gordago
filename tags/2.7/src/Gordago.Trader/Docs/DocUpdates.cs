/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.IO;
using System.Collections;

namespace Gordago.Docs{
	public class DocUpdates {

		#region private property
		public const string FILE_NAME = "descript.uht";
		private string _filename;
		private UpdateRow[] _updrows;
		#endregion

		private string _path;

    public static string GetFile(string path) {
      return path + "\\" + FILE_NAME;
    }

		#region public DocUpdates(string path) 
		public DocUpdates(string path) {
			_path = path;
      this._filename = GetFile(path);

			if (!System.IO.File.Exists(this._filename)){
				this.UpdateRows = new UpdateRow[]{};
				return;
			}

			StreamReader sr = File.OpenText(this._filename);
			String line;
			ArrayList al = new ArrayList();
			while ((line=sr.ReadLine())!=null) {
				if (line.Length > 3)
					al.Add(new UpdateRow(line));
			}
			sr.Close();
			this.UpdateRows = (UpdateRow[])al.ToArray(typeof(UpdateRow));
		}
		#endregion

    #region public void Clear()
    public void Clear() {
      _updrows = new UpdateRow[] { };
    }
    #endregion

    #region public void AddUpdateRow(UpdateRow row)
    public void AddUpdateRow(UpdateRow row){
			foreach (UpdateRow frow in this.UpdateRows){
				if (frow.FileName == row.FileName){
					frow.FileName = row.FileName;
					frow.BeginDTime = row.BeginDTime;
					frow.EndDTime = row.EndDTime;
					return;
				}
			}
			ArrayList al = new ArrayList(this.UpdateRows);
			al.Add(row);
			this.UpdateRows = (UpdateRow[])al.ToArray(typeof(UpdateRow));
		}
		#endregion

		#region public void UpdateFileInfo(string symbolname, string filename, DateTime bdtm, DateTime edtm, int counttick)
		public void UpdateFileInfo(string symbolname, string filename, DateTime bdtm, DateTime edtm, int counttick){
			UpdateRow urow = new UpdateRow(symbolname, filename, bdtm, edtm, counttick);
			this.AddUpdateRow(urow);
//			Save();
		}
		#endregion

		#region public void Save()
		public void Save(){
			if (System.IO.File.Exists(this._filename))
				System.IO.File.Delete(this._filename);
			StreamWriter sw = File.CreateText(this._filename);

			foreach (UpdateRow row in this.UpdateRows){
				sw.WriteLine(row.GetSaveString());
			}
			sw.Close();
		}
		#endregion

		#region public void UpdateSize()
		/// <summary>
		/// Обновление размера архивов
		/// </summary>
		public void UpdateSize(){
			foreach(UpdateRow row in this.UpdateRows){
				string rarfile = row.FileName.Replace(".gtk", ".rar");
				string filename = this._path + "\\" + rarfile;
				if (File.Exists(filename)){
					FileStream fs = File.Open(filename,FileMode.Open);
					row.FileSizeArhive = (int)fs.Length;
					row.FileName = rarfile;
					fs.Close();
				}
			}
			this.Save();
		}
		#endregion

		#region public UpdateRow[] UpdateRows
		public UpdateRow[] UpdateRows{
			get{return this._updrows;}
			set{this._updrows = value;}
		}
		#endregion
	}

	#region public class UpdateRow
	public class UpdateRow{

		#region private property
		private string _filename;
		private DateTime _bdtm;
		private DateTime _edtm;
		private int _filesize;
		private string _symbolname;
		private int _counttick;
		#endregion

		private bool _view = true;

		public UpdateRow(string symbolname, string filename, DateTime bdtm, DateTime edtm, int counttick){
			_symbolname = symbolname;
			_filename = filename;
			_bdtm = bdtm;
			_edtm = edtm;
			_counttick = counttick;
		}

		#region public UpdateRow(string parse)
		public UpdateRow(string parse){
			string[] sa = parse.Split(new char[]{'|'});
			_symbolname = sa[0];
			_filename = sa[1];
			this.BeginDTime = GetDTimeFromString(sa[2]);
			this.EndDTime = GetDTimeFromString(sa[3]);
			_counttick = Convert.ToInt32(sa[4]);
			this.FileSizeArhive = Convert.ToInt32(sa[5]);
		}
		#endregion

		#region public string SymbolName
		public string SymbolName{
			get{return this._symbolname;}
		}
		#endregion

		#region public bool View
		public bool View{
			get{return this._view;}
		}
		#endregion

		#region public string FileName
		public string FileName{
			get{return this._filename;}
			set{this._filename = value;}
		}
		#endregion

		#region public DateTime BeginDTime
		public DateTime BeginDTime{
			get{return this._bdtm;}
			set{this._bdtm = value;}
		}
		#endregion

		#region public DateTime EndDTime
		public DateTime EndDTime{
			get{return this._edtm;}
			set{this._edtm = value;}
		}
		#endregion

		#region public int CountTick
		public int CountTick{
			get{return this._counttick;}
			set{this._counttick = value;}
		}
		#endregion

		#region public int FileSizeArhive
		public int FileSizeArhive{
			get{return this._filesize;}
			set{this._filesize = value;}
		}
		#endregion

		#region public static DateTime GetDTimeFromString(string str)
		public static DateTime GetDTimeFromString(string str){
			int year = Convert.ToInt32(str.Substring(0,4));
			int month = Convert.ToInt32(str.Substring(4,2));
			int day = Convert.ToInt32(str.Substring(6,2));
			int hour = Convert.ToInt32(str.Substring(8,2));
			int minute = Convert.ToInt32(str.Substring(10,2));
			return new DateTime(year, month, day, hour, minute, 0,0);
		}
		#endregion

		#region public string GetSaveString()
		public string GetSaveString(){
			ArrayList al = new ArrayList();
			al.Add(this.SymbolName);
			al.Add(this.FileName);
			al.Add(CreateDateTimeString(this.BeginDTime));
			al.Add(CreateDateTimeString(this.EndDTime));
			al.Add(Convert.ToString(this.CountTick));
			al.Add(this.FileSizeArhive.ToString());
			string[] sa = (string[])al.ToArray(typeof(string));


			return string.Join("|", sa);
		}
		#endregion

		#region private static string CreateDateTimeString(DateTime dtm)
		private static string CreateDateTimeString(DateTime dtm){
			string str = "";
			str += dtm.Year.ToString();
			str += CreateZerroString(dtm.Month, 2, '0');
			str += CreateZerroString(dtm.Day, 2, '0');
			str += CreateZerroString(dtm.Hour, 2, '0');
			str += CreateZerroString(dtm.Minute, 2, '0');
			return str;
		}
		#endregion

		#region private static string CreateZerroString(int val, int count, char ch)
		private static string CreateZerroString(int val, int count, char ch){
			string str  = val.ToString();
			return new string(ch, count - str.Length) + str;
		}
		#endregion
	}
	#endregion
}
