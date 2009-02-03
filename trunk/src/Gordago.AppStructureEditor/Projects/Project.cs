/**
* @version $Id: Project.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.AppStructureEditor.Projects
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.IO;
  using Gordago.Core;

  class Project:AppStructure {

    public Project(DirectoryInfo binDirectory) : base(binDirectory) {

    }

    #region public new string AppRootDirRelative
    public new string AppRootDirRelative {
      get {
        return base.AppRootDirRelative;
      }
      set {
        base.AppRootDirRelative = value;
      }
    }
    #endregion

    #region public new string LanguagesDirRel
    public new string LanguagesDirRel {
      get {
        return base.LanguagesDirRel;
      }
      set {
        base.LanguagesDirRel = value;
      }
    }
    #endregion

    #region public void Save()
    public void Save() {
      this.Service.Save(this.File);
    }
    #endregion

    #region public void SetAppRootDirectory(DirectoryInfo dir)
    public void SetAppRootDirectory(DirectoryInfo dir) {
      base.AppRootDirRelative = FileUtility.GetRelativePath(this.BinDirectory.FullName, dir.FullName);
      this.Save();
    }
    #endregion

    #region public void SetLanguagesDirectory(DirectoryInfo dir)
    public void SetLanguagesDirectory(DirectoryInfo dir) {
      base.LanguagesDirRel = FileUtility.GetRelativePath(this.BinDirectory.FullName, dir.FullName);
      this.Save();
    }
    #endregion

    #region public void SetOptionsDirectory(DirectoryInfo dir)
    public void SetOptionsDirectory(DirectoryInfo dir) {
      base.OptionsDirRel = FileUtility.GetRelativePath(this.BinDirectory.FullName, dir.FullName);
      this.Save();
    }
    #endregion
  }

}
