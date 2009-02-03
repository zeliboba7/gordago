/**
* @version $Id: Program.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.IOTicks
{
  using System;
  using System.Collections.Generic;
  using System.Windows.Forms;
  using Gordago.Core;
  using System.Diagnostics;
  using System.IO;
    using Gordago.Trader;

  static class Program {

    //static TraceSource _TraceSource ;
    static TextWriterTraceListener _TextWriterTraceListener;

    [STAThread]
    static void Main() {

      DirectoryInfo debugDirectory = new DirectoryInfo(Path.Combine(Global.Setup.ApplicationDirectory.FullName, "Debug"));
      
      if (debugDirectory.Exists) {
        FileInfo traceFile = new FileInfo(Path.Combine(debugDirectory.FullName,
          string.Format("{0}.log", DateTime.Now.ToString("yyyyMMddhhmm"))));

        Stream s = traceFile.Create();
        _TextWriterTraceListener = new TextWriterTraceListener(s);
        _TextWriterTraceListener.TraceOutputOptions = TraceOptions.None;

        Trace.Listeners.Add(_TextWriterTraceListener);
        Trace.AutoFlush = true;
      }

      Trace.TraceInformation("Application START: {0}", DateTime.Now);

      Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
      AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

      System.IO.FileInfo file = new System.IO.FileInfo(System.IO.Path.Combine(Global.Setup.ApplicationDirectory.FullName, "Gordago.IOTicks.Properties.xml"));
      
      try {
        Global.Properties.Load(file);
      } catch { }

      TimeFrameManager.TimeFrames.RemoveAllTimeFrame();

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new MainForm());

      try {
        Global.Properties.Save(file);
      } catch { }
      Trace.TraceInformation("Application STOP: {0}", DateTime.Now);
      TraceFlush();
    }

    #region private static void TraceFlush()
    private static void TraceFlush() {
      if (_TextWriterTraceListener == null)
        return;
      Trace.Flush();
      _TextWriterTraceListener = null;
    }
    #endregion

    static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e) {
      AbortApplication(e.Exception);
    }

    static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
      AbortApplication(e.ExceptionObject as Exception);
    }

    #region static void AbortApplication(Exception e)
    static void AbortApplication(Exception e) {

      if (e != null) 
        Trace.TraceError("Critical Error\n{0}", e.ToString());
       else 
        Trace.TraceError("Critical Error!");
      
      TraceFlush();
      Application.Exit();
    }
    #endregion
  }
}