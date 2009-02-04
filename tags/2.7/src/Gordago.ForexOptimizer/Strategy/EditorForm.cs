/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Cursit.Utils;
using Gordago.Strategy.IO;
using Gordago.Analysis;
using Gordago.Strategy.FIndicator;
using System.Collections;
using Language;
using Cursit.Applic.AConfig;
#endregion

namespace Gordago.Strategy {
  partial class EditorForm : Form, IStrategyForm, IIndicatorBoxCollection {

    private StrategyIO _strgIO;

    private bool _issaveflag = false;
    private bool _isnewstrategy = true;
    private SEditorVariants[] _strgs;

    private bool _ischangeUser;

    private string _prevcompile = "";

    private int _groupId = 1;

    private string _filename;
    private bool _destroy;
    private bool _isopen;
    private TestReport _testReport;

    private string _strategyName;

    #region public EditorForm()
    public EditorForm() {
      InitializeComponent();
      this._tbcMain.DrawMode = TabDrawMode.OwnerDrawFixed;

      _strgs = new SEditorVariants[8];
      _strgs[0] = _editEnterBuy;
      _strgs[1] = _editExitBuy;
      _strgs[2] = _editEnterSell;
      _strgs[3] = _editExitSell;
      _strgs[4] = _editPOBuy;
      _strgs[5] = _editPOBuyDelete;
      _strgs[6] = _editPOSell;
      _strgs[7] = _editPOSellDelete;

      _editEnterSell.SetEditorForm(this);
      _editExitSell.SetEditorForm(this);
      _editEnterBuy.SetEditorForm(this);
      _editExitBuy.SetEditorForm(this);
      _editPOBuy.SetEditorForm(this);
      _editPOBuyDelete.SetEditorForm(this);
      _editPOSell.SetEditorForm(this);
      _editPOSellDelete.SetEditorForm(this);
      
      _editPOPriceSell.SetEditorForm(this);
      _editPOPriceBuy.SetEditorForm(this);

      _chkpBStop.TextPeriod =
        _chkpSStop.TextPeriod = Dictionary.GetString(10, 7, "Stop Loss");;

      _chkpBLimit.TextPeriod = 
        _chkpSLimit.TextPeriod = Dictionary.GetString(10, 8, "Take Profit");

      _chkpBTrail.TextPeriod =
        _chkpSTrail.TextPeriod = Dictionary.GetString(10, 9, "Trailing Stop");;

      this._strgIO = new StrategyIO(this);

      _chkpBStop.ValueChanged += new EventHandler(this.Value_Changed);
      _chkpBLimit.ValueChanged += new EventHandler(this.Value_Changed);
      _chkpBTrail.ValueChanged += new EventHandler(this.Value_Changed);

      _chkpSStop.ValueChanged += new EventHandler(this.Value_Changed);
      _chkpSLimit.ValueChanged += new EventHandler(this.Value_Changed);
      _chkpSTrail.ValueChanged += new EventHandler(this.Value_Changed);

      _tbpSell.Text = Dictionary.GetString(10, 2, "Sell");
      _tbpSellPO.Text = Dictionary.GetString(10, 23, "Отложенная продажа");
      _tbpBuy.Text = Dictionary.GetString(10, 3, "Buy");
      _tbpBuyPO.Text = Dictionary.GetString(10, 22, "Отложенная покупка");

      _lblSellEnter.Text = Dictionary.GetString(10, 4, "Entry");
      _lblSellExit.Text = Dictionary.GetString(10, 5, "Exit");

      _lblEnterBuy.Text = Dictionary.GetString(10, 4, "Entry");
      _lblExitBuy.Text = Dictionary.GetString(10, 5, "Exit");

      _tbpAdditional.Text = Dictionary.GetString(10, 17, "Additional");

      _gbxName.Text = Dictionary.GetString(10, 18, "Name");
      _gbxDescription.Text = Dictionary.GetString(10, 19, "Description");
      _gbxSound.Text = Dictionary.GetString(10, 20, "Sound");
      _btnplay.Text = Dictionary.GetString(10, 21, "Play");

      _lblPOBuyPrice.Text = lblPOSellPrice.Text = Dictionary.GetString(10, 24, "Цена");
      _lblPOSell.Text = _lblPOBuy.Text = Dictionary.GetString(10, 25, "Создать или править");
      _lblPOBuyDelete.Text = _lblSellPODelete.Text = Dictionary.GetString(10, 26, "Удалить");

      string[] files = System.IO.Directory.GetFiles(Application.StartupPath + "\\sound", "*.wav");
      this._cmbsound.Items.AddRange(Cursit.Utils.FileEngine.GetDisplayFiles(files));
      if(_cmbsound.Items.Count > 0)
        this._cmbsound.SelectedIndex = 0;

      this.StartPosition = FormStartPosition.Manual;
    }
    #endregion

    #region public string[] StrategyNames
    public string[] StrategyNames {
      get { return new string[] { _shortFileName }; }
    }
    #endregion

    #region Public Property

    #region public TestReport TestReport
    public TestReport TestReport {
      get {
        return _testReport;
      }
      set {
        _testReport = value;
      }
    }
    #endregion

    #region public string FileName
    public string FileName {
      get {
        return this._filename;
      }
      set {
        this._filename = value;
      }
    }
    #endregion

    #region public bool IsDestroy
    public bool IsDestroy {
      get {
        return this._destroy;
      }
      set {
        this._destroy = value;
      }
    }
    #endregion

    #region public bool IsOpen
    public bool IsOpen {
      get { return this._isopen; }
      set { this._isopen = value; }
    }
    #endregion

    #region public string CName
    public string CName {
      get { return this._txtname.Text; }
      set { this._txtname.Text = value; }
    }
    #endregion

    #region public string CDescription
    public string CDescription {
      get { return this._txtdesc.Text; }
      set { this._txtdesc.Text = value; }
    }
    #endregion

    #region public string PrefCompile
    /// <summary>
    /// Скомпилированная строка
    /// </summary>
    public string PrefCompile {
      get { return this._prevcompile; }
    }
    #endregion

    #region public SEditorVariants EditBuy
    public SEditorVariants EditBuy {
      get { return this._editEnterBuy; }
    }
    #endregion

    #region public SEditorVariants EditBuyExit
    public SEditorVariants EditBuyExit {
      get { return this._editExitBuy; }
    }
    #endregion

    #region public SEditorVariants EditSell
    public SEditorVariants EditSell {
      get { return this._editEnterSell; }
    }
    #endregion

    #region public SEditorVariants EditSellExit
    public SEditorVariants EditSellExit {
      get { return this._editExitSell; }
    }
    #endregion

    #region public SEditorVariants EditPOBuy
    public SEditorVariants EditPOBuy {
      get { return this._editPOBuy; }
    }
    #endregion

    #region public SEditorVariants EditPOBuyDelete
    public SEditorVariants EditPOBuyDelete {
      get { return this._editPOBuyDelete; }
    }
    #endregion

    #region public SEditorVariants EditPOSell
    public SEditorVariants EditPOSell {
      get { return this._editPOSell;}
    }
    #endregion

    #region public SEditorVariants EditPOSellDelete
    public SEditorVariants EditPOSellDelete {
      get { return this._editPOSellDelete; }
    }
    #endregion

    #region public POPriceCaclulator EditPOPriceBuy
    public POPriceCaclulator EditPOPriceBuy {
      get { return this._editPOPriceBuy; }
    }
    #endregion

    #region public POPriceCaclulator EditPOPriceSell
    public POPriceCaclulator EditPOPriceSell {
      get { return this._editPOPriceSell; }
    }
    #endregion

    #region public CheckPeriod CheckBuyStop
    public CheckPeriod CheckBuyStop {
      get { return this._chkpBStop; }
    }
    #endregion

    #region public CheckPeriod CheckBuyLimit
    public CheckPeriod CheckBuyLimit {
      get { return this._chkpBLimit; }
    }
    #endregion

    #region public CheckPeriod CheckBuyTrail
    public CheckPeriod CheckBuyTrail {
      get { return this._chkpBTrail; }
    }
    #endregion

    #region public CheckPeriod CheckSellStop
    public CheckPeriod CheckSellStop {
      get { return this._chkpSStop; }
    }
    #endregion

    #region public CheckPeriod CheckSellLimit
    public CheckPeriod CheckSellLimit {
      get { return this._chkpSLimit; }
    }
    #endregion

    #region public CheckPeriod CheckSellTrail
    public CheckPeriod CheckSellTrail {
      get { return this._chkpSTrail; }
    }
    #endregion

    #region public bool IsPOSellModify
    public bool IsPOSellModify {
      get { return this._chkPOSellModify.Checked; }
      set { this._chkPOSellModify.Checked = value; }
    }
    #endregion

    #region public bool IsPOBuyModify
    public bool IsPOBuyModify {
      get { return this._chkPOBuyModify.Checked; }
      set { this._chkPOBuyModify.Checked = value; }
    }
    #endregion

    #region public int GroupId
    public int GroupId {
      get { return this._groupId; }
      set { this._groupId = value; }
    }
    #endregion

    #region public bool IsSave
    public bool IsSave {
      get { return this._issaveflag; }
      set {
        _issaveflag = value;
        string[] sa = this.FileName.Split(new char[] { '\\' });
        _shortFileName = sa[sa.Length - 1];

        this.Text = Dictionary.GetString(10, 1, "Стратегия") + " - " + _shortFileName + (!_issaveflag ? "*" : "");
        if(!_issaveflag && this.TestReport != null) 
          this.TestReport = null;
        GordagoMain.MainForm.Tester.UpdateStrategyList();
      }
    }
    #endregion

    #region public bool IsChangeUser
    public bool IsChangeUser {
      get { return this._ischangeUser; }
      set {
        this._ischangeUser = value;
        if(value)
          this.IsSave = false;
      }
    }
    #endregion

    #region public bool IsNewStrategy
    public bool IsNewStrategy {
      get { return this._isnewstrategy; }
      set { _isnewstrategy = value; }
    }
    #endregion

    #region public string ShortFileName
    private string _shortFileName;
    public string ShortFileName {
      get { return this._shortFileName; }
    }
    #endregion

    #endregion

    #region public void SetSoundFileName(string filename)
    public void SetSoundFileName(string filename) {
      foreach(object obj in this._cmbsound.Items) {
        FileEngine.DisplayFile dfile = (FileEngine.DisplayFile)obj;
        if(filename == dfile.DisplayName) {
          this._cmbsound.SelectedItem = dfile;
        }
      }
    }
    #endregion

    #region public string GetSoundFileName()
    public string GetSoundFileName() {
      if(this._cmbsound.SelectedItem == null)
        return "";
      Cursit.Utils.FileEngine.DisplayFile df = this._cmbsound.SelectedItem as FileEngine.DisplayFile;
      return df.DisplayName;
    }
    #endregion

    #region private void _btnplay_Click(object sender, System.EventArgs e)
    private void _btnplay_Click(object sender, System.EventArgs e) {
      this.PlaySound();
    }
    #endregion

    #region public void PlaySound()
    public void PlaySound() {
      if(this._cmbsound.SelectedItem == null)
        return;
      try {
        Cursit.Utils.FileEngine.DisplayFile dfile = (Cursit.Utils.FileEngine.DisplayFile)this._cmbsound.SelectedItem;
        Cursit.Win32API.PlaySoundFile(dfile.FileName);
      } catch { }
    }
    #endregion

    #region private void _tbcMain_DrawItem(object sender, DrawItemEventArgs e)
    private void _tbcMain_DrawItem(object sender, DrawItemEventArgs e) {
      Brush brush = null;
      if(_tbcMain.TabPages[e.Index] == _tbpSell || _tbcMain.TabPages[e.Index] == _tbpSellPO) {
        brush = new SolidBrush(Color.FromArgb(233, 181, 234));
      } else if(_tbcMain.TabPages[e.Index] == _tbpBuy || _tbcMain.TabPages[e.Index] == _tbpBuyPO) {
        brush = new SolidBrush(Color.FromArgb(152, 224, 221));
      } else {
        brush = new SolidBrush(SystemColors.Control);
      }
      e.Graphics.FillRectangle(brush, e.Bounds);

      StringFormat stringFormat = new StringFormat();
      stringFormat.Alignment = StringAlignment.Center;
      stringFormat.LineAlignment = StringAlignment.Center;
      e.Graphics.DrawString(_tbcMain.TabPages[e.Index].Text, Font, Brushes.Black, e.Bounds, stringFormat);
    }
    #endregion

    #region private void _tbcMain_SelectedIndexChanged(object sender, EventArgs e)
    private void _tbcMain_SelectedIndexChanged(object sender, EventArgs e) {
      ExplorerPanel iibox = GordagoMain.MainForm.Explorer;
      if(iibox != null) {
        iibox.UnSelectAllRowInViewIndicators();
        iibox.ClearIndicatorProperty();
      }
      this.UnSetSelectStatusIndicBox();
    }
    #endregion

    #region private void Value_Changed(object sender, EventArgs e)
    private void Value_Changed(object sender, EventArgs e) {
      this.IsChangeUser = true;
    }
    #endregion

    #region public void SetDefaultFileName()
    public void SetDefaultFileName() {
      string defname = Dictionary.GetString(11, 1);
      string defpath = Application.StartupPath + "\\strategy";
      if(!System.IO.Directory.Exists(defpath))
        System.IO.Directory.CreateDirectory(defpath);

      string path = Config.Users["PathStrategy", defpath];
      if(!System.IO.Directory.Exists(path))
        path = defpath;
      Config.Users["PathStrategy"].SetValue(path);

      string[] files = System.IO.Directory.GetFiles(path, "*.gso");

      /* так же необxодимо просмотреть все открытые окна */
      ArrayList al = new ArrayList(files);
      MainForm fm = GordagoMain.MainForm;
      foreach(Form frm in fm.MdiChildren) {
        if(frm is EditorForm) {
          EditorForm wf = frm as EditorForm;
          if(wf != this) {
            al.Add(wf.FileName);
          }
        }
      }
      files = new string[al.Count];
      al.CopyTo(files);

      for(int i = 1; i < 10000; i++) {
        string retfn = defname + i.ToString() + ".gso";
        string cncfn = path + "\\" + retfn;
        bool isfind = false;
        foreach(string filename in files) {
          if(filename == cncfn) {
            isfind = true;
          }
        }
        if(!isfind) {
          this.FileName = cncfn;

          IsSave = false;
          this._ischangeUser = false;

          return;
        }
      }
      IsSave = false;
      this._ischangeUser = false;
    }
    #endregion

    #region public override bool LoadFromFile(string filename)
    public bool LoadFromFile(string filename) {
      this.FileName = filename;
      bool ret = this._strgIO.Load(filename);

      this.IsSave = true;
      this.IsNewStrategy = false;
      return ret;
    }
    #endregion

    #region public bool Save()
    public bool Save() {
      if(this.IsNewStrategy)
        return SaveAs();

      SaveStrategy(this.FileName);
      return true;
    }
    #endregion

    #region public bool SaveAs()
    public bool SaveAs() {
      SaveFileDialog sfdlg = new SaveFileDialog();
      string path = Config.Users["PathStrategy", Application.StartupPath + "\\strategy"];

      sfdlg.Filter = StrategyManager.FILE_FILTER;
      sfdlg.FilterIndex = 0;
      sfdlg.FileName = this.FileName;
      sfdlg.InitialDirectory = path;

      if(sfdlg.ShowDialog() != DialogResult.OK) return false;
      if(sfdlg.FileName == "") return false;

      this.FileName = sfdlg.FileName;
      path = Cursit.Utils.FileEngine.GetDirectory(sfdlg.FileName);
      Config.Users["PathStrategy"].SetValue(path);
      this.SaveStrategy(this.FileName);
      return true;
    }
    #endregion

    #region private void SaveStrategy(string fileName)
    private void SaveStrategy(string fileName) {

      this._strgIO.SaveAs(fileName);
      this.IsNewStrategy = false;
      this.IsSave = true;
      GordagoMain.MainForm.SetLastOpenStrategyFile(fileName);
    }
    #endregion

    #region private string StrategyVarSLTPTL(CheckPeriod cp, string prefix, TradeVariables tvars)
    private string StrategyVarSLTPTL(CheckPeriod cp, string prefix, TradeVariables tvars) {
      string retstr = "0";
      if(cp.Checked) {
        if(cp.NumberBegin != cp.NumberEnd) {
          string varstr = "opt" + prefix;

          int v1 = (int)cp.NumberBegin;
          int v2 = (int)cp.NumberEnd;
          int step = (int)cp.Step;
          tvars.Add(new TradeVarInt(varstr, v1, v2, step));
          retstr = "$" + varstr;
          cp.OptVar = retstr;
        } else {
          retstr = Convert.ToString(cp.NumberBegin);
          cp.OptVar = retstr;
        }
      }
      return retstr;
    }
    #endregion

    #region public CompileDllData Compile(string strategyName)
    public CompileDllData Compile(string strategyName) {
      StrategyCompile sc = new StrategyCompile(
        _editEnterBuy, _editExitBuy, 
        _editEnterSell, _editExitSell, 
        _editPOBuy, _editPOBuyDelete, _editPOPriceBuy, 
        _editPOSell, _editPOSellDelete, _editPOPriceSell);
      
      this.Refresh();

      string cmpBuyEnter = sc.BuyEnter;
      string cmpBuyExit = sc.BuyExit;
      string cmpSellEnter = sc.SellEnter;
      string cmpSellExit = sc.SellExit;

      bool isbuy = false, issell = false;
      int cn = 0;

      if((cmpBuyEnter.Length > 0 || sc.BuyCreate.Length > 0) && (cmpBuyExit.Length > 0 || _chkpBStop.Checked || _chkpBLimit.Checked || _chkpBTrail.Checked)) {
        isbuy = true;
        cn++;
      }

      if((cmpSellEnter.Length > 0 || sc.SellCreate.Length > 0)&& (cmpSellExit.Length > 0 || _chkpSStop.Checked || _chkpSLimit.Checked || _chkpSTrail.Checked)) {
        issell = true;
        cn++;
      }

      if(!issell && !isbuy) return null;

      if(!this.Save()) return null;

      VisualStrategy strategy = new VisualStrategy();

      strategy.SellVariantsOpen = sc.SellVariantsEntry;
      strategy.SellVariantsClose = sc.SellVariantsExit;
      strategy.SellVariantsCreatePO = sc.SellVariantsCreatePO;
      strategy.SellVariantsDeletePO = sc.SellVariantsDeletePO;
      strategy.SellPOPrice = sc.SellPOPrice;
      strategy.SellPOModify = this.IsPOSellModify;

      strategy.BuyVariantsEntry = sc.BuyVariantsEntry;
      strategy.BuyVariantsExit = sc.BuyVariantsExit;
      strategy.BuyVariantsCreatePO = sc.BuyVariantsCreatePO;
      strategy.BuyVariantsDeletePO = sc.BuyVariantsDeletePO;
      strategy.BuyPOPrice = sc.BuyPOPrice;
      strategy.BuyPOModify = this.IsPOBuyModify;

      strategy.SellStopPoint = CompileCheckPeriod(_chkpSStop);
      strategy.SellLimitPoint = CompileCheckPeriod(_chkpSLimit);
      strategy.SellTrailPoint = CompileCheckPeriod(_chkpSTrail);

      strategy.BuyStopPoint = CompileCheckPeriod(_chkpBStop);
      strategy.BuyLimitPoint = CompileCheckPeriod(_chkpBLimit);
      strategy.BuyTrailPoint = CompileCheckPeriod(_chkpBTrail);

      return new CompileDllData(sc.TradeVariables, strategy);
    }
    #endregion

    #region private TradeVarInt CompileCheckPeriod(CheckPeriod cp)
    private TradeVarInt CompileCheckPeriod(CheckPeriod cp) {
      TradeVarInt tv = !cp.Checked ? null : new TradeVarInt(cp.NumberBegin, cp.NumberEnd, cp.Step);
      if(tv != null) {
        cp.OptVar = "$" + tv.Name;
      } else {
        cp.OptVar = "";
      }
      return tv;
    }
    #endregion

    #region public void UnSetSelectStatusIndicBox()
    public void UnSetSelectStatusIndicBox() {
      this.SelectIndicFunctionBox(null);
    }
    #endregion

    #region public void SynchronizedParam(IndicFunctionBox ifbox)
    public void SynchronizedParam(IndicFunctionBox ifbox) {
      IndicFunctionBox[] indfboxes = this.GetIndicFunctionBoxCollection();
      foreach(IndicFunctionBox ifboxto in indfboxes) {
        if(ifboxto != ifbox &&
          ifboxto.IndicFunction.Parent.GroupId == ifbox.IndicFunction.Parent.GroupId &&
          ifboxto.IndicFunction.Parent.Indicator.Name == ifbox.IndicFunction.Parent.Indicator.Name) {

          ifboxto.SyncronizedParam(ifbox);
        }
      }
    }
    #endregion

    #region private class StrgCreater
    private class StrgCreater {
      ArrayList _alStr;
      public StrgCreater() {
        _alStr = new ArrayList();
      }
      public void AppendString(string appStr) {
        string appS = appStr.Trim();
        if(appS.Length < 1) return;
        _alStr.Add(appStr);
      }

      public string GetString() {
        int cnt = _alStr.Count;
        if(cnt < 1) return "";
        string retstr = "";
        for(int i = 0; i < cnt; i++) {
          retstr += (string)_alStr[i];
        }
        return retstr;
      }
      public string GetString(string separator) {
        int cnt = _alStr.Count;
        if(cnt < 1) return "";

        string[] astr = new string[cnt];
        for(int i = 0; i < cnt; i++) {
          astr[i] = (string)_alStr[i];
        }
        return string.Join(separator, astr);
      }
    }

    #endregion

    #region public bool ShowMessageForSave() - Закрытие окна с проверкой на сохранение
    /// <summary>
    /// Закрытие окна с проверкой на сохранение
    /// </summary>
    /// <returns></returns>
    public bool ShowMessageForSave() {
      if(this.IsSave) return true;
      if(!this.IsChangeUser) return true;

      DialogResult dr = MessageBox.Show(Dictionary.GetString(10, 16, "Сохранить изменения в стратегии") + " " + this.ShortFileName + "?", GordagoMain.MessageCaption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
      if(dr == DialogResult.Cancel) return false;
      if(dr == DialogResult.No) {
        this.IsSave = true;
        return true;
      }

      this.Save();
      if(!this.IsSave) return false;

      return true;
    }
    #endregion

    #region protected override void WndProc(ref Message m)
    protected override void WndProc(ref Message m) {
      if(m.Msg == Cursit.WMsg.WM_CLOSE && !this.ShowMessageForSave()) {
        return;
      }
      base.WndProc(ref m);
    }
    #endregion

    #region internal EditorForm CloneStrategyToOptimized()
    /// <summary>
    /// Клонирование этой оптимизированной стратегии в стратегию без переборов параметров
    /// </summary>
    internal static EditorForm CloneStrategyToOptimized(EditorForm seform, TestReport report) {
      if(report == null) return null;

      EditorForm eform = new EditorForm();
      eform.SetDefaultFileName();
      CreateCloneVariant(seform.EditSell, eform.EditSell, report.Variables);
      CreateCloneVariant(seform.EditSellExit, eform.EditSellExit, report.Variables);
      CreateCloneVariant(seform.EditPOSell, eform.EditPOSell, report.Variables);
      CreateCloneVariant(seform.EditPOSellDelete, eform.EditPOSellDelete, report.Variables);

      CreateCloneVariant(seform.EditBuy, eform.EditBuy, report.Variables);
      CreateCloneVariant(seform.EditBuyExit, eform.EditBuyExit, report.Variables);
      CreateCloneVariant(seform.EditPOBuy, eform.EditPOBuy, report.Variables);
      CreateCloneVariant(seform.EditPOBuyDelete, eform.EditPOBuyDelete, report.Variables);

      CloneSlTp(seform.CheckSellStop, eform.CheckSellStop, report.Variables);
      CloneSlTp(seform.CheckSellLimit, eform.CheckSellLimit, report.Variables);
      CloneSlTp(seform.CheckSellTrail, eform.CheckSellTrail, report.Variables);

      CloneSlTp(seform.CheckBuyStop, eform.CheckBuyStop, report.Variables);
      CloneSlTp(seform.CheckBuyLimit, eform.CheckBuyLimit, report.Variables);
      CloneSlTp(seform.CheckBuyTrail, eform.CheckBuyTrail, report.Variables);

      return eform;
    }
    #endregion

    #region private static void CloneSlTp(CheckPeriod cpFrom, CheckPeriod cpTo, TradeVariables tvars)
    private static void CloneSlTp(CheckPeriod cpFrom, CheckPeriod cpTo, TradeVariables tvars) {
      cpTo.Checked = cpFrom.Checked;
      cpTo.NumberBegin = cpTo.NumberEnd = cpFrom.NumberBegin;
      if(cpFrom.Checked && cpFrom.OptVar != null) {
        if(cpFrom.OptVar.IndexOf("$") > -1) {
          string var = cpFrom.OptVar.Substring(1, cpFrom.OptVar.Length - 1);
          for(int i = 0; i < tvars.Count; i++) {
            if(tvars[i].Name == var) {
              cpTo.NumberBegin = cpTo.NumberEnd = (tvars[i] as TradeVarInt);
              break;
            }
          }
        }
      }

    }
    #endregion

    #region private static void CreateCloneVariant(SEditorVariants variantsFrom, SEditorVariants variantsTo, TradeVariables vars)
    private static void CreateCloneVariant(SEditorVariants variantsFrom, SEditorVariants variantsTo, TradeVariables vars) {
      int curvrnts = 0;
      foreach(SEditorTable tblFrom in variantsFrom.SETables) {
        if(curvrnts >= variantsTo.SETables.Length)
          variantsTo.CraeteNewVariant();

        SEditorTable tblTo = variantsTo.SETables[curvrnts];

        int currow = 0;
        foreach(SEditorRow rowFrom in tblFrom.SERows) {
          if(currow >= tblTo.SERows.Length)
            tblTo.CreateNewRow();
          SEditorRow rowTo = tblTo.SERows[currow];

          foreach(TextBoxObjElement tbelFrom in rowFrom.TextBoxObject.Elements) {
            if(tbelFrom is TextBoxObjElementChar) {
              rowTo.TextBoxObject.Add(tbelFrom);
              rowTo.TextBoxObject.SetCaretPositionToEnd();
            } else if(tbelFrom is TextBoxObjElementCtrl) {
              IndicFunction indf = ((tbelFrom as TextBoxObjElementCtrl).Element as IndicFunctionBox).IndicFunction;
              IndicFunction indfnew = tblTo.AddIndicFunction(rowTo.TextBoxObject, indf.GetOptimIndicFunction(vars)).IndicFunction;
              indfnew.Parent.GroupId = indf.Parent.GroupId;
            }
          }
          rowTo.Status = rowFrom.Status;
          rowTo.TimeFrameId = rowFrom.TimeFrameId;
          currow++;
        }
        curvrnts++;
      }
    }
    #endregion

    #region public void SetTestStatus(bool isStart)
    public void SetTestStatus(bool isStart) {
      _editEnterBuy.Enabled = 
        _editEnterSell.Enabled = 
        _editExitBuy.Enabled = 
        _editExitSell.Enabled = 
        _editPOBuy.Enabled = 
        _editPOPriceBuy.Enabled = 
        _editPOPriceSell.Enabled = 
        _editPOSell.Enabled = 
        _chkpBLimit.Enabled =
        _chkpBStop.Enabled =
        _chkpBTrail.Enabled =
        _chkpSLimit.Enabled =
        _chkpSStop.Enabled =
        _chkpSTrail.Enabled =
        _txtname.Enabled = 
        _txtdesc.Enabled = 
        _cmbsound.Enabled = 
        _btnplay.Enabled = !isStart;
    }
    #endregion

    #region private void _chkPOSellModify_CheckedChanged(object sender, EventArgs e)
    private void _chkPOSellModify_CheckedChanged(object sender, EventArgs e) {
      if (_isopen)
        this.IsChangeUser = true;
    }
    #endregion

    #region private void _chkPOBuyModify_CheckedChanged(object sender, EventArgs e)
    private void _chkPOBuyModify_CheckedChanged(object sender, EventArgs e) {
      if(_isopen)
        this.IsChangeUser = true;
    }
    #endregion

    #region protected override void OnResize(EventArgs e)
    protected override void OnResize(EventArgs e) {
      base.OnResize(e);
     // this.UpdateSLTLayout();
    }
    #endregion

    #region private void UpdateSLTLayout()
    private void UpdateSLTLayout() {
      this.SuspendLayout();
      this._tbcMain.SuspendLayout();
      _tbpSell.SuspendLayout();

      int dw = _tbpSell.Width / 3;
      int h = 65, dh = 4;

      int y = _tbpSell.Height - h;

      _chkpSStop.Location = new Point(0, y);
      _chkpSStop.Size = new Size(dw, h - dh);

      _chkpSLimit.Location = new Point(dw, y);
      _chkpSLimit.Size = new Size(dw, h - dh);

      _chkpSTrail.Location = new Point(dw * 2, y);
      _chkpSTrail.Size = new Size(dw, h - dh);

      _tbpSell.ResumeLayout(true);
      this.ResumeLayout(true);
      this._tbcMain.ResumeLayout(true);
    }
    #endregion

    #region private IndicFunctionBox[] GetIndicFunctionBoxCollection(TextBoxObject tbo)
    private IndicFunctionBox[] GetIndicFunctionBoxCollection(TextBoxObject tbo) {
      List<IndicFunctionBox> list = new List<IndicFunctionBox>();
      foreach (TextBoxObjElement el in tbo.Elements) {
        if (el is TextBoxObjElementCtrl) {
          IndicFunctionBox ifbox = (el as TextBoxObjElementCtrl).Element as IndicFunctionBox;
          list.Add(ifbox);
        }
      }
      return list.ToArray();
    }
    #endregion

    #region public IndicFunctionBox[] GetIndicFunctionBoxCollection()
    public IndicFunctionBox[] GetIndicFunctionBoxCollection() {
      List<IndicFunctionBox> list = new List<IndicFunctionBox>();
      foreach (SEditorVariants strg in this._strgs) {
        foreach (SEditorTable tbl in strg.SETables) {
          foreach (SEditorRow row in tbl.SERows) {
            list.AddRange(GetIndicFunctionBoxCollection(row.TextBoxObject));
          }
        }
      }
      list.AddRange(this.GetIndicFunctionBoxCollection(_editPOPriceSell.TextBoxObject));
      list.AddRange(this.GetIndicFunctionBoxCollection(_editPOPriceBuy.TextBoxObject));
      
      return list.ToArray();
    }
    #endregion

    #region public void SelectIndicFunctionBox(IndicFunctionBox indfuncbox)
    public void SelectIndicFunctionBox(IndicFunctionBox indfuncbox) {
      IndicFunctionBox[] indfboxes = this.GetIndicFunctionBoxCollection();
      IndicatorGUI indic = null;
      if (indfuncbox != null)
        indic = indfuncbox.IndicFunction.Parent;

      foreach (IndicFunctionBox indfbox in indfboxes) {
        IndicatorGUI ind = indfbox.IndicFunction.Parent;
        if (indfuncbox == indfbox) {
          indfbox.IsSelect = true;
          indfbox.IsViewProperty = true;
        } else {
          if (indfuncbox != null && indic.GroupId == ind.GroupId) {
            indfbox.IsSelect = true;
            indfbox.IsViewProperty = false;
          } else {
            indfbox.IsSelect = false;
            indfbox.IsViewProperty = false;
          }
        }
        indfbox.Invalidate();
      }
    #endregion
    }

  }
}