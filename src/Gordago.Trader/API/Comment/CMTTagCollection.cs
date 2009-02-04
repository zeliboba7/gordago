/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.API.Comment {

  public class CMTTagCollection {
    private CMTTag[] _tags;

    public CMTTagCollection() {
      _tags = new CMTTag[0];
    }

    #region public CMTTag this[int index]
    public CMTTag this[int index] {
      get { return this._tags[index]; }
    }
    #endregion

    #region public int Count
    public int Count {
      get { return this._tags.Length; }
    }
    #endregion

    #region public void Add(HtmlTag tag)
    public void Add(CMTTag tag) {
      List<CMTTag> list = new List<CMTTag>(_tags);
      list.Add(tag);
      _tags = list.ToArray();
    }
    #endregion

    #region public void Clear()
    public void Clear() {
      _tags = new CMTTag[0];
    }
    #endregion
  }
}
