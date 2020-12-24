using DepremsizHayat.Business;
using DepremsizHayat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DepremsizHayat.App.Controllers
{
    public class RegisterController : Controller
    {
        UserAccountManager uam = new UserAccountManager();
        // GET: Register
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Reg(UserAccountCreateModel model)
        {
            if (ModelState.IsValid)
            {
                uam.CreateUser(model);
                if (model.IsSuccess)
                {
                    return RedirectToAction("Welcome");
                }
                else
                {
                    //
                }
            }
            else
            {
                model.ErrorCodes.Add("400");

            }
            return View(model);
        }
        public ActionResult Activate(string actCode, string email)
        {
            TempData["Result"] = uam.Activate(actCode, email);
            return View();
        }
    }
}