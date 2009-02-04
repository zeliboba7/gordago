/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region Using
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Language;
using Gordago.API;
#endregion
using System.Collections.Generic;

using System.Threading;
using Cursit.Applic.AConfig;
using System.Xml;

namespace Gordago.Strategy {
	class SULines : UserControl, IBrokerEvents{

		#region private property
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.GroupBox _gboxUseS;
		private Gordago.Strategy.SULine _suline0;
		private Gordago.Strategy.SULine _suline1;
		private Gordago.Strategy.SULine _suline2;
		private Gordago.Strategy.SULine _suline3;
    private Gordago.Strategy.SULine _suline4;
    private System.Windows.Forms.Label _lblstrategy;
    private System.Windows.Forms.Label _lblsymbol;
    #endregion

    private SULine[] _sulines;
		private SUStrategyFileList[] _sufiles;
    //private OnlineServer _server;
    private GroupBox _gboxList;
    private Button _btnAddUS;
    private ListView _lstustrategy;
    private ColumnHeader _clmnfn;
    private ColumnHeader _clmnfilepath;
    private Button _btnDelUS;
    private Button _btnEditUS;

    private string FILEFILTER = "Gordago strategy (*.gso)|*.gso|All files (*.*)|*.*";

    private string _filename;
    private int _nextId = 1;

		#region public SULines() 
		public SULines() {

      _filename = Application.StartupPath + "\\strategy\\OnlineStrategy.xml";
      _sufiles = new SUStrategyFileList[0];

			InitializeComponent();
			_sulines = new SULine[]{
															 _suline0,
															 _suline1,
															 _suline2,
															 _suline3,
															 _suline4
														 };
			
			int i=1;
			foreach (SULine suline in _sulines){
				suline.NumberLine = i++;
				suline.ParametersChanged += new SULineHandler(this.SULineParamtersChanged);
			}

      _sufiles = new SUStrategyFileList[] { };
      try {
        _gboxList.Text = Language.Dictionary.GetString(28, 2, "List");
        _clmnfn.Text = Language.Dictionary.GetString(28, 3, "File name");
        _clmnfilepath.Text = Language.Dictionary.GetString(28, 4, "Path");
        this._btnAddUS.Text = Language.Dictionary.GetString(28, 5, "Add");
        this._btnDelUS.Text = Language.Dictionary.GetString(28, 6, "Remove");
        this._btnEditUS.Text = Language.Dictionary.GetString(28, 7, "Edit");

        this._gboxUseS.Text = Language.Dictionary.GetString(28, 8, "Use");
        this._lblstrategy.Text = Language.Dictionary.GetString(28, 9, "Strategy");
        this._lblsymbol.Text = Language.Dictionary.GetString(28, 10, "Symbol");

        foreach(SULine suline in this._sulines) {
          suline.SetInitComponenet(this);
          suline.SynchronizedSymbolList();
        }
        LoadFileList();
      } catch { }
    }
		#endregion

    #region public BrokerCommandManager BCM
    public BrokerCommandManager BCM {
      get { return GordagoMain.MainForm.BCM; }
    }
    #endregion

    #region protected override void Dispose( bool disposing )
    protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if(components != null) {
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
      this._gboxUseS = new System.Windows.Forms.GroupBox();
      this._lblstrategy = new System.Windows.Forms.Label();
      this._suline0 = new Gordago.Strategy.SULine();
      this._suline1 = new Gordago.Strategy.SULine();
      this._suline2 = new Gordago.Strategy.SULine();
      this._suline3 = new Gordago.Strategy.SULine();
      this._suline4 = new Gordago.Strategy.SULine();
      this._lblsymbol = new System.Windows.Forms.Label();
      this._gboxList = new System.Windows.Forms.GroupBox();
      this._btnAddUS = new System.Windows.Forms.Button();
      this._lstustrategy = new System.Windows.Forms.ListView();
      this._clmnfn = new System.Windows.Forms.ColumnHeader();
      this._clmnfilepath = new System.Windows.Forms.ColumnHeader();
      this._btnDelUS = new System.Windows.Forms.Button();
      this._btnEditUS = new System.Windows.Forms.Button();
      this._gboxUseS.SuspendLayout();
      this._gboxList.SuspendLayout();
      this.SuspendLayout();
      // 
      // _gboxUseS
      // 
      this._gboxUseS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._gboxUseS.Controls.Add(this._lblstrategy);
      this._gboxUseS.Controls.Add(this._suline0);
      this._gboxUseS.Controls.Add(this._suline1);
      this._gboxUseS.Controls.Add(this._suline2);
      this._gboxUseS.Controls.Add(this._suline3);
      this._gboxUseS.Controls.Add(this._suline4);
      this._gboxUseS.Controls.Add(this._lblsymbol);
      this._gboxUseS.Location = new System.Drawing.Point(233, 0);
      this._gboxUseS.Name = "_gboxUseS";
      this._gboxUseS.Size = new System.Drawing.Size(464, 152);
      this._gboxUseS.TabIndex = 2;
      this._gboxUseS.TabStop = false;
      this._gboxUseS.Text = "Use";
      // 
      // _lblstrategy
      // 
      this._lblstrategy.AutoSize = true;
      this._lblstrategy.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this._lblstrategy.Location = new System.Drawing.Point(8, 16);
      this._lblstrategy.Name = "_lblstrategy";
      this._lblstrategy.Size = new System.Drawing.Size(46, 13);
      this._lblstrategy.TabIndex = 2;
      this._lblstrategy.Text = "Strategy";
      // 
      // _suline0
      // 
      this._suline0.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._suline0.Location = new System.Drawing.Point(8, 32);
      this._suline0.Name = "_suline0";
      this._suline0.NumberLine = 0;
      this._suline0.Size = new System.Drawing.Size(448, 24);
      this._suline0.SUStrategyFileList = null;
      this._suline0.SymbolName = "";
      this._suline0.TabIndex = 0;
      // 
      // _suline1
      // 
      this._suline1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._suline1.Location = new System.Drawing.Point(8, 55);
      this._suline1.Name = "_suline1";
      this._suline1.NumberLine = 0;
      this._suline1.Size = new System.Drawing.Size(448, 24);
      this._suline1.SUStrategyFileList = null;
      this._suline1.SymbolName = "";
      this._suline1.TabIndex = 0;
      // 
      // _suline2
      // 
      this._suline2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._suline2.Location = new System.Drawing.Point(8, 78);
      this._suline2.Name = "_suline2";
      this._suline2.NumberLine = 0;
      this._suline2.Size = new System.Drawing.Size(448, 24);
      this._suline2.SUStrategyFileList = null;
      this._suline2.SymbolName = "";
      this._suline2.TabIndex = 0;
      // 
      // _suline3
      // 
      this._suline3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._suline3.Location = new System.Drawing.Point(8, 101);
      this._suline3.Name = "_suline3";
      this._suline3.NumberLine = 0;
      this._suline3.Size = new System.Drawing.Size(448, 24);
      this._suline3.SUStrategyFileList = null;
      this._suline3.SymbolName = "";
      this._suline3.TabIndex = 0;
      // 
      // _suline4
      // 
      this._suline4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._suline4.Location = new System.Drawing.Point(8, 124);
      this._suline4.Name = "_suline4";
      this._suline4.NumberLine = 0;
      this._suline4.Size = new System.Drawing.Size(448, 24);
      this._suline4.SUStrategyFileList = null;
      this._suline4.SymbolName = "";
      this._suline4.TabIndex = 0;
      // 
      // _lblsymbol
      // 
      this._lblsymbol.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this._lblsymbol.AutoSize = true;
      this._lblsymbol.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this._lblsymbol.Location = new System.Drawing.Point(308, 16);
      this._lblsymbol.Name = "_lblsymbol";
      this._lblsymbol.Size = new System.Drawing.Size(41, 13);
      this._lblsymbol.TabIndex = 2;
      this._lblsymbol.Text = "Symbol";
      // 
      // _gboxList
      // 
      this._gboxList.Controls.Add(this._btnAddUS);
      this._gboxList.Controls.Add(this._lstustrategy);
      this._gboxList.Controls.Add(this._btnDelUS);
      this._gboxList.Controls.Add(this._btnEditUS);
      this._gboxList.Location = new System.Drawing.Point(3, 0);
      this._gboxList.Name = "_gboxList";
      this._gboxList.Size = new System.Drawing.Size(230, 152);
      this._gboxList.TabIndex = 4;
      this._gboxList.TabStop = false;
      this._gboxList.Text = "List";
      // 
      // _btnAddUS
      // 
      this._btnAddUS.Location = new System.Drawing.Point(8, 123);
      this._btnAddUS.Name = "_btnAddUS";
      this._btnAddUS.Size = new System.Drawing.Size(70, 23);
      this._btnAddUS.TabIndex = 1;
      this._btnAddUS.Text = "Add";
      this._btnAddUS.UseVisualStyleBackColor = false;
      this._btnAddUS.Click += new System.EventHandler(this._btnAddUS_Click);
      // 
      // _lstustrategy
      // 
      this._lstustrategy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._lstustrategy.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._clmnfn,
            this._clmnfilepath});
      this._lstustrategy.FullRowSelect = true;
      this._lstustrategy.Location = new System.Drawing.Point(8, 16);
      this._lstustrategy.MultiSelect = false;
      this._lstustrategy.Name = "_lstustrategy";
      this._lstustrategy.Size = new System.Drawing.Size(216, 100);
      this._lstustrategy.TabIndex = 0;
      this._lstustrategy.UseCompatibleStateImageBehavior = false;
      this._lstustrategy.View = System.Windows.Forms.View.Details;
      // 
      // _clmnfn
      // 
      this._clmnfn.Text = "File name";
      this._clmnfn.Width = 176;
      // 
      // _clmnfilepath
      // 
      this._clmnfilepath.Text = "Path";
      this._clmnfilepath.Width = 500;
      // 
      // _btnDelUS
      // 
      this._btnDelUS.Location = new System.Drawing.Point(81, 123);
      this._btnDelUS.Name = "_btnDelUS";
      this._btnDelUS.Size = new System.Drawing.Size(70, 23);
      this._btnDelUS.TabIndex = 1;
      this._btnDelUS.Text = "Remove";
      this._btnDelUS.UseVisualStyleBackColor = false;
      this._btnDelUS.Click += new System.EventHandler(this._btnDelUS_Click);
      // 
      // _btnEditUS
      // 
      this._btnEditUS.Location = new System.Drawing.Point(154, 123);
      this._btnEditUS.Name = "_btnEditUS";
      this._btnEditUS.Size = new System.Drawing.Size(70, 23);
      this._btnEditUS.TabIndex = 1;
      this._btnEditUS.Text = "Edit";
      this._btnEditUS.UseVisualStyleBackColor = false;
      this._btnEditUS.Click += new System.EventHandler(this._btnEditUS_Click);
      // 
      // SULines
      // 
      this.BackColor = System.Drawing.Color.White;
      this.Controls.Add(this._gboxList);
      this.Controls.Add(this._gboxUseS);
      this.Name = "SULines";
      this.Size = new System.Drawing.Size(697, 156);
      this._gboxUseS.ResumeLayout(false);
      this._gboxUseS.PerformLayout();
      this._gboxList.ResumeLayout(false);
      this.ResumeLayout(false);

		}
		#endregion

    #region private void SULineParamtersChanged(SULine suline)
    private void SULineParamtersChanged(SULine suline){
			string filename = suline.FileName;
			string sname = suline.SymbolName;

			foreach (SULine sul in _sulines){
				if (suline != sul && sul.FileName == filename && sul.SymbolName == sname){
					suline.SymbolName = "";
					return;
				}
			}
      this.SaveFileList();
    }
    #endregion

    #region public bool Start(CompileDllData cmplData, ISymbol symbol)
    public bool Start(CompileDllData cmplData, ISymbol symbol) {
      return BCM.StrategyEngine.Start(cmplData.Strategy);
    }
    #endregion

    #region public void Stop(CompileDllData cmplData)
    public void Stop(CompileDllData cmplData) {
      this.BCM.StrategyEngine.Stop(cmplData.Strategy);
    }
    #endregion

    #region private void LoadFileList()
    private void LoadFileList() {

      if(!System.IO.File.Exists(_filename)) return;

      XmlDocument doc = new XmlDocument();
      doc.Load(_filename);

      XmlNode node = doc["Gordago"];
      if(node == null) return;

      foreach(XmlNode childnode in node.ChildNodes) {
        if(childnode.Name == "Files") {
          foreach(XmlNode nodeFiles in childnode.ChildNodes) {
            if(nodeFiles.Name == "File") {
              string filename = nodeFiles.InnerText;
              if(System.IO.File.Exists(filename)) {
                int id = Convert.ToInt32(LoadAttribute(nodeFiles, "Id", _nextId.ToString()));
                _nextId = Math.Max(id + 1, _nextId);
                SUStrategyFileList fl = new SUStrategyFileList(filename, id);
                List<SUStrategyFileList> list = new List<SUStrategyFileList>(_sufiles);
                list.Add(fl);
                _sufiles = list.ToArray();
              }
            }
          }
        }
      }

      this.UpdateFileList(false);

      int numline = 0;

      foreach(XmlNode childnode in node.ChildNodes) {
        if(childnode.Name == "List") {
          foreach(XmlNode nodeExec in childnode.ChildNodes) {
            if(nodeExec.Name == "Exec") {
              int id = Convert.ToInt32(LoadAttribute(nodeExec, "Id", "0"));
              string sname = nodeExec.InnerText;
              if(id > 0 && sname.Length > 0) {
                foreach(SUStrategyFileList fl in _sufiles) {
                  if(fl.Id == id) {
                    SULine line = _sulines[numline++];
                    line.SymbolName = sname;
                    line.SUStrategyFileList = fl;
                    break;
                  }
                }
              }
            }
          }
        }
      }
    }
    #endregion

    #region public void SaveFileList()
    public void SaveFileList() {
      Cursit.Utils.FileEngine.CheckDir(_filename);

      string xmlstr = string.Format("<Gordago Version=\"1.0\"></Gordago>");
      XmlDocument doc = new XmlDocument();
      doc.LoadXml(xmlstr);

      XmlNode nodeFiles = doc.CreateElement("Files");
      doc.DocumentElement.AppendChild(nodeFiles);

      for(int i = 0; i < _sufiles.Length; i++) {
        string file = _sufiles[i].FileName;
        if(System.IO.File.Exists(file)) {
          XmlNode nodeFile = doc.CreateElement("File");
          nodeFile.InnerText = file;
          SetValueAttrNode(nodeFile, "Id", _sufiles[i].Id.ToString());
          nodeFiles.AppendChild(nodeFile);
        }
      }

      XmlNode nodeList = doc.CreateElement("List");
      doc.DocumentElement.AppendChild(nodeList);

      for(int i = 0; i < _sulines.Length; i++) {
        SULine line = _sulines[i];
        if(line.SUStrategyFileList != null) {
          XmlNode nodeExec = doc.CreateElement("Exec");

          nodeExec.InnerText = line.SymbolName;
          SetValueAttrNode(nodeExec, "Id", line.SUStrategyFileList.Id.ToString());
          nodeList.AppendChild(nodeExec);
        }
      }
      doc.Save(_filename);
    }
    #endregion

    #region private void UpdateFileList(bool save)
    private void UpdateFileList(bool save) {
      this._lstustrategy.Items.Clear();
      foreach(SUStrategyFileList f in this._sufiles) {
        ListViewItem lvi = new ListViewItem(f.DisplayName);
        lvi.SubItems.Add(f.FileName);
        this._lstustrategy.Items.Add(lvi);
      }
      
      foreach(SULine suline in _sulines) {
        suline.UpdateSUStrategyFileList(_sufiles);
      }

      if (save)
        this.SaveFileList();
    }
    #endregion

    #region private void _btnAddUS_Click(object sender, EventArgs e)
    private void _btnAddUS_Click(object sender, EventArgs e) {
      OpenFileDialog ofdlg = new OpenFileDialog();
      string path = Config.Users["PathStrategy", Application.StartupPath + "\\strategy"];

      ofdlg.Filter = FILEFILTER;
      ofdlg.FilterIndex = 0;
      ofdlg.InitialDirectory = path;
      if(ofdlg.ShowDialog() != DialogResult.OK) return;

      string filename = ofdlg.FileName;
      Config.Users["PathStrategy"].SetValue(System.IO.Directory.GetCurrentDirectory());

      foreach(SUStrategyFileList f in this._sufiles) {
        if(f.FileName == filename)
          return;
      }
      List<SUStrategyFileList> list = new List<SUStrategyFileList>(_sufiles);
      list.Insert(0, new SUStrategyFileList(filename, _nextId++));
      _sufiles = list.ToArray();

      this.UpdateFileList(true);
    }
    #endregion

    #region private void _btnDelUS_Click(object sender, EventArgs e)
    private void _btnDelUS_Click(object sender, EventArgs e) {
      if(this._lstustrategy.SelectedItems.Count != 1)
        return;
      string filename = this._lstustrategy.SelectedItems[0].SubItems[1].Text;

      List<SUStrategyFileList> list = new List<SUStrategyFileList>();

      foreach(SUStrategyFileList sfile in this._sufiles) {
        if(sfile.FileName != filename)
          list.Add(sfile);
      }
      _sufiles = list.ToArray();
      this.UpdateFileList(true);
    }
    #endregion

    #region private void _btnEditUS_Click(object sender, EventArgs e)
    private void _btnEditUS_Click(object sender, EventArgs e) {
      if(this._lstustrategy.SelectedItems.Count != 1)
        return;
      string filename = this._lstustrategy.SelectedItems[0].SubItems[1].Text;
      GordagoMain.MainForm.StrategyManager.OpenFromFile(filename);
    }
    #endregion

    #region private static void SetValueAttrNode(XmlNode node, string name, string value)
    private static void SetValueAttrNode(XmlNode node, string name, string value) {
      node.Attributes.Append(node.OwnerDocument.CreateAttribute(name));
      node.Attributes[name].Value = value;
    }
    #endregion

    #region private static string LoadAttribute(XmlNode node, string name, string defval)
    private static string LoadAttribute(XmlNode node, string name, string defval) {
      XmlAttribute attr = node.Attributes[name];
      if(attr == null || attr.Value == string.Empty) {
        return defval;
      }
      return attr.Value;
    }
    #endregion

    #region IBrokerEvents Members

    public void BrokerConnectionStatusChanged(BrokerConnectionStatus status) {
      foreach (SULine suline in _sulines) {
        suline.BrokerConnectionStatusChanged(status);
      }
    }

    public void BrokerAccountsChanged(BrokerAccountsEventArgs be) {
      throw new Exception("The method or operation is not implemented.");
    }

    public void BrokerOrdersChanged(BrokerOrdersEventArgs be) {
      throw new Exception("The method or operation is not implemented.");
    }

    public void BrokerTradesChanged(BrokerTradesEventArgs be) {
      throw new Exception("The method or operation is not implemented.");
    }

    public void BrokerOnlineRatesChanged(BrokerOnlineRatesEventArgs be) {
      throw new Exception("The method or operation is not implemented.");
    }

    public void BrokerCommandStarting(BrokerCommand command) {
      throw new Exception("The method or operation is not implemented.");
    }

    public void BrokerCommandStopping(BrokerCommand command, BrokerResult result) {
      throw new Exception("The method or operation is not implemented.");
    }

    #endregion
  }

  #region public class SUStrategyFileList
  public class SUStrategyFileList {
    private string _filename;
    private string _displayname;
    private int _id;

    public SUStrategyFileList(string filename, int id) {
      _id = id;
      _filename = filename;
      _displayname = Cursit.Utils.FileEngine.GetFileNameFromPath(filename);
    }

    #region public string FileName
    public string FileName {
      get { return this._filename; }
    }
    #endregion

    #region public string DisplayName
    public string DisplayName {
      get { return this._displayname; }
    }
    #endregion

    #region public int Id
    public int Id {
      get { return _id; }
    }
    #endregion

    #region public override string ToString()
    public override string ToString() {
      return _displayname;
    }
    #endregion
  }
  #endregion
}
