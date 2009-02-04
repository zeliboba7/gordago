using System;
using System.Drawing;
using Gordago.Analysis;

namespace Gordago.StockOptimizer2.Toolbox {
	public class iIchimoku: Indicator {
		public override void Initialize() {
			this.Copyright = DefaultDescription.Copyright;
			this.Link = DefaultDescription.Link;
			this.GroupName = GroupNames.Absolute;
			this.Name = "Ichimoku";
			this.ShortName = "Ichimoku";
			this.SetImage("i.gif");
			this.IsSeparateWindow = false;

			RegFunction("IchimokuTenkan").SetDrawPen(Color.Red);
			RegFunction("IchimokuKijun").SetDrawPen(Color.Blue);
			RegFunction("IchimokuSpanA").SetDrawPen(Color.Green);
			RegFunction("IchimokuSpanB").SetDrawPen(Color.Brown);
      RegFunction("IchimokuChinkou").SetDrawPen(Color.Thistle);
    }
  }

  #region public class IchimokuTenkan: Function
  [Function("IchimokuTenkan")]
	public class IchimokuTenkan: Function {

		protected override void Initialize() {
			RegParameter(new ParameterInteger("Tenkan", 9, 1, 10000));
			RegParameter(new ParameterInteger("Kijun", 26, 1, 10000));
			RegParameter(new ParameterInteger("Senkou", 52, 1, 10000));
			ParameterVector pvlow = new ParameterVector("VectorLow", "Low");
			pvlow.Visible = false;
			RegParameter(pvlow);
			ParameterVector pvhigh = new ParameterVector("VectorHigh", "High");
			pvhigh.Visible = false;
			RegParameter(pvhigh);
			ParameterVector pvclose = new ParameterVector("VectorClose", "Close");
			pvclose.Visible = false;
			RegParameter(pvclose);
      RegParameter(new ParameterColor("ColorIchimokuTenkan", new string[] { "Color Tenkan", "Цвет Tenkan" }, Color.Red));
		}

		
		public override IVector Compute(Analyzer	analyzer, object[]	parameters, IVector result) {

			int period = (int)parameters[0];
			IVector	Low		= (IVector)parameters[3];
			IVector	High	= (IVector)parameters[4];
			IVector	close	= (IVector)parameters[5];

			if(result == null)
				result	= new Vector();

			if (result.Count == close.Count)
				result.RemoveLastValue();

			for (int pos = result.Count; pos < close.Count; pos++){
				if (pos < period){
					result.Add(float.NaN);
				}else{
					float price= 0;
					float high	= High[pos-1];
					float low	= Low[pos-1];
					for(int i = pos - period+1; i < pos; i++) {
						price	= High[i];
					
						if(high < price) high=price;

						price	= Low[i];

						if(low>price) low=price;
					}
					result.Add((high+low)/2);
				}
			}
			return result;
		}
  }
  #endregion

  #region public class IchimokuKijun: Function
  [Function("IchimokuKijun")]
	public class IchimokuKijun: Function {

		protected override void Initialize() {
			RegParameter(GetFunction("IchimokuTenkan"));
      RegParameter(new ParameterColor("ColorIchimokuKijun", new string[] { "Color Kijun", "Цвет Kijun" }, Color.Blue));
		}

		public override IVector Compute(Analyzer	analyzer,object[]	parameters, IVector	result) {

			int	period = (int)parameters[1];

			IVector	Low		= (IVector)parameters[3];
			IVector	High	= (IVector)parameters[4];
			IVector	close	= (IVector)parameters[5];

			if(result == null)
				result	= new Vector();
			
			if (result.Count == close.Count)
				result.RemoveLastValue();

			for (int pos = result.Count; pos < close.Count; pos++){
				if (pos < period){
					result.Add(float.NaN);
				}else{
					float price= 0;
					float high	= High[pos-1];
					float low	= Low[pos-1];
					for(int i = pos - period+1; i < pos; i++) {
						price	= High[i];
						if(high < price) high=price;

						price	= Low[i];
						if(low>price)  low=price;
					}
					result.Add((high+low)/2);
				}
			}
			return result;
		}
  }
  #endregion

  #region public class IchimokuSpanA: Function
  [Function("IchimokuSpanA")]
	public class IchimokuSpanA: Function {

		private Function _ftenkan, _fkijun;

		protected override void Initialize() {
			_ftenkan = GetFunction("IchimokuTenkan");
			_fkijun = GetFunction("IchimokuKijun");

			RegParameter(_ftenkan);
      RegParameter(new ParameterColor("ColorIchimokuSpanA", new string[] { "Color SpanA", "Цвет SpanA" }, Color.Green));
		}

		public override IVector Compute(Analyzer	analyzer, object[]	parameters, IVector result) {
			int		tenkan	= (int)parameters[0];
			int		kijun	= (int)parameters[1];
			int		senkou	= (int)parameters[2];

			IVector	Low		= (IVector)parameters[3];
			IVector	High	= (IVector)parameters[4];
			IVector	Close	= (IVector)parameters[5];
			
			IVector _tenres = analyzer.Compute(_ftenkan, tenkan, kijun, senkou, Low, High, Close);
			IVector _kijres = analyzer.Compute(_fkijun, tenkan, kijun, senkou, Low, High, Close);

			if(result == null)
				result	= new Vector();

			if (result.Count == Close.Count)
				result.RemoveLastValue();

			for (int pos = result.Count; pos < _tenres.Count; pos++){
				float Tenkan_val = _tenres[pos];
				float Kijun_val = _kijres[pos];
				
				result.Add((Tenkan_val + Kijun_val)/2);
			}
			return result;
		}
  }
  #endregion

  #region public class IchimokuSpanB: Function
  [Function("IchimokuSpanB")]
	public class IchimokuSpanB: Function {

		protected override void Initialize() {
			RegParameter(GetFunction("IchimokuTenkan"));
      RegParameter(new ParameterColor("ColorIchimokuSpanB", new string[] { "Color SpanB", "Цвет SpanB" }, Color.Brown));
		}

		public override IVector Compute(Analyzer	analyzer, object[]	parameters, IVector result){
			int period	= (int)parameters[2];

			IVector	Low		= (IVector)parameters[3];
			IVector	High	= (IVector)parameters[4];
			IVector	Close	= (IVector)parameters[5];

			if(result == null)
				result = new Vector();

			if (result.Count == Close.Count)
				result.RemoveLastValue();

			for (int pos = result.Count; pos < Close.Count; pos++){
				if (pos < period){
					result.Add(float.NaN);
				}else{
					float price= 0;
					float high	= High[pos-1];
					float low	= Low[pos-1];
					for(int i = pos - period+1; i < pos; i++) {
						price	= High[i];
						if(high < price)
							high=price;

						price	= Low[i];
						if(low>price) 
							low=price;
					}
					result.Add((high+low)/2);
				}
			}
			return result;
		}
  }
  #endregion

  #region public class IchimokuChinkou: Function
  [Function("IchimokuChinkou")]
	public class IchimokuChinkou: Function {
		protected override void Initialize() {
      //ParameterVector pvclose = new ParameterVector("VectorClose", "Close");
      //pvclose.Visible = false;
      //RegParameter(pvclose);

      
      //RegParameter(GetFunction("IchimokuTenkan"));
      RegParameter(GetFunction("IchimokuTenkan"));

      RegParameter(new ParameterColor("ColorIchimokuChinkou", new string[] { "Color Chinkou", "Цвет Chinkou" }, Color.Brown));
		}

    public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
      IVector close = (IVector)parameters[5];
     
      if (result == null)
        result = new Vector();

      if (result.Count == close.Count)
        result.RemoveLastValue();

      for (int pos = result.Count; pos < close.Count; pos++) {
        result.Add(close[pos]);
      }
      return result;
    }
  }
  #endregion

}
