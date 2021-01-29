using DepremsizHayat.Business.IService;
using DepremsizHayat.DTO;
using DepremsizHayat.DTO.User;
using DepremsizHayat.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DepremsizHayat.Admin.Controllers
{
    public class AccountController : BaseAccountController
    {
        public AccountController(IUserService userService) : base(userService)
        {
        }
        public ActionResult Login(UserLoginRequest request)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Panel");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    BaseResponse response = new BaseResponse() { Status = false };
                    if (_userService.Login(request.E_MAIL, request.PASSWORD))
                    {
                        try
                        {
                            FormsAuthentication.SetAuthCookie(request.E_MAIL, true);
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.NameIdentifier, request.E_MAIL),
                            };
                            var userIdentity = new ClaimsIdentity(claims, "Login");
                            ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                            response.Status = true;
                        }
                        catch (Exception ex)
                        {
                            response.Message = ex.Message;
                        }
                    }
                    else
                    {
                        response.Message = "E-posta veya şifreniz kayıtlarımızdakilerle uyuşmadı.";
                    }
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ModelState.AddModelError("", "E-mail veya şifre hatalı girildi.");
                }
                ViewBag.Response = (TempData["Carrier"] != null) ? TempData["Carrier"] : null;
                return View();
            }
        }
    }
}