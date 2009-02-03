using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace WindowsApplication1 {
  static class Program {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main() {

      /* * * * * * * * * * * * * * * * * * * * * * * * * * * * *
      /* Настройка менеджера обновлений LiteUpdate 
      /* * * * * * * * * * * * * * * * * * * * * * * * * * * * *
      /* Внимание!!! Все настройки в данном примере выполнены на 
      /* http://gordago.ru
      /* * * * * * * * * * * * * * * * * * * * * * * * * * * * */
          
      /* Путь к скриптам менеджера обновления. */
      Gordago.LiteUpdate.Configure.UpdateUrl = "http://gordago.ru/liteupdate";

      /* Идентификатор приложения 
       * Необходим для того, чтобы идентифицировать приложение на стороне 
       * сервера.
       * В данном случае LiteUpdateSample, соответственно путь к файлам
       * пакета обновления данного приложения должен быть 
       * http://gordago.com/liteupdate/files/LiteUpdateSample
       */
      Gordago.LiteUpdate.Configure.ProductId = "LiteUpdateSample";

      /* Наименование продукта */
      Gordago.LiteUpdate.Configure.ProductName = "LiteUpdate Sample";

      /* Корневая директория приложения */
      DirectoryInfo appdir = 
        new DirectoryInfo(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName);
      Gordago.LiteUpdate.Configure.ApplicationDirectory = appdir;

      /* Рабочая директория менеджера обновлений. */
      DirectoryInfo updateDir = new DirectoryInfo(Path.Combine(appdir.FullName, "update"));
      Gordago.LiteUpdate.Configure.UpdateDirectory = updateDir;


      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new Form1());
    }
  }
}