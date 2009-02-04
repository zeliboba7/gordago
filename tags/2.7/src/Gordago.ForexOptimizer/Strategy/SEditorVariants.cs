/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Language;

using Gordago.Strategy.FIndicator;

namespace Gordago.Strategy {
	class SEditorVariants : System.Windows.Forms.UserControl {

		#region private propertyes
		/// <summary>
		/// Необходимо для того, чтобы отслеживать нажатие на индикатор в редакторе,
		/// для того чтоб выводить его свойства
		/// </summary>
		public event System.EventHandler IndicFunctionSelectChanged;

		private string _lngVariant;
		private string _lngBtnAddVariant;

		private TabControl tbcStrategy;
		private Container components = null;

		private SEditorTable[] _seTables;
		private int _nextnumber;

    private EditorForm _eform;
		#endregion

    private ToolStripMenuItem _mniAddVariant, _mniDelVariant;
		public SEditorVariants() {
      _lngVariant = Language.Dictionary.GetString(2, 10, "Вариант");
      _lngBtnAddVariant = Language.Dictionary.GetString(2, 11, "Добавить вариант");
      
      InitializeComponent();

			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
      
      _mniAddVariant = new ToolStripMenuItem(Dictionary.GetString(10,14,"Добавить вариант"),null, new EventHandler(this.tbpCMenu_Click));
      _mniDelVariant = new ToolStripMenuItem(Dictionary.GetString(10,15,"Удалить вариант"), null, new EventHandler(this.tbpCMenu_Click));

			ContextMenuStrip cmenu = new ContextMenuStrip();
      cmenu.Items.AddRange(
        new ToolStripMenuItem []{_mniAddVariant, _mniDelVariant});


			this.tbcStrategy.ContextMenuStrip = cmenu;
			this._nextnumber = 0;
			_seTables = new SEditorTable[]{};
    }

    #region public void SetEditorForm(EditorForm eform)
    public void SetEditorForm(EditorForm eform) {
      _eform = eform;
      this.CraeteNewVariant();
    }
    #endregion

    #region public EditorForm EditorForm
    public EditorForm EditorForm{
			get{return this._eform;}
		}
		#endregion

		#region public SEditorTable[] SETables
		public SEditorTable[] SETables{
			get{return _seTables;}
		}
		#endregion

		#region Component Designer generated code
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if(components != null) {
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		private void InitializeComponent() {
      this.tbcStrategy = new System.Windows.Forms.TabControl();
      this.SuspendLayout();
      // 
      // tbcStrategy
      // 
      this.tbcStrategy.Alignment = System.Windows.Forms.TabAlignment.Bottom;
      this.tbcStrategy.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tbcStrategy.HotTrack = true;
      this.tbcStrategy.Location = new System.Drawing.Point(0, 0);
      this.tbcStrategy.Margin = new System.Windows.Forms.Padding(0);
      this.tbcStrategy.Name = "tbcStrategy";
      this.tbcStrategy.SelectedIndex = 0;
      this.tbcStrategy.Size = new System.Drawing.Size(390, 310);
      this.tbcStrategy.TabIndex = 0;
      // 
      // SEditorVariants
      // 
      this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.Controls.Add(this.tbcStrategy);
      this.Name = "SEditorVariants";
      this.Size = new System.Drawing.Size(390, 310);
      this.ResumeLayout(false);

		}
		#endregion

		#region public void CraeteNewVariant() - создание нового варианта
		public void CraeteNewVariant() {
			ArrayList pnls = new ArrayList(_seTables);
			int cnt = pnls.Count;

			TabPage tbp  = new TabPage();
			tbp.TabIndex = _nextnumber;
			string nameVar = Dictionary.GetString(10,6,"Вариант") + " " + Convert.ToString(_nextnumber+1);
			tbp.Text = nameVar;

			SEditorTable seTable = new SEditorTable(this._eform);
			seTable.VariantName = nameVar;
			seTable.Dock = DockStyle.Fill;
			seTable.TabIndex = cnt;
			
			tbp.Controls.Add(seTable);
			this.tbcStrategy.Controls.Add(tbp);
			this.tbcStrategy.SelectedIndex = this.tbcStrategy.TabCount-1;

			pnls.Add(seTable);
			_seTables = new SEditorTable[pnls.Count];
			pnls.CopyTo(_seTables);
			_nextnumber++;
		}
		#endregion

		#region public void DeleteActiveVariant() - Удаление активного варианта
		/// <summary>
		/// Удаление активного варианта
		/// </summary>
		public void DeleteActiveVariant(){
			if (this.tbcStrategy.TabCount <= 1) return;
			int index = this.tbcStrategy.SelectedIndex;
			
			TabPage tbp = this.tbcStrategy.TabPages[index];
			ArrayList al = new ArrayList(this._seTables);
			al.RemoveAt(this.tbcStrategy.SelectedIndex);
			_seTables = new SEditorTable[al.Count];
			al.CopyTo(_seTables);
			this.tbcStrategy.TabPages.RemoveAt(index);
		}
		#endregion

		#region private void tbpCMenu_Click(object sender, EventArgs e)
		private void tbpCMenu_Click(object sender, EventArgs e){
      ToolStripMenuItem mi = sender as ToolStripMenuItem;
      
      if (mi == _mniAddVariant){
					this.CraeteNewVariant();
        } else if(mi == _mniDelVariant) {
          this.DeleteActiveVariant();
        }
		}
		#endregion

		#region private void IndicFunction_SelecChanged(object sender, System.EventArgs e)
		private void IndicFunction_SelecChanged(object sender, System.EventArgs e){
			if (this.IndicFunctionSelectChanged != null)
				this.IndicFunctionSelectChanged(sender, e);
		}
		#endregion

    #region public IndicFunction[] GetAllIndicFunction()
    public IndicFunction[] GetAllIndicFunction(){
			ArrayList al = new ArrayList();
			foreach (SEditorTable tbl in this._seTables){
				al.AddRange(tbl.GetAllIndicFunction());
			}
			return (IndicFunction[])al.ToArray(typeof(IndicFunction));
    }
    #endregion
  }
}
