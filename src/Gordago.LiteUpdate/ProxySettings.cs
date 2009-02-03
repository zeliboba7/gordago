/**
* @version $Id: ProxySettings.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Net;

  public class ProxySettings {

    private string _server, _userName, _userPassword;
    private int _port;

    private bool _enable=false;

    public ProxySettings() {
    }

    #region public bool Enable
    public bool Enable {
      get { return this._enable; }
      set { this._enable = value; }
    }
    #endregion

    #region public string Server
    public string Server {
      get { return this._server; }
      set { this._server = value; }
    }
    #endregion

    #region public int Port
    public int Port {
      get { return this._port; }
      set { this._port = value; }
    }
    #endregion

    #region public string UserName
    public string UserName {
      get { return this._userName; }
      set { this._userName = value; }
    }
    #endregion

    #region public string UserPassword
    public string UserPassword {
      get { return this._userPassword; }
      set { this._userPassword = value; }
    }
    #endregion

    #region public WebProxy GetWebProxy()
    public WebProxy GetWebProxy() {
      WebProxy proxyObject = null;
     
      if (this.Enable) {
        proxyObject = new WebProxy(this.Server, this.Port);
        proxyObject.Credentials = new NetworkCredential(this.UserName, this.UserPassword);
      }
      return proxyObject;
    }
    #endregion
  }
}
