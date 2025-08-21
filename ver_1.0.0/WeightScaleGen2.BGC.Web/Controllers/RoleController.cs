using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.ViewModels.Menu;
using WeightScaleGen2.BGC.Models.ViewModels.Role;
using WeightScaleGen2.BGC.Models.ViewModels.User;
using WeightScaleGen2.BGC.Web.Controllers.Base;
using WeightScaleGen2.BGC.Web.Services;

namespace WeightScaleGen2.BGC.Web.Controllers
{
    //[Authorize]
    public class RoleController : BaseController
    {
        private readonly ILogger<RoleController> _logger;
        private readonly UserService _userService;

        public RoleController(ILogger<RoleController> logger, UserService userService) : base(userService)
        {
            _logger = logger;
            _userService = userService;
        }

        private void SetPermission()
        {
            this._SetViewBagCurrentUserMenu((long)Models.ViewModels.Base.BaseConst.MENU_DEFINITION.ROLE);
            ViewBag.view = _GetPermission(_GetControllerName(), Constants.Action.View);
            ViewBag.created = _GetPermission(_GetControllerName(), Constants.Action.Created);
            ViewBag.edit = _GetPermission(_GetControllerName(), Constants.Action.Edit);
            ViewBag.deleted = _GetPermission(_GetControllerName(), Constants.Action.Deleted);
            ViewBag.print = _GetPermission(_GetControllerName(), Constants.Action.Print);
            ViewBag.MenuName = _GetMenuName(_GetControllerName());
        }

        private bool GetAction(string action)
        {
            bool result = _GetPermission(_GetControllerName(), action) == "Y" ? true : false;
            return result;
        }

        public bool IsToken()
        {
            string tokenVal = Request.Cookies["token"];
            if (tokenVal != null && !String.IsNullOrEmpty(tokenVal))
            {
                return true;
            }
            return false;
        }

        public IActionResult RoleManagement()
        {
            if (!IsToken())
            {
                return RedirectToAction("Index", "Login");
            }

            SetPermission();

            if (!GetAction(Constants.Action.View))
            {
                return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
            }

            var result = _userService.GetSearchUserCriteriaNotAssign(_Username());
            if (result.isCompleted && result.data.role_dll.Count() > 0)
            {
                return View(result.data);
            }
            else
            {
                _logger.LogWarning(result.message[0]);
                return View();
            }
        }

        public IActionResult SearchRoleManagement(ParamSearchUser param)
        {
            if (!IsToken())
            {
                return RedirectToAction("Index", "Login");
            }

            SetPermission();

            if (!GetAction(Constants.Action.View))
            {
                return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
            }

            var result = _userService.GetMenuRole(param.role_id, _Username());
            if (result.isCompleted && result.data.Count > 0)
            {
                return Json(result.data);
            }
            else
            {
                _logger.LogWarning(result.message[0]);
                return Json(null);
            }
        }

        public IActionResult UpdateRoleItem(ParamUpdateRoleItemViewModel param)
        {
            SetPermission();
            var result = _userService.UpdateRoleItem(param, _Username());
            if (result.isCompleted)
            {
                return Json(result);
            }
            else
            {
                _logger.LogWarning(result.message[0]);
                return Json(result);
            }
        }

        public IActionResult UpdateSectionItem(int roleItemId)
        {
            SetPermission();
            var result = _userService.UpdateRoleSelectItem(new ResultGetMenuSectionViewModel() { role_item_id = roleItemId.ToString() }, _Username());
            if (result.isCompleted)
            {
                return Json(new { status = Constants.Result.Success });
            }
            else
            {
                return Json(new { status = Constants.Result.Error });
            }
        }

        //Modal Action
        public IActionResult RoleItem(string mode, string roleId)
        {
            SetPermission();
            ResultRoleInfo resultObj = new ResultRoleInfo();
            try
            {
                switch (mode)
                {
                    case Constants.Mode.Created:
                        resultObj.mode = Constants.Mode.Created;
                        break;
                    case Constants.Mode.Updated:
                        var resRoleInfo = _userService.GetRole(this._Username(), new ResultRoleInfo() { role_id = roleId });
                        if (resRoleInfo.isCompleted && resRoleInfo.data != null)
                        {
                            resultObj.role_name = resRoleInfo.data.role_name;
                            resultObj.role_desc = resRoleInfo.data.role_desc;
                        }
                        resultObj.mode = Constants.Mode.Updated;
                        resultObj.role_id = roleId;
                        break;
                }

                return PartialView("_ModalAction", resultObj);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public IActionResult RoleInfo(string mode, string roleId)
        {
            SetPermission();
            ResultRoleInfo resultObj = new ResultRoleInfo();
            try
            {
                switch (mode)
                {
                    case Constants.Mode.Created:
                        resultObj.mode = Constants.Mode.Created;
                        break;
                    case Constants.Mode.Updated:
                        var resRoleInfo = _userService.GetRole(this._Username(), new ResultRoleInfo() { role_id = roleId });
                        if (resRoleInfo.isCompleted && resRoleInfo.data != null)
                        {
                            resultObj.role_name = resRoleInfo.data.role_name;
                            resultObj.role_desc = resRoleInfo.data.role_desc;
                        }
                        resultObj.mode = Constants.Mode.Updated;
                        resultObj.role_id = roleId;
                        break;
                }

                return View(resultObj);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public IActionResult SaveRole(ResultRoleInfo model)
        {
            SetPermission();

            if (!GetAction(Constants.Action.Created) && model.mode == Constants.Mode.Created)
            {
                return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
            }

            if (!GetAction(Constants.Action.Edit) && model.mode == Constants.Mode.Updated)
            {
                return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
            }

            switch (model.mode)
            {
                case Constants.Mode.Created:
                    var resCreate = _userService.CreatedRole(this._Username(), model);
                    if (resCreate.isCompleted)
                    {
                        return Json(new { status = Constants.Result.Success });
                    }
                    else
                    {
                        return Json(new { status = Constants.Result.Error, message = resCreate.message[0].ToString() });
                    }
                case Constants.Mode.Updated:
                    var resUpdate = _userService.UpdateRole(this._Username(), model);
                    if (resUpdate.isCompleted)
                    {
                        return Json(new { status = Constants.Result.Success });
                    }
                    else
                    {
                        return Json(new { status = Constants.Result.Error, message = resUpdate.message[0].ToString() });
                    }
                default:
                    return Json(new { status = Constants.Result.Invalid, message = "Invalid." });
            }
        }

        public IActionResult DeleteRole(string roleItemId)
        {
            SetPermission();
            try
            {
                if (!GetAction(Constants.Action.Deleted))
                {
                    return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
                }

                var res = _userService.DeleteRole(this._Username(), new ResultRoleInfo() { role_id = roleItemId });
                if (res.isCompleted && res.data == true)
                {
                    return Json(new { status = Constants.Result.Success });
                }
                else if (res.isCompleted && res.data == false)
                {
                    return Json(new { status = Constants.Result.Invalid, message = "ขอภัยไม่สามารถลบได้เนื่องจากมีผู้ใช้งาน Role นี้ในระบบ" });
                }
                else
                {
                    return Json(new { status = Constants.Result.Invalid, message = "Invalid." });
                }
            }
            catch (System.Exception ex)
            {
                return Json(new { status = Constants.Result.Error, message = ex.Message });
            }
        }

    }
}
