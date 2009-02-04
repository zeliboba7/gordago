/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.IO;
using System.Windows.Forms;
using System.Collections;
using Cursit.Applic.AConfig;

namespace Gordago.Strategy {

	public class StrategyManager {

		private Form[] _wfs;

		private const string _fileExtend = "gso";
		private const string _defDir = "\\strategy";
		public const string FILE_FILTER = "Gordago Strategy (*.gso)|*.gso|Gordago DLL Strategy (*.dll)|*.dll" ;

		public StrategyManager() {
			_wfs = new Form[]{};
		}

    public Form GetStrategyForm(string filename) {
			ArrayList al = new ArrayList();

      foreach(Form form in this._wfs) {
        IStrategyForm sf = (IStrategyForm)form;
				if (sf.FileName == filename && !sf.IsDestroy)
					return form;
        if(!sf.IsDestroy)
					al.Add(form);
			}
      _wfs = (Form[])al.ToArray(typeof(Form));
			return null;
		}

		#region public void OpenFromFileDialog()
		public void OpenFromFileDialog(){
			string filename = this.OpenFileDialog();
			if (filename == "")
				return;
			OpenFromFile(filename);
		}
		#endregion

    public Form OpenFromFile(string filename) {
      Form wf = LoadStrategyForm(filename);
      IStrategyForm sf = (IStrategyForm)wf;

      if(sf.IsOpen)
        wf.Activate();
      else
        wf.Show();
      return wf;
		}

    public Form LoadStrategyForm(string filename) {
      Form form = this.GetStrategyForm(filename);
      if(form != null) {
        return form;
      }

      string[] sa = filename.Split('.');
      switch(sa[sa.Length - 1].ToUpper()) {
        case "GSO":
          EditorForm eform = new EditorForm();
          eform.LoadFromFile(filename);
          form = eform;
          break;
        case "DLL":
          StrategyDLLForm dform = new StrategyDLLForm();
          dform.LoadFromFile(filename);
          form = dform;
          break;
      }
      form.MdiParent = GordagoMain.MainForm;

			ArrayList al = new ArrayList(this._wfs);
			al.Add(form);
      _wfs = (Form[])al.ToArray(typeof(Form));
			return form;
		}

		#region public string OpenFileDialog()
		public string OpenFileDialog(){
			OpenFileDialog ofdlg = new OpenFileDialog();
			string path = Config.Users["PathStrategy", Application.StartupPath + _defDir];
			
			ofdlg.Filter = FILE_FILTER;
			ofdlg.FilterIndex = 0;
			ofdlg.InitialDirectory = path;

			if (ofdlg.ShowDialog() != DialogResult.OK) return "";
			
			path = Cursit.Utils.FileEngine.GetDirectory(ofdlg.FileName);

      Config.Users["PathStrategy"].SetValue(path);
			return ofdlg.FileName;
		}
		#endregion
	}
}

