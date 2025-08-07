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
using WeightScaleGen2.BGC.Models.ViewModels.Log;

namespace WeightScaleGen2.BGC.API.APIRepository
{
    public class LogRepository : ILogRepository
    {
        IDatabaseConnectionFactory _db;
        ISecurityCommon _securityCommon;
        ILogger _logger;
        IConfiguration _configuration;
        private readonly IHttpContextAccessor _context;
        private readonly ApplicationContext _applicationContext;
        private readonly string _connectionStringContext;

        public LogRepository(
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

        public async Task<List<LogData>> Select_ListLogDataDll_By(ParamSearchLogViewModel param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();
            int page = (param.start / param.length) + 1;
            string level = null;
            DateTime? logDtFrom = null;
            DateTime? logDtTo = null;
            if (!String.IsNullOrEmpty(param.level))
            {
                level = this._securityCommon.DecryptDataUrlEncoder(param.level);
            }
            if (param.logDataFrom != null)
            {
                logDtFrom = param.logDataFrom.Value.Date;
            }
            if (param.logDataTo != null)
            {
                logDtTo = param.logDataTo.Value.Date;
            }

            var p = new DynamicParameters();
            p.Add("@Offset", (page - 1) * param.length);
            p.Add("@PageSize", param.length);
            p.Add("@Level", level);
            p.Add("@Username", param.username);
            p.Add("@DtFrom", logDtFrom);
            p.Add("@DtTo", logDtTo);

            var query = @"
                            SELECT
                              COUNT(*) OVER () AS total_record
                             ,sl.log_id
                             ,sl.log_level
                             ,sl.log_type
                             ,sl.log_error_code
                             ,sl.log_date
                             ,sl.log_message
                             ,sl.log_inner_exception
                             ,sl.log_exception_message
                             ,sl.log_additional_Info
                             ,sl.log_caller_member_name
                             ,sl.log_stack_trace
                             ,sl.log_caller_file_path
                             ,sl.log_source_line_number
                             ,sl.log_user
                             ,sl.log_ip_address
                            FROM sy_log sl
                            WHERE sl.log_level = @Level
                            AND 
                            (@Username IS NULL
                                OR (sl.log_user LIKE '%' + @Username + '%')
                            )
                            AND 
                            (CONVERT(DATE, sl.log_date) >= CONVERT(DATE, @DtFrom) AND CONVERT(DATE, sl.log_date) <= CONVERT(DATE, @DtTo)
                                  OR (@DtFrom IS NULL)
                                  OR (@DtTo IS NULL)
                            )
                            ORDER BY sl.log_date
                            OFFSET @Offset ROWS
                            FETCH NEXT @PageSize ROWS ONLY;
                        ";
            var datas = conn.Query<LogData>(query, p).ToList();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_ListLogDataDll_By", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<List<LogLevelData>> Select_ListLogLevelDll_All(UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();
            var query = @"
                            SELECT
                              sm.master_code            AS level_code
                             ,sm.master_value1          AS level_value
                             ,sm.master_desc_th         AS level_desc_th
                             ,sm.master_desc_en         AS level_desc_en
                            FROM sy_master sm
                            WHERE sm.master_type = 'loglevel'
                            AND sm.is_active = 1
                            AND sm.is_all = 1
                        ";
            var datas = conn.Query<LogLevelData>(query).ToList();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_ListLogLevelDll_All", null), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }
    }
}
