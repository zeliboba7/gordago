/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.IO;
using System.Collections;
using System.Data;

namespace Cursit.Utils {
	public class FileEngine {

		#region public static bool DeleteDir(string dir) - Удаление директории со всеми вложенными файлами и папками
		/// <summary>
		/// Удаление директории со всеми вложенными файлами и папками
		/// </summary>
		public static bool DeleteDir(string dir){
			if (!Directory.Exists(dir)) return false;
			DeleteFiles(dir);
			string[] dirs = Directory.GetDirectories(dir);
			foreach (string dr in dirs)
				if (!DeleteDir(dr)) return false;

			try{
				Directory.Delete(dir);
			}catch(Exception){
				return false;
			}
			return true;
		}
		#endregion

		#region public static void DeleteFiles(string dir)
		public static void DeleteFiles(string dir){
			if (!Directory.Exists(dir))
				return;
			string[] files = Directory.GetFiles(dir);
			foreach (string file in files)
				File.Delete(file);
		}
		#endregion

		#region public static void CheckDir(string filename) - Проверка директорий на возможность копирование файла
		/// <summary>
		/// Проверка директорий на возможность копирование файла,
		/// т.е. если директория, или поддериктория несуществует, то она создаеться
		/// </summary>
		public static void CheckDir(string filename){
			string[] da = filename.Split(new char[]{'\\'});
			string bpath = da[0];
			for (int i=1;i<da.Length-1;i++){
				bpath += "\\" + da[i];
				if (!Directory.Exists(bpath))
					Directory.CreateDirectory(bpath);
			}
		}
		#endregion

		#region public static void DeleteFile(string filename)
		public static void DeleteFile(string filename){
			if (File.Exists(filename)){
				File.Delete(filename);
			}
		}
		#endregion

		#region public static long GetSizeFile(string filename)
		public static long GetSizeFile(string filename){
			if (!System.IO.File.Exists(filename)){
				return -1;
			}
			FileStream fs = System.IO.File.OpenRead(filename);
			long lenght = fs.Length;
			fs.Close();
			return lenght;
		}
		#endregion

		#region public static string ConvertFileNameToDisplayString(string filename)
		public static string ConvertFileNameToDisplayString(string filename){
			string[] sa = filename.Split(new char[]{'\\'});
			if (sa.Length < 4)
				return filename;
			return sa[0] + "\\" + sa[1] + "\\...\\" + sa[sa.Length-1];
		}
		#endregion

		#region public static string GetFileNameFromPath(string path)
		public static string GetFileNameFromPath(string path){
			string[] sa = path.Split(new char[]{'\\'});
			return sa[sa.Length-1];
		}
		#endregion

    #region public static string GetFileExt(string filename)
    public static string GetFileExt(string filename) {
      string[] sa = filename.Split('.');
      return sa[sa.Length-1];
    }
    #endregion

    #region public static DisplayFile[] GetDisplayFiles(string[] files)
    public static DisplayFile[] GetDisplayFiles(string[] files){
			ArrayList al = new ArrayList();
			foreach (string file in files){
				al.Add(new DisplayFile(file));
			}
			return (DisplayFile[])al.ToArray(typeof(DisplayFile));
		}
		#endregion

		#region 	public class DisplayFile
		public class DisplayFile{
			private string _filename;
			private string _displayname;

			public DisplayFile(string filename){
				_filename = filename;
				_displayname = Cursit.Utils.FileEngine.GetFileNameFromPath(filename);
			}

			public string FileName{
				get{return this._filename;}
			}

			public string DisplayName{
				get{return this._displayname;}
			}

			public override string ToString() {
				return _displayname;
			}
		}
		#endregion

		#region public static string GetDirectory(string filepath)
		public static string GetDirectory(string filepath){
			string[] sa = filepath.Split(new char[]{'\\'});
			Cursit.Text.StringCreater scr = new Cursit.Text.StringCreater();
			for (int i=0;i<sa.Length-1;i++){
				scr.AppendString(sa[i]);
			}
			return scr.GetString("\\");
		}
		#endregion

    #region public static DataTable GetFiles(string dir)
    public static DataTable GetFiles(string dir) {
      return GetFiles(dir, "*.*");
    }
    #endregion

    #region public static DataTable GetFiles(string dir, string searchPattern)
    public static DataTable GetFiles(string dir, string searchPattern) {
      DataTable table = new DataTable("files");
      DataColumn col = table.Columns.Add("id", typeof(int));
      col.AutoIncrement = true;
      col.AutoIncrementSeed = 1;
      col.AutoIncrementStep = 1;
      col.Unique = true;
      table.PrimaryKey = new DataColumn[] { col};

      table.Columns.Add("path", typeof(string));
      table.Columns.Add("name", typeof(string));
      table.Columns.Add("size", typeof(long));
      table.Columns.Add("size_str", typeof(string),
        "iif(size>1048576, Convert(size/1048576,'System.Int32')+' M', iif(size>1024, Convert(size/1024,'System.Int32')+' K', size))"
        );

      string[] files = Directory.GetFiles(dir, searchPattern);
      foreach(string file in files){
        string name = GetFileNameFromPath(file);
        DataRow row = table.NewRow();
        row["path"] = file;
        row["name"] = name;
        row["size"] = GetSizeFile(file);



        table.Rows.Add(row);
      }

      return table;
    }
    #endregion
  }
}
