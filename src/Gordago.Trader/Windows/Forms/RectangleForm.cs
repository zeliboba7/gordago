/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using Gordago.Analysis.Chart;

namespace Gordago.Windows.Forms {
  class RectangleForm : Form {
    private Pen _pen;
    private bool _paintEnabled = true;

    private int _opacity;
    private static readonly object _opacityChanged = new object();
    private bool _suppressPaint;

    private Rectangle _startBounds;
    private Point _startCursorPostion;
    private Size _deltaLocation, _deltaSize;
    private bool _isWindow = true;
    private ScreenRectTSP _srTSP;
    private ToolStripItemPanel _tsip;

    private DockStyle _borderStyle = DockStyle.Fill;
    private MouseControlType _mouseType;

    public RectangleForm(Rectangle rect, DockStyle borderStyle, ScreenRectTSP srTSP, ToolStripItemPanel tsip, MouseControlType mtype) {
      _tsip = tsip;
      _mouseType = mtype;
      _srTSP = srTSP;
      this.MinimumSize = new Size(10, 10);
      _borderStyle = borderStyle;
      _pen = new Pen(Color.Blue);
      this.FormBorderStyle = FormBorderStyle.None;
      this.ShowInTaskbar = false;
      this.LayeredMode = true;

      this.Opacity = 40;
      this.SetBounds(rect.X, rect.Y, rect.Width, rect.Height);
      _startBounds = rect;
      _startCursorPostion = Cursor.Position;
      _deltaLocation = new Size(Cursor.Position.X - rect.X, Cursor.Position.Y - rect.Y);
      _deltaSize = new Size(Cursor.Position.X - (rect.X + rect.Width), Cursor.Position.Y - (rect.Y + rect.Height));
    }

    #region public MouseControlType MouseType
    public MouseControlType MouseType {
      get { return this._mouseType; }
    }
    #endregion

    #region public ScreenRectTSP SRTSP
    public ScreenRectTSP SRTSP {
      get { return this._srTSP; }
    }
    #endregion

    #region public ToolStripItemPanel TSIP
    public ToolStripItemPanel TSIP {
      get { return _tsip; }
    }
    #endregion

    #region public DockStyle BorderStyle
    public DockStyle BorderStyle {
      get { return this._borderStyle; }
      set { 
        this._borderStyle = value;
        this.Invalidate();
      }
    }
    #endregion

    #region private bool IsWindow
    private bool IsWindow {
      get { return _isWindow; }
      set { 
        _isWindow = value;
        this.Invalidate();
      }
    }
    #endregion

    #region public Rectangle StartBounds
    /// <summary>
    /// Начальное положение и размер формы относительно координат экрана
    /// </summary>
    public Rectangle StartBounds {
      get { return _startBounds; }
    }
    #endregion

    #region public Point StartCursorPosition
    public Point StartCursorPosition {
      get { return this._startCursorPostion; }
    }
    #endregion

    #region public Size DeltaLocation
    /// <summary>
    /// Смещение курсора относительно левого верхнего угла
    /// </summary>
    public Size DeltaLocation {
      get {
        return _deltaLocation;
      }
    }
    #endregion

    #region public Size DeltaSize
    public Size DeltaSize {
      get { return this._deltaSize; }
    }
    #endregion

    #region public bool HitTestEnabled
    /// <summary>
    /// Gets or sets the value indicating whether this layered window receives mouse messages.
    /// </summary>
    public bool HitTestEnabled {
      get {
        return !this.GetValue(WinUser.WS_EX_TRANSPARENT);
      }
      set {
        this.SetValue(WinUser.WS_EX_TRANSPARENT, !value);
      }
    }
    #endregion

    #region public bool LayeredMode
    /// <summary>
    /// Gets or sets the value indicating whether this layered window is in the layered mode.
    /// </summary>
    public bool LayeredMode {
      get {
        return this.GetValue(WinUser.WS_EX_LAYERED);
      }
      set {
        this.SetValue(WinUser.WS_EX_LAYERED, value);
      }
    }
    #endregion

    #region public new int Opacity
    /// <summary>
    /// Gets or sets the opacity level for this layered window. 0 = Opaque. 100 = Transparent.
    /// </summary>
    public new int Opacity {
      get {
        return _opacity;
      }
      set {
        if (_opacity != value) {
          _opacity = value;
          this.Invalidate();

          this.OnOpacityChanged(EventArgs.Empty);
        }
      }
    }
    #endregion

    #region public event EventHandler OpacityChanged
    /// <summary>
    /// Occurs when the value of the <see cref="Opacity"/> property changes.
    /// </summary>
    public event EventHandler OpacityChanged {
      add {
        this.Events.AddHandler(_opacityChanged, value);
      }
      remove {
        this.Events.RemoveHandler(_opacityChanged, value);
      }
    }
    #endregion

    #region protected virtual void OnOpacityChanged(EventArgs e)
    /// <summary>
    /// Will bubble the <see cref="OpacityChanged"/> event.
    /// </summary>
    /// <param name="e"></param>
    protected virtual void OnOpacityChanged(EventArgs e) {
      EventHandler handler = this.Events[_opacityChanged] as EventHandler;

      if (handler != null) {
        handler(this, e);
      }
    }
    #endregion

    #region protected bool SuppressPaint
    /// <summary>
    /// Gets or sets the value indicating whether it is necessary to suppress the
    /// <see cref="E:System.Windows.Forms.Form.Paint"/> event.
    /// Use this to increase performance.
    /// </summary>
    protected bool SuppressPaint {
      get {
        return _suppressPaint;
      }
      set {
        _suppressPaint = value;
      }
    }
    #endregion

    #region protected override CreateParams CreateParams
    /// <summary>
    /// </summary>
    protected override CreateParams CreateParams {
      get {
        CreateParams cp = base.CreateParams;
        cp.ExStyle |= WinUser.WS_EX_TOPMOST;
        return cp;
      }
    }
    #endregion
    #region private void RaisePaintEvent(Graphics g, Rectangle clipRect)
    private void RaisePaintEvent(Graphics g, Rectangle clipRect) {
      if (_paintEnabled) {
        PaintEventArgs e = new PaintEventArgs(g, clipRect);
        this.OnPaint(e);
      }
    }
    #endregion

    #region protected override void OnStyleChanged(EventArgs e)
    /// <summary>
    /// Will bubble the <see cref="E:System.Windows.Forms.Form.StyleChanged"/> event.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnStyleChanged(EventArgs e) {
      base.OnStyleChanged(e);
      this.Invalidate();
    }
    #endregion

    #region protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
    protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified) {
      width = Math.Max(width, this.MinimumSize.Width);
      height = Math.Max(height, this.MinimumSize.Height);

      _paintEnabled = false;
      base.SetBoundsCore(x, y, width, height, specified);
      _paintEnabled = true;

      if (!this.SuppressPaint || specified != BoundsSpecified.Location) {
        this.Invalidate(new Rectangle(x, y, width, height));
      }
    }
    #endregion

    #region protected override void WndProc(ref Message m)
    /// <summary>
    /// </summary>
    /// <param name="m"></param>
    protected override void WndProc(ref Message m) {
      switch (m.Msg) {
        /*
         * Allows the form to redraw itself properly while resizing.
         */
        case WinUser.WM_NCCALCSIZE: {
            m.Result = (IntPtr)(
              WinUser.WVR_ALIGNBOTTOM
              | WinUser.WVR_ALIGNLEFT
              | WinUser.WVR_ALIGNRIGHT
              | WinUser.WVR_ALIGNTOP
              );
            return;
          }
        /*
         * Doesn't allow the host form to lose the focus.
         */
        case WinUser.WM_MOUSEACTIVATE: {
            m.Result = (IntPtr)WinUser.MA_NOACTIVATE;
            return;
          }
      }

      base.WndProc(ref m);
    }
    #endregion

    #region public new void Show()
    /// <summary>
    /// </summary>
    public new void Show() {
      User32.ShowWindow(this.Handle, WinUser.SW_SHOWNA);
    }
    #endregion

    #region public void Show(Point location)
    /// <summary>
    /// </summary>
    /// <param name="location"></param>
    public void Show(Point location) {
      this.Location = location;
      this.Show();
    }
    #endregion

    #region public new virtual void Invalidate()
    /// <summary>
    /// Invalidates the drawing surface.
    /// </summary>
    public new virtual void Invalidate() {
      if (this.Visible) {
        this.Invalidate(this.Bounds);
      }
    }
    #endregion

    #region private new void Invalidate(Rectangle rect)
    private new void Invalidate(Rectangle rect) {
      Bitmap memoryBitmap = new Bitmap(
        Math.Max(rect.Size.Width, 1),
        Math.Max(rect.Size.Height, 1),
        PixelFormat.Format32bppArgb
        );

      using (Graphics g = Graphics.FromImage(memoryBitmap)) {
        Rectangle area = new Rectangle(0, 0, rect.Size.Width, rect.Size.Height);
        this.RaisePaintEvent(g, area);
        IntPtr hDC = User32.GetDC(IntPtr.Zero);
        IntPtr memoryDC = Gdi32.CreateCompatibleDC(hDC);
        IntPtr hBitmap = memoryBitmap.GetHbitmap(Color.FromArgb(0));
        IntPtr oldBitmap = Gdi32.SelectObject(memoryDC, hBitmap);

        SIZE size;
        size.cx = rect.Size.Width;
        size.cy = rect.Size.Height;

        POINT location;
        location.x = rect.Location.X;
        location.y = rect.Location.Y;

        POINT sourcePoint;
        sourcePoint.x = 0;
        sourcePoint.y = 0;

        BLENDFUNCTION blend = new BLENDFUNCTION();
        blend.AlphaFormat = (byte)WinGdi.AC_SRC_ALPHA;
        blend.BlendFlags = 0;
        blend.BlendOp = (byte)WinGdi.AC_SRC_OVER;
        blend.SourceConstantAlpha = (byte)(255 - ((this.Opacity * 255) / 100));

        User32.UpdateLayeredWindow(
          this.Handle,
          hDC,
          ref location,
          ref size,
          memoryDC,
          ref sourcePoint,
          0,
          ref blend,
          WinUser.ULW_ALPHA
          );

        Gdi32.SelectObject(memoryDC, oldBitmap);

        User32.ReleaseDC(IntPtr.Zero, hDC);
        Gdi32.DeleteObject(hBitmap);
        Gdi32.DeleteDC(memoryDC);
      }
    }

    #endregion

    #region protected override void OnPaint(PaintEventArgs e)
    protected override void OnPaint(PaintEventArgs e) {
      Graphics g = e.Graphics;

      if (_isWindow) {
        using (Pen pen = new Pen(Color.Black)) {
          pen.DashStyle = DashStyle.Custom;
          pen.DashPattern = new float[] { 1, 1 };

          switch (_borderStyle) {
            case DockStyle.Fill:
              g.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
              g.DrawRectangle(pen, 1, 1, Width - 3, Height - 3);
              g.DrawRectangle(pen, 2, 2, Width - 5, Height - 5);
              g.DrawRectangle(pen, 3, 3, Width - 7, Height - 7);
              break;
            case DockStyle.Left:
              g.DrawLine(pen, 0, 0, 0, Height);
              g.DrawLine(pen, 1, 1, 1, Height);
              g.DrawLine(pen, 2, 0, 2, Height);
              g.DrawLine(pen, 3, 1, 3, Height);
              break;
            case DockStyle.Top:
              g.DrawLine(pen, 0, 0, Width, 0);
              g.DrawLine(pen, 1, 1, Width, 1);
              g.DrawLine(pen, 0, 2, Width, 2);
              g.DrawLine(pen, 1, 3, Width, 3);
              break;
            case DockStyle.Right:
              int x = Width - 5;
              g.DrawLine(pen, x, 0, x, Height);
              g.DrawLine(pen, x + 1, 1, x + 1, Height);
              g.DrawLine(pen, x + 2, 0, x + 2, Height);
              g.DrawLine(pen, x + 3, 1, x + 3, Height);
              break;
            case DockStyle.Bottom:
              int y = Height - 5;
              g.DrawLine(pen, 0, y, Width, y);
              g.DrawLine(pen, 1, y + 1, Width, y + 1);
              g.DrawLine(pen, 0, y + 2, Width, y + 2);
              g.DrawLine(pen, 1, y + 3, Width, y + 3);
              break;
          }
        }
      } else {
        using (SolidBrush fill = new SolidBrush(Color.FromArgb(3, Color.White)))
        using (Pen pen = new Pen(Color.Blue)) {
          Rectangle rect = this.ClientRectangle;
          rect.Width -= 1;
          rect.Height -= 1;
          g.FillRectangle(fill, rect);
          g.DrawRectangle(pen, rect);
        }
      }
#if DEBUGs
      using (SolidBrush fill = new SolidBrush(Color.FromArgb(3, Color.White)))
      using (Pen pen = new Pen(Color.Blue)) {
        Rectangle rect = this.ClientRectangle;
        rect.Width -= 1;
        rect.Height -= 1;
        g.FillRectangle(fill, rect);
        g.DrawRectangle(pen, rect);
      }
#endif
    }
    #endregion
    
    #region private bool GetValue(int exStyle)
    private bool GetValue(int exStyle) {
      int currentStyle = User32.GetWindowLong(this.Handle, WinUser.GWL_EXSTYLE);
      return (currentStyle & exStyle) != 0;
    }
    #endregion

    #region private void SetValue(int exStyle, bool value)
    private void SetValue(int exStyle, bool value) {
      int currentStyle = User32.GetWindowLong(this.Handle, WinUser.GWL_EXSTYLE);

      if (value)
        currentStyle |= exStyle;
      else
        currentStyle &= ~exStyle;

      User32.SetWindowLong(this.Handle, WinUser.GWL_EXSTYLE, currentStyle);
    }
    #endregion
  }

  #region class User32
  /// <summary>
  /// Imports User32.dll functions.
  /// </summary>
  class User32 {

    #region public static extern IntPtr GetDC
    /// <summary>
    /// Retrieves a handle of a display device context (DC) for the client area of the specified window. 
    /// The display device context can be used in subsequent GDI functions to draw in the client area 
    /// of the window.
    /// </summary>
    /// <param name="hWnd">Identifies the window whose device context is to be retrieved.</param>
    /// <returns>If the function succeeds, the return value identifies the device context for the 
    /// given window's client area; otherwise, the return value is <c>null</c>.</returns>
    [DllImport("User32.dll")]
    public static extern IntPtr GetDC(
      IntPtr hWnd
      );
    #endregion

    #region public extern static int GetWindowLong
    /// <summary>
    /// Retrieves information about the specified window. The function also retrieves the 32-bit (long) value at the specified offset into the extra window memory.
    /// </summary>
    /// <param name="hWnd">Handle to the window and, indirectly, the class to which the window belongs.</param>
    /// <param name="dwStyle">Specifies the zero-based offset to the value to be retrieved. Valid values 
    /// are in the range zero through the number of bytes of extra window memory, minus four; for 
    /// example, if you specified 12 or more bytes of extra memory, a value of 8 would be an index to 
    /// the third 32-bit integer.</param>
    /// <returns>If the function succeeds, the return value is the requested 32-bit value. otherwise, 
    /// the return value is zero. To get extended error information, call GetLastError. If SetWindowLong 
    /// has not been called previously, GetWindowLong returns zero for values in the extra window or 
    /// class memory.</returns>
    [DllImport("User32.dll")]
    public extern static int GetWindowLong(
      IntPtr hWnd,
      int dwStyle
      );
    #endregion

    #region public static extern IntPtr WindowFromPoint(Point point)
    [DllImport("user32.dll")]
    public static extern IntPtr WindowFromPoint(Point point);
    #endregion

    #region public static extern int ReleaseDC
    /// <summary>
    /// Releases a device context (DC), freeing it for use by other applications. The effect of the 
    /// ReleaseDC function depends on the type of DC. It frees only common and window DCs. It has no 
    /// effect on class or private DCs.
    /// </summary>
    /// <param name="hWnd">Handle to the window whose DC is to be released.</param>
    /// <param name="hDC">Handle to the DC to be released.</param>
    /// <returns>The return value indicates whether the DC was released. If the DC was released, 
    /// the return value is 1. If the DC was not released, the return value is zero.</returns>
    [DllImport("User32.dll")]
    public static extern int ReleaseDC(
      IntPtr hWnd,
      IntPtr hDC
      );
    #endregion

    #region public static extern int SetWindowLong
    /// <summary>
    /// Changes an attribute of the specified window. The function also sets the 32-bit (long) value 
    /// at the specified offset into the extra window memory.
    /// </summary>
    /// <remarks>This function has been superseded by the SetWindowLongPtr function. To write code
    /// that is compatible with both 32-bit and 64-bit versions of Microsoft Windows, use the 
    /// SetWindowLongPtr function.</remarks>
    /// <param name="hWnd">Handle to the window and, indirectly, the class to which the window belongs.
    /// Windows 95/98/Me: The SetWindowLong function may fail if the window specified by the hWnd parameter 
    /// does not belong to the same process as the calling thread.</param>
    /// <param name="nIndex">Specifies the zero-based offset to the value to be set. Valid values are 
    /// in the range zero through the number of bytes of extra window memory, minus the size of an 
    /// integer.</param>
    /// <param name="dwNewLong">Specifies the replacement value.</param>
    /// <returns>If the function succeeds, the return value is the previous value of the specified 
    /// 32-bit integer; otherwise, the return value is zero. To get extended error information, 
    /// call GetLastError.</returns>
    [DllImport("User32.dll")]
    public static extern int SetWindowLong(
      IntPtr hWnd,
      int nIndex,
      int dwNewLong
      );
    #endregion

    #region public static extern bool ShowWindow
    /// <summary>
    /// Sets the specified window's show state.
    /// </summary>
    /// <param name="hWnd">Identifies the window.</param>
    /// <param name="nCmdShow">Specifies how the window is to be shown. This parameter is ignored the first 
    /// time an application calls ShowWindow, if the program that launched the application provides a 
    /// STARTUPINFO structure. Otherwise, the first time ShowWindow is called, the value should be the 
    /// value obtained by the WinMain function in its nCmdShow parameter.</param>
    /// <returns>If the window was previously visible, the return value is <see langword="true"/>. 
    /// If the window was previously hidden, the return value is <see langword="false"/>.</returns>
    [DllImport("User32.dll")]
    public static extern bool ShowWindow(
      IntPtr hWnd,
      int nCmdShow
      );
    #endregion

    #region public static extern bool UpdateLayeredWindow
    /// <summary>
    /// Updates the position, size, shape, content, and translucency of a layered window.
    /// </summary>
    /// <param name="hWnd">Handle to a layered window. A layered window is created by specifying 
    /// WS_EX_LAYERED when creating the window with the CreateWindowEx function.</param>
    /// <param name="hdcDst">Handle to a device context (DC) for the screen. This handle is obtained 
    /// by specifying <c>IntPtr.Zero</c> when calling the function. It is used for palette color matching 
    /// when the window contents are updated. If hdcDst is <c>IntPtr.Zero</c>, the default palette will 
    /// be used. If hdcSrc is <c>IntPtr.Zero</c>, hdcDst must be <c>IntPtr.Zero</c>.</param>
    /// <param name="pptDst">Pointer to a POINT structure that specifies the new screen position of the 
    /// layered window. If the current position is not changing, pptDst can be <c>null</c>.</param>
    /// <param name="psize">Pointer to a SIZE structure that specifies the new size of the layered 
    /// window. If the size of the window is not changing, psize can be <c>null</c>. If hdcSrc is 
    /// <c>null</c>, psize must be <c>null</c>.</param>
    /// <param name="hdcSrc">Handle to a DC for the surface that defines the layered window. This handle 
    /// can be obtained by calling the CreateCompatibleDC function. If the shape and visual context of 
    /// the window are not changing, hdcSrc can be <c>null</c>.</param>
    /// <param name="pprSrc">Pointer to a POINT structure that specifies the location of the layer in 
    /// the device context. If hdcSrc is <c>null</c>, pptSrc should be <c>null</c>.</param>
    /// <param name="crKey">Specifies the color key to be used when composing the layered window.</param>
    /// <param name="pblend">Pointer to a BLENDFUNCTION structure that specifies the transparency value to be used when composing the layered window.</param>
    /// <param name="dwFlags">If hdcSrc is <c>null</c>, dwFlags should be zero.</param>
    /// <returns>If the function succeeds, the return value is <c>true</c>; otherwise, the return 
    /// value is <c>false</c>. To get extended error information, call GetLastError.</returns>
    [DllImport("User32.dll")]
    public static extern bool UpdateLayeredWindow(
      IntPtr hWnd,
      IntPtr hdcDst,
      ref POINT pptDst,
      ref SIZE psize,
      IntPtr hdcSrc,
      ref POINT pprSrc,
      int crKey,
      ref BLENDFUNCTION pblend,
      int dwFlags
      );
    #endregion

    #region public enum HookType : int
    public enum HookType : int {
      WH_JOURNALRECORD = 0,
      WH_JOURNALPLAYBACK = 1,
      WH_KEYBOARD = 2,
      WH_GETMESSAGE = 3,
      WH_CALLWNDPROC = 4,
      WH_CBT = 5,
      WH_SYSMSGFILTER = 6,
      WH_MOUSE = 7,
      WH_HARDWARE = 8,
      WH_DEBUG = 9,
      WH_SHELL = 10,
      WH_FOREGROUNDIDLE = 11,
      WH_CALLWNDPROCRET = 12,
      WH_KEYBOARD_LL = 13,
      WH_MOUSE_LL = 14
    }
    #endregion

    public delegate int HookProc(int code, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    public static extern int SetWindowsHookEx(HookType hookType, HookProc lpfn, IntPtr hInstance, int threadId);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
  }
  #endregion

  #region class Gdi32
  /// <summary>
  /// Imports Gdi32.dll functions.
  /// </summary>
  class Gdi32 {
    /// <summary>
    /// Creates a memory device context (DC) compatible with the specified device.
    /// </summary>
    /// <param name="hdc">Identifies the device context. If this handle is <c>IntPtr.Zero</c>, 
    /// the function creates a memory device context compatible with the application's current screen.</param>
    /// <returns>If the function succeeds, the return value is the handle to a memory device context, 
    /// otherwise - <c>IntPtr.Zero</c>.</returns>
    [DllImport("Gdi32.dll")]
    public static extern IntPtr CreateCompatibleDC(
      IntPtr hdc
      );

    /// <summary>
    /// Deletes the specified device context (DC). 
    /// </summary>
    /// <param name="hdc">Identifies the device context.</param>
    /// <returns>If the function succeeds, the return value is <c>true</c>; 
    /// otherwise, <c>false</c>.</returns>
    [DllImport("Gdi32.dll")]
    public static extern IntPtr DeleteDC(
      IntPtr hdc
      );

    /// <summary>
    /// Deletes a logical pen, brush, font, bitmap, region, or palette, freeing all 
    /// system resources associated with the object. After the object is deleted, the specified 
    /// handle is no longer valid. 
    /// </summary>
    /// <param name="hObject">Identifies a logical pen, brush, font, bitmap, region, or palette.</param>
    /// <returns>If the function succeeds, the return value is <c>true</c>; otherwise - <c>false</c>.</returns>
    [DllImport("Gdi32.dll")]
    public static extern bool DeleteObject(
      IntPtr hObject
      );

    /// <summary>
    /// Selects an object into the specified device context. The new object replaces the previous object of the same type. 
    /// </summary>
    /// <param name="hdc">Identifies the device context.</param>
    /// <param name="hgdiobj">Identifies the object to be selected.</param>
    /// <returns>If the selected object is not a region and the function succeeds, the return value 
    /// is the handle of the object being replaced. If the selected object is a region and the 
    /// function succeeds, the return value is one of the following values: SIMPLEREGION
    /// (region consists of a single rectangle), COMPLEXREGION (region consists of more than one 
    /// rectangle), NULLREGION (region is empty). If an error occurs and the selected object is not 
    /// a region, the return value is <c>IntPtr.Zero</c>. Otherwise, it is GDI_ERROR.</returns>
    [DllImport("Gdi32.dll")]
    public static extern IntPtr SelectObject(
      IntPtr hdc,
      IntPtr hgdiobj
      );
  }
  #endregion

  #region struct BLENDFUNCTION
  /// <summary>
  /// Controls blending by specifying the blending functions for source and destination bitmaps.
  /// </summary>
  struct BLENDFUNCTION {
    /// <summary>
    /// Specifies the source blend operation. Currently, the only source and destination blend 
    /// operation that has been defined is AC_SRC_OVER. 
    /// </summary>
    public byte BlendOp;

    /// <summary>
    /// Must be zero.
    /// </summary>
    public byte BlendFlags;

    /// <summary>
    /// Specifies an alpha transparency value to be used on the entire source bitmap. The 
    /// SourceConstantAlpha value is combined with any per-pixel alpha values in the source bitmap. 
    /// If you set SourceConstantAlpha to 0, it is assumed that your image is transparent. Set the 
    /// SourceConstantAlpha value to 255 (opaque) when you only want to use per-pixel alpha values.
    /// </summary>
    public byte SourceConstantAlpha;

    /// <summary>
    /// This member controls the way the source and destination bitmaps are interpreted.
    /// </summary>
    public byte AlphaFormat;
  }
  #endregion

  #region struct POINT
  /// <summary>
  /// Defines the x- and y- coordinates of a point. 
  /// </summary>
  struct POINT {
    /// <summary>
    /// Specifies the x-coordinate of the point.
    /// </summary>
    public int x;

    /// <summary>
    /// Specifies the y-coordinate of the point.
    /// </summary>
    public int y;
  }
  #endregion

  #region struct SIZE
  /// <summary>
  /// Specifies the width and height of a rectangle. 
  /// </summary>
  struct SIZE {
    /// <summary>
    /// Specifies the rectangle's width.
    /// </summary>
    public int cx;

    /// <summary>
    /// Specifies the rectangle's height.
    /// </summary>
    public int cy;
  }
  #endregion

  #region class WinUser
  class WinUser {
    #region Extended Window Styles

    /// <summary>
    /// Windows 2000/XP: Creates a layered window. Note that this cannot be used for child windows.
    /// Also, this cannot be used if the window has a class style of either CS_OWNDC or CS_CLASSDC.
    /// </summary>
    public const int WS_EX_LAYERED = 0x80000;

    /// <summary>
    /// The WS_EX_TRANSPARENT style makes a window transparent; that is, the window can be seen through,
    /// and anything under the window is still visible. Transparent windows are not transparent to mouse
    /// or keyboard events. A transparent window receives paint messages when anything under it changes.
    /// Transparent windows are useful for drawing drag handles on top of other windows or for
    /// implementing "hot-spot" areas without having to hit test because the transparent window receives
    /// click messages.
    /// </summary>
    public const int WS_EX_TRANSPARENT = 0x20;

    /// <summary>
    /// This style applies only to top-level windows; it is ignored for child windows. When this style
    /// is enabled, Windows places the window above any windows that do not have the WS_EX_TOPMOST style.
    /// Beginning with Windows version 3.1, there are two classes of top-level windows: topmost top-level
    /// windows and top-level windows. Topmost top-level windows always appear above top-level windows in
    /// the Z order. Top-level windows can be made topmost top-level windows by calling the SetWindowPos
    /// function with the handle to the window and –1 for the hwndInsertAfter parameter.
    /// Topmost top-level windows can become regular top-level windows by calling SetWindowPos with the
    /// window handle and 1 for the hwndInsertAfter parameter.
    /// </summary>
    public const int WS_EX_TOPMOST = 0x8;

    #endregion

    #region ShowWindow() flags

    /// <summary>
    /// Displays the window in its current state. The active window remains active.
    /// </summary>
    public const int SW_SHOWNA = 8;

    #endregion

    #region UpdateLayeredWindow() flags

    /// <summary>
    /// Use crKey as the transparency color.
    /// </summary>
    public const int ULW_COLORKEY = 1;

    /// <summary>
    /// Use pblend as the blend function. If the display mode is 256 colors or less, the effect of 
    /// this value is the same as the effect of ULW_OPAQUE.
    /// </summary>
    public const int ULW_ALPHA = 2;

    /// <summary>
    /// Draw an opaque layered window.
    /// </summary>
    public const int ULW_OPAQUE = 4;

    #endregion

    #region Window field offsets for GetWindowLong() and SetWindowLong()

    /// <summary>
    /// Gets or sets a new extended window style.
    /// </summary>
    public const int GWL_EXSTYLE = -20;

    /// <summary>
    /// Gets or sets a new window style.
    /// </summary>
    public const int GWL_STYLE = -16;

    #endregion

    #region Window Messages

    /// <summary>
    /// The WM_NCCALCSIZE message is sent when the size and position of a window's client area must be
    /// calculated. By processing this message, an application can control the content of the window's
    /// client area when the size or position of the window changes.
    /// </summary>
    public const int WM_NCCALCSIZE = 0x83;

    /// <summary>
    /// The WM_MOUSEACTIVATE message is sent when the cursor is in an inactive window and the user presses a mouse button.
    /// The parent window receives this message only if the child window passes it to the DefWindowProc function.
    /// </summary>
    public const int WM_MOUSEACTIVATE = 0x21;

    #endregion

    #region WM_MOUSEACTIVATE Return Codes

    /// <summary>
    /// Does not activate the window, and does not discard the mouse message.
    /// </summary>
    public const int MA_NOACTIVATE = 3;

    #endregion

    #region WM_NCCALCSIZE "window valid rect" return values

    /// <summary>
    /// Specifies that the client area of the window is to be preserved and aligned with the top of 
    /// the new position of the window. For example, to align the client area to the upper-left corner, 
    /// return the WVR_ALIGNTOP and WVR_ALIGNLEFT values.
    /// </summary>
    public const int WVR_ALIGNTOP = 0x0010;

    /// <summary>
    /// Specifies that the client area of the window is to be preserved and aligned with the left side 
    /// of the new position of the window. For example, to align the client area to the lower-left 
    /// corner, return the WVR_ALIGNLEFT and WVR_ALIGNBOTTOM values.
    /// </summary>
    public const int WVR_ALIGNLEFT = 0x0020;

    /// <summary>
    /// Specifies that the client area of the window is to be preserved and aligned with the bottom of 
    /// the new position of the window. For example, to align the client area to the top-left corner, 
    /// return the WVR_ALIGNTOP and WVR_ALIGNLEFT values.
    /// </summary>
    public const int WVR_ALIGNBOTTOM = 0x0040;

    /// <summary>
    /// Specifies that the client area of the window is to be preserved and aligned with the right side
    /// of the new position of the window. For example, to align the client area to the upper-right
    /// corner, return the WVR_ALIGNRIGHT and WVR_ALIGNTOP.
    /// </summary>
    public const int WVR_ALIGNRIGHT = 0x0080;

    #endregion
  }
  #endregion

  #region public class WinGdi
  /// <summary>
  /// Defines constants declared in WinGdi.h.
  /// </summary>
  public class WinGdi {
    #region AlphaFormat

    /// <summary>
    /// Currently defined blend function.
    /// </summary>
    public const int AC_SRC_OVER = 0;

    /// <summary>
    /// Alpha format flags.
    /// </summary>
    public const int AC_SRC_ALPHA = 1;

    #endregion
  }
  #endregion
}
