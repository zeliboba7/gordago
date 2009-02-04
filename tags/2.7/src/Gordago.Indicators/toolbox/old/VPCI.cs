using System;
using System.Reflection;
using Gordago.Analysis;
using System.IO;

namespace Gordago.StockOptimizer2.Toolbox {

	/// <summary>
	/// VPCI
	/// Volume Price Confirmation Indicator
	/// Параметы:
	///		0	int		vpc_period	- Период VPC
	///		1	int		vpr_period	- Период VPR
	///		2	int		vm_period1	- Малый период VM
	///		3	int		vm_period2	- Большой период VM
	///		4	IVector	close		- цены закрытия
	///		5	IVector	volume		- обьём сделок за период
	/// </summary>
	[Function("VPCI",typeof(int),typeof(int),typeof(int),typeof(int),typeof(IVector),typeof(IVector))]
	public class VPCI: Function {

		public override IVector Compute(Analyzer	analyzer, object[]	parameters,IVector result) {

			int		vpc_period	= (int)parameters[0];
			int		vpr_period	= (int)parameters[1];
			int		vm_period1	= (int)parameters[2];
			int		vm_period2	= (int)parameters[3];

			IVector	close_vector	= (IVector)parameters[4];
			IVector	volume_vector	= (IVector)parameters[5];

			int size	= close_vector.Count;

			int period = Math.Max(Math.Max(Math.Max(vpc_period, vpr_period), vm_period1), vm_period2);

			if(size < period)
				return null;

			if(result == null){
				result	= new Vector();
				result.ArrayShift = period - 1;
			}

			int rsize = result.Count;
			
			for (int pos=rsize+period-1; pos<size; pos++){
				float VPCI = 0;
				float VPC  = 0;
				float VPR  = 0;
				float VM   = 0;
			
				float tmp  = 0;

				VPC = VWMA_Compute(pos,vpc_period,close_vector,volume_vector) - 
					SMA_Compute(pos,vpc_period,close_vector);
      
				tmp = SMA_Compute(pos,vpr_period,close_vector);
				if(tmp == 0) {
					result.Add(0);
					return result;
				}
				VPR = VWMA_Compute(pos,vpr_period,close_vector,volume_vector)/tmp;
      
				tmp = MA_Compute(pos,vm_period2,volume_vector);
				if(tmp == 0) {
					result.Add(0);
					return result;
				}
				VM = MA_Compute(pos,vm_period1,volume_vector)/tmp;
      
				VPCI = VPC*VPR*VM;

				result.Add(VPCI);
			}
			return result;
		}

		float SMA_Compute(int bar_num,int period,IVector Close) {

			float sma   = 0;
			for(int i = 0; i < period; i++)
				sma   += Close[bar_num-i];  

			return(sma/period);
		}

		float MA_Compute(int bar_num,int period,IVector Volume) {

			float ma   = 0;
			for(int i = 0; i < period; i++)
				ma += Volume[bar_num-i]; 

			return(ma/period);

		}

		float VWMA_Compute(int bar_num, int period, IVector Close, IVector Volume) {

			float close_volume_sum  = 0;
			float volume_sum        = 0;

			for(int i = 0; i < period; i++) {
				close_volume_sum  += Close[bar_num-i]*Volume[bar_num-i];
				volume_sum  += Volume[bar_num-i];  
			}

			return(close_volume_sum/volume_sum);
		}
	}

	/// <summary>
	/// SMMAVPCI
	///	Smoothed Moving Average from the Volume Price Confirmation Indicator
	///
	///	Параметы:
	///		0	int		vpc_period	- Период VPC
	///		1	int		vpr_period	- Период VPR
	///		2	int		vm_period1	- Малый период VM
	///		3	int		vm_period2	- Большой период VM
	///		4	IVector	close		- цены закрытия
	///		5	IVector	volume		- обьём сделок за период
	///		6	int		smma_period	- период вычисления сглаженного скользящего среднего
	/// </summary>
	[Function("SMMAVPCI",typeof(int),typeof(int),typeof(int),typeof(int),typeof(IVector),typeof(IVector),typeof(int))]
	public class SMMAVPCI: Function {

		private Function _vpci_func, _ma_func;

		public override IVector Compute(Analyzer	analyzer, object[] parameters, IVector	result ) {

			int		vpc_period	= (int)parameters[0];
			int		vpr_period	= (int)parameters[1];
			int		vm_period1	= (int)parameters[2];
			int		vm_period2	= (int)parameters[3];

			IVector	close_vector	= (IVector)parameters[4];
			IVector	volume_vector	= (IVector)parameters[5];

			int		smma_period	= (int)parameters[6];

			if(_ma_func == null || _vpci_func == null){
				_vpci_func	= analyzer["VPCI"];
				_ma_func	= analyzer["ma"];
			}

			IVector vpci_vector = analyzer.Compute(_vpci_func, vpc_period, vpr_period, vm_period1, vm_period2, close_vector, volume_vector);

			if(vpci_vector == null) return null;

			int sh1 = vpci_vector.ArrayShift;
			
			result = analyzer.Compute(_ma_func, smma_period, 3, vpci_vector);
			
			if (result == null) return null;

			int sh2 = result.ArrayShift;
			result.ArrayShift = sh1 + sh2;

			return result;
		}
	}
}

