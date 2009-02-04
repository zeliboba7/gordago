/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Gordago.PlugIn {

  /// <summary>
  /// Абстрактный класс для построения подключаемого к программе модуля
  /// </summary>
  public abstract class PlugInModule {

    /// <summary>
    /// Загрузка модуля
    /// </summary>
    /// <param name="mainForm">Интерфейс главного окна</param>
    /// <returns>true - модуль загружен успешно, false - модуль не будет загружен</returns>
    public abstract bool OnLoad(IMainForm mainForm);

    /// <summary>
    /// Закрытие программы
    /// </summary>
    public virtual void OnDestroy() {}
  }

  /// <summary>
  /// Интерфейс для построения подключаемого к программе модуля
  /// </summary>
  public interface IPlugIn {
    /// <summary>
    /// Меню
    /// </summary>
    ToolStripMenuItem MenuItem { get;}

    /// <summary>
    /// Панель инструментов
    /// </summary>
    ToolStrip Toolbar { get; }
  }
}
