using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NuGet.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.API.APIRepository.Interface;
using WeightScaleGen2.BGC.API.Common;
using WeightScaleGen2.BGC.API.Common.Logger;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.DBModelsEF;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.DocumentPO;

namespace WeightScaleGen2.BGC.API.APIRepository
{
    public class DocumentPORepository : IDocumentPORepository
    {
        IDatabaseConnectionFactory _db;
        ILogger _logger;
        IConfiguration _configuration;
        private readonly IHttpContextAccessor _context;
        private readonly ApplicationContext _applicationContext;
        private readonly string _connectionStringContext;

        public DocumentPORepository(
            IDatabaseConnectionFactory db,
            ILogger logger,
            IConfiguration configuration,
            IHttpContextAccessor context,
            ApplicationContext applicationContext)
        {
            _db = db;
            _logger = logger;
            _configuration = configuration;
            _context = context;
            _applicationContext = applicationContext;
            _applicationContext.Database.SetConnectionString(_configuration.GetConnectionString("DBConnection"));
            _connectionStringContext = _configuration.GetConnectionString("DBConnection");
        }

        public async Task<MessageReport> Insert_DocumentPOInfo(ResultGetDocumentPOInfoViewModel param, UserInfoModel userInfo)
        {
            using var dbContext = new ApplicationContext();
            dbContext.Database.SetConnectionString(_connectionStringContext);
            await dbContext.Database.OpenConnectionAsync();
            using var trans = dbContext.Database.BeginTransaction();

            var toDay = DateTimeServer.Now();
            var result = new MessageReport(false, "ERROR!");

            try
            {
                TsDocumentPo itm = new TsDocumentPo
                {
                    PurchaseNumber = param.purchase_number,
                    NumOfRec = param.num_of_rec,
                    CompanyCode = param.company_code,
                    Plant = param.plant,
                    StorageLoc = param.storage_loc,
                    Status = param.status,
                    VenderCode = param.vender_code,
                    VenderName = param.vender_name,
                    MaterialCode = param.material_code,
                    MaterialDesc = param.material_desc,
                    OrderQty = param.order_qty,
                    Uom = param.uom,
                    UomIn = param.uom_in,
                    GoodReceived = param.good_received,
                    PendingQty = param.pending_qty,
                    PendingQtyAll = param.pending_qty_all,
                    Allowance = param.allowance,
                    DlvComplete = param.dlv_complete,
                    CreatedBy = param.created_by,
                    CreatedDate = DateOnly.FromDateTime(param.created_date),
                    CreatedTime = TimeOnly.FromTimeSpan(param.created_time),
                    ModifiedBy = param.modified_by,
                    ModifiedDate = DateOnly.FromDateTime(param.modified_date.Value),
                    ModifiedTime = TimeOnly.FromTimeSpan(param.modified_time),
                };

                await dbContext.TsDocumentPos.AddAsync(itm);
                await dbContext.SaveChangesAsync();
                await trans.CommitAsync();
                result = new MessageReport(true, string.Format("Message: {0}", Constants.Result.Success));
                _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Insert, this.GetType().Name, "Insert_DocumentPOInfo", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                result = new MessageReport(false, string.Format("Error Code: {0} - Details: {1}", ErrorCodes.DocumentPO.Repo.Insert_DocumentPOInfo, ex.Message));
                _logger.WriteError(errorCode: ErrorCodes.DocumentPO.Repo.Insert_DocumentPOInfo, errorMessage: ex.Message, additionalInfo: result.message, exception: ex, user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            finally
            {
                await trans.DisposeAsync();
                await dbContext.DisposeAsync();
            }

            return result;
        }

        public async Task<List<DocumentPOData>> Select_SearchDocumentPOListData_All(UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@comp_code", userInfo.comp_code);
            p.Add("@plant_code", userInfo.plant_code);

            var query = @"EXEC sp_select_sy_document_po_all @comp_code = @comp_code, @plant_code = @plant_code";

            var datas = conn.Query<DocumentPOData>(query, p).ToList();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_SearchDocumentPOListData_All", null), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<List<DocumentPOData>> Select_SearchDocumentPOListData_By(ParamSearchDocumentPOViewModel param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            int page = (param.start / param.length) + 1;
            var _offset = (page - 1) * param.length;
            var _pagesize = param.length;

            var p = new DynamicParameters();
            p.Add("@Offset", _offset);
            p.Add("@PageSize", _pagesize);
            if (param.start_date.HasValue)
            {
                p.Add("@start_date", param.start_date.Value.ToString("yyyy-MM-dd"));
            }
            else
            {
                p.Add("@start_date", null);
            }

            if (param.end_date.HasValue)
            {
                p.Add("@end_date", param.end_date.Value.ToString("yyyy-MM-dd"));
            }
            else
            {
                p.Add("@end_date", null);
            }
            p.Add("@comp_code", userInfo.comp_code);
            p.Add("@plant_code", userInfo.plant_code);

            var query = @"EXEC sp_select_sy_document_po_by
                                 @Offset = @Offset
                                ,@PageSize = @PageSize
                                ,@start_date = @start_date
                                ,@end_date = @end_date
                                ,@comp_code = @comp_code
                                ,@plant_code = @plant_code
                        ";

            var datas = conn.Query<DocumentPOData>(query, p).ToList();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_SearchDocumentPOListData_By", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<DocumentPOData> Select_SearchDocumentPOListData_By_PurchaseNumber(string purchase_number, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@purchase_number", purchase_number);
            p.Add("@num_of_rec", 10);
            p.Add("@comp_code", userInfo.comp_code);
            p.Add("@plant_code", userInfo.plant_code);

            var query = @"EXEC sp_select_sy_document_po_by_purchase_number
                                 @purchase_number = @purchase_number
                                ,@num_of_rec = @num_of_rec
                                ,@comp_code = @comp_code
                                ,@plant_code = @plant_code
                        ";

            var datas = conn.Query<DocumentPOData>(query, p).FirstOrDefault();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_SearchDocumentPOListData_By_PurchaseNumber", purchase_number.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async void Delete_DocumentPO_By_CompanyCode(string CompanyCode, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var result = new MessageReport(false, "ERROR!");

            var p = new DynamicParameters();
            p.Add("@company_code", CompanyCode);

            var query = "DELETE FROM ts_document_po WHERE [company_code] = @company_code";

            using (var trans = conn.BeginTransaction())
            {
                conn.Execute(query, p, trans);
                trans.Commit();
            }

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Delete, this.GetType().Name, "Delete_DocumentPO_By_CompanyCode", CompanyCode.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
        }

        public async Task<MessageReport> Update_DocumentPOInfo(ResultGetDocumentPOInfoViewModel param, UserInfoModel userInfo)
        {
            using var dbContext = new ApplicationContext();
            dbContext.Database.SetConnectionString(_connectionStringContext);
            await dbContext.Database.OpenConnectionAsync();
            using var trans = dbContext.Database.BeginTransaction();

            var toDay = DateTimeServer.Now();
            var result = new MessageReport(false, "ERROR!");

            try
            {
                TsDocumentPo docPOInfo = dbContext.TsDocumentPos.Where(i => i.PurchaseNumber == param.purchase_number && i.NumOfRec == param.num_of_rec).FirstOrDefault();
                if (docPOInfo != null)
                {
                    docPOInfo.PurchaseNumber = param.purchase_number;
                    docPOInfo.NumOfRec = param.num_of_rec;
                    docPOInfo.CompanyCode = param.company_code;
                    docPOInfo.Plant = param.plant;
                    docPOInfo.StorageLoc = param.storage_loc;
                    docPOInfo.Status = param.status;
                    docPOInfo.VenderCode = param.vender_code;
                    docPOInfo.VenderName = param.vender_name;
                    docPOInfo.MaterialCode = param.material_code;
                    docPOInfo.MaterialDesc = param.material_desc;
                    docPOInfo.OrderQty = param.order_qty;
                    docPOInfo.Uom = param.uom;
                    docPOInfo.UomIn = param.uom_in;
                    docPOInfo.GoodReceived = param.good_received;
                    docPOInfo.PendingQty = param.pending_qty;
                    docPOInfo.PendingQtyAll = param.pending_qty_all;
                    docPOInfo.Allowance = param.allowance;
                    docPOInfo.DlvComplete = param.dlv_complete;
                    docPOInfo.CreatedBy = param.created_by;
                    docPOInfo.CreatedDate = DateOnly.FromDateTime(param.created_date);
                    docPOInfo.CreatedTime = TimeOnly.FromTimeSpan(param.created_time);
                    docPOInfo.ModifiedBy = param.modified_by;
                    docPOInfo.ModifiedDate = DateOnly.FromDateTime(param.modified_date.Value);
                    docPOInfo.ModifiedTime = TimeOnly.FromTimeSpan(param.modified_time);
                }

                await dbContext.SaveChangesAsync();
                await trans.CommitAsync();
                result = new MessageReport(true, string.Format("Message: {0}", Constants.Result.Success));
                _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Update, this.GetType().Name, "Update_DocumentPOInfo", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                result = new MessageReport(false, string.Format("Error Code: {0} - Details: {1}", ErrorCodes.DocumentPO.Repo.Update_DocumentPOInfo, ex.Message));
                _logger.WriteError(errorCode: ErrorCodes.DocumentPO.Repo.Update_DocumentPOInfo, errorMessage: ex.Message, additionalInfo: result.message, exception: ex, user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            finally
            {
                await trans.DisposeAsync();
                await dbContext.DisposeAsync();
            }

            return result;
        }
    }
}
