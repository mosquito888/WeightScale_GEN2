using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.API.APIRepository;
using WeightScaleGen2.BGC.API.Common;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.DocumentPO;
using WeightScaleGen2.BGC.Models.ViewModels.ItemMaster;
using WeightScaleGen2.BGC.Models.ViewModels.MMPO;
using WeightScaleGen2.BGC.Models.ViewModels.Supplier;
using WeightScaleGen2.BGC.Models.ViewModels.UOMConversion;
using WeightScaleGen2.BGC.Models.ViewModels.WeightHistory;
using ILogger = WeightScaleGen2.BGC.API.Common.Logger.ILogger;

namespace WeightScaleGen2.BGC.API.APIServices
{
    public class MMPOAPIService : BaseAPIService
    {
        IDatabaseConnectionFactory _db;
        ISecurityCommon _securityCommon;
        ILogger _logger;
        private readonly IHttpContextAccessor _context;
        private readonly UserInfoModel _userInfo;
        private MMPORepository _MMPORepository;
        private DocumentPORepository _documentPORepository;
        private SupplierRepository _supplierRepository;
        private ItemMasterRepository _itemMasterRepository;
        private UOMConversionRepository _conversionRepository;
        private UOMConversionSapRepository _conversionSapRepository;
        private WeightHistoryRepository _weightHistoryRepository;
        private WeightOutRepository _weightOutRepository;

        public MMPOAPIService(
            IDatabaseConnectionFactory db,
            ISecurityCommon securityCommon,
            ILogger logger,
            IHttpContextAccessor context,
            UserInfoModel userInfo,
            MMPORepository MMPORepository,
            DocumentPORepository documentPORepository,
            SupplierRepository supplierRepository,
            ItemMasterRepository itemMasterRepository,
            UOMConversionRepository conversionRepository,
            UOMConversionSapRepository conversionSapRepository,
            WeightHistoryRepository weightHistoryRepository,
            WeightOutRepository weightOutRepository) : base(db, securityCommon)
        {
            _db = db;
            _securityCommon = securityCommon;
            _logger = logger;
            _context = context;
            _userInfo = _getUserInfo(_context.HttpContext.Session.GetString(Constants.Session.User)).Result;
            _MMPORepository = MMPORepository;
            _documentPORepository = documentPORepository;
            _supplierRepository = supplierRepository;
            _itemMasterRepository = itemMasterRepository;
            _conversionRepository = conversionRepository;
            _conversionSapRepository = conversionSapRepository;
            _weightHistoryRepository = weightHistoryRepository;
            _weightOutRepository = weightOutRepository;
        }

        public Task<ReturnList<ResultSearchMMPOViewModel>> GetSearchListMMPO(ParamSearchMMPOViewModel param)
        {
            var result = new ReturnList<ResultSearchMMPOViewModel>();
            try
            {
                var lsData = _MMPORepository.Select_SearchMMPOListData_By(param, _userInfo).Result;

                result.data = _initSearchMMPOListData(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.MMPO.Service.GetSearchListMMPO}");
                _logger.WriteError(errorCode: ErrorCodes.MMPO.Service.GetSearchListMMPO, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnObject<bool>> GetSearchMMPOCheckQtyPending(ParamSearchMMPOQtyPendingViewModel param)
        {
            var result = new ReturnObject<bool>();
            var cal = new ResultCalculateQtyPendingMMPOViewModel();
            try
            {
                var lsData = _MMPORepository.Select_SearchMMPOQtyPendingData(param, _userInfo).Result;
                if (lsData == null)
                {
                    var doc = _documentPORepository.Select_SearchDocumentPOListData_All(_userInfo).Result.Where(d => d.purchase_number == param.document_po && d.num_of_rec == Convert.ToInt32(param.line_number) && d.material_code == param.material_code).FirstOrDefault();
                    if (doc == null)
                    {
                        result.isCompleted = false;
                        result.message.Add("ไม่พบข้อมูลสินค้า และเลขที่ PO นี้");
                        return Task.FromResult(result);
                    }
                    else
                    {
                        cal.po_order = doc.order_qty;
                        cal.po_receive = doc.good_received;
                        cal.po_pending = doc.pending_qty;
                        cal.po_pending_all = doc.pending_qty_all;
                        cal.update_time = doc.modified_date.Value;
                        cal.date = cal.update_time.ToString("yyyy-MM-dd");
                        cal.time = cal.update_time.ToString("hh:mm:ss");
                        cal.last_update = Convert.ToDateTime(cal.date + " " + cal.time);
                        cal.allowance = doc.allowance;
                        cal.uom = doc.uom;
                        cal.sta = doc.status;
                        cal.deliver = doc.dlv_complete;
                        cal.num_of_rec = Convert.ToInt32(doc.num_of_rec);
                    }
                }
                else
                {
                    cal.po_order = lsData.OrderQty;
                    cal.po_receive = lsData.GoodReceived;
                    cal.po_pending = lsData.PendingQty;
                    cal.po_pending_all = lsData.PendingQtyAll;
                    cal.update_time = lsData.UpdatedOn;
                    cal.date = cal.update_time.ToString("yyyy-MM-dd");
                    cal.time = string.Format("{0:00}:{1:00}:{2:00}", lsData.UpdatedTime.Hours, lsData.UpdatedTime.Minutes, lsData.UpdatedTime.Seconds);
                    cal.last_update = Convert.ToDateTime(cal.date + " " + cal.time);
                    cal.allowance = lsData.Allowance;
                    cal.uom = lsData.UOM;
                    cal.sta = lsData.Status;
                    cal.deliver = lsData.DlvComplete;
                    cal.num_of_rec = lsData.NumOfRec;
                }
                var res = CalculateQtyPending(param, cal);

                result.data = res.Count() <= 2 ? true : false;
                result.isCompleted = res.Count() <= 2 ? true : false;
                result.message = res;
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.MMPO.Service.GetSearchMMPOCheckQtyPending}");
                _logger.WriteError(errorCode: ErrorCodes.MMPO.Service.GetSearchMMPOCheckQtyPending, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnObject<bool>> UpdateMMPOSapToDocumentPO(ParamSearchMMPOViewModel param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                _documentPORepository.Delete_DocumentPO_By_CompanyCode(_userInfo.comp_code, _userInfo);

                var lsData = _MMPORepository.Select_SearchMMPOListData_By(param, _userInfo).Result;
                if (lsData.Count > 0)
                {
                    foreach (var d in lsData)
                    {
                        ResultGetDocumentPOInfoViewModel res = new ResultGetDocumentPOInfoViewModel();
                        res.purchase_number = d.PurchaseNumber;
                        res.num_of_rec = d.NumOfRec;
                        res.company_code = d.CompanyCode;
                        res.plant = d.Plant;
                        res.storage_loc = d.StorageLoc;
                        res.status = d.Status;
                        res.vender_code = d.VenderCode;
                        res.vender_name = d.VenderName;
                        res.material_code = d.MaterialCode;
                        res.material_desc = d.MaterialDesc;
                        res.order_qty = d.OrderQty;
                        res.uom = d.UOM;
                        res.uom_in = d.UOM_IN;
                        res.good_received = d.GoodReceived;
                        res.pending_qty = d.PendingQty;
                        res.pending_qty_all = d.PendingQtyAll;
                        res.allowance = d.Allowance;
                        res.dlv_complete = d.DlvComplete;
                        res.created_by = d.CreatedBy;
                        res.created_date = d.CreatedOn;
                        res.created_time = d.CreatedTime;
                        res.modified_by = d.UpdatedBy;
                        res.modified_date = d.UpdatedOn;
                        res.modified_time = d.UpdatedTime;
                        _ = _documentPORepository.Insert_DocumentPOInfo(res, _userInfo);

                        var supData = _supplierRepository.Select_SupplierListData_All(_userInfo).Result.Where(s => s.supplier_code == int.Parse(d.VenderCode)).FirstOrDefault();
                        if (supData != null)
                        {
                            ResultGetSupplierInfoViewModel update = new ResultGetSupplierInfoViewModel();
                            update.supplier_code = supData.supplier_code;
                            update.supplier_name = d.VenderName;
                            update.remark_1 = supData.remark_1;
                            update.remark_2 = supData.remark_2;
                            update.status = "A";
                            _ = _supplierRepository.Update_SupplierInfo(update, _userInfo).Result;
                        }
                        else
                        {
                            ResultGetSupplierInfoViewModel insert = new ResultGetSupplierInfoViewModel();
                            insert.supplier_code = int.Parse(d.VenderCode);
                            insert.supplier_name = d.VenderName;
                            insert.remark_1 = null;
                            insert.remark_2 = null;
                            insert.status = "A";
                            _ = _supplierRepository.Insert_SupplierInfo(insert, _userInfo).Result;
                        }

                        var itemData = _itemMasterRepository.Select_ItemMasterListData_All(_userInfo).Result.Where(s => s.item_code == d.MaterialCode).FirstOrDefault();
                        if (itemData != null)
                        {
                            ResultGetItemMasterInfoViewModel update = new ResultGetItemMasterInfoViewModel();
                            update.item_code = itemData.item_code;
                            update.item_group = itemData.item_group;
                            update.item_name = d.MaterialDesc;
                            update.remark_1 = d.MaterialDesc;
                            update.remark_2 = itemData.remark_2;
                            update.status = "A";
                            _ = _itemMasterRepository.Update_ItemMasterInfo(update, _userInfo).Result;
                        }
                        else
                        {
                            ResultGetItemMasterInfoViewModel insert = new ResultGetItemMasterInfoViewModel();
                            insert.item_code = d.MaterialCode;
                            insert.item_name = d.MaterialDesc;
                            insert.item_group = "9";
                            insert.remark_1 = d.MaterialCode;
                            insert.remark_2 = null;
                            insert.status = "A";
                            _ = _itemMasterRepository.Insert_ItemMasterInfo(insert, _userInfo).Result;
                        }
                    }

                    var uomList = _conversionSapRepository.Select_SearchUOMConversionSapListData_All(_userInfo).Result;
                    if (uomList.Count > 0)
                    {
                        foreach (var uom in uomList)
                        {
                            var uomData = _conversionRepository.Select_SearchUOMConversionListData_All(_userInfo).Result.Where(u => u.base_uom == uom.BaseUOM && u.alter_uom == uom.AlterUOM && u.material_code == uom.MaterialCode).FirstOrDefault();
                            if (uomData != null)
                            {
                                ResultSearchUOMConversionViewModel update = new ResultSearchUOMConversionViewModel();
                                update.material_code = uom.MaterialCode;
                                update.alter_uom = uom.AlterUOM;
                                update.alter_uom_in = uom.BaseUOM;
                                update.alter_uom_in = uom.AlterUOM_IN;
                                update.base_uom_in = uom.BaseUOM_IN;
                                update.conv_weight_n = uom.ConvWeightN;
                                update.conv_weight_d = uom.ConvWeightD;
                                update.net_weight = uom.NetWeight;
                                update.gross_weight = uom.GrossWeight;
                                update.weight_unit = uom.WeightUnit;
                                update.created_by = uom.CreatedBy;
                                update.created_on = uom.CreatedOn;
                                update.created_time = uom.CreatedTime;
                                update.updated_by = uom.UpdatedBy;
                                update.updated_on = uom.UpdatedOn;
                                update.updated_time = uom.UpdatedTime;
                                _ = _conversionRepository.Update_UOMConversionInfo(update, _userInfo);
                            }
                            else
                            {
                                ResultSearchUOMConversionViewModel insert = new ResultSearchUOMConversionViewModel();
                                insert.material_code = uom.MaterialCode;
                                insert.alter_uom = uom.AlterUOM;
                                insert.base_uom = uom.BaseUOM;
                                insert.alter_uom_in = uom.AlterUOM_IN;
                                insert.base_uom_in = uom.BaseUOM_IN;
                                insert.conv_weight_n = uom.ConvWeightN;
                                insert.conv_weight_d = uom.ConvWeightD;
                                insert.net_weight = uom.NetWeight;
                                insert.gross_weight = uom.GrossWeight;
                                insert.weight_unit = uom.WeightUnit;
                                insert.created_by = uom.CreatedBy;
                                insert.created_on = uom.CreatedOn;
                                insert.created_time = uom.CreatedTime;
                                insert.updated_by = uom.UpdatedBy;
                                insert.updated_on = uom.UpdatedOn;
                                insert.updated_time = uom.UpdatedTime;
                                _ = _conversionRepository.Insert_UOMConversionInfo(insert, _userInfo);
                            }
                        }
                    }
                }
                else
                {
                    result.isCompleted = false;
                    result.message.Add($"Data not found");
                    return Task.FromResult(result);
                }

                result.data = true;
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Company.Service.PostInfo}");
                _logger.WriteError(errorCode: ErrorCodes.MMPO.Service.UpdateMMPOSapToDocumentPO, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public async Task<ReturnObject<bool>> UpdateMMPOSapToDocumentPOData()
        {
            var result = new ReturnObject<bool>();
            try
            {
                var lsData = await _MMPORepository.Select_SearchMMPOListData_By_CompanyCode(_userInfo.comp_code, _userInfo);

                if (lsData.Count > 0)
                {
                    var docPOList = await _documentPORepository.Select_SearchDocumentPOListData_All(_userInfo);
                    var supplierList = await _supplierRepository.Select_SupplierListData_All(_userInfo);
                    var itemList = await _itemMasterRepository.Select_ItemMasterListData_All(_userInfo);

                    foreach (var d in lsData)
                    {
                        var docPO = docPOList.FirstOrDefault(i => i.purchase_number == d.PurchaseNumber && i.num_of_rec == d.NumOfRec);

                        if (docPO != null)
                        {
                            var updateDocPO = new ResultGetDocumentPOInfoViewModel
                            {
                                purchase_number = docPO.purchase_number,
                                num_of_rec = d.NumOfRec,
                                company_code = d.CompanyCode,
                                plant = d.Plant,
                                storage_loc = d.StorageLoc,
                                status = d.Status,
                                vender_code = d.VenderCode,
                                vender_name = d.VenderName,
                                material_code = d.MaterialCode,
                                material_desc = d.MaterialDesc,
                                order_qty = d.OrderQty,
                                uom = d.UOM,
                                uom_in = d.UOM_IN,
                                good_received = d.GoodReceived,
                                pending_qty = d.PendingQty,
                                pending_qty_all = d.PendingQtyAll,
                                allowance = d.Allowance,
                                dlv_complete = d.DlvComplete,
                                created_by = d.CreatedBy,
                                created_date = d.CreatedOn,
                                created_time = d.CreatedTime,
                                modified_by = d.UpdatedBy,
                                modified_date = d.UpdatedOn,
                                modified_time = d.UpdatedTime
                            };
                            var res = await _documentPORepository.Update_DocumentPOInfo(updateDocPO, _userInfo);
                            if (res.is_success)
                            {
                                continue;
                            }
                        }
                        else
                        {
                            var insertDocPO = new ResultGetDocumentPOInfoViewModel
                            {
                                purchase_number = d.PurchaseNumber,
                                num_of_rec = d.NumOfRec,
                                company_code = d.CompanyCode,
                                plant = d.Plant,
                                storage_loc = d.StorageLoc,
                                status = d.Status,
                                vender_code = d.VenderCode,
                                vender_name = d.VenderName,
                                material_code = d.MaterialCode,
                                material_desc = d.MaterialDesc,
                                order_qty = d.OrderQty,
                                uom = d.UOM,
                                uom_in = d.UOM_IN,
                                good_received = d.GoodReceived,
                                pending_qty = d.PendingQty,
                                pending_qty_all = d.PendingQtyAll,
                                allowance = d.Allowance,
                                dlv_complete = d.DlvComplete,
                                created_by = d.CreatedBy,
                                created_date = d.CreatedOn,
                                created_time = d.CreatedTime,
                                modified_by = d.UpdatedBy,
                                modified_date = d.UpdatedOn,
                                modified_time = d.UpdatedTime
                            };
                            var res = await _documentPORepository.Insert_DocumentPOInfo(insertDocPO, _userInfo);
                            if (res.is_success)
                            {
                                continue;
                            }
                        }

                        var supplier = supplierList.FirstOrDefault(s => s.supplier_code == int.Parse(d.VenderCode));
                        if (supplier != null)
                        {
                            var update = new ResultGetSupplierInfoViewModel
                            {
                                supplier_code = supplier.supplier_code,
                                supplier_name = d.VenderName,
                                remark_1 = supplier.remark_1,
                                remark_2 = supplier.remark_2,
                                status = "A"
                            };
                            await _supplierRepository.Update_SupplierInfo(update, _userInfo);
                        }
                        else
                        {
                            var insert = new ResultGetSupplierInfoViewModel
                            {
                                supplier_code = int.Parse(d.VenderCode),
                                supplier_name = d.VenderName,
                                remark_1 = null,
                                remark_2 = null,
                                status = "A"
                            };
                            await _supplierRepository.Insert_SupplierInfo(insert, _userInfo);
                        }

                        var item = itemList.FirstOrDefault(s => s.item_code == d.MaterialCode);
                        if (item != null)
                        {
                            var update = new ResultGetItemMasterInfoViewModel
                            {
                                item_code = item.item_code,
                                item_group = item.item_group,
                                item_name = d.MaterialDesc,
                                remark_1 = d.MaterialDesc,
                                remark_2 = item.remark_2,
                                status = "A"
                            };
                            await _itemMasterRepository.Update_ItemMasterInfo(update, _userInfo);
                        }
                        else
                        {
                            var insert = new ResultGetItemMasterInfoViewModel
                            {
                                item_code = d.MaterialCode,
                                item_name = d.MaterialDesc,
                                item_group = "9",
                                remark_1 = d.MaterialCode,
                                remark_2 = null,
                                status = "A"
                            };
                            await _itemMasterRepository.Insert_ItemMasterInfo(insert, _userInfo);
                        }
                    }

                    result.data = true;
                    result.isCompleted = true;
                    result.message.Add(Constants.Result.Success);
                }
                else
                {
                    result.isCompleted = false;
                    result.message.Add("Data not found");
                }
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Company.Service.PostInfo}");
                _logger.WriteError(errorCode: ErrorCodes.MMPO.Service.UpdateMMPOSapToDocumentPO, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }

            return result;
        }

        private List<ResultSearchMMPOViewModel> _initSearchMMPOListData(List<MMPOData> lsMMPO)
        {
            List<ResultSearchMMPOViewModel> result = new List<ResultSearchMMPOViewModel>();

            foreach (MMPOData objMMPO in lsMMPO)
            {
                result.Add(new ResultSearchMMPOViewModel
                {
                    purchase_number = objMMPO.PurchaseNumber,
                    num_of_rec = objMMPO.NumOfRec,
                    company_code = objMMPO.CompanyCode,
                    plant = objMMPO.Plant,
                    storage_loc = objMMPO.StorageLoc,
                    status = objMMPO.Status,
                    vender_code = objMMPO.VenderCode,
                    vender_name = objMMPO.VenderName,
                    material_code = objMMPO.MaterialCode,
                    material_desc = objMMPO.MaterialDesc,
                    order_qty = objMMPO.OrderQty,
                    uom = objMMPO.UOM,
                    uom_in = objMMPO.UOM_IN,
                    good_received = objMMPO.GoodReceived,
                    pending_qty = objMMPO.PendingQty,
                    pending_qty_all = objMMPO.PendingQtyAll,
                    allowance = objMMPO.Allowance,
                    dlv_complete = objMMPO.DlvComplete,
                    created_by = objMMPO.CreatedBy,
                    created_on = objMMPO.CreatedOn,
                    created_time = objMMPO.CreatedTime,
                    updated_by = objMMPO.UpdatedBy,
                    updated_on = objMMPO.UpdatedOn,
                    updated_time = objMMPO.UpdatedTime,
                    total_record = objMMPO.total_record,
                });
            }

            return result;
        }

        private List<string> CalculateQtyPending(ParamSearchMMPOQtyPendingViewModel param, ResultCalculateQtyPendingMMPOViewModel data)
        {
            var result = new List<string>();
            try
            {
                var calWHis = _weightHistoryRepository.Select_SumQtyWeightHistory_By(new ParamGetSumQtyWeightHistoryViewModel { document_po = param.document_po, item_code = param.material_code, line_number = param.line_number, date = data.last_update }, _userInfo).Result;
                var calW = _weightOutRepository.Select_SumQtyWeightOut_By(new ParamGetSumQtyWeightHistoryViewModel { document_po = param.document_po, item_code = param.material_code, line_number = param.line_number, date = data.last_update }, _userInfo).Result;

                var sum = calW + calWHis;
                result.Add("PO เปิดจำนวน " + data.po_order.ToString("#,0.00") + " " + data.uom + " รับไปแล้ว " + data.po_receive.ToString("#,0.00") + " " + data.uom + " รับได้อีก " + data.po_pending.ToString("#,0.00") + " " + data.uom);
                result.Add("ยังไม่ได้ออก RR " + sum.ToString("#,##.##") + " " + data.uom + "สุทธิเหลือรับ " + (Convert.ToDecimal(data.po_pending) - Convert.ToDecimal(sum)).ToString("#,##.##") + " " + data.uom);

                double qtyReceive = Convert.ToDouble(sum) + Convert.ToDouble(data.po_receive);
                if (Convert.ToDouble(data.po_order) < qtyReceive && data.uom == "KG")
                {
                    result.Add("PO ไม่สามารถรับของได้เนื่องจากรับครบแล้ว");
                }
                else if (Convert.ToDouble(data.po_pending_all + data.po_receive) < qtyReceive && data.uom == "KG")
                {
                    result.Add("PO ไม่สามารถรับของได้เนื่องจากเกินจาก Allowance " + data.allowance.ToString("#,0.00") + "%");
                }
                else if (data.sta == "L" || data.sta == "X")
                {
                    result.Add("PO ปิดไปแล้ว");
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Add(ex.Message);
                return result;
            }
        }
    }
}
