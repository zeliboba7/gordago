/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using
using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Collections;

using Gordago.Strategy;
using Gordago.Strategy.FIndicator;
using Gordago.Strategy.FIndicator.FIndicParam;

using Gordago.Analysis;
using Cursit.Applic.AConfig;
using System.Windows.Forms;
#endregion

namespace Gordago.Strategy.IO {
	/// <summary>
	/// Класс для Сохранения и загрузки стратегии
	/// </summary>
	class StrategyIO {

		private EditorForm _eform;

		public StrategyIO(EditorForm eform) {
      _eform = eform;
		}

		#region opublic bool Load(string fileName)
		public bool Load(string fileName){
			try{
				this.LoadMethod(fileName);
			}catch(Exception exc){
				System.Windows.Forms.MessageBox.Show(exc.Message, GordagoMain.MessageCaption);
				return false;
			}
			return true;
		}
		#endregion

		#region private bool LoadMethod(string fileName)
		private bool LoadMethod(string fileName){

			XmlDocument xmldoc = new XmlDocument();
			xmldoc.Load(fileName);
			XmlNode node = xmldoc["Strategy"];
			if (node == null) return false;


			string version = "1.0";
			XmlAttribute attr = node.Attributes["Version"];
			if (attr != null || attr.Value != string.Empty){
				version = attr.Value;
			}

			if (version != "1.0"){
				attr = node.Attributes["Name"];
				if ( attr != null || attr.Value != string.Empty ){
          _eform.CName = attr.Value;
				}
				attr = node.Attributes["Sound"];
				if ( attr != null || attr.Value != string.Empty ){
          _eform.SetSoundFileName(attr.Value);
				}
				XmlNode nddesc = node["Description"];
				if (nddesc != null){
          this._eform.CDescription = nddesc.InnerText;
				}
			}


      XmlNode ndsell = node["Sell"];
      if(ndsell != null) {
        ParseXml(ndsell, _eform.EditSell, version);
      }

      XmlNode ndsellexit = node["SellExit"];
      if(ndsellexit != null) {
        ParseXml(ndsellexit, _eform.EditSellExit, version);
        ParseXmlPeriod(ndsellexit, _eform.CheckSellStop, "StopLoss");
        ParseXmlPeriod(ndsellexit, _eform.CheckSellLimit, "TackeProfit");
        ParseXmlPeriod(ndsellexit, _eform.CheckSellTrail, "TrailingStop");
      }


			XmlNode ndbuy = node["Buy"];
			if (ndbuy != null){
				ParseXml(ndbuy, _eform.EditBuy, version);
			}

			XmlNode ndbuyexit = node["BuyExit"];
			if (ndbuyexit != null){
				ParseXml(ndbuyexit, _eform.EditBuyExit, version);
				ParseXmlPeriod(ndbuyexit, _eform.CheckBuyStop, "StopLoss");
				ParseXmlPeriod(ndbuyexit, _eform.CheckBuyLimit, "TackeProfit");
				ParseXmlPeriod(ndbuyexit, _eform.CheckBuyTrail, "TrailingStop");
			}


      XmlNode ndposell = node["POSell"];
      if(ndposell != null) {
        ParseXml(ndposell, _eform.EditPOSell, version);
        attr = ndposell.Attributes["Modify"];
        if(attr != null && attr.Value != string.Empty) {
          _eform.IsPOSellModify = Convert.ToBoolean(attr.Value);
        }
      }

      XmlNode ndposellDel = node["POSellDelete"];
      if(ndposellDel != null) {
        ParseXml(ndposellDel, _eform.EditPOSellDelete, version);
      }


      XmlNode ndpopricesell = node["POPriceSell"];
      if(ndpopricesell != null) {
        ParseXmlPOPrice(ndpopricesell, _eform.EditPOPriceSell, version);
      }

      XmlNode ndpobuy = node["POBuy"];
      if(ndpobuy != null) {
        ParseXml(ndpobuy, _eform.EditPOBuy, version);
        attr = ndpobuy.Attributes["Modify"];
        if(attr != null && attr.Value != string.Empty) {
          _eform.IsPOBuyModify = Convert.ToBoolean(attr.Value);
        }
      }

      XmlNode ndpobuyDel = node["POBuyDelete"];
      if(ndpobuyDel != null) {
        ParseXml(ndpobuyDel, _eform.EditPOBuyDelete, version);
      }

      XmlNode ndpopricebuy = node["POPriceBuy"];
      if(ndpopricebuy != null) {
        ParseXmlPOPrice(ndpopricebuy, _eform.EditPOPriceBuy, version);
      }

			return true;
		}
		#endregion

		#region private static void ParseXmlPeriod(XmlNode nd, CheckPeriod cp, string nodeName)
		private static void ParseXmlPeriod(XmlNode nd, CheckPeriod cp, string nodeName){
			XmlNode ndcp = nd[nodeName];
			if (ndcp == null) return;

			XmlAttribute attr = ndcp.Attributes["Checked"];
			if ( attr == null || attr.Value == string.Empty )
				cp.Checked = false;
			else
				cp.Checked = bool.Parse(attr.Value);

			XmlNode ndinta = ndcp["inta"];
			if (ndinta != null){
				ArrayList al = new ArrayList();
				foreach (XmlNode ndint in ndinta.ChildNodes){
					if (ndint.Name == "int")
						al.Add(Convert.ToInt32(ndint.InnerText));
				}
				int[] inta = (int[])al.ToArray(typeof(int));
				if (inta.Length >= 2){
					cp.NumberBegin = inta[0];
					cp.NumberEnd = inta[1];
					if (inta.Length > 2)
						cp.Step = inta[2];
				}
			}
		}
		#endregion

    #region private static int ParseXmlAttr(XmlNode node, string attrname, int defval)
    private static int ParseXmlAttr(XmlNode node, string attrname, int defval) {
      XmlAttribute attr = node.Attributes[attrname];
      if(attr == null || attr.Value == string.Empty)
        return defval;
      else
        return Convert.ToInt32(attr.Value);
    }
    #endregion

    #region private static string ParseXmlAttr(XmlNode node, string attrname, string defval)
    private static string ParseXmlAttr(XmlNode node, string attrname, string defval) {
      XmlAttribute attr = node.Attributes[attrname];
      if(attr == null || attr.Value == string.Empty)
        return defval;
      else
        return attr.Value;
    }
    #endregion

    #region private static void ParseXmlPOPrice(XmlNode node, POPriceCaclulator poprice, string version)
    private static void ParseXmlPOPrice(XmlNode node, POPriceCaclulator poprice, string version) {
      foreach(XmlNode ndrow in node.ChildNodes) {

        if(ndrow.Name == "Row") {

          poprice.TimeFrameId = ParseXmlAttr(ndrow, "TimeFrame", 0);;

          ParseXmlTBO(ndrow, null, poprice, poprice.TextBoxObject, version);
        }
      }
    }
    #endregion

    #region private static void ParseXml(XmlNode node, SEditorVariants strgs, string version)
    private static void ParseXml(XmlNode node, SEditorVariants strgs, string version) {
      int ncurVariant = 0;
      XmlAttribute attr;

      foreach(XmlNode ndvariant in node.ChildNodes) {
        if(ndvariant.Name == "Variant") {

          if(ncurVariant >= strgs.SETables.Length)
            strgs.CraeteNewVariant();

          int ncurRow = 0;
          foreach(XmlNode ndrow in ndvariant.ChildNodes) {

            if(ndrow.Name == "Row") {
              if(ncurRow >= strgs.SETables[ncurVariant].SERows.Length)
                strgs.SETables[ncurVariant].CreateNewRow();

              SEditorRow row = strgs.SETables[ncurVariant].SERows[ncurRow];

              attr = ndrow.Attributes["TimeFrame"];
              if(attr == null || attr.Value == string.Empty)
                row.TimeFrameId = 0;
              else
                row.TimeFrameId = Convert.ToInt32(attr.Value);

              ParseXmlTBO(ndrow, strgs.SETables[ncurVariant], null,  row.TextBoxObject, version);

              attr = ndrow.Attributes["Status"];

              if(attr == null || attr.Value == string.Empty)
                row.Status = SEditorRowCompileStatus.Empty;
              else {
                row.Status = GetStatusFromString(attr.Value);
              }

              row.SetCompile();

              ncurRow++;
            }
          }
          ncurVariant++;
        }
      }
    }
    #endregion

    #region private static void ParseXmlTBO(XmlNode ndrow, SEditorTable tbl, POPriceCaclulator poprice, TextBoxObject tbo, string version)
    private static void ParseXmlTBO(XmlNode ndrow, SEditorTable tbl, POPriceCaclulator poprice, TextBoxObject tbo, string version) {
      XmlAttribute attr;

      foreach(XmlNode ndel in ndrow.ChildNodes) {
        tbo.SetCaretPositionToEnd();
        if(ndel.Name == "char") {
          tbo.Add(ndel.InnerText);
        } else if(ndel.Name == "func" || ndel.Name == "Func") {
          string typef = "";

          attr = ndel.Attributes["TypeName"];
          if(attr == null || attr.Value == string.Empty)
            break;

          typef = attr.Value;
          if(typef.IndexOf("Gordago.StockOptimizer2.Kernel.", 0) > -1) {
            typef = typef.Replace("Gordago.StockOptimizer2.Kernel.", "Gordago.Analysis.Kernel.");
          } else if(typef.IndexOf("Gordago.StockOptimizer2.Core.", 0) > -1) {
            typef = typef.Replace("Gordago.StockOptimizer2.Core.", "Gordago.Analysis.Kernel.");
          }

          Indicator indicator = null;
          Function function = null;

          foreach(Indicator findicator in GordagoMain.IndicatorManager.Indicators) {
            foreach(Function ffunction in findicator.Functions) {
              if(typef == ffunction.GetType().FullName) {
                indicator = findicator;
                function = ffunction;
                break;
              }
            }
          }

          if(indicator == null) {
            throw (new Exception("Indicator " + typef + " not found!"));
          }

          IndicatorGUI ind = new IndicatorGUI(indicator, indicator.GetParameters(), null);
          ind.WhoIs = IndicatorGUI.WhoIsWho.Editor;
          IndicFunction indf = ind.GetIndicFunction(function.Name);
          Parameter[] fparams = function.GetParameters();
          ArrayList alp = new ArrayList(fparams);
          bool oldversion = false;
          if(version == "1.1" || version == "1.0") {
            alp.Add(new ParameterInteger("asdfpoijasdpoifjasdpfj", 1));
            oldversion = true;
          }
          alp.Add(ind.CreateShiftParameter());
          fparams = (Parameter[])alp.ToArray(typeof(Parameter));

          attr = ndel.Attributes["Id"];
          if(attr == null || attr.Value == string.Empty)
            ind.GroupId = 0;
          else
            ind.GroupId = Convert.ToInt32(attr.Value);

          foreach(XmlNode ndprms in ndel.ChildNodes) {
            if(ndprms.Name == "Param") {
              for(int i = 0; i < fparams.Length; i++) {
                Parameter prm = fparams[i];

                foreach(XmlNode ndp in ndprms.ChildNodes) {
                  string pname = ParseXmlAttr(ndp, "Name", "");
                  if(pname == prm.Name) {
                    IndicFuncParam ifp = ind.GetFuncParam(prm);
                    if(ifp == null) {
                    } else if(ndp.Name == "int") {
                      if(oldversion && prm is ParameterFloat) {
                        ifp.Value = prm.Value;
                      } else
                        ifp.Value = Convert.ToInt32(ndp.InnerText);
                    } else if(ndp.Name == "float") {
                      ifp.Value = IndicFuncParamFloat.ConvertFromString(ndp.InnerText);
                    } else if(ndp.Name == "str") {
                      ifp.Value = ndp.InnerText;
                    } else if(ndp.Name == "stra") {
                      ArrayList al = new ArrayList();
                      foreach(XmlNode ndstr in ndp.ChildNodes) {
                        if(ndstr.Name == "str")
                          al.Add(ndstr.InnerText);
                      }
                      ifp.SetOptimizerValue((string[])al.ToArray(typeof(string)));
                    } else if(ndp.Name == "inta") {
                      ArrayList al = new ArrayList();
                      foreach(XmlNode ndstr in ndp.ChildNodes) {
                        if(ndstr.Name == "int")
                          al.Add(Convert.ToInt32(ndstr.InnerText));
                        if(ndstr.Name == "step") {
                          IndicFuncParamNumber ifpnumb = ifp as IndicFuncParamNumber;
                          ifpnumb.Step = Convert.ToInt32(ndstr.InnerText);
                        }
                      }
                      int[] inta = (int[])al.ToArray(typeof(int));
                      ifp.SetOptimizerValue(inta);
                    }
                  }
                }
              }
            }
          }
          if(tbl != null)
            tbl.AddIndicFunction(tbo, indf);
          else
            poprice.AddIndicFunction(indf);
        }
      }
    }
    #endregion

    #region public void SaveAs(string fileName)
    public void SaveAs(string fileName){
			XmlDocument xmldoc = new XmlDocument();
      string sfn = _eform.GetSoundFileName();
      xmldoc.LoadXml("<Strategy Name=\"" + 
        _eform.CName + "\" Sound = \"" + 
        sfn + "\" Version=\"1.3\"></Strategy>");
			XmlNode nddesc = xmldoc.CreateElement("Description");
      nddesc.InnerText = _eform.CDescription;
			xmldoc.DocumentElement.AppendChild(nddesc);

      CreateXmlAction(_eform.EditSell, xmldoc, "Sell");
      XmlNode ndsell = CreateXmlAction(_eform.EditSellExit, xmldoc, "SellExit");

      ndsell.AppendChild(CreateXmlPeriod(_eform.CheckSellStop, xmldoc, "StopLoss"));
      ndsell.AppendChild(CreateXmlPeriod(_eform.CheckSellLimit, xmldoc, "TackeProfit"));
      ndsell.AppendChild(CreateXmlPeriod(_eform.CheckSellTrail, xmldoc, "TrailingStop"));
			
			CreateXmlAction(_eform.EditBuy, xmldoc, "Buy");
			XmlNode ndbuy = CreateXmlAction(_eform.EditBuyExit, xmldoc, "BuyExit");

			ndbuy.AppendChild(CreateXmlPeriod(_eform.CheckBuyStop, xmldoc, "StopLoss"));
			ndbuy.AppendChild(CreateXmlPeriod(_eform.CheckBuyLimit, xmldoc, "TackeProfit"));
			ndbuy.AppendChild(CreateXmlPeriod(_eform.CheckBuyTrail, xmldoc, "TrailingStop"));

      CreateXmlPOPrice(_eform.EditPOPriceSell, xmldoc, "POPriceSell");
      XmlNode ndposell = CreateXmlAction(_eform.EditPOSell, xmldoc, "POSell");

      ndposell.Attributes.Append(xmldoc.CreateAttribute("Modify"));
      ndposell.Attributes["Modify"].Value = _eform.IsPOSellModify.ToString();

      CreateXmlAction(_eform.EditPOSellDelete, xmldoc, "POSellDelete");

      CreateXmlPOPrice(_eform.EditPOPriceBuy, xmldoc, "POPriceBuy");
      XmlNode ndpobuy = CreateXmlAction(_eform.EditPOBuy, xmldoc, "POBuy");

      ndpobuy.Attributes.Append(xmldoc.CreateAttribute("Modify"));
      ndpobuy.Attributes["Modify"].Value = _eform.IsPOBuyModify.ToString();

      CreateXmlAction(_eform.EditPOBuyDelete, xmldoc, "POBuyDelete");
			xmldoc.Save(fileName);
		}
		#endregion

		#region private static XmlNode CreateXmlPeriod(CheckPeriod cp, XmlDocument xmldoc, string nodeName)
		private static XmlNode CreateXmlPeriod(CheckPeriod cp, XmlDocument xmldoc, string nodeName){
			XmlNode nd = xmldoc.CreateElement(nodeName);

			nd.Attributes.Append(xmldoc.CreateAttribute("Checked"));
			nd.Attributes["Checked"].Value = cp.Checked.ToString();

			int[] inta = new int[3];
			inta[0] = (int)cp.NumberBegin;
			inta[1] = (int)cp.NumberEnd;
			inta[2] = (int)cp.Step;

			nd.AppendChild(CreateArrIntValue(xmldoc, inta));

			return nd;
		}
		#endregion

		#region private static XmlNode CreateArrIntValue(XmlDocument xmldoc, int[] inta)
		private static XmlNode CreateArrIntValue(XmlDocument xmldoc, int[] inta){
			XmlNode ndinta = xmldoc.CreateElement("inta");
			foreach (int ii in inta){
				XmlNode ndstra = xmldoc.CreateElement("int");
				ndstra.InnerText = ii.ToString();
				ndinta.AppendChild(ndstra);
			}
			return ndinta;
		}
		#endregion

		#region private static XmlNode CreateXmlAction(SEditorVariants variants, XmlDocument xmldoc, string nodeName)
		private static XmlNode CreateXmlAction(SEditorVariants variants, XmlDocument xmldoc, string nodeName){
			XmlNode xmlnode = xmldoc.CreateElement(nodeName);
			foreach (SEditorTable tbl in variants.SETables){
				XmlNode xmlnodeV = CreateXmlVariant(tbl, xmldoc);
				if (xmlnodeV != null)
					xmlnode.AppendChild(xmlnodeV);
			}
			xmldoc.DocumentElement.AppendChild(xmlnode);
			return xmlnode;
		}
		#endregion

    #region private static XmlNode CreateXmlPOPrice (POPriceCaclulator poprice, XmlDocument xmldoc, string nodename)
    private static XmlNode CreateXmlPOPrice (POPriceCaclulator poprice, XmlDocument xmldoc, string nodename) {
      XmlNode xmlnode = xmldoc.CreateElement(nodename);

      XmlNode ndrow = CreateXmlTBO(poprice.TextBoxObject, xmldoc);
      xmlnode.AppendChild(ndrow);

      ndrow.Attributes.Append(xmldoc.CreateAttribute("TimeFrame"));
      ndrow.Attributes["TimeFrame"].Value = poprice.TimeFrameId.ToString();

      xmldoc.DocumentElement.AppendChild(xmlnode);
      return xmlnode;
    }
    #endregion

    #region private static XmlNode CreateXmlVariant(SEditorTable seTbl, XmlDocument xmldoc)
    private static XmlNode CreateXmlVariant(SEditorTable seTbl, XmlDocument xmldoc){
			XmlNode xmlnodeV = xmldoc.CreateElement("Variant");
			int i=0;
			foreach (SEditorRow row in seTbl.SERows){
				if (row.TextBoxObject.Elements.Length > 0){
					i++;

          XmlNode ndrow = CreateXmlTBO(row.TextBoxObject, xmldoc);

          ndrow.Attributes.Append(xmldoc.CreateAttribute("TimeFrame"));
          ndrow.Attributes["TimeFrame"].Value = row.TimeFrameId.ToString();

          ndrow.Attributes.Append(xmldoc.CreateAttribute("Status"));
          ndrow.Attributes["Status"].Value = GetStatusFromRow(row);
          
          xmlnodeV.AppendChild(ndrow);
        }
      }
      if(i > 0) {
        return xmlnodeV;
      }

      return null;
    }
    #endregion

    #region private static XmlNode CreateXmlTBO(TextBoxObject tbo, XmlDocument xmldoc)
    private static XmlNode CreateXmlTBO(TextBoxObject tbo, XmlDocument xmldoc) {
      XmlNode ndrow = xmldoc.CreateElement("Row");

      object[] els = tbo.GetElementsFromType();
      foreach(object el in els) {
        XmlNode ndel = null;
        if(el is string) {
          ndel = xmldoc.CreateElement("char");
          ndel.InnerText = (string)el;
        } else if(el is Control) {
          IndicFunction indf = (el as IndicFunctionBox).IndicFunction;

          ndel = xmldoc.CreateElement("Func");
          ndel.Attributes.Append(xmldoc.CreateAttribute("Id"));
          ndel.Attributes["Id"].Value = indf.Parent.GroupId.ToString();

          ndel.Attributes.Append(xmldoc.CreateAttribute("TypeName"));
          ndel.Attributes["TypeName"].Value = indf.Function.GetType().FullName;

          XmlNode ndPs = xmldoc.CreateElement("Param");

          Parameter[] prms = indf.Function.GetParameters();
          ArrayList alp = new ArrayList(prms);
          alp.Add(indf.Parent.CreateShiftParameter());
          prms = (Parameter[])alp.ToArray(typeof(Parameter));

          foreach(Parameter prm in prms) {
            if(prm.Name != "__TimeFrame" && !(prm is ParameterColor)) {
              IndicFuncParam ifp = indf.Parent.GetFuncParam(prm);
              if(ifp == null) 
                ifp = indf.Parent.CreateParam(prm);
              
              XmlNode ndp = null;
              object optval = ifp.GetOptimizerValue();
              if(optval != null) {
                if(optval is string[]) {
                  string[] stra = (string[])optval;
                  ndp = xmldoc.CreateElement("stra");
                  foreach(string s in stra) {
                    XmlNode ndstra = xmldoc.CreateElement("str");
                    ndstra.InnerText = s;
                    ndp.AppendChild(ndstra);
                  }
                } else if(optval is int[]) {
                  ArrayList al = new ArrayList((int[])optval);

                  int[] inta = (int[])al.ToArray(typeof(int)); ;

                  ndp = CreateArrIntValue(xmldoc, inta);

                  if(ifp is IndicFuncParamNumber) {
                    XmlNode ndstep = xmldoc.CreateElement("step");
                    string sst = Convert.ToInt32((ifp as IndicFuncParamNumber).Step).ToString();
                    ndstep.InnerText = sst;
                    ndp.AppendChild(ndstep);
                  }
                }
              } else {
                string nname = "";
                string nval = "";
                if(ifp.Value is int) {
                  nname = "int";
                  nval = ifp.Value.ToString();
                } else if(ifp.Value is float) {
                  nname = "float";
                  nval = (ifp as IndicFuncParamFloat).Value.ToString();
                } else if(ifp.Value is string) {
                  nname = "str";
                  nval = ifp.Value.ToString();
                }
                ndp = xmldoc.CreateElement(nname);
                ndp.InnerText = nval;
              }
              ndp.Attributes.Append(xmldoc.CreateAttribute("Name"));
              ndp.Attributes["Name"].Value = ifp.Parameter.Name;

              ndPs.AppendChild(ndp);
            }
          }
          ndel.AppendChild(ndPs);
        }
        ndrow.AppendChild(ndel);
      }
      return ndrow;
    }
    #endregion

    #region internal static SEditorRowCompileStatus GetStatusFromString(string strStatus)
    internal static SEditorRowCompileStatus GetStatusFromString(string strStatus){
			switch(strStatus){
				case "Compile":
					return SEditorRowCompileStatus.Compile;
				case "Question":
					return SEditorRowCompileStatus.Question;
				case "Stop":
					return SEditorRowCompileStatus.Stop;
			}
			return SEditorRowCompileStatus.Empty;
		}
		#endregion

		#region internal static string GetStatusFromRow(SEditorRow row)
		internal static string GetStatusFromRow(SEditorRow row){
			switch(row.Status){
				case SEditorRowCompileStatus.Compile:
					return "Compile";
				case SEditorRowCompileStatus.Empty:
					return "Empty";
				case SEditorRowCompileStatus.Question:
					return "Question";
				case SEditorRowCompileStatus.Stop:
					return "Stop";
			}
			return "Empty";
		}
		#endregion
	}
}

