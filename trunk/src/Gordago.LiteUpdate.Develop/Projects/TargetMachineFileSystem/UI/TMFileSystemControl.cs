/**
* @version $Id: TMFileSystemControl.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate.Develop.Projects
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Drawing;
  using System.Data;
  using System.Text;
  using System.Windows.Forms;
  using System.IO;

  public partial class TMFileSystemControl : UserControl {

    #region class TMFolderListViewItem : ListViewItem
    class TMFolderListViewItem : ListViewItem {
      private TMFolder _folder;

      public TMFolderListViewItem(TMFolder folder) {
        _folder = folder;
        this.ImageKey = TMFileSystemControl.IMG_FOLDER;
        this.Text = folder is TMRootFolder ? (folder as TMRootFolder).ToString() : folder.Name;
        this.SubItems.Add("Folder");
      }

      #region public TMFolder Folder
      public TMFolder Folder {
        get { return _folder; }
      }
      #endregion
    }
    #endregion

    #region class TMFileListViewItem : ListViewItem
    class TMFileListViewItem : ListViewItem {
      private TMFile _file;
      
      public TMFileListViewItem(TMFile file) {
        _file = file;
        this.Text = file.Name;
        this.SubItems.Add("File");
        this.SubItems.Add(file.Version.ToString());
        this.SubItems.Add(file.ModifyTime.ToShortDateString() + " " + file.ModifyTime.ToShortTimeString());
        this.ImageKey = TMFileSystemControl.IMG_TMFILE;
      }

      #region public TMFile File
      public TMFile File {
        get { return _file; }
      }
      #endregion
    }
    #endregion

    public event EventHandler DataChanged;

    public static readonly string IMG_COMPUTER = "Computer.png";
    public static readonly string IMG_TMFILE = "TMFile.png";
    public static readonly string IMG_FOLDER = "Folder.png";
    public static readonly string IMG_FOLDER_OPEN = "FolderOpen.png";
    public static readonly string IMG_FOLDER_CLOSE = "FolderClose.png";

    public static readonly string IMG_TMFILE_ERROR = "TMFileError.png";
    public static readonly string IMG_FOLDER_OPEN_ERROR = "FolderOpenError.png";
    public static readonly string IMG_FOLDER_CLOSE_ERROR = "FolderCloseError.png";

    private Project _project;

    public TMFileSystemControl() {
      InitializeComponent();

      _lstDetails.Columns[0].Text = Global.Languages["Project/FileSystem"]["Name"];
      _lstDetails.Columns[1].Text = Global.Languages["Project/FileSystem"]["Type"];
      _lstDetails.Columns[2].Text = Global.Languages["Project/FileSystem"]["Version"];
      _lstDetails.Columns[3].Text = Global.Languages["Project/FileSystem"]["Modify"];
      _mniFileDelete.Text = Global.Languages["Project/FileSystem/CM"]["Delete"];
    }

    #region public void SetProject(Project project)
    public void SetProject(Project project) {
      _project = project;
      this.UpdateTreeView();
    }
    #endregion

    #region private void UpdateTreeView()
    private void UpdateTreeView() {

      TreeNode selectNode = this._treeViewFileSystem.SelectedNode;
      string selectedNodeName = selectNode != null ? selectNode.FullPath : "";

      this._treeViewFileSystem.Nodes.Clear();
      this._treeViewFileSystem.BeginUpdate();
      TreeNode rootNode = new TreeNode(Global.Languages["Project/FileSystem"]["File System on Target Machine"]);
      rootNode.SelectedImageKey = rootNode.ImageKey = IMG_COMPUTER;
      this._treeViewFileSystem.Nodes.Add(rootNode);
      rootNode.Expand();

      foreach (TMRootFolder rootFolder in _project.FileSystem) {
        TMRootFolderNode node = new TMRootFolderNode(rootFolder);
        rootNode.Nodes.Add(node);
        node.Expand();
      }
      this._treeViewFileSystem.EndUpdate();

      if (selectedNodeName == "")
        return;
      TreeNode nnode = this.GetNode(rootNode, selectedNodeName);
      if (nnode == null)
        return;
      _treeViewFileSystem.SelectedNode = nnode;
      nnode.Expand();
    }
    #endregion

    #region private TreeNode GetNode(TreeNode node, string fullPath)
    private TreeNode GetNode(TreeNode node, string fullPath) {
      if (node.FullPath == fullPath)
        return node;
      foreach (TreeNode fnode in node.Nodes) {
        if (fnode.FullPath == fullPath)
          return fnode;
        TreeNode nnode = GetNode(fnode, fullPath);
        if (nnode != null)
          return nnode;
      }
      return null;
    }
    #endregion

    #region private void _cxtmTMTreeView_Opening(object sender, CancelEventArgs e)
    private void _cxtmTMTreeView_Opening(object sender, CancelEventArgs e) {
      TreeNode selectNode = this._treeViewFileSystem.SelectedNode;
      if (selectNode == _treeViewFileSystem.Nodes[0]) {
        _cxtmTMTreeView.Items.Clear();
        ToolStripMenuItem menu = new ToolStripMenuItem(Global.Languages["Project/FileSystem/CM"]["Add Special Folder"]);

        foreach (LURootFolderType specFolder in Enum.GetValues(typeof(LURootFolderType))) {
          TMSpecFolderMenuItem menuItem = new TMSpecFolderMenuItem(specFolder, TMContextMenuAction.AddSpecFolder, new EventHandler(tvCMMenuItem_Click));
          menu.DropDownItems.Add(menuItem);
          foreach (TreeNode node in this._treeViewFileSystem.Nodes[0].Nodes) {
            TMRootFolderNode rnode = node as TMRootFolderNode;
            if ((rnode.Folder as TMRootFolder).RootFolderType == specFolder) {
              menuItem.Enabled = false;
              break;
            }
          }
        }
        _cxtmTMTreeView.Items.Add(menu);
      } else if (selectNode is TMFolderNode) {
        _cxtmTMTreeView.Items.Clear();

        ToolStripMenuItem menuAdd = new ToolStripMenuItem(Global.Languages["Project/FileSystem/CM"]["Add"]);
        menuAdd.DropDownItems.AddRange(new ToolStripItem[] { 
          new TMContextMenuItem(TMContextMenuAction.AddFolder, new EventHandler(this.tvCMMenuItem_Click)),
          new TMContextMenuItem(TMContextMenuAction.AddFile, new EventHandler(this.tvCMMenuItem_Click))
        });

        ToolStripMenuItem menuRename = new TMContextMenuItem(TMContextMenuAction.Rename, new EventHandler(this.tvCMMenuItem_Click));

        menuRename.Enabled = !(this._treeViewFileSystem.SelectedNode is TMRootFolderNode);

        _cxtmTMTreeView.Items.AddRange(new ToolStripItem[] { 
          menuAdd,
          //menuRename,
          new TMContextMenuItem(TMContextMenuAction.Delete, new EventHandler(this.tvCMMenuItem_Click))
        });

      } else {
        e.Cancel = true;
      }
    }
    #endregion

    #region private void _treeViewFileSystem_BeforeSelect(object sender, TreeViewCancelEventArgs e)
    private void _treeViewFileSystem_BeforeSelect(object sender, TreeViewCancelEventArgs e) {
      this.UpdateFileListView(e);
    }
    #endregion

    #region private void UpdateFileListView(TreeViewCancelEventArgs e)
    private void UpdateFileListView(TreeViewCancelEventArgs e) {
      this.UpdateFileListView(e.Node);
    }
    #endregion

    #region private void UpdateFileListView()
    private void UpdateFileListView() {
      if (_treeViewFileSystem.SelectedNode != null) {
        this.UpdateFileListView(_treeViewFileSystem.SelectedNode);
      }
    }
    #endregion

    #region private void UpdateFileListView(TreeNode node)
    private void UpdateFileListView(TreeNode node) {
      _lstDetails.Items.Clear();
      if (node == _treeViewFileSystem.Nodes[0]) {
        foreach (TMRootFolder rootFolder in _project.FileSystem) {
          this.ViewDetail(rootFolder);
        }
      } else if (node is TMFolderNode) {
        TMFolderNode folderNode = node as TMFolderNode;
        foreach (TMFolder folder in folderNode.Folder.Folders) {
          this.ViewDetail(folder);
        }
        foreach (TMFile file in folderNode.Folder.Files) {
          this.ViewDetail(file);
        }
      }
    }
    #endregion

    #region private void ViewDetail(object obj)
    private void ViewDetail(object obj) {

      if (obj is TMFile) {
        this._lstDetails.Items.Add(new TMFileListViewItem(obj as TMFile));
      } else {
        this._lstDetails.Items.Add(new TMFolderListViewItem(obj as TMFolder));
      }
    }
    #endregion

    #region private void _treeViewFileSystem_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
    private void _treeViewFileSystem_AfterLabelEdit(object sender, NodeLabelEditEventArgs e) {
      if (e.Node == _treeViewFileSystem.Nodes[0] || 
        e.Node is TMRootFolderNode) {
        e.CancelEdit = true;
      }
    }
    #endregion

    #region protected virtual void OnDataChanged()
    protected virtual void OnDataChanged() {
      if (this.DataChanged != null)
        this.DataChanged(this, EventArgs.Empty);
    }
    #endregion

    #region private void AddFile(TMFolder folder)
    private void AddFile(TMFolder folder) {
      OpenFileDialog ofd = new OpenFileDialog();
      ofd.InitialDirectory = this.Convert(folder).FullName;
      ofd.Multiselect = true;
      if (ofd.ShowDialog() != DialogResult.OK)
        return;

      foreach (string fileName in ofd.FileNames) {
        TMFile tmFile = new TMFile(new FileInfo(fileName), folder);
        folder.Files.Add(tmFile);
      }
      this.UpdateFileListView();
      this.OnDataChanged();
    }
    #endregion

    #region private void AddFolder(TMFolderNode folderNode)
    private void AddFolder(TMFolderNode folderNode) {
      AddFolderForm form = new AddFolderForm();
      if (form.ShowDialog() == DialogResult.Cancel)
        return;
      folderNode.Folder.Folders.Add(new TMFolder(form.FolderName, folderNode.Folder));
      this.UpdateTreeView();
      this.OnDataChanged();
    }
    #endregion

    #region private void DeleteFolder(TMFolderNode folderNode)
    private void DeleteFolder(TMFolderNode folderNode) {
      _treeViewFileSystem.Nodes.Remove(folderNode);
      _project.FileSystem.Remove(folderNode.Folder);
      this.UpdateFileListView();
      this.OnDataChanged();
    }
    #endregion

    #region private void tvCMMenuItem_Click(object sender, EventArgs e)
    private void tvCMMenuItem_Click(object sender, EventArgs e) {
      if (sender is TMSpecFolderMenuItem) {
        TMSpecFolderMenuItem menuItem = sender as TMSpecFolderMenuItem;
        TMRootFolder rootFolder = new TMRootFolder(menuItem.SpecialFolder);

        _project.FileSystem.Add(rootFolder);
        _treeViewFileSystem.Nodes[0].Nodes.Add(new TMRootFolderNode(rootFolder));
        this.OnDataChanged();
      } else if (sender is TMContextMenuItem) {
        TMContextMenuItem menuItem = sender as TMContextMenuItem;
        TMFolderNode folderNode = this._treeViewFileSystem.SelectedNode as TMFolderNode;
        if (folderNode == null || menuItem == null)
          return;

        switch (menuItem.Action) {
          case TMContextMenuAction.Delete:
            this.DeleteFolder(folderNode);
            break;
          case TMContextMenuAction.AddFile:
            this.AddFile(folderNode.Folder);
            break;
          case TMContextMenuAction.AddFolder:
            this.AddFolder(folderNode);
            break;
        }
      }
    }
    #endregion

    #region private DirectoryInfo Convert(TMFolder folder)
    private DirectoryInfo Convert(TMFolder folder) {
      DirectoryInfo dir = null;
      if (folder.Root.RootFolderType == LURootFolderType.Application)
        dir = _project.AppDirectory;
      else
        dir = LURootFolder.Convert(folder.Root.RootFolderType);

      
      string[] sa = folder.FullName.Split('\\');
      for (int i = 1; i < sa.Length; i++) 
        dir = new DirectoryInfo(Path.Combine(dir.FullName, sa[i]));

      return dir;
    }
    #endregion

    #region private TMFolderNode GetNode(TreeNodeCollection nodes, TMFolder folder)
    private TMFolderNode GetNode(TreeNodeCollection nodes, TMFolder folder) {
      foreach (TreeNode node in nodes) {
        TMFolderNode folderNode = node as TMFolderNode;
        if (folderNode != null) {

          if (folder.FullName == folderNode.Folder.FullName)
            return folderNode;
        }
        folderNode = this.GetNode(node.Nodes, folder);
        if (folderNode != null)
          return folderNode;
      }
      return null;
    }
    #endregion

    #region public void ShowError(TMFile file)
    public void ShowError(TMFile file) {

      TMFolderNode node = this.GetNode(this._treeViewFileSystem.Nodes, file.Folder);
      if (node == null)
        return;

      _treeViewFileSystem.SelectedNode = node;

      foreach (ListViewItem lvi in _lstDetails.Items) {
        TMFileListViewItem flvi = lvi as TMFileListViewItem;
        if (flvi != null && flvi.File.FullName == file.FullName) {
          flvi.Selected = true;
          _lstDetails.Focus();
          break;
        }
      }
    }
    #endregion

    #region private void _mniFileDelete_Click(object sender, EventArgs e)
    private void _mniFileDelete_Click(object sender, EventArgs e) {
      if (_lstDetails.SelectedItems.Count != 1)
        return;
      TMFileListViewItem flvi = _lstDetails.SelectedItems[0] as TMFileListViewItem;
      if (flvi == null)
        return;
      flvi.File.Delete();
      this.UpdateFileListView(new TreeViewCancelEventArgs(_treeViewFileSystem.SelectedNode, false, TreeViewAction.Unknown));
    }
    #endregion

    #region private void _cxtmFiles_Opening(object sender, CancelEventArgs e)
    private void _cxtmFiles_Opening(object sender, CancelEventArgs e) {
      if (_lstDetails.SelectedItems.Count != 1) {
        e.Cancel = true;
      } else {
        TMFileListViewItem flvi = _lstDetails.SelectedItems[0] as TMFileListViewItem;
        if (flvi == null)
          e.Cancel = true;
      }
    }
    #endregion

    #region private void _lstDetails_DoubleClick(object sender, EventArgs e)
    private void _lstDetails_DoubleClick(object sender, EventArgs e) {
      if (_lstDetails.SelectedItems.Count != 1)
        return;
      TMFolderListViewItem flvi = _lstDetails.SelectedItems[0] as TMFolderListViewItem;
      if (flvi == null)
        return;
    }
    #endregion
  }
}
