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
using Gordago.Analysis;
using Language;
using Cursit.Applic.AConfig;
using System.IO;
using System.Diagnostics;
using Gordago.API;
using Gordago.API.VirtualForex;
using Cursit.Table;
#endregion

namespace Gordago.Strategy {
  public partial class TestReportForm:Form {

    private TestReport _reports;
    private TextWriter _tw;

    private string _tmpfilename;

    public TestReportForm() {
      InitializeComponent();
    }

    internal void SetTestServer(TestReport reports, string strategyFileName) {

      _reports = reports;

      #region Dict
      this._btnSave.Text = Dictionary.GetString(16, 43, "Сохранить");
      this._tbpDetail.Text = Dictionary.GetString(16, 26, "Детальный отчет");
      this._tbpDBuy.Text = Dictionary.GetString(16, 27, "Покупка");
      this._tbpDSell.Text = Dictionary.GetString(16, 28, "Продажа");
      this._tbpResult.Text = Dictionary.GetString(16, 60, "Журнал");

      string lngOpenDate = Dictionary.GetString(16, 29, "Open date");
      string lngOpenTime = Dictionary.GetString(16, 30, "Open time");
      string lngOpenPrice = Dictionary.GetString(16, 31, "Open price");
      string lngCloseDate = Dictionary.GetString(16, 32, "Close date");
      string lngCloseTime = Dictionary.GetString(16, 33, "Close time");
      string lngClosePrice = Dictionary.GetString(16, 34, "Close price");
      string lngProfit = Dictionary.GetString(16, 35, "Profit");
      string lngDrawDown = Dictionary.GetString(16, 36, "DrawDown");
      string lngDrawUp = Dictionary.GetString(16, 38, "DrawUp");
      string lngExitCause = Dictionary.GetString(16, 37, "Exit Cause");

      this._lstCommon.Columns.Add("", 300, HorizontalAlignment.Left);
      this._lstCommon.Columns.Add(Dictionary.GetString(16, 22, "Продажа"), -2, HorizontalAlignment.Right);
      this._lstCommon.Columns.Add(Dictionary.GetString(16, 21, "Покупка"), -2, HorizontalAlignment.Right);
      this._lstCommon.Columns.Add(Dictionary.GetString(16, 23, "Всего"), -2, HorizontalAlignment.Right);
      this._lstCommon.Columns.Add("", -2, HorizontalAlignment.Left);

      this._tbpCommon.Text = Dictionary.GetString(16, 25, "Общая информация");
      this._lblCommon.Text = Dictionary.GetString(16, 7, "Общая информация");
      this._lblFilter.Text = Dictionary.GetString(16, 47, "Фильтр");


      this._gtsColSNN.HeaderText = this._gtsColBNN.HeaderText = Dictionary.GetString(16, 49, "№");
      this._gtsColSOpenTime.HeaderText = this._gtsColBOpenTime.HeaderText = Dictionary.GetString(16, 29, "Вход: Дата");
      this._gtsColSOpenRate.HeaderText = this._gtsColBOpenRate.HeaderText = Dictionary.GetString(16, 31, "Цена");
      this._gtsColSLots.HeaderText = this._gtsColBLots.HeaderText = Dictionary.GetString(16, 54, "Лот");
      this._gtsColSCloseTime.HeaderText = this._gtsColBCloseTime.HeaderText = Dictionary.GetString(16, 32, "Время закрытия");
      this._gtsColSCloseRate.HeaderText = this._gtsColBCloseRate.HeaderText = Dictionary.GetString(16, 34, "Цена");
      this._gtsColSProfit.HeaderText = this._gtsColBProfit.HeaderText = Dictionary.GetString(16, 35, "Прибыль");
      this._gtsColSDrawDown.HeaderText = this._gtsColBDrawDown.HeaderText = Dictionary.GetString(16, 36, "Просадка");
      this._gtsColSGorwUp.HeaderText = this._gtsColBGrowUp.HeaderText = Dictionary.GetString(16, 38, "Прирост");
      this._gtsColSExitCause.HeaderText = this._gtsColBExitCause.HeaderText = Dictionary.GetString(16, 42, "Условие");
#endregion

      _tmpfilename = Application.StartupPath + "\\temp\\test_" + 
        DateTime.Now.Year.ToString()+"_"+
        DateTime.Now.Month.ToString()+ "_"+
        DateTime.Now.Day.ToString()+ "_"+
        DateTime.Now.Hour.ToString()+ "_"+
        DateTime.Now.Minute.ToString()+ "_"+
        DateTime.Now.Second.ToString()+ ".html.tmp";
      
      Cursit.Utils.FileEngine.CheckDir(_tmpfilename);

      if(System.IO.File.Exists(_tmpfilename))
        System.IO.File.Delete(_tmpfilename);

      FileStream fs = new FileStream(_tmpfilename, FileMode.CreateNew);
      _tw = new StreamWriter(fs, Encoding.GetEncoding("windows-1251"));

      _tw.WriteLine("<html><head><title>Gordago Forex Optimizer TT</title>");
      _tw.WriteLine("<meta http-equiv=Content-Type content='text/html; charset=windows-1251'>");
      _tw.WriteLine("</head><body>");

      _tw.WriteLine("<style type='text/css'>");
      _tw.WriteLine("H1{FONT-WEIGHT: bold; FONT-SIZE: 14px; COLOR: #4669b1; FONT-FAMILY: Tahoma, Verdana, Arial, helvetica, sans-serif}");
      _tw.WriteLine("H2 {FONT-SIZE: 12px; COLOR: #c7c7c7; FONT-FAMILY: Tahoma, Verdana, Arial, helvetica, sans-serif}");
      _tw.WriteLine("H3 {FONT-SIZE: 14px; FONT-FAMILY: Tahoma, Verdana, Arial, helvetica, sans-serif}");
      _tw.WriteLine("H4 {FONT-SIZE: 12px; COLOR: #c7c7c7; FONT-FAMILY: Tahoma, Verdana, Arial, helvetica, sans-serif}");
      _tw.WriteLine("H5 {FONT-SIZE: 10px; COLOR: #c7c7c7; FONT-FAMILY: Tahoma, Verdana, Arial, helvetica, sans-serif}");
      _tw.WriteLine("H6 {FONT-SIZE: 8px; COLOR: #c7c7c7; FONT-FAMILY: Tahoma, Verdana, Arial, helvetica, sans-serif}");
      _tw.WriteLine("A:visited {COLOR: #1f89b5; TEXT-DECORATION: underline}");
      _tw.WriteLine("A:hover {COLOR: #FF5400; TEXT-DECORATION: underline}");
      _tw.WriteLine("A:link {COLOR: #1f89b5; text-decoration:none}");
      _tw.WriteLine("A:active {COLOR: #1f89b5; TEXT-DECORATION: none}");
      _tw.WriteLine("</style>");

      _tw.WriteLine("<h1>Gordago Forex Optimizer TT: Strategy Tester Report</h1>");

      string sfile = Cursit.Utils.FileEngine.GetFileNameFromPath(strategyFileName);
      _tw.WriteLine(string.Format("<p>{0}, {1}</p>", sfile, DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString()));

      this.UpdateHistoryInfo();

      int countSell = 0, countBuy = 0;
      int countSellProfit = 0, countBuyProfit = 0;
      int sellProfit = 0, sellMaxProfit=0, sellMaxLoss=0; 
      int buyProfit = 0, buyMaxProfit=0, buyMaxLoss=0;
      int sellMaxDrawDown = 0, sellMaxGrowUp = 0;
      int buyMaxDrawDown = 0, buyMaxGrowUp = 0;

      
      _grdSell.DataMember = "";
      _grdBuy.DataMember = "";
      for (int i = 0; i < _reports.ClosedTrades.Count; i++) {
        DataRow row = null;

        ClosedTrade trade = (ClosedTrade)_reports.ClosedTrades[i];
        int profit = Convert.ToInt32(trade.NetPLPoint);
        int drawDown = trade.GetDrawDownPoint();
        int growUp = trade.GetGrowUpPoint();

        if (trade.TradeType == TradeType.Sell) {
          countSell++;
          if (profit > 0) countSellProfit++;
          sellProfit += profit;
          row = _tblSell.NewRow();
          _tblSell.Rows.Add(row);
          sellMaxProfit = Math.Max(sellMaxProfit, profit);
          sellMaxLoss = Math.Min(sellMaxLoss, profit);

          sellMaxDrawDown = Math.Min(drawDown, sellMaxDrawDown);
          sellMaxGrowUp = Math.Max(growUp, sellMaxGrowUp);
        } else {
          countBuy++;
          if (profit > 0) countBuyProfit++;
          buyProfit += profit;
          row = _tblBuy.NewRow();
          _tblBuy.Rows.Add(row);
          buyMaxProfit = Math.Max(buyMaxProfit, profit);
          buyMaxLoss = Math.Min(buyMaxLoss, profit);

          buyMaxDrawDown = Math.Min(drawDown, buyMaxDrawDown);
          buyMaxGrowUp = Math.Max(growUp, buyMaxGrowUp);
        }
        row["NN"] = trade.TradeId;
        row["OpenTime"] = trade.OpenTime.ToShortDateString() + " " + trade.OpenTime.ToShortTimeString();
        row["OpenRate"] = SymbolManager.ConvertToCurrencyString(trade.OpenRate, trade.Symbol.DecimalDigits);
        row["Lots"] = trade.Amount;
        row["CloseTime"] = trade.CloseTime.ToShortDateString() + " " + trade.CloseTime.ToShortTimeString();
        row["CloseRate"] = SymbolManager.ConvertToCurrencyString(trade.CloseRate, trade.Symbol.DecimalDigits);
        row["Profit"] = trade.NetPLPoint;
        row["DrawDown"] = drawDown;
        row["GrowUp"] = growUp;

        string exitCause = "";
        row["ExitCause"] = exitCause;
      }
      _grdSell.DataMember = "Sell";
      _grdBuy.DataMember = "Buy";

      string hcol1 = "";
      string hcol2 = Dictionary.GetString(16, 22, "Продажа");
      string hcol3 = Dictionary.GetString(16, 21, "Покупка");
      string hcol4 = Dictionary.GetString(16, 23, "Всего");

      _tw.WriteLine("<br>");

      HTMLOpenTable(this._lblCommon.Text, true, 0,
        hcol1 + "\t width='250' align='left'",
        hcol2 + "\t width='50' align='center'",
        hcol3 + "\t width='50' align='center'",
        hcol4 + "\t width='50' align='center'");

      AddMainRow("", hcol2,hcol3,hcol4, false);

      AddMainRow(Dictionary.GetString(16, 7, "Общие"), "", "", "", true);
      AddMainRow(Dictionary.GetString(16, 8, "Кол-во сделок"),
        countSell.ToString(),
        countBuy.ToString(),
        ((int)(countBuy + countSell)).ToString(), true);

      AddMainRow(Dictionary.GetString(16, 9, "Кол-во прибыльных сделок"),
        countSellProfit.ToString(), countBuyProfit.ToString(),
        ((int)(countBuyProfit + countSellProfit)).ToString(), true);

      AddMainRow(Dictionary.GetString(16, 10, "Net profit"),
        sellProfit.ToString(),
        buyProfit.ToString(),
        ((int)(sellProfit + buyProfit)).ToString(), true);

      AddMainRow("", "", "", "", true);
      AddMainRow(Dictionary.GetString(16, 13, "По сделке"), "", "", "", true);
      AddMainRow(Dictionary.GetString(16, 14, "Максимальная прибыль"),
        sellMaxProfit.ToString(),
        buyMaxProfit.ToString(),
        Convert.ToString(Math.Max(sellMaxProfit, buyMaxProfit)), true);

      AddMainRow(Dictionary.GetString(16, 17, "Максимальный убыток"),
        sellMaxLoss.ToString(),
        buyMaxLoss.ToString(),
        Convert.ToString(Math.Min(sellMaxLoss, buyMaxLoss)), true);

      AddMainRow(Dictionary.GetString(16, 24, "Максимальная общая просадка"),
        sellMaxDrawDown.ToString(),
        buyMaxDrawDown.ToString(),
        ((int)(Math.Min(sellMaxDrawDown, buyMaxDrawDown))).ToString(), true);

      AddMainRow(Dictionary.GetString(16,39,"Максимальный общий прирост"),
        sellMaxGrowUp.ToString(),
        buyMaxGrowUp.ToString(),
        ((int)(Math.Max(sellMaxGrowUp, buyMaxGrowUp))).ToString(), true);

      HTMLCloseTable();

      _tw.WriteLine("<br>");
      this.UpdateResult(true);

      _tw.WriteLine("<br><center><a href='http://www.gordago.com'>© Copyright 2005-2007 Gordago Software Ltd.</a></center>");

      _tw.WriteLine("</body></html>");
      _tw.Flush();
      fs.Close();

      Terminal.BrokerJournalInitializeTable(_tableJournal);
      for (int i = 0; i < _reports.Journal.Count; i++) 
        Terminal.BrokerJournalRecordAdd(_tableJournal, _reports.Journal[i]);

      GordagoMain.MainForm.SetContextMenuOnTable(this._tableJournal);
      GordagoMain.MainForm.SetContextMenuOnTable(_tableResult);
    }

    #region private void AddMainRow(string col1, string col2, string col3, string col4, bool save)
    private void AddMainRow(string col1, string col2, string col3, string col4, bool save) {
      ListViewItem lvi = new ListViewItem(col1, 0);
      lvi.SubItems.Add(col2);
      lvi.SubItems.Add(col3);
      lvi.SubItems.Add(col4);
      this._lstCommon.Items.Add(lvi);

      if(save) {
        HTMLWriteTableRow(
          col1 + "\t align='left'",
          col2 + "\t align='right'",
          col3 + "\t align='right'",
          col4 + "\t align='right'");
      }
    }
    #endregion

    #region private void _btnOK_Click(object sender, EventArgs e)
    private void _btnOK_Click(object sender, EventArgs e) {
      this.Close();
    }
    #endregion

    #region private void _cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
    private void _cmbFilter_SelectedIndexChanged(object sender, EventArgs e) {
      this.UpdateResult(false);
    }
    #endregion

    #region private void AddInHDataRow(string caption, string value)
    private void AddInHDataRow(string caption, string value) {
      ListViewItem lvi = new ListViewItem(value);
      lvi.SubItems.Add(caption);
      _lstData.Items.Add(lvi);

      HTMLWriteTableRow(caption + "\t align='left'", value + "\t align='right'");
    }
    #endregion

    #region private void UpdateHistoryInfo()
    private void UpdateHistoryInfo() {
      string caption = this._lblData.Text = Dictionary.GetString(16, 46, "История");

      HTMLOpenTable(caption, false, 0,
        "Name\t width='150' align='center'",
        "Value\t width='150' align='center'");

      //TestHistoryInfo info = _reports.HistoryInfo;
      AddInHDataRow(Dictionary.GetString(16, 3, "Символ"), _reports.SymbolName);
      AddInHDataRow(Dictionary.GetString(16, 4, "Начало периода"), _reports.PeriodFrom.ToShortDateString() + " " + _reports.PeriodFrom.ToShortTimeString());
      AddInHDataRow(Dictionary.GetString(16, 5, "Конец периода"), _reports.PeriodTo.ToShortDateString() + " " + _reports.PeriodTo.ToShortTimeString());
      AddInHDataRow(Dictionary.GetString(16, 63, "Затрачено времени"), _reports.TimeTest.ToLongTimeString());
      string tfData = _reports.TimeFrame == null ? "Ticks" : _reports.TimeFrame.Name;
      AddInHDataRow(Dictionary.GetString(7, 21, "Ticks step (sec.)"), tfData);

      //AddInHDataRow(Dictionary.GetString(16, 6, "Кол-во баров"), SymbolManager.ConvertToCurrencyString(info.CountTick, 0));
      //AddInHDataRow(Dictionary.GetString(16, 62, "Минимальная цена"), SymbolManager.ConvertToCurrencyString(info.MinRate, info.Symbol.DecimalDigits));
      //AddInHDataRow(Dictionary.GetString(16, 61, "Максимальная цена"), SymbolManager.ConvertToCurrencyString(info.MaxRate, info.Symbol.DecimalDigits));
      HTMLCloseTable();
    }
    #endregion

    #region private void UpdateResult()
    private void UpdateResult(bool save) {

      if (save) {
        string caption = Dictionary.GetString(16, 48, "Результат");
        string str1 = Dictionary.GetString(16, 49, "№");
        string str2 = Dictionary.GetString(16, 50, "Время");
        string str3 = Dictionary.GetString(16, 51, "Тип");
        string str4 = Dictionary.GetString(16, 52, "Позиция");
        string str5 = Dictionary.GetString(16, 53, "Ордер");
        string str6 = Dictionary.GetString(16, 54, "Лот");
        string str7 = Dictionary.GetString(16, 55, "Цена");
        string str8 = Dictionary.GetString(16, 56, "Стоп");
        string str9 = Dictionary.GetString(16, 57, "Лимит");
        string str10 = Dictionary.GetString(16, 58, "Прибыль");
        string str11 = Dictionary.GetString(16, 59, "Баланс");
        string str12 = "Account";

        _tableResult.Columns.Clear();
        _tableResult.Columns.Add(str1, TableColumnType.Label, 30);
        _tableResult.Columns.Add(str2, TableColumnType.Label, 120);
        _tableResult.Columns.Add(str3, TableColumnType.Label, 100);
        _tableResult.Columns.Add(str4, TableColumnType.Label, 60);
        _tableResult.Columns.Add(str5, TableColumnType.Label, 60);
        _tableResult.Columns.Add(str6, TableColumnType.Label, 60);
        _tableResult.Columns.Add(str7, TableColumnType.Label, 60);
        _tableResult.Columns.Add(str8, TableColumnType.Label, 60);
        _tableResult.Columns.Add(str9, TableColumnType.Label, 60);
        _tableResult.Columns.Add(str10, TableColumnType.Label, 60);
        _tableResult.Columns.Add(str11, TableColumnType.Label, 90);
        _tableResult.Columns.Add(str12, TableColumnType.Label, 50);

        int w = 0;
        for (int c = 0; c < _tableResult.Columns.Count; c++) {
          w += _tableResult.Columns[c].Width;
        }

        HTMLOpenTable(caption, true, w,
          str1 + "\t align='center' width=" + _tableResult.Columns[0].Width.ToString(),
          str2 + "\t align='center' width=" + _tableResult.Columns[1].Width.ToString(),
          str3 + "\t align='center' width=" + _tableResult.Columns[2].Width.ToString(),
          str4 + "\t align='center' width=" + _tableResult.Columns[3].Width.ToString(),
          str5 + "\t align='center' width=" + _tableResult.Columns[4].Width.ToString(),
          str6 + "\t align='center' width=" + _tableResult.Columns[5].Width.ToString(),
          str7 + "\t align='center' width=" + _tableResult.Columns[6].Width.ToString(),
          str8 + "\t align='center' width=" + _tableResult.Columns[7].Width.ToString(),
          str9 + "\t align='center' width=" + _tableResult.Columns[8].Width.ToString(),
          str10 + "\t align='center' width=" + _tableResult.Columns[9].Width.ToString(),
          str11 + "\t align='center' width=" + _tableResult.Columns[10].Width.ToString(),
          str12 + "\t align='center' width=" + _tableResult.Columns[11].Width.ToString());
      }

      _tableResult.Rows.Clear();

      string filter = "";
      if(this._cmbFilter.SelectedItem != null)
        filter = this._cmbFilter.SelectedItem.ToString().ToLower();

      for (int i = 0; i < _reports.Journal.Count; i++) {
        BrokerJournalRecord rec = _reports.Journal[i];

        bool isorder = false;
        if (rec is BJROrder) {
          BJROrder jorder = rec as BJROrder;
          if (jorder.OrderType == OrderType.EntryLimit || jorder.OrderType == OrderType.EntryStop)
            isorder = true;
        }

        if (rec is BJRTrade || (rec is BJROrder && isorder)) {

          string obj1 = Convert.ToString(_tableResult.Rows.Count + 1);
          string obj2 = rec.Time.ToShortDateString() + " " + rec.Time.ToShortTimeString();
          string obj3 = "", obj4 = "", obj5 = "", obj6 = "";
          string obj7 = "", objStopRate = "", objLimitRate = "", obj10 = "";
          string obj11 = "", obj12 = "";

          TradeType tt = TradeType.Sell;

          bool closeStop = false, closeLimit = false, closeCustom = false;
          if (rec is BJRTrade) {
            BJRTrade jtrade = rec as BJRTrade;
            tt = jtrade.TradeType;
            int decdig = jtrade.SymbolDecimalDigits;


            float closeRate = jtrade.CloseRate;
            float limitRate = jtrade.LimitRate;
            float stopRate = jtrade.StopRate;

            /* Определение критерия закрытия */
            if (jtrade.MessageType == BrokerMessageType.Delete) {
              if (jtrade.TradeType == TradeType.Sell) {
                if (stopRate > 0 && closeRate >= stopRate)
                  closeStop = true;
                if (limitRate > 0 && closeRate <= limitRate)
                  closeLimit = true;
              } else {
                if (stopRate > 0 && closeRate <= stopRate)
                  closeStop = true;
                if (limitRate > 0 && closeRate >= limitRate)
                  closeLimit = true;
              }
            }
            obj4 = jtrade.TradeId;
            obj5 = jtrade.ParentOrderId;
            obj6 = jtrade.Lots.ToString();
            objStopRate = SymbolManager.ConvertToCurrencyString(stopRate, decdig, false);
            objLimitRate = SymbolManager.ConvertToCurrencyString(limitRate, decdig, false);
            obj11 = SymbolManager.ConvertToCurrencyString(jtrade.Balance, 2);
            obj12 = jtrade.AccountId;

            string sb = jtrade.TradeType == TradeType.Sell ? "Sell" : "Buy"; 
            switch (jtrade.MessageType) {
              case BrokerMessageType.Add:
                obj3 = sb;
                obj7 = SymbolManager.ConvertToCurrencyString(jtrade.OpenRate, decdig);
                break;
              case BrokerMessageType.Delete:
                obj3 = "Close " + sb;
                obj7 = SymbolManager.ConvertToCurrencyString(jtrade.CloseRate, decdig);
                obj10 = SymbolManager.ConvertToCurrencyString(jtrade.NetPL, 2, false);
                break;
              case BrokerMessageType.Update:
                obj3 = "Modify";
                break;
            }

          } else {
            BJROrder jorder = rec as BJROrder;
            tt = jorder.TradeType;
            int decdig = jorder.SymbolDecimalDigits;

            obj4 = jorder.OrderId;
            obj5 = jorder.TradeId;
            obj6 = jorder.Lots.ToString();
            objStopRate = SymbolManager.ConvertToCurrencyString(jorder.StopRate, decdig, false);
            objLimitRate = SymbolManager.ConvertToCurrencyString(jorder.LimitRate, decdig, false);
//            obj10 = SymbolManager.ConvertToCurrencyString(jorder.NetPL, 2, false);
            obj11 = SymbolManager.ConvertToCurrencyString(jorder.Balance, 2);
            obj12 = jorder.AccountId;
            obj7 = SymbolManager.ConvertToCurrencyString(jorder.Rate, decdig, false);
            switch (jorder.MessageType) {
              case BrokerMessageType.Add:
                obj3 = "Create " + OrderTypeToString(jorder.OrderType, jorder.TradeType);
                break;
              case BrokerMessageType.Delete:
                obj3 = "Delete " + OrderTypeToString(jorder.OrderType, jorder.TradeType);
                break;
              case BrokerMessageType.Update:
                obj3 = "Modify " + OrderTypeToString(jorder.OrderType, jorder.TradeType);
                break;
            }
          }

          if (filter == "" ||
            (filter == "sell" && tt == TradeType.Sell) ||
            (filter == "buy" && tt == TradeType.Buy)) {

            TableRow row = _tableResult.NewRow();

            int n = 0;
            row[n++].Text = obj1;
            row[n++].Text = obj2;
            row[n++].Text = obj3;
            row[n++].Text = obj4;
            row[n++].Text = obj5;
            row[n++].Text = obj6;
            row[n++].Text = obj7;
            row[n++].Text = objStopRate;
            row[n++].Text = objLimitRate;
            row[n++].Text = obj10;
            row[n++].Text = obj11;
            row[n++].Text = obj12;

            _tableResult.Rows.AddRow(row);
          }

          if (save) {
            HTMLWriteTableRow(
              obj1 + "\t align='right'",
              obj2 + "\t align='right'",
              obj3 + "\t align='right'",
              obj4 + "\t align='right'",
              obj5 + "\t align='right'",
              obj6 + "\t align='right'",
              obj7 + "\t align='right'",
              objStopRate + "\t align='right' " + (closeStop ? "bgcolor='#F9B1B1'" : ""),
              objLimitRate + "\t align='right' " + (closeLimit ? "bgcolor='#D5F2DE'" : ""),
              obj10 + "\t align='right'",
              obj11 + "\t align='right'",
              obj12 + "\t align='right'");
          }
        }
      }
      if(save) {
        HTMLCloseTable();
      }

    }
    #endregion

    #region public static string OrderTypeToString(OrderType orderType, TradeType tradeType)
    public static string OrderTypeToString(OrderType orderType, TradeType tradeType) {
      if (orderType == OrderType.EntryLimit) {
        return tradeType == TradeType.Sell ? "SellLimit" : "BuyLimit";
      } else {
        return tradeType == TradeType.Sell ? "SellStop" : "BuyStop";
      }
    }
    #endregion

    #region private void _btnSave_Click(object sender, EventArgs e)
    private void _btnSave_Click(object sender, EventArgs e) {
      ReportBrowserForm rbf = new ReportBrowserForm(_tmpfilename, "Test");
      rbf.ShowDialog();
    }
    #endregion

    #region private void HTMLOpenTable(string caption, params string[] columns)
    private void HTMLOpenTable(string caption, bool headerVisible, int width, params string[] columns) {
      string ww = width > 0 ? "width="+width.ToString() : "";
      _tw.WriteLine("<table "+ww+" border='0' cellpadding='1' cellspacing='1' bgcolor='#78B5FD'>");
      _tw.WriteLine(string.Format("<tr><td bgcolor='#BBD6F7' colspan='{0}' align='center'><b>{1}</b></td></tr>", columns.Length, caption));
      if(headerVisible) {
        _tw.WriteLine("<tr>");
        for(int i = 0; i < columns.Length; i++) {
          string[] sa = columns[i].Split('\t');
          if(sa.Length < 2)
            sa = new string[] { sa[0], "" };
          _tw.WriteLine(string.Format("<td bgcolor='#E4EFFC'{1}>{0}</td>", sa[0], sa[1]));
        }
        _tw.WriteLine("</tr>");
      }
    }
    #endregion

    #region private void HTMLWriteTableRow(params string[] values)
    private void HTMLWriteTableRow(params string[] values) {
      _tw.WriteLine("<tr bgcolor='#FFFFFF'>");
      for(int i = 0; i < values.Length; i++) {
        string[] sa = values[i].Split('\t');
        if(sa.Length < 2)
          sa = new string[] { sa[0], "" };

        _tw.WriteLine(string.Format("<td {1}>{0}</td>", sa[0], sa[1]));
      }
      _tw.WriteLine("</tr>");
    }
    #endregion

    #region private void HTMLCloseTable()
    private void HTMLCloseTable() {
      _tw.WriteLine("</table>");
    }
    #endregion

    #region protected override void OnClosed(EventArgs e)
    protected override void OnClosed(EventArgs e) {
      base.OnClosed(e);
      if(System.IO.File.Exists(_tmpfilename))
        System.IO.File.Delete(_tmpfilename);
    }
    #endregion
  }
}