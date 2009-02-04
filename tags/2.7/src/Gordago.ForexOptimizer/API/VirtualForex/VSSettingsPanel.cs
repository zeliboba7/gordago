/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Cursit.Applic.AConfig;
using Language;

namespace Gordago.API.VirtualForex {

  interface IVirtualBrokerSettings {
    TimeFrame UseTimeFrame{get;}

    float Deposit { get;}

    float Commission { get;}

    int LeverageValue { get;}

    int LotSize { get;}
  }

  partial class VSSettingsPanel : UserControl, IVirtualBrokerSettings {

    #region class Leverage
    class Leverage {
      private string _name;
      private int _value;
      public Leverage(int value, string name) {
        _name = name;
        _value = value;
      }

      #region public string Name
      public string Name {
        get { return _name; }
      }
      #endregion

      #region public int Value
      public int Value {
        get { return _value; }
      }
      #endregion

      #region public override string ToString()
      public override string ToString() {
        return _name;
      }
      #endregion
    }
    #endregion

    #region class LotSize
    class LotSizeValue {
      private int _lotSize;
      public LotSizeValue(int lotSize) {
        _lotSize = lotSize;
      }
      public int Size {
        get { return this._lotSize; }
      }

      public override string ToString() {
        return SymbolManager.ConvertToCurrencyString(_lotSize, 0, " ");
      }
    }
    #endregion

    #region class TimeFrameComboItem
    class TimeFrameComboItem {
      private string _text;
      private TimeFrame _tf;

      public TimeFrameComboItem(TimeFrame tf, string text) {
        _tf = tf;
        _text = text;
      }

      #region public TimeFrame TimeFrame
      public TimeFrame TimeFrame {
        get { return _tf; }
      }
      #endregion

      #region public string Text
      public string Text {
        get { return _text; }
      }
      #endregion

      #region public override string ToString()
      public override string ToString() {
        return _text;
      }
      #endregion
    }
    #endregion

    private ConfigValue _cfgval;

    public VSSettingsPanel() {
      InitializeComponent();

      Leverage l;
      Leverage[] ls = new Leverage[]{
        new Leverage(400, "1:400"),
        l = new Leverage(200, "1:200"),
        new Leverage(100, "1:100"),
        new Leverage(50, "1:50")
      };

      this._cmbLeverage.Items.AddRange(ls);

      List<TimeFrameComboItem> tfList = new List<TimeFrameComboItem>();
      TimeFrameComboItem tfItemTicks = new TimeFrameComboItem(null, "Ticks");
      tfList.Add(tfItemTicks);
      for (int i = 0; i < TimeFrameManager.TimeFrames.Count; i++) {
        TimeFrame tf = TimeFrameManager.TimeFrames[i];
        tfList.Add(new TimeFrameComboItem(tf, tf.Name));
      }
      this._cmbUseTimeFrame.Items.AddRange(tfList.ToArray());
      _cmbUseTimeFrame.SelectedIndex = 0;
      try {
        _cfgval = Config.Users["Tester"];

        _cmbLeverage.SelectedIndex = _cfgval["Leverage", 2];
        _nudBalance.Value = _cfgval["Deposit", 10000];
        decimal cms = 1.25M;
        string newValue = GordagoMain.GetNormalizeNumbler(_cfgval["Commission", cms.ToString()]);
        _nudCommission.Value = Convert.ToDecimal(newValue);

        _cmbLotSize.Items.AddRange(new LotSizeValue[] { new LotSizeValue(10000), new LotSizeValue(100000) });
        _cmbLotSize.SelectedIndex = _cfgval["LotSize", 0];

        _cmbUseTimeFrame.SelectedIndex = _cfgval["UseTF", 0];


        _lblDeposit.Text = Dictionary.GetString(31, 15, "Deposit");
        _lblCommission.Text = Dictionary.GetString(31, 16, "Commission");
        _lblLotSize.Text = Dictionary.GetString(31, 17, "Lot Size");
        _lblLeverage.Text = Dictionary.GetString(31, 18, "Leverage");
        _lblUseTimeFrame.Text = Dictionary.GetString(31, 19, "Use data");
      } catch { }
    }

    public TimeFrame UseTimeFrame {
      get {
        TimeFrameComboItem tfci = this._cmbUseTimeFrame.SelectedItem as TimeFrameComboItem;
        return tfci.TimeFrame;
      }
    }

    #region public float Deposit
    public float Deposit {
      get { return Convert.ToSingle(this._nudBalance.Value); }
    }
    #endregion

    #region public float Commission
    public float Commission {
      get { return Convert.ToSingle(_nudCommission.Value); }
    }
    #endregion

    #region public int Leverage
    public int LeverageValue {
      get {
        if (_cmbLeverage.SelectedValue == null)
          return 0;
        return (_cmbLeverage.SelectedValue as Leverage).Value;
      }
    }
    #endregion

    #region public int LotSize
    public int LotSize {
      get {
        if (_cmbLotSize.SelectedItem == null)
          return 10000;
        LotSizeValue lotSize = _cmbLotSize.SelectedItem as LotSizeValue;
        return lotSize.Size;
      }
    }
    #endregion

    public void Save() {
      _cfgval["LotSize"].SetValue(_cmbLotSize.SelectedIndex);
      _cfgval["Leverage"].SetValue(_cmbLeverage.SelectedIndex);
      _cfgval["Deposit"].SetValue(Convert.ToInt32(_nudBalance.Value));
      _cfgval["Commission"].SetValue(_nudCommission.Value.ToString());
      _cfgval["UseTF"].SetValue(_cmbUseTimeFrame.SelectedIndex);
    }

  }
}
