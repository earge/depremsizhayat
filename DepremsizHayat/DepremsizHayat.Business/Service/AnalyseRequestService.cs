using DepremsizHayat.Business.IService;
using DepremsizHayat.Business.IServiceRepository;
using DepremsizHayat.Business.UnitOfWork;
using DepremsizHayat.DataAccess;
using DepremsizHayat.DTO;
using DepremsizHayat.DTO.Admin;
using DepremsizHayat.Security;
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
        private IAnalyseRequestAnswerRepository _analyseRequestAnswerRepository;
        private IUnitOfWork _unitOfWork;
        public AnalyseRequestService(IAnalyseRequestRepository analyseRequestRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IStatusRepository statusRepository,
            IAnalyseRequestAnswerRepository analyseRequestAnswerRepository)
        {
            this._analyseRequestRepository = analyseRequestRepository;
            this._userRepository = userRepository;
            this._statusRepository = statusRepository;
            this._analyseRequestAnswerRepository = analyseRequestAnswerRepository;
            this._unitOfWork = unitOfWork;
        }
        public List<AnalyseRequest> GetAllRequests()
        {
            List<AnalyseRequest> list = new List<AnalyseRequest>();

            foreach (DataAccess.ANALYSE_REQUEST analyse in _analyseRequestRepository.GetAll())
            {
                var analyseCache = new AnalyseRequest()
                {
                    ANALYSIS_REQUEST_ID = Convert.ToString(analyse.ANALYSIS_REQUEST_ID),
                    COUNTRY = analyse.COUNTRY,
                    DISTRICT = analyse.DISTRICT,
                    NUMBER_OF_FLOORS = analyse.NUMBER_OF_FLOORS,
                    USER_ACCOUNT_ID = Convert.ToString(analyse.USER_ACCOUNT_ID),
                    STATUS_ID = Convert.ToString(analyse.STATUS.STATUS_ID),
                    STATUS_NAME = analyse.STATUS.NAME,
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
                response.Message.Add("Analiz talebi gönderildi.");
            }
            else
            {
                response.Message.Add("Analiz talebi gönderilemedi.");
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
            ANALYSE_REQUEST_ANSWER answerRecord = new ANALYSE_REQUEST_ANSWER()
            {
                CREATED_DATE = DateTime.Now,
                DELETED = false,
                DETAIL = "",
                RISK_SCORE = 0,
                ANALYSIS_REQUEST_ID = request.ANALYSIS_REQUEST_ID,
                USER_ACCOUNT_ID = _userRepository.GetRandomExpertForAnalyse().USER_ACCOUNT_ID
            };
            _analyseRequestAnswerRepository.Add(answerRecord);
            _unitOfWork.Commit();
        }
        public bool DenyRequests(List<string> requests)
        {
            try
            {
                foreach (string id in requests)
                {
                    int dummy = Decryptor.DecryptInt(id);
                    ANALYSE_REQUEST analyse = _analyseRequestRepository.GetById(dummy);
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
        public bool AllowRequests(List<string> requests)
        {
            try
            {
                foreach (string id in requests)
                {
                    int dummy = Decryptor.DecryptInt(id);
                    ANALYSE_REQUEST analyse = _analyseRequestRepository.GetById(dummy);
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
        public AnalyseDetailRequest GetDetailRequest(string id)
        {
            int decryptedId = Decryptor.DecryptInt(id);
            DataAccess.ANALYSE_REQUEST thatRequest = _analyseRequestRepository.GetById(decryptedId);
            AnalyseDetailRequest detail = new AnalyseDetailRequest()
            {
                ANALYSIS_REQUEST_ID=Convert.ToString(thatRequest.ANALYSIS_REQUEST_ID),
                ADDRESS=thatRequest.ADDRESS,
                CREATED_DATE=thatRequest.CREATED_DATE,
                PHONE_NUMBER_1=thatRequest.PHONE_NUMBER_1,
                PHONE_NUMBER_2=thatRequest.PHONE_NUMBER_2,
                USER_NOTE=thatRequest.USER_NOTE
            };
            return detail;
        }
        public BaseResponse UpdateRequestDetail(AnalyseDetailRequest request)
        {
            BaseResponse response = new BaseResponse();
            ANALYSE_REQUEST analyse = _analyseRequestRepository.GetById(Decryptor.DecryptInt(request.ANALYSIS_REQUEST_ID));
            if (request.ADDRESS==analyse.ADDRESS)
            {
                response.Message.Add("Adresler aynı.");
            }
            else
            {
                analyse.ADDRESS = request.ADDRESS;
            }
            if (request.PHONE_NUMBER_1 == analyse.PHONE_NUMBER_1)
            {
                response.Message.Add("Birincil telefon numaraları aynı.");
            }
            else
            {
                analyse.PHONE_NUMBER_1 = request.PHONE_NUMBER_1;
            }
            if (request.PHONE_NUMBER_2 == analyse.PHONE_NUMBER_2)
            {
                response.Message.Add("İkincil telefon numaraları aynı.");
            }
            else
            {
                analyse.PHONE_NUMBER_2 = request.PHONE_NUMBER_2;
            }
            if (request.USER_NOTE == analyse.USER_NOTE)
            {
                response.Message.Add("Açıklamalar aynı.");
            }
            else
            {
                analyse.USER_NOTE = request.USER_NOTE;
            }
            response.Status = (response.Message.Count == 0) ? true : false;
            _unitOfWork.Commit();
            return response;
        }
    }
}
