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
using WeightScaleGen2.BGC.Models.ViewModels.Master;

namespace WeightScaleGen2.BGC.API.APIRepository
{
    public class MasterRepository : IMasterRepository
    {
        IDatabaseConnectionFactory _db;
        ILogger _logger;
        IConfiguration _configuration;
        private readonly IHttpContextAccessor _context;
        private readonly ApplicationContext _applicationContext;
        private readonly string _connectionStringContext;

        public MasterRepository(
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

        public async Task<MessageReport> Insert_Info(ResultGetMasterInfoViewModel param, UserInfoModel userInfo)
        {
            using var dbContext = new ApplicationContext();
            dbContext.Database.SetConnectionString(_connectionStringContext);
            await dbContext.Database.OpenConnectionAsync();
            using var trans = dbContext.Database.BeginTransaction();

            var toDay = DateTimeServer.Now();
            var result = new MessageReport(false, "ERROR!");

            try
            {
                SyMaster syMaster = new SyMaster
                {
                    CompCode = "all",
                    PlantCode = "all",
                    MasterType = param.master_type,
                    MasterCode = param.master_code,
                    MasterValue1 = param.master_value1,
                    MasterValue2 = param.master_value2,
                    MasterValue3 = param.master_value3,
                    MasterDescTh = param.master_desc_th,
                    MasterDescEn = param.master_desc_en,

                    IsActive = true,
                    IsDeleted = false,
                    IsAll = true
                };

                await dbContext.SyMasters.AddAsync(syMaster);
                await dbContext.SaveChangesAsync();
                await trans.CommitAsync();
                result = new MessageReport(true, string.Format("Message: {0}", Constants.Result.Success));
                _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Insert, this.GetType().Name, "Insert_Info", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                result = new MessageReport(false, string.Format("Error Code: {0} - Details: {1}", ErrorCodes.Master.Repo.Insert_Info, ex.Message));
                _logger.WriteError(errorCode: ErrorCodes.Master.Repo.Insert_Info, errorMessage: ex.Message, additionalInfo: result.message, exception: ex, user: _context.HttpContext.Session.GetString(Constants.Session.User));
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
            //p.Add("@comp_code", "all");
            //p.Add("@plant_code", "all");
            //p.Add("@master_type", param.master_type);
            //p.Add("@master_code", param.master_code);
            //p.Add("@master_value1", param.master_value1);
            //p.Add("@master_value2", param.master_value2);
            //p.Add("@master_value3", param.master_value3);
            //p.Add("@master_desc_th", param.master_desc_th);
            //p.Add("@master_desc_en", param.master_desc_en);
            //p.Add("@is_active", true);
            //p.Add("@is_deleted", false);
            //p.Add("@is_all", true);

            //var query = $@"
            //                    INSERT INTO dbo.sy_master
            //                    (
            //                      comp_code
            //                     ,plant_code
            //                     ,master_type
            //                     ,master_code
            //                     ,master_value1
            //                     ,master_value2
            //                     ,master_value3
            //                     ,master_desc_th
            //                     ,master_desc_en
            //                     ,is_active
            //                     ,is_deleted
            //                     ,is_all
            //                    )
            //                    VALUES
            //                    (
            //                      @comp_code
            //                     ,@plant_code
            //                     ,@master_type
            //                     ,@master_code
            //                     ,@master_value1
            //                     ,@master_value2
            //                     ,@master_value3
            //                     ,@master_desc_th
            //                     ,@master_desc_en
            //                     ,@is_active
            //                     ,@is_deleted
            //                     ,@is_all
            //                    );
            //                ";

            //using (var trans = conn.BeginTransaction())
            //{
            //    conn.Execute(query, p, trans);
            //    trans.Commit();
            //}

            //conn.Close();
            #endregion
        }

        public async Task<MessageReport> Update_Info(ResultGetMasterInfoViewModel param, UserInfoModel userInfo)
        {
            using var dbContext = new ApplicationContext();
            dbContext.Database.SetConnectionString(_connectionStringContext);
            await dbContext.Database.OpenConnectionAsync();
            using var trans = dbContext.Database.BeginTransaction();

            var toDay = DateTimeServer.Now();
            var result = new MessageReport(false, "ERROR!");

            try
            {
                SyMaster syMaster = dbContext.SyMasters
                    .Where(i => i.MasterType == param.master_type && i.MasterCode == param.master_code).FirstOrDefault();

                syMaster.MasterValue1 = param.master_value1;
                syMaster.MasterDescTh = param.master_desc_th;

                syMaster.IsActive = true;
                syMaster.IsDeleted = false;
                syMaster.IsAll = true;

                await dbContext.SaveChangesAsync();
                await trans.CommitAsync();
                result = new MessageReport(true, string.Format("Message: {0}", Constants.Result.Success));
                _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Update, this.GetType().Name, "Update_Info", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                result = new MessageReport(false, string.Format("Error Code: {0} - Details: {1}", ErrorCodes.Master.Repo.Update_Info, ex.Message));
                _logger.WriteError(errorCode: ErrorCodes.Master.Repo.Update_Info, errorMessage: ex.Message, additionalInfo: result.message, exception: ex, user: _context.HttpContext.Session.GetString(Constants.Session.User));
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
            //p.Add("@comp_code", "all");
            //p.Add("@plant_code", "all");
            //p.Add("@master_type", param.master_type);
            //p.Add("@master_code", param.master_code);
            //p.Add("@master_value1", param.master_value1);
            //p.Add("@master_value2", param.master_value2);
            //p.Add("@master_value3", param.master_value3);
            //p.Add("@master_desc_th", param.master_desc_th);
            //p.Add("@master_desc_en", param.master_desc_en);
            //p.Add("@is_active", true);
            //p.Add("@is_deleted", false);
            //p.Add("@is_all", true);

            //var query = $@"
            //                    UPDATE dbo.sy_master 
            //                    SET
            //                      master_value1 = @master_value1
            //                     ,master_desc_th = @master_desc_th
            //                    WHERE
            //                          master_type = @master_type
            //                      AND master_code = @master_code;
            //                ";

            //using (var trans = conn.BeginTransaction())
            //{
            //    conn.Execute(query, p, trans);
            //    trans.Commit();
            //}

            //conn.Close();
            #endregion
        }

        public async Task<MessageReport> Delete_Info(ResultGetMasterInfoViewModel param, UserInfoModel userInfo)
        {
            using var dbContext = new ApplicationContext();
            dbContext.Database.SetConnectionString(_connectionStringContext);
            await dbContext.Database.OpenConnectionAsync();
            using var trans = dbContext.Database.BeginTransaction();

            var toDay = DateTimeServer.Now();
            var result = new MessageReport(false, "ERROR!");

            try
            {
                SyMaster syMaster = dbContext.SyMasters
                    .Where(i => i.MasterType == param.master_type && i.MasterCode == param.master_code).FirstOrDefault();

                dbContext.Remove(syMaster);

                await dbContext.SaveChangesAsync();
                await trans.CommitAsync();
                result = new MessageReport(true, string.Format("Message: {0}", Constants.Result.Success));
                _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Delete, this.GetType().Name, "Delete_Info", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                result = new MessageReport(false, string.Format("Error Code: {0} - Details: {1}", ErrorCodes.Master.Repo.Delete_Info, ex.Message));
                _logger.WriteError(errorCode: ErrorCodes.Master.Repo.Delete_Info, errorMessage: ex.Message, additionalInfo: result.message, exception: ex, user: _context.HttpContext.Session.GetString(Constants.Session.User));
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
            //p.Add("@master_type", param.master_type);
            //p.Add("@master_code", param.master_code);

            //var query = $@"
            //                    DELETE FROM dbo.sy_master
            //                    WHERE
            //                          master_type = @master_type
            //                      AND master_code = @master_code;
            //                ";

            //using (var trans = conn.BeginTransaction())
            //{
            //    conn.Execute(query, p, trans);
            //    trans.Commit();
            //}

            //conn.Close();
            #endregion
        }

        public async Task<List<MasterData>> Select_MasterData(ParamSearchMasterViewModel param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            int page = (param.start / param.length) + 1;
            var _offset = (page - 1) * param.length;
            var _pagesize = param.length;

            var p = new DynamicParameters();
            p.Add("@Offset", _offset);
            p.Add("@PageSize", _pagesize);
            p.Add("@master_type", param.master_type);
            p.Add("@master_code", param.master_code);
            p.Add("@master_value1", param.master_value1);

            var query = @"
                            SELECT
                              COUNT(*) OVER() AS total_record
                             ,comp_code
                             ,plant_code
                             ,master_type
                             ,master_code
                             ,master_value1
                             ,master_value2
                             ,master_value3
                             ,master_desc_th
                             ,master_desc_en
                             ,is_active
                             ,is_deleted
                             ,is_all
                            FROM dbo.sy_master
							WHERE 
                                (is_active = 1)
							AND (is_deleted = 0)
                            AND ((master_type LIKE '%' + @master_type + '%') OR @master_type IS NULL)
							AND ((master_code LIKE '%' + @master_code + '%') OR @master_code IS NULL)
							AND ((master_value1 LIKE '%' + @master_value1 + '%') OR @master_value1 IS NULL)
                            ORDER BY master_type
                            OFFSET @Offset ROWS
                            FETCH NEXT @PageSize ROWS ONLY;
                        ";

            var datas = conn.Query<MasterData>(query, p).ToList();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_MasterData", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }
    }
}
