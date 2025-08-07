using System;
using System.ComponentModel;

namespace WeightScaleGen2.BGC.Models.ViewModels.Base
{
    public partial class BaseConst
    {
        public enum MENU_DEFINITION
        {
            NOTSET = 0,
            HOME = 1,
            DOCUMENT_PO = 2,
            CONFIRM_PROCESS = 3,
            WEIGHT_MASTER = 4,
            RETURN_DATA = 5,
            MASTER_DATA = 6,
            ITEM_MASTER = 7,
            ITEM_MASTER_RELATION = 8,
            SUPPLIER = 9,
            SENDER = 10,
            WEIGHT_HISTORY = 11,
            SYSTEM = 12,
            COMPANY = 13,
            PLANT = 14,
            DEPARTMENT = 15,
            LOG = 16,
            REPORT = 17,
            WEIGHT_SUMMARY_DAY = 18,
            WEIGHT_DAILY = 19,
            WEIGHT_COMPARE = 20,
            USER_MANAGEMENT = 21,
            EMPLOYEE = 22,
            ROLE = 23,
            USER = 24,
            WEIGHT_IN = 25,
            WEIGHT_OUT = 26,
        }

        public enum API
        {
            [Description("Kerry")]
            KERRY = 1,
            [Description("Thai Post")]
            THAI_POST = 2,
            [Description("Ninja Van")]
            NINJA_VAN = 3,
            [Description("DHL")]
            DHL = 4,
            [Description("ขนส่ง COM7")]
            SHIPMENT_COM7 = 5,
        }
    }

    public static class EnumExtensionMethods
    {
        public static string GetEnumDescription(this Enum enumValue)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            var descriptionAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return descriptionAttributes.Length > 0 ? descriptionAttributes[0].Description : enumValue.ToString();
        }
    }
}