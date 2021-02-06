using DepremsizHayat.Business.IService;
using DepremsizHayat.DTO;
using DepremsizHayat.DTO.User;
using DepremsizHayat.Security;
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
    public class AccountController : Controller
    {
        private IUserService _userService;
        public AccountController(IUserService userService)
        {
            this._userService = userService;
        }
        public ActionResult Login(UserLoginRequest request,string returnUrl)
        {
            returnUrl = (returnUrl != "undefined") ? returnUrl : null;
            BaseResponse response = new BaseResponse() { Status = false };
            if (HttpContext.User.Identity.IsAuthenticated && HttpContext.User.IsInRole("SystemAdmin"))
            {
                if (returnUrl!=null)
                {
                    response.Status = true;
                    response.Message.Add(returnUrl);
                }
                return RedirectToAction("ListUserRoles", "Panel");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    
                    if (_userService.Login(request.E_MAIL, request.PASSWORD))
                    {
                        try
                        {
                            FormsAuthentication.SetAuthCookie(request.E_MAIL, true);
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.NameIdentifier, request.E_MAIL),
                                new Claim(ClaimTypes.Role,_userService.GetByMail(request.E_MAIL).ROLE.NAME)
                            };
                            var userIdentity = new ClaimsIdentity(claims, "Login");
                            ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                            response.Status = true;
                        }
                        catch (Exception ex)
                        {
                            response.Message.Add(ex.Message);
                        }
                    }
                    else
                    {
                        response.Message.Add("E-posta veya şifreniz kayıtlarımızdakilerle uyuşmadı.");
                    }
                    if (response.Status)
                    {
                        if (returnUrl!=null)
                        {
                            response.Message.Add(returnUrl);
                        }
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
        public ActionResult NameSurname()
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            return Content(string.Concat(_userService.GetByMail(ticket.Name).FIRST_NAME, " ", _userService.GetByMail(ticket.Name).LAST_NAME));
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        public ActionResult ForgotPassword()
        {
            return View();
        }
        public JsonResult SendForgotLink(ForgotPasswordRequest request)
        {
            BaseResponse response = null;
            if (request != null && request.Mail != null && request.Mail != "")
            {
                response = _userService.SendResetMail(request.Mail);
                ViewBag.Response = response;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            else
            {
                response = new BaseResponse();
                response.Message.Add("Mail adresi boş olamaz");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SetForgottenPassword(string authCode, string newPassword)
        {
            if (authCode != null)
            {
                ResetForgottenPaswordResponse response = _userService.CheckResetAuth(Decryptor.Decrypt(authCode));
                if (response.Status != false)
                {
                    if (newPassword != null)
                    {
                        TempData["Carrier"] = ResetForgottenPassword(authCode, newPassword);
                        if (((BaseResponse)TempData["Carrier"]).Status)
                            return RedirectToAction("Login");
                        else
                            return View();
                    }
                    else
                        return View();
                }
                else
                {
                    TempData["Carrier"] = response;
                    return RedirectToAction("Login", "Account");
                }
            }
            else
            {
                //404
                return RedirectToAction("Login", "Account");
            }
        }
        private BaseResponse ResetForgottenPassword(string authCode, string newPassword)
        {
            var user = _userService.GetByResetAuth(Decryptor.Decrypt(authCode));
            ResetPasswordRequest request = new ResetPasswordRequest()
            {
                Mail = user.E_MAIL,
                NewPassword = newPassword,
                PASSWORD_RESET_HELPER = Decryptor.Decrypt(authCode)
            };
            return _userService.ResetForgottenPassword(request);
        }
    }
}