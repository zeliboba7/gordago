using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.PlugIn {
  interface ILangDictionary {
    string MenuItem { get;}
    string SubMenuItemAdd { get;}
    string SubMenuItemRemove { get;}
    string SubMenuItemSettings { get;}
    string SubMenuItemAbout { get;}
  }

  class LangDictEnglish:ILangDictionary {

    public string MenuItem {
      get { return "Clocks"; }
    }

    public string SubMenuItemAdd {
      get { return "Set"; }
    }

    public string SubMenuItemRemove {
      get { return "Remove"; }
    }

    public string SubMenuItemSettings {
      get { return "Settings"; }
    }

    public string SubMenuItemAbout {
      get { return "About..."; }
    }
  }

  class LangDictRussian:ILangDictionary {
    public string MenuItem {
      get { return "Часы"; }
    }

    public string SubMenuItemAdd {
      get { return "Установить"; }
    }

    public string SubMenuItemRemove {
      get { return "Удалить"; }
    }

    public string SubMenuItemSettings {
      get { return "Настройки"; }
    }

    public string SubMenuItemAbout {
      get { return "О модуле..."; }
    }

  }
}
