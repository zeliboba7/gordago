using System;
using Language;

namespace LanguageEditor
{
	public class ListViewLangGroupItem: System.Windows.Forms.ListViewItem
	{
		private LangGroup langGroup;

		public ListViewLangGroupItem(LangGroup langGroup)
		{
			this.langGroup = langGroup;
			base.Text = langGroup.Name;
		}


		public int Id{
			get{return langGroup.Id;}
		}
	}
}
