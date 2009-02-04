using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.PlugIn.MetaQuotesHistory {
  enum MQHErrors {
    /// <summary>
    /// Нет ошибок
    /// </summary>
    NONE,
    
    /// <summary>
    /// В программе нет необходимого для работы ТаймФрейма
    /// </summary>
    TimeFrameNotFound,
    
    /// <summary>
    /// Для дальнейшей работы, необходимо перезапустить программу
    /// </summary>
    RestartProgram

  }
}
