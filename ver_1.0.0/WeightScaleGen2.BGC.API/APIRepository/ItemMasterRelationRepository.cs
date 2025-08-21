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
using WeightScaleGen2.BGC.Models.ViewModels.ItemMasterRelation;

namespace WeightScaleGen2.BGC.API.APIRepository
{
    public class ItemMasterRelationRepository : IItemMasterRelationRepository
    {
        IDatabaseConnectionFactory _db;
        ILogger _logger;
        IConfiguration _configuration;
        private readonly IHttpContextAccessor _context;
        private readonly ApplicationContext _applicationContext;
        private readonly string _connectionStringContext;

        public ItemMasterRelationRepository(
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

        public async Task<MessageReport> Insert_ItemMasterRelationInfo(ResultGetItemMasterRelationInfoViewModel param, UserInfoModel userInfo)
        {
            using var dbContext = new ApplicationContext();
            dbContext.Database.SetConnectionString(_connectionStringContext);
            await dbContext.Database.OpenConnectionAsync();
            using var trans = dbContext.Database.BeginTransaction();

            var toDay = DateTimeServer.Now();
            var result = new MessageReport(false, "ERROR!");

            try
            {
                SyItemMasterRelation itmr = new SyItemMasterRelation
                {
                    ItemCode = param.item_code,
                    SupplierCode = param.supplier_code,
                    Humidity = param.humidity,
                    Gravity = param.gravity,
                    Status = param.status,
                    Remark1 = param.remark_1,
                    Remark2 = param.remark_2
                };

                await dbContext.SyItemMasterRelations.AddAsync(itmr);
                await dbContext.SaveChangesAsync();
                await trans.CommitAsync();
                result = new MessageReport(true, string.Format("Message: {0}", Constants.Result.Success));
                _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Insert, this.GetType().Name, "Insert_ItemMasterRelationInfo", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                result = new MessageReport(false, string.Format("Error Code: {0} - Details: {1}", ErrorCodes.ItemMasterRelation.Repo.Insert_ItemMasterRelationInfo, ex.Message));
                _logger.WriteError(errorCode: ErrorCodes.ItemMasterRelation.Repo.Insert_ItemMasterRelationInfo, errorMessage: ex.Message, additionalInfo: result.message, exception: ex, user: _context.HttpContext.Session.GetString(Constants.Session.User));
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
            //p.Add("@supplier_code", param.supplier_code);
            //p.Add("@humidity", param.humidity);
            //p.Add("@gravity", param.gravity);
            //p.Add("@status", param.status);
            //p.Add("@remark_1", param.remark_1);
            //p.Add("@remark_2", param.remark_2);

            //var query = $@"
            //                    INSERT INTO [dbo].[sy_item_master_relation]
            //                               ([item_code]
            //                               ,[supplier_code]
            //                               ,[humidity]
            //                               ,[gravity]
            //                               ,[status]
            //                               ,[remark_1]
            //                               ,[remark_2])
            //                         VALUES
            //                               (@item_code
            //                               ,@supplier_code
            //                               ,@humidity
            //                               ,@humidity
            //                               ,@status
            //                               ,@remark_1
            //                               ,@remark_2)
            //            ";

            //using (var trans = conn.BeginTransaction())
            //{
            //    conn.Execute(query, p, trans);
            //    trans.Commit();
            //}

            //conn.Close();
            #endregion
        }

        public async Task<ItemMasterRelationData> Select_ItemMasterRelationInfo(ParamItemMasterRelationInfo param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@id", param.id);

            var query = @"EXEC sp_select_sy_item_master_relation_by_id
                                @id = @id
            ";

            var datas = conn.Query<ItemMasterRelationData>(query, p).FirstOrDefault();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_ItemMasterRelationInfo", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<List<ItemMasterRelationData>> Select_ItemMasterRelationListData_All(UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var query = @"EXEC sp_select_sy_item_master_relation_all";

            var datas = conn.Query<ItemMasterRelationData>(query).ToList();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_ItemMasterRelationListData_All", null), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<List<ItemMasterRelationData>> Select_SearchItemMasterRelationListData_By(ParamSearchItemMasterRelationViewModel param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            int page = (param.start / param.length) + 1;
            var _offset = (page - 1) * param.length;
            var _pagesize = param.length;

            var p = new DynamicParameters();
            p.Add("@Offset", _offset);
            p.Add("@PageSize", _pagesize);

            p.Add("@product_code", param.product_code);
            p.Add("@supplier_code", param.supplier_code);

            var query = @"EXEC sp_select_sy_item_master_relation_by 
                                 @Offset = @Offset
                                ,@PageSize = @PageSize
                                ,@product_code = @product_code
                                ,@supplier_code = @supplier_code
                        ";

            var datas = conn.Query<ItemMasterRelationData>(query, p).ToList();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_SearchItemMasterRelationListData_By", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<MessageReport> Update_ItemMasterRelationInfo(ResultGetItemMasterRelationInfoViewModel param, UserInfoModel userInfo)
        {
            using var dbContext = new ApplicationContext();
            dbContext.Database.SetConnectionString(_connectionStringContext);
            await dbContext.Database.OpenConnectionAsync();
            using var trans = dbContext.Database.BeginTransaction();

            var toDay = DateTimeServer.Now();
            var result = new MessageReport(false, "ERROR!");

            try
            {
                SyItemMasterRelation itmR = dbContext.SyItemMasterRelations.Where(i => i.Id == param.id).FirstOrDefault();
                itmR.ItemCode = param.item_code;
                itmR.SupplierCode = param.supplier_code;
                itmR.Humidity = param.humidity;
                itmR.Gravity = param.gravity;
                itmR.Status = param.status;
                itmR.Remark1 = param.remark_1;
                itmR.Remark2 = param.remark_2;

                await dbContext.SaveChangesAsync();
                await trans.CommitAsync();
                result = new MessageReport(true, string.Format("Message: {0}", Constants.Result.Success));
                _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Update, this.GetType().Name, "Update_ItemMasterInfo", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                result = new MessageReport(false, string.Format("Error Code: {0} - Details: {1}", ErrorCodes.ItemMasterRelation.Repo.Update_ItemMasterRelationInfo, ex.Message));
                _logger.WriteError(errorCode: ErrorCodes.ItemMasterRelation.Repo.Update_ItemMasterRelationInfo, errorMessage: ex.Message, additionalInfo: result.message, exception: ex, user: _context.HttpContext.Session.GetString(Constants.Session.User));
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
            //p.Add("@item_code", param.item_code);
            //p.Add("@supplier_code", param.supplier_code);
            //p.Add("@humidity", param.humidity);
            //p.Add("@gravity", param.gravity);
            //p.Add("@status", param.status);
            //p.Add("@remark_1", param.remark_1);
            //p.Add("@remark_2", param.remark_2);

            //var query = $@"
            //                    UPDATE [dbo].[sy_item_master_relation]
            //                    SET 
            //                         [item_code] = @item_code
            //                        ,[supplier_code] = @supplier_code
            //                        ,[humidity] = @humidity
            //                        ,[gravity] = @gravity
            //                        ,[status] = @status
            //                        ,[remark_1] = @remark_1
            //                        ,[remark_2] = @remark_2
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

        public async Task<MessageReport> Delete_Info(ResultGetItemMasterRelationInfoViewModel param, UserInfoModel userInfo)
        {
            using var dbContext = new ApplicationContext();
            dbContext.Database.SetConnectionString(_connectionStringContext);
            await dbContext.Database.OpenConnectionAsync();
            using var trans = dbContext.Database.BeginTransaction();

            var toDay = DateTimeServer.Now();
            var result = new MessageReport(false, "ERROR!");

            try
            {
                SyItemMasterRelation itmRInfo = dbContext.SyItemMasterRelations.Where(i => i.Id == param.id).FirstOrDefault();

                if (itmRInfo != null)
                {
                    dbContext.Remove(itmRInfo);
                    await dbContext.SaveChangesAsync();
                    await trans.CommitAsync();

                    result = new MessageReport(true, string.Format("Message: {0}", Constants.Result.Success));
                    _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Delete, this.GetType().Name, "Delete_ItemMasterRelationInfo", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
                }
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                result = new MessageReport(false, string.Format("Error Code: {0} - Details: {1}", ErrorCodes.ItemMasterRelation.Repo.Delete_ItemMasterRelationInfo, ex.Message));
                _logger.WriteError(errorCode: ErrorCodes.ItemMasterRelation.Repo.Delete_ItemMasterRelationInfo, errorMessage: ex.Message, additionalInfo: result.message, exception: ex, user: _context.HttpContext.Session.GetString(Constants.Session.User));
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
