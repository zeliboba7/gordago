/**
* @version $Id: StartPageDocument.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate.Develop
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Data;
  using System.Drawing;
  using System.Text;
  using System.Windows.Forms;
  using Gordago.Docking;
  using Gordago.LiteUpdate.Develop.Projects;
  using System.IO;

  public partial class StartPageDocument : TabbedDocument {

    #region public class DocumentKey : AbstractDocumentKey
    public class DocumentKey : AbstractDocumentKey {
      private int _hashCode;

      public DocumentKey() {
        string s = "AA1DC487-1340-4968-899C-7400735F8D22";
        _hashCode = s.GetHashCode();
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

    private static readonly string T_VERSION = "<!--VERSION-->";
    private static readonly string T_BEGIN_ROW = "<!--BEGIN_ROW-->";
    private static readonly string T_END_ROW = "<!--END_ROW-->";
    private static readonly string T_ROW_VALUE = "<!--ROW_VALUE-->";
    private readonly List<string> _recentProjects = new List<string>();

    private bool _isInitializing = false;

    public StartPageDocument() {
      InitializeComponent();
      this.SetKey(new DocumentKey());
      this.UpdatePage();
      this.TabText = Global.Languages["StartPage"]["Start Page"];
    }

    #region public static string[] Split(string str, string separator)
    public static string[] Split(string str, string separator) {
      List<string> list = new List<string>();
      int indexOf = str.IndexOf(separator);
      if (indexOf > -1) {
        string temp = str.Substring(0, indexOf);
        list.Add(temp);
        str = str.Substring(indexOf + separator.Length);
        string[] sa = Split(str, separator);
        list.AddRange(sa);
      } else if (str.Length > 0) {
        list.Add(str);
      }
      return list.ToArray();
    }
    #endregion

    public void UpdatePage() {
      RecentProjectManager rProjects = Global.MainForm.ProjectManager.RecentProjects;
      FileInfo file = new FileInfo( Path.Combine(Global.Setup.TemplateDirectory.FullName, "StartPage.html"));
      _recentProjects.Clear();
      string html = "";
      if (!file.Exists) {

      } else {
        html = File.ReadAllText(file.FullName);

        string[] sa = Split(html, T_BEGIN_ROW);
        string top = sa[0];
        sa = Split(sa[1], T_END_ROW);
        string row = sa[0];
        string bottom = sa[1];

        html = top;

        int limit = 0;
        foreach (RecentProject rproject in Global.MainForm.ProjectManager.RecentProjects) {
          if (++limit > 4)
            break;

          string link = string.Format("<a href='project:{0}'>{1}</a>", _recentProjects.Count, rproject.Name);
          _recentProjects.Add(rproject.File.FullName);

          html += row.Replace(T_ROW_VALUE, link);
        }
        for (int i = limit; i < 5; i++) {
          html += row.Replace(T_ROW_VALUE, "&nbsp;");
        }

        html += row.Replace(T_ROW_VALUE,"Open: <a href='project:open'>Project...</a>");
        html += row.Replace(T_ROW_VALUE,"Create: <a href='project:create'>Project...</a>");

        html += bottom;
      }
      Version version = typeof(IDE).Assembly.GetName().Version;

      html = html.Replace(T_VERSION, string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build));

      _isInitializing = true;
      this._webBrowser.DocumentText = html;
      _isInitializing = false;
    }

    #region private void _webBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
    private void _webBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e) {
      if (_isInitializing)
        return;
      e.Cancel = true;
      if (e.Url.OriginalString.IndexOf("project:") > -1) {
        string command = e.Url.OriginalString.Substring("project:".Length);
        if (command == "open") {
          Global.MainForm.ProjectManager.OpenProject();
        } else if (command == "create") {
          Global.MainForm.ProjectManager.ProjectNew();
        } else {
          int index = Convert.ToInt32(command);
          Global.MainForm.ProjectManager.OpenProject(new FileInfo(_recentProjects[index]));
        }
      }
    }
    #endregion
  }
}