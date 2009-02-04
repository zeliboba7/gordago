/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;

namespace Gordago.Strategy {
  public delegate void TBOChangeWidthEventHandler(object sender, TBOChangeWidthEventArgs e);

  public class TBOChangeWidthEventArgs:System.EventArgs {
    private int allControlsWidth;

    public TBOChangeWidthEventArgs(int AllCtrlsWidth) {
      this.allControlsWidth = AllCtrlsWidth;
    }

    public int AllControlsWidth {
      get { return allControlsWidth; }
    }
  }
}
