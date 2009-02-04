using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Gordago.PlugIn.ImpExpHistory {
  public class IEHPlugin : PlugInModule, IPlugIn {

    private IMainForm _mainForm;
    private ToolStripMenuItem _mniMain;
    private static ILangDictionary _langItems;
    private bool _restartProgram = false;

    #region public ToolStripMenuItem MenuItem
    public ToolStripMenuItem MenuItem {
      get { return _mniMain; }
    }
    #endregion

    #region public ToolStrip Toolbar
    public ToolStrip Toolbar {
      get { return null; }
    }
    #endregion

    public override bool OnLoad(IMainForm mainForm) {
      _mainForm = mainForm;

      if (mainForm.Lang == "rus") {
        _langItems = new LangDictRussian();
      } else {
        _langItems = new LangDictEnglish();
      }

      _mniMain = new ToolStripMenuItem(_langItems.MenuItem);
      _mniMain.Click += new EventHandler(_mniMain_Click);

      return true;
    }

    private void _mniMain_Click(object sender, EventArgs e) {
      ImportForm iform = new ImportForm();
      iform.ShowDialog();
    }
  }
}
