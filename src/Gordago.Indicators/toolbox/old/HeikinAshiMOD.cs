using System;
using System.Reflection;
using System.Collections;
using Gordago.Analysis;

namespace Gordago.StockOptimizer2.Toolbox {

	public class Buffer{
		private float[] _buffer;
		public Buffer(){
			_buffer = new float[]{};
		}

		public void AddValue(float val){
			ArrayList al = new ArrayList(this._buffer);
			al.Add(val);
			this._buffer = (float[])al.ToArray(typeof(float));
		}

		public float this[int index]{
			get{return this._buffer[index];}
			set{
				if (index >= this._buffer.Length){
					ArrayList al = new ArrayList(this._buffer);
					al.AddRange(new float[index - this._buffer.Length+1]);
					this._buffer = (float[])al.ToArray(typeof(float));
				}
				this._buffer[index] = value;
			}
		}

		public int Lenght{
			get{return this._buffer.Length;}
		}
	}

	#region public class ArVector: Vector
	public class ArVector: Vector{

		private Buffer _bfopen;
		private Buffer _bfclose;

		public ArVector() {
			_bfopen = new Buffer();
			_bfclose = new Buffer();
		}

		public Buffer BfOpen{
			get{return this._bfopen;}
		}
		public Buffer BfClose{
			get{return this._bfclose;}
		}
	}
	#endregion

	#region public class HeikinAshiMODF1: Function
	[Function("heikinashimodf1",typeof(IVector),typeof(IVector),typeof(IVector),typeof(IVector),typeof(int))]
	public class HeikinAshiMODF1: Function{

		private Function _heikin_ashi_mod;
		
		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			IVector high = (IVector)parameters[0];
			IVector open = (IVector)parameters[1];
			IVector low = (IVector)parameters[2];
			IVector close = (IVector)parameters[3];

			int compBars = (int)parameters[4];

			if (this._heikin_ashi_mod == null){
				this._heikin_ashi_mod = analyzer["heikinashimod"];
			}
			return analyzer.Compute(_heikin_ashi_mod, high, open, low, close, compBars, 0);
		}
	}
	#endregion

	#region public class HeikinAshiMODF2: Function
	[Function("heikinashimodf2",typeof(IVector),typeof(IVector),typeof(IVector),typeof(IVector),typeof(int))]
	public class HeikinAshiMODF2: Function{

		private Function _heikin_ashi_mod;

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			IVector high = (IVector)parameters[0];
			IVector open = (IVector)parameters[1];
			IVector low = (IVector)parameters[2];
			IVector close = (IVector)parameters[3];

			int compBars = (int)parameters[4];

			if (this._heikin_ashi_mod == null){
				this._heikin_ashi_mod = analyzer["heikinashimod"];
			}

			return analyzer.Compute(_heikin_ashi_mod, high, open, low, close, compBars, 1);
		}
	}
	#endregion

	[Function("heikinashimod",typeof(IVector),typeof(IVector),typeof(IVector),typeof(IVector),typeof(int),typeof(int))]
	public class HeikinAshiMOD: Function{

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			IVector high = (IVector)parameters[0];
			IVector open = (IVector)parameters[1];
			IVector low = (IVector)parameters[2];
			IVector close = (IVector)parameters[3];

			int compBars = (int)parameters[4];
			int numline = (int)parameters[5];

			int size = high.Count;
			if (size < 2)
				return null;

			if (result == null){
				result = new ArVector();
			}

			ArVector arres = result as ArVector;
			arres.BfOpen[size-1] = 0;
			arres.BfClose[size-1] = 0;

			int rsize = arres.Count;
			
			arres.BfOpen[0] = open[0];
			arres.BfOpen[1] = open[1];

			for(int index = rsize+2; index < size; index++){

				arres.BfClose[index] = (high[index]+open[index]+low[index]+close[index])/4;
				arres.BfOpen[index] = (arres.BfOpen[index-1] + arres.BfClose[index-1])/2;

				float val1 = float.NaN, val2 = float.NaN;

				if (arres.BfClose[index]<arres.BfOpen[index]){
					val1 = low[index];
					val2 = high[index];
				}

				if (arres.BfClose[index]<arres.BfOpen[index]){
					val2 = low[index];
					val1 = high[index];
				}

				if (index >= compBars){
					float max, min;
					for (int i=index; i >= index - compBars; i--){

						max = Math.Max(arres.BfOpen[i], arres.BfClose[i]);
						min = Math.Min(arres.BfOpen[i], arres.BfClose[i]);
					
						if (arres.BfOpen[index] >= min && arres.BfOpen [index] <= max &&
							arres.BfClose [index] >= min && arres.BfClose[index] <= max){
							if (arres.BfClose[i] < arres.BfOpen[i]){
								val1 = low[index];
								val2 = high[index];
								break;
							}
							if (arres.BfClose[i] > arres.BfOpen[i]){
								val1 = high[index];
								val2 = low[index];
								break;
							}
						}
						int d = 0;
						d++;
					}
				}
				if (numline == 0)
					arres.Add(val1);
				else
					arres.Add(val2);
			}
			// arres.ArrayShift = 2;
			return arres;
		}
	}
}
