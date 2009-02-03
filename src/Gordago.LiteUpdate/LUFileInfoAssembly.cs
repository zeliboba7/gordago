/**
* @version $Id: LUFileInfoAssembly.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate
{
  using System;
  using System.Collections.Generic;
  using System.Text;

  public class LUFileInfoAssembly:LUFileInfo {

    private const int ID = 2;
    private const string EXTENSION = "exe,dll";

    #region public override int Id
    public override int Id {
      get { return ID; }
    }
    #endregion

    #region public override string Extensions
    public override string Extensions {
      get { return EXTENSION; }
    }
    #endregion
  }
}
