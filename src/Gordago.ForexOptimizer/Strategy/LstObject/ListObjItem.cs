/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections;
using System.Windows.Forms;

namespace Gordago.LstObject {
	/// <summary>
	/// Summary description for ListGSOViewItem.
	/// </summary>
	public class ListObjItem: ListViewItem {
		object overobj;
		public ListObjItem(string text, int imageIndex, object OverObject):base(text, imageIndex) {
			overobj = OverObject;
		}

		public ListObjItem(string text, int imageIndex):base(text, imageIndex) {
			overobj = null;
		}

		/// <summary>
		/// Дополнительный объект, используется при DranAndDrop
		/// </summary>
		public object OverObject {
			get{return this.overobj;}
			set{overobj = value;}
		}
	}
}
