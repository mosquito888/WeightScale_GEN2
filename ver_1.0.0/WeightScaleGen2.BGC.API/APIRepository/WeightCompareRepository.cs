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
using WeightScaleGen2.BGC.Models.ViewModels.WeightCompare;

namespace WeightScaleGen2.BGC.API.APIRepository
{
    public class WeightCompareRepository : IWeightCompareRepository
    {
        IDatabaseConnectionFactory _db;
        ILogger _logger;
        IConfiguration _configuration;
        private readonly IHttpContextAccessor _context;
        private readonly ApplicationContext _applicationContext;
        private readonly string _connectionStringContext;

        public WeightCompareRepository(
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

        public async Task<List<WeightCompareData>> Select_SearchWeightCompareListData_By(ParamSearchWeightCompareViewModel param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@start_date", param.start_date.Value.ToString("yyyy-MM-dd"));
            p.Add("@end_date", param.end_date.Value.ToString("yyyy-MM-dd"));
            p.Add("@item_code", param.item_code);
            if (param.supplier_code != 0)
            {
                p.Add("@supplier_code", param.supplier_code);
            }
            else
            {
                p.Add("@supplier_code", null);
            }

            var query = @"EXEC sp_select_weight_compare_by
                                 @start_date = @start_date
                                ,@end_date = @end_date
                                ,@item_code = @item_code
                                ,@supplier_code = @supplier_code
                        ";

            var datas = conn.Query<WeightCompareData>(query, p).ToList();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_SearchWeightCompareListData_By", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }
    }
}
