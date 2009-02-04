/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using
using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using Gordago.API;
using FxComApiTrader;

using Language;
using System.Windows.Forms;
using Gordago.Server;
#endregion

namespace Gordago.API {

  /// <summary>
  /// Процесс работы с АПИ Сервера.
  /// Принцип: Команда ставится в задание, возникает событие на начало выполнение команды, 
  /// по завершению возникает событие на завершению команды
  /// </summary>
	public class BrokerCommandManager{

		#region private class TOSL
		private class TOSL{
			private string _id;
			private int  _ts, _slippage;
			private float _price, _stop, _limit;
			private APICommand _cmd;
			
			public TOSL(APICommand cmd, string id, float price, float stop, int ts, float limit, int slippage){
				_cmd = cmd;
				_id = id;
				_price = price;
				_stop = stop;
				_ts = ts;
				_limit = limit;
				_slippage = slippage;
			}

			#region public APICommand Command
			public APICommand Command{
				get{return this._cmd;}
			}
			#endregion

			#region public string Id
			public string Id{
				get{return this._id;}
			}
			#endregion

			#region public int TS
			public int TS{
				get{return this._ts;}
			}
			#endregion

			#region public float Stop
			public float Stop{
				get{return _stop;}
			}
			#endregion

			#region public float Limit
			public float Limit{
				get{return _limit;}
			}
			#endregion

			#region public int SleepPage
			public int SleepPage{
				get{return this._slippage;}
			}
			#endregion

			public float Rate{
				get{return this._price;}
			}

		}
		#endregion

    private bool _beginlogoff = false;

		private string _defaultAccountId = "";
		private TOSL[] _tosls;

		private APITrader _api;
		private bool _run;

		private string _login, _password;
		private bool _isdemo, _proxyenable;
		
		private string _accountId, _tradeId, _clientTag, _orderId;
		private OnlineRate _pair;
		private ITrade _trade;
		private int _lotCount;
		private int _trailingstopPoint, _slippage;
		private float _rate, _stoplossPrice, _takeprofitPrice;
		private TradeType _buysell;
		private APICommand _curcmd = APICommand.Empty;
		private OrderType _stoplimit;

    private IServer _server;

		public BrokerCommandManager() {
			_tosls = new TOSL[0];
			
		}

		#region public string UserName
		public string UserName{
			get{return this._login;}
		}
		#endregion

		#region public string UserPass
		public string UserPass{
			get{return this._password;}
		}
		#endregion

		#region public bool IsDemo
		public bool IsDemo{
			get{return this._isdemo;}
		}
		#endregion

		#region public bool ProxyEnable
		public bool ProxyEnable{
			get{return this._proxyenable;}
			set{this._proxyenable = value;}
		}
		#endregion

		#region public APITrader APITrader
		public APITrader APITrader{
			get{return this._api;}
		}
		#endregion

		#region public void Dispose()
		public void Dispose(){
      if (_api != null)
        _api.Dispose();
      _run = false;
		}
		#endregion

		#region public bool Busy
		public bool Busy{
			get{return _curcmd != APICommand.Empty;}
		}
		#endregion

		#region private void TOSLAdd(TOSL tosl)
		private void TOSLAdd(TOSL tosl){
			ArrayList al = new ArrayList(_tosls);
			al.Add(tosl);
			_tosls = (TOSL[])al.ToArray(typeof(TOSL));
		}
		#endregion

		#region private TOSL TOSLGet(string id)
		private TOSL TOSLGet(string id){
			foreach (TOSL tosl in _tosls){
				if (tosl.Id == id)
					return tosl;
			}
			return null;
		}
		#endregion

		#region private void TOSLDelete(string id)
		private void TOSLDelete(string id){
			ArrayList al = new ArrayList();
			foreach (TOSL tosl in _tosls){
				if (tosl.Id != id)
					al.Add(tosl);
			}
			_tosls = (TOSL[])al.ToArray(typeof(TOSL));
		}
		#endregion

		#region private void OnCommandStarting(APICommand cmd){
		private void OnCommandStarting(APICommand cmd){ 
			_api.Log.Add(string.Format("Start command: {0}",GetCmdToStr(cmd)));

			if (this.CommandStartingEvent != null)
				this.CommandStartingEvent(cmd);
		}
		#endregion

		#region private void OnCommandStopping(APICommand cmd, APIError error, object data)
		private void OnCommandStopping(APICommand cmd, APIError error, object data){ 
			string errmsg = error == null ? "non":error.Message;
			_api.Log.Add(string.Format("Stop command: {0}, error = '{1}'",GetCmdToStr(cmd), errmsg));
			if (this.CommandStoppingEvent != null)
				this.CommandStoppingEvent(cmd, error, data);
		}
		#endregion

		#region private string GetCmdToStr(APICommand cmd)
		private string GetCmdToStr(APICommand cmd){
			switch(cmd){
				case APICommand.AcceptRejectedOrder:
					return "AcceptRejectedOrder";
				case APICommand.ChangeEntryOrder:
					return "ChangeEntryOrder";
				case APICommand.ChangeStopLimitTrailOnOrder:
					return "ChangeStopLimitTrailOnEntryOrder";
				case APICommand.ChangeStopLimitTrailOnTrade:
					return "ChangeStopLimitTrailOnTrade";
				case APICommand.CloseTrade:
					return "CloseTrade";
				case APICommand.CreateOrder:
					return "CreateEntryOrder";
				case APICommand.CreateTrade:
					return "CreateInitOrder";
				case APICommand.DeleteOrder:
					return "DeleteOrder";
				case APICommand.Logon:
					return "Logon";
			}
			return "Unknow";
		}
		#endregion

		#region public void CreateInitOrder(string accountId, IOnlineRate onlineRate, int lotCount, TradeType buysell, float stopPrice, int trailPoint, float limitPrice, int slippage, string clientTag) 
		public void CreateInitOrder(string accountId, IOnlineRate onlineRate, int lotCount, TradeType buysell, float stopPrice, int trailPoint, float limitPrice, int slippage, string clientTag) {

			if (!WaitBusy()) return;

			_accountId = accountId;
			_pair = pair;
			_lotCount = lotCount;
			_buysell = buysell;
			_clientTag = clientTag;
			_stoplossPrice = stopPrice;
			_trailingstopPoint = trailPoint;
			_takeprofitPrice = limitPrice;
			_slippage = slippage;

			_curcmd = APICommand.CreateTrade;
		}
		#endregion

		#region public void ChangeStopLimitTrailOnTrade(ITrade trade, float stopPrice, int trailPoint, float limitPrice)
		/// <summary>
		/// Выставление, изменение, удаление Стопа, Лимита, Трейла
		/// </summary>
		/// <param name="trade">Trade</param>
		/// <param name="stoploss">Stop - в цене</param>
		/// <param name="trailPoint">Trail - в пунктах</param>
		/// <param name="takeprofit">Limit - в цене</param>
		public void ChangeStopLimitTrailOnTrade(ITrade trade, float stopPrice, int trailPoint, float limitPrice){
			if (!WaitBusy()) return;
			_trade = trade;
			_tradeId = trade.TradeId;

			if (float.IsNaN(limitPrice))
				limitPrice = -1;

			if (float.IsNaN(stopPrice))
				stopPrice = -1;

			float curlimitrate =  trade.LimitOrder != null ? trade.LimitOrder.Rate : -1;
			if (limitPrice != curlimitrate)
				_takeprofitPrice = limitPrice;
			else
				_takeprofitPrice = float.NaN;
			
			float curstoprate = trade.StopOrder != null ? trade.StopOrder.Rate : -1;
			if (stopPrice != curstoprate)
				_stoplossPrice = stopPrice;
			else
				_stoplossPrice = float.NaN;

			int trail = trade.StopOrder != null ? trade.StopOrder.TrailDistance : -1;

			if (trail != trailPoint)
				_trailingstopPoint = trail;
			else
				_trailingstopPoint = 0;

			_curcmd = APICommand.ChangeStopLimitTrailOnTrade;
		}
		#endregion

		#region public void CloseTrade(ITrade trade, int lotCount, int slippage)
		public void CloseTrade(ITrade trade, int lotCount, int slippage) {
			if (!WaitBusy()) return;

			_trade = trade;
			_lotCount = lotCount;
			_slippage = slippage;
			_curcmd = APICommand.CloseTrade;
		}
		#endregion

		#region public void DeleteOrder(IOrder order) 
		public void DeleteOrder(IOrder order) {
			if (!WaitBusy()) return;
			_orderId = order.OrderId;
			_curcmd = APICommand.DeleteOrder;
		}
		#endregion

		#region public void CreateEntryOrder(OrderType stoplimit, string accountId, IOnlineRate onlineRate, int lotCount, TradeType buysell, float rate, float stopRate, float limitRate, string clientTag)
		public void CreateEntryOrder(OrderType stoplimit, string accountId, IOnlineRate onlineRate, int lotCount, TradeType buysell, float rate, float stopRate, float limitRate, string clientTag) {
			if (!WaitBusy()) return;
			_stoplimit = stoplimit;
			_accountId = accountId;
			_pair = pair;
			_lotCount = lotCount;
			_buysell = buysell;
			_rate = rate;
			_stoplossPrice = stopRate;
			_takeprofitPrice = limitRate;
			_clientTag = clientTag;
			_curcmd = APICommand.CreateOrder;
		}
		#endregion

		#region public void AcceptRejectedOrder(IOrder order) 
		public void AcceptRejectedOrder(IOrder order) {
			if (!WaitBusy()) return;
			_orderId = order.OrderId;
			_curcmd = APICommand.AcceptRejectedOrder;
		}
		#endregion

		#region private void ChangeStopLimitTrailOnEntryOrder(IOrder order, float stopPrice, int trailPoint, float limitPrice)
		private void ChangeStopLimitTrailOnEntryOrder(IOrder order, float stopPrice, int trailPoint, float limitPrice) {
			if (!WaitBusy()) return;

			/* Если float.NaN то не производит изменений */
			
			_orderId = order.OrderId;

			if (float.IsNaN(limitPrice))
				limitPrice = -1;

			if (float.IsNaN(stopPrice))
				stopPrice = -1;

			float curlimitrate =  order.LimitOrder != null ? order.LimitOrder.Rate : -1;
			if (limitPrice != curlimitrate)
				_takeprofitPrice = limitPrice;
			else
				_takeprofitPrice = float.NaN;
			
			float curstoprate = order.StopOrder != null ? order.StopOrder.Rate : -1;
			if (stopPrice != curstoprate)
				_stoplossPrice = stopPrice;
			else
				_stoplossPrice = float.NaN;

			_trailingstopPoint = trailPoint;
			_curcmd = APICommand.ChangeStopLimitTrailOnOrder;
		}
		#endregion

		#region public void ChangeEntryOrder(IOrder order, float newRate, float stopPrice, int trailPoint, float limitPrice) 
		public void ChangeEntryOrder(IOrder order, float newRate, float stopPrice, int trailPoint, float limitPrice) {
			if (!WaitBusy()) return;

			_orderId = order.OrderId;
			_rate = newRate;
			_stoplossPrice = stopPrice;
			_trailingstopPoint = trailPoint;
			_takeprofitPrice = limitPrice;
			if (order.Rate != newRate)
				_curcmd = APICommand.ChangeEntryOrder;
			else
				this.ChangeStopLimitTrailOnEntryOrder(order, stopPrice, trailPoint, limitPrice);
		}
		#endregion

		#region over functions
		private FxError fxGetOrdersHistory(string FromOrderId, string ToOrderId, DateTime FromTime, DateTime ToTime, string AccountId, string PairId, out FxOrderList OrderList) {
			OrderList = null;
			return null;
		}

		private FxError fxGetClosedTrades(string FromTradeId, string ToTradeId, DateTime FromTime, DateTime ToTime, string AccountId, string PairId, out FxTradeList TradeList) {
			TradeList = null;
			return null;
		}

		private FxError fxChangeUserPassword(string Password) {
			return null;
		}
		private FxError fxHedgeTrade(string TradeId, int LotCount, out string HedgeOrderId, string ClientTag, FxLotList Lots) {
			HedgeOrderId = null;
			return null;
		}
		#endregion

		#region API_Events
		#region private void API_AccountsChangegEvent(APIAccount account, Gordago.API.BrokerMessageType mtype) 
		private void API_AccountsChangegEvent(APIAccount account, Gordago.API.BrokerMessageType mtype) {
      if(GordagoMain.IsCloseProgram) return;
			if (this.AccountsChangegEvent != null)
				this.AccountsChangegEvent(account, mtype);
		}
		#endregion

		#region private void API_OrdersChangedEvent(IOrder order, Gordago.API.BrokerMessageType mtype) 
		private void API_OrdersChangedEvent(IOrder order, Gordago.API.BrokerMessageType mtype) {
      if(GordagoMain.IsCloseProgram) return;
      if((mtype == BrokerMessageType.Add || mtype == BrokerMessageType.Update) && 
				(order.OrderType == OrderType.EntryLimit || order.OrderType == OrderType.EntryStop)){
				TOSL tosl = TOSLGet(order.OrderId);
				if (tosl != null && 
					(tosl.Command == APICommand.CreateOrder || tosl.Command == APICommand.ChangeEntryOrder) &&
					(tosl.Stop > 0 || tosl.Limit > 0 || tosl.TS > 0)){

					this.ChangeStopLimitTrailOnEntryOrder(order, tosl.Stop, tosl.TS, tosl.Limit);
					this.TOSLDelete(order.OrderId);	
				}
			}else if (mtype == BrokerMessageType.Update && ( order.OrderType == OrderType.RejectInit || order.OrderType == OrderType.RejectClose)){

				bool accept = false;
				TOSL tosl = TOSLGet(order.OrderId);
				if (tosl != null && (tosl.Command == APICommand.CloseTrade || tosl.Command == APICommand.CreateTrade)){
					int dslip = Broker.CalculatePoint(tosl.Rate, order.Rate, order.OnlineRate.Symbol.DecimalDigits, true);
					if (dslip <= tosl.SleepPage){
						accept = true;
					}
					this.TOSLDelete(order.OrderId);	
				}
				if (accept)
					this.AcceptRejectedOrder(order);
				else
					this.DeleteOrder(order);
			}
			if (this.OrdersChangedEvent != null)
				this.OrdersChangedEvent(order, mtype);
		}
		#endregion

		#region private void API_TradesChangedEvent(ITrade trade, Gordago.API.BrokerMessageType mtype) 
		private void API_TradesChangedEvent(ITrade trade, Gordago.API.BrokerMessageType mtype) {
      if(GordagoMain.IsCloseProgram) return;
      if(mtype == BrokerMessageType.Add) {
				TOSL tosl = TOSLGet(trade.ParentOrderId);
				if (tosl != null && tosl.Command == APICommand.CreateTrade &&
					(tosl.Stop > 0 || tosl.Limit > 0 || tosl.TS > 0)){

					this.ChangeStopLimitTrailOnTrade(trade, tosl.Stop, tosl.TS, tosl.Limit);
					this.TOSLDelete(trade.ParentOrderId);	
				}
			}
			if (this.TradesChangedEvent != null)
				this.TradesChangedEvent(trade, mtype);
		}
		#endregion

		#region private void API_PairsChangedEvent(IOnlineRate onlineRate, Gordago.API.BrokerMessageType mtype) 
		private void API_PairsChangedEvent(IOnlineRate onlineRate, Gordago.API.BrokerMessageType mtype) {
      if(GordagoMain.IsCloseProgram) return;
      if(this.PairsChangedEvent != null)
				this.PairsChangedEvent(pair, mtype);
		}
		#endregion

		#region private void API_ConnectionStatusChanged(Gordago.API.BrokerConnectionStatus status) 
		private void API_ConnectionStatusChanged(Gordago.API.BrokerConnectionStatus status) {
      if(GordagoMain.IsCloseProgram) return;
      if(status == BrokerConnectionStatus.Offline) {
        ArrayList al = new ArrayList(_symbolUpdates);
        al.AddRange(_symbolUpdatesDown);
        al.AddRange(_symbolUpdatesWait);

        _symbolUpdates = new SymbolUpdate[0];
        _symbolUpdatesDown = new SymbolUpdate[0];
        _symbolUpdatesWait = (SymbolUpdate[])al.ToArray(typeof(SymbolUpdate));
      } else if(status == BrokerConnectionStatus.Online) {
        Gordago.Analysis.SymbolsProperty sprops = new Gordago.Analysis.SymbolsProperty();
        for(int i = 0; i < _api.Pairs.Count; i++) {
          IOnlineRate onlineRate = _api.Pairs[i];
          string sname = pair.Symbol.Name;
          int spread = Convert.ToInt32( (pair.BuyRate - pair.SellRate) * SymbolManager.GetDelimiter(pair.Symbol.DecimalDigits));
          float pipCost = pair.PipCost;

          Gordago.Analysis.SymbolProperty prop =
            new Gordago.Analysis.SymbolProperty(sname, spread, pipCost);
          sprops.Update(prop);
        }
        sprops.Save();
      }
      if(this.ConnectionStatusChangedEveng != null)
				this.ConnectionStatusChangedEveng(status);
		}
		#endregion

		#region private void API_MarginCallMessage(APIMarginCallMessage message) 
		private void API_MarginCallMessage(APIMarginCallMessage message) {
      if(GordagoMain.IsCloseProgram) return;
      if(this.MarginCallMessageEvent != null)
				this.MarginCallMessageEvent(message);
		}
		#endregion

		#region private void API_ErrorEvent(APIError error)
		private void API_ErrorEvent(APIError error){
      if(GordagoMain.IsCloseProgram) return;
      if(this.ErrorEvent != null)
				this.ErrorEvent(error);
		}
		#endregion

		#region private void API_LogMessageAdded(APILogMessage message)
		private void API_LogMessageAdded(APILogMessage message){
      if(GordagoMain.IsCloseProgram) return;
      if(this.LogMessageAdded != null)
				this.LogMessageAdded(message);
		}
		#endregion

		#region private void API_SymbolDownloadPart(Symbol symbol)
		private void API_SymbolDownloadPart(ISymbol symbol){
      if(GordagoMain.IsCloseProgram) return;
      if(this.SymbolDownloadPart != null)
				this.SymbolDownloadPart(symbol);
		}
		#endregion
		#endregion

    #region public bool CheckAccountTrade()
    /// <summary>
    /// Проверка на возможность торговли.
    /// </summary>
    /// <returns></returns>
    public bool CheckAccountTrade() {
      for(int i = 0; i < this.APITrader.Accounts.Count; i++) {
        APIAccount account = this.APITrader.Accounts[i];
        if(account.AccountId == this.DefaultAccountId && !account.Active) {
          MessageBox.Show( string.Format(Language.Dictionary.GetString(30, 105, "Операция не возможна. Счет №{0} не активен."), account.AccountId), GordagoMain.MessageCaption);
          return false;
        }
      }
      return true;
    }
    #endregion

    #region public static string GetLngOrderType(OrderType ot)
    public static string GetLngOrderType(OrderType ot) {
      switch (ot) {
        case OrderType.Close:
          return LNG_OT_CLOSE;
        case OrderType.EntryFailed:
          return LNG_OT_ENTRYFAILED;
        case OrderType.EntryLimit:
          return LNG_OT_ENTRYLIMIT;
        case OrderType.EntryStop:
          return LNG_OT_ENTRYSTOP;
        case OrderType.Init:
          return LNG_OT_INIT;
        case OrderType.InitFailed:
          return LNG_OT_INITFAILED;
        case OrderType.Limit:
          return LNG_OT_LIMIT;
        case OrderType.LimitFailed:
          return LNG_OT_LIMITFAILED;
        case OrderType.Margin:
          return LNG_OT_MARGIN;
        case OrderType.MinEquity:
          return LNG_OT_MINEQUITY;
        case OrderType.RejectClose:
          return LNG_OT_REJECTCLOSE;
        case OrderType.RejectInit:
          return LNG_OT_REJECTINIT;
        case OrderType.Stop:
          return LNG_OT_STOP;
        case OrderType.StopFailed:
          return LNG_OT_STOPFAILED;
      }
      return "";
    }
    #endregion

  }

  #region public enum APICommand
  public enum APICommand {
    MainProccess,
    Empty,
    Logon,
    Logoff,
    CreateTrade,
    CloseTrade,
    CreateOrder,
    DeleteOrder,
    ChangeStopLimitTrailOnTrade,
    ChangeStopLimitTrailOnOrder,
    AcceptRejectedOrder,
    ChangeEntryOrder
  }
  #endregion

}
