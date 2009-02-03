/**
* @version $Id: ProjectWindow.cs 4 2009-02-03 13:20:59Z AKuzmin $
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
  using WeifenLuo.WinFormsUI.Docking;
  using Gordago.LiteUpdate.Develop.Docking;
  using Gordago.Docking;

  public partial class ProjectWindow : DockWindowPanel {

    #region class VersionFilesModifyTreeNode:VersionTreeNode
    class VersionFilesModifyTreeNode:VersionTreeNode {
      public VersionFilesModifyTreeNode(int version):base(version) {
        this.Text = Global.Languages["ProjectWindow"]["Files Modify"];
      }
    }
    #endregion

    #region class VersionInfoNode : VersionTreeNode
    class VersionInfoNode : VersionTreeNode {
      public VersionInfoNode(int version):base(version) {
        this.Text = Global.Languages["ProjectWindow"]["Version Info"];
        this.ImageKey = ProjectWindow.IMG_TEXT_FILE;
        this.SelectedImageKey = ProjectWindow.IMG_TEXT_FILE;
      }
    }
    #endregion

    #region class VersionTreeNode : TreeNode
    class VersionTreeNode : TreeNode {
      private readonly int _version;
      
      public VersionTreeNode(int version) {
        _version = version;
        this.Text = string.Format(Global.Languages["ProjectWindow"]["Version {0}"],version.ToString());
      }

      public int Version {
        get { return _version; }
      }
    }
    #endregion

    private static readonly string IMG_PROJECT_EXPLORER = "ProjectExplorer.png";
    private static readonly string IMG_PROJECT_PROPERTIES = "ProjectProperties.png";
    private static readonly string IMG_PROJECT_FILE_SYSTEM = "ProjectFileSystem.png";
    private static readonly string IMG_PROJECT_VERSION = "ProjectVersion.png";
    private static readonly string IMG_TEXT_FILE = "TextFile.png";

    private TreeNode _propertiesNode;
    private TreeNode _fileSystemNode;
    private TreeNode _versionNode;

    private Project _project;

    public ProjectWindow() {
      InitializeComponent();
    }

    #region public void SetProject(Project project)
    public void SetProject(Project project) {
      if (_project != null && _project != project) {
        _project.VersionChanged -= new VersionInfoEventHandler(Versions_VersionAdded);
      }

      _project = project;

      this.Text = this.TabText = string.Format(Global.Languages["ProjectWindow"]["Project '{0}'"], (project != null ? project.Name : ""));
      
      _treeView.Nodes.Clear();

      if (project == null)
        return;
      _project.VersionChanged += new VersionInfoEventHandler(Versions_VersionAdded);

      TreeNode rootNode = new TreeNode(this.TabText);
      rootNode.ImageKey = IMG_PROJECT_EXPLORER;
      rootNode.SelectedImageKey = IMG_PROJECT_EXPLORER;
      _treeView.Nodes.Add(rootNode);

      _propertiesNode = new TreeNode(Global.Languages["ProjectWindow"]["Properties"]);
      _propertiesNode.ImageKey = IMG_PROJECT_PROPERTIES;
      _propertiesNode.SelectedImageKey = IMG_PROJECT_PROPERTIES;
      rootNode.Nodes.Add(_propertiesNode);

      _fileSystemNode = new TreeNode(Global.Languages["ProjectWindow"]["File System"]);
      _fileSystemNode.ImageKey = IMG_PROJECT_FILE_SYSTEM;
      _fileSystemNode.SelectedImageKey = IMG_PROJECT_FILE_SYSTEM;
      rootNode.Nodes.Add(_fileSystemNode);

      _versionNode = new TreeNode(Global.Languages["ProjectWindow"]["Versions"]);
      _versionNode.ImageKey = IMG_PROJECT_VERSION;
      _versionNode.SelectedImageKey = IMG_PROJECT_VERSION;

      for (int i = 1; i <= project.Version ; i++) 
        this.AddVersion(i);
      
      rootNode.Nodes.Add(_versionNode);
      rootNode.Expand();
    }
    #endregion

    #region private void Versions_VersionAdded(object sender, VersionInfoEventArgs e)
    private void Versions_VersionAdded(object sender, VersionInfoEventArgs e) {
      this.AddVersion(e.Version.Number);
    }
    #endregion

    #region private void _treeView_DoubleClick(object sender, EventArgs e)
    private void _treeView_DoubleClick(object sender, EventArgs e) {
      TreeNode node = _treeView.SelectedNode;
      if (node == null)
        return;
      if (node == _fileSystemNode) {
        Global.DockManager.ShowFileSystemDocument(_project);
      } else if (node is VersionFilesModifyTreeNode) {
        Global.DockManager.ShowVersionFilesModifyDocument((node as VersionFilesModifyTreeNode).Version);
      } else if (node is VersionInfoNode) {
        Global.DockManager.ShowVersionUserInfoDocument((node as VersionInfoNode).Version);
      } else if (node == _propertiesNode) {
        Global.DockManager.ShowProjectPropertiesDocument(_project);
      }
    }
    #endregion

    #region public void AddVersion(int version)
    public void AddVersion(int version) {
      VersionTreeNode node = new VersionTreeNode(version);
      node.Nodes.Add(new VersionFilesModifyTreeNode(version));
      node.Nodes.Add(new VersionInfoNode(version));
      _versionNode.Nodes.Insert(0, node);
    }
    #endregion
  }
}
