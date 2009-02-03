/**
* @version $Id: IndicatorsManager.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.FO.Instruments
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using Gordago.Trader;
  using System.Reflection;

  class IndicatorsManager:IEnumerable<IndicatorItem> {

    private readonly List<IndicatorItem> _indicators = new List<IndicatorItem>();

    public IndicatorsManager() {
      this.AddIndicators(typeof(Indicator).Assembly);
    }

    #region public int Count
    public int Count {
      get { return _indicators.Count; }
    }
    #endregion

    #region public IndicatorItem this[int index]
    public IndicatorItem this[int index] {
      get { return _indicators[index]; }
    }
    #endregion

    #region public void AddIndicators(Assembly assembly)
    public void AddIndicators(Assembly assembly) {
      Type[] types = assembly.GetTypes();
      
      foreach (Type type in types) {
        if (type.BaseType == typeof(Indicator) && type.IsPublic) {
          _indicators.Add(new IndicatorItem(type));
        }
      }
    }
    #endregion

    #region public IEnumerator<IndicatorItem> GetEnumerator()
    public IEnumerator<IndicatorItem> GetEnumerator() {
      return _indicators.GetEnumerator();
    }
    #endregion

    #region System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
      throw new Exception("The method or operation is not implemented.");
    }
    #endregion
  }
}
