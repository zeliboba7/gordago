using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.PlugIn.Clock {
  class CountryTime {
    private string _name;
    private int _gmt;
    
    public CountryTime(string name, int gmt) {
      _name = name;
      _gmt = gmt;
    }

    #region public string Name
    /// <summary>
    /// Наименование часового пояса
    /// </summary>
    public string Name {
      get { return this._name; }
    }
    #endregion

    #region public int GMT
    /// <summary>
    /// Разница по гринвичу
    /// </summary>
    public int GMT {
      get { return this._gmt; }
    }
    #endregion
  }
}
