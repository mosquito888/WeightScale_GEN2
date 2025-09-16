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
using WeightScaleGen2.BGC.Models.ViewModels.UOMConversion;

namespace WeightScaleGen2.BGC.API.APIRepository
{
    public class UOMConversionRepository : IUOMConversionRepository
    {
        IDatabaseConnectionFactory _db;
        ILogger _logger;
        IConfiguration _configuration;
        private readonly IHttpContextAccessor _context;
        private readonly ApplicationContext _applicationContext;
        private readonly string _connectionStringContext;

        public UOMConversionRepository(
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

        public async Task<List<UOMConversionData>> Select_SearchUOMConversionListData_All(UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();
            var p = new DynamicParameters();
            p.Add("@comp_code", userInfo.comp_code);
            p.Add("@plant_code", userInfo.plant_code);

            var query = @"EXEC sp_select_uom_conversion_all @comp_code = @comp_code, @plant_code = @plant_code";

            var datas = conn.Query<UOMConversionData>(query,p).ToList();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_SearchUOMConversionListData_All", null), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<List<UOMConversionData>> Select_SearchUOMConversionListData_By(ParamSearchUOMConversionViewModel param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@material_code", param.material_code);
            p.Add("@uom", param.uom);
            p.Add("@comp_code", userInfo.comp_code);
            p.Add("@plant_code", userInfo.plant_code);

            var query = @"EXEC sp_select_uom_conversion_by @material_code = @material_code, @uom = @uom, @comp_code = @comp_code, @plant_code = @plant_code";

            var datas = conn.Query<UOMConversionData>(query, p).ToList();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_SearchUOMConversionListData_By", null), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<List<UOMConversionData>> Select_SearchUOMConversionListData_By_MaterialCode(string materialCode, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@material_code", materialCode);
            p.Add("@comp_code", userInfo.comp_code);
            p.Add("@plant_code", userInfo.plant_code);

            var query = @"EXEC sp_select_uom_conversion_by_material_code @material_code = @material_code, @comp_code = @comp_code, @plant_code = @plant_code";

            var datas = conn.Query<UOMConversionData>(query, p).ToList();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_SearchUOMConversionListData_By_MaterialCode", null), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<MessageReport> Insert_UOMConversionInfo(ResultSearchUOMConversionViewModel param, UserInfoModel userInfo)
        {
            using var dbContext = new ApplicationContext();
            dbContext.Database.SetConnectionString(_connectionStringContext);
            await dbContext.Database.OpenConnectionAsync();
            using var trans = dbContext.Database.BeginTransaction();

            var toDay = DateTimeServer.Now();
            var result = new MessageReport(false, "ERROR!");

            try
            {
                TsUomConversion itm = new TsUomConversion
                {
                    MaterialCode = param.material_code,
                    AlterUom = param.alter_uom,
                    BaseUom = param.base_uom,
                    AlterUomIn = param.alter_uom_in,
                    BaseUomIn = param.base_uom_in,
                    ConvWeightN = param.conv_weight_n,
                    ConvWeightD = param.conv_weight_d,
                    NetWeight = param.net_weight,
                    GrossWeight = param.gross_weight,
                    WeightUnit = param.weight_unit,
                    CreatedBy = param.created_by,
                    CreatedOn = DateOnly.FromDateTime(param.created_on),
                    CreatedTime = TimeOnly.FromTimeSpan(param.created_time),
                    UpdatedBy = param.updated_by,
                    UpdatedOn = DateOnly.FromDateTime(param.updated_on),
                    UpdatedTime = TimeOnly.FromTimeSpan(param.updated_time),
                    CompCode = userInfo.comp_code,
                    PlantCode = userInfo.plant_code
                };

                await dbContext.TsUomConversions.AddAsync(itm);
                await dbContext.SaveChangesAsync();
                await trans.CommitAsync();
                result = new MessageReport(true, string.Format("Message: {0}", Constants.Result.Success));
                _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Insert, this.GetType().Name, "Insert_UOMConversionInfo", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                result = new MessageReport(false, string.Format("Error Code: {0} - Details: {1}", ErrorCodes.UOMConversion.Repo.Insert_UOMConversionInfo, ex.Message));
                _logger.WriteError(errorCode: ErrorCodes.UOMConversion.Repo.Insert_UOMConversionInfo, errorMessage: ex.Message, additionalInfo: result.message, exception: ex, user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            finally
            {
                await trans.DisposeAsync();
                await dbContext.DisposeAsync();
            }

            return result;
        }

        public async Task<MessageReport> Update_UOMConversionInfo(ResultSearchUOMConversionViewModel param, UserInfoModel userInfo)
        {
            using var dbContext = new ApplicationContext();
            dbContext.Database.SetConnectionString(_connectionStringContext);
            await dbContext.Database.OpenConnectionAsync();
            using var trans = dbContext.Database.BeginTransaction();

            var toDay = DateTimeServer.Now();
            var result = new MessageReport(false, "ERROR!");

            try
            {
                TsUomConversion itm = dbContext.TsUomConversions.Where(i => i.BaseUom == param.base_uom && i.AlterUom == param.alter_uom && i.MaterialCode == param.material_code).FirstOrDefault();
                itm.ConvWeightN = param.conv_weight_n;
                itm.ConvWeightD = param.conv_weight_d;
                itm.NetWeight = param.net_weight;
                itm.GrossWeight = param.gross_weight;
                itm.WeightUnit = param.weight_unit;
                itm.CreatedBy = param.created_by;
                itm.CreatedOn = DateOnly.FromDateTime(param.created_on);
                itm.CreatedTime = TimeOnly.FromTimeSpan(param.created_time);
                itm.UpdatedBy = param.updated_by;
                itm.UpdatedOn = DateOnly.FromDateTime(param.updated_on);
                itm.UpdatedTime = TimeOnly.FromTimeSpan(param.updated_time);
                itm.CompCode = userInfo.comp_code;
                itm.PlantCode = userInfo.plant_code;

                await dbContext.SaveChangesAsync();
                await trans.CommitAsync();
                result = new MessageReport(true, string.Format("Message: {0}", Constants.Result.Success));
                _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Update, this.GetType().Name, "Update_UOMConversionInfo", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                result = new MessageReport(false, string.Format("Error Code: {0} - Details: {1}", ErrorCodes.UOMConversion.Repo.Update_UOMConversionInfo, ex.Message));
                _logger.WriteError(errorCode: ErrorCodes.UOMConversion.Repo.Update_UOMConversionInfo, errorMessage: ex.Message, additionalInfo: result.message, exception: ex, user: _context.HttpContext.Session.GetString(Constants.Session.User));
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
