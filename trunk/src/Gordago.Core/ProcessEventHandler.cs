/**
* @version $Id: ProcessEventHandler.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago {

  using System;
  using System.Collections.Generic;
  using System.Text;

  public delegate void ProcessEventHandler(object sender, ProcessEventArgs e);
  
  public class ProcessEventArgs:EventArgs  {

    private readonly int _total;
    private readonly int _current;

    public ProcessEventArgs(int total, int current)
      : base() {
      _total = total;
      _current = current;
    }

    #region public int Total
    public int Total {
      get { return _total; }
    }
    #endregion

    #region public int Current
    public int Current {
      get { return _current; }
    }
    #endregion
  }
}
