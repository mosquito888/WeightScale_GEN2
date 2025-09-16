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
using WeightScaleGen2.BGC.Models.ViewModels.Sender;

namespace WeightScaleGen2.BGC.API.APIRepository
{
    public class SenderRepository : ISenderRepository
    {
        IDatabaseConnectionFactory _db;
        ILogger _logger;
        IConfiguration _configuration;
        private readonly IHttpContextAccessor _context;
        private readonly ApplicationContext _applicationContext;
        private readonly string _connectionStringContext;

        public SenderRepository(
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

        public async Task<MessageReport> Insert_SenderInfo(ResultGetSenderInfoViewModel param, UserInfoModel userInfo)
        {
            using var dbContext = new ApplicationContext();
            dbContext.Database.SetConnectionString(_connectionStringContext);
            await dbContext.Database.OpenConnectionAsync();
            using var trans = dbContext.Database.BeginTransaction();

            var toDay = DateTimeServer.Now();
            var result = new MessageReport(false, "ERROR!");

            try
            {
                SySender sen = new SySender
                {
                    SenderName = param.sender_name,
                    FlagDelete = param.flag_delete,
                    //CreatedBy = userInfo.username,
                    //CreatedDate = toDay,
                    IsActive = true,
                    IsDeleted = false,
                    CompCode = userInfo.comp_code,
                    PlantCode = userInfo.plant_code
                };

                await dbContext.SySenders.AddAsync(sen);
                await dbContext.SaveChangesAsync();
                await trans.CommitAsync();
                result = new MessageReport(true, string.Format("Message: {0}", Constants.Result.Success));
                _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Insert, this.GetType().Name, "Insert_SenderInfo", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                result = new MessageReport(false, string.Format("Error Code: {0} - Details: {1}", ErrorCodes.Sender.Repo.Insert_SenderInfo, ex.Message));
                _logger.WriteError(errorCode: ErrorCodes.Sender.Repo.Insert_SenderInfo, errorMessage: ex.Message, additionalInfo: result.message, exception: ex, user: _context.HttpContext.Session.GetString(Constants.Session.User));
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
            //p.Add("@sender_name", param.sender_name);
            //p.Add("@flag_delete", param.flag_delete);
            //p.Add("@created_by", userInfo.username);
            //p.Add("@is_active", true);
            //p.Add("@is_deleted", false);

            //var query = $@"
            //                    INSERT INTO [dbo].[sy_sender]
            //                               ([sender_name]
            //                               ,[flag_delete]
            //                               ,[created_by]
            //                               ,[created_date]
            //                               ,[is_active]
            //                               ,[is_deleted])
            //                         VALUES
            //                               (@sender_name
            //                               ,@flag_delete
            //                               ,@created_by
            //                               ,GETDATE()
            //                               ,@is_active
            //                               ,@is_deleted)
            //            ";

            //using (var trans = conn.BeginTransaction())
            //{
            //    conn.Execute(query, p, trans);
            //    trans.Commit();
            //}

            //conn.Close();
            #endregion
        }

        public async Task<SenderData> Select_SenderInfo(ParamSenderInfo param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@id", param.id);
            p.Add("@comp_code", userInfo.comp_code);
            p.Add("@plant_code", userInfo.plant_code);

            var query = @"EXEC sp_select_sy_sender_by_id
                                @id = @id
                               ,@comp_code = @comp_code
                               ,@plant_code = @plant_code
            ";

            var datas = conn.Query<SenderData>(query, p).FirstOrDefault();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_SenderInfo", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<List<SenderData>> Select_SenderListData_All(UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var query = @"EXEC sp_select_sy_sender_all";

            var datas = conn.Query<SenderData>(query).ToList();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_SenderListData_All", null), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<List<SenderData>> Select_SearchSenderListData_By(ParamSearchSenderViewModel param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            int page = (param.start / param.length) + 1;
            var _offset = (page - 1) * param.length;
            var _pagesize = param.length;

            var p = new DynamicParameters();
            p.Add("@Offset", _offset);
            p.Add("@PageSize", _pagesize);

            p.Add("@sender_name", param.sender_name);
            p.Add("@comp_code", userInfo.comp_code);
            p.Add("@plant_code", userInfo.plant_code);

            var query = @"EXEC sp_select_sy_sender_by 
                                 @Offset = @Offset
                                ,@PageSize = @PageSize
                                ,@sender_name = @sender_name
                                ,@comp_code = @comp_code
                                ,@plant_code = @plant_code
                        ";

            var datas = conn.Query<SenderData>(query, p).ToList();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_SearchSenderListData_By", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<MessageReport> Update_SenderInfo(ResultGetSenderInfoViewModel param, UserInfoModel userInfo)
        {
            using var dbContext = new ApplicationContext();
            dbContext.Database.SetConnectionString(_connectionStringContext);
            await dbContext.Database.OpenConnectionAsync();
            using var trans = dbContext.Database.BeginTransaction();

            var toDay = DateTimeServer.Now();
            var result = new MessageReport(false, "ERROR!");

            try
            {
                SySender sen = dbContext.SySenders.Where(i => i.Id == param.id).FirstOrDefault();

                sen.SenderName = param.sender_name;
                sen.FlagDelete = param.flag_delete;
                //emp.ModifiedBy = userInfo.username;
                //emp.ModifiedDate = toDay;
                sen.IsActive = true;
                sen.IsDeleted = false;
                sen.CompCode = userInfo.comp_code;
                sen.PlantCode = userInfo.plant_code;

                await dbContext.SaveChangesAsync();
                await trans.CommitAsync();
                result = new MessageReport(true, string.Format("Message: {0}", Constants.Result.Success));
                _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Update, this.GetType().Name, "Update_SenderInfo", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                result = new MessageReport(false, string.Format("Error Code: {0} - Details: {1}", ErrorCodes.Sender.Repo.Update_SenderInfo, ex.Message));
                _logger.WriteError(errorCode: ErrorCodes.Sender.Repo.Update_SenderInfo, errorMessage: ex.Message, additionalInfo: result.message, exception: ex, user: _context.HttpContext.Session.GetString(Constants.Session.User));
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
            //p.Add("@id", param.id);
            //p.Add("@sender_name", param.sender_name);
            //p.Add("@flag_delete", param.flag_delete);
            //p.Add("@modified_by", userInfo.username);

            //var query = $@"
            //                    UPDATE [dbo].[sy_sender]
            //                    SET 
            //                         [sender_name] = @sender_name
            //                        ,[flag_delete] = @flag_delete
            //                        ,[modified_by] = @modified_by
            //                        ,[modified_date] = GETDATE()
            //                    WHERE id = @id;
            //                ";

            //using (var trans = conn.BeginTransaction())
            //{
            //    conn.Execute(query, p, trans);
            //    trans.Commit();
            //}

            //conn.Close();
            #endregion
        }

        public async Task<MessageReport> Delete_Info(ResultGetSenderInfoViewModel param, UserInfoModel userInfo)
        {
            using var dbContext = new ApplicationContext();
            dbContext.Database.SetConnectionString(_connectionStringContext);
            await dbContext.Database.OpenConnectionAsync();
            using var trans = dbContext.Database.BeginTransaction();

            var toDay = DateTimeServer.Now();
            var result = new MessageReport(false, "ERROR!");

            try
            {
                SySender sen = dbContext.SySenders.Where(i => i.Id == param.id).FirstOrDefault();
                sen.IsActive = false;
                sen.IsDeleted = true;

                await dbContext.SaveChangesAsync();
                await trans.CommitAsync();
                result = new MessageReport(true, string.Format("Message: {0}", Constants.Result.Success));
                _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Delete, this.GetType().Name, "Delete_SenderInfo", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                result = new MessageReport(false, string.Format("Error Code: {0} - Details: {1}", ErrorCodes.Sender.Repo.Delete_SenderInfo, ex.Message));
                _logger.WriteError(errorCode: ErrorCodes.Sender.Repo.Delete_SenderInfo, errorMessage: ex.Message, additionalInfo: result.message, exception: ex, user: _context.HttpContext.Session.GetString(Constants.Session.User));
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
