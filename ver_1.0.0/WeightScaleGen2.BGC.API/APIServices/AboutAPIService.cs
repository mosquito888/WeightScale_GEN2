using System;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.API.Common;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.ServicesModels;

namespace WeightScaleGen2.BGC.API.APIServices
{
    public class AboutAPIService : BaseAPIService
    {
        IDatabaseConnectionFactory _db;
        ISecurityCommon _securityCommon;

        public AboutAPIService(IDatabaseConnectionFactory db, ISecurityCommon securityCommon) : base(db, securityCommon)
        {
            _db = db;
            _securityCommon = securityCommon;
        }

        public Task<ReturnObject<bool>> GetConnectionDb()
        {
            var result = new ReturnObject<bool>();
            try
            {
                var connection = _getConnectionDb().Result;

                result.data = connection;
                result.isCompleted = connection;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add(ex.Message);
            }

            return Task.FromResult(result);
        }

        private async Task<bool> _getConnectionDb()
        {
            bool connecton = false;
            using var conn = await _db.CreateConnectionAsync();

            if (conn != null)
            {
                connecton = true;
            }

            conn.Close();

            return connecton;
        }

    }
}
