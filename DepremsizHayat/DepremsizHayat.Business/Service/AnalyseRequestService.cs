using DepremsizHayat.Business.IService;
using DepremsizHayat.Business.IServiceRepository;
using DepremsizHayat.Business.UnitOfWork;
using DepremsizHayat.DataAccess;
using DepremsizHayat.DTO;
using DepremsizHayat.DTO.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.Business.Service
{
    public class AnalyseRequestService : IAnalyseRequestService
    {
        private IAnalyseRequestRepository _analyseRequestRepository;
        private IUserRepository _userRepository;
        private IStatusRepository _statusRepository;
        private IUnitOfWork _unitOfWork;
        public AnalyseRequestService(IAnalyseRequestRepository analyseRequestRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IStatusRepository statusRepository)
        {
            this._analyseRequestRepository = analyseRequestRepository;
            this._userRepository = userRepository;
            this._unitOfWork = unitOfWork;
            this._statusRepository = statusRepository;
        }


        public List<AnalyseRequest> GetAllRequests()
        {
            List<AnalyseRequest> list = new List<AnalyseRequest>();

            foreach (DataAccess.ANALYSE_REQUEST analyse in _analyseRequestRepository.GetAll())
            {
                var analyseCache = new AnalyseRequest()
                {
                    ADDRESS = analyse.ADDRESS,
                    ANALYSIS_REQUEST_ID = analyse.ANALYSIS_REQUEST_ID,
                    COUNTRY = analyse.COUNTRY,
                    CREATED_DATE = analyse.CREATED_DATE,
                    DISTRICT = analyse.DISTRICT,
                    NUMBER_OF_FLOORS = analyse.NUMBER_OF_FLOORS,
                    PHONE_NUMBER_1 = analyse.PHONE_NUMBER_1,
                    PHONE_NUMBER_2 = analyse.PHONE_NUMBER_2,
                    USER_ACCOUNT_ID = analyse.USER_ACCOUNT_ID,
                    USER_NOTE = analyse.USER_NOTE,
                    STATUS = analyse.STATUS.NAME,
                    YEAR_OF_CONSTRUCTION = analyse.YEAR_OF_CONSTRUCTION
                };
                var userCache = _userRepository.GetById(analyse.USER_ACCOUNT_ID);
                analyseCache.FIRST_NAME = userCache.FIRST_NAME;
                analyseCache.LAST_NAME = userCache.LAST_NAME;
                list.Add(analyseCache);
            }
            return list;
        }

        public List<ANALYSE_REQUEST> GetRequestsByUserId(int ID)
        {
            return _analyseRequestRepository.GetAll().Where(T => T.USER_ACCOUNT_ID == ID).ToList();
        }

        public BaseResponse SendNewRequest(ANALYSE_REQUEST request)
        {
            BaseResponse response = new BaseResponse();
            if (_analyseRequestRepository.Add(request) != null)
            {
                response.Status = true;
                response.Message = "Analiz talebi gönderildi.";
            }
            else
            {
                response.Message = "Analiz talebi gönderilemedi.";
            }
            _unitOfWork.Commit();
            return response;

        }
        public List<ANALYSE_REQUEST> GetPendingRequests()
        {
            return _analyseRequestRepository.GetAll().Where(p => p.STATUS.STATUS_CODE == "pendingconf").ToList();
        }
        public void ConfirmPendingRequest(DataAccess.ANALYSE_REQUEST request)
        {
            ANALYSE_REQUEST current = _analyseRequestRepository.GetById(request.ANALYSIS_REQUEST_ID);
            current.STATUS_ID = _statusRepository.GetByCode("accepted").STATUS_ID;
            _unitOfWork.Commit();
        }
        public bool DenyRequests(List<ANALYSE_REQUEST> requests)
        {
            try
            {
                foreach (ANALYSE_REQUEST analyse in requests)
                {
                    analyse.STATUS_ID = _statusRepository.GetByCode("denied").STATUS_ID;
                    _unitOfWork.Commit();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AllowRequests(List<ANALYSE_REQUEST> requests)
        {
            try
            {
                foreach (ANALYSE_REQUEST analyse in requests)
                {
                    analyse.STATUS_ID = _statusRepository.GetByCode("accepted").STATUS_ID;
                    _unitOfWork.Commit();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
