/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago {

  #region public interface ISymbolList
  /// <summary>
  /// Список финансовых инструментов.
  /// </summary>
  public interface ISymbolList {

    /// <summary>
    /// Получить финансовый инструмент из массива
    /// </summary>
    /// <param name="index">Индекс в массиве</param>
    /// <returns>Финансовый инструмент</returns>
    ISymbol this[int index]{ get;}
    
    /// <summary>
    /// Количество финансовых инструментов
    /// </summary>
    int Count { get;}
    
    /// <summary>
    /// Получить финансовый инструмент
    /// </summary>
    /// <param name="symbolname">Наименование финансового инструмента</param>
    /// <returns>Финансовый инструмент. null - финансовый инструмент отсутствует</returns>
    ISymbol GetSymbol(string symbolname);

    ISymbol Add(string name, int decimalDigits);
    ISymbol Add(ISymbol symbol);
  }
  #endregion

  #region public interface ISymbol
  /// <summary>
  /// Финансовый инструмент
  /// </summary>
  public interface ISymbol {
    /// <summary>
    /// Наименование инструмента
    /// </summary>
    string Name { get;}
    /// <summary>
    /// Один пункт, например EURUSD: Point = 0.0001
    /// </summary>
    float Point { get;}
    /// <summary>
    /// Кол-во знаков 
    /// </summary>
    int DecimalDigits { get;}
    /// <summary>
    /// Тики
    /// </summary>
    ITickList Ticks { get;set;}

    /// <summary>
    /// Последняя цена Бид
    /// </summary>
    // float Bid { get;}

    /// <summary>
    /// Последняя цена Аск
    /// </summary>
    // float Ask { get;}
  }
  #endregion

  #region public interface ITickList
  /// <summary>
  /// Коллекция тиков
  /// </summary>
  public interface ITickList {
    
    /// <summary>
    /// Кол-во тиков
    /// </summary>
    int Count { get;}

    /// <summary>
    /// Получить тик из коллекции
    /// </summary>
    /// <param name="index">Номер тика в коллекции</param>
    /// <returns>Возвращает тик</returns>
    Tick this[int index] { get;}
    /// <summary>
    /// Последный тик в коллекции
    /// </summary>
    Tick Current { get;}

    /// <summary>
    /// Время первого тика в коллекции
    /// </summary>
    DateTime TimeFrom { get;}

    /// <summary>
    /// Время последнего тика в коллекции
    /// </summary>
    DateTime TimeTo { get;}

    /// <summary>
    /// Массив коллекции баров
    /// </summary>
    IBarList[] BarLists { get;}

    /// <summary>
    /// Получить коллекцию баров
    /// </summary>
    /// <param name="second">Период в секундах</param>
    /// <returns>Коллекция баров</returns>
    IBarList GetBarList(int second);

    /// <summary>
    /// Добавить тик в коллекцию
    /// </summary>
    /// <param name="tick">Тик</param>
    void Add(Tick tick);
  }
  #endregion

  public class TickManagerEventArgs : EventArgs {
    private int _current;
    private int _total;
    private TickManagerStatus _status;

    public TickManagerEventArgs(TickManagerStatus status, int current, int total) {
      _current = current;
      _total = total;
      _status = status;
    }

    #region public int Current
    public int Current {
      get { return this._current; }
    }
    #endregion

    #region public int Total
    public int Total {
      get { return this._total; }
    }
    #endregion

    #region public TickManagerStatus Status
    public TickManagerStatus Status {
      get { return this._status; }
    }
    #endregion
  }

  public delegate void TickManagerEventHandler(object sender, TickManagerEventArgs tme);

  //public delegate void TickManagerHandler();
  //public delegate void TickManagerProcessHandler(int current, int total);

  #region public interface ITickManager
  /// <summary>
  /// Управление тиками на физическом уровне.
  /// </summary>
  public interface ITickManager {
    /// <summary>
    /// Событие окончания процесса кеширования данных
    /// </summary>
    //event TickManagerHandler DataCachingStopping;
    //event TickManagerProcessHandler DataCachingProcess;

    event TickManagerEventHandler DataCachingChanged;

    /// <summary>
    /// Статус определяющий в каком состояние находиться коллекция
    /// </summary>
    TickManagerStatus Status { get;}

    /// <summary>
    /// true - Данные не нуждаються в кеширование
    /// </summary>
    bool IsDataCaching { get;}

    /// <summary>
    /// Использовать события кеширования данных
    /// </summary>
    //bool UseDataCachingEvents { get;set;}

    TickFileInfo InfoHistory { get;}
    TickFileInfo InfoCache { get;}

    int GetPosition(DateTime fdtm);

    /// <summary>
    /// Возвращает позицию на начало дня
    /// </summary>
    /// <param name="fdtm"></param>
    /// <returns></returns>
    int GetPositionFromMap(DateTime fdtm);

    /// <summary>
    /// Есть ли период истории в текущей архивной истории
    /// </summary>
    /// <param name="fromdtm"></param>
    /// <param name="todtm"></param>
    /// <param name="cnttick"></param>
    /// <returns></returns>
    bool CheckPeriodInMap(DateTime fromdtm, DateTime todtm, int cnttick);

    /// <summary>
    /// Выравнивание массивов по дате, кэширование данных,
    /// чтение тиков, преобразование в таймфремы, 
    /// сохранение таймфреймов, сохранение мапинга по файлам
    /// </summary>
    /// <returns>True - процесс будет запущень на выполение</returns>
    void DataCaching();

    void DataCachingMethod();

    void Update(TickCollection ticks);
    void Update(TickCollection ticks, bool isCacheBuffer);
    void Update(TickFileInfo tfi);
    void Update(TickFileInfo tfi, bool isCacheBuffer);
  }
  #endregion

  public interface IBarList {
    int Count { get;}
    Bar this[int index] { get;}
    Bar Current { get;}
    TimeFrame TimeFrame { get;}
    DateTime TimeFrom { get;}
    DateTime TimeTo { get;}

    int GetBarIndex(DateTime time);
#if DEBUG
    // int RealIndex(int index);
#endif
  }

}
