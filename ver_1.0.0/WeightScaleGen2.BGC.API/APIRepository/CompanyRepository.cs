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
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.DBModelsEF;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.Company;
using Constants = WeightScaleGen2.BGC.Models.Constants;

namespace WeightScaleGen2.BGC.API.APIRepository
{
    public class CompanyRepository : ICompanyRepository
    {
        IDatabaseConnectionFactory _db;
        ILogger _logger;
        IConfiguration _configuration;
        private readonly IHttpContextAccessor _context;
        private readonly ApplicationContext _applicationContext;
        private readonly string _connectionStringContext;

        public CompanyRepository(
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

        public async Task<MessageReport> Insert_Info(ResultGetCompInfoViewModel param, UserInfoModel userInfo)
        {
            using var dbContext = new ApplicationContext();
            dbContext.Database.SetConnectionString(_connectionStringContext);
            await dbContext.Database.OpenConnectionAsync();
            using var trans = dbContext.Database.BeginTransaction();

            var toDay = DateTimeServer.Now();
            var result = new MessageReport(false, "ERROR!");

            try
            {
                SyComp compInfo = new SyComp
                {
                    CompCode = param.comp_code,
                    NameThLine1 = param.name_th_line1,
                    NameThLine2 = param.name_th_line2,
                    NameEnLine1 = param.name_en_line1,
                    NameEnLine2 = param.name_en_line2,
                    AddrThLine1 = param.addr_th_line1,
                    AddrThLine2 = param.addr_th_line2,
                    AddrEnLine1 = param.addr_en_line1,
                    AddrEnLine2 = param.addr_en_line2,
                    Phone = param.phone,
                    ProvinceCode = param.province_code,
                    HeadReport = param.@head_report,
                    ApproveName = param.@approve_name,
                    EditAfterPrint = false,

                    CreatedBy = userInfo.username,
                    CreatedDate = toDay,

                    IsActive = true,
                    IsDeleted = false,
                    PlantCode = userInfo.plant_code
                };

                await dbContext.SyComps.AddAsync(compInfo);
                await dbContext.SaveChangesAsync();
                await trans.CommitAsync();
                result = new MessageReport(true, string.Format("Message: {0}", Constants.Result.Success));
                _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Insert, this.GetType().Name, "Insert_Info", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                result = new MessageReport(false, string.Format("Error Code: {0} - Details: {1}", ErrorCodes.Company.Repo.Insert_Info, ex.Message));
                _logger.WriteError(errorCode: ErrorCodes.Company.Repo.Insert_Info, errorMessage: ex.Message, additionalInfo: result.message, exception: ex, user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            finally
            {
                await trans.DisposeAsync();
                await dbContext.DisposeAsync();
            }

            return result;
        }

        public async Task<MessageReport> Update_Info(ResultGetCompInfoViewModel param, UserInfoModel userInfo)
        {
            using var dbContext = new ApplicationContext();
            dbContext.Database.SetConnectionString(_connectionStringContext);
            await dbContext.Database.OpenConnectionAsync();
            using var trans = dbContext.Database.BeginTransaction();

            var toDay = DateTimeServer.Now();
            var result = new MessageReport(false, "ERROR!");

            try
            {
                SyComp compInfo = dbContext.SyComps.Where(i => i.CompCode == param.comp_code).FirstOrDefault();

                if (compInfo != null)
                {
                    compInfo.NameThLine1 = param.name_th_line1;
                    compInfo.NameThLine2 = param.name_th_line2;
                    compInfo.NameEnLine1 = param.name_en_line1;
                    compInfo.NameEnLine2 = param.name_en_line2;
                    compInfo.AddrThLine1 = param.addr_th_line1;
                    compInfo.AddrThLine2 = param.addr_th_line2;
                    compInfo.AddrEnLine1 = param.addr_en_line1;
                    compInfo.AddrEnLine2 = param.addr_en_line2;
                    compInfo.Phone = param.phone;
                    compInfo.ProvinceCode = param.province_code;
                    compInfo.HeadReport = param.@head_report;
                    compInfo.ApproveName = param.@approve_name;
                    compInfo.EditAfterPrint = param.@edit_after_print;

                    compInfo.ModifiedBy = userInfo.username;
                    compInfo.ModifiedDate = toDay;
                    compInfo.PlantCode = userInfo.plant_code;

                    await dbContext.SaveChangesAsync();
                    await trans.CommitAsync();
                    result = new MessageReport(true, string.Format("Message: {0}", Constants.Result.Success));
                    _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Update, this.GetType().Name, "Update_Info", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
                }
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                result = new MessageReport(false, string.Format("Error Code: {0} - Details: {1}", ErrorCodes.Company.Repo.Update_Info, ex.Message));
                _logger.WriteError(errorCode: ErrorCodes.Company.Repo.Update_Info, errorMessage: ex.Message, additionalInfo: result.message, exception: ex, user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            finally
            {
                await trans.DisposeAsync();
                await dbContext.DisposeAsync();
            }

            return result;
        }

        public async Task<MessageReport> Delete_Info(ResultGetCompInfoViewModel param, UserInfoModel userInfo)
        {
            using var dbContext = new ApplicationContext();
            dbContext.Database.SetConnectionString(_connectionStringContext);
            await dbContext.Database.OpenConnectionAsync();
            using var trans = dbContext.Database.BeginTransaction();

            var toDay = DateTimeServer.Now();
            var result = new MessageReport(false, "ERROR!");

            try
            {
                SyComp compInfo = dbContext.SyComps.Where(i => i.CompCode == param.comp_code).FirstOrDefault();

                if (compInfo != null)
                {
                    dbContext.Remove(compInfo);
                    await dbContext.SaveChangesAsync();
                    await trans.CommitAsync();

                    result = new MessageReport(true, string.Format("Message: {0}", Constants.Result.Success));
                    _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Delete, this.GetType().Name, "Delete_Info", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
                }
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                result = new MessageReport(false, string.Format("Error Code: {0} - Details: {1}", ErrorCodes.Company.Repo.Delete_Info, ex.Message));
                _logger.WriteError(errorCode: ErrorCodes.Company.Repo.Delete_Info, errorMessage: ex.Message, additionalInfo: result.message, exception: ex, user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            finally
            {
                await trans.DisposeAsync();
                await dbContext.DisposeAsync();
            }

            return result;
        }

        public async Task<List<CompanyData>> Select_Company_All(UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var query = @"EXEC sp_select_company_all";

            var datas = conn.Query<CompanyData>(query).ToList();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_Company_All", null), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<List<CompanyData>> Select_Company_By(ParamSearchCompViewModel param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            int page = (param.start / param.length) + 1;
            var _offset = (page - 1) * param.length;
            var _pagesize = param.length;

            var p = new DynamicParameters();
            p.Add("@Offset", _offset);
            p.Add("@PageSize", _pagesize);

            p.Add("@comp_code", param.comp_code);
            p.Add("@name_th_line1", param.comp_name_th);
            p.Add("@name_en_line1", param.comp_name_en);

            var query = @"EXEC sp_select_company_by 
                                 @Offset = @Offset
                                ,@PageSize = @PageSize      
                                ,@comp_code = @comp_code
                                ,@name_th_line1 = @name_th_line1
                                ,@name_en_line1 = @name_en_line1
            ";

            var datas = conn.Query<CompanyData>(query, p).ToList();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_Company_By", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }
    }
}
