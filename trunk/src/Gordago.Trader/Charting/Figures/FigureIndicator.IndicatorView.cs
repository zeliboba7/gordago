/**
* @version $Id: FigureIndicator.IndicatorView.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Charting.Figures
{
  using System;
  using System.Collections.Generic;
  using System.Text;

  partial class FigureIndicator {

    public class IndicatorView {

      private readonly Indicator _indicator;
      private readonly FunctionViewInfo[] _functions;
      private readonly TimeFrame _timeFrame;

      public IndicatorView(TimeFrame timeFrame, Indicator indicator, FunctionViewInfo[] functionsView) {
        _indicator = indicator;
        _functions = functionsView;
        _timeFrame = timeFrame;
      }

      #region public Indicator Indicator
      public Indicator Indicator {
        get { return _indicator; }
      }
      #endregion

      #region public FunctionView[] FunctionsView
      public FunctionViewInfo[] FunctionsView {
        get { return this._functions; }
      }
      #endregion

      #region public TimeFrame TimeFrame
      public TimeFrame TimeFrame {
        get { return _timeFrame; }
      }
      #endregion
    }

  }
}
