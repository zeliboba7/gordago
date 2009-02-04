/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.Analysis {
  class TestTickManager:ITickList {

    private int _indexfrom, _count;
    private int _initIndexFrom;
    
    private ITickList _tmanager;
    
    private TestBars[] _barscollections;

    public TestTickManager(ITickList tmanager, int indexfrom) {
      _tmanager = tmanager;
      _initIndexFrom = _indexfrom = indexfrom;
      _count = 0;
      _barscollections = new TestBars[0];
    }

    #region internal int IndexForm
    internal int IndexForm {
      get { return _indexfrom; }
    }
    #endregion

    #region public int Count
    public int Count {
      get { return _count; }
    }
    #endregion

    #region public Tick this[int index]
    public Tick this[int index] {
      get { return _tmanager[index + _indexfrom]; }
    }
    #endregion

    #region public Tick Current
    public Tick Current {
      get { return _tmanager[_indexfrom + _count - 1]; }
    }
    #endregion

    #region public DateTime TimeFrom
    public DateTime TimeFrom {
      get {
        if(this.Count == 0)
          return new DateTime(1970, 1, 1);
        return new DateTime(this[0].Time); 
      }
    }
    #endregion

    #region public DateTime TimeTo
    public DateTime TimeTo {
      get {
        if(this.Count == 0)
          return new DateTime(1970,1,1);
        return new DateTime(this[this.Count - 1].Time); 
      }
    }
    #endregion

    #region public IBarList GetBarList(int second)
    public IBarList GetBarList(int second) {

      for(int i = 0; i < _barscollections.Length; i++) {
        if(_barscollections[i].TimeFrame.Second == second)
          return _barscollections[i];
      }

      /* Такого таймфрейма нет, необходимо его создать, и занести в него тики */
      TimeFrame tf = TimeFrameManager.TimeFrames.GetTimeFrame(second);
      if(tf == null)
        tf = TimeFrameManager.TimeFrames.CreateNew(second);


      TestBars rettbar = new TestBars(tf);

      for(int i = 0; i <= this.Count; i++) {
        rettbar.Add(this[i]);
      }

      List<TestBars> list = new List<TestBars>(_barscollections);
      list.Add(rettbar);
      _barscollections = list.ToArray();
      return rettbar;
    }
    #endregion

    #region public Tick MoveNext()
    public Tick MoveNext() {
      Tick tick = _tmanager[_indexfrom + _count];
      _count++;
      for(int ii = 0; ii < _barscollections.Length; ii++) {
        _barscollections[ii].Add(tick);
      }
      return tick;
    }
    #endregion

    #region public void Clear()
    public void Clear() {
      _indexfrom  = _initIndexFrom;
      _count = 0;
      _barscollections = new TestBars[0];
    }
    #endregion

    #region public void Add(Tick tick)
    public void Add(Tick tick) {
    }
    #endregion

    #region public IBarList[] BarLists
    public IBarList[] BarLists {
      get { return _barscollections; }
    }
    #endregion
  }
}
