﻿using DepremsizHayat.Business.IService;
using DepremsizHayat.DataAccess;
using DepremsizHayat.DTO;
using DepremsizHayat.DTO.Admin;
using DepremsizHayat.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DepremsizHayat.Admin.Controllers
{
    [Authorize(Roles = "SystemAdmin")]
    public class PanelController : Controller
    {
        private IUserService _userService;
        private IRoleService _roleService;
        private IAnalyseRequestService _analyseRequestService;
        public PanelController(IUserService userService,
            IRoleService roleService,
            IAnalyseRequestService analyseRequestService)
        {
            this._userService = userService;
            this._roleService = roleService;
            this._analyseRequestService = analyseRequestService;
        }

        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult ListUserRoles()
        {
            List<RoleRequest> request = new List<RoleRequest>();
            foreach (USER_ACCOUNT user in _userService.GetAll())
            {
                List<ROLE> roles = new List<ROLE>();
                roles.AddRange(_roleService.GetAll().Where(p => p.NAME != user.ROLE.NAME));
                request.Add(new RoleRequest()
                {
                    E_MAIL = user.E_MAIL,
                    FIRST_NAME = user.FIRST_NAME,
                    LAST_NAME = user.LAST_NAME,
                    USER_ACCOUNT_ID = user.USER_ACCOUNT_ID,
                    CURRENTROLE = user.ROLE,
                    AVAILABLEROLES = roles
                });
            }
            ViewBag.RoleResponse = (TempData["Carrier"] != null) ? TempData["Carrier"] : null;
            return View(request);
        }
        public ActionResult EditRoles(int USER_ACCOUNT_ID, int ROLE_ID)
        {
            EditRoleRequest request = new EditRoleRequest()
            {
                NEW_ROLE_ID = ROLE_ID,
                USER_ACCOUNT_ID = USER_ACCOUNT_ID
            };
            BaseResponse response = new BaseResponse();
            if (_userService.UpdateUserRole(request))
            {
                response.Status = true;
                response.Message = "Değişiklikler uygulandı.";
            }
            else
            {
                response.Message = "Değişiklikler uygulanamadı.";
            }
            TempData["Carrier"] = response;
            return RedirectToAction("ListUserRoles", "Panel");
        }
        public ActionResult Requests()
        {
            List<AnalyseRequest> request = _analyseRequestService.GetAllRequests();
            return View(request);
        }
    }
}