/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.Analysis.Chart {
  public class SymbolsChartPanelProperties:ChartPanelProperties {

    private List<string> _hideSymbols;

    public SymbolsChartPanelProperties(ChartPanel chartPanel):base(chartPanel) {
      _hideSymbols = new List<string>();
    }

    #region protected override void OnLoadTemplate(XmlNodeManager nodeManager)
    protected override void OnLoadTemplate(XmlNodeManager nodeManager) {
      base.OnLoadTemplate(nodeManager);
      
      string symbols = nodeManager.GetAttributeString("HideSymbols", "");

      _hideSymbols.Clear();
      string[] sa = symbols.Split('|');
      for (int i = 0; i < sa.Length; i++) {
        this.AddHideSymbol(sa[i]);
      }
    }
    #endregion

    #region protected override void OnSaveTemplate(XmlNodeManager nodeManager)
    protected override void OnSaveTemplate(XmlNodeManager nodeManager) {
      base.OnSaveTemplate(nodeManager);
      string[] sa = new string[_hideSymbols.Count];
      for (int i = 0; i < sa.Length; i++) {
        sa[i] = _hideSymbols[i];
      }
      string symbols = string.Join("|", sa);
      nodeManager.SetAttribute("HideSymbols", symbols);
    }
    #endregion

    #region public void AddHideSymbol(string symbolName)
    public void AddHideSymbol(string symbolName) {
      if (CheckHideSymbol(symbolName))
        return;
      _hideSymbols.Add(symbolName);
    }
    #endregion

    #region public bool CheckHideSymbol(string symbolName)
    public bool CheckHideSymbol(string symbolName) {
      for (int i = 0; i < _hideSymbols.Count; i++) {
        if (_hideSymbols[i] == symbolName)
          return true;
      }
      return false;
    }
    #endregion

    #region public void RemoveHideSymbol(string symbolName)
    public void RemoveHideSymbol(string symbolName) {
      _hideSymbols.Remove(symbolName);
    }
    #endregion

    #region public void ClearHideSymbol()
    public void ClearHideSymbol() {
      _hideSymbols.Clear();
    }
    #endregion
  }
}
