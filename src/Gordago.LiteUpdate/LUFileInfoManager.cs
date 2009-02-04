/**
* @version $Id: LUFileInfoManager.cs 3 2009-02-03 12:52:09Z AKuzmin $
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

  public static class LUFileInfoManager {

    #region class LUFileExtension
    class LUFileExtension {

      #region public struct LUFileExtensionKey
      public struct LUFileExtensionKey {

        private int _hashCode;

        public LUFileExtensionKey(string extension) {
          _hashCode = extension.ToUpper().Trim().GetHashCode();
        }

        #region public override bool Equals(object obj)
        public override bool Equals(object obj) {
          if (!(obj is LUFileExtensionKey))
            return false;
          LUFileExtensionKey key = (LUFileExtensionKey)obj;

          return key._hashCode == _hashCode;
        }
        #endregion

        #region public override int GetHashCode()
        public override int GetHashCode() {
          return _hashCode;
        }
        #endregion
      }
      #endregion

      private readonly string _extension;
      private readonly LUFileInfo _luFileInfo;
      private readonly LUFileExtensionKey _key;

      public LUFileExtension(string extension, LUFileInfo luFileInfo) {
        _extension = extension;
        _luFileInfo = luFileInfo;
        _key = new LUFileExtensionKey(extension);
      }

      #region public string Extension
      public string Extension {
        get { return _extension; }
      }
      #endregion

      #region public LUFileInfo LUFileInfo
      public LUFileInfo LUFileInfo {
        get { return _luFileInfo; }
      }
      #endregion

      #region public LUFileExtensionKey Key
      public LUFileExtensionKey Key {
        get { return _key; }
      }
      #endregion
    }
    #endregion

    private static readonly Dictionary<int, LUFileInfo> _LUFileInfos = new Dictionary<int, LUFileInfo>();
    private static readonly Dictionary<LUFileExtension.LUFileExtensionKey, LUFileExtension> _Extensions = new Dictionary<LUFileExtension.LUFileExtensionKey, LUFileExtension>();

    static LUFileInfoManager() {
      Register(typeof(LUFileInfoOver));
      Register(typeof(LUFileInfoAssembly));
    }

    #region public static void Register(Type luFileInfoType)
    public static void Register(Type luFileInfoType) {
      LUFileInfo luFileInfo = Activator.CreateInstance(luFileInfoType) as LUFileInfo;
      if (luFileInfo == null)
        throw (new ArgumentException("Is not LUFileInfo type.", "luFileInfoType"));
      try {
        _LUFileInfos.Add(luFileInfo.Id, luFileInfo);

        string[] sa = luFileInfo.Extensions.Replace(" ", "").Split(',');
        if (sa.Length == 0) {
          sa = new string[] { "" };
        }
        foreach (string ext in sa) {
          LUFileExtension lufExt = new LUFileExtension(ext, luFileInfo);
          if (_Extensions.ContainsKey(lufExt.Key)) 
            throw (new Exception(string.Format("Extension \"{0}\" is already exist in base.", lufExt.Extension)));
          _Extensions.Add(lufExt.Key, lufExt);
        }

      } catch {
        throw (new Exception(string.Format("LUFileInfo.Id={0} already exist", luFileInfo.Id)));
      }
    }
    #endregion

    #region public static LUFileInfo GetLUFileInfo(FileInfo file)
    public static LUFileInfo GetLUFileInfo(FileInfo file) {

      LUFileExtension lufExt = null;
      LUFileExtension.LUFileExtensionKey lufExtKey = new LUFileExtension.LUFileExtensionKey(file.Extension);
      _Extensions.TryGetValue(lufExtKey, out lufExt);

      if (lufExt == null) {
        lufExtKey = new LUFileExtension.LUFileExtensionKey("");
        _Extensions.TryGetValue(lufExtKey, out lufExt);
      }

      if (lufExt == null)
        throw (new Exception("Can`t find extension info"));

      return lufExt.LUFileInfo;
    }
    #endregion
  }
}
