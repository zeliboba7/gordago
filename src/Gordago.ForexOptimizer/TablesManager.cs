/**
* @version $Id: TablesManager.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.FO
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Windows.Forms;
  using Gordago.Core;

  class TablesManager {

    private readonly EasyPropertiesNode _props;
    private readonly Dictionary<DataGridView, string> _views = new Dictionary<DataGridView, string>();

    public TablesManager() {
      _props = Global.Properties["Tables"];
    }

    #region private string GetDataGridViewId(DataGridView dgv)
    private string GetDataGridViewId(DataGridView dgv) {
      return dgv.Parent.Name + "." + dgv.Name;
    }
    #endregion

    #region public void InitializeDataGridView(DataGridView dgv)
    public void InitializeDataGridView(DataGridView dgv) {
      string id = this.GetDataGridViewId(dgv);
      this._views.Add(dgv, id);

      EasyPropertiesNode ps = _props[id]["ColumsWidth"];

      foreach (DataGridViewColumn col in dgv.Columns) {
        col.Width = ps.GetValue<int>(col.Name, col.Width);
      }
    }
    #endregion

    #region public void SaveDataGridView(DataGridView dgv)
    public void SaveDataGridView(DataGridView dgv) {
      string id = _views[dgv];
      _views.Remove(dgv);

      EasyPropertiesNode ps = _props[id]["ColumsWidth"];

      foreach (DataGridViewColumn col in dgv.Columns)
        ps.SetValue<int>(col.Name, col.Width);
    }
    #endregion
  }
}
