/**
* @version $Id: Chart.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.IOTicks.Charting
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Data;
  using System.Drawing;
  using System.Text;
  using System.Windows.Forms;
    using Gordago.Trader;

  public partial class Chart : Form {

    private readonly ISymbol _symbol;

    public Chart(ISymbol symbol) {
      InitializeComponent();
      _symbol = symbol;
    }

    protected override void OnLoad(EventArgs e) {
      base.OnLoad(e);
      _chartControl.Symbol = _symbol;
    }


  }
}