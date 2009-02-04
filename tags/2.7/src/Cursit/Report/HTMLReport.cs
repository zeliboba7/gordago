using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.IO;

namespace Cursit.Report {
  /*
   * HTML Отчет по шаблону:
   * <!-- CAPTION --> - Разделитель заголовка
   * <!-- ROW --> - разделитель строки
   */
  public class HTMLReport {

    public static string CAPTION_REPORT = "<!-- CAPTION_REPORT -->";

    public static string TABLE_WIDTH = "<!-- TABLE_WIDTH -->";

    private static string CAPTION = "<!-- CAPTION -->";
    private static string COLUMN = "<!-- COLUMN -->";
    private static string COLUMN_NAME = "<!-- COLUMN_NAME -->";
    private static string COLUMN_WIDTH = "<!-- COLUMN_WIDTH -->";
    private static string COLUMN_VALUE = "<!-- COLUMN_VALUE -->";
    private static string ROW = "<!-- ROW -->";

    private static string TITLE = "<!-- TITLE -->";
    private static string DATE = "<!-- DATE -->";
    private static string TIME = "<!-- TIME -->";

    public event EventHandler SavedComplete;

    private DataView _dv;
    private DataGridTableStyle _ts;
    private string _title = "";
    private string _fileTemplate = "", _fileName = "";
    private string _tmpTop = "", _tmpTableCaption = "", _tmpTableMidle = "";
    private string _tmpTableRow = "", _tmpTableBottom = "";
    private Exception _ex = null;

    private string _captionReport = "";

    public HTMLReport(DataView dv, DataGridTableStyle ts, string fileTemplate) {
      _dv = dv;
      _ts = ts;
      _fileTemplate = fileTemplate;
    }

    #region public Exception Exception
    public Exception Exception {
      get { return this._ex; }
    }
    #endregion

    #region public string Title
    public string Title {
      get { return this._title; }
      set { this._title = value; }
    }
    #endregion

    #region public string Caption
    public string Caption {
      get { return this._captionReport; }
      set { this._captionReport = value; }
    }
    #endregion

    #region public string FileName
    public string FileName {
      get { return this._fileName;}
      set { this._fileName = value; }
    }
    #endregion

    #region public void Save(string filename)
    public void Save(string filename) {
      _fileName = filename;
      this.SaveProcess();
    }
    #endregion

    #region public static string[] Split(string str, string separator) 
    public static string[] Split(string str, string separator) {
      List<string> list = new List<string>();
      int indexOf = str.IndexOf(separator);
      if (indexOf > -1) {
        string temp = str.Substring(0, indexOf);
        list.Add(temp);
        str = str.Substring(indexOf + separator.Length);
        string[] sa = Split(str, separator);
        list.AddRange(sa);
      } else if (str.Length > 0) {
        list.Add(str);
      }
      return list.ToArray();
    }
    #endregion

    public void SaveProcess() {
      _ex = null;
      try {
        if (!File.Exists(_fileTemplate))
          throw (new FileNotFoundException("File not found!", _fileTemplate));

        if (File.Exists(_fileName))
          File.Delete(_fileName);

        FileStream fs = new FileStream(_fileTemplate, FileMode.Open);
        TextReader tr = new StreamReader(fs, Encoding.GetEncoding("windows-1251"));

        string template = "";
        string line = "";
        while ((line = tr.ReadLine()) != null) {
          template += line + "\n";
        }
        fs.Close();

        fs = new FileStream(_fileName, FileMode.Create);
        TextWriter tw = new StreamWriter(fs, Encoding.GetEncoding("windows-1251"));

        string[] sa = Split(template, CAPTION);

        if (sa.Length != 3)
          throw (new Exception("Report structure error: " + CAPTION));

        _tmpTop = sa[0];
        _tmpTableCaption = sa[1];

        sa = Split(sa[2], ROW);
        _tmpTableMidle = sa[0];
        _tmpTableRow = sa[1];
        _tmpTableBottom = sa[2];

        int tblWidth = 0;
        float koef = 1.2f;

        for (int i = 0; i < _ts.GridColumnStyles.Count; i++) {
          DataGridColumnStyle col = _ts.GridColumnStyles[i];
          
          tblWidth += Convert.ToInt32(col.Width * koef);
        }

        line = _tmpTop.Replace(TITLE, this.Title);
        line = line.Replace(DATE, DateTime.Now.ToShortDateString());
        line = line.Replace(TIME, DateTime.Now.ToShortTimeString());
        line = line.Replace(TABLE_WIDTH, tblWidth.ToString());
        line = line.Replace(CAPTION_REPORT, this.Caption);

        tw.Write(line);

        /* Заголовок таблицы */
        sa = Split(_tmpTableCaption, COLUMN);
        string strTop = sa[0];
        string strBody = sa[1];
        string strBottom = sa[2];

        line = strTop;
        for (int i = 0; i < _ts.GridColumnStyles.Count; i++) {
          
          DataGridColumnStyle col = _ts.GridColumnStyles[i];
          if (col.Width == 0)
            continue;

          int width = Convert.ToInt32(col.Width * koef);

          string colCapt = strBody.Replace(COLUMN_WIDTH, width.ToString());
          colCapt = colCapt.Replace(COLUMN_NAME, col.HeaderText);
          line += colCapt;
        }
        line += strBottom;
        line += _tmpTableMidle;
        tw.Write(line);

        /* Тело таблицы */
        foreach (DataRowView row in _dv) {
          sa = Split(_tmpTableRow, COLUMN);
          strTop = sa[0];
          strBody = sa[1];
          strBottom = sa[2];

          line = strTop;
          for (int i = 0; i < _ts.GridColumnStyles.Count; i++) {
            DataGridColumnStyle col = _ts.GridColumnStyles[i];
            if (col.Width == 0)
              continue;

            object value = row[col.MappingName];
            if (value is DateTime) {
              value = ((DateTime)value).ToString("dd-MM-yyyy");
            }
            string strval = value.ToString();
            if (strval.Length == 0) {
              strval = "&nbsp";
            }
            string colCapt = strBody.Replace(COLUMN_VALUE, strval);
            line += colCapt;
          }
          line += strBottom;
          tw.Write(line);
        }

        tw.Write(_tmpTableBottom);

        tw.Flush();
        tw.Close();
      } catch (Exception ex) {
        _ex = ex;
      }

      if (SavedComplete != null)
        SavedComplete(this, new EventArgs());
    }
  }
}
