/**
* @version $Id: HostCallbackImplementation.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.FO.Instruments
{
  using System;
  using ICSharpCode.SharpDevelop.Dom;
  using System.Windows.Forms;

  static class HostCallbackImplementation {

    public static void Register(CodeEditorDocument document) {
      // Must be implemented. Gets the parse information for the specified file.
      HostCallback.GetParseInformation = delegate(string fileName) {
        if (fileName != document.ContentFile.FullName)
          throw new Exception("Unknown file");
        if (document.LastCompilationUnit == null)
          return null;
        ParseInformation pi = new ParseInformation();
        pi.ValidCompilationUnit = document.LastCompilationUnit;
        return pi;
      };

      // Must be implemented. Gets the project content of the active project.
      HostCallback.GetCurrentProjectContent = delegate {
        return Global.Projects.CSProjectContent;
      };

      // The default implementation just logs to Log4Net. We want to display a MessageBox.
      // Note that we use += here - in this case, we want to keep the default Log4Net implementation.
      HostCallback.ShowError += delegate(string message, Exception ex) {
        MessageBox.Show(message + Environment.NewLine + ex.ToString());
      };
      HostCallback.ShowMessage += delegate(string message) {
        MessageBox.Show(message);
      };
      HostCallback.ShowAssemblyLoadError += delegate(string fileName, string include, string message) {
        MessageBox.Show("Error loading code-completion information for "
                        + include + " from " + fileName
                        + ":\r\n" + message + "\r\n");
      };
    }
  }
}
