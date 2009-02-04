/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Text;

namespace Gordago.Analysis.Chart {
  public class ChartObjectLabel:ChartObject {

    private string _text;

    private Font _font;
    private Color _foreColor;
    private Brush _foreBrush;
    private StringFormat _sformat;

    public ChartObjectLabel(string name):base(name, new COPointManagerPoint()) {
      _text = "text";
      this.ForeColor = Color.Red;

      _sformat = new StringFormat();
      _sformat.Alignment = StringAlignment.Center;
      _sformat.LineAlignment = StringAlignment.Center;
      _sformat.FormatFlags = StringFormatFlags.NoWrap;

      this.TypeName = "Label";
      this.Cursor = new Cursor(new MemoryStream(global::Gordago.Properties.Resources.COLabel));
      this.Image = global::Gordago.Properties.Resources.m_co_label;
    }

    #region public Font Font
    [Category("Style"), DisplayName("Font")]
    public Font Font {
      get {
        if (this.ChartBox != null) {
          if (this._font == null)
            _font = this.ChartBox.ChartManager.Style.ScaleFont;
        } else {
          this.Font = new Font("Microsoft Sans Serif", 7);
        }

        return this._font; 
      }
      set { this._font = value; }
    }
    #endregion

    #region public Color ForeColor
    [Category("Style"), DisplayName("Fore Color")]
    public Color ForeColor {
      get { return this._foreColor;  }
      set {
        this._foreBrush = new SolidBrush(value);
        this._foreColor = value; 
      }
    }
    #endregion

    #region public string Text
    [Category("Main"), DisplayName("Text")]
    public string Text {
      get { return this._text; }
      set { this._text = value; }
    }
    #endregion

    #region private StringFormat StringFormat
    private StringFormat StringFormat {
      get { return this._sformat; }
      set { this._sformat = value; }
    }
    #endregion

    #region protected override void OnPaintObject(Graphics g)
    protected override void OnPaintObject(Graphics g) {

      COPoint copoint = this.GetCOPoint(0);
      if (copoint == null)
        return;

      int x = this.ChartBox.GetX(copoint.BarIndex);
      int y = this.ChartBox.GetY(copoint.Price);
      
      SizeF sizef = g.MeasureString(this.Text+"W", this.Font, new PointF(x, y), _sformat);
      int w = Convert.ToInt32(sizef.Width);
      int h = Convert.ToInt32(sizef.Height);

      RectangleF rectf = new RectangleF(x - w / 2, y, w, h);
      g.DrawString(this.Text, this.Font, _foreBrush, rectf, _sformat);

      this.GraphicsPath.AddString(this.Text, this.Font.FontFamily, (int)this.Font.Style, this.Font.Size, rectf, _sformat);
    }
    #endregion

    #region protected override void OnSaveTemplate(XmlNodeManager nodeManager)
    protected override void OnSaveTemplate(XmlNodeManager nodeManager) {
      base.OnSaveTemplate(nodeManager);

      nodeManager.SetAttribute("LabelText", this.Text);
      nodeManager.SetAttribute("LabelForeColor", this.ForeColor);
      nodeManager.SetAttribute("LabelFont", this.Font);
    }
    #endregion

    #region protected override void OnLoadTemplate(XmlNodeManager nodeManager)
    protected override void OnLoadTemplate(XmlNodeManager nodeManager) {
      base.OnLoadTemplate(nodeManager);
      this.Text = nodeManager.GetAttributeString("LabelText", "Label");
      this.ForeColor = nodeManager.GetAttributeColor("LabelForeColor", Color.Red);
      this.Font = nodeManager.GetAttributeFont("LabelFont", this.Font);
    }
    #endregion
  }
}
