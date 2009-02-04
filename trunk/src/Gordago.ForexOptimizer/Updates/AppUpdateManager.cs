/**
* @version $Id: AppUpdateManager.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.FO.Updates
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using Gordago.LiteUpdate;
  using Gordago.Core;

  class AppUpdateManager : UpdateManager {

    private bool _autoCheckForUpdates = true;

    public AppUpdateManager() {
      /* Все настройки менеджера обновлений находятся в Gordago.LiteUpdate.Configure */

      /* Путь к скриптам менеджера обновления */
#if DEBUG
      LiteUpdate.Configure.UpdateUrl = "http://gordagocom/liteupdate";
#else
      LiteUpdate.Configure.UpdateUrl = "http://gordago.com/liteupdate";
#endif
      /* Идентификатор приложения */
      LiteUpdate.Configure.ProductId = "ForexOptimizer";

      /* Корневая директория приложения */
      LiteUpdate.Configure.ApplicationDirectory = Global.Setup.AppRootDirectory;

      /* Рабочая директория менеджера обновлений */
      LiteUpdate.Configure.UpdateDirectory = Global.Setup.UpdateDirectory;
    }

    #region public bool AutoCheckForUpdates
    public bool AutoCheckForUpdates {
      get { return _autoCheckForUpdates; }
      set { _autoCheckForUpdates = value; }
    }
    #endregion

    #region public void LoadSettings()
    public void LoadSettings() {

      /* Загрузка настроек подключения к серверу */
      EasyPropertiesNode ps = Global.Properties["Updates"];
      _autoCheckForUpdates = ps.GetValue<bool>("Auto", true);

      ps = Global.Properties["Updates"]["Proxy"];
      ProxySettings proxy = LiteUpdate.Configure.Proxy;
      proxy.Enable = ps.GetValue<bool>("Enable", false);
      proxy.Server = ps.GetValue<string>("Server", "");
      proxy.Port = ps.GetValue<int>("Port", 0);
      proxy.UserName = ps.GetValue<string>("UserName", "");
      proxy.UserPassword = ps.GetValue<string>("UserPassword", "");

    }
    #endregion

    #region public void SaveSettings()
    public void SaveSettings() {
      EasyPropertiesNode ps = Global.Properties["Updates"];
      ps.SetValue<bool>("Auto", _autoCheckForUpdates);

      ps = Global.Properties["Updates"]["Proxy"];
      ProxySettings proxy = Configure.Proxy;
      ps.SetValue<bool>("Enable", proxy.Enable);
      ps.SetValue<string>("Server", proxy.Server);
      ps.SetValue<int>("Port", proxy.Port);
      ps.SetValue<string>("UserName", proxy.UserName);
      ps.SetValue<string>("UserPassword", proxy.UserPassword);
    }
    #endregion

    #region public void LoadUpdateManager()
    public void LoadUpdateManager() {
      this.Load();
      this.LoadSettings();
      /* Автоматическая проверка обновлений */
      if (this.AutoCheckForUpdates)
        this.CheckForUpdates();
    }
    #endregion

    #region protected override void OnStopCheckForUpdates(EventArgs e)
    protected override void OnStopCheckForUpdates(EventArgs e) {
      /* Событие на окончание проверки обновлений */
      base.OnStopCheckForUpdates(e);

      if (!Global.UpdateManager.IsOldVersion)
        return;

      /* Если есть обновление, открываем форму */
      this.ShowUpdateForm();
    }
    #endregion

    #region public void ShowUpdateForm()
    private delegate void ShowUpdateFormHandler();
    public void ShowUpdateForm() {
      if (Global.MainForm.InvokeRequired) {
        Global.MainForm.Invoke(new ShowUpdateFormHandler(ShowUpdateForm));
      } else {
        UpdateForm form = new UpdateForm(this);
        form.ShowDialog();
      }
    }
    #endregion
  }
}
