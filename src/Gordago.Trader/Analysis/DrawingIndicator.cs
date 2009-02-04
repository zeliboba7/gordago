/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Drawing;
using Gordago.Analysis.Chart;

namespace Gordago.Analysis {

  #region public enum DrawingIndicatorStatus
  public enum DrawingIndicatorStatus{
		Default,
		MouseSelected,
		UserSelected
  }
  #endregion

  public abstract class DrawingIndicator {
		
		private IVector[] _vectors;
		private IBarList _bars;
		private IChartBox _chartbox;
		private ChartFigureIndicator _cfindicator;
		private int _barwidth;

		private DrawingIndicatorStatus _status;
		
		#region public Vector[] Vectors
		public IVector[] Vectors{
			get{return this._vectors;}
		}
		#endregion

		#region public DrawingIndicatorStatus Status
		public DrawingIndicatorStatus Status{
			get{ return _status; }
		}
		#endregion

    #region public int BarWidth
    public int BarWidth{
			get{return this._barwidth;}
		}
		#endregion

    #region protected IChartBox ChartBox
    protected IChartBox ChartBox {
      get { return _chartbox; }
    }
    #endregion

    #region protected ChartFigureIndicator Figure
    protected ChartFigureIndicator Figure {
      get { return _cfindicator; }
    }
    #endregion

    #region internal void SetBarWidth(int barwidth)
    internal void SetBarWidth(int barwidth){
			_barwidth = barwidth;
		}
		#endregion

    #region internal void SetChartBox(IChartBox chartbox, ChartFigureIndicator cfindicator)
    internal void SetChartBox(IChartBox chartbox, ChartFigureIndicator cfindicator){
			this._cfindicator = cfindicator;
			this._chartbox = chartbox;
    }
    #endregion

    #region internal void SetBars(IBarList bars)
    internal void SetBars(IBarList bars){
			this._bars = bars;
    }
    #endregion

    #region internal void SetVectors(Vector[] vectors)
    internal void SetVectors(IVector[] vectors){
			this._vectors = vectors;
		}
		#endregion

		#region internal void SetStatus(DrawingIndicatorStatus status)
		internal void SetStatus(DrawingIndicatorStatus status){
			this._status = status;
		}
		#endregion

    protected internal virtual void PaintElementInit(Graphics g) { }
    protected internal virtual void PaintElement(Graphics g, int funcIndex, int vectorIndex, int x) { }
    protected internal virtual void PaintElementFinal(Graphics g) { }
	}
}
