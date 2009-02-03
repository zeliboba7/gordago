/**
* @version $Id: DockWindowPanel.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Docking
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using WeifenLuo.WinFormsUI.Docking;

  public class DockWindowPanel : DockContent {

    public DockWindowPanel() {
      this.DockAreas = DockAreas.Float | DockAreas.DockLeft | DockAreas.DockRight
                  | DockAreas.DockTop | DockAreas.DockBottom;
      this.HideOnClose = true;
    }
  }
}
