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
using WeightScaleGen2.BGC.Models.ViewModels.Employee;

namespace WeightScaleGen2.BGC.API.APIRepository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        IDatabaseConnectionFactory _db;
        ILogger _logger;
        IConfiguration _configuration;
        private readonly IHttpContextAccessor _context;
        private readonly ApplicationContext _applicationContext;
        private readonly string _connectionStringContext;

        public EmployeeRepository(
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

        public async Task<MessageReport> Insert_EmployeeInfo(ResultGetEmpInfoViewModel param, UserInfoModel userInfo)
        {
            using var dbContext = new ApplicationContext();
            dbContext.Database.SetConnectionString(_connectionStringContext);
            await dbContext.Database.OpenConnectionAsync();
            using var trans = dbContext.Database.BeginTransaction();

            var toDay = DateTimeServer.Now();
            var result = new MessageReport(false, "ERROR!");

            try
            {
                SyEmp emp = new SyEmp
                {
                    EmpCode = param.emp_code,
                    CompCode = param.comp_code,
                    PlantCode = param.plant_code,
                    DeptCode = param.dept_code,
                    Name = param.name,
                    PayType = param.pay_type,
                    WorkStartDate = param.work_start_date != null ? param.work_start_date.Value : null,
                    AddrLine1 = param.addr_line1,
                    AddrLine2 = param.addr_line2,
                    Phone = param.phone,
                    Email = param.email,
                    CreatedBy = userInfo.username,
                    CreatedDate = toDay,
                    IsActive = true,
                    IsDeleted = false
                };

                await dbContext.SyEmps.AddAsync(emp);
                await dbContext.SaveChangesAsync();
                await trans.CommitAsync();
                result = new MessageReport(true, string.Format("Message: {0}", Constants.Result.Success));
                _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Insert, this.GetType().Name, "Insert_EmployeeInfo", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                result = new MessageReport(false, string.Format("Error Code: {0} - Details: {1}", ErrorCodes.Employee.Repo.Insert_EmployeeInfo, ex.Message));
                _logger.WriteError(errorCode: ErrorCodes.Employee.Repo.Insert_EmployeeInfo, errorMessage: ex.Message, additionalInfo: result.message, exception: ex, user: _context.HttpContext.Session.GetString(Constants.Session.User));
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
            //p.Add("@emp_code", param.emp_code);
            //p.Add("@comp_code", param.comp_code);
            //p.Add("@plant_code", param.plant_code);
            //p.Add("@dept_code", param.dept_code);
            //p.Add("@name", param.name);
            //p.Add("@pay_type", param.pay_type);
            //p.Add("@work_start_date", param.work_start_date != null ? param.work_start_date.Value.Date : null);
            //p.Add("@addr_line1", param.addr_line1);
            //p.Add("@addr_line2", param.addr_line2);
            //p.Add("@phone", param.phone);
            //p.Add("@email", param.email);
            //p.Add("@created_by", userInfo.username);
            //p.Add("@is_active", true);
            //p.Add("@is_deleted", false);

            //var query = $@"
            //                    INSERT INTO [dbo].[sy_emp]
            //                               ([emp_code]
            //                               ,[comp_code]
            //                               ,[plant_code]
            //                               ,[dept_code]
            //                               ,[name]
            //                               ,[pay_type]
            //                               ,[work_start_date]
            //                               ,[addr_line1]
            //                               ,[addr_line2]
            //                               ,[phone]
            //                               ,[email]
            //                               ,[created_by]
            //                               ,[created_date]
            //                               ,[is_active]
            //                               ,[is_deleted])
            //                         VALUES
            //                               (@emp_code
            //                               ,@comp_code
            //                               ,@plant_code
            //                               ,@dept_code
            //                               ,@name
            //                               ,@pay_type
            //                               ,@work_start_date
            //                               ,@addr_line1
            //                               ,@addr_line2
            //                               ,@phone
            //                               ,@email
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

        public async Task<EmployeeData> Select_EmployeeInfo(ParamEmpInfo param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@emp_code", param.emp_code);

            var query = @"EXEC sp_select_sy_emp_by_code
                                @emp_code = @emp_code
            ";

            var datas = conn.Query<EmployeeData>(query, p).FirstOrDefault();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_EmployeeInfo", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<List<EmployeeData>> Select_EmployeeListData_All(UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var query = @"EXEC sp_select_sy_emp_all";

            var datas = conn.Query<EmployeeData>(query).ToList();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_EmployeeListData_All", null), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<List<EmployeeData>> Select_SearchEmployeeListData_By(ParamSearchEmpViewModel param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            int page = (param.start / param.length) + 1;
            var _offset = (page - 1) * param.length;
            var _pagesize = param.length;

            var p = new DynamicParameters();
            p.Add("@Offset", _offset);
            p.Add("@PageSize", _pagesize);

            p.Add("@emp_code", param.emp_code);
            p.Add("@name", param.name);

            var query = @"EXEC sp_select_sy_emp_by 
                                 @Offset = @Offset
                                ,@PageSize = @PageSize
                                ,@emp_code = @emp_code
                                ,@name = @name
                        ";

            var datas = conn.Query<EmployeeData>(query, p).ToList();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_SearchEmployeeListData_By", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<MessageReport> Update_EmployeeInfo(ResultGetEmpInfoViewModel param, UserInfoModel userInfo)
        {
            using var dbContext = new ApplicationContext();
            dbContext.Database.SetConnectionString(_connectionStringContext);
            await dbContext.Database.OpenConnectionAsync();
            using var trans = dbContext.Database.BeginTransaction();

            var toDay = DateTimeServer.Now();
            var result = new MessageReport(false, "ERROR!");

            try
            {
                SyEmp emp = dbContext.SyEmps.Where(i => i.EmpCode == param.emp_code).FirstOrDefault();

                emp.CompCode = param.comp_code;
                emp.PlantCode = param.plant_code;
                emp.DeptCode = param.dept_code;
                emp.Name = param.name;
                emp.PayType = param.pay_type;
                //emp.WorkStartDate = param.work_start_date != null ? param.work_start_date.Value : null;
                emp.AddrLine1 = param.addr_line1;
                emp.AddrLine2 = param.addr_line2;
                emp.Phone = param.phone;
                emp.Email = param.email;
                emp.ModifiedBy = userInfo.username;
                emp.ModifiedDate = toDay;
                emp.IsActive = true;
                emp.IsDeleted = false;

                await dbContext.SaveChangesAsync();
                await trans.CommitAsync();
                result = new MessageReport(true, string.Format("Message: {0}", Constants.Result.Success));
                _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Update, this.GetType().Name, "Update_EmployeeInfo", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                result = new MessageReport(false, string.Format("Error Code: {0} - Details: {1}", ErrorCodes.Employee.Repo.Update_EmployeeInfo, ex.Message));
                _logger.WriteError(errorCode: ErrorCodes.Employee.Repo.Update_EmployeeInfo, errorMessage: ex.Message, additionalInfo: result.message, exception: ex, user: _context.HttpContext.Session.GetString(Constants.Session.User));
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
            //p.Add("@emp_code", param.emp_code);
            //p.Add("@comp_code", param.comp_code);
            //p.Add("@plant_code", param.plant_code);
            //p.Add("@dept_code", param.dept_code);
            //p.Add("@name", param.name);
            //p.Add("@pay_type", param.pay_type);
            //p.Add("@work_start_date", param.work_start_date);
            //p.Add("@addr_line1", param.addr_line1);
            //p.Add("@addr_line2", param.addr_line2);
            //p.Add("@phone", param.phone);
            //p.Add("@email", param.email);
            //p.Add("@modified_by", userInfo.username);

            //var query = $@"
            //                    UPDATE [dbo].[sy_emp]
            //                    SET 
            //                         [comp_code] = @comp_code
            //                        ,[plant_code] = @plant_code
            //                        ,[dept_code] = @dept_code
            //                        ,[name] = @name
            //                        ,[pay_type] = @pay_type
            //                        ,[work_start_date] = @work_start_date
            //                        ,[addr_line1] = @addr_line1
            //                        ,[addr_line2] = @addr_line2
            //                        ,[phone] = @phone
            //                        ,[email] = @email
            //                        ,[modified_by] = @modified_by
            //                        ,[modified_date] = GETDATE()
            //                    WHERE emp_code = @emp_code;
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
