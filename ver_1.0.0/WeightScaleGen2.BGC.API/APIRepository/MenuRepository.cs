using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NuGet.Protocol;
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
using WeightScaleGen2.BGC.Models.ViewModels.Menu;
using WeightScaleGen2.BGC.Models.ViewModels.Role;

namespace WeightScaleGen2.BGC.API.APIRepository
{
    public class MenuRepository : IMenuRepository
    {
        IDatabaseConnectionFactory _db;
        ISecurityCommon _securityCommon;
        ILogger _logger;
        IConfiguration _configuration;
        private readonly IHttpContextAccessor _context;
        private readonly ApplicationContext _applicationContext;
        private readonly string _connectionStringContext;

        public MenuRepository(
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

        public async void Delete_Role(ResultRoleInfo param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@roleId", this._securityCommon.DecryptDataUrlEncoder(param.role_id));

            var query = @"
                          DELETE sy_role_item WHERE role_id = @roleId
                          DELETE sy_role WHERE role_id = @roleId
                        ";

            using (var trans = conn.BeginTransaction())
            {
                conn.Execute(query, p, trans);
                trans.Commit();
            }

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Delete, this.GetType().Name, "Delete_Role", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
        }

        public async void Insert_MenuSection(int roleId, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@roleId", roleId);
            p.Add("@username", userInfo.username);

            var query = @"
                            INSERT INTO [dbo].[sy_role_item]
                                               ([role_id]
                                               ,[menu_section_id]
                                               ,[is_display]
                                               ,[is_action]
                                               ,[created_by]
                                               ,[created_date]
                                               ,[modified_by]
                                               ,[modified_date]
                                               ,[is_active]
                                               ,[is_deleted])
                                    SELECT 
	                                     @roleId			AS [role_id]
	                                    ,m.menu_section_id	AS [menu_section_id]
	                                    ,1					AS [is_display]
	                                    ,0					AS [is_action]
	                                    ,@username		    AS [created_by]
	                                    ,GETDATE()			AS [created_date]
	                                    ,NULL				AS [modified_by]
	                                    ,NULL				AS [modified_date]
	                                    ,1					AS [is_active]
	                                    ,0					AS [is_deleted]
                                    FROM sy_menu_section AS m

                        ";

            using (var trans = conn.BeginTransaction())
            {
                conn.Execute(query, p, trans);
                trans.Commit();
            }

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Insert, this.GetType().Name, "Insert_MenuSection", roleId.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
        }

        public async Task<int> Insert_Role(ResultRoleInfo param, UserInfoModel userInfo)
        {
            int newIdentity = 0;

            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@id", 0, DbType.Int32, ParameterDirection.Output);
            p.Add("@role_name", param.role_name);
            p.Add("@role_desc", param.role_desc);
            p.Add("@username", userInfo.username);

            var query = @"
                        INSERT INTO [dbo].[sy_role]
                                   ([role_name]
                                   ,[role_desc]
                                   ,[created_by]
                                   ,[created_date]
                                   ,[modified_by]
                                   ,[modified_date]
                                   ,[is_active]
                                   ,[is_deleted]
                                   ,[is_super_role])
                             VALUES
                                   (@role_name
                                   ,@role_desc
                                   ,@username
                                   ,GETDATE()
                                   ,NULL
                                   ,NULL
                                   ,1
                                   ,0
                                   ,0)

                        SELECT @id = @@IDENTITY

                        ";

            using (var trans = conn.BeginTransaction())
            {
                conn.Execute(query, p, trans);
                trans.Commit();
            }

            conn.Close();

            newIdentity = p.Get<int>("@id");

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Insert, this.GetType().Name, "Insert_Role", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return newIdentity;
        }

        public async void Insert_User(PramGetMenuViewModel param, UserInfoModel userInfo)
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

        public async Task<List<MenuData>> Select_MenuRole(int param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var query = @"
                        SELECT
                            tbM.menu_definition,
                            tbM.menu_id,
                            tbM.menu_name,
                            tbM.display_name,
                            tbM.icon,
                            tbM.url_controller,
                            tbM.url,
                            tbM.list_no,
                            tbM.menu_level,
                            tbM.parent_menu_id
                        FROM sy_menu tbM
                        LEFT JOIN sy_menu_section tbMS ON tbMS.menu_id = tbM.menu_id
                        LEFT JOIN sy_role_item tbRI ON tbRI.menu_section_id = tbMS.menu_section_id
                        LEFT JOIN sy_role tbR ON tbR.role_id = tbRI.role_id
                        WHERE 
                                tbRI.role_id = @roleId
                            AND tbRI.is_active = 1
                            AND tbRI.is_deleted = 0
						    AND tbM.is_active = 1
						    AND tbM.is_deleted = 0
                        GROUP BY 
                        tbM.menu_definition,
                        tbM.menu_id,
                        tbM.menu_name,
                        tbM.display_name,
                        tbM.icon,
                        tbM.url_controller,
                        tbM.url,
                        tbM.list_no,
                        tbM.menu_level,
                        tbM.parent_menu_id
				        ORDER BY tbM.list_no
                        ";

            var datas = conn.Query<MenuData>(query, new { roleId = param }).ToList();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_MenuRole", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<List<MenuData>> Select_MenuSectionRole(int param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var query = @"
                        SELECT
                            tbRI.role_item_id,
                            tbM.menu_id,
                            tbM.menu_name,
                            tbM.display_name,
                            tbM.icon,
                            tbM.url_controller,
                            tbM.url,
                            tbM.list_no,
                            tbM.menu_level,
                            tbM.parent_menu_id,
                            tbMS.menu_section_name,
                            tbMS.menu_section_name_display,
                            tbMS.list_no section_no,
                            tbRI.is_action,
                            tbRI.is_display
                        FROM sy_menu tbM
                        LEFT JOIN sy_menu_section tbMS ON tbMS.menu_id = tbM.menu_id
                        LEFT JOIN sy_role_item tbRI ON tbRI.menu_section_id = tbMS.menu_section_id
                        LEFT JOIN sy_role tbR ON tbR.role_id = tbRI.role_id
                        WHERE 
                                tbRI.role_id = @roleId
                            AND tbMS.is_active = 1
                            AND tbMS.is_deleted = 0
                            AND tbRI.is_active = 1
                            AND tbRI.is_deleted = 0
                            AND tbR.is_active = 1
                            AND tbR.is_deleted = 0
						    AND tbM.is_active = 1
						    AND tbM.is_deleted = 0
                        GROUP BY 
                        tbRI.role_item_id,
                        tbM.menu_id,
                        tbM.menu_name,
                        tbM.display_name,
                        tbM.icon,
                        tbM.url_controller,
                        tbM.url,
                        tbM.list_no,
                        tbM.menu_level,
                        tbM.parent_menu_id,
                        tbMS.menu_section_name,
                        tbMS.menu_section_name_display,
                        tbMS.list_no,
                        tbRI.is_action,
                        tbRI.is_display
                        ";

            var datas = conn.Query<MenuData>(query, new { roleId = param }).ToList();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_MenuSectionRole", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<List<MenuData>> Select_MenuSectionUser(PramGetMenuViewModel param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@username", param.username);

            var query = @"EXEC sp_select_sy_menu_section_user @username = @username";

            var datas = conn.Query<MenuData>(query, p).ToList();

            conn.Close();

            //_logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_MenuSectionUser", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<List<MenuData>> Select_MenuUser(PramGetMenuViewModel param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@username", param.username);

            var query = @"EXEC sp_select_sy_menu_user @username = @username";

            var datas = conn.Query<MenuData>(query, p).ToList();

            conn.Close();

            //_logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_MenuUser", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<RoleData> Select_Role(ResultRoleInfo param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@role_id", this._securityCommon.DecryptDataUrlEncoder(param.role_id));

            var query = @"
                            SELECT *
                            FROM sy_role 
                            WHERE 
	                            is_active = 1
                            AND is_deleted = 0
                            AND role_id = @role_id
                        ";

            var datas = conn.Query<RoleData>(query, p).FirstOrDefault();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_Role", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<int> Select_RoleUsing(ResultRoleInfo param, UserInfoModel userInfo)
        {
            int count = 0;

            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@count_usgin", 0, DbType.Int32, ParameterDirection.Output);
            p.Add("@role_id", this._securityCommon.DecryptDataUrlEncoder(param.role_id));

            var query = @"
                            DECLARE @count int = 0;
                            SET @count = (SELECT COUNT(*) FROM sy_user WHERE role_id = @role_id)

                            SELECT @count_usgin = @count
                        ";

            using (var trans = conn.BeginTransaction())
            {
                conn.Execute(query, p, trans);
                trans.Commit();
            }

            conn.Close();

            count = p.Get<int>("@count_usgin");

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_RoleUsing", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return count;
        }

        public async Task<UserData> Select_User_ByUsername(PramGetMenuViewModel param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var query = @"
                        SELECT * 
                        FROM sy_user tblU
                        WHERE 
                            tblU.username = @username
                        AND tblU.is_active = 1
                        AND tblU.is_deleted = 0
                        ";

            var datas = conn.Query<UserData>(query, new { username = param.username }).FirstOrDefault();

            conn.Close();

            //_logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_User_ByUsername", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async void Update_Role(ResultRoleInfo param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@roleId", this._securityCommon.DecryptDataUrlEncoder(param.role_id));
            p.Add("@role_name", param.role_name);
            p.Add("@role_desc", param.role_desc);
            p.Add("@username", userInfo.username);

            var query = @"
                            UPDATE [dbo].[sy_role]
                               SET [role_name] = @role_name
                                  ,[role_desc] = @role_desc
                                  ,[modified_by] = @username
                                  ,[modified_date] = GETDATE()
                             WHERE role_id = @roleId
                        ";

            using (var trans = conn.BeginTransaction())
            {
                conn.Execute(query, p, trans);
                trans.Commit();
            }

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Update_Role", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
        }

        public async void Update_RoleItem(List<UpdateRoleItemSection> param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            using (var trans = conn.BeginTransaction())
            {
                foreach (UpdateRoleItemSection index in param)
                {
                    var p = new DynamicParameters();
                    p.Add("@roleItemId", this._securityCommon.DecryptDataUrlEncoder(index.role_item_id));
                    p.Add("@isAction", index.is_action);
                    p.Add("@isDisplay", index.is_display);
                    var query = @"UPDATE sy_role_item
                        SET is_display = @isDisplay,
                            is_action = @isAction
                        WHERE role_item_id = @roleItemId;";

                    conn.Execute(query, p, trans);
                }
                trans.Commit();
            }

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Update_RoleItem", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
        }

        public async void Update_RoleSelectItem(UpdateRoleItemSection param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            using (var trans = conn.BeginTransaction())
            {
                var p = new DynamicParameters();
                p.Add("@roleItemId", param.role_item_id);

                var query = $@"
                                UPDATE sy_role_item
                                SET 
                                  is_action = CASE WHEN is_action = 1 THEN 0 ELSE 1 END
                                WHERE role_item_id = @roleItemId
                            ";

                conn.Execute(query, p, trans);
                trans.Commit();
            }

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Update_RoleSelectItem", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
        }
    }
}
