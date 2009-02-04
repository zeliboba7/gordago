/**
* @version $Id: LURootFolder.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.IO;
  using System.Reflection;

  public sealed class LURootFolder:LUFolder {
    private LURootFolderType _rootFolderType;

    public LURootFolder(string name, LURootFolderType rootFolderType):base(name) {
      _rootFolderType = rootFolderType;
    }

    #region public LURootFolderType RootFolderType
    public LURootFolderType RootFolderType {
      get { return _rootFolderType; }
      set { _rootFolderType = value;}
    }
    #endregion

    #region public static DirectoryInfo Convert(LURootFolderType rootFolderType)
    public static DirectoryInfo Convert(LURootFolderType rootFolderType) {

      if (rootFolderType == LURootFolderType.Application)
        return (new FileInfo(Assembly.GetExecutingAssembly().Location)).Directory;

      string typeName = rootFolderType.ToString();
      Environment.SpecialFolder spec = (Environment.SpecialFolder)Enum.Parse(typeof(Environment.SpecialFolder), typeName);
      string path = Environment.GetFolderPath(spec);
      return new DirectoryInfo(path);
    }
    #endregion

    public static string Convert(string path) {
      string[] sa = path.Split('\\');
      string rdir = sa[0].Replace("[", "").Replace("]", "");

      DirectoryInfo retDir;
      if (rdir == "Application")
        retDir = Configure.ApplicationDirectory;
      else {
        Environment.SpecialFolder spec = (Environment.SpecialFolder)Enum.Parse(typeof(Environment.SpecialFolder), rdir);
        
        retDir = new DirectoryInfo(Environment.GetFolderPath(spec));
      }

      string retPath = path.Replace(sa[0], retDir.FullName);
      return retPath;
    }
  }
}
