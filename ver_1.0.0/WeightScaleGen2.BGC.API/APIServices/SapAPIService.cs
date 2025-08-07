using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.API.APIRepository;
using WeightScaleGen2.BGC.API.Common;
using WeightScaleGen2.BGC.API.SAPExtension;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.SAPModels;
using WeightScaleGen2.BGC.Models.ServicesModels;

namespace WeightScaleGen2.BGC.API.APIServices
{
    public class SapAPIService : BaseAPIService
    {
        IDatabaseConnectionFactory _db;
        ISecurityCommon _securityCommon;
        private readonly UserInfoModel _userInfo;
        private RfcDestination rfcDestination;
        private ReturnDataRepository _returnDataRespository;

        public SapAPIService(IDatabaseConnectionFactory db, ISecurityCommon securityCommon, UserInfoModel userInfo, ReturnDataRepository returnDataRespository) : base(db, securityCommon)
        {
            _db = db;
            _securityCommon = securityCommon;
            _userInfo = userInfo;
            _returnDataRespository = returnDataRespository;
        }

        public Task<ReturnObject<bool>> ConnectionSAP(string userSAP, string passwordSAP)
        {
            var result = new ReturnObject<bool>();
            try
            {
                RfcConfigParameters parms = new RfcConfigParameters();
                parms.Add(RfcConfigParameters.Name, AppSetting.SAPDestination());
                parms.Add(RfcConfigParameters.LogonGroup, AppSetting.SAPDestination());
                parms.Add(RfcConfigParameters.MessageServerHost, AppSetting.SAPHost());
                parms.Add(RfcConfigParameters.SystemID, AppSetting.SAPSystemID());
                parms.Add(RfcConfigParameters.Client, AppSetting.SAPClient());
                parms.Add(RfcConfigParameters.SystemNumber, AppSetting.SAPSystemNumber());
                parms.Add(RfcConfigParameters.User, userSAP);
                parms.Add(RfcConfigParameters.Password, passwordSAP);
                parms.Add(RfcConfigParameters.Language, AppSetting.SAPLanguage());

                rfcDestination = RfcDestinationManager.GetDestination(parms);
                if (rfcDestination != null)
                {
                    rfcDestination.Ping();
                }

                result.data = true;
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.data = false;
                result.isCompleted = false;
                result.message.Add(ex.Message);
            }

            return Task.FromResult(result);
        }

        public Task<ReturnObject<bool>> SubmissionData(List<SapNcoModel> sapDatas)
        {
            var result = new ReturnObject<bool>();
            try
            {
                RfcRepository rfcRep = rfcDestination.Repository;
                IRfcFunction function = rfcRep.CreateFunction(AppSetting.SAPFunction());
                IRfcTable datasGT = function.GetTable(AppSetting.SAPTable());

                foreach (var sapData in sapDatas)
                {
                    datasGT.Append();
                    datasGT.SetValue("ZWGDOC", sapData.ZWGDOC);
                    datasGT.SetValue("ZWGDOC_SEQ", sapData.ZWGDOC_SEQ);
                    datasGT.SetValue("GR_TYPE", sapData.GR_TYPE);
                    datasGT.SetValue("DOC_DATE", sapData.DOC_DATE);
                    datasGT.SetValue("PSTNG_DATE", sapData.PSTNG_DATE);
                    datasGT.SetValue("REF_DOC_NO", sapData.REF_DOC_NO);
                    datasGT.SetValue("GOODSMVT_CODE", sapData.GOODSMVT_CODE);
                    datasGT.SetValue("MATERIAL", sapData.MATERIAL);
                    datasGT.SetValue("PLANT", sapData.PLANT);
                    datasGT.SetValue("STGE_LOC", sapData.STGE_LOC);
                    datasGT.SetValue("STCK_TYPE", sapData.STCK_TYPE);
                    datasGT.SetValue("ITEM_TEXT", sapData.ITEM_TEXT);
                    datasGT.SetValue("PO_NO", sapData.PO_NO);
                    datasGT.SetValue("PO_ITEM", sapData.PO_ITEM);
                    datasGT.SetValue("TRUCK_NO", sapData.TRUCK_NO);
                    datasGT.SetValue("WEIGHT_IN", sapData.WEIGHT_IN);
                    datasGT.SetValue("WEIGHT_OUT", sapData.WEIGHT_OUT);
                    datasGT.SetValue("WEIGHT_REC", sapData.WEIGHT_REC);
                    datasGT.SetValue("VEND_WEIGHT", sapData.VEND_WEIGHT);
                    datasGT.SetValue("REJ_WEIGHT", sapData.REJ_WEIGHT);
                    datasGT.SetValue("WEIGHT_UNIT", sapData.WEIGHT_UNIT);
                    datasGT.SetValue("P_STDATE", sapData.P_STDATE);
                    datasGT.SetValue("P_ENDATE", sapData.P_ENDATE);
                    datasGT.SetValue("ZPERM", sapData.ZPERM);
                    datasGT.SetValue("REV_MJAHR", sapData.REV_MJAHR);
                    datasGT.SetValue("REV_MBLNR", sapData.REV_MBLNR);
                }

                function.SetValue(AppSetting.SAPTable(), datasGT);
                function.Invoke(rfcDestination);

                IRfcTable poResult = function.GetTable(AppSetting.SAPReturnTable());
                var dtPoResult = poResult.ToDataTable("ex");

                //List<SapNcoResultsModel> poDatasResults = new List<SapNcoResultsModel>();
                foreach (DataRow dtrow in dtPoResult.Rows)
                {
                    var rsWeightOut = dtrow["ZWGDOC"].ToString();
                    decimal rsSeq = Convert.ToDecimal(dtrow["ZWGDOC_SEQ"].ToString());
                    var rsMatDoc = dtrow["MBLNR"].ToString();
                    var rsDocYear = dtrow["MJAHR"].ToString();
                    var rsType = dtrow["MSGTY"].ToString();
                    var rsText = dtrow["MESSAGE"].ToString();

                    if (rsType != "E")
                    {
                        // update results datas.
                        var poData = new SapNcoResultsModel()
                        {
                            ZWGDOC = rsWeightOut,
                            ZWGDOC_SEQ = rsSeq,
                            MBLNR = rsMatDoc,
                            MJAHR = rsDocYear,
                            MSGTY = rsType,
                            MESSAGE = rsText
                        };
                        _ = _returnDataRespository.Update_ReturnDataInfo_By_SAP(poData, _userInfo).Result;
                        //poDatasResults.Add(poData);
                    }
                }

                result.data = true;
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.data = false;
                result.isCompleted = false;
                result.message.Add(ex.Message);
            }

            return Task.FromResult(result);
        }
    }
}
