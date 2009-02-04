using System;
using System.Collections.Generic;
using System.Text;
using Gordago;
using Gordago.API;
using System.IO;
using System.Xml;

namespace IFXMarkets {
  class ClosedTradeList : IClosedTradeList {

    private List<IClosedTrade> _closedTrades;
    private string _dir;
    private IFXMarketsBroker _broker;

    #region public ClosedTradeList(IFXMarketsBroker broker)
    public ClosedTradeList(IFXMarketsBroker broker) {
      _broker = broker;
      _closedTrades = new List<IClosedTrade>();
      _dir = broker.WorkDir + "\\IFXMarkets";
      if (!Directory.Exists(_dir)) {
        Directory.CreateDirectory(_dir);
      }
      this.Load();
    }
    #endregion

    #region public int Count
    public int Count {
      get { return _closedTrades.Count; }
    }
    #endregion

    #region public IClosedTrade this[int index]
    public IClosedTrade this[int index] {
      get { return _closedTrades[index]; }
    }
    #endregion

    #region public void Add(IClosedTrade closeTrade)
    public void Add(IClosedTrade closeTrade) {
      
      for (int i = 0; i < _closedTrades.Count; i++) {
        ClosedTrade ct = _closedTrades[i] as ClosedTrade;
        if (ct.TradeId == closeTrade.TradeId) {
          if (ct.OpenTime == closeTrade.OpenTime && ct.CloseTime == closeTrade.CloseTime) {
            return;
          }
        }
      }
      _closedTrades.Add(closeTrade);
    }
    #endregion

    #region public void Update(IAccount account)
    public void Update(IAccount account) {
      ClosedTrade closedTrade = null;
      for (int i = _closedTrades.Count - 1; i >= 0; i--) {
        ClosedTrade cTrade = _closedTrades[i] as ClosedTrade;
        if (cTrade.AccountId == account.AccountId) {
          closedTrade = cTrade;
          break;
        }
      }
      IClosedTrade[] closedTrades = new IClosedTrade[] { };
//#if !DEBUG
      if (closedTrade == null) {
        closedTrades = _broker.GetClosedTrades(account.AccountId, "", "", new DateTime(1950, 1, 1), DateTime.Now.AddDays(1)).ClosedTrades;
      } else {
        closedTrades = _broker.GetClosedTrades(account.AccountId, closedTrade.TradeId, "", _broker.ConvertToServerTime(closedTrade.CloseTime.AddMinutes(-5)), DateTime.Now.AddDays(1)).ClosedTrades;
      }
//#endif
      for (int i = 0; i < closedTrades.Length; i++) {
        this.Add(closedTrades[i]);
      }
      this.Save();
    }
    #endregion

    #region private void Load()
    private void Load() {
      for (int i = 0; i < _broker.Accounts.Count; i++) {
        ClosedTrade[] closedTrades = this.LoadAccount(_broker.Accounts[i]);
        foreach (ClosedTrade ct in closedTrades) {
          this.Add(ct);
        }
        this.Update(_broker.Accounts[i]);
      }
    }
    #endregion

    #region private string GetFileName(IAccount account)
    private string GetFileName(IAccount account) {
      string fileName = _dir + "\\" + account.AccountId + ".xml";
      return fileName;
    }
    #endregion

    #region private ClosedTrade[] LoadAccount(IAccount account)
    private ClosedTrade[] LoadAccount(IAccount account) {
      List<ClosedTrade> list = new List<ClosedTrade>();
      string fileName = this.GetFileName(account);

      if (!File.Exists(fileName))
        return list.ToArray();

      XmlDocument doc = new XmlDocument();
      doc.Load(fileName);
      XmlNode node = doc["IFXMarkets"];
      if (node == null)
        return list.ToArray();

      foreach (XmlNode childnode in node.ChildNodes) {
        if (childnode.Name == "CTrade") {
          XmlNodeManager nodeManager = new XmlNodeManager(childnode);
          ClosedTrade cTrade = new ClosedTrade(account, nodeManager);
          list.Add(cTrade);
        }
      }

      return list.ToArray();
    }
    #endregion

    #region public void Save()
    public void Save() {
      for (int i = 0; i < _broker.Accounts.Count; i++) {
        this.Save(_broker.Accounts[i] as Account);
      }
    }
    #endregion

    #region private void Save(IAccount account)
    private void Save(IAccount account) {
      string fileName = this.GetFileName(account);
      XmlDocument doc = new XmlDocument();
      doc.LoadXml("<IFXMarkets Account=\"" + account.AccountId + "\" Version=\"1.0\"></IFXMarkets>");

      for (int i = 0; i < _closedTrades.Count; i++) {
        ClosedTrade cTrade = _closedTrades[i] as ClosedTrade;
        if (cTrade.AccountId == account.AccountId) {

          XmlNode node = doc.CreateElement("CTrade");
          doc.DocumentElement.AppendChild(node);

          XmlNodeManager nodeM = new XmlNodeManager(node);
          cTrade.Save(nodeM);
        }
      }
      if (File.Exists(fileName))
        File.Delete(fileName);
      doc.Save(fileName);
    }
    #endregion
  }
}
