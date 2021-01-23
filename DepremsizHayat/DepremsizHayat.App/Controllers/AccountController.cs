using DepremsizHayat.Business;
using DepremsizHayat.Business.IService;
using DepremsizHayat.DTO;
using DepremsizHayat.DTO.Models;
using DepremsizHayat.DTO.User;
using DepremsizHayat.Security;
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
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    string activationCode = Encryptor.Encrypt(new Random().Next(100000, 999999).ToString());
                    RegisterResponse response = new RegisterResponse() { Status = false };
                    try
                    {
                        _userService.CreateUser(new UserModel()
                    {
                        ACTIVE = false,
                        DELETED = false,
                        E_MAIL = request.E_MAIL,
                        FIRST_NAME = request.FIRST_NAME,
                        CREATED_DATE = DateTime.Now,
                        LAST_NAME = request.LAST_NAME,
                        PASSWORD = request.PASSWORD,
                        ROLE_ID = 1,
                        ACTIVATION_CODE = activationCode
                    });
                    response.Status = true;
                    response.Url = "?actCode=&mail=" + Encryptor.Encrypt(request.E_MAIL);
                    }
                    catch (Exception)
                    {
                    }
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ModelState.AddModelError("", "Girilen bilgiler hatalı.");
                }
                return View();
            }
        }
        public ActionResult Activate(string actCode, string mail)
        {
            BaseResponse response = new BaseResponse() { Status = false };
            string email = (mail != null) ? Decryptor.Decrypt(mail) : null;
            if (email != null && _userService.GetByMail(email) != null)
            {
                if (_userService.GetByMail(email).ACTIVE == true)
                {
                    response.Message = "Hesabınız zaten aktifleştirilmiş.";
                    TempData["Response"] = response;
                    return RedirectToAction("Login");
                }
                else
                {
                    if (actCode != null && actCode != "")
                    {
                        actCode = (actCode.Length == 6) ? Encryptor.Encrypt(actCode) : actCode;
                        if (_userService.Activate(actCode, email))
                        {
                            response.Status = true;
                            response.Message = "Hesabınız başarıyla aktifleştirildi.";
                            TempData["Response"] = response;
                            return RedirectToAction("Login");
                        }
                        else
                        {
                            response.Message = "Hesabınız aktifleştirilemedi, lütfen tekrar deneyin.";
                        }
                    }
                    else
                    {
                        return View();
                    }
                }
                TempData["Response"] = response;
                return View();
            }
            return RedirectToAction("Login");
        }
        public ActionResult Login(UserLoginRequest request, BaseResponse model)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
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
                return View();
            }
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        public ActionResult ForgotPassword(string mail)
        {
            BaseResponse response = new BaseResponse();
            if (mail != null)
            {
                var result = _userService.SendResetMail(mail).Split('_');
                if (result[0] == "+")
                {
                    response.Status = true;
                    response.Message = result[1];
                }
                else
                {
                    response.Status = false;
                    response.Message = result[1];
                }
            }
            TempData["Response"] = response;
            return View();
        }
        public ActionResult SetNewPassword(string authCode, ResetPasswordRequest request)
        {
            //if (authCode != null && _userService.CheckResetAuth(authCode, request.Mail))
            //{
            //    BaseResponse response = new BaseResponse() { Status = false };
            //    if (_userService.ResetPassword(request))
            //    {
            //        response.Status = true;
            //        response.Message = "Şifreniz başarıyla güncellendi.";
            //        TempData["Response"] = response;
            //        return RedirectToAction("Login");
            //    }
            //    else
            //    {
            //        TempData["Response"] = response;
            //        response.Message = "Şifreniz güncellenemedi. Lütfen tekrar deneyin.";
            //        return View();
            //    }
            //}
            //else
            //{
            //    return RedirectToAction("Login");
            //}
            return View();
        }
    }
}