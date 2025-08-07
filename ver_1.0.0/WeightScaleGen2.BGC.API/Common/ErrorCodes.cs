namespace WeightScaleGen2.BGC.API.Common
{
    public class ErrorCodes
    {
        public class WeightScaleGen2
        {
            public class Service
            {
                public static int NotMatchDieSpec = 110000;
                public static int InsertSOByUploadFile = 110001;
                public static int InsertSOByManual = 110002;
                public static int UpdateSOByEditItem = 110003;
                public static int UpdateSOByDelay = 110004;
                public static int UpdateSOByAddToOrderSpit = 110005;
                public static int UpdateSOByAddToPostProcess = 110006;
                public static int UpdateSOByAddToProductionItemOrder = 110007;
                public static int UpdateSOByAddToProductionListOrder = 110008;
                public static int UpdateMFByAddToProductionListOrder = 110009;
                public static int UpdateADByAddToProductionListOrder = 110010;
                public static int UpdatePDByAddToProductionListOrder = 110011;
                public static int UpdateMOByAddToProductionListOrder = 110012;
                public static int DeletedSOByOrderProduction = 110013;
                public static int DeletedMFByOrderProduction = 110014;
                public static int DeletedADByOrderProduction = 110015;
                public static int DeletedPDByOrderProduction = 110016;
                public static int DeletedMOByOrderProduction = 110017;
                public static int UpdateSOByAddToDie = 110018;
                public static int DeletedSOByRemoveItem = 110019;
                public static int UpdateSOByMoveItem = 110020;
                public static int UpdateSOByEditOrderStatusItemOrder = 110021;
                public static int UpdateSOByEditOrderStatusListOrder = 110022;
                public static int GetSOById = 110023;
                public static int GetSOBy = 110024;
                public static int GetSODelayBy = 110025;
                public static int GetSOAll = 110026;
                public static int GetSOLastByFgCode = 110027;
                public static int InsertMFByUploadFile = 110028;
                public static int InsertMFByManual = 110029;
                public static int UpdateMFByEditItem = 110030;
                public static int UpdateMFByDelay = 110031;
                public static int UpdateMFByAddToProduction = 110032;
                public static int UpdateMFByAddToDie = 110033;
                public static int DeletedMFByRemoveItem = 110034;
                public static int UpdateMFByMoveItem = 110035;
                public static int UpdateMFByEditOrderStatus = 110036;
                public static int GetMFById = 110037;
                public static int GetMFBy = 110038;
                public static int GetMFAll = 110039;
                public static int InsertADByUploadFile = 110040;
                public static int InsertADByManual = 110041;
                public static int UpdateADByEditItem = 110042;
                public static int UpdateADByDelay = 110043;
                public static int UpdateADByAddToProduction = 110044;
                public static int UpdateADByAddToDie = 110045;
                public static int DeletedADByRemoveItem = 110046;
                public static int UpdateADByMoveItem = 110047;
                public static int UpdateADByEditOrderStatus = 110048;
                public static int GetADById = 110049;
                public static int GetADBy = 110050;
                public static int GetADAll = 110051;
                public static int InsertPDByUploadFile = 110052;
                public static int InsertPDByManual = 110053;
                public static int UpdatePDByEditItem = 110054;
                public static int UpdatePDByDelay = 110055;
                public static int UpdatePDByToProduction = 110056;
                public static int UpdatePDByToDie = 110057;
                public static int DeletedPDByRemoveItem = 110058;
                public static int UpdatePDByMoveItem = 110059;
                public static int UpdatePDByEditOrderStatus = 110060;
                public static int GetPDById = 110061;
                public static int GetPDBy = 110062;
                public static int GetPDAll = 110063;
                public static int InsertMOByUploadFile = 110064;
                public static int InsertMOByManual = 110065;
                public static int UpdateMOByEditItem = 110066;
                public static int UpdateMOByDelay = 110067;
                public static int UpdateMOByAddToProduction = 110068;
                public static int UpdateMOByAddToDie = 110069;
                public static int DeletedMOByRemoveItem = 110070;
                public static int UpdateMOByMoveItem = 110071;
                public static int UpdateMOByEditOrderStatus = 110072;
                public static int UpdateMOByAddToPending = 110073;
                public static int GetMOById = 110074;
                public static int GetMOBy = 110075;
                public static int GetMOAll = 110076;
                public static int GetPendingOrderList = 110077;
                public static int GetPendingStockById = 110078;
                public static int UpdateADByAddToPostProcessListOrder = 110079;
            }

            public class Repo
            {
                public static int Insert_SO_ByUpload = 100000;
                public static int Insert_SO_ByManual = 100001;
                public static int Update_SO_ByEditItem = 100002;
                public static int Update_SO_ByEditItemDieTest = 100003;
                public static int Update_SO_ByDelay = 100004;
                public static int Update_SO_ByAddToOrderSpit = 100005;
                public static int Update_SO_ByAddToDie = 100006;
                public static int Update_SO_ByRemoveItem = 100007;
                public static int Update_SO_ByMoveItem = 100008;
                public static int Update_SO_ByEditOrderStatusItemOrder = 100009;
                public static int Update_SO_ByEditOrderStatusListOrder = 100010;
                public static int Update_SO_ByAddToPostProcess = 100011;
                public static int Select_SO_ById = 100012;
                public static int Select_SO_By = 100013;
                public static int Select_SO_Delay_By = 100014;
                public static int Select_SO_All = 100015;
                public static int Insert_MF_ByUpload = 100016;
                public static int Insert_MF_ByManual = 100017;
                public static int Update_MF_ByEditItem = 100018;
                public static int Update_MF_ByDelay = 100019;
                public static int Update_MF_ByAddToProduction = 100020;
                public static int Update_MF_ByAddToDie = 100021;
                public static int Update_MF_ByRemoveItem = 100022;
                public static int Update_MF_ByMoveItem = 100023;
                public static int Update_MF_ByEditOrderStatus = 100024;
                public static int Select_MF_ById = 100025;
                public static int Select_MF_By = 100026;
                public static int Select_MF_All = 100027;
                public static int Select_Validation_UseDieSeries_BySeriesId = 100028;
                public static int Insert_AD_ByUpload = 100029;
                public static int Insert_AD_ByManual = 100030;
                public static int Update_AD_ByEditItem = 100031;
                public static int Update_AD_ByDelay = 100032;
                public static int Update_AD_ByAddToProduction = 100033;
                public static int Update_AD_ByAddToDie = 100034;
                public static int Update_AD_ByRemoveItem = 100035;
                public static int Update_AD_ByMoveItem = 100036;
                public static int Update_AD_ByEditOrderStatus = 100037;
                public static int Select_AD_ById = 100038;
                public static int Select_AD_By = 100039;
                public static int Select_AD_All = 100040;
                public static int Insert_PD_ByUpload = 100041;
                public static int Insert_PD_ByManual = 100042;
                public static int Update_PD_ByEditItem = 100043;
                public static int Update_PD_ByDelay = 100044;
                public static int Update_PD_ByAddToProduction = 100045;
                public static int Update_PD_ByAddToDie = 100046;
                public static int Update_PD_ByRemoveItem = 100047;
                public static int Update_PD_ByMoveItem = 100048;
                public static int Update_PD_ByEditOrderStatus = 100049;
                public static int Select_PD_ById = 100050;
                public static int Select_PD_By = 100051;
                public static int Select_PD_All = 100052;
                public static int Insert_MO_ByUpload = 100053;
                public static int Insert_MO_ByManual = 100054;
                public static int Update_MO_ByEditItem = 100055;
                public static int Update_MO_ByDelay = 100056;
                public static int Update_MO_ByAddToProduction = 100057;
                public static int Update_MO_ByAddToPending = 100058;
                public static int Update_MO_ByAddToDie = 100059;
                public static int Update_MO_ByRemoveItem = 100060;
                public static int Update_MO_ByMoveItem = 100061;
                public static int Update_MO_ByEditOrderStatus = 100062;
                public static int Select_MO_ById = 100063;
                public static int Select_MO_By = 100064;
                public static int Select_MO_All = 100065;
                public static int Select_Process_Mill_ByOrderId = 100066;
                public static int Select_Process_Oven_ByOrderId = 100067;
                public static int Select_Process_Wip_ByOrderId = 100068;
                public static int Select_Process_IPA_ByOrderId = 100069;
                public static int Select_Process_IPP_ByOrderId = 100070;
                public static int Select_Process_Spit_ByOrderId = 100071;
                public static int Select_Process_PackSpit_ByOrderId = 100072;
                public static int Select_Process_Wh_ByOrderId = 100073;
                public static int Select_SO_Last_ByFgCode = 100074;
                public static int Update_SO_ByAddToProductionItemOrder = 100075;
                public static int Update_SO_ByAddToProductionListOrder = 100076;
                public static int Update_MF_ByAddToProductionListOrder = 100077;
                public static int Update_AD_ByAddToProductionListOrder = 100078;
                public static int Update_PD_ByAddToProductionListOrder = 100079;
                public static int Update_MO_ByAddToProductionListOrder = 100080;
                public static int Get_PendingOrderData = 100081;
                public static int Get_PendingStockByIdData = 100082;
                public static int Delete_SO_ByOrderProduction = 100083;
                public static int Delete_MF_ByOrderProduction = 100084;
                public static int Delete_AD_ByOrderProduction = 100085;
                public static int Delete_PD_ByOrderProduction = 100086;
                public static int Delete_MO_ByOrderProduction = 100087;
                public static int Update_AD_ByAddToPostProcessListOrder = 100088;
            }
        }

        public class Extrusion
        {
            public class Service
            {
                public static int GetOrderMFById = 210000;
                public static int GetOrderMFBy = 210001;
                public static int InsertMillInfo = 210002;
                public static int UpdateMillInfo = 210003;
                public static int ClosedOrderMill = 210004;
                public static int DeleteConfirmMill = 210005;
                public static int GetOrderOVById = 210006;
                public static int GetOrderOVBy = 210007;
                public static int InsertOvenInfo = 210008;
                public static int UpdateOvenInfo = 210009;
                public static int ClosedOrderOven = 210010;
                public static int DeleteConfirmOven = 210011;
                public static int GetOrderIpaById = 210012;
                public static int GetOrderIpaBy = 210013;
                public static int InsertIpaInfo = 210014;
                public static int UpdateIpaInfo = 210015;
                public static int ClosedOrderIpa = 210016;
                public static int DeleteConfirmIpa = 210017;
                public static int GetOrderIppById = 210018;
                public static int GetOrderIppBy = 210019;
                public static int InsertIppInfo = 210020;
                public static int UpdateIppInfo = 210021;
                public static int ClosedOrderIpp = 210022;
                public static int DeleteConfirmIpp = 210023;
                public static int GetOrderPackById = 210024;
                public static int GetOrderPackBy = 210025;
                public static int InsertPackInfo = 210026;
                public static int UpdatePackInfo = 210027;
                public static int ClosedOrderPack = 210028;
                public static int DeleteConfirmPack = 210029;
                public static int GetOrderSpitById = 210030;
                public static int GetOrderSpitBy = 210031;
                public static int InsertSpitInfo = 210032;
                public static int UpdateSpitInfo = 210033;
                public static int ClosedOrderSpit = 210034;
                public static int DeleteConfirmSpit = 210035;
                public static int GetOrderWipById = 210036;
                public static int GetOrderWipBy = 210037;
                public static int InsertWipInfo = 210038;
                public static int UpdateWipInfo = 210039;
                public static int ClosedOrderWip = 210040;
                public static int DeleteConfirmWip = 210041;
                public static int GetOrderWhById = 210042;
                public static int GetOrderWhBy = 210043;
                public static int InsertWhInfo = 210044;
                public static int UpdateWhInfo = 210045;
                public static int ClosedOrderWh = 210046;
                public static int DeleteConfirmWh = 210047;
                public static int GetProductionMillBy = 210048;
                public static int GetProductionMillById = 210049;
                public static int GetProductionOvenBy = 210050;
                public static int GetProductionOvenById = 210051;
                public static int GetProductionIpaBy = 210052;
                public static int GetProductionIpaById = 210053;
                public static int GetProductionIppBy = 210054;
                public static int GetProductionIppById = 210055;
                public static int GetProductionPackBy = 210056;
                public static int GetProductionPackById = 210057;
                public static int GetProductionSpitBy = 210058;
                public static int GetProductionSpitById = 210059;
                public static int GetProductionWipBy = 210060;
                public static int GetProductionWipById = 210061;
                public static int GetProductionWhBy = 210062;
                public static int GetProductionWhById = 210063;
                public static int GetReportIdentifyProductDocumentMill = 210064;
                public static int GetReportIdentifyProductDocumentOven = 210065;
                public static int GetReportIdentifyProductDocumentIpa = 210066;
                public static int GetReportIdentifyProductDocumentIpp = 210067;
                public static int GetReportIdentifyProductDocumentPack = 210068;
                public static int GetReportIdentifyProductDocumentSpit = 210069;
                public static int GetReportIdentifyProductDocumentWip = 210070;
                public static int GetReportIdentifyProductDocumentWh = 210071;
            }

            public class Repo
            {
                public static int Select_Mill_By = 200000;
                public static int Select_Mill_ById = 200001;
                public static int Select_Oven_By = 200002;
                public static int Select_Oven_ById = 200003;
                public static int Select_Ipa_By = 200004;
                public static int Select_Ipa_ById = 200005;
                public static int Select_Ipp_By = 200006;
                public static int Select_Ipp_ById = 200007;
                public static int Select_Pack_By = 200008;
                public static int Select_Pack_ById = 200009;
                public static int Select_Spit_By = 200010;
                public static int Select_Spit_ById = 200011;
                public static int Select_Wip_By = 200012;
                public static int Select_Wip_ById = 200013;
                public static int Select_Wh_By = 200014;
                public static int Select_Wh_ById = 200015;
                public static int Select_Mill_Production_By = 200016;
                public static int Select_Mill_Production_ById = 200017;
                public static int Select_MillDefect_Production_ByMFId = 200018;
                public static int Select_Oven_Production_By = 200019;
                public static int Select_Oven_Production_ById = 200020;
                public static int Select_Ipa_Production_By = 200021;
                public static int Select_Ipa_Production_ById = 200022;
                public static int Select_Ipp_Production_By = 200023;
                public static int Select_Ipp_Production_ById = 200024;
                public static int Select_Pack_Production_By = 200025;
                public static int Select_Pack_Production_ById = 200026;
                public static int Select_Spit_Production_By = 200027;
                public static int Select_Spit_Production_ById = 200028;
                public static int Select_Wip_Production_By = 200029;
                public static int Select_Wip_Production_ById = 200030;
                public static int Select_Wh_Production_By = 200031;
                public static int Select_Wh_Production_ById = 200032;
                public static int Insert_ConfirmMill = 200033;
                public static int Update_ConfirmMill = 200034;
                public static int Delete_ConfirmMill = 200035;
                public static int Update_ConfirmMill_ClosedOrder = 200036;
                public static int Insert_ConfirmOven = 200037;
                public static int Update_ConfirmOven = 200038;
                public static int Delete_ConfirmOven = 200039;
                public static int Update_ConfirmOven_ClosedOrder = 200040;
                public static int Insert_ConfirmIpa = 200041;
                public static int Update_ConfirmIpa = 200042;
                public static int Delete_ConfirmIpa = 200043;
                public static int Update_ConfirmIpa_ClosedOrder = 200044;
                public static int Insert_ConfirmIpp = 200045;
                public static int Update_ConfirmIpp = 200046;
                public static int Delete_ConfirmIpp = 200047;
                public static int Update_ConfirmIpp_ClosedOrder = 200048;
                public static int Insert_ConfirmPack = 200049;
                public static int Update_ConfirmPack = 200050;
                public static int Delete_ConfirmPack = 200051;
                public static int Update_ConfirmPack_ClosedOrder = 200052;
                public static int Insert_ConfirmSpit = 200053;
                public static int Update_ConfirmSpit = 200054;
                public static int Delete_ConfirmSpit = 200055;
                public static int Update_ConfirmSpit_ClosedOrder = 200056;
                public static int Insert_ConfirmWip = 200057;
                public static int Update_ConfirmWip = 200058;
                public static int Delete_ConfirmWip = 200059;
                public static int Update_ConfirmWip_ClosedOrder = 200060;
                public static int Insert_ConfirmWh = 200061;
                public static int Update_ConfirmWh = 200062;
                public static int Delete_ConfirmWh = 200063;
                public static int Update_ConfirmWh_ClosedOrder = 200064;
            }
        }

        public class Machine
        {
            public class Service
            {
                public static int GetSearchListMachineBy = 310000;
                public static int GetSearchMachineById = 310001;
                public static int GetSearchListMachineAll = 310002;
                public static int GetSearchListMachineAllByDieNo = 310003;
                public static int GetSearchListMachineAllByDieId = 310004;
                public static int GetSearchListMachineDealerAll = 310005;
                public static int InsertMachineInfo = 310006;
                public static int UpdateMachineInfo = 310007;
                public static int DeleteMachineInfo = 310008;
                public static int GetSearchListMachineTitleBy = 310009;
                public static int GetSearchMachineTitleById = 310010;
                public static int GetSearchListMachineTitleAll = 310011;
                public static int InsertMachineTitle = 310012;
                public static int UpdateMachineTitle = 310013;
                public static int DeleteMachineTitle = 310014;
                public static int GetSearchListMachineSeriesBy = 310015;
                public static int GetSearchListMachineSeriesByDieInfoId = 310016;
                public static int GetSearchMachineSeriesById = 310017;
                public static int GetSearchListMachineSeriesAll = 310018;
                public static int InsertMachineSeries = 310019;
                public static int UpdateMachineSeries = 310020;
                public static int UpdateMachineSeriesByUploadFile = 310021;
                public static int UpdateMachineSeriesByExpire = 310022;
                public static int DeleteMachineSeries = 310023;
                public static int GetReportDieFailList = 310024;
                public static int GetReportDieLife = 310025;
                public static int GetReportDieFromOrderPending = 310026;
                public static int GetReportDieTest = 310027;
                public static int InsertDieTest = 310028;
                public static int UpdateDieTest = 310029;
                public static int DeleteDieTest = 310030;
            }

            public class Repo
            {
                public static int Insert_MachineInfo = 300000;
                public static int Update_MachineInfo = 300001;
                public static int Delete_MachineInfo = 300002;
                public static int Insert_MachineTitle = 300003;
                public static int Update_MachineTitle = 300004;
                public static int Delete_MachineTitle = 300005;
                public static int Select_MachineData_ById = 300006;
                public static int Select_MachineData_ByDieNumber = 300007;
                public static int Select_MachineDataList_By = 300008;
                public static int Select_MachineDataList_All = 300009;
                public static int Select_MachineDataList_ByDieNo = 300010;
                public static int Select_MachineDataList_ByDieId = 300011;
                public static int Select_MachineTitleData_ById = 300012;
                public static int Select_MachineTitleDataList_By = 300013;
                public static int Select_MachineTitleDataList_All = 300014;
                public static int Select_MachineSeriesData_ById = 300015;
                public static int Select_MachineSeriseDataList_By = 300016;
                public static int Select_MachineSeriseDataList_All = 300017;
                public static int Select_MachineSeriesTypeData_BySerieId = 300018;
                public static int Insert_MachineSeries = 300019;
                public static int Update_MachineSeries = 300020;
                public static int Delete_MachineSeries = 300021;
                public static int Delete_MachineSeriesType_BySerieId = 300022;
                public static int Select_MachineDealerListAll = 300023;
                public static int Update_MachineStatus_ByUploadFile = 300024;
                public static int Update_MachineSeries_ByExpire = 300025;
                public static int Select_MachineSeriesDataList_ByDieInfoId = 300026;
                public static int Select_Report_DieFailListData = 300027;
                public static int Select_Report_DieFromOrderPendingData = 300028;
                public static int Select_Report_DieLifeData = 300029;
                public static int Select_Report_DieTest = 300030;
                public static int Insert_DieTest = 300031;
                public static int Update_DieTest = 300032;
                public static int Delete_DieTest = 300033;
            }
        }

        public class Scrap
        {
            public class Service
            {
                public static int GetReportYieldBySKU = 410000;
                public static int GetReportWipLastMonth = 410001;
            }

            public class Repo
            {
                public static int Select_Report_YieldBySKU = 400000;
                public static int Select_Report_WipLastMonth = 400000;
            }
        }

        public class FG
        {
            public class Service
            {
                public static int PostInfo = 510000;
                public static int PutInfo = 510001;
                public static int DeleteInfo = 510002;
                public static int GetData_ById = 510003;
                public static int GetDataList_By = 510004;
                public static int GetDataList_ByCode = 510005;
                public static int GetDataList_All = 510006;
            }

            public class Repo
            {
                public static int Insert_Info = 500001;
                public static int Update_Info = 500002;
                public static int Delete_Info = 500003;
                public static int Select_Data_ById = 500004;
                public static int Select_DataList_By = 500005;
                public static int Select_DataList_All = 500006;
                public static int Select_DataList_ByFgCode = 500007;
            }
        }

        public class Die
        {
            public class Service
            {

            }

            public class Repo
            {

            }
        }

        public class Report
        {
            public class Service
            {

            }

            public class Repo
            {

            }
        }
        ///
        /// SYSTEM
        ///
        public class Company
        {
            public class Service
            {
                public static int PostInfo = 1001;
                public static int PutInfo = 1002;
                public static int DeleteInfo = 1003;
                public static int GetListComp = 1004;
                public static int GetSearchListComp = 1005;
            }

            public class Repo
            {
                public static int Insert_Info = 1006;
                public static int Update_Info = 1007;
                public static int Delete_Info = 1008;
                public static int Select_Company_All = 1009;
                public static int Select_Company_By = 1010;
            }
        }

        public class Plant
        {
            public class Service
            {
                public static int PostInfo = 2001;
                public static int PutInfo = 2002;
                public static int DeleteInfo = 2003;
                public static int GetListPlant = 2004;
                public static int GetSearchListPlant = 2005;
            }

            public class Repo
            {
                public static int Delete_Info = 2006;
                public static int Insert_Info = 2007;
                public static int Select_Plant_All = 2008;
                public static int Select_Plant_By = 2009;
                public static int Update_Info = 2010;
            }
        }

        public class Department
        {
            public class Service
            {
                public static int PostInfo = 3001;
                public static int PutInfo = 3002;
                public static int DeleteInfo = 3003;
                public static int GetListDept = 3004;
                public static int GetSearchListDept = 3005;
            }

            public class Repo
            {
                public static int Delete_Info = 3006;
                public static int Insert_Info = 3007;
                public static int Select_DepartmentData_All = 3008;
                public static int Select_DepartmentData_By = 3009;
                public static int Update_Info = 3010;
            }
        }

        public class Employee
        {
            public class Service
            {
                public static int PostEmployeeInfo = 4001;
                public static int PutEmployeeInfo = 4002;
                public static int GetSearchListEmp = 4003;
                public static int GetEmpInfo = 4004;
                public static int GetListEmp = 4005;
            }

            public class Repo
            {
                public static int Insert_EmployeeInfo = 4006;
                public static int Select_EmployeeInfo = 4007;
                public static int Select_EmployeeListData_All = 4008;
                public static int Select_SearchEmployeeListData_By = 4009;
                public static int Update_EmployeeInfo = 4010;
            }
        }

        public class Log
        {
            public class Service
            {
                public static int SearchLogData = 5001;
                public static int GetSearchLogCriteria = 5002;
            }

            public class Repo
            {
                public static int Select_ListLogDataDll_By = 5003;
                public static int Select_ListLogLevelDll_All = 5004;
            }
        }

        public class Master
        {
            public class Service
            {
                public static int PostInfo = 6001;
                public static int PutInfo = 6002;
                public static int DeleteInfo = 6003;
                public static int GetListMasterType = 6004;
                public static int GetListMaster = 6005;
                public static int GetSearchListMaster = 6006;
            }

            public class Repo
            {
                public static int Insert_Info = 6007;
                public static int Update_Info = 6008;
                public static int Delete_Info = 6009;
                public static int Select_MasterData = 6010;
            }
        }

        public class Menu
        {
            public class Service
            {
                public static int AuthenticateMenuUser = 7001;
                public static int GetRoleInfo = 7002;
                public static int CreatedRole = 7003;
                public static int UpdatedRole = 7004;
                public static int DeletedRole = 7005;
                public static int UpdateMenuRole = 7006;
                public static int UpdateMenuRoleSelectItem = 7007;
                public static int GetMenuRole = 7008;
            }

            public class Repo
            {
                public static int Delete_Role = 7009;
                public static int Insert_MenuSection = 7010;
                public static int Insert_Role = 7011;
                public static int Insert_User = 7012;
                public static int Select_MenuRole = 7013;
                public static int Select_MenuSectionRole = 7014;
                public static int Select_MenuSectionUser = 7015;
                public static int Select_MenuUser = 7016;
                public static int Select_Role = 7017;
                public static int Select_RoleUsing = 7018;
                public static int Select_User_ByUsername = 7019;
                public static int Update_Role = 7020;
                public static int Update_RoleItem = 7021;
                public static int Update_RoleSelectItem = 7022;
            }
        }

        public class PrefixDoc
        {
            public class Service
            {
                public static int GetPrefixDoc = 8001;
            }

            public class Repo
            {
                public static int Select_RunningCode = 8002;
            }
        }

        public class System
        {
            public class Service
            {
                public static int GetListSystem = 9001;
            }

            public class Repo
            {
                public static int Select_SystemData = 9002;
            }
        }

        public class User
        {
            public class Service
            {
                public static int GetSearchUserCriteriaNotAssign = 1101;
                public static int GetSearchUserCriteria = 1102;
                public static int SearchUser = 1103;
                public static int GetImageAll = 1104;
                public static int GetUserById = 1105;
                public static int GetUserByUsername = 1106;
                public static int GetUserByName = 1107;
                public static int UpdateUser = 1108;
                public static int UploadImage = 1109;
                public static int PostUserInfo = 1110;
                public static int GetUserByUsernamePassword = 1111;
            }

            public class Repo
            {
                public static int Upload_Image = 1112;
                public static int Update_User = 1113;
                public static int Select_User_ById = 1114;
                public static int Select_User_ByUsername = 1115;
                public static int Select_User_ByName = 1116;
                public static int Select_User = 1117;
                public static int Select_RoleDll = 1116;
                public static int Select_ImageAll = 1117;
            }
        }

        public class ItemMaster
        {
            public class Service
            {
                public static int PostItemMasterInfo = 1201;
                public static int PutItemMasterInfo = 1202;
                public static int GetSearchListItemMaster = 1203;
                public static int GetItemMasterInfo = 1204;
                public static int GetListItemMaster = 1205;
                public static int DeleteItemMasterInfo = 1206;
            }

            public class Repo
            {
                public static int Insert_ItemMasterInfo = 1207;
                public static int Select_ItemMasterInfo = 1208;
                public static int Select_ItemMasterListData_All = 1209;
                public static int Select_SearchItemMasterListData_By = 1210;
                public static int Update_ItemMasterInfo = 1211;
                public static int Delete_ItemMasterInfo = 1212;
            }
        }

        public class Supplier
        {
            public class Service
            {
                public static int PostSupplierInfo = 1301;
                public static int PutSupplierInfo = 1302;
                public static int GetSearchListSupplier = 1303;
                public static int GetSupplierInfo = 1304;
                public static int GetListSupplier = 1305;
                public static int DeleteSupplierInfo = 1306;
            }

            public class Repo
            {
                public static int Insert_SupplierInfo = 1307;
                public static int Select_SupplierInfo = 1308;
                public static int Select_SupplierListData_All = 1309;
                public static int Select_SearchSupplierListData_By = 1310;
                public static int Update_SupplierInfo = 1311;
                public static int Delete_SupplierInfo = 1312;
            }
        }

        public class Sender
        {
            public class Service
            {
                public static int PostSenderInfo = 1401;
                public static int PutSenderInfo = 1402;
                public static int GetSearchListSender = 1403;
                public static int GetSenderInfo = 1404;
                public static int GetListSender = 1405;
                public static int DeleteSenderInfo = 1406;
            }

            public class Repo
            {
                public static int Insert_SenderInfo = 1407;
                public static int Select_SenderInfo = 1408;
                public static int Select_SenderListData_All = 1409;
                public static int Select_SearchSenderListData_By = 1410;
                public static int Update_SenderInfo = 1411;
                public static int Delete_SenderInfo = 1412;
            }
        }

        public class GroupMaster
        {
            public class Service
            {
                public static int PostGroupMasterInfo = 1501;
                public static int PutGroupMasterInfo = 1502;
                public static int DeleteGroupMasterInfo = 1503;
                public static int GetListGroupMasterType = 1504;
                public static int GetListGroupMaster = 1505;
                public static int GetSearchListGroupMaster = 1506;
            }

            public class Repo
            {
                public static int Insert_GroupMasterInfo = 1507;
                public static int Update_GroupMasterInfo = 1508;
                public static int Delete_GroupMasterInfo = 1509;
                public static int Select_GroupMasterData = 1510;
            }
        }

        public class ItemMasterRelation
        {
            public class Service
            {
                public static int PostItemMasterRelationInfo = 1601;
                public static int PutItemMasterRelationInfo = 1602;
                public static int DeleteItemMasterRelationInfo = 1603;
                public static int GetItemMasterRelationInfo = 1604;
                public static int GetListItemMasterRelation = 1605;
                public static int GetSearchListItemMasterRelation = 1606;
            }

            public class Repo
            {
                public static int Insert_ItemMasterRelationInfo = 1607;
                public static int Update_ItemMasterRelationInfo = 1608;
                public static int Delete_ItemMasterRelationInfo = 1609;
                public static int Select_ItemMasterRelationData = 1610;
            }
        }

        public class WeightHistory
        {
            public class Service
            {
                public static int PostWeightHistoryInfo = 1701;
                public static int PutWeightHistoryInfo = 1702;
                public static int GetSearchListWeightHistory = 1703;
                public static int GetWeightHistoryInfo = 1704;
                public static int GetListWeightHistory = 1705;
                public static int DeleteWeightHistoryInfo = 1706;
            }

            public class Repo
            {
                public static int Insert_WeightHistoryInfo = 1707;
                public static int Select_WeightHistoryInfo = 1708;
                public static int Select_WeightHistoryListData_All = 1709;
                public static int Select_SearchWeightHistoryListData_By = 1710;
                public static int Update_WeightHistoryInfo = 1711;
                public static int Delete_WeightHistoryInfo = 1712;
            }
        }

        public class WeightIn
        {
            public class Service
            {
                public static int PostWeightInInfo = 1801;
                public static int PutWeightInInfo = 1802;
                public static int GetSearchListWeightIn = 1803;
                public static int GetWeightInInfo = 1804;
                public static int GetWeightInInfoByCarLicense = 1805;
                public static int GetListWeightIn = 1806;
                public static int DeleteWeightInInfo = 1807;
            }

            public class Repo
            {
                public static int Insert_WeightInInfo = 1808;
                public static int Select_WeightInInfo = 1809;
                public static int Select_WeightInListData_All = 1810;
                public static int Select_SearchWeightInListData_By = 1811;
                public static int Update_WeightInInfo = 1812;
                public static int Delete_WeightInInfo = 1813;
            }
        }

        public class WeightOut
        {
            public class Service
            {
                public static int PostWeightOutInfo = 1901;
                public static int PutWeightOutInfo = 1902;
                public static int GetSearchListWeightOut = 1903;
                public static int GetWeightOutInfo = 1904;
                public static int GetWeightOutInfoByCarLicense = 1905;
                public static int GetListWeightOut = 1906;
                public static int DeleteWeightOutInfo = 1907;
            }

            public class Repo
            {
                public static int Insert_WeightOutInfo = 1908;
                public static int Select_WeightOutInfo = 1909;
                public static int Select_WeightOutListData_All = 1910;
                public static int Select_SearchWeightOutListData_By = 1911;
                public static int Update_WeightOutInfo = 1912;
                public static int Delete_WeightOutInfo = 1913;
            }
        }

        public class WeightOutHistory
        {
            public class Service
            {
                public static int PostWeightOutHistoryInfo = 2001;
                public static int PutWeightOutHistoryInfo = 2002;
                public static int GetSearchListWeightOutHistory = 2003;
                public static int GetWeightOutHistoryInfo = 2004;
                public static int GetListWeightOutHistory = 2005;
                public static int DeleteWeightOutHistoryInfo = 2006;
            }

            public class Repo
            {
                public static int Insert_WeightOutHistoryInfo = 2007;
                public static int Select_WeightOutHistoryInfo = 2008;
                public static int Select_WeightOutHistoryListData_All = 2009;
                public static int Select_SearchWeightOutHistoryListData_By = 2010;
                public static int Update_WeightOutHistoryInfo = 2011;
                public static int Delete_WeightOutHistoryInfo = 2012;
            }
        }

        public class WeightCompare
        {
            public class Service
            {
                public static int PostWeightCompareInfo = 2101;
                public static int PutWeightCompareInfo = 2102;
                public static int GetSearchListWeightCompare = 2103;
                public static int GetWeightCompareInfo = 2104;
                public static int GetListWeightCompare = 2105;
                public static int DeleteWeightCompareInfo = 2106;
            }

            public class Repo
            {
                public static int Insert_WeightCompareInfo = 2107;
                public static int Select_WeightCompareInfo = 2108;
                public static int Select_WeightCompareListData_All = 2109;
                public static int Select_SearchWeightCompareListData_By = 2110;
                public static int Update_WeightCompareInfo = 2111;
                public static int Delete_WeightCompareInfo = 2112;
            }
        }

        public class WeightDaily
        {
            public class Service
            {
                public static int PostWeightDailyInfo = 2201;
                public static int PutWeightDailyInfo = 2202;
                public static int GetSearchListWeightDaily = 2203;
                public static int GetWeightDailyInfo = 2204;
                public static int GetListWeightDaily = 2205;
                public static int DeleteWeightDailyInfo = 2206;
            }

            public class Repo
            {
                public static int Insert_WeightDailyInfo = 2207;
                public static int Select_WeightDailyInfo = 2208;
                public static int Select_WeightDailyListData_All = 2209;
                public static int Select_SearchWeightDailyListData_By = 2210;
                public static int Update_WeightDailyInfo = 2211;
                public static int Delete_WeightDailyInfo = 2212;
            }
        }

        public class WeightSummaryDay
        {
            public class Service
            {
                public static int PostWeightSummaryDayInfo = 2301;
                public static int PutWeightSummaryDayInfo = 2302;
                public static int GetSearchListWeightSummaryDay = 2303;
                public static int GetWeightSummaryDayInfo = 2304;
                public static int GetListWeightSummaryDay = 2305;
                public static int DeleteWeightSummaryDayInfo = 2306;
            }

            public class Repo
            {
                public static int Insert_WeightSummaryDayInfo = 2307;
                public static int Select_WeightSummaryDayInfo = 2308;
                public static int Select_WeightSummaryDayListData_All = 2309;
                public static int Select_SearchWeightSummaryDayListData_By = 2310;
                public static int Update_WeightSummaryDayInfo = 2311;
                public static int Delete_WeightSummaryDayInfo = 2312;
            }
        }

        public class DocumentPO
        {
            public class Service
            {
                public static int PostDocumentPOInfo = 2401;
                public static int PutDocumentPOInfo = 2402;
                public static int GetSearchListDocumentPO = 2403;
                public static int GetDocumentPOInfo = 2404;
                public static int GetListDocumentPO = 2405;
                public static int DeleteDocumentPOInfo = 2406;
            }

            public class Repo
            {
                public static int Insert_DocumentPOInfo = 2407;
                public static int Select_DocumentPOInfo = 2408;
                public static int Select_DocumentPOListData_All = 2409;
                public static int Select_SearchDocumentPOListData_By = 2410;
                public static int Update_DocumentPOInfo = 2411;
                public static int Delete_DocumentPOInfo = 2412;
            }
        }

        public class ReturnData
        {
            public class Service
            {
                public static int PostReturnDataInfo = 2501;
                public static int PutReturnDataInfo = 2502;
                public static int GetSearchListReturnData = 2503;
                public static int GetReturnDataInfo = 2504;
                public static int GetListReturnData = 2505;
                public static int DeleteReturnDataInfo = 2506;
            }

            public class Repo
            {
                public static int Insert_ReturnDataInfo = 2507;
                public static int Select_ReturnDataInfo = 2508;
                public static int Select_ReturnDataListData_All = 2509;
                public static int Select_SearchReturnDataListData_By = 2510;
                public static int Update_ReturnDataInfo = 2511;
                public static int Delete_ReturnDataInfo = 2512;
            }
        }

        public class Dashboard
        {
            public class Service
            {
                public static int GetSearchListDashboardSummaryData = 2601;
                public static int GetSearchListDashboardSummaryHistoryData = 2602;
            }
        }

        public class WeightMaster
        {
            public class Service
            {
                public static int CopyDeleteWeightMaster = 2701;
            }
        }

        public class WeightInHistory
        {
            public class Service
            {
                public static int PostWeightInHistoryInfo = 2801;
                public static int PutWeightInHistoryInfo = 2802;
                public static int GetSearchListWeightInHistory = 2803;
                public static int GetWeightInHistoryInfo = 2804;
                public static int GetListWeightInHistory = 2805;
                public static int DeleteWeightInHistoryInfo = 2806;
            }

            public class Repo
            {
                public static int Insert_WeightInHistoryInfo = 2807;
                public static int Select_WeightInHistoryInfo = 2808;
                public static int Select_WeightInHistoryListData_All = 2809;
                public static int Select_SearchWeightInHistoryListData_By = 2810;
                public static int Update_WeightInHistoryInfo = 2811;
                public static int Delete_WeightInHistoryInfo = 2812;
            }
        }

        public class MMPO
        {
            public class Service
            {
                public static int PostMMPOInfo = 2901;
                public static int PutMMPOInfo = 2902;
                public static int GetSearchListMMPO = 2903;
                public static int GetMMPOInfo = 2904;
                public static int GetListMMPO = 2905;
                public static int DeleteMMPOInfo = 2906;
                public static int UpdateMMPOSapToDocumentPO = 2907;
                public static int GetSearchMMPOCheckQtyPending = 2908;
            }

            public class Repo
            {
                public static int Insert_MMPOInfo = 2909;
                public static int Select_MMPOInfo = 2910;
                public static int Select_MMPOListData_All = 2911;
                public static int Select_SearchMMPOListData_By = 2912;
                public static int Update_MMPOInfo = 2913;
                public static int Delete_MMPOInfo = 2914;
            }
        }

        public class UOMConversion
        {
            public class Service
            {
                public static int PostUOMConversionInfo = 3001;
                public static int PutUOMConversionInfo = 3002;
                public static int GetSearchListUOMConversion = 3003;
                public static int GetUOMConversionInfo = 3004;
                public static int GetListUOMConversion = 3005;
                public static int GetListUOMConversion_By = 3006;
                public static int GetListUOMConversion_By_MaterialCode = 3007;
                public static int DeleteUOMConversionInfo = 3008;
            }

            public class Repo
            {
                public static int Insert_UOMConversionInfo = 3009;
                public static int Select_UOMConversionInfo = 3010;
                public static int Select_UOMConversionListData_All = 3011;
                public static int Select_SearchUOMConversionListData_By = 3012;
                public static int Update_UOMConversionInfo = 3013;
                public static int Delete_UOMConversionInfo = 3014;
            }
        }

        public class SenderMapping
        {
            public class Service
            {
                public static int PostSenderMappingInfo = 3101;
                public static int PutSenderMappingInfo = 3102;
                public static int GetSearchListSenderMapping = 3103;
                public static int GetSenderMappingInfo = 3104;
                public static int GetListSenderMapping = 3105;
                public static int DeleteSenderMappingInfo = 3106;
            }

            public class Repo
            {
                public static int Insert_SenderMappingInfo = 3107;
                public static int Select_SenderMappingInfo = 3108;
                public static int Select_SenderMappingListData_All = 3109;
                public static int Select_SearchSenderMappingListData_By = 3110;
                public static int Update_SenderMappingInfo = 3111;
                public static int Delete_SenderMappingInfo = 3112;
            }
        }
    }
}
