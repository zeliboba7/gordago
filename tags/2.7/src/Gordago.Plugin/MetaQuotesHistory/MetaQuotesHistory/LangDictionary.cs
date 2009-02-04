using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.PlugIn.MetaQuotesHistory {
  interface ILangDictionary {
    string MenuItem { get;}
    string SubMenuItemAdd { get;}
    string SubMenuItemRemove { get;}
    string SubMenuItemImport { get;}
    string SubMenuItemConvertToTicks { get; }

    string WarningNewTimeFrame { get;}
    string RestartProgram { get;}

    string WarningImportFileNotFound { get;}
    string ConvertComplete { get;}
  }

  class LangDictEnglish:ILangDictionary {

    public string MenuItem {
      get { return "MetaQuotes History"; }
    }

    public string SubMenuItemAdd {
      get { return "Set"; }
    }

    public string SubMenuItemRemove {
      get { return "Remove"; }
    }
  
    public string SubMenuItemImport {
      get { return "Import"; }
    }

    public string SubMenuItemConvertToTicks {
      get { return "Convert to ticks"; }
    }

    public string WarningNewTimeFrame {
      get { return "Warning!\nFor work of the module it is necessary {0} timeframe.\nTo add the necessary period?"; }
    }

    public string RestartProgram {
      get { return "Please, restart the program!"; }
    }

    public string WarningImportFileNotFound {
      get { return "Warning!\nThe MetaQuotes file of history not found."; }
    }

    public string ConvertComplete {
      get { return "Convert to ticks complete!\nPlease, restart the program!"; }
    }
  }

  class LangDictRussian:ILangDictionary {
    public string MenuItem {
      get { return "История MetaQuotes"; }
    }

    public string SubMenuItemAdd {
      get { return "Установить"; }
    }

    public string SubMenuItemRemove {
      get { return "Удалить"; }
    }

    public string SubMenuItemImport { 
      get { return "Импорт"; }
    }

    public string SubMenuItemConvertToTicks {
      get { return "Конвертировать в тиковую историю Forex Optimizer"; }
    }

    public string WarningNewTimeFrame {
      get { return "Предупреждение!\nДля работы модуля необходим период: {0}.\nДобавить этот период?"; }
    }

    public string RestartProgram {
      get { return "Пожалуйста, перезапустите программу!"; }
    }

    public string WarningImportFileNotFound {
      get { return "Предупреждение!\nФайлы истории MetaQuotes не найдены."; }
    }

    public string ConvertComplete {
      get { return "Конвертирование в тиковую историю закончено!\nПожалуйста, перезапустите программу!"; }
    }

  }
}
