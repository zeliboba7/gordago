/**
* @version $Id: SessionSource.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Core
{
  using System;
  using System.Collections;
  using System.Text;

  public class SessionSource {
    private uint _sessionIdLevel1 = 0;
    private uint _sessionIdLevel2 = 0;
    private uint _sessionIdLevel3 = 0;

    private SessionSource _owner;

    public SessionSource() { }

    public SessionSource(SessionSource owner) {
      _owner = owner;
    }

    #region public uint SessionIdLevel1
    public uint SessionIdLevel1 {
      get {
        if (_owner == null)
          return _sessionIdLevel1;
        return _sessionIdLevel1 + _owner.SessionIdLevel1;
      }
    }
    #endregion

    #region public uint SessionIdLevel2
    public uint SessionIdLevel2 {
      get {
        if (_owner == null)
          return _sessionIdLevel2;
        return _sessionIdLevel2 + _owner.SessionIdLevel2;
      }
    }
    #endregion

    #region public uint SessionIdLevel3
    public uint SessionIdLevel3 {
      get {
        if (_owner == null)
          return _sessionIdLevel3;
        return _sessionIdLevel3 + _owner.SessionIdLevel3;
      }
    }
    #endregion

    #region public void IncrementsLevel(int level)
    public void IncrementsLevel(int level) {
      switch (level) {
        case 1:
          this.IncrementsLevel1();
          break;
        case 2:
          this.IncrementsLevel2();
          break;
        case 3:
          this.IncrementsLevel3();
          break;
      }
    }
    #endregion

    #region public void IncrementsLevel1 ()
    public void IncrementsLevel1() {
      _sessionIdLevel1++;
      _sessionIdLevel2++;
      _sessionIdLevel3++;
    }
    #endregion

    #region public void IncrementsLevel2()
    public void IncrementsLevel2() {
      _sessionIdLevel2++;
      _sessionIdLevel3++;
    }
    #endregion

    #region public void IncrementsLevel3()
    public void IncrementsLevel3() {
      _sessionIdLevel3++;
    }
    #endregion
  }
}
