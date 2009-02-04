/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

namespace Gordago.Analysis.Chart {

  public class ChartPanelProperties {
    private ChartPanel _chartPanel;

    public ChartPanelProperties(ChartPanel chartPanel) {
      _chartPanel = chartPanel;
    }

    #region internal ChartPanelContainer PanelContainer
    internal ChartPanelContainer PanelContainer {
      get { return this._chartPanel.Parent as ChartPanelContainer; }
    }
    #endregion

    #region protected ChartPanel ChartPanel
    protected ChartPanel ChartPanel {
      get { return this._chartPanel; }
    }
    #endregion

    #region public Point Location
    [Category("Main"), DisplayName("Location")]
    public Point Location {
      get {
        return this._chartPanel.Location; 
      }
      set {
         this._chartPanel.Location = value; 
      }
    }
    #endregion

    #region public Size Size
    [Category("Main"), DisplayName("Size")]
    public Size Size {
      get { return this._chartPanel.Size; }
      set { this._chartPanel.Size = value; }
    }
    #endregion

    #region public Color BorderColor
    [Category("Style"), DisplayName("Border Color")]
    public Color BorderColor {
      get { return this.PanelContainer.BorderColor; }
      set { this.PanelContainer.BorderColor = value; }
    }
    #endregion

    #region public Color CaptionBackColor
    [Category("Style"), DisplayName("Caption Back Color")]
    public Color CaptionBackColor {
      get { return this.PanelContainer.CaptionBackColor; }
      set { this.PanelContainer.CaptionBackColor = value; }
    }
    #endregion

    #region public Color CaptionForeColor
    [Category("Style"), DisplayName("Caption Fore Color")]
    public Color CaptionForeColor {
      get { return this.PanelContainer.CaptionForeColor; }
      set { this.PanelContainer.CaptionForeColor = value; }
    }
    #endregion

    #region public bool IsMaximized
    [Category("Main"), DisplayName("IsMaximized")]
    public bool IsMaximized {
      get { return this.PanelContainer.IsMaximized; }
      set { this.PanelContainer.IsMaximized = value; }
    }
    #endregion

    #region public Color BackColor
    [Category("Style"), DisplayName("Back Color")]
    public Color BackColor {
      get { return this.ChartPanel.BackColor; }
      set { this.ChartPanel.BackColor = value; }
    }
    #endregion

    #region public Color ForeColor
    [Category("Style"), DisplayName("Fore Color")]
    public Color ForeColor {
      get { return this.ChartPanel.ForeColor; }
      set { this._chartPanel.ForeColor = value; }
    }
    #endregion

    #region public AnchorStyles Anchor
    [Category("Style"), DisplayName("Anchor")]
    public AnchorStyles Anchor {
      get { return _chartPanel.Anchor; }
      set { this._chartPanel.Anchor = value; }
    }
    #endregion

    #region protected virtual void OnSaveTemplate(XmlNodeManager nodeManager)
    protected internal virtual void OnSaveTemplate(XmlNodeManager nodeManager) {
      nodeManager.SetAttribute("Type", this._chartPanel.GetType().FullName);
      nodeManager.SetAttribute("Anchor", this.Anchor.ToString());
      nodeManager.SetAttribute("X", this.Location.X);
      nodeManager.SetAttribute("Y", this.Location.Y);
      nodeManager.SetAttribute("Width", this.Size.Width);
      nodeManager.SetAttribute("Height", this.Size.Height);

      nodeManager.SetAttribute("BackClr", this.BackColor);
      nodeManager.SetAttribute("BorderClr", this.BorderColor);
      nodeManager.SetAttribute("CaptionBackClr", this.CaptionBackColor);
      nodeManager.SetAttribute("CaptionForeClr", this.CaptionForeColor);
      nodeManager.SetAttribute("IsMaximized", this.IsMaximized);
    }
    #endregion

    #region protected virtual void OnLoadTemplate(XmlNodeManager nodeManager)
    protected internal virtual void OnLoadTemplate(XmlNodeManager nodeManager) {

      this.Anchor = (AnchorStyles)Enum.Parse(typeof(AnchorStyles), nodeManager.GetAttributeString("Anchor", this.Anchor.ToString()));
      int x = nodeManager.GetAttributeInt32("X", this.Location.X);
      int y = nodeManager.GetAttributeInt32("Y", this.Location.Y);
      int w = nodeManager.GetAttributeInt32("Width", this.Size.Width);
      int h = nodeManager.GetAttributeInt32("Height", this.Size.Height);
      this.Location = new Point(x, y);
      this.Size = new Size(w, h);

      this.BackColor = nodeManager.GetAttributeColor("BackClr", this.BackColor);
      this.BorderColor = nodeManager.GetAttributeColor("BorderClr", this.BorderColor);
      this.CaptionBackColor = nodeManager.GetAttributeColor("CaptionBackClr", this.CaptionBackColor);
      this.CaptionForeColor = nodeManager.GetAttributeColor("CaptionForeClr", this.CaptionForeColor);
      this.IsMaximized = nodeManager.GetAttributeBoolean("IsMaximized", true);
    }
    #endregion
  }
}
