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
using WeightScaleGen2.BGC.Models.ViewModels.Department;

namespace WeightScaleGen2.BGC.API.APIRepository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        IDatabaseConnectionFactory _db;
        ILogger _logger;
        IConfiguration _configuration;
        private readonly IHttpContextAccessor _context;
        private readonly ApplicationContext _applicationContext;
        private readonly string _connectionStringContext;

        public DepartmentRepository(
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

        public async Task<MessageReport> Delete_Info(ResultGetDeptInfoViewModel param, UserInfoModel userInfo)
        {
            using var dbContext = new ApplicationContext();
            dbContext.Database.SetConnectionString(_connectionStringContext);
            await dbContext.Database.OpenConnectionAsync();
            using var trans = dbContext.Database.BeginTransaction();

            var toDay = DateTimeServer.Now();
            var result = new MessageReport(false, "ERROR!");

            try
            {
                SyDepartment syDepartment = dbContext.SyDepartments.Where(i => i.DeptCode == param.dept_code).FirstOrDefault();
                dbContext.SyDepartments.Remove(syDepartment);

                await dbContext.SaveChangesAsync();
                await trans.CommitAsync();
                result = new MessageReport(true, string.Format("Message: {0}", Constants.Result.Success));
                _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Delete, this.GetType().Name, "Delete_Info", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                result = new MessageReport(false, string.Format("Error Code: {0} - Details: {1}", ErrorCodes.Department.Repo.Delete_Info, ex.Message));
                _logger.WriteError(errorCode: ErrorCodes.Department.Repo.Delete_Info, errorMessage: ex.Message, additionalInfo: result.message, exception: ex, user: _context.HttpContext.Session.GetString(Constants.Session.User));
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
            //p.Add("@dept_code", param.dept_code);

            //var query = $@"
            //                    DELETE dbo.sy_department 
            //                    WHERE dept_code = @dept_code;
            //                ";

            //using (var trans = conn.BeginTransaction())
            //{
            //    conn.Execute(query, p, trans);
            //    trans.Commit();
            //}

            //conn.Close();
            #endregion
        }

        public async Task<MessageReport> Insert_Info(ResultGetDeptInfoViewModel param, UserInfoModel userInfo)
        {
            using var dbContext = new ApplicationContext();
            dbContext.Database.SetConnectionString(_connectionStringContext);
            await dbContext.Database.OpenConnectionAsync();
            using var trans = dbContext.Database.BeginTransaction();

            var toDay = DateTimeServer.Now();
            var result = new MessageReport(false, "ERROR!");

            try
            {
                SyDepartment syDepartment = new SyDepartment
                {
                    DeptCode = param.dept_code,
                    CompCode = "all",
                    PlantCode = "all",
                    ShortCode = param.short_code,
                    NameTh = param.name_th,
                    NameEn = param.name_en,
                    CreatedBy = userInfo.username,
                    CreatedDate = toDay,
                    IsActive = true,
                    IsDeleted = false,
                    IsAll = true
                };

                await dbContext.SyDepartments.AddAsync(syDepartment);
                await dbContext.SaveChangesAsync();
                await trans.CommitAsync();
                result = new MessageReport(true, string.Format("Message: {0}", Constants.Result.Success));
                _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Insert, this.GetType().Name, "Insert_Info", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                result = new MessageReport(false, string.Format("Error Code: {0} - Details: {1}", ErrorCodes.Department.Repo.Insert_Info, ex.Message));
                _logger.WriteError(errorCode: ErrorCodes.Department.Repo.Insert_Info, errorMessage: ex.Message, additionalInfo: result.message, exception: ex, user: _context.HttpContext.Session.GetString(Constants.Session.User));
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
            //p.Add("@dept_code", param.dept_code);
            //p.Add("@comp_code", "all");
            //p.Add("@plant_code", "all");
            //p.Add("@short_code", param.short_code);
            //p.Add("@name_th", param.name_th);
            //p.Add("@name_en", param.name_en);
            //p.Add("@created_by", userInfo.username);
            //p.Add("@is_active", true);
            //p.Add("@is_deleted", false);
            //p.Add("@is_all", true);

            //var query = $@"
            //                    INSERT INTO dbo.sy_department
            //                    (
            //                      dept_code
            //                     ,comp_code
            //                     ,plant_code
            //                     ,short_code
            //                     ,name_th
            //                     ,name_en
            //                     ,created_by
            //                     ,created_date
            //                     ,is_active
            //                     ,is_deleted
            //                     ,is_all
            //                    )
            //                    VALUES
            //                    (
            //                      @dept_code
            //                     ,@comp_code
            //                     ,@plant_code
            //                     ,@short_code
            //                     ,@name_th
            //                     ,@name_en
            //                     ,@created_by
            //                     ,GETDATE()
            //                     ,@is_active
            //                     ,@is_deleted
            //                     ,@is_all
            //                    );
            //            ";

            //using (var trans = conn.BeginTransaction())
            //{
            //    conn.Execute(query, p, trans);
            //    trans.Commit();
            //}

            //conn.Close();
            #endregion
        }

        public async Task<List<DepartmentData>> Select_DepartmentData_All()
        {
            using var conn = await _db.CreateConnectionAsync();

            var query = @"EXEC sp_select_dept_all";

            var datas = conn.Query<DepartmentData>(query).ToList();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_DepartmentData_All", null), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<List<DepartmentData>> Select_DepartmentData_By(ParamSearchDeptViewModel param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            int page = (param.start / param.length) + 1;
            var _offset = (page - 1) * param.length;
            var _pagesize = param.length;

            var p = new DynamicParameters();
            p.Add("@Offset", _offset);
            p.Add("@PageSize", _pagesize);

            p.Add("@dept_code", param.dept_code);
            p.Add("@name_th", param.dept_name);

            var query = @"EXEC sp_select_dept_by 
                                 @Offset = @Offset
                                ,@PageSize = @PageSize      
                                ,@dept_code = @dept_code
                                ,@name_th = @name_th
            ";

            var datas = conn.Query<DepartmentData>(query, p).ToList();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_DepartmentData_By", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<MessageReport> Update_Info(ResultGetDeptInfoViewModel param, UserInfoModel userInfo)
        {
            using var dbContext = new ApplicationContext();
            dbContext.Database.SetConnectionString(_connectionStringContext);
            await dbContext.Database.OpenConnectionAsync();
            using var trans = dbContext.Database.BeginTransaction();

            var toDay = DateTimeServer.Now();
            var result = new MessageReport(false, "ERROR!");

            try
            {
                SyDepartment syDepartment = dbContext.SyDepartments.Where(i => i.DeptCode == param.dept_code).FirstOrDefault();

                syDepartment.ShortCode = param.short_code;
                syDepartment.NameTh = param.name_th;
                syDepartment.NameEn = param.name_en;
                syDepartment.ModifiedBy = userInfo.username;
                syDepartment.ModifiedDate = toDay;

                await dbContext.SaveChangesAsync();
                await trans.CommitAsync();
                result = new MessageReport(true, string.Format("Message: {0}", Constants.Result.Success));
                _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Update, this.GetType().Name, "Update_Info", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                result = new MessageReport(false, string.Format("Error Code: {0} - Details: {1}", ErrorCodes.Department.Repo.Update_Info, ex.Message));
                _logger.WriteError(errorCode: ErrorCodes.Department.Repo.Update_Info, errorMessage: ex.Message, additionalInfo: result.message, exception: ex, user: _context.HttpContext.Session.GetString(Constants.Session.User));
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
            //p.Add("@dept_code", param.dept_code);
            //p.Add("@short_code", param.short_code);
            //p.Add("@name_th", param.name_th);
            //p.Add("@name_en", param.name_en);
            //p.Add("@modified_by", userInfo.username);

            //var query = $@"
            //                    UPDATE dbo.sy_department 
            //                    SET
            //                      short_code = @short_code
            //                     ,name_th = @name_th
            //                     ,name_en = @name_en
            //                     ,modified_by = @modified_by
            //                     ,modified_date = GETDATE()
            //                    WHERE
            //                      dept_code = @dept_code;
            //                ";

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
