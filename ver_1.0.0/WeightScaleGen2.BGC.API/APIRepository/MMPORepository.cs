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
using WeightScaleGen2.BGC.Models.ViewModels.MMPO;

namespace WeightScaleGen2.BGC.API.APIRepository
{
    public class MMPORepository : IMMPORepository
    {
        IDatabaseConnectionFactoryPO _db;
        ILogger _logger;
        IConfiguration _configuration;
        private readonly IHttpContextAccessor _context;
        private readonly ApplicationContext _applicationContext;
        private readonly string _connectionStringContext;

        public MMPORepository(
            IDatabaseConnectionFactoryPO db,
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
            _applicationContext.Database.SetConnectionString(_configuration.GetConnectionString("DBConnectionPO"));
            _connectionStringContext = _configuration.GetConnectionString("DBConnectionPO");
        }

        public async Task<List<MMPOData>> Select_SearchMMPOListData_By(ParamSearchMMPOViewModel param, UserInfoModel userInfo)
        {
            int page = (param.start / param.length) + 1;
            var _offset = (page - 1) * param.length;
            var _pagesize = param.length;

            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@Offset", _offset);
            p.Add("@PageSize", _pagesize);
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
            p.Add("@company_code", userInfo.comp_code);
            p.Add("@plant_code", userInfo.plant_code);

            var query = @"SELECT COUNT(*) OVER () AS total_record, * 
                                 FROM MM_PO
                                 WHERE (@start_date IS NULL OR CONVERT(date, CreatedOn) >= @start_date)  
                                 AND (@end_date IS NULL OR CONVERT(date, CreatedOn) <= @end_date)  
                                 AND CompanyCode = @company_code
                                 AND Plant = @plant_code
                                 ORDER BY PurchaseNumber
                                 OFFSET @Offset ROWS
                                 FETCH NEXT @PageSize ROWS ONLY;
                        ";

            var datas = conn.Query<MMPOData>(query, p).ToList();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_SearchMMPOListData_By", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<List<MMPOData>> Select_SearchMMPOListData_By_CompanyCode(string companyCode, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@company_code", companyCode);

            var query = @"SELECT * 
                                 FROM MM_PO
                                 WHERE CreatedOn >= DATEADD(MONTH, -5, GETDATE()) 
                                 AND CompanyCode = @company_code;
                        ";

            var datas = conn.Query<MMPOData>(query, p).ToList();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_SearchMMPOListData_By_CompanyCode", null), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<MMPOData> Select_SearchMMPOQtyPendingData(ParamSearchMMPOQtyPendingViewModel param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@document_po", param.document_po);
            p.Add("@material_code", param.material_code);
            p.Add("@line_number", param.line_number);
            p.Add("@company_code", userInfo.comp_code);

            var query = @"SELECT * 
                                 FROM MM_PO
                                 WHERE PurchaseNumber = @document_po
                                 AND NumOfRec = @line_number
                                 AND MaterialCode = @material_code
                                 AND STATUS != 'L' AND DlvComplete !='X'
                                 AND CompanyCode = @company_code;
                        ";

            var datas = conn.Query<MMPOData>(query, p).FirstOrDefault();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_SearchMMPOQtyPendingData", null), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }
    }
}
