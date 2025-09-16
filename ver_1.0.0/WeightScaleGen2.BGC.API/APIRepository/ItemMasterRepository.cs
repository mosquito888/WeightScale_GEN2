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
using WeightScaleGen2.BGC.Models.ViewModels.ItemMaster;

namespace WeightScaleGen2.BGC.API.APIRepository
{
    public class ItemMasterRepository : IItemMasterRepository
    {
        IDatabaseConnectionFactory _db;
        ILogger _logger;
        IConfiguration _configuration;
        private readonly IHttpContextAccessor _context;
        private readonly ApplicationContext _applicationContext;
        private readonly string _connectionStringContext;

        public ItemMasterRepository(
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

        public async Task<MessageReport> Insert_ItemMasterInfo(ResultGetItemMasterInfoViewModel param, UserInfoModel userInfo)
        {
            using var dbContext = new ApplicationContext();
            dbContext.Database.SetConnectionString(_connectionStringContext);
            await dbContext.Database.OpenConnectionAsync();
            using var trans = dbContext.Database.BeginTransaction();

            var toDay = DateTimeServer.Now();
            var result = new MessageReport(false, "ERROR!");

            try
            {
                SyItemMaster itm = new SyItemMaster
                {
                    ItemCode = param.item_code,
                    ItemName = param.item_name,
                    ItemGroup = param.item_group,
                    Status = param.status,
                    Remark1 = param.remark_1,
                    Remark2 = param.remark_2,
                    //CreatedBy = userInfo.username,
                    //CreatedDate = toDay,
                    IsActive = true,
                    IsDeleted = false,
                    CompCode = userInfo.comp_code,
                    PlantCode = userInfo.plant_code
                };

                await dbContext.SyItemMasters.AddAsync(itm);
                await dbContext.SaveChangesAsync();
                await trans.CommitAsync();
                result = new MessageReport(true, string.Format("Message: {0}", Constants.Result.Success));
                _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Insert, this.GetType().Name, "Insert_ItemMasterInfo", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                result = new MessageReport(false, string.Format("Error Code: {0} - Details: {1}", ErrorCodes.ItemMaster.Repo.Insert_ItemMasterInfo, ex.Message));
                _logger.WriteError(errorCode: ErrorCodes.ItemMaster.Repo.Insert_ItemMasterInfo, errorMessage: ex.Message, additionalInfo: result.message, exception: ex, user: _context.HttpContext.Session.GetString(Constants.Session.User));
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
            //p.Add("@item_code", param.item_code);
            //p.Add("@item_name", param.item_name);
            //p.Add("@item_group", param.item_group);
            //p.Add("@status", param.status);
            //p.Add("@remark_1", param.remark_1);
            //p.Add("@remark_2", param.remark_2);
            //p.Add("@created_by", userInfo.username);
            //p.Add("@is_active", true);
            //p.Add("@is_deleted", false);

            //var query = $@"
            //                    INSERT INTO [dbo].[sy_item_master]
            //                               ([item_code]
            //                               ,[item_name]
            //                               ,[item_group]
            //                               ,[status]
            //                               ,[remark_1]
            //                               ,[remark_2]
            //                               ,[created_by]
            //                               ,[created_date]
            //                               ,[is_active]
            //                               ,[is_deleted])
            //                         VALUES
            //                               (@item_code
            //                               ,@item_name
            //                               ,@item_group
            //                               ,@status
            //                               ,@remark_1
            //                               ,@remark_2
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

        public async Task<ItemMasterData> Select_ItemMasterInfo(ParamItemMasterInfo param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@product_code", param.product_code);
            p.Add("@comp_code", userInfo.comp_code);
            p.Add("@plant_code", userInfo.plant_code);

            var query = @"EXEC sp_select_sy_item_master_by_item_code
                                @product_code = @product_code
                                ,@comp_code = @comp_code
                                ,@plant_code = @plant_code
            ";

            var datas = conn.Query<ItemMasterData>(query, p).FirstOrDefault();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_ItemMasterInfo", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<List<ItemMasterData>> Select_ItemMasterListData_All(UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var query = @"EXEC sp_select_sy_item_master_all";

            var datas = conn.Query<ItemMasterData>(query).ToList();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_ItemMasterListData_All", null), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<List<ItemMasterData>> Select_SearchItemMasterListData_By(ParamSearchItemMasterViewModel param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            int page = (param.start / param.length) + 1;
            var _offset = (page - 1) * param.length;
            var _pagesize = param.length;

            var p = new DynamicParameters();
            p.Add("@Offset", _offset);
            p.Add("@PageSize", _pagesize);

            p.Add("@product_code", param.product_code);
            p.Add("@product_name", param.product_name);
            p.Add("@status", param.status);

            p.Add("@comp_code", userInfo.comp_code);
            p.Add("@plant_code", userInfo.plant_code);

            var query = @"EXEC sp_select_sy_item_master_by 
                                 @Offset = @Offset
                                ,@PageSize = @PageSize
                                ,@product_code = @product_code
                                ,@product_name = @product_name
                                ,@status = @status
                                ,@comp_code = @comp_code
                                ,@plant_code = @plant_code
                        ";

            var datas = conn.Query<ItemMasterData>(query, p).ToList();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_SearchItemMasterListData_By", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<MessageReport> Update_ItemMasterInfo(ResultGetItemMasterInfoViewModel param, UserInfoModel userInfo)
        {
            using var dbContext = new ApplicationContext();
            dbContext.Database.SetConnectionString(_connectionStringContext);
            await dbContext.Database.OpenConnectionAsync();
            using var trans = dbContext.Database.BeginTransaction();

            var toDay = DateTimeServer.Now();
            var result = new MessageReport(false, "ERROR!");

            try
            {
                SyItemMaster itm = dbContext.SyItemMasters.Where(i => i.ItemCode == param.item_code).FirstOrDefault();
                itm.ItemName = param.item_name;
                itm.ItemGroup = param.item_group;
                itm.Status = param.status;
                itm.Remark1 = param.remark_1;
                itm.Remark2 = param.remark_2;
                itm.IsActive = true;
                itm.IsDeleted = false;
                itm.CompCode = userInfo.comp_code;
                itm.PlantCode = userInfo.plant_code;

                await dbContext.SaveChangesAsync();
                await trans.CommitAsync();
                result = new MessageReport(true, string.Format("Message: {0}", Constants.Result.Success));
                _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Update, this.GetType().Name, "Update_ItemMasterInfo", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                result = new MessageReport(false, string.Format("Error Code: {0} - Details: {1}", ErrorCodes.ItemMaster.Repo.Update_ItemMasterInfo, ex.Message));
                _logger.WriteError(errorCode: ErrorCodes.ItemMaster.Repo.Update_ItemMasterInfo, errorMessage: ex.Message, additionalInfo: result.message, exception: ex, user: _context.HttpContext.Session.GetString(Constants.Session.User));
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
            //p.Add("@item_code", param.item_code);
            //p.Add("@item_name", param.item_name);
            //p.Add("@item_group", param.item_group);
            //p.Add("@status", param.status);
            //p.Add("@remark_1", param.remark_1);
            //p.Add("@remark_2", param.remark_2);
            //p.Add("@modified_by", userInfo.username);

            //var query = $@"
            //                    UPDATE [dbo].[sy_item_master]
            //                    SET 
            //                         [item_name] = @item_name
            //                        ,[item_group] = @item_group
            //                        ,[status] = @status
            //                        ,[remark_1] = @remark_1
            //                        ,[remark_2] = @remark_2
            //                        ,[modified_by] = @modified_by
            //                        ,[modified_date] = GETDATE()
            //                    WHERE item_code = @item_code;
            //                ";

            //using (var trans = conn.BeginTransaction())
            //{
            //    conn.Execute(query, p, trans);
            //    trans.Commit();
            //}

            //conn.Close();
            #endregion
        }

        public async Task<MessageReport> Delete_Info(ResultGetItemMasterInfoViewModel param, UserInfoModel userInfo)
        {
            using var dbContext = new ApplicationContext();
            dbContext.Database.SetConnectionString(_connectionStringContext);
            await dbContext.Database.OpenConnectionAsync();
            using var trans = dbContext.Database.BeginTransaction();

            var toDay = DateTimeServer.Now();
            var result = new MessageReport(false, "ERROR!");

            try
            {
                SyItemMaster itm = dbContext.SyItemMasters.Where(i => i.ItemCode == param.item_code).FirstOrDefault();
                itm.IsActive = false;
                itm.IsDeleted = true;

                await dbContext.SaveChangesAsync();
                await trans.CommitAsync();
                result = new MessageReport(true, string.Format("Message: {0}", Constants.Result.Success));
                _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Delete, this.GetType().Name, "Delete_ItemMasterInfo", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                result = new MessageReport(false, string.Format("Error Code: {0} - Details: {1}", ErrorCodes.ItemMaster.Repo.Delete_ItemMasterInfo, ex.Message));
                _logger.WriteError(errorCode: ErrorCodes.ItemMaster.Repo.Delete_ItemMasterInfo, errorMessage: ex.Message, additionalInfo: result.message, exception: ex, user: _context.HttpContext.Session.GetString(Constants.Session.User));
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
