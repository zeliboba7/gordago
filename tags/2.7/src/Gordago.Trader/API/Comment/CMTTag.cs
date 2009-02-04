/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.API.Comment {
  public abstract class CMTTag {
    private CMTTagCollection _tags;
    private string _id;
    private string _innerText;

    public CMTTag(string id) {
      _id = id;
      _tags = new CMTTagCollection();
    }

    #region public string Id
    public string Id {
      get { return this._id; }
    }
    #endregion

    #region public string InnerText
    public string InnerText {
      get { return this._innerText; }
      set { _innerText = value; }
    }
    #endregion

    #region public CMTTagCollection Tags
    public CMTTagCollection Tags {
      get { return this._tags; }
    }
    #endregion
  }
}
