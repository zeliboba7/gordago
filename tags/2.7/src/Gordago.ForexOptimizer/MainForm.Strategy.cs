/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;

using Language;
using Gordago.Strategy;
using Gordago.Strategy.IO;
using Gordago.Stock;
using Gordago.API;
using Gordago.Analysis.Chart;
using Cursit.Applic.AConfig;
using Cursit.Utils;
#endregion

namespace Gordago {
  partial class MainForm {

    #region private void ExportMQL4()
    private void ExportMQL4() {
      EditorForm wf = null;
      if(this.ActiveMdiChild is EditorForm) {
        wf = this.ActiveMdiChild as EditorForm;
      } else {
        MessageBox.Show(Dictionary.GetString(1, 3), GordagoMain.MessageCaption);
        return;
      }

      StrategyExportMQL4OptionsForm soform = new StrategyExportMQL4OptionsForm();
      if(soform.ShowDialog() != DialogResult.OK) {
        return;
      }

      if(soform.MQL4Options.PatternFile == "") {
        MessageBox.Show(Dictionary.GetString(25, 15, "Файл шаблона для экспорта в MQL не найден"), GordagoMain.MessageCaption);
        return;
      }

#if DEMO
      MessageBox.Show(Dictionary.GetString(1, 8), GordagoMain.MessageCaption);
      this.ShowRegPage();
      return;
#else

      StrategyExportMQL4 mql4 = null;
      try {
        mql4 = new StrategyExportMQL4(wf, soform.MQL4Options);
        if(mql4.NoConverted) {
          MessageBox.Show(Dictionary.GetString(25, 13, "В стратегии присутствуют индикаторы которые не подлежат конвертации"), GordagoMain.MessageCaption);
          return;
        }
      } catch(Exception e) {
        MessageBox.Show(e.Message, GordagoMain.MessageCaption);
        return;
      }


      StrategyExportMQLCustomIndicForm semcf = new StrategyExportMQLCustomIndicForm(mql4);

      if(mql4.DefIndicators.Length > 0) {
        if(semcf.ShowDialog() != DialogResult.OK)
          return;
      }

      string KEY_REESTR_APP_PATH = @"Software\MetaQuotes Software\MetaTrader 4\Settings";
      string app_path = "";
      RegistryKey regKey = null;
      try {
        regKey = Registry.CurrentUser.OpenSubKey(KEY_REESTR_APP_PATH);
        if(regKey != null) {
          String[] valueNames = regKey.GetValueNames();
          foreach(String valueName in valueNames) {
            if(valueName == "ProgramPath") {
              object obj = regKey.GetValue(valueName);
              string strkey = obj as string;
              string[] sa = strkey.Split(new char[] { '\\' });
              if(sa.Length >= 2) {
                string[] sat = new string[sa.Length - 1];
                for(int i = 0; i < sa.Length - 1; i++) {
                  sat[i] = sa[i];
                }
                app_path = String.Join("\\", sat);
              }
            }
          }
        }
        if(app_path.Length <= 2 || !System.IO.Directory.Exists(app_path))
          app_path = "";

      } finally {
        if(regKey != null) {
          regKey.Close();
        }
      }

      //////////////////////////////////////////////////////////////////////////
      /// сохранение стратегии в мт4
      string tmppath = app_path + "\\experts";
      string path = "";
      if(System.IO.Directory.Exists(tmppath))
        path = Config.Users["PathMT4Experts", tmppath];

      System.Windows.Forms.SaveFileDialog sfd = new SaveFileDialog();
      sfd.Filter = "MetaQuotes language (*.mq4)|*.mq4|All files (*.*)|*.*";
      sfd.InitialDirectory = path;
      sfd.ShowDialog();
      if(sfd.FileName == "") return;

      path = sfd.InitialDirectory;
      Config.Users["PathMT4Experts"].SetValue(path);

      if(File.Exists(sfd.FileName))
        File.Delete(sfd.FileName);

      string dir = System.IO.Directory.GetParent(sfd.FileName).FullName;

      FileStream fs = new FileStream(sfd.FileName, FileMode.CreateNew);
      TextWriter tw = new StreamWriter(fs, new Cursit.Text.Encoding866());
      tw.Write(mql4.Template);
      tw.Flush();
      fs.Close();

      //////////////////////////////////////////////////////////////////////////
      /// Экспорт индикатора в МТ4
      tmppath = app_path + "\\experts\\indicators";
      path = "";
      if(System.IO.Directory.Exists(tmppath))
        path = Config.Users["PathMT4Indicators", tmppath];


      foreach(Strategy.IO.DefineIndicator dind in semcf.DefineIndicators) {
        sfd = new SaveFileDialog();
        sfd.Filter = "MetaQuotes language (*.mq4)|*.mq4|All files (*.*)|*.*";
        sfd.InitialDirectory = path;
        sfd.FileName = dind.Name + ".mq4";
        sfd.ShowDialog();
        if(sfd.FileName != "") {
          path = sfd.InitialDirectory;
          Config.Users["PathMT4Indicators"].SetValue(path);

          string filename = sfd.FileName;
          if(File.Exists(filename))
            File.Delete(filename);

          FileStream fsi = new FileStream(filename, FileMode.CreateNew);
          TextWriter twi = new StreamWriter(fsi, new Cursit.Text.Encoding866());
          twi.Write(dind.Body);
          twi.Flush();
          fsi.Close();
        }
      }
#endif
    }
    #endregion

    #region private void OpenStrategyFromMenuFastFile(int cntitemfile)
    private void OpenStrategyFromMenuFastFile(int cntitemfile) {
      string filename = this._laststrategyfiles[cntitemfile];
      if(!System.IO.File.Exists(filename)) {
        this.UpdateLastOpenStrategyFileInMenu();
        return;
      }
      _sewfs.OpenFromFile(filename);

    }
    #endregion

    #region private void UpdateLastOpenStrategyFileInMenu()
    private void UpdateLastOpenStrategyFileInMenu() {

      ArrayList al = new ArrayList();

      for(int i = 0; i < 5; i++) {
        string menuname = "_mniFileSymbol" + i.ToString();
        ToolStripItem mni = null;
        switch(i) {
          case 0:
            mni = _mniFileSymbol0;
            break;
          case 1:
            mni = _mniFileSymbol1;
            break;
          case 2:
            mni = _mniFileSymbol2;
            break;
          case 3:
            mni = _mniFileSymbol3;
            break;
          case 4:
            mni = _mniFileSymbol4;
            break;
        }
        string strategyname = "FStrategy" + i.ToString();

        string filename = Config.Users["Strategy"][strategyname, ""];
        if(filename != "" && System.IO.File.Exists(filename)) {
          Config.Users["Strategy"][strategyname].SetValue(filename);
          al.Add(filename);
          mni.Text = FileEngine.ConvertFileNameToDisplayString(filename);
          mni.Visible = true;
        } else {
          Config.Users["Strategy"][strategyname].SetValue("");
          mni.Visible = false;
        }

      }
      _laststrategyfiles = (string[])al.ToArray(typeof(string));
    }
    #endregion

    #region public void SetLastOpenStrategyFile(string filename)
    public void SetLastOpenStrategyFile(string filename) {
      ArrayList al = new ArrayList();
      al.Add(filename);
      foreach(string s in _laststrategyfiles) {
        if(s != filename)
          al.Add(s);
      }

      string[] sa = (string[])al.ToArray(typeof(string));

      for(int i = 0; i < 5; i++) {
        string strategyname = "FStrategy" + (i).ToString();
        if(i < sa.Length) {
          Config.Users["Strategy"][strategyname].SetValue(sa[i]);
        } else {
          Config.Users["Strategy"][strategyname].SetValue("");
        }
      }
      this.UpdateLastOpenStrategyFileInMenu();
    }
    #endregion

    #region private void CreateNewStrategy()
    private void CreateNewStrategy() {
#if ASDF
      CreateStrategyOrIndicatorForm soiFrm = new CreateStrategyOrIndicatorForm();
      if (soiFrm.ShowDialog() == DialogResult.Cancel) 
        return;
      if (soiFrm.SelectedIndex == 0) {
#endif
      EditorForm eform = new EditorForm();
      eform.SetDefaultFileName();
      eform.MdiParent = this;
      eform.Visible = true;
#if ASDF
      } else {
        CustomIndicatorForm indFrm = new CustomIndicatorForm();
        indFrm.MdiParent = this;
        indFrm.Visible = true;
      }
#endif
    }
    #endregion

    #region private void SaveAsStrategy()
    private void SaveAsStrategy() {
      if(this.ActiveMdiChild is EditorForm) {
        (this.ActiveMdiChild as EditorForm).SaveAs();
      }
    }
    #endregion

    #region private void SaveStrategy()
    private void SaveStrategy() {
      if(this.ActiveMdiChild is EditorForm) {
        (this.ActiveMdiChild as EditorForm).Save();
      }
    }
    #endregion

    #region private bool CloseAllStrategy()
    /// <summary>
    /// Проверяет на наличие открытых и несохраненных стратегий.
    /// Если находит таковые, то предлогает сохранить
    /// </summary>
    private bool CloseAllStrategy() {
      EditorForm[] wfs = this.GetStrategyForms();
      foreach(EditorForm wf in wfs) {
        if(wf.ShowMessageForSave()) {
        } else
          return false;
      }
      return true;
    }
    #endregion

    #region private EditorForm[] GetStrategyForms()
    private EditorForm[] GetStrategyForms() {
      ArrayList al = new ArrayList();
      foreach(Form frm in this.MdiChildren) {
        if(frm is EditorForm)
          al.Add(frm);
      }
      EditorForm[] wfs = (EditorForm[])al.ToArray(typeof(EditorForm));
      return wfs;
    }
    #endregion
  }
}
