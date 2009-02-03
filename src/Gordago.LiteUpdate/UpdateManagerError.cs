/**
* @version $Id: UpdateManagerError.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate
{
  using System;
  using System.Collections.Generic;
  using System.Text;

  public enum UpdateManagerError {
    None,
    ConnectionError,
    ServerError,
    UserAbort,
    Unknow
  }

  #region public static class UpdateManagerErrorParse
  public static class UpdateManagerErrorParse {
    public static string ToString(UpdateManagerError error) {
      switch(error){
        case UpdateManagerError.None:
          return "None";
        case UpdateManagerError.ConnectionError:
          return "Connection error";
        case UpdateManagerError.ServerError:
          return "Server error";
        case UpdateManagerError.Unknow:
          return "Unknow error";
      }
      return "Error";
    }
  }
  #endregion
}
