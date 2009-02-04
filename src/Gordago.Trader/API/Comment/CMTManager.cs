/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Gordago.API.Comment {

  public class CMTManager {
    private CMTTagCollection _tags;
    
    public CMTManager() {
      _tags = new CMTTagCollection();
    }

    public void Write(string htmlText) {
      _tags = new CMTTagCollection();
    }
  }

}
