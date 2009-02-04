/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Language;

namespace Gordago.GConfig {
	public class ConfigForm : System.Windows.Forms.Form {
		private System.Windows.Forms.TreeView _trwGroup;
		private System.Windows.Forms.Button _btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Panel pnlFItem;
		private System.ComponentModel.Container components = null;
		
		private int id = 0;
		private ArrayList cfiTNs;

		public ConfigForm(){
			InitializeComponent();
			this.Text = Dictionary.GetString(26,1,"Настройки");
			this.btnCancel.Text = Dictionary.GetString(26,12,"Отмена");
			this._btnOK.Text = Dictionary.GetString(26,11,"OK");
    }
    #region public TreeView ConfigList
    public TreeView ConfigList {
      get { return this._trwGroup; }
    }
    #endregion

    #region public Button ButtonOK
    public Button ButtonOK {
      get { return this._btnOK; }
    }
    #endregion

    #region Windows Form Designer generated code
    protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if(components != null) {
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		private void InitializeComponent()		{
      this.pnlFItem = new System.Windows.Forms.Panel();
      this._trwGroup = new System.Windows.Forms.TreeView();
      this._btnOK = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.pnlFItem.SuspendLayout();
      this.SuspendLayout();
      // 
      // pnlFItem
      // 
      this.pnlFItem.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.pnlFItem.Controls.Add(this._trwGroup);
      this.pnlFItem.Location = new System.Drawing.Point(0, 0);
      this.pnlFItem.Name = "pnlFItem";
      this.pnlFItem.Size = new System.Drawing.Size(632, 416);
      this.pnlFItem.TabIndex = 0;
      // 
      // _trwGroup
      // 
      this._trwGroup.Location = new System.Drawing.Point(0, 0);
      this._trwGroup.Name = "_trwGroup";
      this._trwGroup.Size = new System.Drawing.Size(200, 414);
      this._trwGroup.TabIndex = 0;
      this._trwGroup.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trwGroup_AfterSelect);
      // 
      // _btnOK
      // 
      this._btnOK.Location = new System.Drawing.Point(408, 424);
      this._btnOK.Name = "_btnOK";
      this._btnOK.Size = new System.Drawing.Size(96, 23);
      this._btnOK.TabIndex = 1;
      this._btnOK.Text = "ОК";
      this._btnOK.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(520, 424);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(96, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Отмена";
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      // 
      // ConfigForm
      // 
      this.AcceptButton = this._btnOK;
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(632, 453);
      this.Controls.Add(this._btnOK);
      this.Controls.Add(this.pnlFItem);
      this.Controls.Add(this.btnCancel);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ConfigForm";
      this.ShowInTaskbar = false;
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Load += new System.EventHandler(this.ConfigForm_Load);
      this.pnlFItem.ResumeLayout(false);
      this.ResumeLayout(false);

		}
		#endregion

		#region private void btnCancel_Click(object sender, System.EventArgs e) 
		private void btnCancel_Click(object sender, System.EventArgs e) {
			this.Close();
		}
		#endregion

		#region private void btnOK_Click(object sender, System.EventArgs e) 
		private void btnOK_Click(object sender, System.EventArgs e) {
			int cnt = cfiTNs.Count;
			for (int i=0;i<cnt;i++){
				ConfigFormItem cfgFItem = cfiTNs[i] as ConfigFormItem;
				cfgFItem.SaveConfig();
			}
			this.Close();
		}
		#endregion

		private void ConfigForm_Load(object sender, System.EventArgs e) {
			cfiTNs = new ArrayList();

			AddConfigItem(new CFIGraph(), this._trwGroup.Nodes);
			AddConfigItem(new CFIHistory(), this._trwGroup.Nodes);
			AddConfigItem(new CFIConnect(), this._trwGroup.Nodes);
			this.ChoiseCFIItem(0);
		}

		#region private void AddConfigItem(ConfigFormItem cfgFItem, TreeNodeCollection nodes)
		private void AddConfigItem(ConfigFormItem cfgFItem, TreeNodeCollection nodes){
			id++;
			CFITreeNode node = new CFITreeNode(cfgFItem.Text, id);
			nodes.Add(node);
			cfiTNs.Add(cfgFItem);
			cfgFItem.Location = new Point(201, 1);
			cfgFItem.Visible = false;
			cfgFItem.ID = id;
      cfgFItem.SetParentForm(this);
			this.pnlFItem.Controls.Add(cfgFItem);
		}
		#endregion

		#region private void trwGroup_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e) 
		private void trwGroup_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e) {
			CFITreeNode node = e.Node as CFITreeNode;
			this.ChoiseCFIItem(node.ID);
		}
		#endregion

		#region private void ChoiseCFIItem(int id)
		private void ChoiseCFIItem(int id){
			int cnt = cfiTNs.Count;
			for(int i=0;i<cnt;i++){
				ConfigFormItem cfi = cfiTNs[i] as ConfigFormItem;
				if (cfi.ID == id) cfi.Visible = true;
				else cfi.Visible = false;
			}
		}
		#endregion
	}
	#region internal class CFITreeNode: TreeNode 
	internal class CFITreeNode: TreeNode {
		private int id;
		public CFITreeNode(string Text, int Id):base(Text) {
			id = Id;
		}
		public int ID{
			get{return id;}
		}
	}
	#endregion
}
