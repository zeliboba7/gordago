/**
* @version $Id: PropertiesWindow.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.FO
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Drawing;
  using System.Data;
  using System.Text;
  using System.Windows.Forms;
  using Gordago.Docking;

  public partial class PropertiesWindow : DockWindowPanel {
    public PropertiesWindow() {
      InitializeComponent();
    }

    #region public void SetObject(object obj)
    public void SetObject(object obj) {
      _propertyGrid.SelectedObject = obj;
    }
    #endregion

    #region protected override void OnLoad(EventArgs e)
    protected override void OnLoad(EventArgs e) {
      base.OnLoad(e);
      this.Text = this.TabText = Global.Languages["Properties Window"]["Properties"];
    }
    #endregion
  }
}
