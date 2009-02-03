/**
* @version $Id: ChartControl.ChartPanelCollection.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Charting
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.ComponentModel;
  using System.ComponentModel.Design;
  using System.Drawing.Design;
  using System.Drawing;

  partial class ChartControl {

    public event EventHandler SelectedIndexChanged;

    private int _selectedIndex = -1;

    private int _savedWidth = -1, _savedHeight = -1, _savedChartBoxCount = 0;

    private readonly ChartPanelCollection _chartPanelCollection;
    private readonly List<ChartPanel> _listChartPanel = new List<ChartPanel>();
    private readonly List<ChartBox> _boxes = new List<ChartBox>();
    private readonly List<string> _boxesLayout = new List<string>();

    #region public ChartPanelCollection ChartPanels
    //[Category("Behavior")]
    //[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    //[Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
    public ChartPanelCollection ChartPanels {
      get { return _chartPanelCollection; ; }
    }
    #endregion

    #region public int SelectedIndex
    /// <summary>
    /// Gets or sets the index of the currently selected chart panel.
    /// </summary>
    /// <returns>
    /// The zero-based index of the currently selected chart panel. The default is -1, which is also the value if no tab page is selected
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">The value is less than -1. </exception>
    [Browsable(false), DefaultValue(-1)]
    public int SelectedIndex {
      get { return _selectedIndex; }
      set {
        if (value < -1) {
          throw new ArgumentOutOfRangeException("SelectedIndex");
        }
        if (_selectedIndex != value) {
          this.OnSelectedIndexChanged();
        }
      }
    }

    #endregion

    #region public ChartPanel SelectedChartPanel
    /// <summary>
    /// Gets or sets the currently selected chart box.
    /// </summary>
    /// <returns>A <see cref="ChartBox"/> that represents the selected chart box. If no chart box is selected, the value is null.</returns>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
    public ChartPanel SelectedChartPanel {
      get {
        if (_selectedIndex < 0)
          return null;
        return _listChartPanel[_selectedIndex];
      }
      set {
        int num = this.FindChartPanel(value);
        this.SelectedIndex = num;
      }
    }
    #endregion

    #region protected virtual void OnSelectedIndexChanged()
    protected virtual void OnSelectedIndexChanged() {
      if (this.SelectedIndexChanged != null) {
        this.SelectedIndexChanged(this, EventArgs.Empty);
        this.Invalidate();
      }
    }
    #endregion

    #region private void InitChartPanel(ChartBox chartBox)
    private void InitChartPanel(ChartPanel chartPanel) {
      chartPanel.SetOwner(this);
      this.SelectedIndex = -1;
      this.ChartPanels.SyncTabIndex();
      this.OnChartPanelAdded(new ChartPanelEventArgs(chartPanel));
      this.Invalidate();
    }
    #endregion

    #region private ChartPanel[] GetChartPanels(Type type)
    private ChartPanel[] GetChartPanels(Type type) {
      List<ChartPanel> list = new List<ChartPanel>();
      for (int i = 0; i < _listChartPanel.Count; i++) {
        ChartPanel panel = _listChartPanel[i];
        if (panel.GetType() == type) {
          list.Add(panel);
        }
      }
      return list.ToArray();
    }
    #endregion

    #region private void SyncronyzedChartBoxesCollection()
    private void SyncronyzedChartBoxesCollection() {
      _boxes.Clear();
      ChartPanel[] panels = this.GetChartPanels(typeof(ChartBox));
      foreach (ChartPanel panel in panels) {
        _boxes.Add(panel as ChartBox);
      }
    }
    #endregion

    #region protected override void OnResize(EventArgs e)
    protected override void OnResize(EventArgs e) {
      this.OnChartBoxesLayout();
      base.OnResize(e);
    }
    #endregion

    #region internal void ChartBoxesLayout()
    internal void ChartBoxesLayout() {
      _savedHeight = 0;
      this.OnChartBoxesLayout();
    }
    #endregion

    #region private bool CheckChartBoxLayout(ChartBox box)
    private bool CheckChartBoxLayout(ChartBox box) {
      for (int i = 0; i < _boxesLayout.Count; i++) {
        if (box.GUID == _boxesLayout[i]) {
          return true;
        }
      }
      return false;
    }
    #endregion

    #region protected virtual void OnChartBoxesLayout()
    protected virtual void OnChartBoxesLayout() {
      if (_savedWidth == this.Width && _savedHeight == this.Height && _savedChartBoxCount == _listChartPanel.Count)
        return;

      if (_boxes.Count == 1) {
        _boxes[0].SetBounds(0, 0, this.Width, this.Height);
      } else if (_boxes.Count > 1) {

        if (_savedChartBoxCount < _boxes.Count) {

          float added = this.Height / _boxes.Count;
          
          float removed = added*0.15F;

          foreach (ChartBox cbox in _boxes) {
            if (!CheckChartBoxLayout(cbox)) {
              cbox.Height = (int)added;
            } else {
              cbox.Height -= (int)removed;
            }
          }
        }

        /* распределение процентов в случае удалени или добавления бокса */
        int total = 0;
        foreach (ChartBox cbox in _boxes) {
          total += cbox.Height;
        }
        int div = (this.Height - total) / _boxes.Count;

        _boxesLayout.Clear();
        int y = 0, i = 0;
        foreach (ChartBox cbox in _boxes) {
          cbox.HorizontalScale.Visible = i == this._boxes.Count - 1;

          //cbox.Height += div;
          //int h = Convert.ToInt32((float)this.Height / 100 * cbox.HeightPercent);

          int h = cbox.Height + div;
          cbox.SetBounds(0, y, this.Width, h);
          y += cbox.Height;

          cbox.SplitBottom = false;
          cbox.SplitTop = false;
          if (_boxes.Count > 1) {
            if (i < _boxes.Count-1) 
              cbox.SplitBottom = true;
            if (i > 0)
              cbox.SplitTop = true;
          }
          _boxesLayout.Add(cbox.GUID);
          i++;
        }
      }
      _savedWidth = this.Width;
      _savedHeight = this.Height;
      _savedChartBoxCount = this._boxes.Count;

      this.Invalidate();
    }
    #endregion

    #region public ChartPanel GetChartPanel(Point p)
    public ChartPanel GetChartPanel(Point p) {
      for (int i = 0; i < _chartPanelCollection.Count; i++) {
        ChartPanel panel = _chartPanelCollection[i];
        if (panel.Bounds.Contains(p))
          return panel;  
      }
      return null;
    }
    #endregion

    #region public class ChartPanelCollection:IList<ChartPanel>
    #region internal int FindChartPanel(ChartPanel panel)
    internal int FindChartPanel(ChartPanel panel) {
      for (int i = 0; i < _listChartPanel.Count; i++) {
        if (_listChartPanel.Equals(panel))
          return i;
      }
      return -1;
    }
        #endregion

    #region internal void Insert(int index, ChartPanel item)
    internal void Insert(int index, ChartPanel item) {
      _listChartPanel.Insert(index, item);
      SyncronyzedChartBoxesCollection();
      this.ChartPanels.SyncTabIndex();
      this.InitChartPanel(item);
    }
    #endregion

    #region internal void RemoveAt(int index)
    internal void RemoveAt(int index) {
      ChartPanel chartPanel = _listChartPanel[index];
      this.Remove(chartPanel);
    }
    #endregion

    #region internal void Add(ChartPanel item)
    internal void Add(ChartPanel item) {
      _listChartPanel.Add(item);
      SyncronyzedChartBoxesCollection();
      this.InitChartPanel(item);
    }
    #endregion

    #region internal void Clear()
    internal void Clear() {
      try {
        while (_listChartPanel.Count != 0) {
          this.RemoveAt(_listChartPanel.Count - 1);
        }
      } catch { }
    }
    #endregion

    #region internal bool Remove(ChartPanel item)
    internal bool Remove(ChartPanel item) {
      bool ret = _listChartPanel.Remove(item);
      SyncronyzedChartBoxesCollection();
      this.SelectedIndex = -1;
      this.ChartPanels.SyncTabIndex();
      this.OnChartPanelRemoved(new ChartPanelEventArgs(item));
      return ret;
    }
    #endregion

    #region protected virtual void OnChartPanelRemoved(ChartPanelEventArgs e)
    protected virtual void OnChartPanelRemoved(ChartPanelEventArgs e) {
      this.OnChartBoxesLayout();
      if (this.ChartPanelRemoved != null)
        this.ChartPanelRemoved(this, e);
    }
    #endregion

    #region protected virtual void OnChartPanelAdded(ChartPanelEventArgs e)
    protected virtual void OnChartPanelAdded(ChartPanelEventArgs e) {
      this.OnChartBoxesLayout();
      if (this.ChartPanelAdded != null) {
        this.ChartPanelAdded(this, e);
      }
    }
    #endregion

    public class ChartPanelCollection : IList<ChartPanel> {

      private readonly ChartControl _owner;

      internal ChartPanelCollection(ChartControl owner) {
        _owner = owner;
      }

      #region public ChartPanel this[int index]
      public ChartPanel this[int index] {
        get {
          return this._owner._listChartPanel[index];
        }
        set { }
      }
      #endregion

      #region public int Count
      public int Count {
        get { return _owner._listChartPanel.Count; }
      }
      #endregion

      #region public bool IsReadOnly
      public bool IsReadOnly {
        get { return false; }
      }
      #endregion

      #region public int IndexOf(ChartPanel item)
      public int IndexOf(ChartPanel item) {
        return this._owner._listChartPanel.IndexOf(item);
      }
      #endregion

      #region public bool Contains(ChartPanel item)
      public bool Contains(ChartPanel item) {
        return _owner._listChartPanel.Contains(item);
      }
      #endregion

      #region public void CopyTo(ChartPanel[] array, int arrayIndex)
      public void CopyTo(ChartPanel[] array, int arrayIndex) {
        _owner._listChartPanel.CopyTo(array, arrayIndex);
      }
      #endregion

      #region public IEnumerator<ChartPanel> GetEnumerator()
      public IEnumerator<ChartPanel> GetEnumerator() {
        return _owner._listChartPanel.GetEnumerator();
      }
      #endregion

      #region System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
      System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
        return _owner._listChartPanel.GetEnumerator();
      }
      #endregion

      #region public void Insert(int index, ChartPanel item)
      public void Insert(int index, ChartPanel item) {
        _owner.Insert(index, item);
      }
      #endregion

      #region public void RemoveAt(int index)
      public void RemoveAt(int index) {
        _owner.RemoveAt(index);
      }
      #endregion

      #region public void Add(ChartPanel item)
      public void Add(ChartPanel item) {
        _owner.Add(item);
      }
      #endregion

      #region public void Clear()
      public void Clear() {
        _owner.Clear();
      }
      #endregion

      #region public bool Remove(ChartPanel item)
      public bool Remove(ChartPanel item) {
        return _owner.Remove(item);
      }
      #endregion

      #region private static int CompareByTabIndex(ChartPanel panel1, ChartPanel panel2)
      private static int CompareByTabIndex(ChartPanel panel1, ChartPanel panel2) {
        return panel1.TabIndex.CompareTo(panel2.TabIndex);
      }
      #endregion

      #region internal void Sort()
      internal void Sort() {
        _owner._listChartPanel.Sort(CompareByTabIndex);
        this.SyncTabIndex();
        this._owner._savedHeight = 0;
        this._owner.ChartBoxesLayout();
      }
      #endregion

      #region internal void SyncTabIndex()
      internal void SyncTabIndex() {
        for (int i = 0; i < this.Count; i++) {
          this[i].NativeTabIndex = i;
        }
        this._owner.SyncronyzedChartBoxesCollection();
      }
      #endregion
    }
    #endregion
  }
}
