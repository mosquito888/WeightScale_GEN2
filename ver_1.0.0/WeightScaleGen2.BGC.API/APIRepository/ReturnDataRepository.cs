using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NuGet.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.API.APIRepository.Interface;
using WeightScaleGen2.BGC.API.Common;
using WeightScaleGen2.BGC.API.Common.Logger;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.DBModelsEF;
using WeightScaleGen2.BGC.Models.SAPModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.ReturnData;

namespace WeightScaleGen2.BGC.API.APIRepository
{
    public class ReturnDataRepository : IReturnDataRepository
    {
        IDatabaseConnectionFactory _db;
        ILogger _logger;
        IConfiguration _configuration;
        private readonly IHttpContextAccessor _context;
        private readonly ApplicationContext _applicationContext;
        private readonly string _connectionStringContext;

        public ReturnDataRepository(
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

        public async Task<List<ReturnData>> Select_SearchReturnDataListData_By(ParamSearchReturnDataViewModel param, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            int page = (param.start / param.length) + 1;
            var _offset = (page - 1) * param.length;
            var _pagesize = param.length;

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
            p.Add("@status", param.status);

            var query = @"EXEC sp_select_return_data_by
                                 @Offset = @Offset
                                ,@PageSize = @PageSize
                                ,@start_date = @start_date
                                ,@end_date = @end_date
                                ,@status = @status
                        ";

            var datas = conn.Query<ReturnData>(query, p).ToList();

            conn.Close();

            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Inquiry, this.GetType().Name, "Select_SearchReturnDataListData_By", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return datas;
        }

        public async Task<MessageReport> Insert_ReturnData_By_Weight(string companyCode, UserInfoModel userInfo)
        {
            using var conn = await _db.CreateConnectionAsync();

            var p = new DynamicParameters();
            p.Add("@company_code", companyCode);

            var query = @"EXEC sp_insert_return_data_by_weight @company_code";

            var datas = conn.Query(query, p).ToList();
            var result = new MessageReport(false, "ERROR!");

            conn.Close();

            result = new MessageReport(true, string.Format("Message: {0}", Constants.Result.Success));
            _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Insert, this.GetType().Name, "Insert_ReturnData_By_Weight", null), user: _context.HttpContext.Session.GetString(Constants.Session.User));

            return result;
        }

        public async Task<MessageReport> Update_ReturnDataInfo_By_SAP(SapNcoResultsModel param, UserInfoModel userInfo)
        {
            using var dbContext = new ApplicationContext();
            dbContext.Database.SetConnectionString(_connectionStringContext);
            await dbContext.Database.OpenConnectionAsync();
            using var trans = dbContext.Database.BeginTransaction();

            var toDay = DateTimeServer.Now();
            var result = new MessageReport(false, "ERROR!");

            try
            {
                SyReturnDatum ret = dbContext.SyReturnData.Where(i => i.WeightOutNo == param.zwgdoc && i.Sequence == Convert.ToDecimal(param.zwgdoc_seq)).FirstOrDefault();

                ret.Message = param.message;
                ret.MessageType = param.msgty;
                ret.SendData = "Y";
                ret.MaterialDocument = param.mblnr;
                ret.DocumentYear = Convert.ToDecimal(param.mjahr);

                await dbContext.SaveChangesAsync();
                await trans.CommitAsync();
                result = new MessageReport(true, string.Format("Message: {0}", Constants.Result.Success));
                _logger.WriteActivity(message: string.Format(Constants.LogsPattern.Update, this.GetType().Name, "Update_ReturnDataInfo_By_SAP", param.ToJson()), user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                result = new MessageReport(false, string.Format("Error Code: {0} - Details: {1}", ErrorCodes.Sender.Repo.Update_SenderInfo, ex.Message));
                _logger.WriteError(errorCode: ErrorCodes.Sender.Repo.Update_SenderInfo, errorMessage: ex.Message, additionalInfo: result.message, exception: ex, user: _context.HttpContext.Session.GetString(Constants.Session.User));
            }
            finally
            {
                await trans.DisposeAsync();
                await dbContext.DisposeAsync();
            }

            return result;
        }
    }
}
