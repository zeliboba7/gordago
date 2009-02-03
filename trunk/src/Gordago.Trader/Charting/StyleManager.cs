/**
* @version $Id: StyleManager.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Charting
{
  using System;
  using System.ComponentModel;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Text;
  using System.ComponentModel.Design;
  using System.Drawing.Design;

  public partial class StyleManager : Component {

    private List<Style> _styles = new List<Style>();
    private Style _default = null;
    private ChartControl _chart;

    #region public StyleManager()
    public StyleManager() {
      InitializeComponent();
      this.Init();
    }
    #endregion

    #region public StyleManager(IContainer container)
    public StyleManager(IContainer container) {
      container.Add(this);
      InitializeComponent();
      this.Init();
    }
    #endregion

    #region public List<Style> Styles
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [EditorAttribute(typeof(CollectionEditor), typeof(UITypeEditor))]
    public List<Style> Styles {
      get { return _styles; }
      set {
        this._styles = value;
      }
    }
    #endregion

    #region public Style Default
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public Style Default {
      get {
        if (_default == null) {
          if (_styles.Count == 0) {
            _default = new Style();
            _styles.Add(_default);
          } else {
            _default = _styles[0];
          }
        }
        return _default;
      }
      set {
        _default = value;
      }
    }
    #endregion

    private void Init() {
      _styles.Add(new Style());
    }
  }
}
