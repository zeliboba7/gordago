/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;

namespace Gordago.API {

  public delegate void BCMConnectionStatusHandler(BrokerConnectionStatus status);
  public delegate void BCMAccountsHandler(BrokerAccountsEventArgs be);
  public delegate void BCMOnlineRatesHandler(BrokerOnlineRatesEventArgs be);
  public delegate void BCMTradesHandler(BrokerTradesEventArgs be);
  public delegate void BCMOrdersHandler(BrokerOrdersEventArgs be);
  public delegate void BCMCommandStarting(BrokerCommand command);
  public delegate void BCMCommandStopping(BrokerCommand command, BrokerResult result);

	public interface IBrokerEvents {
    void BrokerConnectionStatusChanged(BrokerConnectionStatus status);
    void BrokerAccountsChanged(BrokerAccountsEventArgs be);
    void BrokerOrdersChanged(BrokerOrdersEventArgs be);
    void BrokerTradesChanged(BrokerTradesEventArgs be);
		void BrokerOnlineRatesChanged(BrokerOnlineRatesEventArgs be);
    void BrokerCommandStarting(BrokerCommand command);
    void BrokerCommandStopping(BrokerCommand command, BrokerResult result);
	}

  public interface IBrokerSymbolsEvents {
    void BrokerUpdateSymbolStarting(UpdateSymbolEventArgs se);
    void BrokerUpdateSymbolStopping(UpdateSymbolEventArgs se);
    void BrokerUpdateSymbolDownloadPart(UpdateSymbolEventArgs se);
  }
}
 