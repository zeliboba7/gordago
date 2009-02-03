/**
* @version $Id: VersionFilesModifyControl.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Gordago.LiteUpdate.Develop.Projects.Versions {
  public partial class VersionFilesModifyControl : UserControl {

    private VersionInfo _version;
    
    public VersionFilesModifyControl() {
      InitializeComponent();

      try {
        _filesUpdateControl.Text = Global.Languages["Project/VFM"]["Updates"];
        _filesAddControl.Text = Global.Languages["Project/VFM"]["Add"];
        _filesRemoveControl.Text = Global.Languages["Project/VFM"]["Remove"];
      } catch { }
    }

    #region public bool ReadOnly
    public bool ReadOnly {
      get { return _filesUpdateControl.ReadOnly; }
      set {
        _filesUpdateControl.ReadOnly =
          _filesAddControl.ReadOnly =
          _filesRemoveControl.ReadOnly = value; 
      }
    }
    #endregion

    #region public void SetVersion(VersionInfo version)
    public void SetVersion(VersionInfo version) {
      _version = version;
      _filesAddControl.SetFiles(version.Modify.FilesAdded);
      _filesRemoveControl.SetFiles(version.Modify.FilesRemoved);
      _filesUpdateControl.SetFiles(version.Modify.FilesUpdated);
    }
    #endregion
  }
}
