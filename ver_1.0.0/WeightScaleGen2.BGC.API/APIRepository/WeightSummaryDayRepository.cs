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
using WeightScaleGen2.BGC.Models.ViewModels.WeightSummaryDay;

namespace WeightScaleGen2.BGC.API.APIRepository
{
    public class WeightSummaryDayRepository : IWeightSummaryDayRepository
    {
        IDatabaseConnectionFactory _db;
        ILogger _logger;
        IConfiguration _configuration;
        private readonly IHttpContextAccessor _context;
        private readonly ApplicationContext _applicationContext;
        private readonly string _connectionStringContext;

        public WeightSummaryDayRepository(
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

        public async Task<List<WeightSummaryDayData>> Select_SearchWeightSummaryDayListData_By(ParamSearchWeightSummaryDayViewModel param, UserInfoModel userInfo)
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

            if (param.item_group == 0)
            {
                p.Add("@item_group", null);
            }
            else 
            {
                p.Add("@item_group", param.item_group);
            }
            p.Add("@comp_code", userInfo.comp_code);
            p.Add("@plant_code", userInfo.plant_code);

            var query = @"EXEC sp_select_summary_day_by
                                 @start_date = @start_date
                                ,@end_date = @end_date
                                ,@item_group = @item_group
                                ,@comp_code = @comp_code
                                ,@plant_code = @plant_code
                        ";

            var datas = conn.Query<WeightSummaryDayData>(query, p).ToList();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_SearchWeightSummaryDayListData_By", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }
    }
}
