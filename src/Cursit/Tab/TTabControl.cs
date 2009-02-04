using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Cursit.Tab {
  [Designer(typeof(ParentControlDesigner))]
  public partial class TTabControl:UserControl {

    public TTabControl() {
      InitializeComponent();
      
    }
  }
}
