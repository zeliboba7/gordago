/**
* @version $Id: ChartDocumentKey.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.FO.Charting
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using Gordago.Trader;
  using Gordago.Docking;

  class ChartDocumentKey : TabbedDocument.AbstractDocumentKey {
    
    private readonly int _hashCode;

    public ChartDocumentKey() {
      _hashCode = Guid.NewGuid().GetHashCode();
    }

    #region public override int GetHashCode()
    public override int GetHashCode() {
      return _hashCode;
    }
    #endregion

    #region public override bool Equals(object obj)
    public override bool Equals(object obj) {
      if (!(obj is ChartDocumentKey))
        return false;

      return (obj as ChartDocumentKey)._hashCode == _hashCode;
    }
    #endregion
  }
}
