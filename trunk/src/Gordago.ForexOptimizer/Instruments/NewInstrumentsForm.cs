/**
* @version $Id: NewInstrumentsForm.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.FO.Instruments
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Data;
  using System.Drawing;
  using System.Text;
  using System.Windows.Forms;

  partial class NewInstrumentsForm : Form {

    #region class TreeNodeInstType : TreeNode
    class TreeNodeInstType : TreeNode {
      private InstrumentType _instType;
      private CodeLang _codeLang;

      public TreeNodeInstType(string text, InstrumentType instType, CodeLang codeLang) {
        _instType = instType;
        _codeLang = codeLang;
        this.Text = text;
      }

      #region public InstrumentType InstrumentType
      public InstrumentType InstrumentType {
        get { return _instType; }
      }
      #endregion

      #region public CodeEditorLang CodeEditorLang
      public CodeLang CodeEditorLang {
        get { return _codeLang; }
      }
      #endregion
    }
    #endregion

    private TreeNode _nodeIndicatorCSharp;
    private TreeNode _nodeIndicatorVB;
    private TreeNode _nodeStrategyCSharp;
    private TreeNode _nodeStreategyVB;

    private InstrumentType _instType;
    private CodeLang _codeLang;
    
    public NewInstrumentsForm() {
      InitializeComponent();
    }

    #region public InstrumentType InstrumentType
    public InstrumentType InstrumentType {
      get { return _instType; }
    }
    #endregion

    #region public string InstrumentName
    public string InstrumentName {
      get { return _txtName.Text; }
    }
    #endregion

    #region public CodeEditorLang CodeLang
    public CodeLang CodeLang {
      get { return _codeLang; }
    }
    #endregion

    protected override void OnLoad(EventArgs e) {
      base.OnLoad(e);

      string langPart = "Instruments/New";

      this.Text = Global.Languages[langPart]["New"];
      this._lblName.Text = Global.Languages[langPart]["Name"];
      this._btnOK.Text = Global.Languages["Buttons"]["OK"];
      this._btnCancel.Text = Global.Languages["Buttons"]["Cancel"];

      string strCSharp = Global.Languages[langPart]["Visual C#"];
      string strVB = Global.Languages[langPart]["Visual Basic"];

      TreeNode nodeStrategy = new TreeNode(Global.Languages[langPart]["Strategy"]);
      _nodeStrategyCSharp = new TreeNodeInstType(strCSharp, InstrumentType.Strategy, CodeLang.CSharp);
      _nodeStreategyVB = new TreeNodeInstType(strVB, InstrumentType.Strategy, CodeLang.VisualBasic);

      nodeStrategy.Nodes.Add(_nodeStrategyCSharp);
      nodeStrategy.Nodes.Add(_nodeStreategyVB);
      _treeView.Nodes.Add(nodeStrategy);

      TreeNode nodeIndicator = new TreeNode(Global.Languages[langPart]["Indicator"]);
      _nodeIndicatorCSharp = new TreeNodeInstType(strCSharp, InstrumentType.Indicator, CodeLang.CSharp);
      _nodeIndicatorVB = new TreeNodeInstType(strVB, InstrumentType.Indicator, CodeLang.VisualBasic);

      nodeIndicator.Nodes.Add(_nodeIndicatorCSharp);
      nodeIndicator.Nodes.Add(_nodeIndicatorVB);
      _treeView.Nodes.Add(nodeIndicator);
    }

    #region private void _btnOK_Click(object sender, EventArgs e)
    private void _btnOK_Click(object sender, EventArgs e) {
      if (!(_treeView.SelectedNode is TreeNodeInstType))
        return;

      TreeNodeInstType node = _treeView.SelectedNode as TreeNodeInstType;
      _instType = node.InstrumentType;
      _codeLang = node.CodeEditorLang;

      this.DialogResult = DialogResult.OK;
      this.Close();
    }
    #endregion

    #region private void _btnCancel_Click(object sender, EventArgs e)
    private void _btnCancel_Click(object sender, EventArgs e) {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }
    #endregion
  }
}