/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Gordago.Analysis {

  #region Sample
  /// <summary>
  ///       Gordago.Analysis.TradeVariables vars = new Gordago.Analysis.TradeVariables();
      //vars.Add(new Gordago.Analysis.TradeVarInt("var0", 0, 1, 1));
      //vars.Add(new Gordago.Analysis.TradeVarInt("var1", 0, 1, 1));
      //vars.Add(new Gordago.Analysis.TradeVarInt(0, 1, 1));
      //vars.Add(new Gordago.Analysis.TradeVarInt(0, 1, 1));
      //int countvar = 1;
      //for(int i = 0; i < vars.Count; i++) {
      //  countvar *= vars[i].GetTradeVarEnumerator().Count;
      //}
      //System.Collections.Generic.Dictionary<string, string> dict = new System.Collections.Generic.Dictionary<string, string>();
      //int counttest = 0;

      //while(true) {

      //  for(int i = 0; i < vars.Count; i++) {
      //    Gordago.Analysis.ITradeVarEnumerator varenum = vars[i].GetTradeVarEnumerator();

      //    /* проход по всем значения текущего массива
      //     * Выбор значения и перебор всех остальных значений*/
      //    do {

      //      /* проход по всем остальным значениям */
      //      for(int iii = 0; iii < vars.Count; iii++) {

      //        if(i != iii) {

      //          Gordago.Analysis.ITradeVar varview = vars[iii];
      //          Gordago.Analysis.ITradeVarEnumerator varViewEnum = varview.GetTradeVarEnumerator();
      //          do {
      //            string[] sa = new string[vars.Count];
      //            for(int snum = 0; snum < vars.Count; snum++) {
      //              sa[snum] = ((int)(vars[snum] as Gordago.Analysis.TradeVarInt)).ToString();
      //            }

      //              string id = string.Format("({0})", string.Join(", ", sa));
      //            if(!dict.ContainsKey(id)) {
      //            System.Diagnostics.Debug.WriteLine(id);
      //              dict.Add(id, id);
      //              counttest++;
      //            }
      //          } while(varViewEnum.MoveNext());
      //          varViewEnum.MoveFirst();
      //        }
      //      }
      //    } while(varenum.MoveNext());
      //    varenum.MoveFirst();
      //  }

      //  bool isdowncount = false;
      //  for(int i = 0; i < vars.Count; i++) {
      //    Gordago.Analysis.ITradeVarEnumerator tvenum = vars[i].GetTradeVarEnumerator();
      //    if(tvenum.Count > 1) {
      //      tvenum.DownCount();
      //      isdowncount = true;
      //      break;
      //    }
      //  }
      //  if(!isdowncount) {
      //    break;
      //  }
      //}
      
      //System.Diagnostics.Debug.WriteLine(countvar.ToString() + " = " + counttest.ToString());
      //if(vars != null) return;
  /// </summary>
  #endregion

  public class TradeVariables: ICloneable {
    private ITradeVar[] _tvars;
    private Dictionary<string, string> _dict;

    public TradeVariables() {
      _tvars = new ITradeVar[] { };
      _dict = new Dictionary<string, string>();
    }

    #region public int Count
    public int Count {
      get { return this._tvars.Length; }
    }
    #endregion

    #region public ISParam this[int index]
    public ITradeVar this[int index] {
      get { return this._tvars[index]; }
    }
    #endregion

    #region public void Add(ITradeVar tv)
    public void Add(ITradeVar tv) {
      foreach(ITradeVar tvfind in _tvars) {
        if(tvfind.Name == tv.Name) return;
      }

      List<ITradeVar> lst = new List<ITradeVar>(_tvars);
      lst.Add(tv);
      _tvars = lst.ToArray();
    }
    #endregion

    #region public void Add(Strategy strategy)
    public void Add(Strategy strategy) {
      PropertyInfo[] pis = strategy.GetType().GetProperties();
      foreach(PropertyInfo pi in pis) {
        object o = pi.GetValue(strategy, null);
        if (o is ITradeVar) {
          ITradeVar tv = o as ITradeVar;
          
          this.Add(o as ITradeVar);
        }
      }
    }
    #endregion

    #region public void Reset()
    public void Reset() {
      _dict = new Dictionary<string, string>();
      for(int i = 0; i < _tvars.Length; i++) {
        _tvars[i].GetTradeVarEnumerator().Reset();
      }
    }
    #endregion

    #region public int GetCount()
    public int GetCount() {
      this.Reset();
      int cnt = 0;
      while(this.MoveNext()) {
        cnt++;
        if (cnt > 10000)
          break;
      }
      this.Reset();
      return cnt;
    }
    #endregion

    #region public bool MoveNext()
    public bool MoveNext() {
      int cntstep = 0;
      while(true) {
        if (GordagoMain.IsCloseProgram) return false;

        for(int i = 0; i < this.Count; i++) {
          Gordago.Analysis.ITradeVarEnumerator varenum = _tvars[i].GetTradeVarEnumerator();

          cntstep =0;

          /* проход по всем значения текущего массива
           * Выбор значения и перебор всех остальных значений*/
          do {
            cntstep++;
            if (cntstep > 1000 || GordagoMain.IsCloseProgram) {
              return false;
            }
            /* проход по всем остальным значениям */
            for(int iii = 0; iii < this.Count; iii++) {

              if(i != iii || (i==0 && iii==0 && this.Count == 1)) {

                Gordago.Analysis.ITradeVar varview = _tvars[iii];
                Gordago.Analysis.ITradeVarEnumerator varViewEnum = varview.GetTradeVarEnumerator();
                do {
                  string[] sa = new string[this.Count];
                  for(int snum = 0; snum < this.Count; snum++) 
                    sa[snum] = _tvars[snum].ToString();

                  string id = string.Format("({0})", string.Join(", ", sa));
                  if(!_dict.ContainsKey(id)) {
                    _dict.Add(id, id);
                    return true;
                  }
                } while(varViewEnum.MoveNext());
                varViewEnum.MoveFirst();
              }
            }
          } while(varenum.MoveNext());
          varenum.MoveFirst();
        }

        bool isdowncount = false;
        for(int i = 0; i < this.Count; i++) {
          Gordago.Analysis.ITradeVarEnumerator tvenum = _tvars[i].GetTradeVarEnumerator();
          if(tvenum.Count > 1) {
            tvenum.DownCount();
            isdowncount = true;
            break;
          }
        }
        if(!isdowncount) {
          break;
        }
      }
      return false;
    }
    #endregion

    #region public object Clone()
    public object Clone() {
      TradeVariables vars = new TradeVariables();

      for(int i = 0; i < this.Count; i++) {
        vars.Add(this[i].Clone() as ITradeVar);
      }
      return vars;
    }
    #endregion
  }
}
