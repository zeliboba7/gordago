/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using
using System;
using System.Collections;
using Gordago.Docs;
using System.IO;
using Application = System.Windows.Forms.Application;
using Gordago.Analysis;
using Cursit.Applic.AConfig;

using Gordago.Analysis.Chart;
using System.Drawing;

using Gordago.API;
using System.Threading;
#endregion

namespace Gordago {
	
	class GordagoMain {

		private static IndicatorManager _indman;
		private static ISymbolList _symbolengine, _virtualSymbolManager = null;
		private static DocProviderIndic[] _provMQL4;

		private static ChartStyleManager _csmanager;

		internal static Gordago.WebUpdate.UpdateEngine UpdateEngine = null;

		public const string MessageCaption = "Gordago Forex Optimizer";

		private static string _lang = "eng";
		private static Color _backColor = Color.FromArgb(148, 191, 220);

    private static bool _virtualProgramMode = false;

		#region public static string Lang
		public static string Lang{
			get{return _lang;}
		}
		#endregion

		public static int CountEvalutionDay = 1;
    internal static MainForm _mainform;
    internal static bool IsCloseProgram = false;

    internal const string TIMEFRAMES_INIT_DATA = "60|300|600|900|1800|3600|14400|86400|604800|2592000";

		internal static void Start(string[] args){

			Cursit.Utils.FileEngine.DeleteDir(System.Windows.Forms.Application.StartupPath + "\\" + Gordago.WebUpdate.UpdateEngine.TEMP_DIR_OLD_FILES);

			Config.Initialize("Gordago", "StockOptimizer TT");
			Config.LoadUsers();
			Config.Users["Path"].SetValue(System.Windows.Forms.Application.StartupPath);
			string path = System.Windows.Forms.Application.StartupPath;

			_csmanager = new ChartStyleManager();

			#region Loading Images
			GordagoImages.Images.CreateImage(path + "\\resources\\i.gif", "indic", "iibox");
			GordagoImages.Images.CreateImage(path + "\\resources\\f.gif", "func", "iibox");

			GordagoImages.Images.CreateImage(path + "\\resources\\b.gif", "buy", "strg");
			GordagoImages.Images.CreateImage(path + "\\resources\\bc.gif", "buyc", "strg");
			GordagoImages.Images.CreateImage(path + "\\resources\\be.gif", "buyExit", "strg");
			GordagoImages.Images.CreateImage(path + "\\resources\\s.gif", "sell", "strg");
			GordagoImages.Images.CreateImage(path + "\\resources\\sc.gif", "sellc", "strg");
			GordagoImages.Images.CreateImage(path + "\\resources\\se.gif", "sellExit", "strg");
			GordagoImages.Images.CreateImage(path + "\\resources\\ok.gif", "ok", "strg");
			GordagoImages.Images.CreateImage(path + "\\resources\\no.gif", "no", "strg");

			GordagoImages.Images.CreateImage(path + "\\resources\\start.gif", "start", "tester");
			GordagoImages.Images.CreateImage(path + "\\resources\\stop.gif", "stop", "tester");
			GordagoImages.Images.CreateImage(path + "\\resources\\pause.gif", "pause", "tester");

			#endregion

			#region Language
			string lng = Config.Users["Language", "English"];
			string lngfile = "";
			switch (lng){
				case "Russian":
					lngfile = "russian.lng";
					_lang = "rus";
          try {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ru-RU", false);
          } catch { }
					break;
				case "English":
					lngfile = "english.lng";
					_lang = "eng";
					break;
			}

			Language.LanguageData lngdata = new Language.LanguageData();
			lngdata.Load(path + "\\language\\"+lngfile);
			Language.Dictionary.Init(lngdata);

			Language.Dictionary.LanguageId = Lang;

			Cursit.Applic.APropGrid.PropGridFormPeriod.LngCancelText = Language.Dictionary.GetString(9,7);
			Cursit.Applic.APropGrid.PropGridFormPeriod.LngBeginText = Language.Dictionary.GetString(7,3,"От:");
			Cursit.Applic.APropGrid.PropGridFormPeriod.LngEndText = Language.Dictionary.GetString(7,4,"До:");
			Cursit.Applic.APropGrid.PropGridFormPeriod.LngStepText = Language.Dictionary.GetString(7,18,"Шаг");
			#endregion
      
      TimeFrame[] tfs = LoadTimeFrames();
      TimeFrameManager.TimeFrames.RemoveAllTimeFrame();
      foreach(TimeFrame tf in tfs) {
        TimeFrameManager.TimeFrames.AddTimeFrame(tf);
      }
      int tfcount = TimeFrameManager.TimeFrames.Count;

			string pathhistory = System.Windows.Forms.Application.StartupPath + "\\history";
			string pathindic = System.Windows.Forms.Application.StartupPath + "\\indicators";
			_symbolengine = new SymbolManager(pathhistory, pathhistory + "\\cache");
			_indman = new IndicatorManager(pathindic);

			ArrayList alprovmt4 = new ArrayList();
			string[] provfiles = System.IO.Directory.GetFiles(pathindic + "\\mt4prov", "*.xml");
			foreach (string provfile in provfiles){
				alprovmt4.AddRange(DocProvider.LoadFromXML(provfile));
			}
			_provMQL4 = (DocProviderIndic[])alprovmt4.ToArray(typeof(DocProviderIndic));
      _mainform = new MainForm();

      try {
        if(args != null && args.Length > 0) {
          string filegso = args[0];
          if(System.IO.File.Exists(filegso)) {
            _mainform.StrategyManager.OpenFromFile(filegso);
          }
        }
      } catch {}

      Application.Run(_mainform);
      IsCloseProgram = true;

      if(tfcount != TimeFrameManager.TimeFrames.Count) {
        string[] sa = new string[TimeFrameManager.TimeFrames.Count];
        for(int i = 0; i < sa.Length; i++) {
          TimeFrame tf = TimeFrameManager.TimeFrames[i];
          sa[i] = Convert.ToString(tf.Second);
        }
        string timeframesdata = string.Join("|", sa);
        Config.Users["Symbol"]["TimeFrames"].SetValue(timeframesdata);
      }

			Config.SaveUsers();

			if (UpdateEngine != null){
				try{
					//Gordago.API.APITrader.UnRegisterLibrary(Application.StartupPath);
					UpdateEngine.ApplyUpdate();
				}catch(Exception e){
					System.Windows.Forms.MessageBox.Show("Update application error: " + e.Message);
				}
			}
    }

    #region internal static MainForm MainForm
    internal static MainForm MainForm {
      get { return _mainform; }
    }
    #endregion

		#region internal static IndicatorManager IndicatorManager
		internal static IndicatorManager IndicatorManager{
			get{return _indman;}
		}
		#endregion

    #region internal static ISymbolList SymbolEngine
    internal static ISymbolList SymbolEngine{
			get{
        if (_virtualProgramMode)
          return _virtualSymbolManager;
        return _symbolengine;
      }
    }
    #endregion

    #region internal static ISymbolList FSymbolManager
    internal static ISymbolList FSymbolManager {
      get { return _symbolengine; }
    }
    #endregion

    #region internal static ISymbolList VSymbolManager
    internal static ISymbolList VSymbolManager {
      get { return _virtualSymbolManager; }
    }
    #endregion

    #region internal static bool VirtualProgrammMode
    internal static bool VirtualProgrammMode {
      get { return _virtualProgramMode; }
    }
    #endregion

    #region internal static DocProviderIndic[] ProviderMQL4
    internal static DocProviderIndic[] ProviderMQL4{
			get{return _provMQL4;}
		}
		#endregion

		#region internal static ChartStyleManager ChartStyleManager
		internal static ChartStyleManager ChartStyleManager{
			get{return _csmanager;}
		}
		#endregion

		#region internal static TimeFrame[] LoadTimeFrames()
		internal static TimeFrame[] LoadTimeFrames(){
			string stfs = Config.Users["Symbol"]["TimeFrames", TIMEFRAMES_INIT_DATA];
			string[] sa = stfs.Split('|');

			TimeFrame[] tfs = new TimeFrame[sa.Length];
			for (int i=0;i<sa.Length;i++){
				int second = Convert.ToInt32(sa[i]);
        tfs[i] = TimeFrameManager.TimeFrames.CreateNew(second);
			}
			return tfs;
		}
		#endregion

    internal static string GetNormalizeNumbler(string decNumber) {
      string dp = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
      decNumber = decNumber.Replace(",", dp);
      decNumber = decNumber.Replace(".", dp);
      return decNumber;
    }
	}
}
