/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Language;

namespace Gordago.Analysis.Chart {
  public partial class TickChartPanel : ChartPanel {
    public TickChartPanel() {
      InitializeComponent();
      this.Text = Dictionary.GetString(32, 30, "Tick Chart");
    }

    #region public ISymbol Symbol
    public ISymbol Symbol {
      get { return this._chartTicksManager.Symbol; }
      set { this._chartTicksManager.Symbol = value; }
    }
    #endregion

    #region public ChartTicksManager ChartTicksManager
    public ChartTicksManager ChartTicksManager {
      get { return _chartTicksManager; }
      set { _chartTicksManager = value; }
    }
    #endregion
  }

  
}

