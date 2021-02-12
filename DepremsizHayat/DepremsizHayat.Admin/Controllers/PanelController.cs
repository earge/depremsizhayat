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

namespace DepremsizHayat.Admin.Controllers
{
    [Authorize(Roles = "SystemAdmin")]
    public class PanelController : Controller
    {
        private IUserService _userService;
        private IRoleService _roleService;
        private IAnalyseRequestService _analyseRequestService;
        private IStatusService _statusService;
        public PanelController(IUserService userService,
            IRoleService roleService,
            IAnalyseRequestService analyseRequestService,
            IStatusService statusService)
        {
            this._userService = userService;
            this._roleService = roleService;
            this._analyseRequestService = analyseRequestService;
            this._statusService = statusService;
        }
        public ActionResult EditRequest(AnalyseDetailRequest request)
        {
            BaseResponse response = _analyseRequestService.UpdateRequestDetail(request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult ListUserRoles(int? p)
        {
            p = (p != null) ? (int)p : 1;
            List<RoleRequest> request = new List<RoleRequest>();
            decimal totalPages = _userService.GetAll().Count / 5;
            foreach (USER_ACCOUNT user in _userService.GetAll().ToList().ToPagedList(p ?? 1, 5))
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
                CurrentPage = (int)p,
                TotalPages = (int)Math.Ceiling(totalPages),
                DataList = request
            };
            ViewBag.RoleResponse = (TempData["Carrier"] != null) ? TempData["Carrier"] : null;
            return View(pagination);
        }
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
        public ActionResult Requests()
        {
            List<AnalyseRequest> request = _analyseRequestService.GetAllRequests();
            return View(request);
        }
        public ActionResult RequestDetail(string id)
        {
            AnalyseDetailRequest request = _analyseRequestService.GetDetailRequest(id);
            return Json(request);
        }
        public ActionResult GetRequests()
        {
            List<AnalyseRequest> request = _analyseRequestService.GetAllRequests();
            return Json(request, JsonRequestBehavior.AllowGet);
        }
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
    }
}