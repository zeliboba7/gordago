/**
* @version $Id: OutputWindow.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate.Develop
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

  public partial class OutputWindow : DockWindowPanel {

    public OutputWindow() {
      InitializeComponent();
      this.TabText = Global.Languages["Output"]["Output"];
    }

    public RichTextBox TextBox {
      get { return _outputTextBox; }
    }
  }
}
