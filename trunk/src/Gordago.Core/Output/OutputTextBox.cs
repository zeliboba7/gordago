/**
* @version $Id: OutputTextBox.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Core
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Windows.Forms;
  using System.IO;

  public class OutputTextBox : RichTextBox {

    private TextWriter _standartOut = null;

    public OutputTextBox() {
      this.OutOn();
    }

    #region protected override void Dispose(bool disposing)
    protected override void Dispose(bool disposing) {
      this.OutOff();
      base.Dispose(disposing);
    }
    #endregion

    #region public void OutOn()
    public void OutOn() {
      if (_standartOut != null)
        return;

      this._standartOut = Console.Out;
      Console.SetOut(new OutputWriter(this));
    }
    #endregion

    #region public void OutOff()
    public void OutOff() {
      if (_standartOut == null)
        return;
      Console.SetOut(_standartOut);
      _standartOut = null;
    }
    #endregion
  }
}
