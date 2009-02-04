/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace Gordago.Analysis.Chart {

  #region public enum ChartPanelBorderStyle
  public enum ChartPanelBorderStyle {
    Sizable,
    None
  }
  #endregion

  public class ChartPanel:UserControl {

    public event EventHandler PanelClosed;
    public event EventHandler PropertyView;

    private string _text = "";

    private ChartPanelBorderStyle _borderStyle = ChartPanelBorderStyle.Sizable;

    private ChartPanelProperties _properties;

    private bool _visible = true;

    public ChartPanel() { }

    #region internal ChartPanelContainer ChartPanelContainer
    internal ChartPanelContainer ChartPanelContainer {
      get { return this.Parent as ChartPanelContainer; }
    }
    #endregion

    #region public ChartPanelProperties Properties
    public ChartPanelProperties Properties {
      get {
        if (_properties == null)
          _properties = new ChartPanelProperties(this);
        return _properties; 
      }
      set { _properties = value; }
    }
    #endregion

    #region public string Text
    [Browsable(true)]
    [Category("Misc"), DisplayName("Text")]
    public new string Text {
      get { return this._text; }
      set { this._text = value; }
    }
    #endregion

    #region public ChartPanelBorderStyle BorderStyle
    public new ChartPanelBorderStyle BorderStyle {
      get { return this._borderStyle; }
      set { 
        this._borderStyle = value;
      }
    }
    #endregion

    #region public new bool Visible
    public new bool Visible {
      get {
        if (this.Parent == null)
          return _visible;
        _visible = this.Parent.Visible;
        return _visible;
      }
      set {
        if (this.Parent != null) 
          this.Parent.Visible = value;
        _visible = value;
        // this.Dock = DockStyle;
      }
    }
    #endregion

    #region internal bool BaseVisible
    internal bool BaseVisible {
      get { return base.Visible; }
      set { base.Visible = value; }
    }
    #endregion

    #region internal new DockStyle Dock
    internal new DockStyle Dock {
      get { return base.Dock; }
      set { base.Dock = value; }
    }
    #endregion

    #region public override AnchorStyles Anchor
    public override AnchorStyles Anchor {
      get {
        if (this.Parent == null)
          return base.Anchor;
        return this.Parent.Anchor;
      }
      set {
        if (this.Parent == null)
          base.Anchor = value;
        else
          this.Parent.Anchor = value;
      }
    }
    #endregion

    #region internal AnchorStyles BaseAnchor
    internal AnchorStyles BaseAnchor {
      get { return base.Anchor; }
      set { base.Anchor = value; }
    }
    #endregion

    #region public new Point Location
    public new Point Location {
      get {
        if (this.Parent == null)
          return base.Location;
        return this.Parent.Location;
      }
      set {
        if (this.Parent == null)
          base.Location = value;
        else
          this.Parent.Location = value;
      }
    }
    #endregion

    #region internal Point BaseLocation
    internal Point BaseLocation {
      get { return base.Location; }
      set { base.Location = value; }
    }
    #endregion

    #region public new Size Size
    public new Size Size {
      get {
        return base.Size;
      }
      set {
        base.Size = value;
        if (this.Parent != null)
          this.ChartPanelContainer.CheckSizeFromPanel();
      }
    }
    #endregion

    #region public new int Width
    public new int Width {
      get {
        return base.Width;
      }
      set {
        base.Width = value;
        if (this.Parent != null)
          this.ChartPanelContainer.CheckSizeFromPanel();
      }
    }
    #endregion

    #region public new int Height
    public new int Height {
      get {
        return base.Height;
      }
      set {
        base.Height = value;
        if (this.ChartPanelContainer != null)
          this.ChartPanelContainer.CheckSizeFromPanel();
      }
    }
    #endregion

    #region public void Close()
    public void Close() {
      if (this.Parent == null && !(this.Parent is ChartPanelContainer))
        return;

      ChartPanelContainer cpc = this.Parent as ChartPanelContainer;
      if (!(cpc.Parent is ChartManager))
        return;
      ChartManager cm = cpc.Parent as ChartManager;
      cm.ChartPanels.Remove(this);

      if (PanelClosed != null) {
        this.PanelClosed(this, new EventArgs());
      }
    }
    #endregion

    #region public void SaveTemplate(XmlNodeManager nodeManager)
    public void SaveTemplate(XmlNodeManager nodeManager) {
      this.Properties.OnSaveTemplate(nodeManager);
    }
    #endregion

    #region public void LoadTemplate(XmlNodeManager nodeManager)
    public void LoadTemplate(XmlNodeManager nodeManager) {
      this.Properties.OnLoadTemplate(nodeManager);
      this.OnLoadSettingsCompleate();
    }
    #endregion

    #region internal void ShowProperty()
    internal void ShowProperty() {
      if (this.PropertyView != null)
        this.PropertyView(this, new EventArgs());
    }
    #endregion

    protected virtual void OnLoadSettingsCompleate() {}
  }
}
