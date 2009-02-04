/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.ComponentModel.Design;
using System.Drawing;
using System.ComponentModel;
using Gordago.Analysis.Chart;

namespace Gordago.Design {

  public class ArrayTypeConverter:TypeConverter {

    #region public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
      if(sourceType == typeof(string)) {
        return true;
      }
      return base.CanConvertFrom(context, sourceType);
    }
    #endregion

    #region public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
    public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value) {
      try {
        string sval = value as string;
        if(context.PropertyDescriptor.PropertyType == typeof(float[])) {
          string[] svals = sval.Split(new char[]{';'});
          float[] fvals = new float[svals.Length];
          for(int i = 0; i < fvals.Length; i++) {
            fvals[i] = Convert.ToSingle(svals[i]);
          }
          return fvals;
        } else if (context.PropertyDescriptor.PropertyType == typeof(int[])) {
          string[] svals = sval.Split(new char[] { ';' });
          int[] ivals = new int[svals.Length];
          for (int i = 0; i < ivals.Length; i++) {
            ivals[i] = Convert.ToInt32(svals[i]);
          }
          return ivals;
        } else if (context.PropertyDescriptor.PropertyType == typeof(FibonacciLevel[])) {
          string[] svals = sval.Split(new char[] { ';' });
          FibonacciLevel[] fvals = new FibonacciLevel[svals.Length];
          for (int i = 0; i < fvals.Length; i++) {
            fvals[i] = new FibonacciLevel();
            fvals[i].Value =  Convert.ToSingle(svals[i]);
          }
          return fvals;
        }
        return base.ConvertFrom(context, culture, value);
      } catch {
        if(context.PropertyDescriptor.PropertyType == typeof(float[])) {
          return new float[0];
        }
        return null;
      }
    }
    #endregion

    #region public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
    public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType) {
      if(value is float[]) {
        float[] values = value as float[];
        string[] svalues = new string[values.Length];
        for(int i = 0; i < values.Length; i++) {
          svalues[i] = values[i].ToString();
        }
        return string.Join("; ", svalues);
      }else if (value is FibonacciLevel[]){
        FibonacciLevel[] values = value as FibonacciLevel[];
        string[] svalues = new string[values.Length];
        for (int i = 0; i < values.Length; i++) {
          svalues[i] = values[i].Value.ToString();
        }
        return string.Join("; ", svalues);
      } else if (value is int[]){
        int[] values = value as int[];
        string[] svalues = new string[values.Length];
        for (int i = 0; i < values.Length; i++) {
          svalues[i] = values[i].ToString();
        }
        return string.Join("; ", svalues);
      }else{
        return base.ConvertTo(context, culture, value, destinationType);
      }
    }
    #endregion

    #region public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
      if (destinationType == typeof(float[]))
        return true;
      else if (destinationType == typeof(int[]))
        return true;
      else if (destinationType == typeof(FibonacciLevel))
        return true;
      return base.CanConvertTo(context, destinationType);
    }
    #endregion
  }
}
