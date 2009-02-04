/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;

namespace Gordago {
	class Symbol: ISymbol {

		private string _name;
		private int _decimaldigits = -1;
		private float _point;

		private ITickList _tickmanager;

		private string _dirhistory, _dircache;

		public Symbol(string name, int decimaldigits){
			_name = name;
			_decimaldigits = decimaldigits;
      this._point = 1F / SymbolManager.GetDelimiter(decimaldigits);
			_tickmanager = new TickManager(this);
		}

		#region public int DecimalDigits
		public int DecimalDigits{
			get{
#if DEBUG
				if (_decimaldigits == 0)
					throw(new Exception(string.Format("DecimalDigits in {0} is 0!", this._name)));
#endif

				return this._decimaldigits;
			}
		}
		#endregion

		#region public float Point
		public float Point{
			get{return this._point;}
		}
		#endregion

		#region public string Name
		public string Name{
			get{return this._name;}
		}
		#endregion

    #region public ITickList Ticks
    public ITickList Ticks {
      get { return this._tickmanager; }
      set {
        if (value == null)
          throw(new Exception("ITickList can not be null"));
        this._tickmanager = value;
      }
    }
    #endregion

    #region internal string DirHistory
    internal string DirHistory{
			get{return this._dirhistory;}
			set{this._dirhistory = value;}
		}
		#endregion

		#region internal string DirCache
		internal string DirCache{
			get{return this._dircache;}
			set{this._dircache = value;}
		}
		#endregion

		#region public override string ToString() 
		public override string ToString() {
			return this._name;
		}
		#endregion

    #region public float Bid
    public float Bid {
      get { return float.NaN;}
    }
    #endregion

    #region public float Ask
    public float Ask {
      get { return float.NaN; }
    }
    #endregion
  }
}
