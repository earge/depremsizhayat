using DepremsizHayat.Business;
using DepremsizHayat.Business.IService;
using DepremsizHayat.DTO.User;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DepremsizHayat.App.Controllers
{
    public class AccountController : Controller
    {
        private IUserService _userService;
        public AccountController(IUserService userService)
        {
            this._userService = userService;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Register(CreateUserRequest request)
        {
            if (ModelState.IsValid)
            {
                _userService.CreateUser(new DataAccess.USER()
                {
                    ACTIVE = false,
                    DELETED = false,
                    E_MAIL = request.E_MAIL,
                    FIRST_NAME = request.FIRST_NAME,
                    LAST_NAME = request.LAST_NAME,
                    PASSWORD = request.PASSWORD,
                    PROFILE_IMAGE = "dummy",
                    ROLE_ID = 1
                });
                return RedirectToAction("Login");
            }
            return View();
        }
        public ActionResult Activate(string actCode, string email)
        {
            return View();
        }
        public ActionResult Login(UserLoginRequest request)
        {
            if (ModelState.IsValid)
            {
                if (_userService.Login(request.E_MAIL,request.PASSWORD))
                {
                    FormsAuthentication.SetAuthCookie(request.E_MAIL, true);
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, request.E_MAIL),
            };
                    var userIdentity = new ClaimsIdentity(claims, "Login");
                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "E-mail veya şifre hatalı girildi.");
            }
            return View();
        }
    }
}