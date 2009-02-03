/**
* @version $Id: FileSystemDocument.cs 4 2009-02-03 13:20:59Z AKuzmin $
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
  using Gordago.LiteUpdate.Develop.Docking;
  using WeifenLuo.WinFormsUI.Docking;
  using Gordago.Docking;

  public partial class FileSystemDocument : ProjectDocument {

    private readonly Project _project;

    public FileSystemDocument(Project project) {
      InitializeComponent();
      _project = project;
      this.SetKey(new FileSystemDocumentKey(project));

      _fileSystemControl.SetProject(project);

      this.TabText = Global.Languages["Project/FileSystem"]["File System"];
    }

    #region public Project Project
    public Project Project {
      get { return _project; }
    }
    #endregion

    #region protected override void Dispose(bool disposing)
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }
    #endregion

    #region public void ShowError(TMFile file)
    public void ShowError(TMFile file) {
      _fileSystemControl.ShowError(file);
    }
    #endregion
  }
}
