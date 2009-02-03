/**
* @version $Id: SplashForm.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.FO
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Windows.Forms;
  using System.Drawing;
  using System.ComponentModel;
  using Gordago.Core;

  class SplashForm : Form {

    private Image _img;
    private bool startIDE;
    private string _processText = "";
    private LanguageManager.PhraseGroup _phrase;

    #region public SplashForm()
    public SplashForm() {
      _img = global::Gordago.FO.Properties.Resources.ForexOptimizerSplash;
      this.ClientSize = new System.Drawing.Size(460, 180);
      this.ShowInTaskbar = false;
      this.TopMost = true;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.SetStyle(System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, true);
      this.SetStyle(System.Windows.Forms.ControlStyles.DoubleBuffer, true);
    }
    #endregion

    #region public bool StartIDE
    public bool StartIDE {
      get {
        return this.startIDE;
      }
    }
    #endregion

    #region protected override void OnPaint(PaintEventArgs e)
    protected override void OnPaint(PaintEventArgs e) {
      base.OnPaint(e);
      if (_img != null) {
        e.Graphics.DrawImage(_img, 0, 0, _img.Width, _img.Height);
      }
      using (Font font = new Font("Microsoft Sans Serif", 7F, FontStyle.Bold)) {
        using (Brush brush = new SolidBrush(Color.Black)) {
          e.Graphics.DrawString(this._processText, font, brush, 10, this.Height - 51);
        }
      }
    }
    #endregion

    #region private void SetProgressText(string text)
    private void SetProgressText(string text) {
      _processText = text;
      this.Invalidate();
    }
    #endregion

    #region protected override void OnLoad(EventArgs e)
    protected override void OnLoad(EventArgs e) {
      System.ComponentModel.BackgroundWorker worker = new BackgroundWorker();
      worker.DoWork += new DoWorkEventHandler(this.worker_DoWork);
      worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.worker_RunWorkerCompleted);
      worker.RunWorkerAsync();
      base.OnLoad(e);
    }
    #endregion
    
    #region private void worker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
    private void worker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e) {

      this.SetProgressText("Loading Properties...");
      try {
        Global.Properties = new EasyProperties();
        Global.Properties.Load(Global.Setup.AppConfigFile);
      } catch { }

      this.SetProgressText("Initialize Languages...");
      Global.Languages = new LanguageManager(Global.Setup.LanguagesDirectory, "Gordago.FO");
      Global.Languages.Select(Global.Properties.GetValue<string>("Language", "en-US"));

      _phrase = Global.Languages["SplashForm"];
      this.SetProgressText(_phrase.Phrase("LoadQuotes", "Loading quotes..."));
      Global.Quotes = new Gordago.FO.Quotes.QuotesManager();

      this.SetProgressText(_phrase.Phrase("LoadIndicators", "Loading indicators..."));
      Global.Indicators = new Gordago.FO.Instruments.IndicatorsManager();

      this.SetProgressText(_phrase.Phrase("InitCompiler", "Initialization of the compiler..."));
      Global.Projects = new Gordago.FO.Instruments.ProjectsManager();
      Global.Projects.LoadReferencedAssembly += new Gordago.FO.Instruments.ProjectsManager.LoadReferencedAssembliesEventHandler(Projects_LoadReferencedAssembly);
      Global.Projects.Load();
    }
    #endregion

    #region private void Projects_LoadReferencedAssembly(object sender, Gordago.FO.Instruments.ProjectsManager.LoadReferencedAssembliesEventArgs e)
    private void Projects_LoadReferencedAssembly(object sender, Gordago.FO.Instruments.ProjectsManager.LoadReferencedAssembliesEventArgs e) {
      this.SetProgressText(
        string.Format(_phrase.Phrase("InitCompilerParam", "Initialization of the compiler ({0})..."), e.TypeName));
    }
    #endregion

    #region private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    private void worker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e) {
      if (e.Error != null) {
        MessageBox.Show(this, e.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.startIDE = false;
      } else {
        this.startIDE = true;
      }
      base.Close();
    }
    #endregion
  }
}
