using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NuGet.Protocol;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.API.APIRepository.Interface;
using WeightScaleGen2.BGC.API.Common.Logger;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.DBModelsEF;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.WeightInHistory;

namespace WeightScaleGen2.BGC.API.APIRepository
{
    public class WeightInHistoryRepository : IWeightInHistoryRepository
    {
        IDatabaseConnectionFactory _db;
        ILogger _logger;
        IConfiguration _configuration;
        private readonly IHttpContextAccessor _context;
        private readonly ApplicationContext _applicationContext;
        private readonly string _connectionStringContext;

        public WeightInHistoryRepository(
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

        public async Task<List<WeightInHistoryData>> Select_SearchWeightInHistoryListData(UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@comp_code", userInfo.comp_code);
            p.Add("@plant_code", userInfo.plant_code);

            var query = @"EXEC sp_select_history_weight_in_all @comp_code = @comp_code, @plant_code = @plant_code";

            var datas = conn.Query<WeightInHistoryData>(query, p).ToList();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_SearchWeightInHistoryListData", null), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<List<WeightInHistoryData>> Select_SearchWeightInHistoryListData_By(ParamSearchWeightInHistoryViewModel param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@car_license", param.car_license);
            p.Add("@comp_code", userInfo.comp_code);
            p.Add("@plant_code", userInfo.plant_code);

            var query = @"EXEC sp_select_history_weight_in_by_license
                                 @car_license = @car_license,
                                 @comp_code = @comp_code,
                                 @plant_code = @plant_code
                        ";

            var datas = conn.Query<WeightInHistoryData>(query, p).ToList();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_SearchWeightInHistoryListData_By", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }
    }
}
