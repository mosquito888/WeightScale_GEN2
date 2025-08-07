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

namespace WeightScaleGen2.BGC.API.APIRepository
{
    public class PrefixDocRepository : IPrefixDocRepository
    {
        IDatabaseConnectionFactory _db;
        ILogger _logger;
        IConfiguration _configuration;
        private readonly IHttpContextAccessor _context;
        private readonly ApplicationContext _applicationContext;
        private readonly string _connectionStringContext;

        public PrefixDocRepository(
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

        public async Task<string> Select_RunningCode(string docType, string compCode, string plantCode, string plantShortCode)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@DocType", docType);
            p.Add("@CompCode", compCode);
            p.Add("@PlantCode", plantCode);
            p.Add("@PlantShortCode", plantShortCode);

            var query = @"EXEC sp_get_prefix_doc
                                 @DocType = @DocType
                                ,@CompCode = @CompCode
                                ,@PlantCode = @PlantCode
                                ,@PlantShortCode = @PlantShortCode
            ";

            var data = conn.Query<string>(query, p).FirstOrDefault();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_RunningCode", null), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return data;
        }
    }
}
