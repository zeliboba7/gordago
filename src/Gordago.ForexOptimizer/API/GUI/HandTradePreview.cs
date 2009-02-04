/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
#endregion

namespace Gordago.API {
  partial class HandTradePreview:UserControl {

    private bool _issell = false;
    private float _priceStop, _priceLimit;
    private OrderType _orderStopLimit;
    private bool _orderStopLimitError;
    
    public HandTradePreview() {
      InitializeComponent();
      IsSellType = false;
    }

    #region public new string Text
    public new string Text {
      get { return this._lblinfo.Text; }
    }
    #endregion

    #region public bool IsSellType
    [DefaultValue(false), Category("Behavior")]
    public bool IsSellType {
      get { return _issell; }
      set { 
        _issell = value;
      }
    }
    #endregion

    #region public float PriceStop
    public float PriceStop {
      get { return this._priceStop; }
    }
    #endregion

    #region public float PriceLimit
    public float PriceLimit {
      get { return this._priceLimit; }
    }
    #endregion

    #region public bool OrderStopLimitError
    public bool OrderStopLimitError {
      get { return this._orderStopLimitError; }
    }
    #endregion

    #region public OrderType OrderType
    public OrderType OrderType {
      get { return this._orderStopLimit; }
    }
    #endregion

    #region public void SetTrade(IOnlineRate onlineRate, int lots, bool usePO, float pricePO, bool useStop, int pointStop, bool useLimit, int pointLimit)
    public void SetTrade(IOnlineRate onlineRate, int lots, bool usePO, float pricePO, bool useStop, int pointStop, bool useLimit, int pointLimit) {
      if(onlineRate == null) return;
      _orderStopLimitError = false;
      float bid = onlineRate.SellRate;
      float ask = onlineRate.BuyRate;
      string frst = "";
      float rbid, rask;
      float point = onlineRate.Symbol.Point;
      int dig = onlineRate.Symbol.DecimalDigits;

      string s = BrokerCommandManager.LNG_HO_RATE + ": {0}\n" + BrokerCommandManager.LNG_HO_LOTS + ": {1}";

      if(usePO) {
        rbid = rask = pricePO;
        string ps = "";
        if(_issell) {
          if(rbid > bid) {
            ps = BrokerCommandManager.LNG_HO_LIMITORDER; _orderStopLimit = OrderType.Limit;
          } else if(rbid < bid) {
            ps = BrokerCommandManager.LNG_HO_STOPORDER; _orderStopLimit = OrderType.Stop;
          } else {
            ps = BrokerCommandManager.LNG_HO_NONORDER; _orderStopLimitError = true;
          }
        } else {
          if(rask < ask) {
            ps = BrokerCommandManager.LNG_HO_LIMITORDER; _orderStopLimit = OrderType.Limit;
          } else if(rask > ask) {
            ps = BrokerCommandManager.LNG_HO_STOPORDER; _orderStopLimit = OrderType.Stop;
          } else {
            ps = BrokerCommandManager.LNG_HO_NONORDER; _orderStopLimitError = true;
          }
        }

        frst = ps + "\n";
      } else {
        rbid = bid;
        rask = ask;
      }

      string str = _issell ?
        frst + string.Format(s, SymbolManager.ConvertToCurrencyString(rbid, dig), lots) :
        frst + string.Format(s, SymbolManager.ConvertToCurrencyString(rask, dig), lots);

      if (useStop) {
        s = "\n" + BrokerCommandManager.LNG_HO_STOP + ": {0}";

        float sl = pointStop * point;
        _priceStop = _issell ? rask + sl : rbid - sl;

        str += _issell ?
          string.Format(s, SymbolManager.ConvertToCurrencyString(_priceStop, dig)) :
          string.Format(s, SymbolManager.ConvertToCurrencyString(_priceStop, dig));
      } else {
        _priceStop = float.NaN;
      }

      if (useLimit) {
        s = "\n" + BrokerCommandManager.LNG_HO_LIMIT + ": {0}";
        float tp = pointLimit * point;

        _priceLimit = _issell ? rbid - tp : rask + tp;

        str += _issell ?
          string.Format(s, SymbolManager.ConvertToCurrencyString(_priceLimit, dig)) :
          string.Format(s, SymbolManager.ConvertToCurrencyString(_priceLimit, dig));
      } else {
        _priceLimit = float.NaN;
      }

      this._lblinfo.Text = str;
    }
    #endregion

    #region public void Clear()
    public void Clear() {
      this._lblinfo.Text = "";
    }
    #endregion
  }
}
