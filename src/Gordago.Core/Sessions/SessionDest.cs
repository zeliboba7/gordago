/**
* @version $Id: SessionDest.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Core
{
  using System;
  using System.Collections.Generic;
  using System.Text;

  public class SessionDest {
    private uint _savedSessionIdLevel1 = 0;
    private uint _savedSessionIdLevel2 = 0;
    private uint _savedSessionIdLevel3 = 0;

    private SessionSource _sessionSource;

    public SessionDest(SessionSource sessionSource) {
      this.SetSessionSource(sessionSource);
    }

    public void SetSessionSource(SessionSource sessionSource) {
      _sessionSource = sessionSource;
    }

    #region public int CheckSession()
    public int CheckSession() {
      if (_sessionSource.SessionIdLevel1 != _savedSessionIdLevel1) {
        return 1;
      }else if (_sessionSource.SessionIdLevel2 != _savedSessionIdLevel2) {
        return 2;
      } else if (_sessionSource.SessionIdLevel3 != _savedSessionIdLevel3) {
        return 3;
      }
      return 0;
    }
    #endregion

    #region public void Complete()
    public void Complete() {
      _savedSessionIdLevel1 = _sessionSource.SessionIdLevel1;
      _savedSessionIdLevel2 = _sessionSource.SessionIdLevel2;
      _savedSessionIdLevel3 = _sessionSource.SessionIdLevel3;
    }
    #endregion
  }
}
