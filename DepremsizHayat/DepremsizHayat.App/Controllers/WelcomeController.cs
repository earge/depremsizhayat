﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DepremsizHayat.App.Controllers
{
    public class WelcomeController : Controller
    {
        // GET: Welcome
        public ActionResult Index()
        {
            return View();
        }
    }
}