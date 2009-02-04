/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;
using Gordago.Analysis.Chart;
using System.Drawing;
using System.ComponentModel;

namespace Gordago.API {
  public class HOCPProperties:ChartPanelProperties {

    public HandOperateChartPanel _hocp;

    public HOCPProperties(ChartPanel chartPanel) : base(chartPanel) {
      _hocp = chartPanel as HandOperateChartPanel;
    }

    #region public bool OneClick
    [Category("Main"), DisplayName("One Click")]
    public bool OneClick {
      get { return _hocp.OneClick; }
      set { this._hocp.OneClick = value; }
    }
    #endregion

    #region public bool AllProperties
    [Category("Main"), DisplayName("All Properties")]
    public bool AllProperties {
      get { return this._hocp.AllProperty; }
      set { this._hocp.AllProperty = value; }
    }
    #endregion

    #region public Color BidAskForeColor
    [Category("Style"), DisplayName("BidAsk Fore Color")]
    public Color BidAskForeColor {
      get { return this._hocp.BidAskForeColor; }
      set { this._hocp.BidAskForeColor = value; }
    }
    #endregion

    #region public Color BidAskBackColor
    [Category("Style"), DisplayName("BidAsk Back Color")]
    public Color BidAskBackColor {
      get { return this._hocp.BidAskBackColor; }
      set {this._hocp.BidAskBackColor = value;}
    }
    #endregion

    #region public Color SellForeColor
    [Category("Style"), DisplayName("Sell Fore Color")]
    public Color SellForeColor {
      get { return this._hocp.SellForeColor; }
      set { this._hocp.SellForeColor = value; }
    }
    #endregion

    #region public Color SellBackColor
    [Category("Style"), DisplayName("Sell Back Color")]
    public Color SellBackColor {
      get { return this._hocp.SellBackColor; }
      set { this._hocp.SellBackColor = value; }
    }
    #endregion

    #region public Color SellBorderColor
    [Category("Style"), DisplayName("Sell Border Color")]
    public Color SellBorderColor {
      get { return this._hocp.SellBorderColor; }
      set { this._hocp.SellBorderColor = value; }
    }
    #endregion

    #region public Color BuyForeColor
    [Category("Style"), DisplayName("Buy Fore Color")]
    public Color BuyForeColor {
      get { return this._hocp.BuyForeColor; }
      set { this._hocp.BuyForeColor = value; }
    }
    #endregion

    #region public Color BuyBackColor
    [Category("Style"), DisplayName("Buy Back Color")]
    public Color BuyBackColor {
      get { return this._hocp.BuyBackColor; }
      set { this._hocp.BuyBackColor = value; }
    }
    #endregion

    #region public Color BuyBorderColor
    [Category("Style"), DisplayName("Buy Border Color")]
    public Color BuyBorderColor {
      get { return this._hocp.BuyBorderColor; }
      set { this._hocp.BuyBorderColor = value; }
    }
    #endregion

    protected override void OnSaveTemplate(XmlNodeManager nodeManager) {
      base.OnSaveTemplate(nodeManager);
      nodeManager.SetAttribute("OneClick", this.OneClick);
      nodeManager.SetAttribute("AllProp", this.AllProperties);
      nodeManager.SetAttribute("BAForeClr", this.BidAskForeColor);
      nodeManager.SetAttribute("BABackClr", this.BidAskBackColor);

      nodeManager.SetAttribute("SellForeClr", this.SellForeColor);
      nodeManager.SetAttribute("SellBackClr", this.SellBackColor);
      nodeManager.SetAttribute("SellBorderClr", this.SellBorderColor);

      nodeManager.SetAttribute("BuyForeClr", this.BuyForeColor);
      nodeManager.SetAttribute("BuyBackClr", this.BuyBackColor);
      nodeManager.SetAttribute("BuyBorderClr", this.BuyBorderColor);
    }

    protected override void OnLoadTemplate(XmlNodeManager nodeManager) {

      this.OneClick = nodeManager.GetAttributeBoolean("OneClick", this.OneClick);
      
      this.AllProperties = nodeManager.GetAttributeBoolean("AllProp", this.AllProperties);

      this.BidAskForeColor = nodeManager.GetAttributeColor("BAForeClr", this.BidAskForeColor);
      this.BidAskBackColor = nodeManager.GetAttributeColor("BABackClr", this.BidAskBackColor);

      this.SellForeColor = nodeManager.GetAttributeColor("SellForeClr", this.SellForeColor);
      this.SellBackColor = nodeManager.GetAttributeColor("SellBackClr", this.SellBackColor);
      this.SellBorderColor = nodeManager.GetAttributeColor("SellBorderClr", this.SellBorderColor);

      this.BuyForeColor = nodeManager.GetAttributeColor("BuyForeClr", this.BuyForeColor);
      this.BuyBackColor = nodeManager.GetAttributeColor("BuyBackClr", this.BuyBackColor);
      this.BuyBorderColor = nodeManager.GetAttributeColor("BuyBorderClr", this.BuyBorderColor);
      base.OnLoadTemplate(nodeManager);
    }
  }
}
