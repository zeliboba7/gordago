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

namespace Gordago.GConfig {
	public class ConfigFormItem : System.Windows.Forms.UserControl {
		private System.ComponentModel.Container components = null;
		private string _text;
		private int _id = -1;
    private ConfigForm _cfgform;

		public ConfigFormItem() {
			InitializeComponent();
			this.Text= "CFIItem";
			
		}

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
			// 
			// ConfigFormItem
			// 
			this.Name = "ConfigFormItem";
			this.Size = new System.Drawing.Size(426, 410);

		}
		#endregion

		#region public override string Text 
		public override string Text {
			get {return this._text;;}
			set {this._text = value;}
		}
		#endregion

    #region public int ID
    public int ID{
			get{return _id;}
			set{_id = value;}
		}
    #endregion

    #region public ConfigForm ConfigForm
    public ConfigForm ConfigForm {
      get { return this._cfgform; }
    }
    #endregion


    public void SetParentForm(ConfigForm cfgform) {
      _cfgform = cfgform;
    }

		public virtual void SaveConfig(){}
	}
}
