using System;
using System.Windows.Forms;

using Language;

namespace LanguageEditor
{
	/// <summary>
	/// Summary description for ListViewLangItem.
	/// </summary>
	public class ListViewLangItem: ListViewItem
	{
		/// <summary>
		/// Идентификатор элемента
		/// </summary>
		private LangValue langValue;

		public ListViewLangItem(LangValue langValue):base()
		{
			this.langValue = langValue;
			base.Text = this.langValue.Text;
		}

		public new string Text{
			get{return base.Text;}
			set{
				base.Text = value;
				this.langValue.Text = value;
			}
		}
		public int Id{
			get{return this.langValue.Id;}
		}
	}
}
