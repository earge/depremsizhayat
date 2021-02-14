using DepremsizHayat.Business.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DepremsizHayat.Admin.Controllers
{
    public class BaseController : Controller
    {
        public IUserService _userService;
        public IRoleService _roleService;
        public IAnalyseRequestService _analyseRequestService;
        public IStatusService _statusService;
        public BaseController(IUserService userService,
            IRoleService roleService,
            IAnalyseRequestService analyseRequestService,
            IStatusService statusService)
        {
            this._userService = userService;
            this._roleService = roleService;
            this._analyseRequestService = analyseRequestService;
            this._statusService = statusService;
        }
        protected override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            TempData["Role"] = _userService.GetByMail(ticket.Name).ROLE.NAME;
        }
    }
}