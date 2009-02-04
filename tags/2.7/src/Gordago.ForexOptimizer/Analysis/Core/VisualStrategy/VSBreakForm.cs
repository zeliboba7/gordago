/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Gordago.Strategy {
  partial class VSBreakForm : Form {

    public VSBreakForm() {
      InitializeComponent();
    }

    #region public string TextBox
    public string TextBox {
      get { return _textBox.Text; }
      set { 
        this._textBox.Text = value;
        this._textBox.SelectAll();
        this._textBox.Focus();
      }
    }
    #endregion

    private void VSBreakForm_Load(object sender, EventArgs e) {

    }

    public static BreackRecord[] CheckBreakPointTime() {
      string path = Application.StartupPath + "\\vsbreak.txt";

      List<BreackRecord> dtm = new List<BreackRecord>();

      if (!File.Exists(path)) 
        return dtm.ToArray();
      int year = 0, month = 0, day = 0, hour = 0, minute = 0, second = 0;
      bool ext = false;
      TextReader tr = new StreamReader(path);

      string line;
      while ((line = tr.ReadLine()) != null) {
        try {
          string[] sa = line.Split('|');
          year = Convert.ToInt32(sa[0]);
          month = Convert.ToInt32(sa[1]);
          day = Convert.ToInt32(sa[2]);
          hour = Convert.ToInt32(sa[3]);
          minute = Convert.ToInt32(sa[4]);
          second = Convert.ToInt32(sa[5]);
          ext = Convert.ToBoolean(sa[6]);
          dtm.Add(new BreackRecord(new DateTime(year, month, day, hour, minute, second), ext));
        } catch { 

        }
      }

      return dtm.ToArray();
    }

    private void _btnClose_Click(object sender, EventArgs e) {
      this.Close();
    }
  }
}