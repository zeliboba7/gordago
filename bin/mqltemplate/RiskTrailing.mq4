//+------------------------------------------------------------------+
//|                      Copyright 2005-2007, Gordago Software Corp. |
//|                                          http://www.gordago.com/ |
//|                                                      version 1.2 |
//+------------------------------------------------------------------+

#property copyright "Copyright 2005-2007, Gordago Software Corp."
#property link      "http://www.gordago.com"

#define MIN_STOPLOSS_POINT 10
#define MIN_TAKEPROFIT_POINT 10 
#define MAGIC %magic_number%

extern bool SellUseRiskTrailing = true;
extern double SellFloorAmount1 = 15;
extern double SellRiskPercent1 = 60;
extern double SellFloorAmount2 = 60;
extern double SellRiskPercent2 = 30;
extern double SellFloorAmount3 = 90;
extern double SellRiskPercent3 = 10;
double sellRTMax;
int sellRiskTrailSwitcher = 0;


extern bool BuyUseRiskTrailing = true;
extern double BuyFloorAmount1 = 15;
extern double BuyRiskPercent1 = 60;
extern double BuyFloorAmount2 = 60;
extern double BuyRiskPercent2 = 30;
extern double BuyFloorAmount3 = 90;
extern double BuyRiskPercent3 = 10;
double buyRTMax;
int buyRiskTrailSwitcher = 0;


extern int nAccount = %account_number%;

extern double dBuyStopLossPoint = %buy_stop_loss_point%;
extern double dBuyTakeProfitPoint = %buy_take_profit_point%;
extern double dBuyTrailingStopPoint = %buy_trailing_stop_point%;

extern double dSellStopLossPoint = %sell_stop_loss_point%;
extern double dSellTakeProfitPoint = %sell_take_profit_point%;
extern double dSellTrailingStopPoint = %sell_trailing_stop_point%;

extern double dLots = %lots_size%; 
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

string sNameExpert = "Generate from Gordago";

void deinit() {}

//+------------------------------------------------------------------+
//|                                                                  |
//+------------------------------------------------------------------+
int start(){
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
         if (CloseFromBuyRiskTrailing()) lFlagBuyClose = true;
      
         if (lFlagBuyClose){
            bool flagCloseBuy = OrderClose(OrderTicket(), OrderLots(), Bid, nSlippage, colorCloseBuy); 
            if (flagCloseBuy){
               buyRTMax = 0;
               buyRiskTrailSwitcher = 0;
               if (lFlagUseSound) PlaySound(sSoundFileName); 
            }
            return(0);
         }
      }
      if(OrderType()==OP_SELL){
         if (CloseFromSellRiskTrailing()) lFlagSellClose = true;
      
         if (lFlagSellClose){
            bool flagCloseSell = OrderClose(OrderTicket(), OrderLots(), Ask, nSlippage, colorCloseSell); 
            if (flagCloseSell){
               sellRTMax = 0;
               sellRiskTrailSwitcher = 0;
               if (lFlagUseSound) PlaySound(sSoundFileName); 
            }
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
     dTakeProfit = Bid + dBuyTakeProfitPoint * Point; 
   
   int numorder = OrderSend(Symbol(), OP_BUY, dLots, Ask, nSlippage, dStopLoss, dTakeProfit, sNameExpert, MAGIC, 0, colorOpenBuy); 
   
   if (numorder > -1 && lFlagUseSound) 
      PlaySound(sSoundFileName);
} 

void OpenSell() { 
   double dStopLoss = 0, dTakeProfit = 0;
   
   if (dSellStopLossPoint > 0)
      dStopLoss = Ask+dSellStopLossPoint*Point;
   
   if (dSellTakeProfitPoint > 0)
      dTakeProfit = Ask-dSellTakeProfitPoint*Point;
   
   int numorder = OrderSend(Symbol(),OP_SELL, dLots, Bid, nSlippage, dStopLoss, dTakeProfit, sNameExpert, MAGIC, 0, colorOpenSell); 
   
   if (numorder > -1 && lFlagUseSound) 
      PlaySound(sSoundFileName); 
} 

void ViCo(string str){Print(str);}

bool CloseFromBuyRiskTrailing(){
   double profit = (Bid-OrderOpenPrice())/Point;

   if (profit > buyRTMax && BuyUseRiskTrailing){
      buyRTMax = profit;
   
      if (buyRTMax >= BuyFloorAmount3){
         if (buyRiskTrailSwitcher != 3)
            ViCo("Enable BuyRiskTrailing 3: " + buyRTMax + " .");
         buyRiskTrailSwitcher = 3;
      }else if (buyRTMax >= BuyFloorAmount2){
         if (buyRiskTrailSwitcher != 2)
            ViCo("Enable BuyRiskTrailing 2: " + buyRTMax + " .");
         buyRiskTrailSwitcher = 2;
      }else if (buyRTMax >= BuyFloorAmount1){
         if (buyRiskTrailSwitcher != 1)
            ViCo("Enable BuyRiskTrailing 1: " + buyRTMax + " .");
         buyRiskTrailSwitcher = 1;
      }
   }
         
   switch(buyRiskTrailSwitcher){
      case 3:
         if (profit <= buyRTMax-buyRTMax*(BuyRiskPercent3/100)){
            ViCo("Close position from BuyRiskTrailing 3: Profit=" + profit + ", Max="+buyRTMax+".");
            return(true);
         }
         break;
      case 2:
         if (profit <= buyRTMax-buyRTMax*(BuyRiskPercent2/100)){
            ViCo("Close position from BuyRiskTrailing 2: Profit=" + profit + ", Max="+buyRTMax+".");
            return (true);
         }
         break;
      case 1:
         if (profit <= buyRTMax-buyRTMax*(BuyRiskPercent1/100)){
            ViCo("Close position from BuyRiskTrailing 1: Profit=" + profit + ", Max="+buyRTMax+".");
            return (true);
         }
         break;
   }
   return (false);
}

bool CloseFromSellRiskTrailing(){
   double profit = (OrderOpenPrice()-Ask)/Point;

   if (profit > sellRTMax && SellUseRiskTrailing){

      sellRTMax = profit;
   
      if (sellRTMax >= SellFloorAmount3){
         if (sellRiskTrailSwitcher != 3)
            ViCo("Enable SellRiskTrailing 3: " + sellRTMax + " .");
         sellRiskTrailSwitcher = 3;
      }else if (sellRTMax >= SellFloorAmount2){
         if (sellRiskTrailSwitcher != 2)
            ViCo("Enable SellRiskTrailing 2: " + sellRTMax + " .");
         sellRiskTrailSwitcher = 2;
      }else if (sellRTMax >= SellFloorAmount1){
         if (sellRiskTrailSwitcher != 1)
            ViCo("Enable SellRiskTrailing 1: " + sellRTMax + " .");
         sellRiskTrailSwitcher = 1;
      }
   }
         
   switch(sellRiskTrailSwitcher){
      case 3:
         if (profit <= sellRTMax-sellRTMax*(SellRiskPercent3/100)){
            ViCo("Close position from SellRiskTrailing 3: Profit=" + profit + ", Max="+sellRTMax+".");
            return(true);
         }
         break;
      case 2:
         if (profit <= sellRTMax-sellRTMax*(SellRiskPercent2/100)){
            ViCo("Close position from SellRiskTrailing 2: Profit=" + profit + ", Max="+sellRTMax+".");
            return (true);
         }
         break;
      case 1:
         if (profit <= sellRTMax-sellRTMax*(SellRiskPercent1/100)){
            ViCo("Close position from SellRiskTrailing 1: Profit=" + profit + ", Max="+sellRTMax+".");
            return (true);
         }
         break;
   }
   return (false);
}