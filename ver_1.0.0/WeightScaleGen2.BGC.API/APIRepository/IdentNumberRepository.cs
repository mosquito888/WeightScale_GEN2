using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NuGet.Protocol;
using System.Linq;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.API.APIRepository.Interface;
using WeightScaleGen2.BGC.API.Common.Logger;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.DBModelsEF;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.IdentNumber;

namespace WeightScaleGen2.BGC.API.APIRepository
{
    public class IdentNumberRepository : IIdentNumberRepository
    {
        IDatabaseConnectionFactory _db;
        ILogger _logger;
        IConfiguration _configuration;
        private readonly IHttpContextAccessor _context;
        private readonly ApplicationContext _applicationContext;
        private readonly string _connectionStringContext;

        public IdentNumberRepository(
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

        public async Task<string> Select_IdentNumber(ParamGetIdentNumberViewModel param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@type", param.type);
            p.Add("@company", param.company);
            p.Add("@plant_code", userInfo.plant_code);

            var query = @"EXEC sp_get_weight_in_number @type, @company, @plant_code";

            var data = conn.Query<string>(query, p).FirstOrDefault();

            conn.Close();

            _logger.WriteActivity(
                message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_IdentNumber", param.ToJson()),
                user: _context.HttpContext.Session.GetString(Constants.Session.User)
            );

            return data;
        }
    }
}
