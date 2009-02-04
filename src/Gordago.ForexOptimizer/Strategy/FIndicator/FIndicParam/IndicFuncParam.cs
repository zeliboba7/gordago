/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using Gordago.Analysis;
using Cursit.Applic.APropGrid;

namespace Gordago.Strategy.FIndicator.FIndicParam {
	/// <summary>
	/// ������� ����� ��� ��������� �������
	/// </summary>
	public abstract class IndicFuncParam {

		public event System.EventHandler ParamValue_Changed;
		private object _value;
		private string _caption;
		private DocParameterUsing _using;
		private Parameter _parameter;

		private CompVar _compVar;
		private string _rptstr = "";

		public IndicFuncParam(Parameter parameter) {
			_parameter = parameter;
			string lng = parameter.Captions[0];
			if (GordagoMain.Lang == "rus" && parameter.Captions.Length == 2)
				lng = parameter.Captions[1];
				
			_caption = lng;

		}

		#region public Parameter Parameter
		public Parameter Parameter{
			get{return this._parameter;}
		}
		#endregion

		#region public DocParameterUsing Using
		public DocParameterUsing Using{
			get{return _using;}
			set{_using = value;}
		}
		#endregion

		#region public string Name
		public string Name{
			get{return this._parameter.Name;}
		}
		#endregion

		#region public string ReportString
		public string ReportString{
			get{return this._rptstr;}
			set{this._rptstr = value;}
		}
		#endregion

		#region public virtual object Value
		public virtual object Value{
			get{return _value;}
			set{
				this._value = value;
			}
		}
		#endregion

		#region public string Caption
		/// <summary>
		/// ������������ ���������
		/// </summary>
		public string Caption{
			get{return _caption;}
		}
		#endregion

		#region internal CompVar CompVar - ��������� ��������� ����������, � �� �� ��� �� ���� �������� ��� ������
		/// <summary>
		/// ��������� ��������� ����������, � �� �� ��� �� ���� �������� ��� ������
		/// </summary>
		internal CompVar CompVar{
			get{return this._compVar;}
			set{this._compVar = value;}
		}
		#endregion

		#region public virtual void CopyTo(IndicFuncParam ifp)
		public void CopyTo(IndicFuncParam ifp){
			ifp.Value = this.Value;
			ifp.SetOptimizerValue(this.GetOptimizerValue());
		}
		#endregion

		#region public override string ToString() 
		public override string ToString() {
			return Convert.ToString(this.Value);
		}
		#endregion

		#region public void Refresh()
		public void Refresh(){
			if (this.ParamValue_Changed != null)
				this.ParamValue_Changed(this, new EventArgs());
		}
		#endregion

		/// <summary>
		/// ������������ ������� �� ���� �����, 
		/// ������ ��� ������ � ������
		/// ������ ��� ������ � tooltip
		/// </summary>
		public abstract string[] GetEditorString();

		public abstract PropGridValue GetPropGridValue(IndicatorInsertBoxType iiboxType);

		/// <summary>
		/// ���������� ������ �� ���� ��������:
		/// 1 - ������, ��� ������������
		/// 2 - �������� ��� ������ �� ������ ����������, ��� ����
		/// </summary>
		internal abstract string GetOptimizerString(int timeFrame, string prefix, TradeVariables tvars);
		internal abstract object GetOptimizerValue();
		internal abstract void SetOptimizerValue(object values);

	}

	#region internal class CompVar
	internal class CompVar{
		private string _varname;
		private string[] _vars;

		public CompVar(string varname, string[] vars){
			this._varname = varname;
			this._vars = vars;
		}

		public string VarName{
			get{return this._varname;}
		}

		public string[] Vars{
			get{return this._vars;}
		}
	}
	#endregion
}
