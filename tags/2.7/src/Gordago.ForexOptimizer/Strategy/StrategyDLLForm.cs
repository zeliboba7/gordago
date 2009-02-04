/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Xml.Serialization;
using System.IO;
using Gordago.Analysis;
#endregion

namespace Gordago.Strategy {
  partial class StrategyDLLForm:Form, IStrategyForm {

    private string _filename;
    private bool _destroy;
    private bool _isopen;
    private Gordago.Analysis.Strategy[] _strategyes;
    private TestReport _report;
    private string[] _strategyNames;

    public StrategyDLLForm() {
      InitializeComponent();
      this.StartPosition = FormStartPosition.Manual;
      _strategyes = new Gordago.Analysis.Strategy[0];
      _strategyNames = new string[] { };
    }

    #region public string[] StrategyNames
    public string[] StrategyNames {
      get { return _strategyNames; }
    }
    #endregion

    #region public string FileName
    public string FileName {
      get {
        return _filename;
      }
      set {
        _filename = value;
      }
    }
    #endregion

    #region public bool IsDestroy
    public bool IsDestroy {
      get {
        return _destroy;
      }
      set {
        _destroy = value; ;
      }
    }
    #endregion

    #region public bool IsOpen
    public bool IsOpen {
      get {
        return _isopen;
      }
      set {
        _isopen = value;
      }
    }
    #endregion

    #region public Gordago.Analysis.Strategy Strategy
    public Gordago.Analysis.Strategy Strategy {
      get {
        Gordago.Analysis.Strategy strategy = this._lstStrategy.SelectedItem as Gordago.Analysis.Strategy;
        return strategy;
      }
    }
    #endregion

    private Assembly _ass;

    #region public bool LoadFromFile(string filename)
    public bool LoadFromFile(string filename) {
      this.FileName = filename;
      List<string> sa = new List<string>();
      ArrayList al = new ArrayList();
      try {
        _ass = Assembly.LoadFile(filename);
        Type[] types = _ass.GetTypes();
        foreach(Type type in types) {
          if(type.BaseType == typeof(Gordago.Analysis.Strategy) && type.IsPublic) {
            Gordago.Analysis.Strategy strategy = Activator.CreateInstance(type) as Gordago.Analysis.Strategy;
            al.Add(strategy);
            _lstStrategy.Items.Add(strategy);
            sa.Add(strategy.ToString());
//            this.SaveStrategy(strategy);
          }
        }

      } catch { }
      _strategyes = (Gordago.Analysis.Strategy[])al.ToArray(typeof(Gordago.Analysis.Strategy));
      if(_strategyes.Length > 0)
        _lstStrategy.SelectedIndex = 0;
      _strategyNames = sa.ToArray();
      return true;
    }
    #endregion

    #region private void _lstStrategy_SelectedIndexChanged(object sender, EventArgs e)
    private void _lstStrategy_SelectedIndexChanged(object sender, EventArgs e) {
      _propGrid.SelectedObject = _lstStrategy.SelectedItem;
    }
    #endregion

    #region private void SaveStrategy(Gordago.Analysis.Strategy strategy)
    private void SaveStrategy(Gordago.Analysis.Strategy strategy) {
      string file = Cursit.Utils.FileEngine.GetFileNameFromPath(this.FileName);
      string dir = FileName.Replace(file, "");
      string filexml = dir + strategy.GetType().FullName + ".xml";

      /* Если производить загрузку модуля не из корневой папки программы, 
       * необходимые файлы дополнительных библиотек не могут быть найдены.
       * Возникает событие, в котором необходимо вернуть сборку искомой библитеки
       * в данном случае это MetaQuotesHistory.dll */

      AppDomain currentDomain = AppDomain.CurrentDomain;
      currentDomain.AssemblyResolve += new ResolveEventHandler(OnAssemblyResolveEventHandler);

      try {
        XmlSerializer xmlser = new XmlSerializer(strategy.GetType());
        TextWriter writer = new StreamWriter(filexml);
        xmlser.Serialize(writer, strategy);
        writer.Close();
      } finally {
        currentDomain.AssemblyResolve -= new ResolveEventHandler(OnAssemblyResolveEventHandler);
      }
    }
    #endregion

    #region private Assembly OnAssemblyResolveEventHandler(object sender, ResolveEventArgs args)
    private Assembly OnAssemblyResolveEventHandler(object sender, ResolveEventArgs args) {
      return typeof(ISymbol).Assembly;
    }
    #endregion

    #region public Gordago.Analysis.TestReport TestReport
    public Gordago.Analysis.TestReport TestReport {
      get {
        return _report;
      }
      set {
        this._report = value;
      }
    }
    #endregion

    #region public CompileDllData Compile(string strategyName)
    public CompileDllData Compile(string strategyName) {

      foreach (object o in this._lstStrategy.Items) {
        if (strategyName == o.ToString()) {
          this._lstStrategy.SelectedItem = o;
          break;
        }
      }

      if(this.Strategy == null) {
        return null;
      }

      return new CompileDllData(new TradeVariables(), this.Strategy);
    }
    #endregion

    #region public void SetTestStatus(bool isStart)
    public void SetTestStatus(bool isStart) {
      _lstStrategy.Enabled =
        _propGrid.Enabled = !isStart;
    }
    #endregion

  }
}

