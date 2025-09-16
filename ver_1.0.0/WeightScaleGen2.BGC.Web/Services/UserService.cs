using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.Menu;
using WeightScaleGen2.BGC.Models.ViewModels.Role;
using WeightScaleGen2.BGC.Models.ViewModels.User;
using WeightScaleGen2.BGC.Web.Middleware;

namespace WeightScaleGen2.BGC.Web.Services
{
    public class UserService
    {
        private IConfiguration _config;
        private Uri _baseUri;
        private ApiKeyMiddleware _apiKey;

        public UserService(IConfiguration config)
        {
            _config = config;
            _baseUri = new Uri(config.GetSection("Api").GetSection("BaseUrl").Value);
            _apiKey = new ApiKeyMiddleware(this._config);
        }
        public ReturnObject<bool> Create(ResultGetUserInfo param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var options = new RestClientOptions(this._baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("/api/v1/User/PostItem", Method.Post);

                request.AddHeader("SecretKey", this._apiKey.EncryptKey());
                request.AddParameter("application/json", JsonConvert.SerializeObject(param), ParameterType.RequestBody);
                //request.Timeout = 0;
                var response = client.ExecutePost(request);
                if (response.ErrorException != null)
                {
                    result.isCompleted = false;
                    result.message.Add(response.ErrorException.Message);
                    return result;
                }
                ReturnObject<bool> respObj = JsonConvert.DeserializeObject<ReturnObject<bool>>(response.Content);
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

        public ReturnList<ResultGetMenuViewModel> GetMenu(string name, string username)
        {
            var result = new ReturnList<ResultGetMenuViewModel>();
            try
            {
                PramGetMenuViewModel pReq = new PramGetMenuViewModel
                {
                    name = name,
                    username = username
                };

                var options = new RestClientOptions(this._baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("/api/v1/Menu/AuthenticateMenuUser", Method.Get);

                request.AddHeader("SecretKey", this._apiKey.EncryptKey());
                request.AddHeader("User", username);
                request.AddParameter("application/json", JsonConvert.SerializeObject(pReq), ParameterType.RequestBody);
                //request.Timeout = 0;
                var response = client.ExecuteGet(request);
                if (response.ErrorException != null)
                {
                    result.isCompleted = false;
                    result.message.Add(response.ErrorException.Message);
                    return result;
                }
                ReturnList<ResultGetMenuViewModel> respObj = JsonConvert.DeserializeObject<ReturnList<ResultGetMenuViewModel>>(response.Content);
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

        public ReturnList<ResultSearchUser> SearchUser(ParamSearchUser param, string username)
        {
            var result = new ReturnList<ResultSearchUser>();
            try
            {
                var options = new RestClientOptions(this._baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("/api/v1/User/SearchUser", Method.Get);

                request.AddHeader("SecretKey", this._apiKey.EncryptKey());
                request.AddHeader("User", username);
                request.AddParameter("application/json", JsonConvert.SerializeObject(param), ParameterType.RequestBody);
                //request.Timeout = 0;
                var response = client.ExecuteGet(request);
                if (response.ErrorException != null)
                {
                    result.isCompleted = false;
                    result.message.Add(response.ErrorException.Message);
                    return result;
                }
                ReturnList<ResultSearchUser> respObj = JsonConvert.DeserializeObject<ReturnList<ResultSearchUser>>(response.Content);
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

        public ReturnObject<bool> UpdateUser(ParamUpdateUser param, string username)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var options = new RestClientOptions(this._baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("/api/v1/User/UpdateUser", Method.Put);

                request.AddHeader("SecretKey", this._apiKey.EncryptKey());
                request.AddHeader("User", username);
                request.AddParameter("application/json", JsonConvert.SerializeObject(param), ParameterType.RequestBody);
                //request.Timeout = 0;
                var response = client.ExecutePut(request);
                if (response.ErrorException != null)
                {
                    result.isCompleted = false;
                    result.message.Add(response.ErrorException.Message);
                    return result;
                }
                ReturnObject<bool> respObj = JsonConvert.DeserializeObject<ReturnObject<bool>>(response.Content);
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

        public ReturnObject<bool> UpdateRoleItem(ParamUpdateRoleItemViewModel param, string username)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var options = new RestClientOptions(this._baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("/api/v1/Menu/UpdateMenuRole", Method.Put);

                request.AddHeader("SecretKey", this._apiKey.EncryptKey());
                request.AddHeader("User", username);
                request.AddParameter("application/json", JsonConvert.SerializeObject(param), ParameterType.RequestBody);
                //request.Timeout = 0;
                var response = client.ExecutePut(request);
                if (response.ErrorException != null)
                {
                    result.isCompleted = false;
                    result.message.Add(response.ErrorException.Message);
                    return result;
                }
                ReturnObject<bool> respObj = JsonConvert.DeserializeObject<ReturnObject<bool>>(response.Content);
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

        public ReturnObject<bool> UpdateRoleSelectItem(ResultGetMenuSectionViewModel param, string username)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var options = new RestClientOptions(this._baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("/api/v1/Menu/UpdateMenuRoleSelect", Method.Put);

                request.AddHeader("SecretKey", this._apiKey.EncryptKey());
                request.AddHeader("User", username);
                request.AddParameter("application/json", JsonConvert.SerializeObject(param), ParameterType.RequestBody);
                //request.Timeout = 0;
                var response = client.ExecutePut(request);
                if (response.ErrorException != null)
                {
                    result.isCompleted = false;
                    result.message.Add(response.ErrorException.Message);
                    return result;
                }
                ReturnObject<bool> respObj = JsonConvert.DeserializeObject<ReturnObject<bool>>(response.Content);
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

        public ReturnObject<bool> CreatedRole(string username, ResultRoleInfo param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var options = new RestClientOptions(this._baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("/api/v1/Menu/PostNewRole", Method.Post);

                request.AddHeader("SecretKey", this._apiKey.EncryptKey());
                request.AddHeader("User", username);
                request.AddParameter("application/json", JsonConvert.SerializeObject(param), ParameterType.RequestBody);
                //request.Timeout = 0;

                var response = client.ExecutePost(request);

                if (response.ErrorException != null)
                {
                    result.isCompleted = false;
                    result.message.Add(response.ErrorException.Message);
                    return result;
                }
                ReturnObject<bool> respObj = JsonConvert.DeserializeObject<ReturnObject<bool>>(response.Content);
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

        public ReturnObject<bool> UpdateRole(string username, ResultRoleInfo param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var options = new RestClientOptions(this._baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("/api/v1/Menu/PutRole", Method.Put);

                request.AddHeader("SecretKey", this._apiKey.EncryptKey());
                request.AddHeader("User", username);
                request.AddParameter("application/json", JsonConvert.SerializeObject(param), ParameterType.RequestBody);
                //request.Timeout = 0;
                var response = client.ExecutePut(request);
                if (response.ErrorException != null)
                {
                    result.isCompleted = false;
                    result.message.Add(response.ErrorException.Message);
                    return result;
                }
                ReturnObject<bool> respObj = JsonConvert.DeserializeObject<ReturnObject<bool>>(response.Content);
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

        public ReturnObject<ResultRoleInfo> GetRole(string username, ResultRoleInfo param)
        {
            var result = new ReturnObject<ResultRoleInfo>();
            try
            {
                var options = new RestClientOptions(this._baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("/api/v1/Menu/GetRoleInfo", Method.Get);

                request.AddHeader("SecretKey", this._apiKey.EncryptKey());
                request.AddHeader("User", username);
                request.AddParameter("application/json", JsonConvert.SerializeObject(param), ParameterType.RequestBody);
                //request.Timeout = 0;
                var response = client.ExecuteGet(request);
                if (response.ErrorException != null)
                {
                    result.isCompleted = false;
                    result.message.Add(response.ErrorException.Message);
                    return result;
                }
                ReturnObject<ResultRoleInfo> respObj = JsonConvert.DeserializeObject<ReturnObject<ResultRoleInfo>>(response.Content);
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

        public ReturnObject<bool> DeleteRole(string username, ResultRoleInfo param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var options = new RestClientOptions(this._baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("/api/v1/Menu/DeleteRole", Method.Delete);

                request.AddHeader("SecretKey", this._apiKey.EncryptKey());
                request.AddHeader("User", username);
                request.AddParameter("application/json", JsonConvert.SerializeObject(param), ParameterType.RequestBody);
                //request.Timeout = 0;
                var response = client.Delete(request);
                if (response.ErrorException != null)
                {
                    result.isCompleted = false;
                    result.message.Add(response.ErrorException.Message);
                    return result;
                }
                ReturnObject<bool> respObj = JsonConvert.DeserializeObject<ReturnObject<bool>>(response.Content);
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

        public ReturnObject<ResultSearchUserCriteria> GetSearchUserCriteria(string username)
        {
            var result = new ReturnObject<ResultSearchUserCriteria>();
            try
            {
                var options = new RestClientOptions(this._baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("/api/v1/User/GetSearchUserCriteria", Method.Get);

                request.AddHeader("SecretKey", this._apiKey.EncryptKey());
                request.AddHeader("User", username);
                //request.Timeout = 0;
                var response = client.ExecuteGet(request);
                if (response.ErrorException != null)
                {
                    result.isCompleted = false;
                    result.message.Add(response.ErrorException.Message);
                    return result;
                }
                ReturnObject<ResultSearchUserCriteria> respObj = JsonConvert.DeserializeObject<ReturnObject<ResultSearchUserCriteria>>(response.Content);
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

        public ReturnObject<ResultSearchUserCriteria> GetSearchUserCriteriaNotAssign(string username)
        {
            var result = new ReturnObject<ResultSearchUserCriteria>();
            try
            {
                var options = new RestClientOptions(this._baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("/api/v1/User/GetSearchUserCriteriaNotAssign", Method.Get);

                request.AddHeader("SecretKey", this._apiKey.EncryptKey());
                request.AddHeader("User", username);
                //request.Timeout = 0;
                var response = client.ExecuteGet(request);
                if (response.ErrorException != null)
                {
                    result.isCompleted = false;
                    result.message.Add(response.ErrorException.Message);
                    return result;
                }
                ReturnObject<ResultSearchUserCriteria> respObj = JsonConvert.DeserializeObject<ReturnObject<ResultSearchUserCriteria>>(response.Content);
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

        public ReturnObject<ResultGetUserInfo> GetUserByUsername(string username)
        {
            var result = new ReturnObject<ResultGetUserInfo>();
            try
            {
                var options = new RestClientOptions(this._baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("/api/v1/User/GetUserByUsername?username=" + username, Method.Get);

                request.AddHeader("SecretKey", this._apiKey.EncryptKey());
                request.AddHeader("User", username);
                //request.Timeout = 0;
                var response = client.ExecuteGet(request);
                if (response.ErrorException != null)
                {
                    result.isCompleted = false;
                    result.message.Add(response.ErrorException.Message);
                    return result;
                }
                ReturnObject<ResultGetUserInfo> respObj = JsonConvert.DeserializeObject<ReturnObject<ResultGetUserInfo>>(response.Content);
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

        public ReturnObject<ResultGetUserInfo> GetUser(string id, string username)
        {
            var result = new ReturnObject<ResultGetUserInfo>();
            try
            {
                var options = new RestClientOptions(this._baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("/api/v1/User/GetUser?id=" + id, Method.Get);

                request.AddHeader("SecretKey", this._apiKey.EncryptKey());
                request.AddHeader("User", username);
                //request.Timeout = 0;
                var response = client.ExecuteGet(request);
                if (response.ErrorException != null)
                {
                    result.isCompleted = false;
                    result.message.Add(response.ErrorException.Message);
                    return result;
                }
                ReturnObject<ResultGetUserInfo> respObj = JsonConvert.DeserializeObject<ReturnObject<ResultGetUserInfo>>(response.Content);
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

        public ReturnList<ResultGetMenuViewModel> GetMenuRole(string id, string username)
        {
            var result = new ReturnList<ResultGetMenuViewModel>();
            try
            {
                var options = new RestClientOptions(this._baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("/api/v1/Menu/GetMenuRole?id=" + id, Method.Get);

                request.AddHeader("SecretKey", this._apiKey.EncryptKey());
                request.AddHeader("User", username);
                //request.Timeout = 0;
                var response = client.ExecuteGet(request);
                if (response.ErrorException != null)
                {
                    result.isCompleted = false;
                    result.message.Add(response.ErrorException.Message);
                    return result;
                }
                ReturnList<ResultGetMenuViewModel> respObj = JsonConvert.DeserializeObject<ReturnList<ResultGetMenuViewModel>>(response.Content);
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

        public ReturnObject<bool> UpLoadImage(ParamUploadImage param, string username)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var options = new RestClientOptions(this._baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("/api/v1/User/UploadImage", Method.Post);

                request.AddHeader("SecretKey", this._apiKey.EncryptKey());
                request.AddHeader("User", username);
                request.AddParameter("application/json", JsonConvert.SerializeObject(param), ParameterType.RequestBody);
                //request.Timeout = 0;
                var response = client.ExecutePost(request);
                if (response.ErrorException != null)
                {
                    result.isCompleted = false;
                    result.message.Add(response.ErrorException.Message);
                    return result;
                }
                ReturnObject<bool> respObj = JsonConvert.DeserializeObject<ReturnObject<bool>>(response.Content);
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

        public ReturnList<ResultGetImage> GetImageAll(string username)
        {
            var result = new ReturnList<ResultGetImage>();
            try
            {
                var options = new RestClientOptions(this._baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("/api/v1/User/GetAllImage", Method.Get);

                request.AddHeader("SecretKey", this._apiKey.EncryptKey());
                request.AddHeader("User", username);
                //request.Timeout = 0;
                var response = client.ExecuteGet(request);

                if (response.ErrorException != null)
                {
                    result.isCompleted = false;
                    result.message.Add(response.ErrorException.Message);
                    return result;
                }
                ReturnList<ResultGetImage> respObj = JsonConvert.DeserializeObject<ReturnList<ResultGetImage>>(response.Content);
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

        public ReturnObject<ResultGetUserInfo> GetUserByUsernamePassword(ParamLoginUser param, string username)
        {
            var result = new ReturnObject<ResultGetUserInfo>();
            try
            {
                var options = new RestClientOptions(this._baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("/api/v1/User/GetUserByUsernamePassword", Method.Get);

                request.AddHeader("SecretKey", this._apiKey.EncryptKey());
                request.AddHeader("User", username);
                request.AddParameter("application/json", JsonConvert.SerializeObject(param), ParameterType.RequestBody);
                //request.Timeout = 0;
                var response = client.ExecuteGet(request);
                if (response.ErrorException != null)
                {
                    result.isCompleted = false;
                    result.message.Add(response.ErrorException.Message);
                    return result;
                }
                ReturnObject<ResultGetUserInfo> respObj = JsonConvert.DeserializeObject<ReturnObject<ResultGetUserInfo>>(response.Content);
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

    }
}
