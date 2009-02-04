/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Gordago.Windows.Forms {
  class ScreenRectTSP {
    private Rectangle _bounds;
    private Rectangle _maxBounds;

    private ToolStripPanel _tsPanel;

    public ScreenRectTSP(ToolStripPanel tsPanel, Rectangle boudns, Rectangle region) {
      _tsPanel = tsPanel;
      _bounds = boudns;
      _maxBounds = region;
    }

    #region public Rectangle Bounds
    public Rectangle Bounds {
      get { return this._bounds; }
    }
    #endregion

    #region public Rectangle Region
    public Rectangle Region {
      get { return this._maxBounds; }
    }
    #endregion

    #region public ToolStripPanel TSPanel
    public ToolStripPanel TSPanel {
      get { return _tsPanel; }
    }
    #endregion
  }
}
