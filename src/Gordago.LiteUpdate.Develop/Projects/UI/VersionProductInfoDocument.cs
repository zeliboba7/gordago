/**
* @version $Id: VersionProductInfoDocument.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate.Develop.Projects
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Drawing;
  using System.Data;
  using System.Text;
  using System.Windows.Forms;

  public partial class VersionProductInfoDocument : ProjectDocument {

    #region public class DocumentKey : AbstractDocumentKey
    public class DocumentKey : AbstractDocumentKey {
      private readonly int _hashCode;

      public DocumentKey(VersionInfo version) {
        _hashCode = ((string)(version.Number.ToString() + "UserInfo")).GetHashCode();
      }

      public override int GetHashCode() {
        return _hashCode;
      }

      public override bool Equals(object obj) {
        if (!(obj is DocumentKey))
          return false;
        return (obj as DocumentKey)._hashCode == _hashCode;
      }
    }
    #endregion

    private readonly VersionInfo _version;

    public VersionProductInfoDocument(VersionInfo version) {
      _version = version;
      InitializeComponent();

      this.SetKey(new DocumentKey(version));
      this.Text = this.TabText = string.Format("Version {0}: Version Info", version.Number);
    }

    #region protected override void OnLoad(EventArgs e)
    protected override void OnLoad(EventArgs e) {
      base.OnLoad(e);
      _htmlEditor.TextHtml = _version.InfoHtml;
      _htmlEditor.UserTextChanged += new EventHandler(_htmlEditor_UserTextChanged);
    }
    #endregion

    #region private void _htmlEditor_UserTextChanged(object sender, EventArgs e)
    private void _htmlEditor_UserTextChanged(object sender, EventArgs e) {
      _version.InfoHtml = _htmlEditor.TextHtml;
    }
    #endregion
  }
}
