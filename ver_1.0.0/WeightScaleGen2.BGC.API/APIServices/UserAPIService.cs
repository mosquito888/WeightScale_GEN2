using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.API.APIRepository;
using WeightScaleGen2.BGC.API.Common;
using WeightScaleGen2.BGC.API.Common.Logger;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.Base;
using WeightScaleGen2.BGC.Models.ViewModels.User;

namespace WeightScaleGen2.BGC.API.APIServices
{
    public class UserAPIService : BaseAPIService
    {
        IDatabaseConnectionFactory _db;
        ISecurityCommon _securityCommon;
        ILogger _logger;
        private readonly IHttpContextAccessor _context;
        private readonly UserInfoModel _userInfo;
        private UserRepository _userRepository;

        public UserAPIService(
            IDatabaseConnectionFactory db,
            ISecurityCommon securityCommon,
            ILogger logger,
            IHttpContextAccessor context,
            UserInfoModel userInfo,
            UserRepository userRepository) : base(db, securityCommon)
        {
            _db = db;
            _securityCommon = securityCommon;
            _logger = logger;
            _context = context;
            _userInfo = _getUserInfo(_context.HttpContext.Session.GetString(Constants.Session.User)).Result;
            _userRepository = userRepository;
        }

        public Task<ReturnObject<bool>> PostInfo(ResultGetUserInfo param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                if (_userRepository.Select_User_ByUsername(param.username).Result == null)
                {
                    _userRepository.Insert_User(param);
                }
                result.data = true;
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.User.Service.PostUserInfo}");
                _logger.WriteError(errorCode: ErrorCodes.User.Service.PostUserInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }

            return Task.FromResult(result);
        }

        public Task<ReturnObject<ResultSearchUserCriteria>> GetSearchUserCriteriaNotAssign()
        {
            var result = new ReturnObject<ResultSearchUserCriteria>();
            try
            {
                ResultSearchUserCriteria res = new ResultSearchUserCriteria();
                var roles = _userRepository.Select_RoleDll().Result.Where(i => i.role_id != 1).ToList();

                res.role_dll = _initRoleToRoleDll(roles);
                result.data = res;
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.User.Service.GetSearchUserCriteriaNotAssign}");
                _logger.WriteError(errorCode: ErrorCodes.User.Service.GetSearchUserCriteriaNotAssign, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }

            return Task.FromResult(result);
        }

        public Task<ReturnObject<ResultSearchUserCriteria>> GetSearchUserCriteria()
        {
            var result = new ReturnObject<ResultSearchUserCriteria>();
            try
            {
                ResultSearchUserCriteria res = new ResultSearchUserCriteria();
                var roles = _userRepository.Select_RoleDll().Result;

                res.role_dll = _initRoleToRoleDll(roles);
                result.data = res;
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.User.Service.GetSearchUserCriteria}");
                _logger.WriteError(errorCode: ErrorCodes.User.Service.GetSearchUserCriteria, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }

            return Task.FromResult(result);
        }

        public Task<ReturnList<ResultSearchUser>> SearchUser(ParamSearchUser param)
        {
            var result = new ReturnList<ResultSearchUser>();
            try
            {
                var searchData = _userRepository.Select_User(param).Result;

                result.data = _initSearchUser(searchData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.User.Service.SearchUser}");
                _logger.WriteError(errorCode: ErrorCodes.User.Service.SearchUser, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }

            return Task.FromResult(result);
        }

        public Task<ReturnList<ResultGetImage>> GetImageAll()
        {
            var result = new ReturnList<ResultGetImage>();
            var dataMap = new List<ResultGetImage>();
            try
            {
                var data = _userRepository.Select_ImageAll().Result;

                foreach (FileData index in data)
                {
                    dataMap.Add(new ResultGetImage
                    {
                        file = index.file_base6
                    });
                }

                result.data = dataMap;
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.User.Service.GetImageAll}");
                _logger.WriteError(errorCode: ErrorCodes.User.Service.GetImageAll, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }

            return Task.FromResult(result);
        }

        public Task<ReturnObject<ResultGetUserInfo>> GetUserById(string param)
        {
            var result = new ReturnObject<ResultGetUserInfo>();
            try
            {
                var userID = this._securityCommon.DecryptDataUrlEncoder(param);
                var data = _userRepository.Select_User_ById(int.Parse(userID)).Result;
                var roles = _userRepository.Select_RoleDll().Result;

                result.data = _initGetUser(data);
                result.data.role_dll = _initRoleToRoleDll(roles);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.User.Service.GetUserById}");
                _logger.WriteError(errorCode: ErrorCodes.User.Service.GetUserById, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }

            return Task.FromResult(result);
        }

        public Task<ReturnObject<ResultGetUserInfo>> GetUserByUsername(string username)
        {
            var result = new ReturnObject<ResultGetUserInfo>();
            try
            {
                if (_userRepository.Select_User_ByUsername(username).Result == null)
                {
                    _userRepository.Insert_User(new ResultGetUserInfo { name = username, username = username });
                }

                var data = _userRepository.Select_User_ByUsername(username).Result;
                var roles = _userRepository.Select_RoleDll().Result;

                result.data = _initGetUser(data);
                result.data.role_dll = _initRoleToRoleDll(roles);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.User.Service.GetUserByUsername}");
                _logger.WriteError(errorCode: ErrorCodes.User.Service.GetUserByUsername, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }

            return Task.FromResult(result);
        }

        public Task<ReturnObject<ResultGetUserInfo>> GetUserByName(string param)
        {
            var result = new ReturnObject<ResultGetUserInfo>();
            try
            {
                var data = _userRepository.Select_User_ByName(param).Result;
                var roles = _userRepository.Select_RoleDll().Result;

                result.data = _initGetUser(data);
                result.data.role_dll = _initRoleToRoleDll(roles);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.User.Service.GetUserByName}");
                _logger.WriteError(errorCode: ErrorCodes.User.Service.GetUserByName, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }

            return Task.FromResult(result);
        }

        public Task<ReturnObject<bool>> UpdateUser(ParamUpdateUser param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var res = _userRepository.Update_User(param, _userInfo).Result;

                result.data = res.is_success;
                result.isCompleted = res.is_success;
                result.message.Add(res.message);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.User.Service.UpdateUser}");
                _logger.WriteError(errorCode: ErrorCodes.User.Service.UpdateUser, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }

            return Task.FromResult(result);
        }

        public Task<ReturnObject<bool>> UploadImage(ParamUploadImage param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                _userRepository.Upload_Image(param);

                result.data = true;
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.User.Service.UploadImage}");
                _logger.WriteError(errorCode: ErrorCodes.User.Service.UploadImage, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }

            return Task.FromResult(result);
        }

        public Task<ReturnObject<ResultGetUserInfo>> GetUserByUsernamePassword(ParamLoginUser param)
        {
            var result = new ReturnObject<ResultGetUserInfo>();
            try
            {
                var data = _userRepository.Select_User_ByUsernamePassword(param).Result;
                var roles = _userRepository.Select_RoleDll().Result;

                result.data = _initGetUser(data);
                result.data.role_dll = _initRoleToRoleDll(roles);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.User.Service.GetUserByUsernamePassword}");
                _logger.WriteError(errorCode: ErrorCodes.User.Service.GetUserByUsernamePassword, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }

            return Task.FromResult(result);
        }

        private ResultGetUserInfo _initGetUser(UserData data)
        {
            ResultGetUserInfo result = new ResultGetUserInfo
            {
                email = data.username,
                name = data.name,
                role_id = this._securityCommon.EncryptDataUrlEncoder(data.role_id.ToString()),
                role_name = data.role_name,
                user_id = this._securityCommon.EncryptDataUrlEncoder(data.user_id.ToString()),
                emp_code = data.emp_code,
                serial_port = data.serial_port
            };

            return result;
        }

        private IEnumerable<BaseDLLViewModel> _initRoleToRoleDll(List<RoleData> roles)
        {
            List<BaseDLLViewModel> result = new List<BaseDLLViewModel>();

            foreach (RoleData p in roles)
            {
                result.Add(new BaseDLLViewModel
                {
                    text = p.role_name,
                    value = this._securityCommon.EncryptDataUrlEncoder(p.role_id.ToString()),
                    is_active = true
                });
            }

            return result;
        }

        private List<ResultSearchUser> _initSearchUser(List<UserData> searchData)
        {
            List<ResultSearchUser> result = new List<ResultSearchUser>();

            foreach (UserData p in searchData)
            {
                result.Add(new ResultSearchUser
                {
                    name = p.name,
                    email = p.username,
                    user_id = this._securityCommon.EncryptDataUrlEncoder(p.user_id.ToString()),
                    role_name = p.role_name,
                    total_record = p.total_record
                });
            }

            return result;
        }
    }
}
