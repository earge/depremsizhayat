using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DepremsizHayat.Admin.Controllers
{
    //[Authorize(Roles = "SystemAdmin")]
    public class PanelController : Controller
    {
        public ActionResult Dashboard()
        {
            return View();
        }
    }
}