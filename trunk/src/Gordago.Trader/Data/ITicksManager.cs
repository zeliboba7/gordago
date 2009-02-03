/**
* @version $Id: ITicksManager.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Data
{
  using System;
  using System.Collections.Generic;
  using System.Text;

  public delegate void TicksManagerTaskEventHandler(object sender, TicksManagerTaskEventArgs e);
  public delegate void TicksManagerTaskProcessEventHandler(object sender, TicksManagerProcessEventArgs e);

  #region public class TicksManagerTaskEventArgs : EventArgs
  public class TicksManagerTaskEventArgs : EventArgs {
    private readonly TicksManagerTask _task;

    public TicksManagerTaskEventArgs(TicksManagerTask task):base() {
      _task = task;
    }

    #region public TicksManagerTask Task
    public TicksManagerTask Task {
      get { return _task; }
    }
    #endregion
  }
  #endregion

  #region public class TicksManagerProcessEventArgs : TicksManagerTaskEventArgs
  public class TicksManagerProcessEventArgs : TicksManagerTaskEventArgs {
    private readonly int _total;
    private readonly int _current;

    public TicksManagerProcessEventArgs(TicksManagerTask task, int total, int current):base(task) {
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
  #endregion

  #region public enum TicksManagerTask
  public enum TicksManagerTask {
    None,
    UpdateHistory,
    UpdateCache,
    DataCaching
  }
  #endregion

  public interface ITicksManager {

    event TicksManagerTaskEventHandler StartTask;
    event TicksManagerTaskEventHandler StopTask;

    event TicksManagerTaskProcessEventHandler TaskProcessChanged;
    
    void Update(Tick[] ticks);
    void Update(Tick[] ticks, bool isHistory);
  }
}
