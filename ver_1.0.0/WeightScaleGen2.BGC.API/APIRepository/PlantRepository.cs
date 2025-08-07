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
using WeightScaleGen2.BGC.Models.ViewModels.Plant;

namespace WeightScaleGen2.BGC.API.APIRepository
{
    public class PlantRepository : IPlantRepository
    {
        IDatabaseConnectionFactory _db;
        ILogger _logger;
        IConfiguration _configuration;
        private readonly IHttpContextAccessor _context;
        private readonly ApplicationContext _applicationContext;
        private readonly string _connectionStringContext;

        public PlantRepository(
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

        public async Task<MessageReport> Delete_Info(ResultGetPlantInfoViewModel param, UserInfoModel userInfo)
        {
            using var dbContext = new ApplicationContext();
            dbContext.Database.SetConnectionString(_connectionStringContext);
            await dbContext.Database.OpenConnectionAsync();
            using var trans = dbContext.Database.BeginTransaction();

            var toDay = DateTimeServer.Now();
            var result = new MessageReport(false, "ERROR!");

            try
            {
                SyPlant syPlant = dbContext.SyPlants
                    .Where(i => i.PlantCode == param.plant_code && i.CompCode == param.comp_code).FirstOrDefault();

                dbContext.Remove(syPlant);

                await dbContext.SaveChangesAsync();
                await trans.CommitAsync();
                result = new MessageReport(true, string.Format("Message: {0}", Constants.Result.Success));
                _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Delete, this.GetType().Name, "Delete_Info", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                result = new MessageReport(false, string.Format("Error Code: {0} - Details: {1}", ErrorCodes.Plant.Repo.Delete_Info, ex.Message));
                _logger.WriteError(errorCode: ErrorCodes.Plant.Repo.Delete_Info, errorMessage: ex.Message, additionalInfo: result.message, exception: ex, user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            finally
            {
                await trans.DisposeAsync();
                await dbContext.DisposeAsync();
            }

            return result;
            #region Native SQL
            //using var conn = await _db.CreateConnectionAsync();

            //var p = new DynamicParameters();
            //p.Add("@plant_code", param.plant_code);
            //p.Add("@comp_code", param.comp_code);

            //var query = $@"
            //                        DELETE FROM dbo.sy_plant
            //                        WHERE
            //                            plant_code = @plant_code
            //                        AND comp_code = @comp_code
            //                    ";

            //using (var trans = conn.BeginTransaction())
            //{
            //    conn.Execute(query, p, trans);
            //    trans.Commit();
            //}

            //conn.Close();
            #endregion
        }

        public async Task<MessageReport> Insert_Info(ResultGetPlantInfoViewModel param, UserInfoModel userInfo)
        {
            using var dbContext = new ApplicationContext();
            dbContext.Database.SetConnectionString(_connectionStringContext);
            await dbContext.Database.OpenConnectionAsync();
            using var trans = dbContext.Database.BeginTransaction();

            var toDay = DateTimeServer.Now();
            var result = new MessageReport(false, "ERROR!");

            try
            {
                SyPlant syPlant = new SyPlant();
                syPlant.ShortPlantCode = param.plant_code;
                syPlant.CompCode = param.comp_code;
                syPlant.ShortCode = param.short_code;
                syPlant.ProvinceCode = param.province_code;
                syPlant.NameTh = param.name_th;
                syPlant.NameEn = param.name_en;
                syPlant.AddrThLine1 = param.addr_th_line1;
                syPlant.AddrThLine2 = param.addr_th_line2;
                syPlant.AddrEnLine1 = param.addr_en_line1;
                syPlant.AddrEnLine2 = param.addr_en_line2;
                syPlant.HeadReport = param.head_report;
                syPlant.CreatedBy = userInfo.username;
                syPlant.CreatedDate = toDay;
                syPlant.IsActive = true;
                syPlant.IsDeleted = false;

                await dbContext.AddAsync(syPlant);
                await dbContext.SaveChangesAsync();
                await trans.CommitAsync();
                result = new MessageReport(true, string.Format("Message: {0}", Constants.Result.Success));
                _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Insert, this.GetType().Name, "Insert_Info", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                result = new MessageReport(false, string.Format("Error Code: {0} - Details: {1}", ErrorCodes.Plant.Repo.Insert_Info, ex.Message));
                _logger.WriteError(errorCode: ErrorCodes.Plant.Repo.Insert_Info, errorMessage: ex.Message, additionalInfo: result.message, exception: ex, user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            finally
            {
                await trans.DisposeAsync();
                await dbContext.DisposeAsync();
            }

            return result;
            #region Native SQL
            //using var conn = await _db.CreateConnectionAsync();

            //var p = new DynamicParameters();
            //p.Add("@plant_code", param.plant_code);
            //p.Add("@comp_code", param.comp_code);
            //p.Add("@short_code", param.short_code);
            //p.Add("@province_code", param.province_code);
            //p.Add("@name_th", param.name_th);
            //p.Add("@name_en", param.name_en);
            //p.Add("@addr_th_line1", param.addr_th_line1);
            //p.Add("@addr_th_line2", param.addr_th_line2);
            //p.Add("@addr_en_line1", param.addr_en_line1);
            //p.Add("@addr_en_line2", param.addr_en_line2);
            //p.Add("@head_report", param.head_report);
            //p.Add("@created_by", userInfo.username);
            //p.Add("@is_active", true);
            //p.Add("@is_deleted", false);

            //var query = $@"
            //                        INSERT INTO dbo.sy_plant
            //                        (
            //                          plant_code
            //                         ,comp_code
            //                         ,short_code
            //                         ,province_code
            //                         ,name_th
            //                         ,name_en
            //                         ,addr_th_line1
            //                         ,addr_th_line2
            //                         ,addr_en_line1
            //                         ,addr_en_line2
            //                         ,head_report
            //                         ,created_by
            //                         ,created_date
            //                         ,is_active
            //                         ,is_deleted
            //                        )
            //                        VALUES
            //                        (
            //                          @plant_code
            //                         ,@comp_code
            //                         ,@short_code
            //                         ,@province_code
            //                         ,@name_th
            //                         ,@name_en
            //                         ,@addr_th_line1
            //                         ,@addr_th_line2
            //                         ,@addr_en_line1
            //                         ,@addr_en_line2
            //                         ,@head_report
            //                         ,@created_by
            //                         ,GETDATE()
            //                         ,@is_active
            //                         ,@is_deleted
            //                        );
            //                    ";

            //using (var trans = conn.BeginTransaction())
            //{
            //    conn.Execute(query, p, trans);
            //    trans.Commit();
            //}

            //conn.Close();
            #endregion
        }

        public async Task<List<PlantData>> Select_Plant_All(UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var query = @"EXEC sp_select_sy_plant_all";

            var datas = conn.Query<PlantData>(query).ToList();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_Plant_All", null), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<List<PlantData>> Select_Plant_By(ParamSearchPlantViewModel param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            int page = (param.start / param.length) + 1;
            var _offset = (page - 1) * param.length;
            var _pagesize = param.length;

            var p = new DynamicParameters();
            p.Add("@Offset", _offset);
            p.Add("@PageSize", _pagesize);

            p.Add("@plant_code", param.plant_code);
            p.Add("@comp_code", param.comp_code);
            p.Add("@name_th", param.plant_name_th);
            p.Add("@name_en", param.plant_name_en);

            var query = @"EXEC sp_select_sy_plant_by
                                 @Offset = @Offset
                                ,@PageSize = @PageSize
                                ,@plant_code = @plant_code
                                ,@comp_code = @comp_code
                                ,@name_th = @name_th
                                ,@name_en = @name_en
            ";

            var datas = conn.Query<PlantData>(query, p).ToList();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_Plant_By", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<MessageReport> Update_Info(ResultGetPlantInfoViewModel param, UserInfoModel userInfo)
        {
            using var dbContext = new ApplicationContext();
            dbContext.Database.SetConnectionString(_connectionStringContext);
            await dbContext.Database.OpenConnectionAsync();
            using var trans = dbContext.Database.BeginTransaction();

            var toDay = DateTimeServer.Now();
            var result = new MessageReport(false, "ERROR!");

            try
            {
                SyPlant syPlant = dbContext.SyPlants
                    .Where(i => i.PlantCode == param.plant_code && i.CompCode == param.comp_code).FirstOrDefault();

                syPlant.ShortPlantCode = param.plant_code;
                syPlant.CompCode = param.comp_code;
                syPlant.ShortCode = param.short_code;
                syPlant.ProvinceCode = param.province_code;
                syPlant.NameTh = param.name_th;
                syPlant.NameEn = param.name_en;
                syPlant.AddrThLine1 = param.addr_th_line1;
                syPlant.AddrThLine2 = param.addr_th_line2;
                syPlant.AddrEnLine1 = param.addr_en_line1;
                syPlant.AddrEnLine2 = param.addr_en_line2;
                syPlant.HeadReport = param.head_report;
                syPlant.ModifiedBy = userInfo.username;
                syPlant.ModifiedDate = toDay;

                await dbContext.SaveChangesAsync();
                await trans.CommitAsync();
                result = new MessageReport(true, string.Format("Message: {0}", Constants.Result.Success));
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                result = new MessageReport(false, string.Format("Message: {0} - Details: {1}", ex.Message, ex.InnerException != null ? ex.InnerException.Message : Constants.Result.Error));
            }
            finally
            {
                await trans.DisposeAsync();
                await dbContext.DisposeAsync();
            }

            return result;
            #region Native SQL
            //using var conn = await _db.CreateConnectionAsync();

            //var p = new DynamicParameters();
            //p.Add("@plant_code", param.plant_code);
            //p.Add("@comp_code", param.comp_code);
            //p.Add("@short_code", param.short_code);
            //p.Add("@province_code", param.province_code);
            //p.Add("@name_th", param.name_th);
            //p.Add("@name_en", param.name_en);
            //p.Add("@addr_th_line1", param.addr_th_line1);
            //p.Add("@addr_th_line2", param.addr_th_line2);
            //p.Add("@addr_en_line1", param.addr_en_line1);
            //p.Add("@addr_en_line2", param.addr_en_line2);
            //p.Add("@head_report", param.head_report);
            //p.Add("@modified_by", userInfo.username);

            //var query = $@"
            //                        UPDATE dbo.sy_plant 
            //                        SET
            //                          plant_code = @plant_code
            //                         ,comp_code = @comp_code
            //                         ,short_code = @short_code
            //                         ,province_code = @province_code
            //                         ,name_th = @name_th
            //                         ,name_en = @name_en
            //                         ,addr_th_line1 = @addr_th_line1
            //                         ,addr_th_line2 = @addr_th_line2
            //                         ,addr_en_line1 = @addr_en_line1
            //                         ,addr_en_line2 = @addr_en_line2
            //                         ,head_report = @head_report
            //                         ,modified_by = @modified_by
            //                         ,modified_date = GETDATE()
            //                        WHERE
            //                            plant_code = @plant_code
            //                        AND comp_code = @comp_code;
            //                    ";

            //using (var trans = conn.BeginTransaction())
            //{
            //    conn.Execute(query, p, trans);
            //    trans.Commit();
            //}

            //conn.Close();
            #endregion
        }
    }
}
