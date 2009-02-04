/**
* @version $Id: iAcceleratorOscillator.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Indicators
{
  using System;
  using System.Collections.Generic;
  using System.Text;

  public class iAcceleratorOscillator:Indicator {

    public class FxAO:Function {

      private readonly int _fast;

      public FxAO(IFunction inData, int fast, int slow, int smooth):base(inData) {

      }

      protected override void OnCompute() {

      }
    }
  }
}
