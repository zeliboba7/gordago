/**
* @version $Id: TicksMapItem.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.Trader.Data {
  class TicksMapItem {
    private long _time;
    private int _counttick;

    public TicksMapItem(long time, int counttick) {
      _time = time;
      _counttick = counttick;
    }

    #region public long Time
    public long Time {
      get { return this._time; }
      set { this._time = value; }
    }
    #endregion

    #region public int CountTick
    public int CountTick {
      get { return this._counttick; }
      set { this._counttick = value; }
    }
    #endregion
  }
}
