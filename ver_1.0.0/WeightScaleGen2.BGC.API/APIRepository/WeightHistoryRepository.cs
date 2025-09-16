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
using WeightScaleGen2.BGC.Models.ViewModels.WeightHistory;

namespace WeightScaleGen2.BGC.API.APIRepository
{
    public class WeightHistoryRepository : IWeightHistoryRepository
    {
        IDatabaseConnectionFactory _db;
        ILogger _logger;
        IConfiguration _configuration;
        private readonly IHttpContextAccessor _context;
        private readonly ApplicationContext _applicationContext;
        private readonly string _connectionStringContext;

        public WeightHistoryRepository(
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

        public async Task<WeightHistoryData> Select_WeightHistoryInfo(ParamWeightHistoryInfo param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@id", param.id);
            p.Add("@comp_code", userInfo.comp_code);
            p.Add("@plant_code", userInfo.plant_code);

            var query = @"EXEC sp_select_weight_history_by_id
                                @id = @id
                               ,@comp_code = @comp_code
                               ,@plant_code = @plant_code
            ";

            var datas = conn.Query<WeightHistoryData>(query, p).FirstOrDefault();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_WeightHistoryInfo", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        //public async Task<List<ItemMasterData>> Select_ItemMasterListData_All(UserInfoModel userInfo)
        //{
        //    using var conn = await _db.CreateConnectionAsync();

        //    var query = @"EXEC sp_select_sy_item_master_all";

        //    var datas = conn.Query<ItemMasterData>(query).ToList();

        //    conn.Close();

        //    _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_ItemMasterListData_All", null), user: _context.HttpContext.Session.GetString(Constants.Session.User));

        //    return datas;
        //}

        public async Task<List<WeightHistoryData>> Select_SearchWeightHistoryListData_By(ParamSearchWeightHistoryViewModel param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            int page = (param.start / param.length) + 1;
            var _offset = (page - 1) * param.length;
            var _pagesize = param.length;

            var p = new DynamicParameters();
            p.Add("@Offset", _offset);
            p.Add("@PageSize", _pagesize);
            if (param.date != null)
            {
                p.Add("@date", param.date.Value.ToString("yyyy-MM-dd"));
            }
            else
            {
                p.Add("@date", null);
            }
            p.Add("@po_number", param.po_number);
            p.Add("@comp_code", userInfo.comp_code);
            p.Add("@plant_code", userInfo.plant_code);

            var query = @"EXEC sp_select_weight_history_by 
                                 @Offset = @Offset
                                ,@PageSize = @PageSize
                                ,@date = @date
                                ,@po_number = @po_number
                                ,@comp_code = @comp_code
                                ,@plant_code = @plant_code
                        ";

            var datas = conn.Query<WeightHistoryData>(query, p).ToList();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_SearchWeightHistoryListData_By", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<decimal> Select_SumQtyWeightHistory_By(ParamGetSumQtyWeightHistoryViewModel param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@document_po", param.document_po);
            p.Add("@item_code", param.item_code);
            p.Add("@line_number", param.line_number);
            p.Add("@date", param.date.ToString("yyyy-MM-dd"));
            p.Add("@comp_code", userInfo.comp_code);
            p.Add("@plant_code", userInfo.plant_code);

            var query = @"EXEC sp_get_calculate_weight_history_by @document_po, @item_code, @line_number, @date, @comp_code, @plant_code";

            var data = conn.Query<decimal>(query, p).FirstOrDefault();

            conn.Close();

            _logger.WriteActivity(
                message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_SumQtyWeightHistory_By", param.ToJson()),
                user: _context.HttpContext.Session.GetString(Constants.Session.User)
            );

            return data;
        }
    }
}
