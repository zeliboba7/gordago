/**
* @version $Id: ProjectDocument.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate.Develop.Projects
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using Gordago.Docking;

  #region public class FileSystemDocumentKey : ProjectDocumtenKey
  public class FileSystemDocumentKey : ProjectDocumtenKey {
    public FileSystemDocumentKey(Project project):base(project, "fs") {
    }
  }
  #endregion

  #region public class ProjectPropertiesDocumentKey : ProjectDocumtenKey
  public class ProjectPropertiesDocumentKey : ProjectDocumtenKey {
    public ProjectPropertiesDocumentKey(Project project)
      : base(project, "pp") {

    }
  }
  #endregion

  #region public class ProjectDocumtenKey : TabbedDocument.AbstractDocumentKey
  public class ProjectDocumtenKey : TabbedDocument.AbstractDocumentKey {
    private readonly int _hashCode;

    public ProjectDocumtenKey(Project project, string prefix) {
      _hashCode = (project.GUID+prefix).GetHashCode();
    }

    public override int GetHashCode() {
      return _hashCode;
    }

    public override bool Equals(object obj) {
      if (!(obj is ProjectDocumtenKey))
        return false;

      return (obj as ProjectDocumtenKey)._hashCode == _hashCode;
    }
  }
  #endregion

  public class ProjectDocument : TabbedDocument {
    
    
    public ProjectDocument() {
      Global.Builder.StartBuild += new BuilderEventHandler(Builder_StartBuild);
      Global.Builder.StopBuild += new BuilderEventHandler(Builder_StopBuild);
    }

    #region protected override void Dispose(bool disposing)
    protected override void Dispose(bool disposing) {
      Global.Builder.StartBuild -= new BuilderEventHandler(Builder_StartBuild);
      Global.Builder.StopBuild -= new BuilderEventHandler(Builder_StopBuild);
      base.Dispose(disposing);
    }
    #endregion

    #region private void Builder_StopBuild(object sender, BuilderEventArgs e)
    private void Builder_StopBuild(object sender, BuilderEventArgs e) {
      if (!this.InvokeRequired) {
        this.Enabled = true;
      } else {
        this.Invoke(new BuilderEventHandler(Builder_StopBuild), sender, e);
      }
    }
    #endregion

    #region private void Builder_StartBuild(object sender, BuilderEventArgs e)
    private void Builder_StartBuild(object sender, BuilderEventArgs e) {
      if (!this.InvokeRequired) {
        this.Enabled = false;
      } else {
        this.Invoke(new BuilderEventHandler(Builder_StartBuild), sender, e);
      }
    }
    #endregion

  }
}
