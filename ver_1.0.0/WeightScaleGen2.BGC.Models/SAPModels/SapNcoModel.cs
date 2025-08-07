namespace WeightScaleGen2.BGC.Models.SAPModels
{
    public class SapNcoModel
    {
        public string ZWGDOC { get; set; }
        public int ZWGDOC_SEQ { get; set; }
        public string GR_TYPE { get; set; }
        public string DOC_DATE { get; set; }
        public string PSTNG_DATE { get; set; }
        public string REF_DOC_NO { get; set; }
        public string GOODSMVT_CODE { get; set; }
        public string MATERIAL { get; set; }
        public string PLANT { get; set; }
        public string STGE_LOC { get; set; }
        public string STCK_TYPE { get; set; }
        public string ITEM_TEXT { get; set; }
        public string PO_NO { get; set; }
        public int PO_ITEM { get; set; }
        public string TRUCK_NO { get; set; }
        public decimal WEIGHT_IN { get; set; }
        public decimal WEIGHT_OUT { get; set; }
        public decimal WEIGHT_REC { get; set; }
        public decimal VEND_WEIGHT { get; set; }
        public decimal REJ_WEIGHT { get; set; }
        public string WEIGHT_UNIT { get; set; }
        public string P_STDATE { get; set; }
        public string P_ENDATE { get; set; }
        public string ZPERM { get; set; }
        public string REV_MJAHR { get; set; }
        public string REV_MBLNR { get; set; }
    }
}
