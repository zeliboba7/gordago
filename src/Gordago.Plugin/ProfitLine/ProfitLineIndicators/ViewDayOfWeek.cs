using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.Analysis {
  
  [Function("ViewDayOfWeek")]
  public class ViewDayOfWeek:Function {


    protected override void Initialize() {
      RegParameter(Function.CreateDefParam_ApplyTo("Close"));
      
      ParameterVector pvtime = new ParameterVector("vectorTime", "Time");
      pvtime.Visible = false;
      RegParameter(pvtime);
    }

    public override Vector Compute(Analyzer analyzer, object[] parameters, Vector result) {
      Vector vector = (Vector)parameters[0];
      BarsVector bars = parameters[1] as BarsVector;

      if(result == null)
        result = new Vector();

      if(result.Size == vector.Size)
        result.RemoveLastValue();

      for(int i = result.Size; i < vector.Size; i++) {
        DateTime time = new DateTime(bars.Bars[i].Time);

        float val = float.NaN;
        switch(time.DayOfWeek){
          case DayOfWeek.Sunday:
            val = 0;
            break;
          case DayOfWeek.Monday:
            val = 1;
            break;
          case DayOfWeek.Tuesday:
            val = 2;
            break;
          case DayOfWeek.Wednesday:
            val = 3;
            break;
          case DayOfWeek.Thursday:
            val = 4;
            break;
          case DayOfWeek.Friday:
            val = 5;
            break;
          case DayOfWeek.Saturday:
            val = 6;
            break;
        }
        result.Add(val);
      }
      return result;
    }
  }

  public class iViewDayOfWeek:Indicator {
    public override void Initialize() {
      this.Copyright = "Copyright © 2006, Gordago Software Ltd.";
      this.Link = "http://www.gordago.com/";
      this.GroupName = "ProfitLine";
      this.Name = "ViewDayOfWeek";
      this.ShortName = "ViewDayOfWeek";
      this.SetImage("i.gif");
      this.IsSeparateWindow = true;

      RegFunction("ViewDayOfWeek");
      //this.SetCustomDrawingIndicator(typeof(iViewDayOfWeekDrawing));
    }
  }
}
