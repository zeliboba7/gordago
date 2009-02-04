/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using
using System;
using System.Collections.Generic;
using System.Text;
using Gordago.Analysis;
using System.ComponentModel;
using Gordago.Analysis.Vm;
using Gordago.Analysis.Vm.Compiler;
using Gordago.API;
using System.Diagnostics;
#endregion

namespace Gordago.Strategy {

  class BreackRecord {
    private DateTime _time;
    private bool _ext;

    public BreackRecord(DateTime time, bool ext) {
      _time = time;
      _ext = ext;
    }

    public DateTime Time {
      get { return _time; }
    }

    public bool Ext {
      get { return this._ext; }
    }
  }

  class VisualStrategy : Gordago.Analysis.Strategy {

    #region Private Property
    private VSVariantList _sellVariantsEntry, _sellVariantsExit;
    private VSVariantList _sellVariantsCreatePO, _sellVariantsDeletePO;

    private VSVariantList _buyVariantsEntry, _buyVariantsExit;
    private VSVariantList _buyVariantsCreatePO, _buyVariantsDeletePO;
    private VSRow _sellPOPrice, _buyPOPrice;

    private TradeVarInt _sellStopPoint, _sellLimitPoint, _sellTrailPoint;
    private TradeVarInt _buyStopPoint, _buyLimitPoint, _buyTrailPoint;

    private bool _useBuyTrading, _useSellTrading, _useBuyPO, _useSellPO;

    private bool _usePOSellModify, _usePOBuyModify;
    private string _soundFileName;
    #endregion

    private int _maxBarsBack = 0;
    private string[] _functionNames;

    private ISymbol _symbol;

    private Analyzer _analyzer;

    private BreackRecord[] _breakRecord;
    private int _showBreak = 0;

#if DEBUG
    private bool firstStart = false;
#endif

    #region Public Property

    #region public ISymbol Symbol
    public ISymbol Symbol {
      get { return this._symbol; }
      set { this._symbol = value; }
    }
    #endregion

    #region public VSVariantList SellVariantsOpen
    public VSVariantList SellVariantsOpen {
      get { return this._sellVariantsEntry; }
      set { this._sellVariantsEntry = value; }
    }
    #endregion

    #region public VSVariantList SellVariantsClose
    public VSVariantList SellVariantsClose {
      get { return this._sellVariantsExit; }
      set { this._sellVariantsExit = value; }
    }
    #endregion

    #region public VSVariantList SellVariantsCreatePO
    public VSVariantList SellVariantsCreatePO {
      get { return this._sellVariantsCreatePO; }
      set { this._sellVariantsCreatePO = value; }
    }
    #endregion

    #region public VSVariantList SellVariantsDeletePO
    public VSVariantList SellVariantsDeletePO {
      get { return this._sellVariantsDeletePO; }
      set { this._sellVariantsDeletePO = value; }
    }
    #endregion

    #region public VSRow SellPOPrice
    public VSRow SellPOPrice {
      get {
        if (_sellPOPrice == null)
          _sellPOPrice = new VSRow("0");
        return this._sellPOPrice;
      }
      set { this._sellPOPrice = value; }
    }
    #endregion

    #region public VSVariantList BuyVariantsEntry
    public VSVariantList BuyVariantsEntry {
      get { return this._buyVariantsEntry; }
      set { this._buyVariantsEntry = value; }
    }
    #endregion

    #region public VSVariantList BuyVariantsExit
    public VSVariantList BuyVariantsExit {
      get { return this._buyVariantsExit; }
      set { this._buyVariantsExit = value; }
    }
    #endregion

    #region public VSVariantList BuyVariantsCreatePO
    public VSVariantList BuyVariantsCreatePO {
      get { return this._buyVariantsCreatePO; }
      set { this._buyVariantsCreatePO = value; }
    }
    #endregion

    #region public VSVariantList BuyVariantsDeletePO
    public VSVariantList BuyVariantsDeletePO {
      get { return this._buyVariantsDeletePO; }
      set { this._buyVariantsDeletePO = value; }
    }
    #endregion

    #region public VSRow BuyPOPrice
    public VSRow BuyPOPrice {
      get {
        if (_buyPOPrice == null)
          _buyPOPrice = new VSRow("0");
        return this._buyPOPrice;
      }
      set { this._buyPOPrice = value; }
    }
    #endregion

    #region public bool SellPOModify
    public bool SellPOModify {
      get { return this._usePOSellModify; }
      set { _usePOSellModify = value; }
    }
    #endregion

    #region public bool BuyPOModify
    public bool BuyPOModify {
      get { return this._usePOBuyModify; }
      set { this._usePOBuyModify = value; }
    }
    #endregion

    #region public SParamInt SellStopPoint
    public TradeVarInt SellStopPoint {
      get { return this._sellStopPoint; }
      set {
        if (value == null) value = 0;
        this._sellStopPoint = value;
      }
    }
    #endregion

    #region public TradeVarInt SellLimitPoint
    public TradeVarInt SellLimitPoint {
      get {
        return this._sellLimitPoint;
      }
      set {
        if (value == null) value = 0;
        this._sellLimitPoint = value;
      }
    }
    #endregion

    #region public TradeVarInt SellTrailPoint
    public TradeVarInt SellTrailPoint {
      get { return this._sellTrailPoint; }
      set {
        if (value == null) value = 0;
        this._sellTrailPoint = value;
      }
    }
    #endregion

    #region public TradeVarInt BuyStopPoint
    public TradeVarInt BuyStopPoint {
      get { return _buyStopPoint; }
      set {
        if (value == null) value = 0;
        this._buyStopPoint = value;
      }
    }
    #endregion

    #region public TradeVarInt BuyLimitPoint
    public TradeVarInt BuyLimitPoint {
      get { return this._buyLimitPoint; }
      set {
        if (value == null) value = 0;
        this._buyLimitPoint = value;
      }
    }
    #endregion

    #region public TradeVarInt BuyTrailPoint
    public TradeVarInt BuyTrailPoint {
      get { return this._buyTrailPoint; }
      set {
        if (value == null) value = 0;
        this._buyTrailPoint = value;
      }
    }
    #endregion

    #region public string SoundFileName
    public string SoundFileName {
      get { return this._soundFileName; }
      set { this._soundFileName = value; }
    }
    #endregion
    #endregion

    #region internal void Compile(TradeVariables variables, ISymbol symbol)
    internal void Compile(TradeVariables variables, ISymbol symbol) {
      this.Symbol = symbol;


      if (_sellVariantsEntry == null)
        _sellVariantsEntry = new VSVariantList();
      if (_sellVariantsExit == null)
        _sellVariantsExit = new VSVariantList();
      if (_sellVariantsCreatePO == null)
        _sellVariantsCreatePO = new VSVariantList();
      if (_sellVariantsDeletePO == null)
        _sellVariantsDeletePO = new VSVariantList();
      if (_sellPOPrice == null)
        _sellPOPrice = new VSRow("0");

      if (_buyVariantsEntry == null)
        _buyVariantsEntry = new VSVariantList();
      if (_buyVariantsExit == null)
        _buyVariantsExit = new VSVariantList();
      if (_buyVariantsCreatePO == null)
        _buyVariantsCreatePO = new VSVariantList();
      if (_buyVariantsDeletePO == null)
        _buyVariantsDeletePO = new VSVariantList();
      if (_buyPOPrice == null)
        _buyPOPrice = new VSRow("0");


      List<IVSRow> listVariants = new List<IVSRow>();
      listVariants.Add(SellVariantsOpen);
      listVariants.Add(SellVariantsClose);
      listVariants.Add(SellVariantsCreatePO);
      listVariants.Add(SellVariantsDeletePO);
      listVariants.Add(BuyVariantsEntry);
      listVariants.Add(BuyVariantsExit);
      listVariants.Add(BuyVariantsCreatePO);
      listVariants.Add(BuyVariantsDeletePO);
      listVariants.Add(SellPOPrice);
      listVariants.Add(BuyPOPrice);

      _maxBarsBack = 0;
      List<string> list = new List<string>();

      for (int i = 0; i < listVariants.Count; i++) {
        listVariants[i].Compile(variables);
        _maxBarsBack = Math.Max(_maxBarsBack, listVariants[i].MaxBarsBack);
        foreach (string fname in listVariants[i].FunctionNames) {
          VSRow.AddFNameInList(list, fname);
        }
      }
      _functionNames = list.ToArray();


      _useSellPO = _sellVariantsCreatePO.Count > 0;
      _useBuyPO = _buyVariantsCreatePO.Count > 0;

      _useSellTrading = _sellVariantsEntry.Count > 0 || _useSellPO;
      _useBuyTrading = _buyVariantsEntry.Count > 0 || _useBuyPO;
    }
    #endregion

    #region public override bool OnLoad()
    public override bool OnLoad() {
      _breakRecord = VSBreakForm.CheckBreakPointTime();
      _showBreak = 0;
      return true; 
    }
    #endregion

    #region private bool CheckCondition(VSVariantList variants)
    private bool CheckCondition(VSVariantList variants) {
      for (int i = 0; i < variants.Count; i++) {
        bool flag = true;
        VSVariant variant = variants[i];
        for (int ii = 0; ii < variant.Count; ii++) {
          if (!variant[ii].Execute(_analyzer)) {
            flag = false;
              break;
          }
        }
        if (flag)
          return true;
      }
      return false;
    }
    #endregion

    #region private void CheckAllCondition(VSVariantList variants)
    private void CheckAllCondition(VSVariantList variants) {
      for (int i = 0; i < variants.Count; i++) {
        VSVariant variant = variants[i];
        for (int ii = 0; ii < variant.Count; ii++) 
          variant[ii].Execute(_analyzer);
      }
    }
    #endregion

    #region public override void OnConnect()
    public override void OnConnect() {
      _analyzer = this.Analyzer.GetAnalyzer(this._symbol);
    }
    #endregion

    #region public override void  OnOnlineRateChanged(IOnlineRate onlineRate)
    /// <summary>
    /// Алгоритм работы стратегии:
    /// </summary>
    /// <param name="onlineRate"></param>
    public override void OnOnlineRateChanged(IOnlineRate onlineRate) {

//#if DEBUG
//      if (!firstStart) {
//        this.firstStart = true;
//        this.UpdateAllAnalyzer();
//      }
//#endif
      if (onlineRate.Symbol.Name != this.Symbol.Name)
        return;

      if (!this.CheckTraderCondition(TradeType.Buy, onlineRate))
        this.CheckTraderCondition(TradeType.Sell, onlineRate);

      if (_showBreak<_breakRecord.Length) {
        if (_breakRecord[_showBreak].Time < onlineRate.Time) {
          VSBreakForm frm = new VSBreakForm();
          frm.TextBox =
            "Current Time: " + DateTime.Now.ToString() + "\n" + 
            "Break: " + onlineRate.Time.ToString() + "\n" + 
            this.GetAnalyzerComment(_breakRecord[_showBreak].Ext);
          frm.ShowDialog();
          _showBreak++;
        }
      }
    }
    #endregion

    #region private bool CheckTraderCondition(TradeType tradeType, IOnlineRate onlineRate)
    private bool CheckTraderCondition(TradeType tradeType, IOnlineRate onlineRate) {

      string accountId = this.Trader.Accounts[0].AccountId;

      VSVariantList conditionClose, conditionOpen, conditionCreateOrder, conditionDeleteOrder;
      int trailPoint;
      bool usePendingOrder;
      bool pendingOrderModify;
      VSRow pendingOrderRate;

      if (tradeType == TradeType.Sell) {
        if (!_useSellTrading)
          return false;
        conditionOpen = this.SellVariantsOpen;
        conditionClose = this.SellVariantsClose;
        conditionCreateOrder = this.SellVariantsCreatePO;
        conditionDeleteOrder = this.SellVariantsDeletePO;
        trailPoint = this.SellTrailPoint;
        usePendingOrder = _useSellPO;
        pendingOrderModify = SellPOModify;
        pendingOrderRate = SellPOPrice;
      } else {
        if (!_useBuyTrading)
          return false;
        conditionClose = BuyVariantsExit;
        conditionOpen = BuyVariantsEntry;
        conditionCreateOrder = this.BuyVariantsCreatePO;
        conditionDeleteOrder = this.BuyVariantsDeletePO;
        trailPoint = this.BuyTrailPoint;
        usePendingOrder = _useBuyPO;
        pendingOrderModify = BuyPOModify;
        pendingOrderRate = BuyPOPrice;
      }

      ITrade trade = GetTrade(tradeType);

      if (trade != null) {
        /* Позиция открыта */
        if (CheckCondition(conditionClose)) {
          /* нет ли условия на закрытие позиции */
          string comment = this.GetAnalyzerComment(false);
          if (this.Trader.TradeClose(trade.TradeId, trade.Lots, 0, comment).Error == null)
            return true;
        } else if (trailPoint > 0) {
          CheckTrail(trade);
        }
      } else {
        bool open = CheckCondition(conditionOpen);
        IOrder order = GetOrder(tradeType);

        if (open && order != null) {
          if (CheckCondition(conditionDeleteOrder)) {
            string comment = this.GetAnalyzerComment(false);
            if (this.Trader.OrderDelete(order.OrderId).Error == null)
              return true;
            order = null;
          }
        }

        if (open && order == null) {
          float stopRate = this.GetStopPrice(tradeType, onlineRate);
          float limitRate = this.GetLimitPrice(tradeType, onlineRate);

          string comment = this.GetAnalyzerComment(false);

          if (Trader.TradeOpen(accountId, Symbol.Name, tradeType, 1, 0, stopRate, limitRate, comment).Error == null) {
            return true;
          }
        } else if (usePendingOrder) {

          bool create = false;
          bool modify = false;
          bool condCreateOrModify = CheckCondition(conditionCreateOrder);

          if (order == null && condCreateOrModify) {
            create = true;
          } else if (order != null && CheckCondition(conditionDeleteOrder)) {
            this.Trader.OrderDelete(order.OrderId);
          } else if (order != null && pendingOrderModify && condCreateOrModify) {
            modify = true;
          }

          if (create || modify) {
            float rate = pendingOrderRate.Calculate(_analyzer);
            if (rate > 0) {
              float stopRate = this.GetStopPricePO(tradeType, rate);
              float limitRate = this.GetLimitPricePO(tradeType, rate);

              if (create) {
                string comment = this.GetAnalyzerComment(false);
                this.Trader.OrderCreate(accountId, this.Symbol.Name, tradeType, 1, rate, stopRate, limitRate, comment);
              } else
                this.Trader.OrderModify(order.OrderId, 1, rate, stopRate, limitRate);
              return true;
            }
          }
        }

      }
      return false;
    }
    #endregion

    public override void OnDestroy() { }

    #region private float GetStopPrice(TradeType tradeType, IOnlineRate onlineRate)
    private float GetStopPrice(TradeType tradeType, IOnlineRate onlineRate) {
      if (tradeType == TradeType.Sell)
        return this.SellStopPoint > 0 ? onlineRate.BuyRate + Symbol.Point * this.SellStopPoint : 0;
      else
        return this.BuyStopPoint > 0 ? onlineRate.SellRate - Symbol.Point * this.BuyStopPoint : 0;
    }
    #endregion

    #region private float GetLimitPrice(TradeType tradeType, IOnlineRate onlineRate)
    private float GetLimitPrice(TradeType tradeType, IOnlineRate onlineRate) {
      if (tradeType == TradeType.Sell)
        return this.SellLimitPoint > 0 ? onlineRate.BuyRate - Symbol.Point * this.SellLimitPoint : 0;
      else
        return this.BuyLimitPoint > 0 ? onlineRate.SellRate + Symbol.Point * this.BuyLimitPoint : 0;
    }
    #endregion

    #region private float GetStopPricePO(Trade tradeType, float rate)
    private float GetStopPricePO(TradeType tradeType, float rate) {
      if (tradeType == TradeType.Sell)
        return this.SellStopPoint > 0 ? rate + Symbol.Point * this.SellStopPoint : 0;
      else
        return this.BuyStopPoint > 0 ? rate - Symbol.Point * this.BuyStopPoint : 0;
    }
    #endregion

    #region private float GetLimitPricePO(TradeType tradeType, float rate)
    private float GetLimitPricePO(TradeType tradeType, float rate) {
      if (tradeType == TradeType.Sell)
        return this.SellLimitPoint > 0 ? rate - Symbol.Point * this.SellLimitPoint : 0;
      else
        return this.BuyLimitPoint > 0 ? rate + Symbol.Point * this.BuyLimitPoint : 0;
    }
    #endregion

    #region private ITrade GetTrade(TradeType tradeType)
    private ITrade GetTrade(TradeType tradeType) {
      for (int i = 0; i < Trader.Trades.Count; i++) {
        ITrade trade = Trader.Trades[i];
        if (trade.OnlineRate.Symbol == this.Symbol && trade.TradeType == tradeType)
          return trade;
      }
      return null;
    }
    #endregion

    #region public IOrder GetOrder(TradeType tradeType)
    public IOrder GetOrder(TradeType tradeType) {
      for (int i = 0; i < Trader.Orders.Count; i++) {
        IOrder order = Trader.Orders[i];
        if (order.OnlineRate.Symbol == this.Symbol && order.TradeType == tradeType &&
          (order.OrderType == OrderType.EntryLimit || order.OrderType == OrderType.EntryStop)) {
          return order;
        }
      }
      return null;
    }
    #endregion

    #region private void CheckTrail(ITrade trade)
    private void CheckTrail(ITrade trade) {
      if (trade.OnlineRate.Symbol != Symbol)
        return;

      float limitRate = 0;

      if (trade.LimitOrder != null)
        limitRate = trade.LimitOrder.Rate;

      if (trade.TradeType == TradeType.Sell) {

        float trail = this.SellTrailPoint * Symbol.Point;
        float newStopRate = trade.OnlineRate.BuyRate + trail;

        if (trade.OpenRate - trade.OnlineRate.BuyRate > trail &&
          (trade.StopOrder == null || trade.StopOrder.Rate > newStopRate)) {
          float currate = trade.StopOrder == null ? 0 : trade.StopOrder.Rate;

          this.Trader.TradeModify(trade.TradeId, newStopRate, limitRate);
        }
      } else {

        float trail = this.BuyTrailPoint * Symbol.Point;
        float newLimitRate = trade.OnlineRate.SellRate - trail;

        if (trade.OnlineRate.SellRate - trade.OpenRate > trail &&
          (trade.StopOrder == null || trade.StopOrder.Rate < newLimitRate)) {
          this.Trader.TradeModify(trade.TradeId, newLimitRate, limitRate);
        }
      }
    }
    #endregion

    private void UpdateAllAnalyzer() {
      this.CheckAllCondition(this.BuyVariantsCreatePO);
      this.CheckAllCondition(this.BuyVariantsDeletePO);
      this.CheckAllCondition(this.BuyVariantsEntry);
      this.CheckAllCondition(this.BuyVariantsExit);
      this.CheckAllCondition(this.SellVariantsCreatePO);
      this.CheckAllCondition(this.SellVariantsDeletePO);
      this.CheckAllCondition(this.SellVariantsOpen);
      this.CheckAllCondition(this.SellVariantsClose);
    }

    private string GetAnalyzerComment(bool ext) {
      this.UpdateAllAnalyzer();

      List<string> list = new List<string>();
      if (ext)
        list.Add(string.Format("Bid={0}", this.Symbol.Ticks.Current.Price));

      for (int i = 0; i < this._analyzer.Cache.Count; i++) {
//        BaseVector bvector = this._analyzer.Cache[i];
        string comment = _analyzer.Cache[i].GetComment(_maxBarsBack + 1);
        if (!ext) {
          if (CheckString(comment, "Close(")) {
          } else if (CheckString(comment, "Low(")) {
          } else if (CheckString(comment, "High(")) {
          } else if (CheckString(comment, "Open(")) {
          } else if (CheckString(comment, "Time(")) {
          } else if (CheckString(comment, "sub(")) {
          } else {
            //foreach (string fname in _functionNames) {
            //  if (CheckString(comment, fname)) {
            //    list.Add(comment);
            //    break;
            //  }
            //}
            list.Add(comment);
          }
        } else {
          list.Add(comment);
        }
      }

      if (ext) {
        list.Add(string.Format("Ticks={0}", this.Symbol.Ticks.Count));

        for (int i = 0; i < this.Symbol.Ticks.BarLists.Length; i++) {
          IBarList bars = this.Symbol.Ticks.BarLists[i];
          //list.Add(string.Format("Bar[{1}:{3}]={0}  {2}",
          //  bars.Count,
          //  bars.TimeFrame.Name,
          //  bars[bars.Count - 2].ToString(true), bars.RealIndex(bars.Count - 2)));
          list.Add(string.Format("Bar[{1}]={0}  {2}",
            bars.Count,
            bars.TimeFrame.Name,
            bars.Current.ToString(true)));
        }
      }
      return string.Join("\n", list.ToArray());
    }

    #region private bool CheckString(string source, string find)
    private bool CheckString(string source, string find) {
      source = source.Substring(0, find.Length).ToUpper();
      if (source == find.ToUpper())
        return true;
      return false;
    }
    #endregion
  }
}
