using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using WeightScaleGen2.BGC.Models.ViewModels.Base;
using WeightScaleGen2.BGC.Models.ViewModels.Menu;
using WeightScaleGen2.BGC.Web.Services;

namespace WeightScaleGen2.BGC.Web.Controllers.Base
{
    public class BaseController : Controller
    {
        private readonly UserService _userService;

        public BaseController(UserService userService)
        {
            _userService = userService;
        }

        protected string _GetControllerName()
        {
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            return controllerName;
        }

        protected string _GetPermission(string controller, string action)
        {
            string result = "N";

            var section = _GetSection();

            if (section == null) { return result; }
            ;

            var objController = section.FirstOrDefault(i => i.url_controller == controller);
            if (objController != null)
            {
                var resultSection = objController.section.FirstOrDefault(i => i.menu_section_name.Equals(action) && i.is_action == true);
                result = resultSection != null ? "Y" : "N";
            }

            return result;
        }

        protected string _GetMenuName(string controller)
        {
            string result = "";

            var section = _GetSection();
            if (section == null) { return result; }
            ;

            var objController = section.FirstOrDefault(i => i.url_controller == controller);
            if (objController != null)
            {
                var objParentMenuId = objController.parent_menu_id;
                if (objParentMenuId != 0)
                {
                    var getMenuMain = section.Where(i => i.menu_id == objParentMenuId).FirstOrDefault();
                    if (getMenuMain != null)
                    {
                        result = "Home / " + getMenuMain.display_name + " / " + objController.display_name;
                    }
                }
                else
                {
                    result = "Home / " + objController.display_name;

                }
            }

            return result;
        }

        protected List<ResultGetMenuViewModel> _GetSection()
        {
            string username = "";
            string name = "";
            string tokenVal = Request.Cookies["token"];
            if (!String.IsNullOrEmpty(tokenVal))
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(tokenVal);
                var tokenS = jsonToken as JwtSecurityToken;
                username = tokenS.Claims.FirstOrDefault(claim => claim.Type == "username")?.Value;
                name = tokenS.Claims.FirstOrDefault(claim => claim.Type == "name")?.Value;
            }
            else
            {
                username = User.Claims.FirstOrDefault(claim => claim.Type == "preferred_username").Value;
                name = User.Claims.FirstOrDefault(claim => claim.Type == "name").Value;
            }
            return _userService.GetMenu(name: name, username: username).data;
        }

        protected void _SetViewBagCurrentUserMenu(long menuDiffinition)
        {
            string username = "";
            string name = "";
            string tokenVal = Request.Cookies["token"];
            if (!String.IsNullOrEmpty(tokenVal))
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(tokenVal);
                var tokenS = jsonToken as JwtSecurityToken;
                username = tokenS.Claims.FirstOrDefault(claim => claim.Type == "username")?.Value;
                name = tokenS.Claims.FirstOrDefault(claim => claim.Type == "name")?.Value;
            }
            else
            {
                username = User.Claims.FirstOrDefault(claim => claim.Type == "preferred_username").Value;
                name = User.Claims.FirstOrDefault(claim => claim.Type == "name").Value;
            }

            ViewBag.Login = "N";
            if (username == "" || username == null)
            {
                var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                var urlIndex = configuration.GetSection("Home").GetSection("LoginUrl").Value;
                ViewBag.Login = "Y";
                ViewBag.LoginUrl = urlIndex;
            }

            ResultGetMenuViewModel res = new ResultGetMenuViewModel();

            bool isPermis = false;
            var getUserInfo = _userService.GetUserByUsername(_Username()).data;
            var getUserMenu = _userService.GetMenu(name: name, username: username).data;

            ViewBag.UserMenu = getUserMenu;
            ViewBag.username = _Username();
            ViewBag.name = _Name();
            ViewBag.token = Token();
            ViewBag.rolename = $"({getUserInfo.role_name})";
            ViewBag.AssignPermission = "N";

            if (getUserMenu != null)
            {
                if (getUserMenu.Count > 0)
                {
                    ViewBag.AssignPermission = "Y";
                }
            }

            if (menuDiffinition != (long)BaseConst.MENU_DEFINITION.NOTSET)
            {
                foreach (var index in getUserMenu)
                {
                    if (index.menu_definition == menuDiffinition)
                    {
                        res = index;
                        isPermis = true;
                        break;
                    }
                }

                if (isPermis)
                {
                    ViewBag.UserMenuSection = res;
                }
                else
                {
                    ViewBag.UserMenuSection = null;
                }
            }

            ViewBag.CurrentMenu = menuDiffinition;
        }

        protected string _Username()
        {
            string username = "";
            string tokenVal = Request.Cookies["token"];
            if (!String.IsNullOrEmpty(tokenVal))
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(tokenVal);
                var tokenS = jsonToken as JwtSecurityToken;
                username = tokenS.Claims.FirstOrDefault(claim => claim.Type == "username")?.Value;
            }
            else
            {
                username = User.Claims.FirstOrDefault(claim => claim.Type == "preferred_username").Value;
            }
            return username;
        }

        protected string _Name()
        {
            string name = "";
            string tokenVal = Request.Cookies["token"];
            if (!String.IsNullOrEmpty(tokenVal))
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(tokenVal);
                var tokenS = jsonToken as JwtSecurityToken;
                name = tokenS.Claims.FirstOrDefault(claim => claim.Type == "name")?.Value;
            }
            else
            {
                name = User.Claims.FirstOrDefault(claim => claim.Type == "name").Value;
            }
            return name;
        }

        public IActionResult GetListNotify()
        {
            NotifyAlert resultObj = new NotifyAlert();
            try
            {
                //NotifyAlertList notifyAlertList1 = new NotifyAlertList();
                //notifyAlertList1.url = "/Home/Index";
                //notifyAlertList1.status = "primary";
                //notifyAlertList1.text = "Welcome to BGC System";
                //notifyAlertList1.id = "1,2,3,4,5,6";
                //resultObj.data.Add(notifyAlertList1);

                //NotifyAlertList notifyAlertList2 = new NotifyAlertList();
                //notifyAlertList2.url = "/Home/Index";
                //notifyAlertList2.status = "success";
                //notifyAlertList2.text = "Welcome to BGC System";
                //notifyAlertList2.id = "1,2,3,4,5,6";
                //resultObj.data.Add(notifyAlertList2);

                //NotifyAlertList notifyAlertList3 = new NotifyAlertList();
                //notifyAlertList3.url = "/Home/Index";
                //notifyAlertList3.status = "warning";
                //notifyAlertList3.text = "Welcome to BGC System";
                //notifyAlertList3.id = "1,2,3,4,5,6";
                //resultObj.data.Add(notifyAlertList3);

                //NotifyAlertList notifyAlertList4 = new NotifyAlertList();
                //notifyAlertList4.url = "/Home/Index";
                //notifyAlertList4.status = "danger";
                //notifyAlertList4.text = "Welcome to BGC System";
                //notifyAlertList4.id = "1,2,3,4,5,6";
                //resultObj.data.Add(notifyAlertList4);

                resultObj.total = resultObj.data.Count;

                return Json(new { res = resultObj });
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string Token()
        {
            return !String.IsNullOrEmpty(Request.Cookies["token"]) ? Request.Cookies["token"] : null;
        }
    }
}
