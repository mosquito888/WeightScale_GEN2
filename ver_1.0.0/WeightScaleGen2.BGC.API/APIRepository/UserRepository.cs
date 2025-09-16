using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NuGet.Protocol;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.API.APIRepository.Interface;
using WeightScaleGen2.BGC.API.Common;
using WeightScaleGen2.BGC.API.Common.Logger;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.DBModelsEF;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.User;

namespace WeightScaleGen2.BGC.API.APIRepository
{
    public class UserRepository : IUserRepository
    {
        IDatabaseConnectionFactory _db;
        ISecurityCommon _securityCommon;
        ILogger _logger;
        IConfiguration _configuration;
        private readonly IHttpContextAccessor _context;
        private readonly ApplicationContext _applicationContext;
        private readonly string _connectionStringContext;

        public UserRepository(
            IDatabaseConnectionFactory db,
            ISecurityCommon securityCommon,
            ILogger logger,
            IConfiguration configuration,
            IHttpContextAccessor context,
            ApplicationContext applicationContext)
        {
            _db = db;
            _securityCommon = securityCommon;
            _logger = logger;
            _configuration = configuration;
            _context = context;
            _applicationContext = applicationContext;
            _applicationContext.Database.SetConnectionString(_configuration.GetConnectionString("DBConnection"));
            _connectionStringContext = _configuration.GetConnectionString("DBConnection");
        }

        public async void Insert_User(ResultGetUserInfo param)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@id", 0, DbType.Int32, ParameterDirection.Output);
            p.Add("@roleId", 1); // default blank
            p.Add("@name", param.name);
            p.Add("@username", param.username);
            p.Add("@emp_code", "00000000"); // default blank

            var query = @"
                        INSERT INTO sy_user
                        (
                            role_id,
                            emp_code,
                            name,
                            username,
                            created_by,
                            created_date,
                            is_active,
                            is_deleted
                        )
                        VALUES
                        (
                            @roleId,
                            @emp_code,
                            @name,
                            @username,
                            'system',
                            GETDATE(),
                            1,
                            0
                        );

                        SELECT @id = @@IDENTITY

                        ";

            using (var trans = conn.BeginTransaction())
            {
                conn.Execute(query, p, trans);
                trans.Commit();
            }

            conn.Close();

            int newIdentity = p.Get<int>("@id");

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Insert, this.GetType().Name, "Insert_User", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
        }

        public async void Upload_Image(ParamUploadImage param)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@file_base6", param.file);
            var query = @"INSERT INTO file_upload_demo
                           (file_name
                           ,file_ext
                           ,file_base6)
                            VALUES
                           ('sldwsssxxx'
                           ,'sldwsssxxx'
                           ,@file_base6);";

            using (var trans = conn.BeginTransaction())
            {
                conn.Execute(query, p, trans);
                trans.Commit();
            }

            conn.Close();
        }

        public async Task<MessageReport> Update_User(ParamUpdateUser param, UserInfoModel userInfo)
        {
            using var dbContext = new ApplicationContext();
            dbContext.Database.SetConnectionString(_connectionStringContext);
            await dbContext.Database.OpenConnectionAsync();
            using var trans = dbContext.Database.BeginTransaction();

            var toDay = DateTimeServer.Now();
            var result = new MessageReport(false, "ERROR!");

            try
            {
                var roleId = Convert.ToInt64(this._securityCommon.DecryptDataUrlEncoder(param.role_id));
                var userId = Convert.ToInt64(this._securityCommon.DecryptDataUrlEncoder(param.user_id));

                SyUser user = dbContext.SyUsers.Find(userId);

                user.RoleId = roleId;
                user.EmpCode = param.emp_code;

                user.ModifiedBy = userInfo.username;
                user.ModifiedDate = toDay;

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
            //p.Add("@roleId", this._securityCommon.DecryptDataUrlEncoder(param.role_id));
            //p.Add("@userId", this._securityCommon.DecryptDataUrlEncoder(param.user_id));
            //p.Add("@empCode", param.emp_code);

            //var query = @"
            //            UPDATE sy_user 
            //            SET 
            //                 role_id = @roleId
            //                ,emp_code = @empCode
            //            WHERE user_id = @userId;
            //            ";

            //using (var trans = conn.BeginTransaction())
            //{
            //    conn.Execute(query, p, trans);
            //    trans.Commit();
            //}

            //conn.Close();
            #endregion
        }

        public async Task<UserData> Select_User_ById(int param)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@userId", param);

            var query = @"SELECT 
                            tbu.* 
                        FROM sy_user tbu
                        WHERE 
                        tbu.is_active = 1 
                        AND tbu.is_deleted = 0
                        AND tbu.user_id = @userId;";

            var datas = conn.Query<UserData>(query, p).FirstOrDefault();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_User_ById", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<UserData> Select_User_ByUsername(string username)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@username", username);

            var query = @"
                        SELECT 
                          tbu.user_id
                         ,tbu.role_id
                         ,(SELECT TOP 1 sr.role_name FROM sy_role sr WHERE sr.role_id = tbu.role_id) AS role_name
                         ,tbu.emp_code
                         ,tbu.name
                         ,tbu.username
                         ,tbu.password
                         ,tbu.created_by
                         ,tbu.created_date
                         ,tbu.modified_by
                         ,tbu.modified_date
                         ,tbu.is_active
                         ,tbu.is_deleted
                         ,sp.serial_port
                        FROM sy_user tbu
                        LEFT JOIN sy_emp sye ON tbu.emp_code = sye.emp_code
                        LEFT JOIN sy_plant sp ON sye.plant_code = sp.plant_code
                        WHERE 
                        tbu.is_active = 1 
                        AND tbu.is_deleted = 0
                        AND tbu.username = @username;";

            var datas = conn.Query<UserData>(query, p).FirstOrDefault();

            conn.Close();

            //_logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_User_ByUsername", username.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<UserData> Select_User_ByName(string name)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@name", name);

            var query = @"
                        SELECT 
                          tbu.user_id
                         ,tbu.role_id
                         ,(SELECT TOP 1 sr.role_name FROM sy_role sr WHERE sr.role_id = tbu.role_id) AS role_name
                         ,tbu.emp_code
                         ,tbu.name
                         ,tbu.username
                         ,tbu.password
                         ,tbu.created_by
                         ,tbu.created_date
                         ,tbu.modified_by
                         ,tbu.modified_date
                         ,tbu.is_active
                         ,tbu.is_deleted 
                        FROM sy_user tbu
                        WHERE 
                        tbu.is_active = 1 
                        AND tbu.is_deleted = 0
                        AND tbu.name = @name;";

            var datas = conn.Query<UserData>(query, p).FirstOrDefault();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_User_ByName", name.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<List<UserData>> Select_User(ParamSearchUser param)
        {
            using var conn = await _db.CreateConnectionAsync();

            int page = (param.start / param.length) + 1;
            int? roleId = null;
            if (!String.IsNullOrEmpty(param.role_id))
            {
                roleId = int.Parse(this._securityCommon.DecryptDataUrlEncoder(param.role_id));
            }
            var p = new DynamicParameters();
            p.Add("@Search", param.name);
            p.Add("@RoleID", roleId);
            p.Add("@Offset", (page - 1) * param.length);
            p.Add("@PageSize", param.length);

            var query = @"SELECT COUNT(*) OVER() AS total_record,
                            tbu.*,tbr.role_name 
                        FROM sy_user tbu
                        LEFT JOIN sy_role tbr ON tbr.role_id = tbu.role_id
                        WHERE 
                        tbu.is_active = 1 
                        AND tbu.is_deleted = 0
                        AND 
                        (@Search IS NULL 
	                        OR (tbu.name Like '%' + @Search + '%')
	                        OR (tbu.username Like '%' + @Search + '%')
                        )
                        AND
                        ( @RoleID IS NULL
	                        OR (tbr.role_id = @RoleID)
                        )
                        ORDER BY created_date
                        OFFSET @Offset ROWS
                        FETCH NEXT @PageSize ROWS ONLY;";

            var datas = conn.Query<UserData>(query, p).ToList();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_User", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<List<RoleData>> Select_RoleDll()
        {
            using var conn = await _db.CreateConnectionAsync();

            var query = @"
                            SELECT *
                            FROM sy_role 
                            WHERE 
	                            is_active = 1
                            AND is_deleted = 0
                        ";

            var datas = conn.Query<RoleData>(query).ToList();

            conn.Close();

            //_logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_RoleDll", null), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<List<FileData>> Select_ImageAll()
        {
            using var conn = await _db.CreateConnectionAsync();

            var query = @"SELECT * 
                        FROM file_upload_demo;";

            var datas = conn.Query<FileData>(query).ToList();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_ImageAll", null), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<UserData> Select_User_ByUsernamePassword(ParamLoginUser param)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@username", param.username);
            p.Add("@password", this._securityCommon.EncryptDataUrlEncoder(param.password));

            var query = @"EXEC [sp_user_login] @username = @username, @password = @password";

            var datas = conn.Query<UserData>(query, p).FirstOrDefault();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_User_ByName", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }
    }
}
