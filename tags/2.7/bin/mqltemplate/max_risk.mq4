//+------------------------------------------------------------------+
//|                           Copyright 2005, Gordago Software Corp. |
//|                                          http://www.gordago.com/ |
//|                                                      version 2.0 |
//+------------------------------------------------------------------+

#property copyright "Copyright 2005, Gordago Software Corp."
#property link      "http://www.gordago.com"

#define MIN_STOPLOSS_POINT 10 
#define MIN_TAKEPROFIT_POINT 10 
#define MAGIC %magic_number%

extern double MaximumRisk = 10;
extern string sNameExpert = "Generate from Gordago";
extern int nAccount = %account_number%;
extern double dBuyStopLossPoint = %buy_stop_loss_point%;
extern double dSellStopLossPoint = %sell_stop_loss_point%;
extern double dBuyTakeProfitPoint = %buy_take_profit_point%;
extern double dSellTakeProfitPoint = %sell_take_profit_point%;
extern double dBuyTrailingStopPoint = %buy_trailing_stop_point%;
extern double dSellTrailingStopPoint = %sell_trailing_stop_point%;
extern int nSlippage = %slip_page%;
extern bool lFlagUseHourTrade = %flag_use_hour_trade%;
extern int nFromHourTrade = %from_hour_trade%;
extern int nToHourTrade = %to_hour_trade%;
extern bool lFlagUseSound = %flag_use_sound%;
extern string sSoundFileName = "%sound_file_name%";
extern color colorOpenBuy = Blue;
extern color colorCloseBuy = Aqua;
extern color colorOpenSell = Red;
extern color colorCloseSell = Aqua;

double dLots = %lots_size%; 

void deinit() {
   Comment("");
}

//+------------------------------------------------------------------+
//|                                                                  |
//+------------------------------------------------------------------+
int start(){
   dLots=NormalizeDouble(AccountFreeMargin()*MaximumRisk/50000,1);
   
   if (lFlagUseHourTrade){
      if (!(Hour()>=nFromHourTrade && Hour()<=nToHourTrade)) {
         Comment("Time for trade has not come else!");
         return(0);
      }
   }
   
   if(Bars < 100){
      Print("bars less than 100");
      return(0);
   }
   
   if (nAccount > 0 && nAccount != AccountNumber()){
      Comment("Trade on account :"+AccountNumber()+" FORBIDDEN!");
      return(0);
   }
   
   if((dBuyStopLossPoint > 0 && dBuyStopLossPoint < MIN_STOPLOSS_POINT) ||
      (dSellStopLossPoint > 0 && dSellStopLossPoint < MIN_STOPLOSS_POINT)){
      Print("StopLoss less than " + MIN_STOPLOSS_POINT);
      return(0);
   }
   if((dBuyTakeProfitPoint > 0 && dBuyTakeProfitPoint < MIN_TAKEPROFIT_POINT) ||
      (dSellTakeProfitPoint > 0 && dSellTakeProfitPoint < MIN_TAKEPROFIT_POINT)){
      Print("TakeProfit less than " + MIN_TAKEPROFIT_POINT);
      return(0);
   }

   %var_expression%

   if(AccountFreeMargin() < (1000*dLots)){
      Print("We have no money. Free Margin = " + AccountFreeMargin());
      return(0);
   }
   
   bool lFlagBuyOpen = false, lFlagSellOpen = false, lFlagBuyClose = false, lFlagSellClose = false;
   
   lFlagBuyOpen = %buy_open_expression%;
   lFlagSellOpen = %sell_open_expression%;
   lFlagBuyClose = %buy_close_expression%;
   lFlagSellClose = %sell_close_expression%;
   
   if (!ExistPositions()){

      if (lFlagBuyOpen){
         OpenBuy();
         return(0);
      }

      if (lFlagSellOpen){
         OpenSell();
         return(0);
      }
   }
   if (ExistPositions()){
      if(OrderType()==OP_BUY){
         if (lFlagBuyClose){
            bool flagCloseBuy = OrderClose(OrderTicket(), OrderLots(), Bid, nSlippage, colorCloseBuy); 
            if (flagCloseBuy && lFlagUseSound) 
               PlaySound(sSoundFileName); 
            return(0);
         }
      }
      if(OrderType()==OP_SELL){
         if (lFlagSellClose){
            bool flagCloseSell = OrderClose(OrderTicket(), OrderLots(), Ask, nSlippage, colorCloseSell); 
            if (flagCloseSell && lFlagUseSound) 
               PlaySound(sSoundFileName); 
            return(0);
         }
      }
   }
   
   if (dBuyTrailingStopPoint > 0 || dSellTrailingStopPoint > 0){
      
      for (int i=0; i<OrdersTotal(); i++) { 
         if (OrderSelect(i, SELECT_BY_POS, MODE_TRADES)) { 
            bool lMagic = true;
            if (MAGIC > 0 && OrderMagicNumber() != MAGIC)
               lMagic = false;
            
            if (OrderSymbol()==Symbol() && lMagic) { 
               if (OrderType()==OP_BUY && dBuyTrailingStopPoint > 0) { 
                  if (Bid-OrderOpenPrice() > dBuyTrailingStopPoint*Point) { 
                     if (OrderStopLoss()<Bid-dBuyTrailingStopPoint*Point) 
                        ModifyStopLoss(Bid-dBuyTrailingStopPoint*Point); 
                  } 
               } 
               if (OrderType()==OP_SELL) { 
                  if (OrderOpenPrice()-Ask>dSellTrailingStopPoint*Point) { 
                     if (OrderStopLoss()>Ask+dSellTrailingStopPoint*Point || OrderStopLoss()==0)  
                        ModifyStopLoss(Ask+dSellTrailingStopPoint*Point); 
                  } 
               } 
            } 
         } 
      } 
   }
   return (0);
}

bool ExistPositions() {
	for (int i=0; i<OrdersTotal(); i++) {
		if (OrderSelect(i, SELECT_BY_POS, MODE_TRADES)) {
         bool lMagic = true;
         
         if (MAGIC > 0 && OrderMagicNumber() != MAGIC)
            lMagic = false;

			if (OrderSymbol()==Symbol() && lMagic) {
				return(True);
			}
		} 
	} 
	return(false);
}

void ModifyStopLoss(double ldStopLoss) { 
   bool lFlagModify = OrderModify(OrderTicket(), OrderOpenPrice(), ldStopLoss, OrderTakeProfit(), 0, CLR_NONE); 
   if (lFlagModify && lFlagUseSound) 
      PlaySound(sSoundFileName); 
} 

void OpenBuy() { 
   double dStopLoss = 0, dTakeProfit = 0;

   if (dBuyStopLossPoint > 0)
      dStopLoss = Bid-dBuyStopLossPoint*Point;
   
   if (dBuyTakeProfitPoint > 0)
     dTakeProfit = Ask + dBuyTakeProfitPoint * Point; 
 
   
   int numorder = OrderSend(Symbol(), OP_BUY, dLots, Ask, nSlippage, dStopLoss, dTakeProfit, sNameExpert, MAGIC, 0, colorOpenBuy); 
   
   if (numorder > -1 && lFlagUseSound) 
      PlaySound(sSoundFileName);
} 

void OpenSell() { 
   double dStopLoss = 0, dTakeProfit = 0;
   
   if (dSellStopLossPoint > 0)
      dStopLoss = Ask+dSellStopLossPoint*Point;
   
   if (dSellTakeProfitPoint > 0)
      dTakeProfit = Bid-dSellTakeProfitPoint*Point;
   
   int numorder = OrderSend(Symbol(),OP_SELL, dLots, Bid, nSlippage, dStopLoss, dTakeProfit, sNameExpert, MAGIC, 0, colorOpenSell); 
   
   if (numorder > -1 && lFlagUseSound) 
      PlaySound(sSoundFileName); 
} 

