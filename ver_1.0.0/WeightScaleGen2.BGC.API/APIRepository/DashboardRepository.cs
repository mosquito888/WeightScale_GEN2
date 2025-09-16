using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.API.APIRepository.Interface;
using WeightScaleGen2.BGC.API.Common.Logger;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.DBModelsEF;
using WeightScaleGen2.BGC.Models.ServicesModels;

namespace WeightScaleGen2.BGC.API.APIRepository
{
    public class DashboardRepository : IDashboardRepository
    {
        IDatabaseConnectionFactory _db;
        ILogger _logger;
        IConfiguration _configuration;
        private readonly IHttpContextAccessor _context;
        private readonly ApplicationContext _applicationContext;
        private readonly string _connectionStringContext;

        public DashboardRepository(
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

        public async Task<List<DashboardSummaryData>> Select_SearchDashboardSummaryData_By(UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@comp_code", userInfo.comp_code);
            p.Add("@plant_code", userInfo.plant_code);

            var query = @"EXEC sp_select_dashboard_summary_weight @comp_code = @comp_code, @plant_code = @plant_code
                        ";

            var datas = conn.Query<DashboardSummaryData>(query, p).ToList();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_SearchDashboardListData_By", null), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<List<DashboardSummaryHistoryData>> Select_SearchDashboardSummaryHistoryWeightInData_By(UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@comp_code", userInfo.comp_code);
            p.Add("@plant_code", userInfo.plant_code);

            var query = @"EXEC sp_select_dashboard_summary_history_weight_in @comp_code = @comp_code, @plant_code = @plant_code
                        ";

            var datas = conn.Query<DashboardSummaryHistoryData>(query, p).ToList();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_SearchDashboardSummaryHistoryWeightInData_By", null), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<List<DashboardSummaryHistoryData>> Select_SearchDashboardSummaryHistoryWeightOutData_By(UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@comp_code", userInfo.comp_code);
            p.Add("@plant_code", userInfo.plant_code);

            var query = @"EXEC sp_select_dashboard_summary_history_weight_out @comp_code = @comp_code, @plant_code = @plant_code
                        ";

            var datas = conn.Query<DashboardSummaryHistoryData>(query, p).ToList();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_SearchDashboardSummaryHistoryWeightOutData_By", null), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }
    }
}
