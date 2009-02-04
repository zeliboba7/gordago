/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Gordago.API {
  public class TTabControl: TabControl {
    
    
    public TTabControl():base(){
      this.DrawMode = TabDrawMode.OwnerDrawFixed;
    }

    protected override void OnDrawItem(DrawItemEventArgs e) {
      //base.OnDrawItem(e);
      e.Graphics.FillRectangle(Brushes.White, e.Bounds); 
      
      StringFormat stringFormat = new StringFormat(); 
      stringFormat.Alignment = StringAlignment.Center; 
      stringFormat.LineAlignment = StringAlignment.Center; 
      e.Graphics.DrawString(this.TabPages[e.Index].Text, Font, Brushes.Black, e.Bounds, stringFormat);
    }

    protected override void OnPaintBackground(PaintEventArgs pevent) {
      //base.OnPaintBackground(pevent);
    }
  }
}
