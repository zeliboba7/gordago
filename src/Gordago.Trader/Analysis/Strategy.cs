/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using System.IO;
using Gordago.API;
#endregion

namespace Gordago.Analysis {

  public abstract class Strategy {

    private ITrader _trader;
    private AnalyzerManager _analyzerManager;
    private bool _isTester;

    public void SetEngine(ITrader trader, AnalyzerManager analyzerManager, bool isTester) {
      _trader = trader;
      _analyzerManager = analyzerManager;
      _isTester = isTester;
    }

    #region public ITrader Trader
    [Browsable(false)]
    [XmlIgnore]
    public ITrader Trader {
      get { return this._trader; }
    }
    #endregion

    #region protected AnalyzerManager Analyzer
    protected AnalyzerManager Analyzer {
      get { return this._analyzerManager; }
    }
    #endregion

    #region public abstract bool OnLoad()
    /// <summary>
    /// Инициализация стратегии
    /// </summary>
    /// <returns>Истина - стратегия инициализированна и готова работать.
    /// Ложь - стратегия не инициализированна.</returns>
    public abstract bool OnLoad();
    #endregion
    #region public abstract void OnDestroy()
    public abstract void OnDestroy();
    #endregion

    public virtual void OnConnect() { }
    public abstract void OnOnlineRateChanged(IOnlineRate onlineRate);
    public virtual void OnDisconnect() { }

    #region public Vector Function(ISymbol symbol, string name, params object[] parameter)
    public IVector Function(ISymbol symbol, string name, params object[] parameters) {
      return _analyzerManager.Compute(symbol, name, parameters);
    }
    #endregion

    #region public Vector Function(string symbolName, string name, params object[] parameters)
    public IVector Function(string symbolName, string name, params object[] parameters) {
      IOnlineRate onlineRate = this.Trader.OnlineRates.GetOnlineRate(symbolName);
      if (onlineRate == null)
        return null;
      return this.Function(onlineRate.Symbol, name, parameters);
    }
    #endregion

    #region public void Comment(string text)
    public void Comment(string text) {
      throw (new Exception("public void Comment(string text)"));
      //_trader.Comment(text);
    }
    #endregion

    #region public void Comment(string format, params object[] args)
    public void Comment(string format, params object[] args) {
      string txt = format;

      try {
        txt = string.Format(format, args);
      } catch (Exception e){
        txt += " " + e.Message;
      }
      throw (new Exception("public void Comment(string text)"));
      //_trader.Comment(txt);
    }
    #endregion

    #region private enum SoundFlags:int
    [Flags]
    private enum SoundFlags:int {
      SND_SYNC = 0x0000,  // play synchronously (default) 
      SND_ASYNC = 0x0001,  // play asynchronously 
      SND_NODEFAULT = 0x0002,  // silence (!default) if sound not found 
      SND_MEMORY = 0x0004,  // pszSound points to a memory file
      SND_LOOP = 0x0008,  // loop the sound until next sndPlaySound 
      SND_NOSTOP = 0x0010,  // don't stop any currently playing sound 
      SND_NOWAIT = 0x00002000, // don't wait if the driver is busy 
      SND_ALIAS = 0x00010000, // name is a registry alias 
      SND_ALIAS_ID = 0x00110000, // alias is a predefined ID
      SND_FILENAME = 0x00020000, // name is file name 
      SND_RESOURCE = 0x00040004  // name is resource name or atom 
    }
    #endregion

    #region static extern bool PlaySound(string pszSound, IntPtr hMod, SoundFlags sf)
    [DllImport("winmm.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
    static extern bool PlaySound(string pszSound, IntPtr hMod, SoundFlags sf);
    #endregion

    #region public bool PlaySoundFile(string filename)
    public bool PlaySoundFile(string filename) {
      if(_isTester) return false;

      if(!System.IO.File.Exists(filename))
        return false;

      try {
        if(!PlaySound(filename, IntPtr.Zero,
          SoundFlags.SND_FILENAME | SoundFlags.SND_ASYNC))
          return false;
      } catch {
        return false;
      }
      return true;
    }
    #endregion
  }

  #region public interface ITradeVar
  public interface ITradeVar:ICloneable {
    string Name { get;}
    ITradeVarEnumerator GetTradeVarEnumerator();
  }
  #endregion

  #region public interface ITradeVarEnumerator
  public interface ITradeVarEnumerator{
    /// <summary>
    /// Кол-во перебераемых параметров
    /// </summary>
    int Count { get;}
    /// <summary>
    /// Порядковый номер элемента 
    /// </summary>
    int CurrentIndex { get;}
    /// <summary>
    /// Переключает на следующий параметр
    /// </summary>
    /// <returns>true - есть еще параметры для перебора, 
    /// false - это был последний перебираемый параметр</returns>
    bool MoveNext();
    /// <summary>
    /// Перемещение на первый указатель
    /// </summary>
    void MoveFirst();

    /// <summary>
    /// Смещение кол-во переборов на одно значение вперед
    /// </summary>
    void DownCount();

    void Reset();
  }
  #endregion

  #region public class TradeVarInt:ITradeVar
  public class TradeVarInt:ITradeVar{

    private int[] _values ;
    private TradeVarEnumerator _enum;
    private string _name = "";

    public TradeVarInt(int[] values):this("", values) { }

    #region public TradeVarInt(string name, int[] values)
    public TradeVarInt(string name, int[] values) {
      _name = name;
      if(values.Length == 0)
        throw new Exception("The lenght of Values can not be 0");
      _values = values;
      _enum = new TradeVarEnumerator(this);
    }
    #endregion

    public TradeVarInt(int minVal, int maxVal, int step):this("", minVal, maxVal, step) { }

    #region public TradeVarInt(string name, int minVal, int maxVal, int step)
    public TradeVarInt(string name, int minVal, int maxVal, int step) {
      this._name = name;
      if(minVal > maxVal) {
        step = Math.Min(-1, step);
      } else if(minVal <= maxVal)
        step = Math.Max(1, step);
      
      List<int> values = new List<int>();
      for(int i = minVal; i <= maxVal; i += step) {
        values.Add(i);
      }
      _values = values.ToArray();
      _enum = new TradeVarEnumerator(this);
    }
    #endregion

    #region public string Name
    public string Name {
      get {
        if(_name == "")
          _name = Guid.NewGuid().ToString();
        return _name; 
      }
    }
    #endregion

    public TradeVarInt(int value) : this(value, value, 0) { }
    public TradeVarInt(string name, int value) : this(name, value, value, 0) { }

    #region private int Value
    private int Value {
      get { return _values[_enum.CurrentIndex];}
      set { this._values[_enum.CurrentIndex] = value; }
    }
    #endregion

    #region public static implicit operator TradeVarInt(int value)
    public static implicit operator TradeVarInt(int value) {
      return new TradeVarInt(value);
    }
    #endregion

    #region public static implicit operator int(TradeVarInt spint)
    public static implicit operator int(TradeVarInt spint) {
      return spint.Value;
    }
    #endregion

    #region public static implicit operator TradeVarInt(int[] values)
    public static implicit operator TradeVarInt(int[] values) {
      return new TradeVarInt(values);
    }
    #endregion

    #region public ISParamEnumerator GetEnumerator()
    public ITradeVarEnumerator GetTradeVarEnumerator() {
      return _enum;
    }
    #endregion

    #region class TradeVarEnumerator:ITradeVarEnumerator
    class TradeVarEnumerator:ITradeVarEnumerator {
      private TradeVarInt _sParamInt;

      private int _cindex;
      private int _downcount;
      
      public TradeVarEnumerator(TradeVarInt sParamInt) {
        _sParamInt = sParamInt;
      }

      #region public int CurrentIndex
      public int CurrentIndex {
        get { return _cindex; }
      }
      #endregion

      #region public int Count
      public int Count {
        get { return _sParamInt._values.Length - _downcount; }
      }
      #endregion

      #region public bool MoveNext()
      public bool MoveNext() {
        _cindex++;
        return _cindex < this.Count;
      }
      #endregion

      #region public void MoveFirst()
      public void MoveFirst() {
        _cindex = _downcount;
      }
      #endregion

      #region public void DownCount()
      public void DownCount() {
        _downcount++;
      }
      #endregion

      #region public void Reset()
      public void Reset() {
        _cindex = 0;
        _downcount = 0;
      }
      #endregion

      internal void SetFromClone(ITradeVarEnumerator tvenum) {
        this._cindex = tvenum.CurrentIndex;
      }
    }
    #endregion

    #region public override string ToString()
    public override string ToString() {
      return Value.ToString();
    }
    #endregion

    #region public object Clone()
    public object Clone() {
      TradeVarInt var = new TradeVarInt(this.Name, _values);
      var._enum.SetFromClone(this._enum);
      return var;
    }
    #endregion
  }
  #endregion
}
