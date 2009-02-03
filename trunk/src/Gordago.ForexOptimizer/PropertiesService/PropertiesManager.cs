/**
* @version $Id: PropertiesManager.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.FO
{
  using System;
  using System.Collections.Generic;
  using System.Text;

  class PropertiesManager {

    private PropertiesWindow _window;

    public PropertiesManager() {
      Global.DockManager.ContentAdded += new EventHandler<WeifenLuo.WinFormsUI.Docking.DockContentEventArgs>(DockManager_ContentAdded);
      Global.DockManager.ContentRemoved += new EventHandler<WeifenLuo.WinFormsUI.Docking.DockContentEventArgs>(DockManager_ContentRemoved);
    }

    #region private void DockManager_ContentAdded(object sender, WeifenLuo.WinFormsUI.Docking.DockContentEventArgs e)
    private void DockManager_ContentAdded(object sender, WeifenLuo.WinFormsUI.Docking.DockContentEventArgs e) {
      if (e.Content is PropertiesWindow)
        _window = e.Content as PropertiesWindow;
    }
    #endregion

    #region private void DockManager_ContentRemoved(object sender, WeifenLuo.WinFormsUI.Docking.DockContentEventArgs e)
    private void DockManager_ContentRemoved(object sender, WeifenLuo.WinFormsUI.Docking.DockContentEventArgs e) {
      if (e.Content is PropertiesWindow)
        _window = null;
    }
    #endregion

    #region public void SetPropertiesObject(object obj)
    public void SetPropertiesObject(object obj) {
      if (_window == null)
        return;
      _window.SetObject(obj);
    }
    #endregion
  }
}
