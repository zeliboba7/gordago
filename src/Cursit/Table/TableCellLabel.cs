/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Drawing;

namespace Cursit.Table {
	public class TableCellLabel: TableCell {

		private Image _image;
		
		internal TableCellLabel(TableControl table, TableColumn column): base(table, column){ }

    #region public Image Image
    public Image Image{
			get{return this._image;}
			set{this._image = value;}
    }
    #endregion

    public override void Paint(Graphics g, TableCellStyle style, int x, int y, int w, int h) {
			int dx = 0;
      if (_image != null) {
        dx = _image.Width + 1;
        int by = (this.Height - _image.Height) / 2;
        int iw = Math.Min(_image.Width, this.Width);
        int ih = Math.Min(_image.Height, this.Height);
        g.DrawImageUnscaled(_image, x, y + by, w, h);
      }
			RectangleF rectf = new RectangleF(x+dx, y, w - dx, h);
			g.DrawString(base.Text, style.Font, style.ForeBrush, rectf, style.StringFormat); 
		}
	}
}
