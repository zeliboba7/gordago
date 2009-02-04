/**
* @version $Id: ProjectsManager.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.FO.Instruments
{
  using System.Windows.Forms;
  using ICSharpCode;
  using ICSharpCode.SharpDevelop;
  using System.IO;
  using NRefactory = ICSharpCode.NRefactory;
  using Dom = ICSharpCode.SharpDevelop.Dom;
  using System;

  class ProjectsManager {

    public delegate void LoadReferencedAssembliesEventHandler(object sender, LoadReferencedAssembliesEventArgs e);

    #region public class LoadReferencedAssembliesEventArgs:EventArgs
    public class LoadReferencedAssembliesEventArgs : EventArgs {
      private readonly string _typeName;
      public LoadReferencedAssembliesEventArgs(string typeName)
        : base() {
        _typeName = typeName;
      }

      public string TypeName {
        get { return _typeName; }
      }
    }
    #endregion

    public event LoadReferencedAssembliesEventHandler LoadReferencedAssembly;

    //private ToolStripMenuItem _editMenuItem;
    //private ToolStripMenuItem _projectMenuItem;

    private readonly Dom.ProjectContentRegistry _pcRegistry;
    private readonly Dom.DefaultProjectContent _csProjectContent;

    public ProjectsManager() {
      _pcRegistry = new Dom.ProjectContentRegistry();
      _pcRegistry.ActivatePersistence(Path.Combine(Path.GetTempPath(), "FOCodeCompletion"));

      _csProjectContent = new Dom.DefaultProjectContent();
    }

    #region public Dom.DefaultProjectContent CSProjectContent
    public Dom.DefaultProjectContent CSProjectContent {
      get { return _csProjectContent; }
    }
    #endregion

    #region public void Load()
    public void Load() {
      _csProjectContent.Language = Dom.LanguageProperties.CSharp;
      _csProjectContent.AddReferencedContent(_pcRegistry.Mscorlib);

      string[] referencedAssemblies = {
				"System", 
        "System.Data", 
        "System.Drawing", 
        "System.Xml", 
        "System.Windows.Forms",
        "Gordago.Trader"
			};
      foreach (string assemblyName in referencedAssemblies) {
        if (LoadReferencedAssembly != null)
          this.LoadReferencedAssembly(this, new LoadReferencedAssembliesEventArgs(assemblyName));
        _csProjectContent.AddReferencedContent(_pcRegistry.GetProjectContentForReference(assemblyName, assemblyName));
      }
    }
    #endregion

    #region private Dom.ICompilationUnit ConvertCompilationUnit(NRefactory.Ast.CompilationUnit cu)
    private Dom.ICompilationUnit ConvertCompilationUnit(NRefactory.Ast.CompilationUnit cu) {
      Dom.NRefactoryResolver.NRefactoryASTConvertVisitor converter;
      converter = new Dom.NRefactoryResolver.NRefactoryASTConvertVisitor(_csProjectContent);
      cu.AcceptVisitor(converter, null);
      return converter.Cu;
    }
    #endregion
  }
}
