/**
* @version $Id: MainForm.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.AppStructureEditor
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Data;
  using System.Drawing;
  using System.Text;
  using System.Windows.Forms;
  using Gordago.AppStructureEditor.Projects;

  public partial class MainForm : Form {

    private readonly ProjectsManager _projectsManager = new ProjectsManager();

    public MainForm() {
      InitializeComponent();
      Global.MainForm = this;
    }

    protected override void OnLoad(EventArgs e) {
      base.OnLoad(e);
      _projectsManager.FileMenuItem = _mniFile;
    }

    private void _mniAbout_Click(object sender, EventArgs e) {
      AboutBox box = new AboutBox();
      box.ShowDialog();
    }
  }
}