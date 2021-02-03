using DepremsizHayat.Business.IService;
using DepremsizHayat.DTO;
using DepremsizHayat.DTO.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        [Authorize(Roles = "SystemAdmin,User,Expert")]
        public ActionResult Index()
        {
            return View();
        }
        FormsAuthenticationTicket GetTicket()
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            return FormsAuthentication.Decrypt(authCookie.Value);
        }
        public ActionResult Name()
        {
            return Content(_userService.GetByMail(GetTicket().Name).FIRST_NAME);
        }
        public ActionResult Surname()
        {
            return Content(_userService.GetByMail(GetTicket().Name).LAST_NAME);
        }
        public ActionResult SendAnalyseRequest()
        {
            if (Request.Form["images"].Length>0)
            {
                foreach (var item in Request.Form["images"])
                {
                    item.ToString();

                }
            }
            if (Request.Files.Count > 0)
            {
                try
                {
                    System.Web.HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname = file.FileName;
                        if (!(Directory.Exists(Server.MapPath("~//Sources/" + "someValue"))))
                        {
                            Directory.CreateDirectory(Server.MapPath("~/Sources/" + "someValue"));
                        }
                        fname = Path.Combine(Server.MapPath("~/Sources/" + "someValue"),
                                       fname);
                        file.SaveAs(fname);
                    }
                    DataAccess.ANALYSE_REQUEST request = new DataAccess.ANALYSE_REQUEST()
                    {
                        ADDRESS = Request.Form[0],
                        COUNTRY = Request.Form[1],
                        CREATED_DATE = Convert.ToDateTime(Request.Form[2]),
                        DELETED = false,
                        DISTRICT = Request.Form[3],
                        NUMBER_OF_FLOORS = Convert.ToInt32(Request.Form[4]),
                        PHONE_NUMBER_1 = Request.Form[5],
                        PHONE_NUMBER_2 = Request.Form[6],
                        STATUS_ID = _statusService.GetIdByName("Bekliyor"),
                        USER_ACCOUNT_ID = CurrentUser().USER_ACCOUNT_ID,
                        USER_NOTE = Request.Form[8],
                        YEAR_OF_CONSTRUCTION = Convert.ToInt32(Request.Form[9])

                    };
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                }
            }
            else
            {
                //tek resim
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
        public ActionResult EditProfile(EditNameSurnameRequest request)
        {
            BaseResponse response = new BaseResponse();
            if (ModelState.IsValid && (request.Name != null || request.Surname != null))
            {
                request.USER_ACCOUNT_ID = CurrentUser().USER_ACCOUNT_ID;
                response = _userService.EditNameSurname(request);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            return View(response);
        }
        //[ChildActionOnly]
        public ActionResult MyRequests()
        {
            //List<DataAccess.ANALYSE_REQUEST> request = _analyseRequestService.GetRequestsByUserId(CurrentUser().USER_ACCOUNT_ID);
            return View(/*request*/);
        }
    }
}