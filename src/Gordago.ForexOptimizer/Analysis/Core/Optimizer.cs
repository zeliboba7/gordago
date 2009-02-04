/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Gordago.Strategy.IO;
using Gordago.Strategy;
using System.Windows.Forms;
using System.IO;
using Gordago.API.VirtualForex;
using Gordago.API;
#endregion

namespace Gordago.Analysis {

  public delegate void OptimizerHandler();
  public delegate void OptimizerCallbackHandler(int @value, int bar);

  #region class VirtualSymbol : ISymbol
  class VirtualSymbol : ISymbol {

    private string _name;
    private float _point;
    private int _decimalDigits;
    private ITickList _ticks;

    public VirtualSymbol(ISymbol symbol) {
      _name = symbol.Name;
      _point = symbol.Point;
      _decimalDigits = symbol.DecimalDigits;
      _ticks = symbol.Ticks;
    }

    #region public string Name
    public string Name {
      get { return _name; }
    }
    #endregion

    #region public float Point
    public float Point {
      get { return _point; }
    }
    #endregion

    #region public int DecimalDigits
    public int DecimalDigits {
      get { return _decimalDigits; }
    }
    #endregion

    #region public ITickList Ticks
    public ITickList Ticks {
      get { return _ticks; }
      set { _ticks = value; }
    }
    #endregion
  }
  #endregion

  #region class VirtualSymbolList : ISymbolList
  class VirtualSymbolList : ISymbolList {

    private List<ISymbol> _symbols;

    public VirtualSymbolList() {
      _symbols = new List<ISymbol>();
    }

    #region public ISymbol this[int index]
    public ISymbol this[int index] {
      get { return _symbols[index]; }
    }
    #endregion

    #region public int Count
    public int Count {
      get { return _symbols.Count; }
    }
    #endregion

    #region public ISymbol GetSymbol(string symbolname)
    public ISymbol GetSymbol(string symbolname) {
      for (int i = 0; i < _symbols.Count; i++) {
        if (_symbols[i].Name == symbolname)
          return _symbols[i];
      }
      return null;
    }
    #endregion

    #region public ISymbol Add(string name, int decimalDigits)
    public ISymbol Add(string name, int decimalDigits) {
      return null;
    }
    #endregion

    #region public ISymbol Add(ISymbol symbol)
    public ISymbol Add(ISymbol symbol) {
      _symbols.Add(symbol);
      return symbol;
    }
    #endregion
  }
  #endregion

  class Optimizer {
    
    public event OptimizerCallbackHandler ProgressChangedEvent;
    public event OptimizerHandler ProcessStatusChangedEvent;

    private ISymbol _virtSymbol, _realSymbol;
    private Strategy _strategy;
    private long _periodfrom, _periodto;
    private OptimizerProcessStatus _processstatus;
    private bool _pause, _stop;
    private TradeVariables _variables;

    private TestReport _testReport;
    private float _balance, _fee;
    private int _marginCall;

    private VirtualSymbolList _symbols;
    private VSSettingsPanel _settings;

    public Optimizer( ISymbol symbol, Strategy strategy, TradeVariables variables,
            long periodfrom, long periodto, float balance, int marginCall, float fee, VSSettingsPanel settings) {
      
      _settings = settings;

      _symbols = new VirtualSymbolList();
      _realSymbol = symbol;
      _virtSymbol = new VirtualSymbol(symbol);
      _symbols.Add(_virtSymbol);

      _balance = balance;
      _marginCall = marginCall;
      _fee = fee;

      if(variables == null)
        variables = new TradeVariables();

      _variables = variables;
      _processstatus = OptimizerProcessStatus.Stopping;
      _strategy = strategy;
      _periodfrom = periodfrom;
      _periodto = periodto;
    }

    #region public Correlation Correlation
    public TestReport Correlation {
      get { return this._testReport; }
    }
    #endregion

    #region public OptimizerProcessStatus ProcessStatus
    public OptimizerProcessStatus ProcessStatus {
      get { return this._processstatus; }
    }
    #endregion

    #region private void SetProccessStatus(OptimizerProcessStatus status)
    private void SetProccessStatus(OptimizerProcessStatus status) {
      _processstatus = status;
      OnProcessStatusChanged();
    }
    #endregion

    #region public void Pause()
    public void Pause() {
      _pause = true;
    }
    #endregion

    #region public void Resume()
    public void Resume() {
      _pause = false;
    }
    #endregion

    #region public void Stop()
    public void Stop() {
      _stop = true;
    }
    #endregion

    #region protected virtual void OnProgressChanged(int val, int bar)
    protected virtual void OnProgressChanged(int val, int bar) {
      if(ProgressChangedEvent != null)
        ProgressChangedEvent(val, bar);
    }
    #endregion

    #region protected virtual void OnProcessStatusChanged()
    protected virtual void OnProcessStatusChanged() {
      if(this.ProcessStatusChangedEvent != null)
        this.ProcessStatusChangedEvent();
    }
    #endregion

    #region public void Start()
    public void Start() {
      SetProccessStatus(OptimizerProcessStatus.Starting);
      OnProgressChanged(0, 0);
      try {
        Optimize();
      } catch { }

      OnProgressChanged(0, 0);
      SetProccessStatus(OptimizerProcessStatus.Stopping);
    }
    #endregion

    private void Optimize() {

      #region Init History
      int beginindex = 0, endindex = _realSymbol.Ticks.Count-1;
      
#if DEMO
      if(_strategy is VisualStrategy) {
        if(_periodfrom <= 0)
          _periodfrom = _realSymbol.Ticks.TimeFrom.Ticks;

        if(_periodto <= 0)
          _periodto = _realSymbol.Ticks.TimeTo.Ticks;

        DateTime dtmto = new DateTime(_periodto);
        DateTime dtmfrom = dtmto.AddDays(-14);

        _periodfrom = Math.Max(dtmfrom.Ticks, _periodfrom);
      }
#endif

      ITickManager tmanager = this._realSymbol.Ticks as ITickManager;

      if(_periodfrom > 0 || _periodto > 0) {
        if (!tmanager.IsDataCaching) {
          TickManagerEventHandler handler = new TickManagerEventHandler(tmanager_DataCachingChanged);
          tmanager.DataCachingChanged += handler;
          tmanager.DataCachingMethod();
          tmanager.DataCachingChanged -= handler;
        }
      }

      if(_periodfrom > 0)
        beginindex = tmanager.GetPositionFromMap(new DateTime(_periodfrom));

      if(_periodto > 0)
        endindex = tmanager.GetPositionFromMap(new DateTime(_periodto).AddDays(1));

      beginindex = Math.Max(beginindex, 0);
      endindex = Math.Min(endindex, _realSymbol.Ticks.Count-1);

      _periodfrom = this._realSymbol.Ticks[beginindex].Time;
      _periodto = this._realSymbol.Ticks[endindex].Time;
      #endregion

       _variables.Add(_strategy);

      if(_variables.Count == 0) 
        _variables.Add(new TradeVarInt(0));

      int valueCount = Math.Max(1, _variables.GetCount());

      int curentIndexVariable = 0;

      _testReport = new TestReport(new DateTime( _periodfrom), new DateTime(_periodto), _settings.UseTimeFrame);
      bool abortStrategyTest = false;

      int secondStep = 1;
      if (_settings.UseTimeFrame != null)
        secondStep = _settings.UseTimeFrame.Second;

      while (_variables.MoveNext() || abortStrategyTest) {
        if (GordagoMain.IsCloseProgram)
          return;

        int bar = 0;

        //VirtualBroker.StartupSettings stt = new VirtualBroker.StartupSettings();
        //stt.FromTime = _periodfrom;
        //stt.StartTime = new DateTime(_periodfrom);
        //stt.IsTester = true;
        //stt.AccountSettings = _settings;
        //VirtualBroker.StartSettings = stt;

        VirtualBroker broker = new VirtualBroker(_symbols,
         _settings, new DateTime( _periodfrom), _periodfrom, true, true);
        AnalyzerManager analyzerManager = new AnalyzerManager(GordagoMain.IndicatorManager, _symbols, typeof(VirtualAnalyzer));
        _strategy.SetEngine((ITrader)broker, analyzerManager, true);

        if (_strategy is VisualStrategy) 
          (_strategy as VisualStrategy).Compile(_variables, _virtSymbol);
        
        broker.Logon("", "", null, true);

        #region _strategy.OnLoad()
        try {
          if (!_strategy.OnLoad()) {
            //            tserver.Journal.Add(DateTime.Now, _strategy.GetType().Name + ": strategy cannot be loaded");
            return;
          } else {
            //            tserver.Journal.Add(DateTime.Now, _strategy.GetType().Name + ": loaded successfully");
          }
        } catch {
          System.Windows.Forms.MessageBox.Show(_strategy.GetType().FullName + ".OnLoad() - Error");
          return;
        }
        #endregion

        broker.SetStrategy(_strategy);
        _strategy.OnConnect();

        int countMinute = Convert.ToInt32( (_periodto - _periodfrom) / 600000000L);
        DateTime timeTest = DateTime.Now;

        while (broker.Time.Ticks < _periodto) {

          if (GordagoMain.IsCloseProgram)
            return;

          int curMinute = Convert.ToInt32((broker.Time.Ticks-_periodfrom) / 600000000L);

          int temp = curMinute * 100 / countMinute;
          if (bar != temp) {
            bar = temp;
            OnProgressChanged(curentIndexVariable * 100 / valueCount, bar);
          }

          #region if (_pause){...}
          if (_pause) {
            this._processstatus = OptimizerProcessStatus.Pause;
            this.OnProcessStatusChanged();
            while (_pause && !_stop) {
              Thread.Sleep(1);
            }
            if (!_pause) {
              this._processstatus = OptimizerProcessStatus.Starting;
              this.OnProcessStatusChanged();
            }
          }
          if (_stop) {
            abortStrategyTest = true;
            break;
          }
          #endregion

          try {
            /* произошла ошибка в стратегии, выход */
            broker.MoveNextSecond(secondStep);
          } catch (Exception e) {
            MessageBox.Show(_strategy.GetType().FullName + ".OnExecute() - Error\nDetails: " + e.Message);
            abortStrategyTest = true;
          }
        }
        curentIndexVariable++;
        _testReport.SetVariables(_variables, broker,_realSymbol.Name, DateTime.Now.Ticks-timeTest.Ticks);
        try {
          _strategy.OnDisconnect();
          _strategy.OnDestroy();
        } catch {
          MessageBox.Show(_strategy.GetType().FullName + ".OnDestroy() - Error");
        }
        broker.Logoff();
        if (abortStrategyTest)
          break;
      }
    }

    void tmanager_DataCachingChanged(object sender, TickManagerEventArgs tme) {
      throw new Exception("The method or operation is not implemented.");
    }

    #region private void TickManager_DataCachingProccess(int current, int total)
    private void TickManager_DataCachingProccess(int current, int total) {
      this.OnProgressChanged(0, (current) * 100 / (total));
    }
    #endregion

    #region public static void BreakPoint(string breakText)
    public static void BreakPoint(string breakText) {
      string fn = Application.StartupPath + "\\debug.log";
      if (!System.IO.File.Exists(fn)) return;
      //TextWriter tw = System.IO.File.OpenWrite(fn);
      //tw.WriteLine(breakText);
      MessageBox.Show(breakText);
    }
    #endregion
  }

  #region public enum OptimizerProcessStatus
  public enum OptimizerProcessStatus {
    Starting,
    Stopping,
    Pause
  }
  #endregion

}
