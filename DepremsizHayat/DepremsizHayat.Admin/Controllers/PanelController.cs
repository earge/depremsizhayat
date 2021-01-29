using DepremsizHayat.Business.IService;
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
        public PanelController(IUserService userService)
        {
            this._userService = userService;
        }

        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult EditRoles()
        {
            return View(_userService.GetAll());
        }
    }
}