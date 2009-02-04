using System;
using System.Text;
using System.Drawing;
using System.Collections.Generic;

using Gordago.Analysis.Chart;

namespace Gordago.PlugIn.Clock {

  class ClockFigure:ChartFigure {

    private const double STEP60 = (Math.PI * 2) / 60;
    private const double STEP12 = (Math.PI * 2) / 12;

    private long _deltaTime;
    private CountryTime _country;

    private Font _font;
    private StringFormat _stringFormat;
    private int _x;
    private int _y;

    private Color _foreColor;
    private Brush _foreBrush;
    
    private Color _clockColor;
    private Pen _clockPen;
    private Color _backColor;
    private Brush _backBrush;

    private Bitmap _clockBitmap;

    public ClockFigure(string name, CountryTime country):base(name) {
      _country = country;

      DateTime tm = DateTime.UtcNow.AddMinutes(country.GMT);
      _deltaTime = DateTime.Now.Ticks - tm.Ticks;

      this.Font = new Font("Microsoft Sans Serif", 8);
      this.ClockColor = Color.White;
      this.ForeColor = Color.Black;
      this.BackColor = Color.FromArgb(150, 225, 225, 225);

      
      _stringFormat = new StringFormat();
      _stringFormat.Alignment = StringAlignment.Center;
      _stringFormat.LineAlignment = StringAlignment.Center;
      _clockBitmap = global::Gordago.PlugIn.Clock.Properties.Resources.clock_small;
    }

    #region public int ClockWidth
    public int ClockWidth {
      get { return this._clockBitmap.Width; }
    }
    #endregion

    #region public int ClockHeight
    public int ClockHeight {
      get { return this._clockBitmap.Height; }
    }
    #endregion

    #region public Font Font
    public Font Font {
      get { return this._font; }
      set { this._font = value; }
    }
    #endregion

    #region public Color ForeColor
    public Color ForeColor {
      get { return this._foreColor; }
      set { this._foreColor = value;
        _foreBrush = new SolidBrush(value);
      }
    }
    #endregion

    #region public Color ClockColor
    public Color ClockColor {
      get { return this._clockColor; }
      set { 
        this._clockColor = value;
        this._clockPen = new Pen(value);
      }
    }
    #endregion

    #region public Color BackColor
    public Color BackColor {
      get { return this._backColor; }
      set {
        this._backColor = value;
        _backBrush = new SolidBrush(value);
      }
    }
    #endregion

    #region public void SetLocation(int x, int y)
    public void SetLocation(int x, int y) {
      _x = x;
      _y = y;
    }
    #endregion

    protected override void OnPaint(Graphics g) {
      ChartStyle cs = this.ChartBox.ChartManager.Style;
      if(this.ClockColor != cs.BarColor) {
        this.ClockColor = cs.BarColor;
        this.ForeColor = cs.ScaleForeColor;
      }

      int d = 3;

      Rectangle rect = new Rectangle(_x + d, _y + d, this.ClockWidth - d * 2, this.ClockHeight - d * 2);

      g.FillEllipse(_backBrush, rect);
      g.DrawImageUnscaled(_clockBitmap, _x, _y);

      DateTime dtm = new DateTime(DateTime.Now.Ticks - _deltaTime);

      int dh = 20;
      Rectangle rectHour = new Rectangle(_x + dh, _y + dh, this.ClockWidth - dh * 2, this.ClockHeight - dh * 2);
      this.ClockLine(g, _clockPen, rectHour, STEP12 * dtm.Hour - STEP12 * 15);


      int dm = 10;
      Rectangle rectMinute = new Rectangle(_x + dm, _y + dm, this.ClockWidth - dm * 2, this.ClockHeight - dm * 2);
      this.ClockLine(g, _clockPen, rectMinute, STEP60 * dtm.Minute-STEP60*15);

      Rectangle rectdesc = new Rectangle(_x, _y + this.ClockHeight, this.ClockWidth, 25);
      g.FillRectangle(_backBrush, rectdesc);
      g.DrawString(_country.Name+"\n"+dtm.ToShortTimeString(), this.Font, _foreBrush, new RectangleF(_x, _y + this.ClockHeight, this.ClockWidth, 25), _stringFormat);
      g.DrawRectangle(_clockPen, rectdesc);
    }

    #region private void ClockLine(Graphics g, Pen pen, Rectangle rect, double angle)
    private void ClockLine(Graphics g, Pen pen, Rectangle rect, double angle) {

      int rx = rect.Width / 2;
      int ry = rect.Height / 2;

      int dx = Convert.ToInt32(rx * Math.Cos(angle));
      int dy = Convert.ToInt32(ry * Math.Sin(angle));

      int x = rect.X + rx;
      int y = rect.Y + ry;


      g.DrawLine(pen, x, y, x + dx, y + dy);
    }
    #endregion
  }
}
