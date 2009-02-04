/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Gordago.Analysis.Chart {
  public class GDI {

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT {
      public int X;
      public int Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT {
      public int Left;
      public int Top;
      public int Right;
      public int Bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PAINTSTRUCT {
      public IntPtr HDC;
      public int FErase;
      public RECT RCPaint;
      public int FRestore;
      public int FIncUpdate;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
      public byte[] RGBReserved;
    }

    [DllImport("User32.dll", EntryPoint = "BeginPaint")]
    internal static extern IntPtr BeginPaint(IntPtr hwnd, out PAINTSTRUCT lpPaint);

    [DllImport("User32.dll", EntryPoint = "EndPaint")]
    internal static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT lpPaint);


    [DllImport("gdi32.dll", EntryPoint = "SelectObject")]
    internal static extern IntPtr SelectObject(IntPtr hdc, IntPtr hObject);

    [DllImport("gdi32.dll")]
    internal extern static bool DeleteObject(IntPtr hObject);

    [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool TextOut(IntPtr hdc, int xStart, int yStart, string text, int textLen);

    [DllImport("gdi32.dll")]
    internal static extern IntPtr CreatePen(int nStyle, int nWidth, int crColor);

    [DllImport("gdi32.dll")]
    internal static extern IntPtr CreateSolidBrush(int crColor);

    [DllImport("gdi32.dll")]
    internal static extern bool LineTo(IntPtr hdc, int nXEnd, int nYEnd);

    [DllImport("gdi32.dll")]
    internal static extern bool MoveToEx(IntPtr hdc, int x, int y, out POINT lpPoint);

    [DllImport("gdi32.dll")]
    internal static extern bool Polyline(IntPtr hdc, POINT[] lppt, int cCount);

    [DllImport("gdi32.dll", EntryPoint = "Rectangle")]
    internal static extern bool Rectangle(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

    [DllImport("Gdi32.dll")]
    internal static extern int GetPixel(IntPtr hdc, int nXPos, int nYPos);

    public static void DrawLine(IntPtr hdc, Pen pen, int x1, int y1, int x2, int y2) {
      IntPtr ptrPen = CreatePen(0, (int)pen.Width, ColorTranslator.ToWin32(pen.Color));
      IntPtr ptrPenOld = SelectObject(hdc, ptrPen);
      POINT p;
      MoveToEx(hdc, x1, y1, out p);
      LineTo(hdc, x2, y2);

      SelectObject(hdc, ptrPenOld);
      DeleteObject(ptrPen);
    }

    public static void DrawLines(IntPtr hdc, Pen pen, POINT[] point) {
      IntPtr ptrPen = CreatePen(0, (int)pen.Width, ColorTranslator.ToWin32(pen.Color));
      IntPtr ptrPenOld = SelectObject(hdc, ptrPen);

      Polyline(hdc, point, point.Length);

      SelectObject(hdc, ptrPenOld);
      DeleteObject(ptrPen);
    }
  }
}
