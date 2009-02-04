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

namespace Gordago.Strategy {
  partial class CustomFuntionControl : UserControl {

    public static readonly string DEF_FUNC_NAME = "Func";
    private CustomIndicatorForm _indicatorForm;

    #region public CustomFuntionControl(CustomIndicatorForm indicatorForm)
    public CustomFuntionControl(CustomIndicatorForm indicatorForm):this() {
      _indicatorForm = indicatorForm;
    }
    #endregion

    public CustomFuntionControl() {
      InitializeComponent();
    }

    #region public FunctionBilderControl Bilder
    public FunctionBilderControl Bilder {
      get { return this._bilder; }
    }
    #endregion

    #region public string FunctionName
    public string FunctionName {
      get { return _txtName.Text; }
      set { 
        this._txtName.Text = value;
      }
    }
    #endregion

    #region private void _txtName_TextChanged(object sender, EventArgs e)
    private void _txtName_TextChanged(object sender, EventArgs e) {
      this.UpdateName();
    }
    #endregion

    #region public void UpdateName()
    public void UpdateName() {
      if (this.Parent == null) return;
      this.Parent.Text = _indicatorForm.IndicatorName + "_" + this.FunctionName;
    }
    #endregion
  }
}
