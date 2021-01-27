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
                    if (_userService.GetByMail(request.E_MAIL) == null)
                    {
                        string activationCode = Encryptor.Encrypt(new Random().Next(100000, 999999).ToString());
                        RegisterResponse response = new RegisterResponse() { Status = false };
                        //try
                        //{
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
                        //}
                        //catch (Exception)
                        //{
                        //}
                        return Json(response, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new BaseResponse() { Status = false, Message = "Bu e-posta adresi zaten sistemimizde kayıtlı." }, JsonRequestBehavior.AllowGet);
                    }
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
                    TempData["Carrier"] = response;
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
                            TempData["Carrier"] = response;
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
                ViewBag.Response = response;
                return View();
            }
            return RedirectToAction("Login");
        }
        public ActionResult Login(UserLoginRequest request)
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
                ViewBag.Response = (TempData["Carrier"] != null) ? TempData["Carrier"] : null;
                return View();
            }
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
                response = new BaseResponse()
                {
                    Message = "Mail adresi boş olamaz"
                };
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

