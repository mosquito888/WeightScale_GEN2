using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.Company;
using WeightScaleGen2.BGC.Models.ViewModels.Department;
using WeightScaleGen2.BGC.Models.ViewModels.Employee;
using WeightScaleGen2.BGC.Models.ViewModels.GroupMaster;
using WeightScaleGen2.BGC.Models.ViewModels.ItemMaster;
using WeightScaleGen2.BGC.Models.ViewModels.Master;
using WeightScaleGen2.BGC.Models.ViewModels.Plant;
using WeightScaleGen2.BGC.Models.ViewModels.Sender;
using WeightScaleGen2.BGC.Models.ViewModels.Supplier;
using WeightScaleGen2.BGC.Web.Common;
using WeightScaleGen2.BGC.Web.Middleware;

namespace WeightScaleGen2.BGC.Web.Services
{
    public static class SelectListService
    {
        #region SYSTEM
        public static ReturnList<ResultGetPlantViewModel> GetPlantListData(string username = Constants.Session.Common)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            Uri _baseUri = new Uri(AppSetting.BaseUrl());
            ApiKeyMiddleware _apiKey = new ApiKeyMiddleware(configuration);

            var result = new ReturnList<ResultGetPlantViewModel>();
            try
            {
                var options = new RestClientOptions(_baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("/api/v1/Plant/GetPlantListData", Method.Get);

                request.AddHeader("SecretKey", _apiKey.EncryptKey());
                request.AddHeader("User", username);
                //request.Timeout = 0;
                var response = client.ExecuteGet(request);
                if (response.ErrorException != null)
                {
                    result.isCompleted = false;
                    result.message.Add(response.ErrorException.Message);
                    return result;
                }
                ReturnList<ResultGetPlantViewModel> respObj = JsonConvert.DeserializeObject<ReturnList<ResultGetPlantViewModel>>(response.Content);
                result = respObj;
                return result;
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add(ex.Message);
                return result;
            }
        }

        public static ReturnList<ResultGetDeptViewModel> GetDepartmentListData(string username = Constants.Session.Common)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            Uri _baseUri = new Uri(AppSetting.BaseUrl());
            ApiKeyMiddleware _apiKey = new ApiKeyMiddleware(configuration);

            var result = new ReturnList<ResultGetDeptViewModel>();
            try
            {
                var options = new RestClientOptions(_baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("/api/v1/Department/GetDepartmentListData", Method.Get);

                request.AddHeader("SecretKey", _apiKey.EncryptKey());
                request.AddHeader("User", username);
                //request.Timeout = 0;
                var response = client.ExecuteGet(request);
                if (response.ErrorException != null)
                {
                    result.isCompleted = false;
                    result.message.Add(response.ErrorException.Message);
                    return result;
                }
                ReturnList<ResultGetDeptViewModel> respObj = JsonConvert.DeserializeObject<ReturnList<ResultGetDeptViewModel>>(response.Content);
                result = respObj;
                return result;
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add(ex.Message);
                return result;
            }
        }

        public static ReturnList<ResultGetEmpViewModel> GetEmpListData(string username = Constants.Session.Common)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            Uri _baseUri = new Uri(AppSetting.BaseUrl());
            ApiKeyMiddleware _apiKey = new ApiKeyMiddleware(configuration);

            var result = new ReturnList<ResultGetEmpViewModel>();
            try
            {
                var options = new RestClientOptions(_baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("/api/v1/Employee/GetEmployeeListData", Method.Get);

                request.AddHeader("SecretKey", _apiKey.EncryptKey());
                request.AddHeader("User", username);
                //request.Timeout = 0;
                var response = client.ExecuteGet(request);
                if (response.ErrorException != null)
                {
                    result.isCompleted = false;
                    result.message.Add(response.ErrorException.Message);
                    return result;
                }
                ReturnList<ResultGetEmpViewModel> respObj = JsonConvert.DeserializeObject<ReturnList<ResultGetEmpViewModel>>(response.Content);
                result = respObj;
                return result;
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add(ex.Message);
                return result;
            }
        }

        public static ReturnList<ResultGetMasterViewModel> GetMasterListData(string username = Constants.Session.Common)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            Uri _baseUri = new Uri(AppSetting.BaseUrl());
            ApiKeyMiddleware _apiKey = new ApiKeyMiddleware(configuration);

            var result = new ReturnList<ResultGetMasterViewModel>();
            try
            {
                var options = new RestClientOptions(_baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("/api/v1/Master/GetMasterListData", Method.Get);

                request.AddHeader("SecretKey", _apiKey.EncryptKey());
                request.AddHeader("User", username);
                //request.Timeout = 0;
                var response = client.ExecuteGet(request);
                if (response.ErrorException != null)
                {
                    result.isCompleted = false;
                    result.message.Add(response.ErrorException.Message);
                    return result;
                }
                ReturnList<ResultGetMasterViewModel> respObj = JsonConvert.DeserializeObject<ReturnList<ResultGetMasterViewModel>>(response.Content);
                result = respObj;
                return result;
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add(ex.Message);
                return result;
            }
        }

        public static ReturnList<ResultGetMasterTypeViewModel> GetMasterListDataType(string username = Constants.Session.Common)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            Uri _baseUri = new Uri(AppSetting.BaseUrl());
            ApiKeyMiddleware _apiKey = new ApiKeyMiddleware(configuration);

            var result = new ReturnList<ResultGetMasterTypeViewModel>();
            try
            {
                var options = new RestClientOptions(_baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("/api/v1/Master/GetMasterListDataType", Method.Get);

                request.AddHeader("SecretKey", _apiKey.EncryptKey());
                request.AddHeader("User", username);
                //request.Timeout = 0;
                var response = client.ExecuteGet(request);
                if (response.ErrorException != null)
                {
                    result.isCompleted = false;
                    result.message.Add(response.ErrorException.Message);
                    return result;
                }
                ReturnList<ResultGetMasterTypeViewModel> respObj = JsonConvert.DeserializeObject<ReturnList<ResultGetMasterTypeViewModel>>(response.Content);
                result = respObj;
                return result;
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add(ex.Message);
                return result;
            }
        }

        public static ReturnList<ResultGetCompViewModel> GetCompanyListData(string username = Constants.Session.Common)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            Uri _baseUri = new Uri(AppSetting.BaseUrl());
            ApiKeyMiddleware _apiKey = new ApiKeyMiddleware(configuration);

            var result = new ReturnList<ResultGetCompViewModel>();
            try
            {
                var options = new RestClientOptions(_baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("/api/v1/Company/GetCompanyListData", Method.Get);

                request.AddHeader("SecretKey", _apiKey.EncryptKey());
                request.AddHeader("User", username);
                //request.Timeout = 0;
                var response = client.ExecuteGet(request);
                if (response.ErrorException != null)
                {
                    result.isCompleted = false;
                    result.message.Add(response.ErrorException.Message);
                    return result;
                }
                ReturnList<ResultGetCompViewModel> respObj = JsonConvert.DeserializeObject<ReturnList<ResultGetCompViewModel>>(response.Content);
                result = respObj;
                return result;
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add(ex.Message);
                return result;
            }
        }

        public static ReturnList<ResultGetGroupMasterViewModel> GetGroupMasterListData(string username = Constants.Session.Common)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            Uri _baseUri = new Uri(AppSetting.BaseUrl());
            ApiKeyMiddleware _apiKey = new ApiKeyMiddleware(configuration);

            var result = new ReturnList<ResultGetGroupMasterViewModel>();
            try
            {
                var options = new RestClientOptions(_baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("/api/v1/GroupMaster/GetGroupMasterListData", Method.Get);

                request.AddHeader("SecretKey", _apiKey.EncryptKey());
                request.AddHeader("User", username);
                //request.Timeout = 0;
                var response = client.ExecuteGet(request);
                if (response.ErrorException != null)
                {
                    result.isCompleted = false;
                    result.message.Add(response.ErrorException.Message);
                    return result;
                }
                ReturnList<ResultGetGroupMasterViewModel> respObj = JsonConvert.DeserializeObject<ReturnList<ResultGetGroupMasterViewModel>>(response.Content);
                result = respObj;
                return result;
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add(ex.Message);
                return result;
            }
        }

        public static ReturnList<ResultSearchItemMasterViewModel> GetItemMasterListData(string username = Constants.Session.Common)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            Uri _baseUri = new Uri(AppSetting.BaseUrl());
            ApiKeyMiddleware _apiKey = new ApiKeyMiddleware(configuration);

            var result = new ReturnList<ResultSearchItemMasterViewModel>();
            try
            {
                var options = new RestClientOptions(_baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("/api/v1/ItemMaster/GetItemMasterListData", Method.Get);

                request.AddHeader("SecretKey", _apiKey.EncryptKey());
                request.AddHeader("User", username);
                //request.Timeout = 0;
                var response = client.ExecuteGet(request);
                if (response.ErrorException != null)
                {
                    result.isCompleted = false;
                    result.message.Add(response.ErrorException.Message);
                    return result;
                }
                ReturnList<ResultSearchItemMasterViewModel> respObj = JsonConvert.DeserializeObject<ReturnList<ResultSearchItemMasterViewModel>>(response.Content);
                result = respObj;
                return result;
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add(ex.Message);
                return result;
            }
        }

        public static ReturnList<ResultSearchSupplierViewModel> GetSupplierListData(string username = Constants.Session.Common)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            Uri _baseUri = new Uri(AppSetting.BaseUrl());
            ApiKeyMiddleware _apiKey = new ApiKeyMiddleware(configuration);

            var result = new ReturnList<ResultSearchSupplierViewModel>();
            try
            {
                var options = new RestClientOptions(_baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("/api/v1/Supplier/GetSupplierListData", Method.Get);

                request.AddHeader("SecretKey", _apiKey.EncryptKey());
                request.AddHeader("User", username);
                //request.Timeout = 0;
                var response = client.ExecuteGet(request);
                if (response.ErrorException != null)
                {
                    result.isCompleted = false;
                    result.message.Add(response.ErrorException.Message);
                    return result;
                }
                ReturnList<ResultSearchSupplierViewModel> respObj = JsonConvert.DeserializeObject<ReturnList<ResultSearchSupplierViewModel>>(response.Content);
                result = respObj;
                return result;
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add(ex.Message);
                return result;
            }
        }

        public static ReturnList<ResultSearchSenderViewModel> GetSenderListData(string username = Constants.Session.Common)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            Uri _baseUri = new Uri(AppSetting.BaseUrl());
            ApiKeyMiddleware _apiKey = new ApiKeyMiddleware(configuration);

            var result = new ReturnList<ResultSearchSenderViewModel>();
            try
            {
                var options = new RestClientOptions(_baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("/api/v1/Sender/GetSenderListData", Method.Get);

                request.AddHeader("SecretKey", _apiKey.EncryptKey());
                request.AddHeader("User", username);
                //request.Timeout = 0;
                var response = client.ExecuteGet(request);
                if (response.ErrorException != null)
                {
                    result.isCompleted = false;
                    result.message.Add(response.ErrorException.Message);
                    return result;
                }
                ReturnList<ResultSearchSenderViewModel> respObj = JsonConvert.DeserializeObject<ReturnList<ResultSearchSenderViewModel>>(response.Content);
                result = respObj;
                return result;
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add(ex.Message);
                return result;
            }
        }
        #endregion
    }
}
