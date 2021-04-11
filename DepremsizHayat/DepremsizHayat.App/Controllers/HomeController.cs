using DepremsizHayat.Business.IService;
using DepremsizHayat.DTO;
using DepremsizHayat.DTO.User;
using DepremsizHayat.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DepremsizHayat.App.Controllers
{
    [Authorize(Roles = "SystemAdmin, User, Expert")]
    public class HomeController : Controller
    {
        private IUserService _userService;
        private IAnalyseRequestService _analyseRequestService;
        private IStatusService _statusService;
        public HomeController(IUserService userService,
            IAnalyseRequestService analyseRequestService,
            IStatusService statusService)
        {
            this._userService = userService;
            this._analyseRequestService = analyseRequestService;
            this._statusService = statusService;
        }
        private DataAccess.USER_ACCOUNT CurrentUser()
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            return _userService.GetByMail(ticket.Name);
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RequestSent(string Code)
        {
            if (!string.IsNullOrEmpty(Code))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Name()
        {
            return Content(CurrentUser().FIRST_NAME);
        }
        public ActionResult Surname()
        {
            return Content(CurrentUser().LAST_NAME);
        }
        public ActionResult Mail()
        {
            return Content(CurrentUser().E_MAIL);
        }
        public ActionResult SendAnalyseRequest()
        {
            var encryptedId = Encryptor.Encrypt(CurrentUser().USER_ACCOUNT_ID.ToString());
            if (Request.Files.Count > 0)
            {
                try
                {
                    int totalContentLength = 0;
                    List<string> photos = new List<string>();
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname = file.FileName;
                        photos.Add("/Sources/" + encryptedId + "/" + fname);
                        if (!Directory.Exists(Server.MapPath("~/Sources/" + encryptedId + "/")))
                        {
                            Directory.CreateDirectory(Server.MapPath("~/Sources/" + encryptedId + "/"));
                        }
                        fname = Path.Combine(Server.MapPath("~/Sources/" + encryptedId + "/"),
                                       fname);
                        file.SaveAs(fname);
                        totalContentLength = totalContentLength + file.ContentLength;
                    }
                    DataAccess.ANALYSE_REQUEST request = new DataAccess.ANALYSE_REQUEST()
                    {
                        ADDRESS = Request.Form["address"],
                        COUNTRY = Request.Form["country"],
                        CREATED_DATE = DateTime.Now,
                        DELETED = false,
                        DISTRICT = Request.Form["district"],
                        NUMBER_OF_FLOORS = Convert.ToInt32(Request.Form["floor"]),
                        PHONE_NUMBER_1 = Request.Form["phone1"],
                        PHONE_NUMBER_2 = Request.Form["phone2"],
                        STATUS_ID = _statusService.GetIdByCode(Resources.AnalyseRequestStatusCodes.WaitingAdminConfirmation),
                        USER_ACCOUNT_ID = CurrentUser().USER_ACCOUNT_ID,
                        USER_NOTE = Request.Form["note"],
                        YEAR_OF_CONSTRUCTION = Convert.ToInt32(Request.Form["year"])
                    };
                    var response = _analyseRequestService.SendNewRequest(request, photos);
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                }
            }
            else
            {
                DataAccess.ANALYSE_REQUEST request = new DataAccess.ANALYSE_REQUEST()
                {
                    ADDRESS = Request.Form["address"],
                    COUNTRY = Request.Form["country"],
                    CREATED_DATE = DateTime.Now,
                    DELETED = false,
                    DISTRICT = Request.Form["district"],
                    NUMBER_OF_FLOORS = Convert.ToInt32(Request.Form["floor"]),
                    PHONE_NUMBER_1 = Request.Form["phone1"],
                    PHONE_NUMBER_2 = Request.Form["phone2"],
                    STATUS_ID = _statusService.GetIdByCode("pendingconf"),
                    USER_ACCOUNT_ID = CurrentUser().USER_ACCOUNT_ID,
                    USER_NOTE = Request.Form["note"],
                    YEAR_OF_CONSTRUCTION = Convert.ToInt32(Request.Form["year"])
                };
                return Json(_analyseRequestService.SendNewRequest(request, null), JsonRequestBehavior.AllowGet);
            }
            return View();
        }
        public ActionResult PageNotFound()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult EditProfile(EditProfileRequest request)
        {
            BaseResponse response = new BaseResponse();
            if (request.Name != null || request.Surname != null || request.Password != null)
            {
                request.USER_ACCOUNT_ID = Convert.ToString(CurrentUser().USER_ACCOUNT_ID);
                response = _userService.EditProfile(request);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            return View(response);
        }
        public ActionResult MyRequests()
        {
            return View();
        }
        public JsonResult MyRequests_()
        {
            List<MyAnalyseRequest> request = _analyseRequestService.GetRequestsByUserId(CurrentUser().USER_ACCOUNT_ID);
            return Json(request, JsonRequestBehavior.AllowGet);
        }
        public JsonResult RequestDetail(string requestId)
        {
            return Json(_analyseRequestService.GetRequestDetail(requestId), JsonRequestBehavior.AllowGet);
        }
    }
}