using System;
using System.Reflection;

[assembly: AssemblyTitle("Stock Toolbox")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("Build")]
[assembly: AssemblyCompany("Gordago Software Ltd.")]
[assembly: AssemblyProduct("Gordago Forex Optimizer TT")]
[assembly: AssemblyCopyright("Copyright © 2004-2006 Gordago Software Ltd.")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: AssemblyVersion("1.16.*")]
[assembly: CLSCompliant(false)]

namespace Gordago {
	internal class GroupNames{
		public const string TopPrice = "Price";
		public const string TopNumber = "Number";
		public const string Absolute = "Absolute scaled";
		public const string Relative = "Relative scaled";
	}
	internal class DefaultDescription{
		public const string Copyright = "Copyright © 2006, Gordago Software Ltd.";
		public const string Link = "http://www.gordago.com/"; 
		
		private new bool Equals(object obj) {
			return base.Equals (obj);
		}
		private new int GetHashCode() {
			return base.GetHashCode ();
		}

		public override string ToString() {
			return base.ToString ();
		}
	}
}