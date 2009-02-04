using System;
using System.Drawing;
using Gordago.Analysis;

namespace Gordago.Analysis.Toolbox {

	#region public class iPeriodOfTime: Indicator
	public class iPeriodOfTime: Indicator{
		public override void Initialize() {
			this.Copyright = "Copyright © 2006, Gordago Software Ltd.";
			this.Link = "http://www.gordago.com/";
			this.GroupName = "";
			this.Name = "PeriodOfTime";
			this.ShortName = "PeriodOfTime";
			this.SetImage("i.gif");
			this.IsSeparateWindow = false;

			RegFunction("PeriodOfTime");
			this.SetCustomDrawingIndicator(typeof(iTimeCustomDrawing));
		}
	}
	#endregion

	[Function("PeriodOfTime")]
	public class PeriodOfTime: Function{
		protected override void Initialize() {
			RegParameter(new ParameterInteger("Hour", new string[]{"Hour", "Час"}, 0, 0, 23, 1));
			RegParameter(new ParameterInteger("Minute", new string[]{"Minute", "Минута"}, 0, 0, 59, 1));
			RegParameter(new ParameterInteger("TPeriod", new string[]{"Period (minute)", "Интервал (минута)"}, 1, 1, 1440, 1));
			RegParameter(Function.CreateDefParam_ApplyTo("Close"));
			ParameterVector pvtime = new ParameterVector("vectorTime", "Time");
			pvtime.Visible = false;
			RegParameter(pvtime);
		}

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			int hour = (int)parameters[0];
			int minute = (int)parameters[1];
			int tperiod = (int)parameters[2];

			IVector vector = (IVector)parameters[3];
			BarsVector bars = (BarsVector)parameters[4];

			if (result == null)
				result = new Vector();

			if (result.Count == vector.Count)
				result.RemoveLastValue();

			for (int i=result.Count;i<vector.Count;i++){
				DateTime dtm = bars.Bars[i].Time;
        long ldtmb = dtm.Ticks;

				long ldtmfrom = new DateTime(dtm.Year, dtm.Month, dtm.Day, hour, minute, 0, 0).Ticks;
				long ldtmto = ldtmfrom + tperiod * 60 * 10000000L;
				if ((dtm.Ticks > ldtmfrom && dtm.Ticks < ldtmto) || (ldtmfrom >= ldtmb && ldtmfrom <= dtm.Ticks))
					result.Add(vector[i]);
				else
					result.Add(float.NaN);
			}

			return result;
		}
	}

	#region public class iDayOfWeek: Indicator
	public class iDayOfWeek: Indicator{
		public override void Initialize() {
			this.Copyright = "Copyright © 2006, Gordago Software Ltd.";
			this.Link = "http://www.gordago.com/";
			this.GroupName = "";
			this.Name = "DayOfWeek";
			this.ShortName = "DayOfWeek";
			this.SetImage("i.gif");
			this.IsSeparateWindow = false;

			RegFunction("DayOfWeek");
			this.SetCustomDrawingIndicator(typeof(iTimeCustomDrawing));
		}
	}
	#endregion

	#region public class DayOfWeek: Function
	[Function("DayOfWeek")]
	public class __DayOfWeek: Function{
		protected override void Initialize() {
			RegParameter(new ParameterEnum("Week", new string[]{"Day of Week", "День недели"}, 0, new string[]{"Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"}));
			RegParameter(Function.CreateDefParam_ApplyTo("Close"));

			ParameterVector pvtime = new ParameterVector("vectorTime", "Time");
			pvtime.Visible = false;
			RegParameter(pvtime);
		}

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			int numWeek = (int)parameters[0];
			IVector vector = (IVector)parameters[1];
			BarsVector bars = (BarsVector)parameters[2];

			if (result == null)
				result = new Vector();

			if (result.Count == vector.Count)
				result.RemoveLastValue();

			for (int i=result.Count;i<vector.Count;i++){
				DateTime dtm = bars.Bars[i].Time;
				if ((int)dtm.DayOfWeek == numWeek)
					result.Add(vector[i]);
				else
					result.Add(float.NaN);
			}
			
			return result;
		}
	}
	#endregion

	#region public class iValueOfTime: Indicator
	public class iValueOfTime: Indicator{
		public override void Initialize() {
			this.Copyright = "Copyright © 2006, Gordago Software Ltd.";
			this.Link = "http://www.gordago.com/";
			this.GroupName = "";
			this.Name = "ValueOfTime";
			this.ShortName = "ValueOfTime";
			this.SetImage("i.gif");
			this.IsSeparateWindow = false;

			RegFunction("ValueOfTime").SetDrawPen(new Pen(Color.DarkOrange,2));
		}
	}
	#endregion

	#region public class ValueOfTime: Function
	[Function("ValueOfTime")]
	public class ValueOfTime: Function{
		private Function _funcPOfTime;
		protected override void Initialize() {
			_funcPOfTime = this.GetFunction("PeriodOfTime");
			this.RegParameter(_funcPOfTime);
		}

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			int hour = (int)parameters[0];
			int minute = (int)parameters[1];
			int tperiod = (int)parameters[2];

			IVector vector = (IVector)parameters[3];
			BarsVector bars = (BarsVector)parameters[4];

			if (result == null)
				result = new Vector();

			IVector poftres = analyzer.Compute(_funcPOfTime, hour, minute, tperiod, vector, bars);

			if (result.Count == poftres.Count)
				result.RemoveLastValue();

			for (int i=result.Count;i<poftres.Count;i++){
				float val = float.NaN;
				if (i>0){
					float valprev = poftres[i-1];
					float valcur = poftres[i];
					if (!float.IsNaN(valcur) && float.IsNaN(valprev)){
						val = valcur;
					} else {
						val = result[i-1];
					}
				}
				result.Add(val);
			}
			return result;
		}
	}
	#endregion
}