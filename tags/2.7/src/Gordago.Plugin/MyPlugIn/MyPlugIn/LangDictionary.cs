using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.PlugIn {
  interface ILangDictionary {
    string MenuItem { get;}
    string SubMenuItemMyPlugInAdd { get;}
    string SubMenuItemMyPlugInRemove { get;}
    string SubMenuItemMyPlugInSettings { get;}

    string WarningNewTimeFrame { get;}
    string RestartProgram { get;}
  }

  class LangDictEnglish:ILangDictionary {

    public string MenuItem {
      get { return "My PlugIn"; }
    }

    public string SubMenuItemMyPlugInAdd {
      get { return "Set"; }
    }

    public string SubMenuItemMyPlugInRemove {
      get { return "Remove"; }
    }
  
    public string SubMenuItemMyPlugInSettings {
      get { return "Settings"; }
    }

    public string WarningNewTimeFrame {
      get { return "Warning!\nFor work of the module it is necessary {0} timeframe.\nTo add the necessary period?"; }
    }

    public string RestartProgram {
      get { return "Please, restart the program!"; }
    }

  }

  class LangDictRussian:ILangDictionary {
    public string MenuItem {
      get { return "Мой плагин"; }
    }

    public string SubMenuItemMyPlugInAdd {
      get { return "Установить"; }
    }

    public string SubMenuItemMyPlugInRemove {
      get { return "Удалить"; }
    }

    public string SubMenuItemMyPlugInSettings {
      get { return "Настройки"; }
    }

    public string WarningNewTimeFrame {
      get { return "Предупреждение!\nДля работы модуля необходим период: {0}.\nДобавить этот период?"; }
    }

    public string RestartProgram {
      get { return "Пожалуйста, перезапустите программу!"; }
    }

  }
}
