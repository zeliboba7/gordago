using System;
using System.Collections;

namespace Cursit.Applic.APropGrid {
			public delegate void PropGridRowsEventHandler(object sender, System.EventArgs e);
		
		/// <summary>
		/// Управляемая коллекция записей в редакторе свойств
		/// </summary>
		public class PropGridRowCollection {

				public event PropGridRowsEventHandler SelectedItemChanged;
				public event PropGridRowsEventHandler SplitterChanged;

				private EventArgs _eventArgs;

				private ArrayList _pgrs;
				private int _currentId;
				public PropGridRowCollection() {
						this._eventArgs = new EventArgs();
						_pgrs = new ArrayList();
						_currentId = 1;
				}

				public int Add(PropGridRowPrimitive propGridRow){
						_pgrs.Add(propGridRow);
						int id = _currentId++;
						propGridRow._id = id;
						propGridRow.SplitterChanged += new PropGridRowEventHandler(this.PropGridRowSplitter_Changed);
						propGridRow.Click += new EventHandler(this.PropGridRow_Click);
						return id;
				}

				public void Clear(){
						this._pgrs.Clear();
						this._currentId = 1;
				}

				private void PropGridRow_Click(object sender, System.EventArgs e){
						PropGridRowPrimitive pgr = sender as PropGridRowPrimitive;
						UnSelectAllRow();
						pgr.IsSelect = true;
						if (this.SelectedItemChanged != null){
								this.SelectedItemChanged(pgr, new System.EventArgs());
						}
				}

				/// <summary>
				/// Событие на изменеия разделителя в PropGridRow.
				/// </summary>
				private void PropGridRowSplitter_Changed(object sender, System.EventArgs e){
						PropGridRowPrimitive pgr = sender as PropGridRowPrimitive;
						int splitt = pgr.Splitter;
						int cnt = this._pgrs.Count;
						for (int i=0;i<cnt;i++){
								PropGridRowPrimitive pgr_v = this._pgrs[i] as PropGridRowPrimitive;
								if (pgr != pgr_v){
										pgr_v.Splitter = splitt;
								}
						}
						if (SplitterChanged != null)
								SplitterChanged(pgr, _eventArgs);
				}

				public void UnSelectAllRow(){
						int cnt = this._pgrs.Count;
						for (int i=0;i<cnt;i++){
								PropGridRowPrimitive pgr_v = this._pgrs[i] as PropGridRowPrimitive;
									pgr_v.IsSelect = false;
						}
				}

				public int Count{
						get{return _pgrs.Count;}
				}

				public PropGridRowPrimitive this[int index]{
						get{
								return _pgrs[index] as PropGridRowPrimitive;
						}
				}
				
				internal int Height{
						get{
								int cnt = _pgrs.Count;
								int h = 0;
								for (int i =0;i<cnt;i++){
										h += this[i].Height;
								}
								return h;
						}
				}
		}
}
