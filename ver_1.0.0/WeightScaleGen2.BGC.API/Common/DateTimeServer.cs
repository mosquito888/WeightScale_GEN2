using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace WeightScaleGen2.BGC.API.Common
{
    public static class DateTimeServer
    {
        public static DateTime Now()
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(AppSetting.Connection());
                SqlCommand cmd = new SqlCommand
                {
                    CommandText = "SELECT GETDATE() AS DateTime",
                    CommandType = CommandType.Text,
                    Connection = sqlConnection
                };
                sqlConnection.Open();
                var result = (DateTime)cmd.ExecuteScalar();
                sqlConnection.Dispose();
                sqlConnection.Close();
                return result;
            }
            catch (Exception)
            {
                return DateTime.Now;
            }
        }
    }
}
