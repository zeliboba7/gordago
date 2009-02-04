/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;

namespace Gordago {
  public class GSOStart {

    internal static LoadingForm LForm;

    [System.Runtime.InteropServices.DllImport("User32.dll")]
    static extern int SetForegroundWindow(IntPtr hWnd);

    const int SW_MAXIMIZED = 3;
    [System.Runtime.InteropServices.DllImport("User32.dll")]
    static extern int ShowWindow(IntPtr hWnd, Int32 Mode);


    [STAThread]
    static void Main(string[] args) {

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
      System.Windows.Forms.Application.EnableVisualStyles();
      System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);


#if !DEBUG
      LForm = new LoadingForm();
      LForm.Show();
      LForm.Refresh();
      System.Threading.Thread.Sleep(1000);
#endif
      try {
        GordagoMain.Start(args);
      } catch (Exception e) {
        if (LForm != null)
          LForm.Visible = false;
        string errfile = System.Windows.Forms.Application.StartupPath + "\\error.log";
        if (System.IO.File.Exists(errfile))
          System.IO.File.Delete(errfile);
        System.IO.StreamWriter sw = System.IO.File.CreateText(errfile);
        sw.WriteLine(e.Message);
        sw.WriteLine(e.Source);
        sw.WriteLine(e.StackTrace);
        sw.Flush();
        sw.Close();
      }
    }
  }
}
