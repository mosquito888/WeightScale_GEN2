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
using WeightScaleGen2.BGC.Models.ViewModels.Supplier;

namespace WeightScaleGen2.BGC.API.APIRepository
{
    public class SupplierRepository : ISupplierRepository
    {
        IDatabaseConnectionFactory _db;
        ILogger _logger;
        IConfiguration _configuration;
        private readonly IHttpContextAccessor _context;
        private readonly ApplicationContext _applicationContext;
        private readonly string _connectionStringContext;

        public SupplierRepository(
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

        public async Task<MessageReport> Insert_SupplierInfo(ResultGetSupplierInfoViewModel param, UserInfoModel userInfo)
        {
            using var dbContext = new ApplicationContext();
            dbContext.Database.SetConnectionString(_connectionStringContext);
            await dbContext.Database.OpenConnectionAsync();
            using var trans = dbContext.Database.BeginTransaction();

            var toDay = DateTimeServer.Now();
            var result = new MessageReport(false, "ERROR!");

            try
            {
                SySupplier itm = new SySupplier
                {
                    SupplierCode = param.supplier_code,
                    SupplierName = param.supplier_name,
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

                await dbContext.SySuppliers.AddAsync(itm);
                await dbContext.SaveChangesAsync();
                await trans.CommitAsync();
                result = new MessageReport(true, string.Format("Message: {0}", Constants.Result.Success));
                _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Insert, this.GetType().Name, "Insert_SupplierInfo", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                result = new MessageReport(false, string.Format("Error Code: {0} - Details: {1}", ErrorCodes.Supplier.Repo.Insert_SupplierInfo, ex.Message));
                _logger.WriteError(errorCode: ErrorCodes.Supplier.Repo.Insert_SupplierInfo, errorMessage: ex.Message, additionalInfo: result.message, exception: ex, user: _context.HttpContext.Session.GetString(Constants.Session.User));
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
            //p.Add("@supplier_name", param.supplier_name);
            //p.Add("@status", param.status);
            //p.Add("@remark_1", param.remark_1);
            //p.Add("@remark_2", param.remark_2);
            //p.Add("@created_by", userInfo.username);
            //p.Add("@is_active", true);
            //p.Add("@is_deleted", false);

            //var query = $@"
            //                    INSERT INTO [dbo].[sy_item_master]
            //                               ([supplier_name]
            //                               ,[status]
            //                               ,[remark_1]
            //                               ,[remark_2]
            //                               ,[created_by]
            //                               ,[created_date]
            //                               ,[is_active]
            //                               ,[is_deleted])
            //                         VALUES
            //                               (@supplier_name
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

        public async Task<SupplierData> Select_SupplierInfo(ParamSupplierInfo param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@supplier_code", param.supplier_code);
            p.Add("@comp_code", userInfo.comp_code);
            p.Add("@plant_code", userInfo.plant_code);

            var query = @"EXEC sp_select_sy_supplier_by_supplier_code
                                @supplier_code = @supplier_code
                               ,@comp_code = @comp_code
                               ,@plant_code = @plant_code
            ";

            var datas = conn.Query<SupplierData>(query, p).FirstOrDefault();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_SupplierInfo", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<List<SupplierData>> Select_SupplierListData_All(UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var query = @"EXEC sp_select_sy_supplier_all";

            var datas = conn.Query<SupplierData>(query).ToList();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_SupplierListData_All", null), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<List<SupplierData>> Select_SearchSupplierListData_By(ParamSearchSupplierViewModel param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            int page = (param.start / param.length) + 1;
            var _offset = (page - 1) * param.length;
            var _pagesize = param.length;

            var p = new DynamicParameters();
            p.Add("@Offset", _offset);
            p.Add("@PageSize", _pagesize);

            p.Add("@supplier_code", param.supplier_code);
            p.Add("@supplier_name", param.supplier_name);
            p.Add("@status", param.status);
            p.Add("@comp_code", userInfo.comp_code);
            p.Add("@plant_code", userInfo.plant_code);

            var query = @"EXEC sp_select_sy_supplier_by 
                                 @Offset = @Offset
                                ,@PageSize = @PageSize
                                ,@supplier_code = @supplier_code
                                ,@supplier_name = @supplier_name
                                ,@status = @status
                                ,@comp_code = @comp_code
                                ,@plant_code = @plant_code
                        ";

            var datas = conn.Query<SupplierData>(query, p).ToList();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_SearchSupplierListData_By", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<MessageReport> Update_SupplierInfo(ResultGetSupplierInfoViewModel param, UserInfoModel userInfo)
        {
            using var dbContext = new ApplicationContext();
            dbContext.Database.SetConnectionString(_connectionStringContext);
            await dbContext.Database.OpenConnectionAsync();
            using var trans = dbContext.Database.BeginTransaction();

            var toDay = DateTimeServer.Now();
            var result = new MessageReport(false, "ERROR!");

            try
            {
                SySupplier sup = dbContext.SySuppliers.Where(i => i.SupplierCode == param.supplier_code).FirstOrDefault();
                sup.SupplierName = param.supplier_name;
                sup.Status = param.status;
                sup.Remark1 = param.remark_1;
                sup.Remark2 = param.remark_2;
                sup.IsActive = true;
                sup.IsDeleted = false;
                sup.CompCode = userInfo.comp_code;
                sup.PlantCode = userInfo.plant_code;

                await dbContext.SaveChangesAsync();
                await trans.CommitAsync();
                result = new MessageReport(true, string.Format("Message: {0}", Constants.Result.Success));
                _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Update, this.GetType().Name, "Update_SupplierInfo", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                result = new MessageReport(false, string.Format("Error Code: {0} - Details: {1}", ErrorCodes.Supplier.Repo.Update_SupplierInfo, ex.Message));
                _logger.WriteError(errorCode: ErrorCodes.Supplier.Repo.Update_SupplierInfo, errorMessage: ex.Message, additionalInfo: result.message, exception: ex, user: _context.HttpContext.Session.GetString(Constants.Session.User));
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
            //p.Add("@supplier_code", param.item_code);
            //p.Add("@supplier_name", param.item_name);
            //p.Add("@status", param.status);
            //p.Add("@remark_1", param.remark_1);
            //p.Add("@remark_2", param.remark_2);
            //p.Add("@modified_by", userInfo.username);

            //var query = $@"
            //                    UPDATE [dbo].[sy_supplier]
            //                    SET 
            //                         [supplier_name] = @supplier_name
            //                        ,[status] = @status
            //                        ,[remark_1] = @remark_1
            //                        ,[remark_2] = @remark_2
            //                        ,[modified_by] = @modified_by
            //                        ,[modified_date] = GETDATE()
            //                    WHERE supplier_code = @supplier_code;
            //                ";

            //using (var trans = conn.BeginTransaction())
            //{
            //    conn.Execute(query, p, trans);
            //    trans.Commit();
            //}

            //conn.Close();
            #endregion
        }

        public async Task<MessageReport> Delete_Info(ResultGetSupplierInfoViewModel param, UserInfoModel userInfo)
        {
            using var dbContext = new ApplicationContext();
            dbContext.Database.SetConnectionString(_connectionStringContext);
            await dbContext.Database.OpenConnectionAsync();
            using var trans = dbContext.Database.BeginTransaction();

            var toDay = DateTimeServer.Now();
            var result = new MessageReport(false, "ERROR!");

            try
            {
                SySupplier sup = dbContext.SySuppliers.Where(i => i.SupplierCode == param.supplier_code).FirstOrDefault();
                sup.IsActive = false;
                sup.IsDeleted = true;

                await dbContext.SaveChangesAsync();
                await trans.CommitAsync();
                result = new MessageReport(true, string.Format("Message: {0}", Constants.Result.Success));
                _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Delete, this.GetType().Name, "Delete_SupplierInfo", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                result = new MessageReport(false, string.Format("Error Code: {0} - Details: {1}", ErrorCodes.Supplier.Repo.Delete_SupplierInfo, ex.Message));
                _logger.WriteError(errorCode: ErrorCodes.Supplier.Repo.Delete_SupplierInfo, errorMessage: ex.Message, additionalInfo: result.message, exception: ex, user: _context.HttpContext.Session.GetString(Constants.Session.User));
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
