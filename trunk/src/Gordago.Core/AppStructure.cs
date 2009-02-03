/**
* @version $Id: AppStructure.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Core
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.IO;

  public class AppStructure {

    public readonly string STRUCTURE_FILE_NAME = "AppStructure.xml";

    private readonly EasyProperties _props = new EasyProperties();
    private readonly DirectoryInfo _binDirectory;
    private readonly FileInfo _file;

    public AppStructure(DirectoryInfo binDirectory) {
      _binDirectory = binDirectory;
      _file = new FileInfo(Path.Combine(_binDirectory.FullName, STRUCTURE_FILE_NAME));
      this.Reload();
    }

    #region public DirectoryInfo BinDirectory
    public DirectoryInfo BinDirectory {
      get { return _binDirectory; }
    }
    #endregion

    #region public FileInfo File
    public FileInfo File {
      get { return _file; }
    }
    #endregion

    #region public EasyPropertiesNode this[string name]
    public EasyPropertiesNode this[string name] {
      get { return _props[name]; }
    }
    #endregion

    #region protected EasyProperties Service
    protected EasyProperties Service {
      get { return _props; }
    }
    #endregion

    #region public DirectoryInfo AppRootDirectory
    public DirectoryInfo AppRootDirectory {
      get { 
        return new DirectoryInfo(Path.Combine(_binDirectory.FullName, this.AppRootDirRelative)); 
      }
    }
    #endregion

    #region public DirectoryInfo LanguagesDirectory
    public DirectoryInfo LanguagesDirectory {
      get { 
        return new DirectoryInfo(Path.Combine(_binDirectory.FullName, this.LanguagesDirRel)); 
      }
    }
    #endregion

    #region public DirectoryInfo OptionsDirectory
    public DirectoryInfo OptionsDirectory {
      get {
        return new DirectoryInfo(Path.Combine(_binDirectory.FullName, this.OptionsDirRel));
      }
    }
    #endregion

    #region protected virtual string AppRootDirRelative
    protected virtual string AppRootDirRelative {
      get { return _props.GetValue<string>("AppRootDirectory", ""); }
      set { _props.SetValue<string>("AppRootDirectory", value); }
    }
    #endregion

    #region protected virtual string LanguagesDirRel
    protected virtual string LanguagesDirRel {
      get { return _props.GetValue<string>("LanguagesDirectory", "languages"); }
      set { _props.SetValue<string>("LanguagesDirectory", value); }
    }
    #endregion

    #region protected virtual string OptionsDirRel
    protected virtual string OptionsDirRel {
      get { return _props.GetValue<string>("OptionsDirectory", "options"); }
      set { _props.SetValue<string>("OptionsDirectory", value); }
    }
    #endregion

    #region public void Reload()
    public void Reload() {
      _props.Clear();
      _props.Load(_file);
    }
    #endregion
  }
}
