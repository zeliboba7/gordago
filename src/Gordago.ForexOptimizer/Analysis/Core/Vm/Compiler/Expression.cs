/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections;

namespace Gordago.Analysis.Vm.Compiler {
  public abstract class Expression {

    public abstract VmCommand[] Compile();

    public abstract Type Type { get; }

    private static Expression Parse1(string expression, ref int c) {

      if (expression[c] == '(') {
        c++;
        Expression formula = Parse5(expression, ref c);
        if (expression[c++] != ')')
          throw new CompilerException("синтаксическая ошибка: ожидается )", expression, c);
        return formula;
      } else {
        if (char.IsDigit(expression[c])) {
          string @value = string.Empty;
          while (char.IsDigit(expression[c]))
            @value += expression[c++];

          if (expression[c] == '.') {
            @value += ".";
            c++;
            while (char.IsDigit(expression[c]))
              @value += expression[c++];
          }
          if (@value.IndexOf('.') == -1)
            return new ValueExpression(int.Parse(@value));
          else
            return new ValueExpression(float.Parse(@value, System.Globalization.CultureInfo.InvariantCulture.NumberFormat));
        } else if (char.IsLetter(expression[c])) {
          string identifier = string.Empty;
          while (char.IsLetterOrDigit(expression[c]))
            identifier += expression[c++];
          switch (identifier) {
            case "true":
              return new ValueExpression(true);
            case "false":
              return new ValueExpression(false);
            default:
              Function function = GordagoMain.IndicatorManager.GetFunction(identifier);
              if (function == null)
                throw new CompilerException(string.Format("функция {0} не зарегистрирована", identifier),
                  expression, c);

              ArrayList parameters = new ArrayList();
              if (expression[c] == '(') {
                c++;
                while (expression[c] != ')') {
                  parameters.Add(Parse5(expression, ref c));
                  if (expression[c] == ';')
                    c++;
                }
                if (expression[c++] != ')')
                  throw new CompilerException("синтаксическая ошибка: ожидается )", expression, c);
              }
              Expression element = null;
              if (expression[c] == '[') {
                c++;
                element = Parse2(expression, ref c);
                if (expression[c++] != ']')
                  throw new CompilerException("синтаксическая ошибка: ожидается ]", expression, c);
              }
              try {
                return new InvokeExpression(function, (Expression[])parameters.ToArray(typeof(Expression)), element);
              } catch (Exception ex) {
                throw new CompilerException(ex.Message, expression, c);
              }
          }
        }
      }
      throw new CompilerException("синтаксическая ошибка", expression, c);
    }

    #region private static Expression Parse2(string expression, ref int c)
    private static Expression Parse2(string expression, ref int c) {
      if (expression[c] == '+') {
        c++;
        return Parse1(expression, ref c);
      } else if (expression[c] == '-') {
        c++;
        return new ArithmeticExpression(new ValueExpression(0),
          Parse1(expression, ref c), "-");
      } else if (expression[c] == '!') {
        c++;
        try {
          return new NotExpression(Parse1(expression, ref c));
        } catch (Exception ex) {
          throw new CompilerException(ex.Message, expression, c);
        }
      } else
        return Parse1(expression, ref c);
    }
    #endregion

    #region private static Expression Parse3(string expression, ref int c)
    private static Expression Parse3(string expression, ref int c) {
      Expression term = Parse2(expression, ref c);
      while (expression[c] == '*' || expression[c] == '/' || expression[c] == '&') {
        string @operator = expression[c++].ToString();
        try {
          if (@operator != "&")
            term = new ArithmeticExpression(term,
              Parse2(expression, ref c), @operator);
          else
            term = new LogicalExpression(term,
              Parse2(expression, ref c), @operator);
        } catch (Exception ex) {
          throw new CompilerException(ex.Message, expression, c);
        }
      }
      return term;
    }
    #endregion

    #region private static Expression Parse4(string expression, ref int c)
    private static Expression Parse4(string expression, ref int c) {
      Expression formula = Parse3(expression, ref c);
      while (expression[c] == '+' || expression[c] == '-' || expression[c] == '|') {
        string @operator = expression[c++].ToString();
        try {
          if (@operator != "|")
            formula = new ArithmeticExpression(formula,
              Parse3(expression, ref c), @operator);
          else
            formula = new LogicalExpression(formula,
              Parse3(expression, ref c), @operator);
        } catch (Exception ex) {
          throw new CompilerException(ex.Message, expression, c);
        }
      }
      return formula;
    }
    #endregion

    #region private static Expression Parse5(string expression,ref int c)
    private static Expression Parse5(string expression, ref int c) {
      Expression relation = Parse4(expression, ref c);
      while (expression[c] == '=' || expression[c] == '>' || expression[c] == '<') {
        string @operator = expression[c++].ToString();
        if (expression[c] == '=' || expression[c] == '>' || expression[c] == '<')
          @operator += expression[c++].ToString();
        try {
          relation = new RelationExpression(relation,
            Parse4(expression, ref c), @operator);
        } catch (Exception ex) {
          throw new CompilerException(ex.Message, expression, c);
        }
      }
      return relation;
    }
    #endregion

    #region public static VmCommand[] Compile(string expression)
    public static VmCommand[] Compile(string expression) {
      int c = 0;
      expression = expression.Replace(" ", string.Empty) + " ";
      Expression formula = Parse5(expression, ref c);
      if (expression[c] != ' ')
        throw new CompilerException("синтаксическая ошибка", expression, c);
      return formula.Compile();
    }
    #endregion

    #region private static Type Check1(string expression,ref int c)
    private static Type Check1(string expression, ref int c) {
      if (expression[c] == '(') {
        c++;
        Type type = Check5(expression, ref c);
        if (expression[c++] != ')')
          throw new CompilerException("синтаксическая ошибка: ожидается )", expression, c);
        return type;
      } else if (expression[c] == '$') {
        c++;
        while (char.IsLetterOrDigit(expression[c]))
          c++;
        return typeof(int);
      } else {
        if (char.IsDigit(expression[c])) {
          string @value = string.Empty;
          while (char.IsDigit(expression[c]))
            @value += expression[c++];
          if (expression[c] == '.') {
            @value += ".";
            c++;
            while (char.IsDigit(expression[c]))
              @value += expression[c++];
          }
          //					Console.WriteLine("LoadValue {0}",@value);
          if (@value.IndexOf('.') == -1)
            return typeof(int);
          else
            return typeof(float);
        } else if (char.IsLetter(expression[c])) {
          string identifier = string.Empty;
          while (char.IsLetterOrDigit(expression[c]))
            identifier += expression[c++];
          switch (identifier) {
            case "true":
            case "false":
              //							Console.WriteLine("LoadValue {0}",identifier);
              return typeof(bool);
            default:
              if (expression[c] == '(') {
                c++;
                while (expression[c] != ')') {
                  Check4(expression, ref c);
                  if (expression[c] == ';')
                    c++;
                }
                if (expression[c++] != ')')
                  throw new CompilerException("синтаксическая ошибка: ожидается )", expression, c);
              }
              if (expression[c] == '[') {
                c++;
                Check2(expression, ref c);
                if (expression[c++] != ']')
                  throw new CompilerException("синтаксическая ошибка: ожидается ]", expression, c);
              }
              return typeof(IVector);
          }
        }
      }
      throw new CompilerException("синтаксическая ошибка", expression, c);
    }
    #endregion

    #region private static Type Check2(string expression,ref int c)
    private static Type Check2(string expression, ref int c) {
      if (expression[c] == '+') {
        c++;
        Type type = Check1(expression, ref c);
        if (type == typeof(int) || type == typeof(float) || type == typeof(IVector)) {
          return type;
        } else
          throw new CompilerException("несоответсвие типов: требуется число", expression, c);
      } else if (expression[c] == '-') {
        c++;
        Type type = Check1(expression, ref c);
        if (type == typeof(int) || type == typeof(float) || type == typeof(IVector))
          return type;
        else
          throw new CompilerException("несоответсвие типов: требуется число", expression, c);
      } else if (expression[c] == '!') {
        c++;
        Type type = Check1(expression, ref c);
        if (type == typeof(bool))
          return type;
        else
          throw new CompilerException("несоответсвие типов: требуется булевское значение", expression, c);
      } else
        return Check1(expression, ref c);
    }
    #endregion

    #region private static Type Check3(string expression,ref int c)
    private static Type Check3(string expression, ref int c) {
      Type type = Check2(expression, ref c);
      while (expression[c] == '*' || expression[c] == '/' || expression[c] == '&') {
        string @operator = expression[c++].ToString();
        Type temp = Check2(expression, ref c);
        if (@operator == "*" || @operator == "/") {
          if ((type == typeof(int) || type == typeof(float) || type == typeof(IVector)) &&
            (temp == typeof(int) || temp == typeof(float) || temp == typeof(IVector))) {
            type = (type == typeof(IVector) || temp == typeof(IVector) || type == typeof(float) || temp == typeof(float) || @operator == "/") ? typeof(float) : typeof(int);
          } else
            throw new CompilerException("несоответсвие типов: требуется число", expression, c);
        } else {
          if (type != typeof(bool) || temp != typeof(bool))
            throw new CompilerException("несоответсвие типов: требуется булевское значение", expression, c);
        }
      }
      return type;
    }
    #endregion

    #region private static Type Check4(string expression,ref int c)
    private static Type Check4(string expression, ref int c) {
      Type type = Check3(expression, ref c);
      while (expression[c] == '+' || expression[c] == '-' || expression[c] == '|') {
        string @operator = expression[c++].ToString();
        Type temp = Check3(expression, ref c);
        if (@operator == "+" || @operator == "-") {
          if ((type == typeof(int) || type == typeof(float) || type == typeof(IVector)) &&
            (temp == typeof(int) || temp == typeof(float) || temp == typeof(IVector))) {
            type = (type == typeof(IVector) || temp == typeof(IVector) || type == typeof(float) || temp == typeof(float)) ? typeof(float) : typeof(int);
          } else
            throw new CompilerException("несоответсвие типов: требуется число", expression, c);
        } else {
          if (type != typeof(bool) || temp != typeof(bool))
            throw new CompilerException("несоответсвие типов: требуется булевское значение", expression, c);
        }
      }
      return type;
    }
    #endregion

    #region private static Type Check5(string expression,ref int c)
    private static Type Check5(string expression, ref int c) {
      Type type = Check4(expression, ref c);
      while (expression[c] == '=' || expression[c] == '>' || expression[c] == '<') {
        string @operator = expression[c++].ToString();
        if (expression[c] == '=' || expression[c] == '>' || expression[c] == '<')
          @operator += expression[c++].ToString();
        Type temp = Check4(expression, ref c);
        if ((type == typeof(bool) && temp != typeof(bool)) ||
          (type != typeof(bool) && temp == typeof(bool)))
          throw new CompilerException("несоответсвие типов", expression, c);
        type = typeof(bool);
      }
      return type;
    }
    #endregion

    #region public static Type Check(string expression)
    public static Type Check(string expression) {
      int c = 0;
      expression = expression.Replace(" ", string.Empty) + " ";
      Type type = Check5(expression, ref c);
      if (expression[c] != ' ')
        throw new CompilerException("синтаксическая ошибка", expression, c);
      return type;
    }
    #endregion
  }
}
