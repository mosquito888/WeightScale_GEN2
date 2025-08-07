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
using WeightScaleGen2.BGC.Models.ViewModels.WeightDaily;

namespace WeightScaleGen2.BGC.API.APIRepository
{
    public class WeightDailyRepository : IWeightDailyRepository
    {
        IDatabaseConnectionFactory _db;
        ILogger _logger;
        IConfiguration _configuration;
        private readonly IHttpContextAccessor _context;
        private readonly ApplicationContext _applicationContext;
        private readonly string _connectionStringContext;

        public WeightDailyRepository(
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

        public async Task<List<WeightDailyData>> Select_SearchWeightDailyListData_By(ParamSearchWeightDailyViewModel param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            if (param.start_date.HasValue)
            {
                p.Add("@start_date", param.start_date.Value.ToString("yyyy-MM-dd"));
            }
            else
            {
                p.Add("@start_date", null);
            }

            if (param.end_date.HasValue)
            {
                p.Add("@end_date", param.end_date.Value.ToString("yyyy-MM-dd"));
            }
            else
            {
                p.Add("@end_date", null);
            }
            p.Add("@close_work", param.close_work);

            var query = @"EXEC sp_select_daily_report_by
                                 @start_date = @start_date
                                ,@end_date = @end_date
                                ,@close_work = @close_work
                        ";

            var datas = conn.Query<WeightDailyData>(query, p).ToList();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_SearchWeightDailyListData_By", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }
    }
}
