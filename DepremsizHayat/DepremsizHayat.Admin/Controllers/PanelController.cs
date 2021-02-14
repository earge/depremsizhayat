using DepremsizHayat.Business.IService;
using DepremsizHayat.DataAccess;
using DepremsizHayat.DTO;
using DepremsizHayat.DTO.Admin;
using DepremsizHayat.Security;
using DepremsizHayat.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Web.Security;

namespace DepremsizHayat.Admin.Controllers
{
    [Authorize(Roles = "SystemAdmin,Expert")]
    public class PanelController : BaseController
    {
        public PanelController(IUserService userService, IRoleService roleService, IAnalyseRequestService analyseRequestService, IStatusService statusService) : base(userService, roleService, analyseRequestService, statusService)
        {
        }

        [Authorize(Roles = "SystemAdmin")]
        public ActionResult EditRequest(AnalyseDetailRequest request)
        {
            BaseResponse response = _analyseRequestService.UpdateRequestDetail(request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Dashboard()
        {
            return View();
        }
        [Authorize(Roles = "SystemAdmin")]
        public ActionResult ListUserRoles(int? p)
        {
            int dataPerPage = 9;
            decimal count = _userService.GetAll().Count;
            decimal totalPages = Math.Ceiling(count / (decimal)dataPerPage);
            p = (p != null) ? (int)p : 1;
            p = (p >= totalPages) ? (int?)totalPages : p;
            p = (p <= 0) ? 1 : p;
            List<RoleRequest> request = new List<RoleRequest>();
            foreach (USER_ACCOUNT user in _userService.GetAll().ToList().ToPagedList(p ?? 1, dataPerPage))
            {
                List<ROLE> roles = new List<ROLE>();
                roles.AddRange(_roleService.GetAll().Where(prmtr => prmtr.NAME != user.ROLE.NAME));
                request.Add(new RoleRequest()
                {
                    E_MAIL = user.E_MAIL,
                    FIRST_NAME = user.FIRST_NAME,
                    LAST_NAME = user.LAST_NAME,
                    USER_ACCOUNT_ID = Convert.ToString(user.USER_ACCOUNT_ID),
                    CURRENTROLE = user.ROLE,
                    AVAILABLEROLES = roles
                });
            }
            PaginationModel<RoleRequest> pagination = new PaginationModel<RoleRequest>()
            {
                DataCount = _userService.GetAll().Count,
                DataList = request,
                DataPerPage = dataPerPage
            };
            ViewBag.RoleResponse = (TempData["Carrier"] != null) ? TempData["Carrier"] : null;
            return View(pagination);
        }
        [Authorize(Roles = "SystemAdmin")]
        public ActionResult EditRoles(string USER_ACCOUNT_ID, int ROLE_ID)
        {
            EditRoleRequest request = new EditRoleRequest()
            {
                NEW_ROLE_ID = ROLE_ID,
                USER_ACCOUNT_ID = Convert.ToString(Decryptor.Decrypt(USER_ACCOUNT_ID))
            };
            BaseResponse response = new BaseResponse();
            if (_userService.UpdateUserRole(request))
            {
                response.Status = true;
                response.Message.Add("Değişiklikler uygulandı.");
                response.Message.Add("1Değişiklikler uygulandı.");
                response.Message.Add("2Değişiklikler uygulandı.");
            }
            else
            {
                response.Message.Add("Değişiklikler uygulanamadı.");
            }
            TempData["Carrier"] = response;
            return RedirectToAction("ListUserRoles", "Panel");
        }
        [Authorize(Roles = "SystemAdmin")]
        public ActionResult Requests(int? p)
        {
            int dataPerPage = 7;
            decimal count = _analyseRequestService.GetAllRequests().Count;
            decimal totalPages = Math.Ceiling(count / (decimal)dataPerPage);
            p = (p != null) ? (int)p : 1;
            p = (p >= totalPages) ? (int?)totalPages : p;
            p = (p <= 0) ? 1 : p;
            List<AnalyseRequest> list = new List<AnalyseRequest>();
            list.AddRange(_analyseRequestService.GetAllRequests().ToPagedList(p ?? 1, dataPerPage));
            PaginationModel<AnalyseRequest> request = new PaginationModel<AnalyseRequest>()
            {
                DataList = list,
                DataCount = (int)count,
                DataPerPage = dataPerPage
            };
            return View(request);
        }
        [Authorize(Roles = "SystemAdmin")]
        public ActionResult RequestDetail(string id)
        {
            AnalyseDetailRequest request = _analyseRequestService.GetDetailRequest(id);
            return Json(request);
        }
        //[Authorize(Roles = "SystemAdmin")]
        //public ActionResult GetRequests()
        //{
        //    List<AnalyseRequest> request = _analyseRequestService.GetAllRequests();
        //    return Json(request, JsonRequestBehavior.AllowGet);
        //}
        [Authorize(Roles = "SystemAdmin")]
        public ActionResult DenyRequests(List<string> idList)
        {
            BaseResponse response = new BaseResponse();
            if (_analyseRequestService.DenyRequests(idList))
            {
                response.Status = true;
                response.Message.Add("Seçili talepler reddedildi.");
            }
            else
                response.Message.Add("İşlem başarısız. Lütfen tekrar deneyin.");
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "SystemAdmin")]
        public ActionResult AllowRequests(List<string> idList)
        {
            BaseResponse response = new BaseResponse();
            if (_analyseRequestService.AllowRequests(idList))
            {
                response.Status = true;
                response.Message.Add("Seçili talepler onaylandı.");
            }
            else
                response.Message.Add("İşlem başarısız. Lütfen tekrar deneyin.");
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Expert")]
        public ActionResult ExpertRequests(int? p)
        {
            int dataPerPage = 15;
            decimal count = _analyseRequestService.GetAllRequests().Count;
            decimal totalPages = Math.Ceiling(count / (decimal)dataPerPage);
            p = (p != null) ? (int)p : 1;
            p = (p >= totalPages) ? (int?)totalPages : p;
            p = (p <= 0) ? 1 : p;
            List<AnalyseRequest> list = new List<AnalyseRequest>();
            list.AddRange(_analyseRequestService.GetAllRequests().ToPagedList(p ?? 1, dataPerPage));
            PaginationModel<AnalyseRequest> request = new PaginationModel<AnalyseRequest>()
            {
                DataList = list,
                DataCount = (int)count,
                DataPerPage = dataPerPage
            };
            return View(request);
        }
        public ActionResult PageNotFound()
        {
            return View();
        }
    }
}