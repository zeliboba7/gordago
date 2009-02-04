/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Gordago.Strategy {

  interface ITextBoxObjectElement {
    
    int Width { get;}
    int Height { get;}

    bool Underline { get;}

    void Paint(Graphics g);

  }
}
