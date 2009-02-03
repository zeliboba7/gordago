/**
* @version $Id: RecentFilesManager.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Core.RecentFiles
{
  using System;
  using System.Collections.Generic;
  using System.Windows.Forms;
  using System.IO;

  public class RecentFilesEventArgs:EventArgs {
    private readonly FileInfo _file;

    public RecentFilesEventArgs(FileInfo file):base() {
      _file = file;
    }

    #region public FileInfo File
    public FileInfo File {
      get { return _file; }
    }
    #endregion
  }

  public delegate void RecentFilesEventHanler(object sender, RecentFilesEventArgs e);

  public class RecentFilesManager {

    #region class RecentFileMenuItem : ToolStripMenuItem
    class RecentFileMenuItem : ToolStripMenuItem {

      private readonly FileInfo _file;
      private int _index = 0;

      public RecentFileMenuItem(FileInfo file) {
        _file = file;
        this.Update();
      }

      #region public FileInfo RecentFile
      public FileInfo RecentFile {
        get { return _file; }
      }
      #endregion

      #region public int Index
      public int Index {
        get { return _index; }
        set { 
          _index = value;
          this.Update();
        }
      }
      #endregion

      #region private void Update()
      private void Update() {
        this.Text = string.Format("{0} {1}",this._index, _file.FullName);
      }
      #endregion
    }
    #endregion

    private readonly ToolStripMenuItem _recentMenuItem;
    private readonly EasyPropertiesNode _properties;

    public event RecentFilesEventHanler RecentMenuItemClick;

    public RecentFilesManager(ToolStripMenuItem recentMenuItem, EasyPropertiesNode properties) {
      _recentMenuItem = recentMenuItem;
      _properties = properties;
      _recentMenuItem.DropDownOpening += new EventHandler(_recentMenuItem_DropDownOpening);

      this.Load();
      this.Update();
    }

    #region public ToolStripMenuItem RecentMenuItem
    public ToolStripMenuItem RecentMenuItem {
      get { return _recentMenuItem; }
    }
    #endregion

    #region private void Load()
    private void Load() {
      string[] files = _properties.GetValue<string[]>("Files", new string[] { });
      foreach (string file in files) {
        FileInfo fi = new FileInfo(file);
        
        if (!fi.Exists)
          continue;
        if (this.ExistsBase(fi))
          continue;
        this.Add(fi);
      }
    }
    #endregion

    #region private void Save()
    private void Save() { 
      List<RecentFileMenuItem> list = this.GetArray();
      List<string> files = new List<string>();
      foreach (RecentFileMenuItem rmi in list) {
        files.Add(rmi.RecentFile.FullName);
      }
      _properties.SetValue<string[]>("Files", files.ToArray());
    }
    #endregion

    #region private List<RecentFileMenuItem> GetArray()
    private List<RecentFileMenuItem> GetArray() {
      List<RecentFileMenuItem> list = new List<RecentFileMenuItem>();
      foreach (ToolStripItem menu in _recentMenuItem.DropDownItems) {
        RecentFileMenuItem rmi = menu as RecentFileMenuItem;
        if (rmi == null)
          continue;
        list.Add(rmi);
      }
      return list;
    }
    #endregion

    #region private bool ExistsBase(FileInfo file)
    private bool ExistsBase(FileInfo file) {
      List<RecentFileMenuItem> list = this.GetArray();
      foreach (RecentFileMenuItem rfmi in list) {
        if (rfmi.RecentFile.FullName.ToLower().Equals(file.FullName.ToLower()))
          return true;
      }
      return false;
    }
    #endregion

    #region public void Add(FileInfo file)
    public void Add(FileInfo file) {
      if (this.ExistsBase(file))
        return;

      RecentFileMenuItem rmi = new RecentFileMenuItem(file);
      rmi.Click += new EventHandler(rmi_Click);
      _recentMenuItem.DropDownItems.Add(rmi);
    }
    #endregion

    #region private void Remove(RecentFileMenuItem rmi)
    private void Remove(RecentFileMenuItem rmi) {
      rmi.Click -= new EventHandler(rmi_Click);
      _recentMenuItem.DropDownItems.Remove(rmi);
    }
    #endregion

    #region protected virtual void OnRecentMenuItemClick(RecentFilesEventArgs e)
    protected virtual void OnRecentMenuItemClick(RecentFilesEventArgs e) {
      if (this.RecentMenuItemClick == null) return;
      this.RecentMenuItemClick(this, e);
    }
    #endregion

    #region private void rmi_Click(object sender, EventArgs e)
    private void rmi_Click(object sender, EventArgs e) {
      this.OnRecentMenuItemClick(new RecentFilesEventArgs((sender as RecentFileMenuItem).RecentFile));
    }
    #endregion

    #region private static int CompareByTime(RecentFileMenuItem f1, RecentFileMenuItem f2)
    private static int CompareByTime(RecentFileMenuItem f1, RecentFileMenuItem f2) {
      return f1.RecentFile.LastWriteTime.CompareTo(f2.RecentFile.LastWriteTime);
    }
    #endregion

    #region public void Update()
    public void Update() {
      List<RecentFileMenuItem> list = this.GetArray();
      
      foreach (RecentFileMenuItem rmi in list) {
        rmi.RecentFile.Refresh();
        if (!rmi.RecentFile.Exists) 
          this.Remove(rmi);
      }
      list = this.GetArray();

      list.Sort(CompareByTime);
      for (int i = 0; i < list.Count; i++) {
        RecentFileMenuItem rmi = list[i];
        if (i < 5) {
          rmi.Index = i + 1;
        } else {
          this.Remove(rmi);
        }
      }
      this._recentMenuItem.DropDownItems.Clear();
      this._recentMenuItem.DropDownItems.AddRange(list.ToArray());
      this.Save();
    }
    #endregion

    #region private void _recentMenuItem_DropDownOpening(object sender, EventArgs e)
    private void _recentMenuItem_DropDownOpening(object sender, EventArgs e) {
      this.Update();
    }
    #endregion
  }
}
