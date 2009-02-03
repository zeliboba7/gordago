/**
* @version $Id: FileUtility.cs 3 2009-02-03 12:52:09Z AKuzmin $
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

  /// <summary>
  /// FileUtitity
  /// used from FileUtility.cs of SharpDevelop
  /// http://www.sharpdevelop.com/OpenSource/SD/Default.aspx
  /// </summary>
  public class FileUtility {

    readonly static char[] separators = { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar, Path.VolumeSeparatorChar };

    public static bool IsUrl(string path) {
      return path.IndexOf(':') >= 2;
    }

    /// <summary>
    /// Converts a given absolute path and a given base path to a path that leads
    /// from the base path to the absoulte path. (as a relative path)
    /// </summary>
    public static string GetRelativePath(string baseDirectoryPath, string absPath) {
      if (IsUrl(absPath) || IsUrl(baseDirectoryPath)) {
        return absPath;
      }
      try {
        baseDirectoryPath = Path.GetFullPath(baseDirectoryPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
        absPath = Path.GetFullPath(absPath);
      } catch (Exception ex) {
        throw new ArgumentException("GetRelativePath error '" + baseDirectoryPath + "' -> '" + absPath + "'", ex);
      }

      string[] bPath = baseDirectoryPath.Split(separators);
      string[] aPath = absPath.Split(separators);
      int indx = 0;
      for (; indx < Math.Min(bPath.Length, aPath.Length); ++indx) {
        if (!bPath[indx].Equals(aPath[indx], StringComparison.OrdinalIgnoreCase))
          break;
      }

      if (indx == 0) {
        return absPath;
      }

      StringBuilder erg = new StringBuilder();

      if (indx == bPath.Length) {
        //				erg.Append('.');
        //				erg.Append(Path.DirectorySeparatorChar);
      } else {
        for (int i = indx; i < bPath.Length; ++i) {
          erg.Append("..");
          erg.Append(Path.DirectorySeparatorChar);
        }
      }
      erg.Append(String.Join(Path.DirectorySeparatorChar.ToString(), aPath, indx, aPath.Length - indx));
      return erg.ToString();
    }

    /// <summary>
    /// Converts a given relative path and a given base path to a path that leads
    /// to the relative path absoulte.
    /// </summary>
    public static string GetAbsolutePath(string baseDirectoryPath, string relPath) {
      return Path.GetFullPath(Path.Combine(baseDirectoryPath, relPath));
    }

  }
}
