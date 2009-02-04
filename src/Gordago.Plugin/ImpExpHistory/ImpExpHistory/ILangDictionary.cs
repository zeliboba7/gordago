using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.PlugIn.ImpExpHistory {
  
  interface ILangDictionary {
    string MenuItem { get;}

    string WarningNewTimeFrame { get;}
    string RestartProgram { get;}
  }

  class LangDictEnglish : ILangDictionary {

    public string MenuItem {
      get { return "Import History"; }
    }

    public string WarningNewTimeFrame {
      get { return "Warning!\nFor work of the module it is necessary {0} timeframe.\nTo add the necessary period?"; }
    }

    public string RestartProgram {
      get { return "Please, restart the program!"; }
    }
  }

  class LangDictRussian : ILangDictionary {
    public string MenuItem {
      get { return "Импорт данных"; }
    }

    public string WarningNewTimeFrame {
      get { return "Предупреждение!\nДля работы модуля необходим период: {0}.\nДобавить этот период?"; }
    }

    public string RestartProgram {
      get { return "Пожалуйста, перезапустите программу!"; }
    }
  }
}
