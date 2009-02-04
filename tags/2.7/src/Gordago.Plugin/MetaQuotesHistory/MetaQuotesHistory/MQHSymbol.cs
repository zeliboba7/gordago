using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Gordago.PlugIn.MetaQuotesHistory {
  public class MQHSymbol:ISymbol {

    private ISymbol _gSymbol;
    private MQHTickList _ticks;

    public MQHSymbol(ISymbol gSymbol) {
      _gSymbol = gSymbol;
      _ticks = new MQHTickList(gSymbol.Ticks);
    }

    #region public string Name
    public string Name {
      get { return _gSymbol.Name; }
    }
    #endregion

    #region public float Point
    public float Point {
      get { return _gSymbol.Point; }
    }
    #endregion

    #region public int DecimalDigits
    public int DecimalDigits {
      get { return _gSymbol.DecimalDigits; }
    }
    #endregion

    #region public ITickList Ticks
    public ITickList Ticks {
      get { return _ticks; }
      set { _ticks = value as MQHTickList; }
    }
    #endregion
  }
}
