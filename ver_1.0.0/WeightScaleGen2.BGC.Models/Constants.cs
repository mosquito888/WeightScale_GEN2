namespace WeightScaleGen2.BGC.Models
{
    public static class Constants
    {
        public class ConditionDateSO
        {
            public const string FindFinishDate = "CDTSO01";
            public const string FindStartDate = "CDTSO02";
            public const string FindRequestDate = "CDTSO03";
            public const string FindRecommendStartDate = "CDTSO04";
            public const string FindPackingDate = "CDTSO05";
        }

        public class ProcessingOrder
        {
            public const string Anodize = "A";
            public const string PowderCoating = "P";
            public const string MillFinish = "M";
        }

        public class Station
        {
            public const string SaleOrder = "ST00";
            public const string MillFinish = "ST01";
            public const string Oven = "ST02";
            public const string Anodize = "ST03";
            public const string PowderCoating = "ST04";
            public const string Packing = "ST05";
            public const string Other = "ST06";
        }

        public class OrderStatus
        {
            public const string Active = "ACT";
            public const string InActive = "INT";
            public const string ClosedOrder = "CLS";

            //public const string SuccessMF = "CMF";
            //public const string CompletedBeing = "CBE";
            //public const string CompletedOrder = "COR";
            //public const string Reject = "REJ";
            //public const string Delete = "DEL";
        }

        public class TransactionStatus
        {
            public const string Manual = "MAN";
            public const string Import = "IMP";
            public const string ImportUpdate = "UPD";

            public const string Edit = "EDT";
            public const string Remove = "REM";
            public const string MoveOrder = "MOV";
            public const string DeleteOrderProduction = "DOP";

            public const string Delay = "DLA";

            public const string AddToDie = "DIE";
            public const string AddToProduction = "PRO";
            public const string AddToPending = "PEN";
            public const string AddToPostProcess = "POS";

            public const string DieTest = "DIT";
            public const string DieTestUpdate = "DUP";

            public const string UpdateDieStatusAutoAddToDie = "ATD";
        }

        public class DataTable
        {
            public class UpdateMachineSeries
            {
                public const string DieNo = "DieNo";
                public const string DieShape = "DieShape";
                public const string DieSeries = "DieSeries";
                public const string Status = "Status";
            }

            public class UploadPlan
            {
                public const string DocumentNumber = "Document Number";
                public const string SoldToParty = "Sold-to Party";
                public const string SoldToName1 = "Sold-to Name1";
                public const string MaterialNumber = "Material Number";
                public const string MaterialDescription = "Material Description";
                public const string ReqDeliveryDate = "Req.Delivery Date";
                public const string ProductionQty_PC = "Production Qty(PC)";
                public const string Thikness = "thikness";
                public const string MaterialDescription8 = "Material Description8";
                public const string RequiredQTY_PC = "Required QTY(PC)";
                public const string DeliveryQTY_PC = "Delivery QTY(PC)";
                public const string OpenQTY_PC = "Open QTY(PC)";
                public const string StockQTY_PC = "Stock QTY(PC)";
                public const string StockQTY_Ton = "Stock QTY(Ton)";
                public const string ItemsNumber = "Items Number";
                public const string OverallProcessing_S = "Overall Processing S";
                public const string ReasonForRejection = "Reason for Rejection";
                public const string PONumber = "PO number";
                public const string PODate = "PO Date";
                public const string CreateDate = "Create Date";
                public const string SoldToName2 = "Sold-to Name2";
                public const string Reserve = "Reserve";
                public const string NetWeight_KGUnit = "Net Weight(KG/Unit)";
                public const string DateSystem = "Date System";
                public const string CollectiveNo = "Collective No.";
                public const string OpenQTY_Ton = "Open QTY(Ton)";
            }

            public class UploadPlanFixValue
            {
                public const int recomment_start_day = 14;
            }
        }

        public class FileType
        {
            public const string XLSX = ".xlsx";
        }

        public class ActivityStatus
        {
            public const string New = "N";
            public const string Update = "U";
        }

        public class MachineType
        {
            public const string Die = "mc01";
        }

        public class MachineStatus
        {
            public const string OK = "01";
            public const string NG = "02";
            public const string RW = "03";
        }

        public class MachineSeriesStatus
        {
            public const string OK = "OK";
        }

        public class DocType
        {
            public const string SaleOrder = "SO";
            public const string SaleOrderManual = "PO";
            public const string SaleOrderDieTest = "DE";

            public const string Received = "R";
            public const string Maintenance = "M";
            public const string Material = "T";
        }

        public class Session
        {
            public const string Common = "Common";
            public const string UnknownUser = "Unknown";
            public const string User = "Username";
            public const string IpAddress = "IpAddress";
        }

        public class LogsPattern
        {
            public const string Inquiry = "INQUIRY|Controller:'{0}'|Action:'{1}'|Value:'{2}'";
            public const string Insert = "CREATE|Controller:'{0}'|Action:'{1}'|Value:'{2}'";
            public const string Update = "UPDATE|Controller:'{0}'|Action:'{1}'|Value:'{2}'";
            public const string Delete = "DELETE|Controller:'{0}'|Action:'{1}'|Value:'{2}'";
            public const string Error = "ERROR|Controller:'{0}'|Action:'{1}|Value:'{2}'";
            public const string GenReport = "GEN-REPORT|Controller:'{0}'|Action:'{1}'|Value:'{2}'";
            public const string GenExport = "GEN-EXPORT|Controller:'{0}'|Action:'{1}'|Value:'{2}'";
        }

        public class Message
        {
            public const string NotFoundPermission = "You don't have permission to access this page.";
            public const string ValidateDataInvalid = "Validate Data Invalid.";
            public const string DuplicatedData = "Data duplicated in a database.";
            public const string AlreadyUsed = "Can't be delete because is already used.";
            public const string DataNotFound = "Data not found.";
            public const string NotMatchDieSeriesInDatabase = "Not match die series in database.";
            public const string NotMatchDieSpecInDatabase = "Not match die spec in database.";
            public const string DataNotFoundLastSaleOrder = "Data not found last sale order by item fg.";
            public const string PleaseSpecifyDateExportReport = "Please specify the date of export report.";
            public const string LimitMaxPeriodExportReport = "Limit max period 180 day.";
            public const string StartDateNotMoreThanEndDate = "Please start date not more than end date";

            public const string Invalid = "Invalid.";
            public const string Success = "Success.";
        }

        public class RunningDoc
        {
            public const string Auto = "AUTO GENERATE";
        }

        public class Mode
        {
            public const string Created = "C";
            public const string Updated = "E";
            public const string Deleted = "D";
        }

        public class Result
        {
            public const string Success = "success";
            public const string Error = "error";
            public const string Invalid = "invalid";
        }

        public class Action
        {
            public const string Created = "created";
            public const string Edit = "edit";
            public const string View = "view";
            public const string Print = "print";
            public const string Deleted = "deleted";
        }

        public struct System
        {
            public class Type
            {
                public const string MaxPeriod = "MaxPeriod";
            }

            public class Code
            {
                public const string S001 = "S001"; // max period get report
            }

        }

        public struct SelectOption
        {
            public static string SelectAll = "-- Select All --";
            public static string SelectOne = "-- Please Select One --";
            public static string SelectActive = "Active";
            public static string SelectInActive = "InActive";
        }

        public struct MasterType
        {
            public static string AssetType = "asset_type";
            public static string ReceivedType = "received_type";
            public static string ReportType = "report_type";
            public static string GroupType = "group_type";
            public static string ObjectType = "object_type";
            public static string UnitType = "unit_type";
            public static string PayType = "pay_type";
            public static string Material = "material";
            public static string FileSystem = "file_system";
            public static string ShapeType = "shape_type";
            public static string MachineStatus = "machine_status";
            public static string SeriesTypeCode = "series_type_code";
            public static string SeriesType = "series_type";
            public static string SeriesStatus = "series_status";
            public static string PlanOrder = "plan_order";
            public static string Station = "station";
            public static string OrderStatus = "order_status";
            public static string ConditionDateSO = "condition_date_so";
            public static string WorkShift = "work_shift";
            public static string AcBilletType = "ac_billet_type";
            public static string DieReasonCodeFaill = "reason_faill";
        }

        public struct SessionKey
        {
            public static string ConfirmMill = "ConfirmMill";
            public static string UpdateMill = "UpdateMill";

            public static string UserLoginInfo = "UserLoginInfo";
        }
    }
}
