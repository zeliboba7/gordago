/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections;
using System.Collections.Specialized;

using Gordago.API;
using Gordago.API.VirtualForex;

namespace Gordago.Analysis{
	class TestReport {

    private TradeVariables _variables;
    private BrokerJournal _journal;
    private float _balance = 0;
    private ClosedTradeList _closedTrades;
    private string _symbolName;
    private DateTime _periodFrom, _periodTo;
    private long _timeTest;
    private TimeFrame _tf;

    public TestReport(DateTime periodFrom, DateTime periodTo, TimeFrame tf) {
      _tf = tf;
      _periodFrom = periodFrom;
      _periodTo = periodTo;
    }

    #region public TimeFrame TimeFrame
    public TimeFrame TimeFrame {
      get { return _tf; }
    }
    #endregion

    #region public DateTime PeriodFrom
    public DateTime PeriodFrom {
      get { return _periodFrom; }
    }
    #endregion

    #region public DateTime PeriodTo
    public DateTime PeriodTo {
      get { return _periodTo; }
    }
    #endregion

    #region public TradeVariable Variables
    public TradeVariables Variables {
      get { return _variables; }
    }
    #endregion

    #region public BrokerJornal Jornal
    public BrokerJournal Journal {
      get { return _journal; }
    }
    #endregion

    #region public ClosedTradeList ClosedTrades
    public ClosedTradeList ClosedTrades {
      get { return this._closedTrades; }
    }
    #endregion

    #region public string SymbolName
    public string SymbolName {
      get { return this._symbolName; }
    }
    #endregion

    #region public DateTime TimeTest
    public DateTime TimeTest {
      get { return new DateTime(_timeTest); }
    }
    #endregion

    #region public void SetVariables(TradeVariables variables, VirtualBroker broker, string symbolName, long timeTest)
    public void SetVariables(TradeVariables variables, VirtualBroker broker, string symbolName, long timeTest) {
      _timeTest = timeTest;
      _symbolName = symbolName;

      float balance = 0;

      for (int i = 0; i < broker.Accounts.Count; i++) {
        balance += broker.Accounts[i].Balance;
      }

      if (_journal == null) {
        _variables = variables.Clone() as TradeVariables;
        _journal = broker.Journal;
        _closedTrades = (ClosedTradeList)broker.ClosedTrades;
        _balance = balance;
      } else {


        if (balance > _balance) {
          _variables = variables.Clone() as TradeVariables;
          _journal = broker.Journal;
          _closedTrades = (ClosedTradeList)broker.ClosedTrades;
        }
      }
    }
    #endregion
  }
}
