using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.API.APIRepository;
using WeightScaleGen2.BGC.API.Common;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.Menu;
using WeightScaleGen2.BGC.Models.ViewModels.Role;
using ILogger = WeightScaleGen2.BGC.API.Common.Logger.ILogger;

namespace WeightScaleGen2.BGC.API.APIServices
{
    public class MenuAPIService : BaseAPIService
    {
        IDatabaseConnectionFactory _db;
        ISecurityCommon _securityCommon;
        ILogger _logger;
        private readonly IHttpContextAccessor _context;
        private readonly UserInfoModel _userInfo;
        private MenuRepository _menuRepository;

        public MenuAPIService(
            IDatabaseConnectionFactory db,
            ISecurityCommon securityCommon,
            ILogger logger,
            IHttpContextAccessor context,
            UserInfoModel userInfo,
            MenuRepository menuRepository) : base(db, securityCommon)
        {
            _db = db;
            _securityCommon = securityCommon;
            _logger = logger;
            _context = context;
            _userInfo = _getUserInfo(_context.HttpContext.Session.GetString(Constants.Session.User)).Result;
            _menuRepository = menuRepository;
        }

        public Task<ReturnList<ResultGetMenuViewModel>> AuthenticateMenuUser(PramGetMenuViewModel param)
        {
            var result = new ReturnList<ResultGetMenuViewModel>();
            try
            {
                if (_menuRepository.Select_User_ByUsername(param, _userInfo).Result != null)
                {
                    var menuData = _menuRepository.Select_MenuUser(param, _userInfo).Result;
                    var sectionData = _menuRepository.Select_MenuSectionUser(param, _userInfo).Result;

                    result.isCompleted = true;
                    result.message.Add(Constants.Result.Success);
                    result.data = _initMenuDataToModel(menuData, sectionData);
                }
                else
                {
                    _menuRepository.Insert_User(param, _userInfo);
                    var menuData = _menuRepository.Select_MenuUser(param, _userInfo).Result;
                    var sectionData = _menuRepository.Select_MenuSectionUser(param, _userInfo).Result;

                    result.isCompleted = true;
                    result.message.Add(Constants.Result.Success);
                    result.data = _initMenuDataToModel(menuData, sectionData);
                }
            }
            catch (Exception ex)
            {
                result.message.Add($"Error Code: {ErrorCodes.Menu.Service.AuthenticateMenuUser}");
                _logger.WriteError(errorCode: ErrorCodes.Menu.Service.AuthenticateMenuUser, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }

            return Task.FromResult(result);
        }

        public Task<ReturnObject<ResultRoleInfo>> GetRoleInfo(ResultRoleInfo param)
        {
            var result = new ReturnObject<ResultRoleInfo>();
            try
            {
                var objData = _menuRepository.Select_Role(param, _userInfo).Result;

                result.data = _initRoleToModel(objData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Menu.Service.GetRoleInfo}");
                _logger.WriteError(errorCode: ErrorCodes.Menu.Service.GetRoleInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }

            return Task.FromResult(result);
        }

        public Task<ReturnObject<bool>> CreatedRole(ResultRoleInfo param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var roleId = _menuRepository.Insert_Role(param, _userInfo).Result;
                _menuRepository.Insert_MenuSection(roleId, _userInfo);

                result.data = true;
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.data = false;
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Menu.Service.CreatedRole}");
                _logger.WriteError(errorCode: ErrorCodes.Menu.Service.CreatedRole, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }

            return Task.FromResult(result);
        }

        public Task<ReturnObject<bool>> UpdatedRole(ResultRoleInfo param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                _menuRepository.Update_Role(param, _userInfo);

                result.data = true;
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {

                result.data = false;
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Menu.Service.UpdatedRole}");
                _logger.WriteError(errorCode: ErrorCodes.Menu.Service.UpdatedRole, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }

            return Task.FromResult(result);
        }

        public Task<ReturnObject<bool>> DeletedRole(ResultRoleInfo param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var usingRole = _menuRepository.Select_RoleUsing(param, _userInfo).Result;
                if (usingRole > 0)
                {
                    result.data = false;
                    result.isCompleted = true;
                    result.message.Add("invalid");
                }
                else
                {
                    _menuRepository.Delete_Role(param, _userInfo);

                    result.data = true;
                    result.isCompleted = true;
                    result.message.Add(Constants.Result.Success);
                }
            }
            catch (Exception ex)
            {
                result.data = false;
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Menu.Service.DeletedRole}");
                _logger.WriteError(errorCode: ErrorCodes.Menu.Service.DeletedRole, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }

            return Task.FromResult(result);
        }

        public Task<ReturnObject<bool>> UpdateMenuRole(ParamUpdateRoleItemViewModel param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                _menuRepository.Update_RoleItem(param.role_item.ToList(), _userInfo);

                result.data = true;
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.data = false;
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Menu.Service.UpdateMenuRole}");
                _logger.WriteError(errorCode: ErrorCodes.Menu.Service.UpdateMenuRole, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }

            return Task.FromResult(result);
        }

        public Task<ReturnObject<bool>> UpdateMenuRoleSelectItem(UpdateRoleItemSection param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                _menuRepository.Update_RoleSelectItem(param, _userInfo);

                result.data = true;
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.data = false;
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Menu.Service.UpdateMenuRoleSelectItem}");
                _logger.WriteError(errorCode: ErrorCodes.Menu.Service.UpdateMenuRoleSelectItem, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }

            return Task.FromResult(result);
        }

        public Task<ReturnList<ResultGetMenuViewModel>> GetMenuRole(string param)
        {
            var result = new ReturnList<ResultGetMenuViewModel>();
            try
            {
                int roleID = int.Parse(this._securityCommon.DecryptDataUrlEncoder(param));
                var menuData = _menuRepository.Select_MenuRole(roleID, _userInfo).Result;
                var sectionData = _menuRepository.Select_MenuSectionRole(roleID, _userInfo).Result;

                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
                result.data = _initMenuDataToModel(menuData, sectionData);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Menu.Service.GetMenuRole}");
                _logger.WriteError(errorCode: ErrorCodes.Menu.Service.GetMenuRole, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }

            return Task.FromResult(result);
        }

        private List<ResultGetMenuViewModel> _initMenuDataToModel(List<MenuData> menuData, List<MenuData> sectionData)
        {
            List<ResultGetMenuViewModel> res = new List<ResultGetMenuViewModel>();

            foreach (MenuData p in menuData)
            {
                List<ResultGetMenuSectionViewModel> section = new List<ResultGetMenuSectionViewModel>();
                bool displayMenu = false;
                foreach (MenuData sp in sectionData)
                {
                    if (sp.menu_id == p.menu_id)
                    {
                        section.Add(new ResultGetMenuSectionViewModel
                        {
                            role_item_id = sp.role_item_id.ToString(),
                            is_action = sp.is_action,
                            is_display = sp.is_display,
                            menu_section_name = sp.menu_section_name,
                            menu_section_name_display = sp.menu_section_name_display,
                            section_no = sp.section_no
                        });
                        if (sp.is_display == true)
                        {
                            displayMenu = true;
                        }
                    }
                }
                res.Add(new ResultGetMenuViewModel
                {
                    menu_id = p.menu_id,
                    menu_level = p.menu_level,
                    parent_menu_id = p.parent_menu_id,
                    menu_name = p.menu_name,
                    display_name = p.display_name,
                    icon = p.icon,
                    url_controller = p.url_controller,
                    url = p.url,
                    section = section,
                    is_display = displayMenu,
                    menu_definition = p.menu_definition,
                    order_by = p.list_no
                });
            }

            return res;
        }

        private ResultRoleInfo _initRoleToModel(RoleData objData)
        {
            ResultRoleInfo res = new ResultRoleInfo();

            if (objData != null)
            {
                res.role_id = this._securityCommon.EncryptDataUrlEncoder(objData.role_id.ToString());
                res.role_name = objData.role_name;
                res.role_desc = objData.role_desc;
                res.is_super_role = objData.is_super_role;
            }

            return res;
        }
    }
}
