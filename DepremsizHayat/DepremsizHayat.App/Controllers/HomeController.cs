using DepremsizHayat.Business.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DepremsizHayat.App.Controllers
{
    public class HomeController : Controller
    {
        private IUserService _userService;
        public HomeController(IUserService userService)
        {
            this._userService = userService;
        }
        public ActionResult Index()
        {
            var x = _userService.GetAll();
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
    }
}