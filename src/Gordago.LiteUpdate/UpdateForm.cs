/**
* @version $Id: UpdateForm.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Data;
  using System.Drawing;
  using System.Text;
  using System.Windows.Forms;
  using System.Threading;
  using System.IO;

  public partial class UpdateForm : Form {

    private readonly UpdateManager _updateManager ;
    private string _htmlDetails = "";
    private bool _updateStarted = false;

    public UpdateForm(UpdateManager updateManager) {
      _updateManager = updateManager;
      InitializeComponent();
      _updateManager.StartGetUpdateDetails += new EventHandler(_updateManager_StartGetUpdateDetails);
      _updateManager.StopGetUpdateDetails += new EventHandler(_updateManager_StopGetUpdateDetails);
      _updateManager.WebReader.StartDownloadFile += new WebReaderEventHandler(WebReader_StartDownloadFile);
      _updateManager.WebReader.StopDownloadFile += new WebReaderEventHandler(WebReader_StopDownloadFile);
      _updateManager.WebReader.DownloadFileProcess += new WebReaderProcessEventHandler(WebReader_DownloadFileProcess);
      
      _updateManager.StartUpdate += new EventHandler(_updateManager_StartUpdate);
      _updateManager.StopUpdate += new EventHandler(_updateManager_StopUpdate);

      _updateManager.StartUnZipFile += new UnZipFileEventHandler(_updateManager_StartUnZipFile);
      _updateManager.UnZipFileProgress += new UnZipProgressEventHandler(_updateManager_UnZipFileProgress);
      _updateManager.StopUnZipFile += new UnZipFileEventHandler(_updateManager_StopUnZipFile);
    }

    #region protected override void OnClosed(EventArgs e)
    protected override void OnClosed(EventArgs e) {
      base.OnClosed(e);
      _updateManager.StartGetUpdateDetails -= new EventHandler(_updateManager_StartGetUpdateDetails);
      _updateManager.StopGetUpdateDetails -= new EventHandler(_updateManager_StopGetUpdateDetails);
      _updateManager.WebReader.StartDownloadFile -= new WebReaderEventHandler(WebReader_StartDownloadFile);
      _updateManager.WebReader.StopDownloadFile -= new WebReaderEventHandler(WebReader_StopDownloadFile);
      _updateManager.WebReader.DownloadFileProcess -= new WebReaderProcessEventHandler(WebReader_DownloadFileProcess);

      _updateManager.StartUpdate -= new EventHandler(_updateManager_StartUpdate);
      _updateManager.StopUpdate -= new EventHandler(_updateManager_StopUpdate);

      _updateManager.StartUnZipFile -= new UnZipFileEventHandler(_updateManager_StartUnZipFile);
      _updateManager.UnZipFileProgress -= new UnZipProgressEventHandler(_updateManager_UnZipFileProgress);
      _updateManager.StopUnZipFile -= new UnZipFileEventHandler(_updateManager_StopUnZipFile);
    }
    #endregion

    #region protected override void Dispose(bool disposing)
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }
    #endregion

    #region protected override void OnLoad(EventArgs e)
    protected override void OnLoad(EventArgs e) {
      base.OnLoad(e);
      _lblProcessName.Text = "";
      _lblANewVersion.Text = string.Format("A new version of {0} is available.", Configure.ProductName);
      _txtLog.Clear();
    }
    #endregion

    #region private void _btnViewDetails_Click(object sender, EventArgs e)
    private void _btnViewDetails_Click(object sender, EventArgs e) {
      _updateManager.GetUpdateDetails();
    }
    #endregion

    #region private void WriteLog(string text)
    private void WriteLog(string text) {
      _txtLog.AppendText(text);
    }
    #endregion

    #region private void SetEnabledButton(bool enabled)
    private void SetEnabledButton(bool enabled) {
      _btnStart.Enabled = _btnViewDetails.Enabled = enabled;
    }
    #endregion

    #region private void _updateManager_StartGetUpdateDetails(object sender, EventArgs e)
    private void _updateManager_StartGetUpdateDetails(object sender, EventArgs e) {
      if (this.InvokeRequired) {
        this.Invoke(new EventHandler(_updateManager_StartGetUpdateDetails), sender, e);
      } else {
        _lblProcessName.Text = "Creation of the detailed information";
        SetEnabledButton(false);
      }
    }
    #endregion

    #region private void _updateManager_StopGetUpdateDetails(object sender, EventArgs e)
    private void _updateManager_StopGetUpdateDetails(object sender, EventArgs e) {
      if (this.InvokeRequired) {
        this.Invoke(new EventHandler(_updateManager_StopGetUpdateDetails), sender, e);
      } else {
        _lblProcessName.Text = "";
        if (_updateManager.Error == UpdateManagerError.None) {
          _htmlDetails =_updateManager.HtmlDetailsInfo;
        } else {
          string html = "<html><head><title>{0}</title></head><body><h1>Error</h1><p>{0}=\"{1}\"</p></body></html>";
          _htmlDetails = string.Format(html, UpdateManagerErrorParse.ToString(_updateManager.Error), _updateManager.ErrorMessage);
        }
        SetEnabledButton(true);
        this.OpenHtml();
      }
    }
    #endregion

    #region private void OpenHtml()
    private void OpenHtml() {
      FileInfo file = new FileInfo( Path.Combine(Configure.UpdateDirectory.FullName, "UpdateDetailed.html"));
      if (file.Exists)
        file.Delete();
      File.WriteAllText(file.FullName, _htmlDetails);
      System.Diagnostics.Process.Start(file.FullName);
    }
    #endregion

    #region private void WebReader_StartDownloadFile(object sender, WebReaderEventArgs e)
    private void WebReader_StartDownloadFile(object sender, WebReaderEventArgs e) {
      if (this.InvokeRequired) {
        this.Invoke(new WebReaderEventHandler(WebReader_StartDownloadFile), sender, e);
        return;
      }
      _progressBar.Value = 0;
      this.WriteLog(string.Format("Download '{0}' - ", e.File.FullName));
    }
    #endregion

    #region private void WebReader_StopDownloadFile(object sender, WebReaderEventArgs e)
    private void WebReader_StopDownloadFile(object sender, WebReaderEventArgs e) {
      if (this.InvokeRequired) {
        this.Invoke(new WebReaderEventHandler(WebReader_StopDownloadFile), sender, e);
      } else {
        _lblProcessName.Text = "";
        _progressBar.Value = 0;
        if (e.Error != UpdateManagerError.None) {
          this.WriteLog(string.Format("{0}=\"{1}\"", UpdateManagerErrorParse.ToString(e.Error), e.ErrorMessage));
        } else {
          this.WriteLog(string.Format("ok ({0})", UpdateDetailsManager.ConvertToSize(e.Size)));
        }
        this.WriteLog(Environment.NewLine);
      }
    }
    #endregion

    #region private void WebReader_DownloadFileProcess(object sender, WebReaderProcessEventArgs e)
    private void WebReader_DownloadFileProcess(object sender, WebReaderProcessEventArgs e) {
      if (this.InvokeRequired) {
        this.Invoke(new WebReaderProcessEventHandler(WebReader_DownloadFileProcess), sender, e);
      } else {
        _progressBar.Maximum = e.Total;
        _progressBar.Value = e.Current;
      }
    }
    #endregion

    #region private void _updateManager_StartUpdate(object sender, EventArgs e)
    private void _updateManager_StartUpdate(object sender, EventArgs e) {
      if (this.InvokeRequired) {
        this.Invoke(new EventHandler(_updateManager_StartUpdate), sender, e);
        return;
      }
      _txtLog.Clear();
      _btnViewDetails.Enabled = false;
      _btnStart.Text = "Cancel";
      _updateStarted = true;
    }
    #endregion

    #region private void _updateManager_StopUpdate(object sender, EventArgs e)
    private void _updateManager_StopUpdate(object sender, EventArgs e) {
      if (this.InvokeRequired) {
        this.Invoke(new EventHandler(_updateManager_StopUpdate), sender, e);
        return;
      }
      _updateStarted = false;
      _btnStart.Enabled = true;
      _btnViewDetails.Enabled = true;
      if (_updateManager.IsDownloadUpdateFiles)
        _btnStart.Text = "Close";
      else
        _btnStart.Text = "Start";

      if (this._updateManager.IsUpdateComplete) {
        MessageBox.Show("Please, restart the program!", "Update Complete!");
      }
    }
    #endregion

    #region private void _updateManager_StartUnZipFile(object sender, UnZipFileEventArgs e)
    private void _updateManager_StartUnZipFile(object sender, UnZipFileEventArgs e) {
      #region if (this.InvokeRequired) {...}
      if (this.InvokeRequired) {
        this.Invoke(new UnZipFileEventHandler(_updateManager_StartUnZipFile), sender, e);
        return;
      }
      #endregion

      _progressBar.Value = 0;
      this.WriteLog(string.Format("Extract '{0}' - ", e.File.FullName));
    }
    #endregion

    #region private void _updateManager_UnZipFileProgress(object sender, UnZipProgressEventArgs e)
    private void _updateManager_UnZipFileProgress(object sender, UnZipProgressEventArgs e) {
      #region if (this.InvokeRequired) {...}
      if (this.InvokeRequired) {
        this.Invoke(new UnZipProgressEventHandler(_updateManager_UnZipFileProgress), sender, e);
        return;
      }
      #endregion
      
      _progressBar.Maximum = e.Total;
      _progressBar.Value = e.Current;
    }
    #endregion

    #region private void _updateManager_StopUnZipFile(object sender, UnZipFileEventArgs e)
    private void _updateManager_StopUnZipFile(object sender, UnZipFileEventArgs e) {
      #region if (this.InvokeRequired) {...}
      if (this.InvokeRequired) {
        this.Invoke(new UnZipFileEventHandler(_updateManager_StopUnZipFile), sender, e);
        return;
      }
      #endregion
      
      _lblProcessName.Text = "";
      _progressBar.Value = 0;
      if (_updateManager.Error != UpdateManagerError.None) {
        this.WriteLog(string.Format("{0}=\"{1}\"", UpdateManagerErrorParse.ToString(_updateManager.Error), _updateManager.ErrorMessage));
      } else {
        this.WriteLog("ok");
      }
      this.WriteLog(Environment.NewLine);
    }
    #endregion

    #region private void _btnStart_Click(object sender, EventArgs e)
    private void _btnStart_Click(object sender, EventArgs e) {
      if (_updateStarted) {
        _btnStart.Enabled = false;
        _updateManager.WebReader.Abort();
      } else if (_updateManager.IsUpdateComplete)
        this.Close();
      else
        _updateManager.Update();
    }
    #endregion

  }
}