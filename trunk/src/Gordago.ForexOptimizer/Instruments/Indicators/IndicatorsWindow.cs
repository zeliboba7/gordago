/**
* @version $Id: IndicatorsWindow.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.FO.Instruments
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Drawing;
  using System.Data;
  using System.Text;
  using System.Windows.Forms;
  using Gordago.Docking;

  partial class IndicatorsWindow : DockWindowPanel {

    #region class IndicatorTreeNode : TreeNode
    class IndicatorTreeNode : TreeNode {
      private readonly IndicatorItem _indicator;

      public IndicatorTreeNode(IndicatorItem indicator):base(indicator.Name) {
        _indicator = indicator;
      }

      public IndicatorItem Indicator {
        get { return _indicator; }
      }
    }
    #endregion

    public IndicatorsWindow() {
      InitializeComponent();
    }

    #region protected override void OnLoad(EventArgs e)
    protected override void OnLoad(EventArgs e) {
      base.OnLoad(e);
      this.Text = this.TabText = Global.Languages["Indicators Window"]["Indicators"];
      this.UpdateList();
    }
    #endregion

    #region private void UpdateList()
    private void UpdateList() {
      this._treeView.Nodes.Clear();
      foreach (IndicatorItem indicator in Global.Indicators) 
        _treeView.Nodes.Add(new IndicatorTreeNode(indicator));

    }
    #endregion

    #region private void _treeView_ItemDrag(object sender, ItemDragEventArgs e)
    private void _treeView_ItemDrag(object sender, ItemDragEventArgs e) {
      this.DoDragDrop((e.Item as IndicatorTreeNode).Indicator, DragDropEffects.All);
    }
    #endregion
  }
}
