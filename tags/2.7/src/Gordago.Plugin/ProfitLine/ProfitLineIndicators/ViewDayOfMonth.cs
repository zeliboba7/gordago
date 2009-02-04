using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.Analysis {

  [Function("ViewDayOfMonth")]
  public class ViewDayOfMonth:Function {


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
        result.Add(time.Day);
      }
      return result;
    }
  }

  public class iViewDayOfMonth:Indicator {
    public override void Initialize() {
      this.Copyright = "Copyright © 2006, Gordago Software Ltd.";
      this.Link = "http://www.gordago.com/";
      this.GroupName = "ProfitLine";
      this.Name = "ViewDayOfMonth";
      this.ShortName = "ViewDayOfMonth";
      this.SetImage("i.gif");
      this.IsSeparateWindow = true;

      RegFunction("ViewDayOfMonth");
    }
  }
}
