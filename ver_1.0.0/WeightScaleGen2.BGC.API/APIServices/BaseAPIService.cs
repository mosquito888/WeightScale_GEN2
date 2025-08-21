using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.API.Common;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;

namespace WeightScaleGen2.BGC.API.APIServices
{
    public class BaseAPIService
    {
        IDatabaseConnectionFactory _db;
        ISecurityCommon _securityCommon;

        public BaseAPIService(IDatabaseConnectionFactory db, ISecurityCommon securityCommon)
        {
            _db = db;
            _securityCommon = securityCommon;
        }

        public async Task<UserInfoModel> _getUserInfo(string username)
        {
            using var conn = await _db.CreateConnectionAsync();

            if (username.Equals(Constants.Session.Common) || username.Equals(Constants.Session.UnknownUser))
            {
                return new UserInfoModel() { username = username };
            }

            var query = $@"
                            SELECT TOP(1)
	                            emp.[comp_code]			        AS [comp_code],
	                            emp.[plant_code]		        AS [plant_code],
	                            plant.[short_code]		        AS [short_code],	
	                            emp.[emp_code]			        AS [emp_code],
	                            emp.[dept_code]			        AS [dept_code],
                                usr.[user_id]                   AS [user_id],
	                            usr.[name]				        AS [name],
	                            usr.[username]			        AS [username],
	                            usr.[password]			        AS [password],
	                            usr.[role_id]			        AS [role_id],
	                            role_info.is_super_role         AS [is_super_role]
                            FROM sy_emp AS emp
                            INNER JOIN sy_user AS usr
                            ON emp.emp_code = usr.emp_code
                            INNER JOIN sy_plant AS plant
                            ON emp.comp_code = plant.comp_code AND emp.plant_code = plant.plant_code
                            LEFT JOIN sy_role AS role_info
                            ON usr.role_id = role_info.role_id
                            ---------------------------------------------------------------
                            WHERE usr.username = '{username}'
                        ";
            var datas = conn.Query<UserInfoModel>(query).FirstOrDefault();

            conn.Close();

            return datas;
        }

        public async Task<List<MasterData>> _getMasterData(UserInfoModel userInfo, string type, string code)
        {
            using var conn = await _db.CreateConnectionAsync();

            var query = $@"
                            SELECT 
                                 [comp_code]
                                ,[plant_code]
                                ,[master_type]
                                ,[master_code]
                                ,[master_value1]
                                ,[master_value2]
                                ,[master_value3]
                                ,[master_desc_th]
                                ,[master_desc_en]
                                ,[is_active]
                                ,[is_deleted]
                                ,[is_all]
                            FROM [dbo].[sy_master] AS sm
                            WHERE 
                                sm.master_type = '{type}'
                            AND sm.master_code = '{code}'
                            AND is_active = 1
                            AND is_deleted = 0
                        ";
            var datas = conn.Query<MasterData>(query).ToList();

            conn.Close();

            return datas;
        }

        public async Task<List<MasterData>> _getMasterDataAll()
        {
            using var conn = await _db.CreateConnectionAsync();

            var query = $@"
                            SELECT 
                                 [comp_code]
                                ,[plant_code]
                                ,[master_type]
                                ,[master_code]
                                ,[master_value1]
                                ,[master_value2]
                                ,[master_value3]
                                ,[master_desc_th]
                                ,[master_desc_en]
                                ,[is_active]
                                ,[is_deleted]
                                ,[is_all]
                            FROM [dbo].[sy_master] AS sm
                            WHERE 
                                is_active = 1
                            AND is_deleted = 0
                        ";
            var datas = conn.Query<MasterData>(query).ToList();

            conn.Close();

            return datas;
        }

        public async Task<List<MasterDataType>> _getMasterDataType()
        {
            using var conn = await _db.CreateConnectionAsync();

            var query = $@"
                            SELECT 
	                               [master_type]
                                  ,[master_type_desc]
                                  ,[is_add]
                                  ,[is_not_edit]
                                  ,[is_not_del]
                            FROM [dbo].[sy_master_type]
                        ";
            var datas = conn.Query<MasterDataType>(query).ToList();

            conn.Close();

            return datas;
        }

    }
}
