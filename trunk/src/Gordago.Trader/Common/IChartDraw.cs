/**
* @version $Id: IChartDraw.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Common
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Drawing;
  using System.Drawing.Drawing2D;
  using System.Windows.Forms;
  using System.Drawing.Imaging;

  interface IChartDraw {

    Color BackColor { get; set; }
    Font Font { get; set; }
    int FontHeight { get; }
    FontStyle FontStyle { get; set; }
    int FontWidth { get; }
    Color ForeColor { get; set; }
    Graphics Graphics { get; }
    bool IsMonoSpaced { get; }
    bool Opaque { get; set; }
    StringFormat StringFormat { get; set; }
    Color TextColor { get; set; }
    Matrix Transformation { get; }

    void BeginPaint(Graphics graphics);
    int CharWidth(char ch, int count);
    int CharWidth(char ch, int width, out int count);
    void Clear();
    void DrawDotLine(int x1, int y1, int x2, int y2, Color color);
    void DrawEdge(ref Rectangle rect, Border3DStyle border, Border3DSide sides);
    void DrawEdge(ref Rectangle rect, Border3DStyle border, Border3DSide sides, int flags);
    void DrawFocusRect(Rectangle rect, Color color);
    void DrawFocusRect(int x, int y, int width, int height, Color color);
    void DrawImage(ImageList images, int index, Rectangle rect);
    void DrawImage(ImageList images, int index, Rectangle rect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttr);
    void DrawLine(int x1, int y1, int x2, int y2);
    void DrawLine(int x1, int y1, int x2, int y2, Color color, int width, DashStyle penStyle);
    void DrawPolygon(Point[] points, Color color);
    void DrawRectangle(Rectangle rect);
    void DrawRectangle(int x, int y, int width, int height);
    void DrawRoundRectangle(int left, int top, int right, int bottom, int width, int height);
    void DrawText(string text, int len, Rectangle rect);
    void DrawThemeBackground(IntPtr handle, int partID, int stateID, Rectangle rect);
    void DrawWave(Rectangle rect, Color color);
    void EndPaint();
    void EndTransform();
    void ExcludeClipRect(Rectangle rect);
    void ExcludeClipRect(int x, int y, int width, int height);
    void FillGradient(Rectangle rect, Color beginColor, Color endColor, Point point1, Point point2);
    void FillGradient(int x, int y, int width, int height, Color beginColor, Color endColor, Point point1, Point point2);
    void FillPolygon(Color color, Point[] points);
    void FillRectangle(Rectangle rect);
    void FillRectangle(Color color, Rectangle rect);
    void FillRectangle(int x, int y, int width, int height);
    void FillRectangle(Color color, int x, int y, int width, int height);
    void IntersectClipRect(Rectangle rect);
    void IntersectClipRect(int x, int y, int width, int height);
    void RestoreClip(IntPtr rgn);
    IntPtr SaveClip(Rectangle rect);
    void StretchDrawImage(Rectangle rect, Rectangle stretchRect, Rectangle imageRect, Bitmap image);
    int StringWidth(string text);
    int StringWidth(string text, int pos, int len);
    int StringWidth(string text, int width, out int count, bool exact);
    int StringWidth(string text, int pos, int len, int width, out int count);
    int StringWidth(string text, int pos, int len, int width, out int count, bool exact);
    void TextOut(string text, int len, Rectangle rect);
    void TextOut(string text, int len, int x, int y);
    void TextOut(string text, int len, Rectangle rect, bool clipped, bool opaque);
    void TextOut(string text, int len, int x, int y, bool clipped, bool opaque);
    void TextOut(string text, int len, Rectangle rect, int x, int y, bool clipped, bool opaque);
    void TextOut(string text, int len, Rectangle rect, int x, int y, bool clipped, bool opaque, int space);
    void Transform(int x, int y, float scaleX, float scaleY);
  }
}
