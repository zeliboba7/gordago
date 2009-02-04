/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Net;

namespace Gordago.WebUpdate {
  public class ProxySetting {
		private string _proxyserver, _proxyuser, _proxypass;
		private int _proxyport;
		private bool _useproxy, _usefromie;


		public ProxySetting() {
		}

		#region public bool ProxyEnable
		public bool ProxyEnable{
			get{return this._useproxy;}
			set{this._useproxy = value;}
		}
		#endregion

		#region public string ProxyServer
		public string ProxyServer{
			get{return this._proxyserver;}
			set{this._proxyserver = value;}
		}
		#endregion

		#region public int ProxyPort
		public int ProxyPort{
			get{return this._proxyport;}
			set{this._proxyport = value;}
		}
		#endregion

		#region public string ProxyUserName
		public string ProxyUserName{
			get{return this._proxyuser;}
			set{this._proxyuser = value;}
		}
		#endregion

		#region public string ProxyPass
		public string ProxyPass{
			get{return this._proxypass;}
			set{this._proxypass = value;}
		}
		#endregion

		#region public bool ProxySettingFromIE
		public bool ProxySettingFromIE{
			get{return this._usefromie;}
			set{this._usefromie = value;}
		}
		#endregion

    #region public WebProxy GetWebProxy()
    public WebProxy GetWebProxy() {
      WebProxy proxyObject = null;
      //if (this.ProxySettingFromIE){
      //  proxyObject = WebProxy.GetDefaultProxy();
      //  if (proxyObject != null && proxyObject.Address != null) 
      //    proxyObject.Credentials = CredentialCache.DefaultCredentials;
      //}else
      if(this.ProxyEnable && this.ProxyUserName.Length > 0) {
        proxyObject = new WebProxy(this.ProxyServer, this.ProxyPort);
        proxyObject.Credentials = new NetworkCredential(this.ProxyUserName, this.ProxyPass);
      }
      return proxyObject;
    }
    #endregion
  }
}
