using DepremsizHayat.Business.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DepremsizHayat.App.Controllers
{

    public class HomeController : Controller
    {
        private IUserService _userService;
        public HomeController(IUserService userService)
        {
            this._userService = userService;

        }
        //private DataAccess.USER_ACCOUNT CurrentUser()
        //{
        //    HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
        //    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
        //    return _userService.GetByMail(ticket.Name);
        //}
        public ActionResult NameSurname()
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            return Content(string.Concat(_userService.GetByMail(ticket.Name).FIRST_NAME, " ", _userService.GetByMail(ticket.Name).LAST_NAME));
        }
        [Authorize(Roles = "SystemAdmin,User,Expert")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        public ActionResult PageNotFound()
        {
            return View();
        }
    }
}