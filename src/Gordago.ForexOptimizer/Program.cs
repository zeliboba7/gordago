/**
* @version $Id: Program.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.FO
{
  using System;
  using System.Collections.Generic;
  using System.Windows.Forms;
  using Gordago.Core;

  static class Program {

    [System.Runtime.InteropServices.DllImport("User32.dll")]
    static extern int SetForegroundWindow(IntPtr hWnd);

    const int SW_MAXIMIZED = 3;
    [System.Runtime.InteropServices.DllImport("User32.dll")]
    static extern int ShowWindow(IntPtr hWnd, Int32 Mode);

    [STAThread]
    static void Main() {

      #region Mutex
      bool createdNew;
      System.Threading.Mutex mutex = new System.Threading.Mutex(false, "Gordago Mutex", out createdNew);
      if (!createdNew) {
        //получаем имя нашего процесса (название файла без расширения '.exe')
        string processName = System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName;
        processName = processName.Substring(0, processName.IndexOf(".exe"));

        System.Diagnostics.Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();

        System.Diagnostics.Process[] processList = System.Diagnostics.Process.GetProcessesByName(processName);

        //перебираем все процессы с искомым именем
        foreach (System.Diagnostics.Process process in processList) {
          //текущий экземпляр нас не интересует
          if (process.Id == currentProcess.Id)
            continue;

          //могут быть разные приложения с одинаковым именем
          //исполняемого файла. Проверяем что-бы это был 'наш' файл
          if (process.MainModule.FileName != currentProcess.MainModule.FileName)
            continue;

          //Активизируем основное окно приложения
          SetForegroundWindow(process.MainWindowHandle);
          ShowWindow(process.MainWindowHandle, SW_MAXIMIZED);
          return;
        }
      }
      #endregion

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      SplashForm splashForm = null;
      try {
        splashForm = new SplashForm();
        Application.Run(splashForm);

        if (!splashForm.StartIDE)
          Environment.Exit(-1);
      } finally {
        if (splashForm != null)
          splashForm.Dispose();
        splashForm = null;
      }

      Application.Run(new MainForm());

      try {
        Global.Properties.Save(Global.Setup.AppConfigFile);
        Global.Languages.BuildComplete();
      } catch { }
    }
  }
}