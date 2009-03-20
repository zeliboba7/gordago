using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;

using Gordago.Analysis;
using Gordago.API;
using Gordago;

namespace TestStrategy {
  public class TestStrategy: Strategy {

    #region Private property
    private bool _useBuyTrading = true, _useSellTrading = true;

    private bool _sellUseStop = true, _sellUseLimit = true, _sellUseTrail = true;
    private bool _buyUseStop = false, _buyUseLimit = false, _buyUseTrail = false;

    private int _sellStopPoint = 20;
    private int _sellLimitPoint = 30, _sellTrailPoint = 25;
    private int _buyStopPoint = 20, _buyLimitPoint = 30, _buyTrailPoint = 15;
    #endregion

    private string _symbolName = "EURUSD";

    #region Public Property
    [Category("Sell"), DisplayName("Use Sell Trading"), DefaultValue(true)]
    public bool UseSellTrading {
      get { return this._useSellTrading; }
      set { this._useSellTrading = value; }
    }

    [Category("Sell"), DisplayName("Stop Enabled"), DefaultValue(false)]
    public bool SellUseStop {
      get { return this._sellUseStop; }
      set { this._sellUseStop = value; }
    }

    [Category("Sell"), DisplayName("Stop Point"), DefaultValue(20)]
    public int SellStopPoint {
      get { return this._sellStopPoint; }
      set { this._sellStopPoint = value; }
    }

    [Category("Sell"), DisplayName("Limit Enabled"), DefaultValue(false)]
    public bool SellUseLimit {
      get { return this._sellUseLimit; }
      set { this._sellUseLimit = value; }
    }

    [Category("Sell"), DisplayName("Limit Point"), DefaultValue(30)]
    public int SellLimitPoint {
      get { return this._sellLimitPoint; }
      set { this._sellLimitPoint = value; }
    }

    [Category("Sell"), DisplayName("Trail Enabled"), DefaultValue(false)]
    public bool SellUseTrail {
      get { return this._sellUseTrail; }
      set { this._sellUseTrail = value; }
    }

    [Category("Sell"), DisplayName("Trail Point"), DefaultValue(15)]
    public int SellTrailPoint {
      get { return this._sellTrailPoint; }
      set { this._sellTrailPoint = value; }
    }

    [Category("Buy"), DisplayName("Use Buy Trading"), DefaultValue(true)]
    public bool UseBuyTrading {
      get { return this._useBuyTrading; }
      set { this._useBuyTrading = value; }
    }

    [Category("Buy"),  DisplayName("Stop Enabled"), DefaultValue(false)]
    public bool BuyUseStop {
      get { return this._buyUseStop; }
      set { _buyUseStop = value; }
    }

    [Category("Buy"), DisplayName("Stop Point"), DefaultValue(20)]
    public int BuyStopPoint {
      get { return _buyStopPoint; }
      set { this._buyStopPoint = value; }
    }

    [Category("Buy"), DisplayName("Limit Enabled"), DefaultValue(false)]
    public bool BuyUseLimit {
      get { return this._buyUseLimit; }
      set { this._buyUseLimit = value; }
    }

    [Category("Buy"), DisplayName("Limit Point"), DefaultValue(30)]
    public int BuyLimitPoint {
      get { return this._buyLimitPoint; }
      set { this._buyLimitPoint = value; }
    }

    [Category("Buy"), DisplayName("Trail Enabled"), DefaultValue(false)]
    public bool BuyUseTrail {
      get { return this._buyUseTrail; }
      set { this._buyUseTrail = value; }
    }

    [Category("Buy"), DisplayName("Trail Point"), DefaultValue(15)]
    public int BuyTrailPoint {
      get { return this._buyTrailPoint; }
      set { this._buyTrailPoint = value; }
    }
    #endregion
    
    private float Shift(IVector vector, int shift){
    	int count = vector.Count;
    	if (count - 1 - shift < 0)
    		return float.NaN;
      return vector[count - 1 - shift];
    }

    /// <summary>
    /// OnLoad the method at start of strategy
    /// </summary>
    /// <returns>true if the strategy can be starting</returns>
    public override bool OnLoad() {return true;}
    
    public override void OnOnlineRateChanged(IOnlineRate onlineRate) {
      if (onlineRate.Symbol.Name != _symbolName)
        return;

      IAccount account = Trader.Accounts[0];
      ISymbol symbol = onlineRate.Symbol;
      
      /* * * * * * * * * * * * * Sell * * * * * * * * * * * */
      if(this.UseSellTrading) {

        IVector close = this.Function(symbol, "Close", 900);
        IVector ma13 = this.Function(symbol, "MA", 13, 1, close);
        IVector ma34 = this.Function(symbol, "MA", 34, 1, close);

        ITrade tradeSell = GetTrade(TradeType.Sell);
        if(tradeSell == null) {

           if(ma13.Current > ma34.Current && this.Shift(ma13, 1) <= this.Shift(ma34, 1)) {
            float stopRate = this.SellUseStop ? onlineRate.BuyRate + symbol.Point * this.SellStopPoint : 0;
            float limitRate = this.SellUseLimit ? onlineRate.BuyRate - symbol.Point * this.SellLimitPoint : 0;

            this.Trader.TradeOpen(account.AccountId, symbol.Name, TradeType.Sell, 1, 0, stopRate, limitRate);
          }
        } else {
          CheckTrail(tradeSell);
        }
      }

      /* * * * * * * * * * * * * Buy * * * * * * * * * * * */
      if(this.UseBuyTrading) {
        IVector close = this.Function(symbol, "Close", 900);
        IVector ma13 = this.Function(symbol, "MA", 13, 1, close);
        IVector ma34 = this.Function(symbol, "MA", 34, 1, close);

        ITrade tradeBuy = GetTrade(TradeType.Buy);
        
        if(tradeBuy == null) {
        	
        	if(ma13.Current > ma34.Current && this.Shift(ma13, 1) <= this.Shift(ma34, 1)) {
            float stopRate = this.BuyUseStop ? onlineRate.SellRate - symbol.Point * this.BuyStopPoint : 0;
            float limitRate = this.BuyUseLimit ? onlineRate.SellRate + symbol.Point * this.BuyLimitPoint : 0;
            this.Trader.TradeOpen(account.AccountId, symbol.Name, TradeType.Buy, 1, 0, stopRate, limitRate);
          }
        } else {
          if(ma13.Current < ma34.Current && this.Shift(ma13, 1) >= this.Shift(ma34, 1)) {
            this.Trader.TradeClose(tradeBuy.TradeId, tradeBuy.Lots, 0);
          } else {
            CheckTrail(tradeBuy);
          }
        }
      }
    }

    public override void OnDestroy() {}

    private ITrade GetTrade(TradeType tradeType) {
      for(int i = 0; i < this.Trader.Trades.Count; i++) {
        ITrade trade = this.Trader.Trades[i];
        if(trade.OnlineRate.Symbol.Name == _symbolName && trade.TradeType == tradeType)
          return trade;
      }
      return null;
    }

    private void CheckTrail(ITrade trade) {
      if (trade.OnlineRate.Symbol.Name != _symbolName)
        return;
      if(trade.TradeType == TradeType.Sell) {
        if(!this.SellUseTrail) return;

        float trail = this.SellTrailPoint * trade.OnlineRate.Symbol.Point;
        float newStopRate = trade.OnlineRate.BuyRate + trail;

        if (trade.OpenRate - trade.OnlineRate.BuyRate > trail &&
          (trade.StopOrder == null || trade.StopOrder.Rate > newStopRate)) {
          float currate = trade.StopOrder == null ? 0 : trade.StopOrder.Rate;

          float limitRate = trade.LimitOrder == null ? 0 : trade.LimitOrder.Rate;

          this.Trader.TradeModify(trade.TradeId, newStopRate, limitRate);
        }
      } else {
        if(!this.BuyUseTrail) return;

        float trail = this.BuyTrailPoint * trade.OnlineRate.Symbol.Point;
        float newStopRate = trade.OnlineRate.BuyRate - trail;

        if (trade.OnlineRate.BuyRate - trade.OpenRate > trail &&
          (trade.StopOrder == null || trade.StopOrder.Rate < newStopRate)) {

          float limitRate = trade.LimitOrder == null ? 0 : trade.LimitOrder.Rate;

          this.Trader.TradeModify(trade.TradeId, newStopRate, limitRate);
        }
      }
    }
  }
}
