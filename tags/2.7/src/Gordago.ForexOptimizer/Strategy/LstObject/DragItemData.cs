/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections;

namespace Gordago.LstObject {
	public class DragItemData {

		private ListObject m_listView;
		private ArrayList m_dragItems;

		public ListObject ListView {
			get { return m_listView; }
		}

		public ArrayList DragItems {
			get { return m_dragItems; }
		}

		public DragItemData(ListObject listView) {
			m_listView = listView;
			m_dragItems = new ArrayList();
		}
	}
}
