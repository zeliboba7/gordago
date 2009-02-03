/**
* @version $Id: MainForm.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.IOTicks
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Data;
  using System.Drawing;
  using System.Text;
  using System.Windows.Forms;
  using Gordago.Trader.Data;
  using System.IO;
    using Gordago.Trader;

  public partial class MainForm : Form {

    #region private class SymbolKey
    private class SymbolKey {
      private readonly int _hashCode;

      public SymbolKey(string symbolName) {
        _hashCode = symbolName.GetHashCode();
      }

      public override bool Equals(object obj) {
        return (obj as SymbolKey)._hashCode == _hashCode; ;
      }

      public override int GetHashCode() {
        return _hashCode;
      }
    }
    #endregion

    private class SymbolTask {
      public int Total = 0;
      public int Current = 0;
    }

    private DataTable _table;
    private long _savedTime = DateTime.Now.Ticks;
    private readonly Dictionary<SymbolKey, SymbolTask> _symbolsTask = new Dictionary<SymbolKey, SymbolTask>();

    public MainForm() {
      InitializeComponent();
      Global.CommandManager.StopCommand += new CommandEventHandler(CommandManager_StopCommand);
      Global.CommandManager.StartCommand += new CommandEventHandler(CommandManager_StartCommand);
      Global.CommandManager.ProcessCommand += new CommandProcessEventHandler(CommandManager_ProcessCommand);
      Global.History.SymbolAdded += new HistoryManagerEventHandler(History_SymbolAdded);

      _table = new DataTable("symbol");
      _table.Columns.Add("name", typeof(string));
      _table.Columns.Add("digits", typeof(int));
      _table.Columns.Add("from", typeof(string));
      _table.Columns.Add("to", typeof(string));
      _table.Columns.Add("ticks_count", typeof(string));
      _table.Columns.Add("task", typeof(string));
      _dataGridView.DataSource = _table.DefaultView;
    }

    #region private void History_SymbolAdded(object sender, HistoryManagerEventArgs e)
    private void History_SymbolAdded(object sender, HistoryManagerEventArgs e) {
      if (this.InvokeRequired) {
        this.Invoke(new HistoryManagerEventHandler(History_SymbolAdded), sender, e);
        return;
      }

      ISymbol symbol = e.Symbol;
      ITicksManager ticksDataCacheEvent = symbol.Ticks as ITicksManager;
      ticksDataCacheEvent.StartTask += new TicksManagerTaskEventHandler(ticksDataCacheEvent_StartTask);
      ticksDataCacheEvent.StopTask += new TicksManagerTaskEventHandler(ticksDataCacheEvent_StopTask);
      ticksDataCacheEvent.TaskProcessChanged += new TicksManagerTaskProcessEventHandler(ticksDataCacheEvent_TaskProcessChanged);

      DataRow row = _table.NewRow();
      row["name"] = symbol.Name;
      row["digits"] = symbol.Digits;
      this.UpdateRow(row, symbol.Ticks, "");
      _table.Rows.Add(row);
      this._dataGridView.Refresh();
    }
    #endregion

    #region private void UpdateRow(DataRow row, ITickCollection ticks, string task)
    private void UpdateRow(DataRow row, ITickCollection ticks, string task) {
      row["ticks_count"] = ticks.Count;
      if (ticks.Count > 0) {
        row["from"] = new DateTime (ticks[0].Time);
        row["to"] = new DateTime(ticks.Current.Time); ;
      }
      row["task"] = task;
    }
    #endregion

    #region private void CommandManager_ProcessCommand(object sender, CommandProcessEventArgs e)
    private void CommandManager_ProcessCommand(object sender, CommandProcessEventArgs e) {
      #region if (this.InvokeRequired) {...}
      if (this.InvokeRequired) {
        this.Invoke(new CommandProcessEventHandler(CommandManager_ProcessCommand), sender, e);
        return;
      }
      #endregion

      string text = "Process";

      switch (Global.CommandManager.Command) {
        case CommandType.Import:
          FileInfo file = e.Data as FileInfo;
          if (file != null) 
            text = string.Format("Import: {0}", file.Name);
          break;
      }

      _lblStatusInfo.Text = text;
      _progressBar.Maximum = e.Total;
      _progressBar.Value = e.Current;
    }
    #endregion

    #region private void CommandManager_StartCommand(object sender, CommandEventArgs e)
    private void CommandManager_StartCommand(object sender, CommandEventArgs e) {
      #region if (this.InvokeRequired) {...}
      if (this.InvokeRequired) {
        this.Invoke(new CommandEventHandler(CommandManager_StartCommand), sender, e);
        return;
      }
      #endregion

      string text = "";

      switch (e.Command) {
        case CommandType.LoadSymbols:
          text = "Load Symbols";
          break;
      }
      _lblStatusInfo.Text = text;
      this.SetEnabled(false);

    }
    #endregion

    #region private void CommandManager_StopCommand(object sender, CommandEventArgs e)
    private void CommandManager_StopCommand(object sender, CommandEventArgs e) {
      #region if (this.InvokeRequired) {...}
      if (this.InvokeRequired) {
        this.Invoke(new CommandEventHandler(CommandManager_StopCommand), sender, e);
        return;
      }
      #endregion

      switch (e.Command) {
        case CommandType.LoadSymbols:
          //this.UpdateTable();
          break;
      }
      _lblStatusInfo.Text = "Ready";
      _progressBar.Maximum = 1;
      _progressBar.Value = 0;
      this.SetEnabled(true);
    }
    #endregion

    #region private void SetEnabled(bool enabled)
    private void SetEnabled(bool enabled) {
      _btnImport.Enabled = enabled;
      _progressBar.Visible = !enabled;
    }
    #endregion

    #region protected override void OnLoad(EventArgs e)
    protected override void OnLoad(EventArgs e) {
      base.OnLoad(e);
      Global.CommandManager.LoadSymbols();
    }
    #endregion

    #region private void UpdateSymbolProgress()
    private void UpdateSymbolProgress() {
      bool visible = _symbolsTask.Count > 0;
      if (_pgsSymbols.Visible == visible)
        return;
      _pgsSymbols.Visible = visible;
      if (visible) {
        _pgsSymbols.Value = 0;
        _pgsSymbols.Maximum = 1;
      }
    }
    #endregion

    #region private void ticksDataCacheEvent_TaskProcessChanged(object sender, TicksManagerProcessEventArgs e)
    private void ticksDataCacheEvent_TaskProcessChanged(object sender, TicksManagerProcessEventArgs e) {
      if (this.InvokeRequired) {
        this.Invoke(new TicksManagerTaskProcessEventHandler(ticksDataCacheEvent_TaskProcessChanged), sender, e);
        return;
      }

      ISymbolInfo symbol = sender as ISymbolInfo;
      SymbolTask cST = _symbolsTask[new SymbolKey(symbol.Name)];
      cST.Total = e.Total;
      cST.Current = e.Current;

      if (DateTime.Now.Ticks - _savedTime < 10000000L)
        return;
      _savedTime = DateTime.Now.Ticks;

      int total = 0;
      int current = 0;
      foreach (SymbolTask st in _symbolsTask.Values) {
        total += st.Total;
        current += st.Current;
      }
      total = Math.Max(total, 1);
      _pgsSymbols.Maximum = total;
      _pgsSymbols.Value = current;
    }
    #endregion

    #region private void ticksDataCacheEvent_StartTask(object sender, TicksManagerTaskEventArgs e)
    private void ticksDataCacheEvent_StartTask(object sender, TicksManagerTaskEventArgs e) {
      if (this.InvokeRequired) {
        this.Invoke(new TicksManagerTaskEventHandler(ticksDataCacheEvent_StartTask), sender, e);
        return;
      }

      ISymbolInfo symbol = sender as ISymbolInfo;
      _symbolsTask.Add(new SymbolKey(symbol.Name), new  SymbolTask());

      foreach (DataGridViewRow row in _dataGridView.Rows) {
        if (row.Cells[0].Value.ToString() == symbol.Name) {
          row.DefaultCellStyle.BackColor = e.Task == TicksManagerTask.DataCaching ? Color.Red : Color.Green;
          row.Cells["task"].Value = e.Task.ToString();
          break;
        }
      }
      this.UpdateSymbolProgress();
    }
    #endregion

    #region private void ticksDataCacheEvent_StopTask(object sender, TicksManagerTaskEventArgs e)
    private void ticksDataCacheEvent_StopTask(object sender, TicksManagerTaskEventArgs e) {
      if (this.InvokeRequired) {
        this.Invoke(new TicksManagerTaskEventHandler(ticksDataCacheEvent_StopTask), sender, e);
        return;
      }
      ISymbolInfo symbol = sender as ISymbolInfo;

      _symbolsTask.Remove(new SymbolKey(symbol.Name));

      foreach (DataGridViewRow row in _dataGridView.Rows) {
        if (row.Cells[0].Value.ToString() == symbol.Name) {
          row.DefaultCellStyle.BackColor = Color.White;
          break;
        }
      }
      foreach (DataRow row in _table.Rows) {
        if ((string)row["name"] != symbol.Name)
          continue;
        this.UpdateRow(row, sender as ITickCollection, "");
        break;
      }
      
      this.UpdateSymbolProgress();
    }
    #endregion

    #region private void _btnClose_Click(object sender, EventArgs e)
    private void _btnClose_Click(object sender, EventArgs e) {
      this.Close();
    }
    #endregion

    #region private void _btnImport_Click(object sender, EventArgs e)
    private void _btnImport_Click(object sender, EventArgs e) {
      Global.CommandManager.Import();
    }
    #endregion
  }
}