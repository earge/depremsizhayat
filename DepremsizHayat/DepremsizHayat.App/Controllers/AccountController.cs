using DepremsizHayat.Business;
using DepremsizHayat.Business.IService;
using DepremsizHayat.DTO;
using DepremsizHayat.DTO.Models;
using DepremsizHayat.DTO.User;
using DepremsizHayat.Security;
using DepremsizHayat.Utility;
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
    public class AccountController : BaseAccountController
    {
        public AccountController(IUserService userService) : base(userService)
        {
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
    }
}

