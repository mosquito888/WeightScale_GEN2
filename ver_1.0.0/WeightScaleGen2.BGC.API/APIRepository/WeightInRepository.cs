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
using WeightScaleGen2.BGC.Models.ViewModels.WeightIn;

namespace WeightScaleGen2.BGC.API.APIRepository
{
    public class WeightInRepository : IWeightInRepository
    {
        IDatabaseConnectionFactory _db;
        ILogger _logger;
        IConfiguration _configuration;
        private readonly IHttpContextAccessor _context;
        private readonly ApplicationContext _applicationContext;
        private readonly string _connectionStringContext;

        public WeightInRepository(
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

        public async Task<WeightInData> Select_WeightInInfo(ParamWeightInInfo param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@id", param.id);
            p.Add("@comp_code", userInfo.comp_code);
            p.Add("@plant_code", userInfo.plant_code);

            var query = @"EXEC sp_select_weight_in_by_id
                                @id = @id
                               ,@comp_code = @comp_code
                               ,@plant_code = @plant_code
            ";

            var datas = conn.Query<WeightInData>(query, p).FirstOrDefault();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_WeightInInfo", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<WeightInData> Select_WeightInInfoByCarLicense(ParamWeightInInfo param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@car_license", param.car_license);
            p.Add("@company_code", param.company_code);
            p.Add("@status", param.status);
            p.Add("@comp_code", userInfo.comp_code);
            p.Add("@plant_code", userInfo.plant_code);

            var query = @"EXEC sp_select_weight_in_by_car_license
                                @car_license = @car_license,
                                @company_code = @company_code,
                                @status = @status,
                                @comp_code = @comp_code,
                                @plant_code = @plant_code
            ";

            var datas = conn.Query<WeightInData>(query, p).FirstOrDefault();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_WeightInInfoByCarLicense", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        //public async Task<List<ItemMasterData>> Select_ItemMasterListData_All(UserInfoModel userInfo)
        //{
        //    using var conn = await _db.CreateConnectionAsync();

        //    var query = @"EXEC sp_select_sy_item_master_all";

        //    var datas = conn.Query<ItemMasterData>(query).ToList();

        //    conn.Close();

        //    _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_ItemMasterListData_All", null), user: _context.HttpContext.Session.GetString(Constants.Session.User));

        //    return datas;
        //}

        public async Task<MessageReport> Insert_WeightInInfo(ResultGetWeightInInfoViewModel param, UserInfoModel userInfo)
        {
            using var dbContext = new ApplicationContext();
            dbContext.Database.SetConnectionString(_connectionStringContext);
            await dbContext.Database.OpenConnectionAsync();
            using var trans = dbContext.Database.BeginTransaction();

            var toDay = DateTimeServer.Now();
            var result = new MessageReport(false, "ERROR!");

            try
            {
                TsWeightIn itm = new TsWeightIn
                {
                    CarType = param.car_type,
                    WeightInNo = param.weight_in_no,
                    CarLicense = param.car_license,
                    SupplierCode = param.supplier_code,
                    DocumentRef = param.document_ref,
                    DocumentPo = param.document_po,
                    DocTypePo = param.doc_type_po,
                    WeightIn = param.weight_in,
                    LineNumber = param.line_number,
                    ItemCode = param.item_code,
                    ItemName = param.item_name,
                    WeightInType = "ชั่งเข้า",
                    Date = toDay,
                    Status = "Active",
                    UserId = userInfo.user_id.ToString(),
                    Reprint = 0,
                    Company = param.company,
                    DocSend = param.doc_send,
                    DocStart = param.doc_start,
                    DocStop = param.doc_stop,
                    PlantCode = userInfo.plant_code,
                };

                await dbContext.TsWeightIns.AddAsync(itm);
                await dbContext.SaveChangesAsync();
                await trans.CommitAsync();
                result = new MessageReport(true, string.Format("Message: {0}", Constants.Result.Success));
                _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Insert, this.GetType().Name, "Insert_WeightInInfo", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                result = new MessageReport(false, string.Format("Error Code: {0} - Details: {1}", ErrorCodes.WeightIn.Repo.Insert_WeightInInfo, ex.Message));
                _logger.WriteError(errorCode: ErrorCodes.WeightIn.Repo.Insert_WeightInInfo, errorMessage: ex.Message, additionalInfo: result.message, exception: ex, user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            finally
            {
                await trans.DisposeAsync();
                await dbContext.DisposeAsync();
            }

            return result;
        }

        public async Task<List<WeightInData>> Select_SearchWeightInListData_By(ParamSearchWeightInViewModel param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            int page = (param.start / param.length) + 1;
            var _offset = (page - 1) * param.length;
            var _pagesize = param.length;

            var p = new DynamicParameters();
            p.Add("@Offset", _offset);
            p.Add("@PageSize", _pagesize);
            p.Add("@weight_in_no", param.weight_in_no);
            p.Add("@comp_code", userInfo.comp_code);
            p.Add("@plant_code", userInfo.plant_code);

            var query = @"EXEC sp_select_weight_in_by 
                                 @Offset = @Offset
                                ,@PageSize = @PageSize
                                ,@weight_in_no = @weight_in_no
                                ,@comp_code = @comp_code
                                ,@plant_code = @plant_code
                        ";

            var datas = conn.Query<WeightInData>(query, p).ToList();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_SearchWeightInListData_By", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<MessageReport> Update_WeightInInfo(ResultGetWeightInInfoViewModel param, UserInfoModel userInfo)
        {
            using var dbContext = new ApplicationContext();
            dbContext.Database.SetConnectionString(_connectionStringContext);
            await dbContext.Database.OpenConnectionAsync();
            using var trans = dbContext.Database.BeginTransaction();

            var toDay = DateTimeServer.Now();
            var result = new MessageReport(false, "ERROR!");

            try
            {
                TsWeightIn itm = dbContext.TsWeightIns.Where(i => i.WeightInNo == param.weight_in_no).FirstOrDefault();
                itm.SupplierCode = param.supplier_code;
                itm.ItemCode = param.item_code;
                itm.ItemName = param.item_name;
                itm.CarLicense = param.car_license;
                itm.CarType = param.car_type;
                itm.DocTypePo = param.doc_type_po;
                itm.LineNumber = param.line_number;
                itm.DocumentRef = param.document_ref;
                itm.DocumentPo = param.document_po;
                itm.DocSend = param.doc_send;
                itm.UserEdit3 = param.user_edit_2;
                itm.UserEdit2 = param.user_edit_1;
                itm.UserEdit1 = userInfo != null ? userInfo.username : "admin";
                itm.PlantCode = userInfo.plant_code;

                await dbContext.SaveChangesAsync();
                await trans.CommitAsync();
                result = new MessageReport(true, string.Format("Message: {0}", Constants.Result.Success));
                _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Update, this.GetType().Name, "Update_WeightInInfo", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                result = new MessageReport(false, string.Format("Error Code: {0} - Details: {1}", ErrorCodes.WeightIn.Repo.Update_WeightInInfo, ex.Message));
                _logger.WriteError(errorCode: ErrorCodes.WeightIn.Repo.Update_WeightInInfo, errorMessage: ex.Message, additionalInfo: result.message, exception: ex, user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            finally
            {
                await trans.DisposeAsync();
                await dbContext.DisposeAsync();
            }

            return result;
        }

        public async Task<MessageReport> Update_WeightInStatus(ResultGetWeightInInfoViewModel param, UserInfoModel userInfo)
        {
            using var dbContext = new ApplicationContext();
            dbContext.Database.SetConnectionString(_connectionStringContext);
            await dbContext.Database.OpenConnectionAsync();
            using var trans = dbContext.Database.BeginTransaction();

            var toDay = DateTimeServer.Now();
            var result = new MessageReport(false, "ERROR!");

            try
            {
                TsWeightIn itm = dbContext.TsWeightIns.Where(i => i.WeightInNo == param.weight_in_no).FirstOrDefault();
                itm.Status = param.status;

                await dbContext.SaveChangesAsync();
                await trans.CommitAsync();
                result = new MessageReport(true, string.Format("Message: {0}", Constants.Result.Success));
                _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Update, this.GetType().Name, "Update_WeightInStatus", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                result = new MessageReport(false, string.Format("Error Code: {0} - Details: {1}", ErrorCodes.WeightIn.Repo.Update_WeightInInfo, ex.Message));
                _logger.WriteError(errorCode: ErrorCodes.WeightIn.Repo.Update_WeightInInfo, errorMessage: ex.Message, additionalInfo: result.message, exception: ex, user: _context.HttpContext.Session.GetString(Constants.Session.User));
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
