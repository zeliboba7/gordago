/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Drawing;

namespace Gordago.Analysis {
	public abstract class Parameter {
		private bool _visible = true;
		private object _value;
		private string _name;
		private string[] _captions;
		private Function _function;

		internal Parameter(string name, object value): this(name, name, value){}

		internal Parameter(string name, string caption, object value): this(name, new string[]{caption}, value){}

		internal Parameter(string name, string[] captions, object value){
			_name = name;
			this._captions = captions;
			this._value = value;
		}

		public abstract Parameter Clone();
		
		protected void CopyTo(Parameter param){
			param.Visible = this.Visible;
		}

		#region public bool Visible
		public bool Visible{
			get{return this._visible;}
			set{this._visible = value;}
		}
		#endregion

		#region public object Value
		public object Value{
			get{return this._value;}
			set{this._value  = value;}
		}
		#endregion

		#region public string Name
		public string Name{
			get{return this._name;}
		}
		#endregion

		#region public string[] Captions
		public string[] Captions{
			get{return this._captions;}
		}
		#endregion

		#region public Function Parent
		public Function Parent{
			get{return this._function;}
		}
		#endregion

		#region internal void SetFunction(Function function)
		internal void SetFunction(Function function){
			this._function = function;
		}
		#endregion

    #region public virtual string ToSaveString()
    public virtual string ToSaveString() {
      return this.Value.ToString();
    }
    #endregion
  }

	#region public class ParameterInteger : Parameter
	public class ParameterInteger : Parameter{
		private int _min = -2147483648 , _max = 2147483647, _step = 1;

		public ParameterInteger(string name, int value): base(name, value){
		}

		public ParameterInteger(string name, int value, int min, int max): base(name, value){
			this._min = min;
			this._max = max;
		}

		public ParameterInteger(string name, int value, int min, int max, int step): this(name, value, min, max){
			this._step = step;
		}

		public ParameterInteger(string name, string caption, int value): base(name, caption, value){
		}

		public ParameterInteger(string name, string caption, int value, int min, int max): base(name, caption, value){
			this._min = min;
			this._max = max;
		}

		public ParameterInteger(string name, string caption, int value, int min, int max, int step): this(name, caption, value, min, max){
			this._step = step;
		}

		public ParameterInteger(string name, string[] captions, int value): base(name, captions, value){
		}

		public ParameterInteger(string name, string[] captions, int value, int min, int max): base(name, captions, value){
			this._min = min;
			this._max = max;
		}

		public ParameterInteger(string name, string[] captions, int value, int min, int max, int step): this(name, captions, value, min, max){
			this._step = step;
		}

		#region public new int Value
		public new int Value{
			get{return (int)base.Value;}
			set{base.Value = value;}
		}
		#endregion
		
		#region public int Minimum
		public int Minimum{
			get{return this._min;}
			set{this._min = value;}
		}
		#endregion

		#region public int Maximum
		public int Maximum{
			get{return this._max;}
			set{this._max = value;}
		}
		#endregion

		#region public int Step
		public int Step{
			get{return this._step;}
			set{this._step = value;}
		}
		#endregion

		#region public override Parameter Clone() 
		public override Parameter Clone() {
			ParameterInteger prm = new ParameterInteger(this.Name, (string[])this.Captions.Clone(), this.Value, this.Minimum, this.Maximum);
			this.CopyTo(prm);
			return prm;
		}
		#endregion

    #region public override string ToString()
    public override string ToString() {
      return this.Value.ToString();
    }
    #endregion
  }
	#endregion

	#region public class ParameterFloat: Parameter
	public class ParameterFloat: Parameter{
		private float _min = -10000000, _max = 100000000, _step = 0.01F;
		private int _point = 2;

		public ParameterFloat(string name, float value):base(name, name, value){
		}

		public ParameterFloat(string name, string caption, float value):base(name, caption, value){
		}
		public ParameterFloat(string name, string[] captions, float value):base(name, captions, value){
		}

		public ParameterFloat(string name, string[] captions, float value, float min, float max):base(name, captions, value){
			this._min = min;
			this._max = max;
		}

		public ParameterFloat(string name, string[] captions, float value, float min, float max, float step):this(name, captions, value, min, max){
			this._step = step;
		}

		public ParameterFloat(string name, string[] captions, float value, float min, float max, float step, int point):this(name, captions, value, min, max, step){
			this._point = point;
		}

		#region public new float Value
		public new float Value{
			get{return (float)base.Value;}
			set{base.Value = value;}
		}
		#endregion

		#region public float Minimum
		public float Minimum{
			get{return this._min;}
			set{this._min = value;}
		}
		#endregion

		#region public float Maximum
		public float Maximum{
			get{return this._max;}
			set{this._max = value;}
		}
		#endregion

		#region public float Step
		public float Step{
			get{return this._step;}
			set{this._step = value;}
		}
		#endregion

		#region public int Point
		public int Point{
			get{return this._point;}
			set{this._point = Math.Max(value,0);}
		}
		#endregion

    #region public override Parameter Clone()
    public override Parameter Clone() {
			ParameterFloat prm = new ParameterFloat(this.Name, (string[])this.Captions.Clone(), this.Value, this.Minimum, this.Maximum, this.Step, this.Point);
			this.CopyTo(prm);
			return prm;
    }
    #endregion

    public override string ToString() {
      return this.Value.ToString();
    }
  }
	#endregion

	#region public class ParameterEnum: Parameter 
	public class ParameterEnum: Parameter {
		private string[] _enumvalues;

		public ParameterEnum(string name, int value, string[] enumValues): base(name, value){
			this._enumvalues = enumValues;
		}


		public ParameterEnum(string name, string caption, int value, string[] enumValues): base (name, caption, value){
			this._enumvalues = enumValues;
		}

		public ParameterEnum(string name, string[] captions, int value, string[] enumValues): base(name, captions, value){
			this._enumvalues = enumValues;
		}

		#region public string[] EnumValues
		public string[] EnumValues{
			get{return this._enumvalues;}
		}
		#endregion

		#region public new int Value
		public new int Value{
			get{return (int)base.Value;}
			set{
				value = Math.Max(value, 0);
				value = Math.Min(this._enumvalues.Length-1, value);
				base.Value = value;
			}
		}
		#endregion

    #region public override Parameter Clone()
    public override Parameter Clone() {
			ParameterEnum prm = new ParameterEnum(this.Name, this.Captions, this.Value, this.EnumValues);
			this.CopyTo(prm);
			return prm;
    }
    #endregion

    #region public override string ToString()
    public override string ToString() {
      return this.EnumValues[this.Value];
    }
    #endregion
  }
	#endregion

	#region public class ParameterVector: Parameter
	public class ParameterVector: Parameter{

		private Parameter[] _parameters;

		public ParameterVector(string name, string functionName): base(name, functionName){}

		public ParameterVector(string name, string caption, string functionName): base(name, caption, functionName){}
		
		public ParameterVector(string name, string[] captions, string functionName): base(name, captions, functionName){ }

    #region public new string Value
    public new string Value{
			get{return (string)base.Value;}
			set{
				base.Value = value;
      }
    }
    #endregion

    #region public Parameter[] Parameters
    public Parameter[] Parameters{
			get{return this._parameters;}
			set{this._parameters = value;}
    }
    #endregion

    #region public override Parameter Clone()
    public override Parameter Clone() {
			ParameterVector prm = new ParameterVector(this.Name, this.Captions, this.Value);
			prm.Parameters = this.Parameters;
			this.CopyTo(prm);
			return prm;
		}
		#endregion

    #region public override string ToString()
    public override string ToString() {
      return this.Value;
    }
    #endregion
  }
	#endregion

  public class ParameterColor:Parameter {
    public ParameterColor(string name, Color color):base(name, color) {}
    public ParameterColor(string name, string caption, Color color) : base(name, caption, color) { }
    public ParameterColor(string name, string[] captions, Color color) : base(name, captions, color) { }

    #region public new Color Value
    public new Color Value {
      get { return (Color)base.Value; }
      set { base.Value = value; }
    }
    #endregion

    #region public override Parameter Clone()
    public override Parameter Clone() {
      ParameterColor prm = new ParameterColor(this.Name, this.Captions, this.Value);
      this.CopyTo(prm);
      return prm;
    }
    #endregion

    #region public override string ToSaveString()
    public override string ToSaveString() {
      return this.Value.ToArgb().ToString();
    }
    #endregion

    #region public override string ToString()
    public override string ToString() {
      return this.Value.ToString();
    }
    #endregion
  }
}
