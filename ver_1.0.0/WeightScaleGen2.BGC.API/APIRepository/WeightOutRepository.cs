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
using WeightScaleGen2.BGC.Models.ViewModels.WeightHistory;
using WeightScaleGen2.BGC.Models.ViewModels.WeightOut;

namespace WeightScaleGen2.BGC.API.APIRepository
{
    public class WeightOutRepository : IWeightOutRepository
    {
        IDatabaseConnectionFactory _db;
        ILogger _logger;
        IConfiguration _configuration;
        private readonly IHttpContextAccessor _context;
        private readonly ApplicationContext _applicationContext;
        private readonly string _connectionStringContext;

        public WeightOutRepository(
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

        public async Task<WeightOutData> Select_WeightOutInfo(ParamWeightOutInfo param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@id", param.id);

            var query = @"EXEC sp_select_weight_out_by_id
                                @id = @id
            ";

            var datas = conn.Query<WeightOutData>(query, p).FirstOrDefault();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_WeightOutInfo", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<WeightOutData> Select_WeightOutInfoByCarLicense(ParamWeightOutInfo param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@car_license", param.car_license);

            var query = @"EXEC sp_select_weight_out_by_car_license
                                @car_license = @car_license
            ";

            var datas = conn.Query<WeightOutData>(query, p).FirstOrDefault();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_WeightInInfo", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<MessageReport> Insert_WeightOutInfo(ResultGetWeightOutInfoViewModel param, UserInfoModel userInfo)
        {
            using var dbContext = new ApplicationContext();
            dbContext.Database.SetConnectionString(_connectionStringContext);
            await dbContext.Database.OpenConnectionAsync();
            using var trans = dbContext.Database.BeginTransaction();

            var toDay = DateTimeServer.Now();
            var result = new MessageReport(false, "ERROR!");

            try
            {
                TsWeightOut itm = new TsWeightOut
                {
                    CarLicense = param.car_license,
                    WeightOutNo = param.weight_out_no,
                    WeightOutType = param.weight_out_type,
                    BeforeWeightOut = param.before_weight_out,
                    WeightOut = param.weight_out,
                    WeightReceive = param.weight_total,
                    Date = toDay,
                    Status = "Active",
                    WeightInNo = param.weight_in_no,
                    PercentHumidityDiff = param.percent_humidity_diff,
                    PercentHumidityOk = param.percent_humidity_ok,
                    PercentHumidityOut = param.percent_humidity_out,
                    WeightPallet = param.weight_pallet,
                    QtyPallet = param.qty_pallet,
                    TotalWeightPallet = param.total_weight_pallet,
                    WeightBag = param.weight_bag,
                    QtyBag = param.qty_bag,
                    TotalWeightBag = param.total_weight_bag,
                    SgBg = param.sg_bg,
                    SgSupplier = param.sg_supplier,
                    ApiBg = param.api_bg,
                    ApiSupplier = param.api_supplier,
                    TempBg = param.temp_bg,
                    TempSupplier = param.temp_supplier,
                    VolumeBySupplier = param.volume_by_supplier,
                    WeightBySupplier = param.weight_by_supplier,
                    Remark1 = param.remark_1,
                    Remark2 = param.remark_2,
                    Reprint = 0,
                    UserId = userInfo.user_id.ToString(),
                    Company = param.company,
                    BaseUnit = param.base_unit,
                    UnitReceive = param.unit_receive,
                    GrossUom = param.gross_uom,
                    NetUom = param.net_uom,
                };

                await dbContext.TsWeightOuts.AddAsync(itm);
                await dbContext.SaveChangesAsync();
                await trans.CommitAsync();
                result = new MessageReport(true, string.Format("Message: {0}", Constants.Result.Success));
                _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Insert, this.GetType().Name, "Insert_WeightOutInfo", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                result = new MessageReport(false, string.Format("Error Code: {0} - Details: {1}", ErrorCodes.WeightOut.Repo.Insert_WeightOutInfo, ex.Message));
                _logger.WriteError(errorCode: ErrorCodes.WeightOut.Repo.Insert_WeightOutInfo, errorMessage: ex.Message, additionalInfo: result.message, exception: ex, user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            finally
            {
                await trans.DisposeAsync();
                await dbContext.DisposeAsync();
            }

            return result;
        }

        public async Task<List<WeightOutData>> Select_SearchWeightOutListData_By(ParamSearchWeightOutViewModel param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            int page = (param.start / param.length) + 1;
            var _offset = (page - 1) * param.length;
            var _pagesize = param.length;

            var p = new DynamicParameters();
            p.Add("@Offset", _offset);
            p.Add("@PageSize", _pagesize);
            p.Add("@weight_out_no", param.weight_out_no);
            p.Add("@weight_in_no", param.weight_in_no);

            var query = @"EXEC sp_select_weight_out_by 
                                 @Offset = @Offset
                                ,@PageSize = @PageSize
                                ,@weight_out_no = @weight_out_no
                                ,@weight_in_no = @weight_in_no
                        ";

            var datas = conn.Query<WeightOutData>(query, p).ToList();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_SearchWeightOutListData_By", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<decimal> Select_SumQtyWeightOut_By(ParamGetSumQtyWeightHistoryViewModel param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@document_po", param.document_po);
            p.Add("@item_code", param.item_code);
            p.Add("@line_number", param.line_number);
            p.Add("@date", param.date.ToString("yyyy-MM-dd"));

            var query = @"EXEC sp_get_calculate_weight_by @document_po, @item_code, @line_number, @date";

            var data = conn.Query<decimal>(query, p).FirstOrDefault();

            conn.Close();

            _logger.WriteActivity(
                message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_SumQtyWeightOut_By", param.ToJson()),
                user: _context.HttpContext.Session.GetString(Constants.Session.User)
            );

            return data;
        }

        public async Task<MessageReport> Update_WeightOutInfo(ResultGetWeightOutInfoViewModel param, UserInfoModel userInfo)
        {
            using var dbContext = new ApplicationContext();
            dbContext.Database.SetConnectionString(_connectionStringContext);
            await dbContext.Database.OpenConnectionAsync();
            using var trans = dbContext.Database.BeginTransaction();

            var toDay = DateTimeServer.Now();
            var result = new MessageReport(false, "ERROR!");

            try
            {
                TsWeightOut itm = dbContext.TsWeightOuts.Where(i => i.WeightOutNo == param.weight_out_no && i.WeightInNo == param.weight_in_no).FirstOrDefault();
                itm.Remark1 = param.remark_1;
                itm.Remark2 = param.remark_2;
                itm.WeightBySupplier = param.weight_by_supplier;
                itm.VolumeBySupplier = param.volume_by_supplier;
                itm.SgBg = param.sg_bg;
                itm.SgSupplier = param.sg_bg;
                itm.ApiBg = param.api_bg;
                itm.ApiSupplier = param.api_supplier;
                itm.TempBg = param.temp_bg;
                itm.TempSupplier = param.temp_supplier;

                await dbContext.SaveChangesAsync();
                await trans.CommitAsync();
                result = new MessageReport(true, string.Format("Message: {0}", Constants.Result.Success));
                _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Update, this.GetType().Name, "Update_WeightOutInfo", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                result = new MessageReport(false, string.Format("Error Code: {0} - Details: {1}", ErrorCodes.WeightOut.Repo.Update_WeightOutInfo, ex.Message));
                _logger.WriteError(errorCode: ErrorCodes.WeightOut.Repo.Update_WeightOutInfo, errorMessage: ex.Message, additionalInfo: result.message, exception: ex, user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            finally
            {
                await trans.DisposeAsync();
                await dbContext.DisposeAsync();
            }

            return result;
        }

        public async Task<MessageReport> Update_WeightOutStatus(ResultGetWeightOutInfoViewModel param, UserInfoModel userInfo)
        {
            using var dbContext = new ApplicationContext();
            dbContext.Database.SetConnectionString(_connectionStringContext);
            await dbContext.Database.OpenConnectionAsync();
            using var trans = dbContext.Database.BeginTransaction();

            var toDay = DateTimeServer.Now();
            var result = new MessageReport(false, "ERROR!");

            try
            {
                TsWeightOut itm = dbContext.TsWeightOuts.Where(i => i.WeightOutNo == param.weight_out_no && i.WeightInNo == param.weight_in_no && i.Status == "Active").FirstOrDefault();
                itm.Status = param.status;

                await dbContext.SaveChangesAsync();
                await trans.CommitAsync();
                result = new MessageReport(true, string.Format("Message: {0}", Constants.Result.Success));
                _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Update, this.GetType().Name, "Update_WeightOutStatus", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                result = new MessageReport(false, string.Format("Error Code: {0} - Details: {1}", ErrorCodes.WeightOut.Repo.Update_WeightOutInfo, ex.Message));
                _logger.WriteError(errorCode: ErrorCodes.WeightOut.Repo.Update_WeightOutInfo, errorMessage: ex.Message, additionalInfo: result.message, exception: ex, user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            finally
            {
                await trans.DisposeAsync();
                await dbContext.DisposeAsync();
            }

            return result;
        }

        public async Task<MessageReport> Update_WeightOutCarLicense(ResultGetWeightOutInfoViewModel param, UserInfoModel userInfo)
        {
            using var dbContext = new ApplicationContext();
            dbContext.Database.SetConnectionString(_connectionStringContext);
            await dbContext.Database.OpenConnectionAsync();
            using var trans = dbContext.Database.BeginTransaction();

            var toDay = DateTimeServer.Now();
            var result = new MessageReport(false, "ERROR!");

            try
            {
                TsWeightOut itm = dbContext.TsWeightOuts.Where(i => i.WeightInNo == param.weight_in_no).FirstOrDefault();
                itm.CarLicense = param.car_license;

                await dbContext.SaveChangesAsync();
                await trans.CommitAsync();
                result = new MessageReport(true, string.Format("Message: {0}", Constants.Result.Success));
                _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Update, this.GetType().Name, "Update_WeightOutCarLicense", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                result = new MessageReport(false, string.Format("Error Code: {0} - Details: {1}", ErrorCodes.WeightOut.Repo.Update_WeightOutInfo, ex.Message));
                _logger.WriteError(errorCode: ErrorCodes.WeightOut.Repo.Update_WeightOutInfo, errorMessage: ex.Message, additionalInfo: result.message, exception: ex, user: _context.HttpContext.Session.GetString(Constants.Session.User));
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
