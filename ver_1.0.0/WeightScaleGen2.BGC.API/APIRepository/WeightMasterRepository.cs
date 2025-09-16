using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.API.APIRepository.Interface;
using WeightScaleGen2.BGC.API.Common.Logger;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.DBModelsEF;
using WeightScaleGen2.BGC.Models.ServicesModels;

namespace WeightScaleGen2.BGC.API.APIRepository
{
    public class WeightMasterRepository : IWeightMasterRepository
    {
        IDatabaseConnectionFactory _db;
        ILogger _logger;
        IConfiguration _configuration;
        private readonly IHttpContextAccessor _context;
        private readonly ApplicationContext _applicationContext;
        private readonly string _connectionStringContext;

        public WeightMasterRepository(
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

        public async Task<MessageReport> Copy_Delete_WeightMaster(string companyCode, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@company_code", companyCode);
            p.Add("@plant_code", userInfo.plant_code);

            var query = @"EXEC sp_copy_and_delete_data_today @company_code, @plant_code";

            var datas = conn.Query(query, p).ToList();
            var result = new MessageReport(false, "ERROR!");

            conn.Close();

            result = new MessageReport(true, string.Format("Message: {0}", Constants.Result.Success));
            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Update, this.GetType().Name, "COPY_AND_DELETE_WeightMaster", null), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return result;
        }
    }
}
