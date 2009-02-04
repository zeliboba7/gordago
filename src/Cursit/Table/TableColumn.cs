/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Drawing;

namespace Cursit.Table {

	public delegate void TableColumnHandler(TableColumn column);

	public class TableColumn {

		private TableColumnType _columntype;

		private int _width;
		private bool _visible;
		private string _caption;
		private static int _minimunwidth = 10;

		private Color _captionforecolor;
		private Brush _captionforebrush;
		private Font _captionfont;
		private ContentAlignment _captiontextalignment;
		private StringFormat _captionstringformat;

		private ContentAlignment _cellalignment;
		private StringFormat _cellstringformat;

    private TableColumnCollection _parent;

    private int _x;
    private string _name = "column";

		public TableColumn(string caption, TableColumnType columntype, int width) {
			_columntype = columntype;
			_caption = caption;
			this.Width = width;
			_visible = true;
			this.CaptionForeColor = Color.Black;
			this.CaptionTextAlignment = ContentAlignment.MiddleLeft;
			this.CellAlignment = ContentAlignment.MiddleLeft;
    }

    #region public string Name
    public string Name {
      get { return this._name; }
      set { this._name = value; }
    }
    #endregion

    #region internal int X
    internal int X {
      get { return _x; }
      set { _x = value; }
    }
    #endregion

    #region internal int XSplit
    internal int XSplit {
      get { return _x + this._width; }
    }
    #endregion

    #region public TableColumnType Type
    public TableColumnType Type{
			get{return this._columntype;}
		}
		#endregion

		#region internal static int MinimunWidth
		internal static int MinimunWidth{
			get{return _minimunwidth;}
			set{_minimunwidth = value;}
		}
		#endregion

		#region public int Width
		public int Width{
			get{return this._width;}
			set{
        int newvalue = Math.Max(value, MinimunWidth);
        this._width = newvalue;
        this.Refresh();
			}
		}
		#endregion

		#region public bool Visible
		public bool Visible{
			get{return this._visible;}
			set{
				this._visible = value;
        this.Refresh();
      }
		}
		#endregion

		#region Caption property

		#region public string Caption
		public string Caption{
			get{return this._caption;}
			set{this._caption = value;}
		}
		#endregion

		#region public Font CaptionFont
		public Font CaptionFont{
			get{return this._captionfont;}
			set{this._captionfont = value;}
		}
		#endregion

		#region public Color CaptionForeColor
		public Color CaptionForeColor{
			get{return this._captionforecolor;}
			set{this._captionforecolor = value;
				this._captionforebrush = new SolidBrush(value);
			}
		}
		#endregion

		#region internal Brush CaptionForeBrush
		internal Brush CaptionForeBrush{
			get{return this._captionforebrush;}
		}
		#endregion

		#region public ContentAlignment CaptionTextAlignment
		public ContentAlignment CaptionTextAlignment{
			get{return this._captiontextalignment;}
			set{
				this._captiontextalignment = value;
				StringAlignment[] sals = TableControl.ConvertContentAlignmentToSAlignment(value);
				_captionstringformat = new StringFormat();
				_captionstringformat.LineAlignment = sals[0];
				_captionstringformat.Alignment = sals[1];
				_captionstringformat.FormatFlags = StringFormatFlags.NoWrap;
			}
		}
		#endregion

		#region internal StringFormat CaptionStringFormat
		internal StringFormat CaptionStringFormat{
			get{return this._captionstringformat;}
		}
		#endregion

		#endregion

    #region public ContentAlignment CellAlignment
    public ContentAlignment CellAlignment{
			get{return this._cellalignment;}
			set{
				this._cellalignment = value;
				StringAlignment[] sals = TableControl.ConvertContentAlignmentToSAlignment(value);
				_cellstringformat = new StringFormat();
				_cellstringformat.LineAlignment = sals[0];
				_cellstringformat.Alignment = sals[1];
				_cellstringformat.FormatFlags = StringFormatFlags.NoWrap;
			}
    }
    #endregion

    #region internal StringFormat CellStringFormat
    internal StringFormat CellStringFormat{
			get{return this._cellstringformat;}
    }
    #endregion

    #region internal void SetParent(TableColumnCollection parent)
    internal void SetParent(TableColumnCollection parent) {
      _parent = parent;

      if (this.Name == "column") {
        this.Name = "column" + ((int)(parent.Count+1)).ToString();
      }
    }
    #endregion

    #region private void Refresh()
    private void Refresh() {
      if (_parent == null)
        return;
      _parent.Refresh();
    }
    #endregion
  }
}
