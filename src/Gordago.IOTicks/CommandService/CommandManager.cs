/**
* @version $Id: CommandManager.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.IOTicks
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Threading;
  using System.IO;
  using System.Globalization;
  using Gordago.Trader.Data;
  using System.Diagnostics;
    using Gordago.Trader;

  delegate void CommandEventHandler(object sender, CommandEventArgs e);
  delegate void CommandProcessEventHandler(object sender, CommandProcessEventArgs e);

  #region class CommandEventArgs
  class CommandEventArgs:EventArgs {
    
    private readonly CommandType _command;

    public CommandEventArgs(CommandType cmd):base() {
      _command = cmd;
    }

    public CommandType Command {
      get { return _command; }
    }
  }
  #endregion

  #region class CommandProcessEventArgs : ProcessEventArgs
  class CommandProcessEventArgs : ProcessEventArgs {
    private readonly object _data;
    
    public CommandProcessEventArgs(object data, int total, int current)
      : base(total, current) {
      _data = data;
    }

    #region public Object Data
    public Object Data {
      get { return _data; }
    }
    #endregion
  }
  #endregion

  class CommandManager {
    public event CommandEventHandler StartCommand;
    public event CommandEventHandler StopCommand;
    public event CommandProcessEventHandler ProcessCommand;

    private CommandType _currentCommand = CommandType.None;

    public CommandManager() {
      Thread th = new Thread(new ThreadStart(this.Process));
      th.IsBackground = true;
      th.Start();
    }

    #region public CommandType Command
    public CommandType Command {
      get { return _currentCommand; }
    }
    #endregion

    #region protected virtual void OnStartCommand(CommandEventArgs e)
    protected virtual void OnStartCommand(CommandEventArgs e) {
      if (StartCommand != null)
        this.StartCommand(this, e);
    }
    #endregion

    #region protected virtual void OnStopCommand(CommandEventArgs e)
    protected virtual void OnStopCommand(CommandEventArgs e) {
      if (StopCommand != null)
        this.StopCommand(this, e);
    }
    #endregion

    #region protected virtual void OnCommandProcess(CommandProcessEventArgs e)
    protected virtual void OnCommandProcess(CommandProcessEventArgs e) {
      if (ProcessCommand != null)
        ProcessCommand(this, e);
    }
    #endregion

    #region private void Process()
    private void Process() {
      while (true){
        if (_currentCommand == CommandType.None) {
          Thread.Sleep(10);
          continue;
        }
        Trace.TraceInformation("CommandManager.Process - Start: Command={0}", _currentCommand);
        this.OnStartCommand(new CommandEventArgs(_currentCommand));
        switch (_currentCommand) {
          case CommandType.LoadSymbols:
            Global.History.Load();
            break;
          case CommandType.Import:
            this.ImportProcess();
            break;
        }

        CommandType savedType = _currentCommand;
        _currentCommand = CommandType.None;
        Trace.TraceInformation("CommandManager.Process - Stop: Command={0}", savedType);
        this.OnStopCommand(new CommandEventArgs(savedType));
      }
    }
    #endregion

    #region private void StartCommandMethod(CommandType command)
    private void StartCommandMethod(CommandType command) {
      if (_currentCommand != CommandType.None)
        throw (new Exception("CommandManager is busy"));
      _currentCommand = command;
    }
    #endregion

    #region public void LoadSymbols()
    public void LoadSymbols() {
      this.StartCommandMethod(CommandType.LoadSymbols);
    }
    #endregion

    #region public void Import()
    public void Import() {
      this.StartCommandMethod(CommandType.Import);
    }
    #endregion

    #region private void FillGCFiles(DirectoryInfo dir, List<FileInfo> files)
    private void FillGCFiles(DirectoryInfo dir, List<FileInfo> files) {
      DirectoryInfo[] dirs = dir.GetDirectories();
      foreach (DirectoryInfo directory in dirs) {
        this.FillGCFiles(directory, files);
      }
      files.AddRange(dir.GetFiles("*.csv"));
    }
    #endregion

    #region private void ImportProcess()
    private void ImportProcess() {
      DirectoryInfo dir = Global.Setup.ImportGainCapitalDirectory;
      List<FileInfo> files = new List<FileInfo>();
      this.FillGCFiles(dir, files);

      for (int i = 0; i < files.Count; i++) {
        this.ImportFromGC(files[i]);
        OnCommandProcess(new CommandProcessEventArgs(files[i], files.Count, i));
      }
    }
    #endregion

    #region private int GetDigitsGC(string number)
    private int GetDigitsGC(string number) {
      string[] sa = number.Split('.');
      if (sa.Length != 2)
        return 0;

      number = sa[1];
      number = number.Trim();
      while (number.Length > 0 && number[number.Length-1] == '0') {
        number = number.Substring(0, number.Length - 1);
      }
      return number.Length;
    }
    #endregion

    #region private void ImportFromGC(FileInfo file)
    private void ImportFromGC(FileInfo file) {
      Trace.TraceInformation("CommandManager.ImportFromGC({0}) - Start", file.FullName);
      DateTimeFormatInfo dateTimeFormatInfo = new CultureInfo("en-us").DateTimeFormat;

      string sname = "";
      ISymbol symbol = null;
      
      string dp = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

      List<Tick> ticks = new List<Tick>();
      bool error = false;
      int digits = 0;
      StreamReader sr = file.OpenText();
      try {
        long time = DateTime.Now.Ticks;
        string input;
        while ((input = sr.ReadLine()) != null) {

          input = input.Replace("\"", "");

          string[] sa = input.Split(new char[] { ',' });
          string symbolname = sa[1].Replace("/", "");

          if (sname == "") {
            sname = symbolname;
            symbol = Global.History[symbolname];
          }

          /* normalize GMT */
          DateTime dtm = DateTime.Parse(sa[2], dateTimeFormatInfo).AddHours(5);

          string sbid = sa[3].Replace(".", dp);
          string sask = sa[4].Replace(".", dp);

          float bid = Convert.ToSingle(sbid);
          float ask = Convert.ToSingle(sask);

          if (symbol == null) {
            digits = Math.Max(digits, this.GetDigitsGC(sa[3]));
          }

          ticks.Add(new Tick(dtm.Ticks, bid));

          #region if (DateTime.Now.Ticks - 5000000L > time) {...}
          if (DateTime.Now.Ticks - 5000000L > time) {
            this.OnCommandProcess(new CommandProcessEventArgs(file, Convert.ToInt32(file.Length), Convert.ToInt32(sr.BaseStream.Position)));
            time = DateTime.Now.Ticks;
          }
          #endregion
        }

      } catch (Exception e) {
        error = true;
        Trace.TraceInformation("CommandManager.ImportFromGC({0}) - Error: {1}", file.FullName, e.Message);
      }

      if (!error) {
        Trace.TraceInformation("CommandManager.ImportFromGC({0}) - Load={1}Ticks", file.FullName, ticks.Count);
        if (symbol == null) {
          symbol = Global.History.Create(sname, digits);
          Global.History.Add(symbol);
        }

        ITicksManager tm = symbol.Ticks as ITicksManager;
        tm.Update(ticks.ToArray(), true);
        sr.Close();
        // file.Delete();
      }
      Trace.TraceInformation("CommandManager.ImportFromGC({0}) - Stop", file.FullName);
    }
    #endregion
  }
}

