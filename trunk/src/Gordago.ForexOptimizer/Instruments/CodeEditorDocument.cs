/**
* @version $Id: CodeEditorDocument.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.FO.Instruments
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Windows.Forms;
  using Gordago.Docking;
  using System.IO;
  using ICSharpCode.SharpDevelop.Dom;
  using ICSharpCode.TextEditor.Gui.CompletionWindow;

  using NRefactory = ICSharpCode.NRefactory;
  using Dom = ICSharpCode.SharpDevelop.Dom;
  using Gordago.Core;

  public partial class CodeEditorDocument : TabbedDocument, ITabbedDocument, ITabbedFileDocument {

    public event EventHandler ContentChanged;
    public event EventHandler ContentSaved;

    private bool _isContentSaved = true;

    private FileInfo _codeFile;
    private string _guid;
    private CodeCompletionWindow codeCompletionWindow;
    private ICompilationUnit _lastCompilationUnit;
    private DateTime _lastUpdateTimeCompilationUnit = DateTime.Now.AddMinutes(-1);

    public CodeEditorDocument(FileInfo file)
      : this() {
      this.InitCodeEditor(file);
    }

    public CodeEditorDocument() {
      InitializeComponent();
    }

    #region public bool IsContentSaved
    public bool IsContentSaved {
      get { return _isContentSaved; }
    }
    #endregion

    #region public FileInfo ContentFile
    public FileInfo ContentFile {
      get { return _codeFile; }
    }
    #endregion

    #region public string GUID
    public string GUID {
      get { return _guid; }
    }
    #endregion

    #region public ImageList Images
    public ImageList Images {
      get { return imageList1; }
    }
    #endregion

    #region public ICompilationUnit LastCompilationUnit
    public ICompilationUnit LastCompilationUnit {
      get {
        if (DateTime.Now.AddSeconds(-3) <= _lastUpdateTimeCompilationUnit)
          return _lastCompilationUnit;

        string code = null;
        Invoke(new MethodInvoker(delegate {
          code = _textEditor.Text;
        }));

        TextReader textReader = new StringReader(code);
        ICompilationUnit newCompilationUnit;
        using (NRefactory.IParser p = NRefactory.ParserFactory.CreateParser(NRefactory.SupportedLanguage.CSharp, textReader)) {
          p.Parse();
          newCompilationUnit = ConvertCompilationUnit(p.CompilationUnit);
        }
        // Remove information from lastCompilationUnit and add information from newCompilationUnit.
        Global.Projects.CSProjectContent.UpdateCompilationUnit(_lastCompilationUnit, newCompilationUnit, this.ContentFile.FullName);
        _lastCompilationUnit = newCompilationUnit;

        _lastUpdateTimeCompilationUnit = DateTime.Now;
        return _lastCompilationUnit;
      }
    }
    #endregion

    #region private Dom.ICompilationUnit ConvertCompilationUnit(NRefactory.Ast.CompilationUnit cu)
    private Dom.ICompilationUnit ConvertCompilationUnit(NRefactory.Ast.CompilationUnit cu) {
      Dom.NRefactoryResolver.NRefactoryASTConvertVisitor converter;
      converter = new Dom.NRefactoryResolver.NRefactoryASTConvertVisitor(Global.Projects.CSProjectContent);
      cu.AcceptVisitor(converter, null);
      return converter.Cu;
    }
    #endregion

    #region private void InitCodeEditor(FileInfo file)
    private void InitCodeEditor(FileInfo file) {
      _codeFile = file;
      this.SetKey(new CodeEditorDocumentKey(file));
      this.Text = file.Name;
      _guid = Guid.NewGuid().ToString();
      string ext = _codeFile.Extension.ToLower();
      switch (ext) {
        case ".cs":
          _textEditor.SetHighlighting("C#");
          break;
        case ".vb":
          break;
      }
      
      _textEditor.ShowEOLMarkers = false;
      _textEditor.LoadFile(_codeFile.FullName);

      _textEditor.Document.DocumentChanged += new ICSharpCode.TextEditor.Document.DocumentEventHandler(Document_DocumentChanged);
      _textEditor.ActiveTextAreaControl.TextArea.KeyEventHandler += this.TextAreaKeyEventHandler;
      _textEditor.Disposed += this.CloseCodeCompletionWindow;

      HostCallbackImplementation.Register(this);
      this.UpdateText();
    }
    #endregion

    #region private void Document_DocumentChanged(object sender, ICSharpCode.TextEditor.Document.DocumentEventArgs e)
    private void Document_DocumentChanged(object sender, ICSharpCode.TextEditor.Document.DocumentEventArgs e) {
      _isContentSaved = false;
      this.UpdateText();
      this.OnContentChanged(EventArgs.Empty);
    }
    #endregion

    #region private bool TextAreaKeyEventHandler(char key)
    private bool TextAreaKeyEventHandler(char key) {
      if (codeCompletionWindow != null) {
        if (codeCompletionWindow.ProcessKeyEvent(key))
          return true;
      }
      if (key == '.') {
        ICompletionDataProvider completionDataProvider = new CodeCompletionProvider(this);

        codeCompletionWindow = CodeCompletionWindow.ShowCompletionWindow(
          this,					// The parent window for the completion window
          _textEditor, 					// The text editor to show the window for
          this.ContentFile.FullName,		// Filename - will be passed back to the provider
          completionDataProvider,		// Provider to get the list of possible completions
          key							// Key pressed - will be passed to the provider
        );
        if (codeCompletionWindow != null) {
          codeCompletionWindow.Closed += new EventHandler(CloseCodeCompletionWindow);
        }
      }
      return false;
    }
    #endregion

    #region private void CloseCodeCompletionWindow(object sender, EventArgs e)
    private void CloseCodeCompletionWindow(object sender, EventArgs e) {
      if (codeCompletionWindow != null) {
        codeCompletionWindow.Closed -= new EventHandler(CloseCodeCompletionWindow);
        codeCompletionWindow.Dispose();
        codeCompletionWindow = null;
      }
    }
    #endregion

    #region public void LoadProperties(FileInfo file)
    public void LoadProperties(FileInfo file) {
      EasyProperties ps = new EasyProperties();
      ps.Load(file);
      _guid = ps.GetValue<string>("GUID", Guid.NewGuid().ToString());
      _codeFile = new FileInfo(ps.GetValue<string>("CodeFile", "o:\asdf.asdf"));
      if (!_codeFile.Exists)
        throw new Exception();
      this.InitCodeEditor(_codeFile);
    }
    #endregion

    #region public void SaveProperties(FileInfo file)
    public void SaveProperties(FileInfo file) {
      EasyProperties ps = new EasyProperties();
      ps.SetValue<string>("GUID", _guid);
      ps.SetValue<string>("CodeFile", _codeFile.FullName);
      ps.Save(file);
    }
    #endregion

    #region protected virtual void OnContentSaved(EventArgs e)
    protected virtual void OnContentSaved(EventArgs e) {
      if (this.ContentSaved != null)
        this.ContentSaved(this, e);
    }
    #endregion

    #region protected virtual void OnContentChanged(EventArgs e)
    protected virtual void OnContentChanged(EventArgs e) {
      if (this.ContentChanged != null)
        this.ContentChanged(this, e);
    }
    #endregion

    #region public void SaveContent()
    public void SaveContent() {
      _isContentSaved = true;
      _textEditor.SaveFile(_codeFile.FullName);
      this.OnContentSaved(EventArgs.Empty);
      this.UpdateText();
    }
    #endregion

    #region public void SaveAsContent(FileInfo file)
    public void SaveAsContent(FileInfo file) {
      FileInfo oldFile = _codeFile;
      _codeFile = file;
      this.SaveContent();
      oldFile.Delete();
      this.UpdateText();
    }
    #endregion

    private void UpdateText() {
      this.Text = this.TabText = 
        _codeFile.Name + (_isContentSaved ? "" : "*");
    }
  }
}