/**
* @version $Id: InstrumentBuilder.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using Gordago.Trader.Indicators;
  using Gordago.Trader.Builder;

  public class InstrumentBuilder:ClassBuilder {

    public InstrumentBuilder(Type type) : base(type) {

    }

    /// <summary>
    /// Пример:
    /// iMACD macd = new i
    /// 
    /// </summary>
    /// <param name="barsData"></param>

    public void CreateInstance(IBarsData barsData) {
      // iMACD macd = new iMACD(
      if (this.Constructors.Length == 0)
        throw(new Exception(string.Format("Public constructor in {0} not found", this.ClassType.FullName)));

      // ClassConstructor constructor = Constructors[if];
    }
  }
}
