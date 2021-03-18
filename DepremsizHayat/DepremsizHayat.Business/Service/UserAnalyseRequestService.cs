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
    public class UserAnalyseRequestService : IUserAnalyseRequestService
    {
        private IUnitOfWork _unitOfWork;
        private IUserAnalyseRequestRepository _userAnalyseRequestRepository;
        private IUserRepository _userRepository;
        private IAnalyseRequestRepository _analyseRequestRepository;
        private IStatusRepository _statusRepository;
        public UserAnalyseRequestService(IUnitOfWork unitOfWork,
            IUserAnalyseRequestRepository userAnalyseRequestRepository,
            IUserRepository userRepository,
            IAnalyseRequestRepository analyseRequestRepository,
            IStatusRepository statusRepository)
        {
            this._unitOfWork = unitOfWork;
            this._userAnalyseRequestRepository = userAnalyseRequestRepository;
            this._userRepository = userRepository;
            this._analyseRequestRepository = analyseRequestRepository;
            this._statusRepository = statusRepository;
        }
        public List<ExpertWaitingAnalyseRequest> ExpertNotConfirmedRequests(int expertId)
        {
            var list = _userAnalyseRequestRepository.GetExpertsWaitingRequests(expertId);
            List<ExpertWaitingAnalyseRequest> request = new List<ExpertWaitingAnalyseRequest>();
            foreach (USER_ANALYSE_REQUEST expert in list)
            {
                var userRequest = _analyseRequestRepository.GetById((int)expert.ANALYSE_REQUEST_ID);
                request.Add(new ExpertWaitingAnalyseRequest()
                {
                    ACTIVE = true,
                    ANALYSE_REQUEST_ID = Convert.ToString(expert.ANALYSE_REQUEST_ID),
                    DELETED = false,
                    EXPERT_USER = _userRepository.GetById((int)expert.USER_ACCOUNT_ID),
                    STATUS = expert.USER_ANALYSE_REQUEST_STATUS,
                    ANALYSE_REQUEST = userRequest,
                    REQUESTER_USER = _userRepository.GetById(userRequest.USER_ACCOUNT_ID),
                    USER_ANALYSE_REQUEST_ID = Convert.ToString(expert.USER_ANALYSE_REQUEST_ID)
                });
            }
            return request;
        }
        public List<ExpertNotAnsweredRequest> ExpertNotAnsweredRequests(int expertId)
        {
            var list = _userAnalyseRequestRepository.GetExpertsNotAnsweredRequests(expertId);
            List<ExpertNotAnsweredRequest> request = new List<ExpertNotAnsweredRequest>();
            foreach (USER_ANALYSE_REQUEST expert in list)
            {
                var userRequest = _analyseRequestRepository.GetById((int)expert.ANALYSE_REQUEST_ID);
                request.Add(new ExpertNotAnsweredRequest()
                {
                    ACTIVE = true,
                    ANALYSE_REQUEST_ID = Convert.ToString(expert.ANALYSE_REQUEST_ID),
                    DELETED = false,
                    EXPERT_USER = _userRepository.GetById((int)expert.USER_ACCOUNT_ID),
                    STATUS = expert.USER_ANALYSE_REQUEST_STATUS,
                    ANALYSE_REQUEST = userRequest,
                    REQUESTER_USER = _userRepository.GetById(userRequest.USER_ACCOUNT_ID),
                    USER_ANALYSE_REQUEST_ID = Encryptor.Encrypt(expert.USER_ANALYSE_REQUEST_ID.ToString())
                });
            }
            return request;
        }
        public BaseResponse ProcessTheRequest(int requestId, string type)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                USER_ANALYSE_REQUEST assignedRequest = _userAnalyseRequestRepository.GetById(requestId);
                if (assignedRequest.USER_ANALYSE_REQ_STATUS_CODE == Resources.UserAnalyseRequestStatusCodes.Waiting)
                {
                    assignedRequest.USER_ANALYSE_REQ_STATUS_CODE = type;

                    var update = _analyseRequestRepository.GetById((int)_userAnalyseRequestRepository.GetById(requestId).ANALYSE_REQUEST_ID);
                    if (type == Resources.UserAnalyseRequestStatusCodes.Accepted)
                    {
                        update.STATUS_ID = _statusRepository.GetByCode(Resources.AnalyseRequestStatusCodes.Sent).STATUS_ID;
                    }
                    else if (type == Resources.UserAnalyseRequestStatusCodes.Denied)
                    {
                        update.STATUS_ID = _statusRepository.GetByCode(Resources.AnalyseRequestStatusCodes.Denied).STATUS_ID;
                    }
                    _analyseRequestRepository.Update(update);
                    _unitOfWork.Commit();
                }
                response.Status = true;
                response.Message.Add("Talep durumu kaydedildi.");
            }
            catch (Exception ex)
            {
                response.Message.Add("Bir hata oluştu. Hata ayrıntısı: " + ex.Data);
            }
            return response;

        }
        public List<USER_ANALYSE_REQUEST> GetWaitingRequests()
        {
            return _userAnalyseRequestRepository
                .GetAll()
                .Where(p =>
                p.USER_ANALYSE_REQ_STATUS_CODE == Resources.UserAnalyseRequestStatusCodes.Waiting
                )
                .ToList();
        }
        public void CancelRequest(int id)
        {
            var waitingRequest = _userAnalyseRequestRepository.GetById(id);
            waitingRequest.USER_ANALYSE_REQ_STATUS_CODE = Resources.UserAnalyseRequestStatusCodes.Canceled;
            _userAnalyseRequestRepository.Update(waitingRequest);
            _userAnalyseRequestRepository.OfferAssignment(
                (int)waitingRequest.ANALYSE_REQUEST_ID,
                _userRepository.GetById((int)waitingRequest.USER_ACCOUNT_ID),
                null
                );
            _unitOfWork.Commit();
        }

        public List<USER_ANALYSE_REQUEST> GetAcceptedRequests()
        {
            return _userAnalyseRequestRepository
                 .GetAll()
                 .Where(p =>
                 p.USER_ANALYSE_REQ_STATUS_CODE == Resources.UserAnalyseRequestStatusCodes.Accepted
                 )
                 .ToList();
        }

        public List<USER_ANALYSE_REQUEST> GetAtQueue()
        {
            return _userAnalyseRequestRepository
                .GetAll()
                .Where(p =>
                p.USER_ANALYSE_REQ_STATUS_CODE == Resources.UserAnalyseRequestStatusCodes.Queue
                )
                .ToList();
        }

        public void OfferAssignment(int analyseRequestId, USER_ACCOUNT expert, USER_ANALYSE_REQUEST queue)
        {
            _userAnalyseRequestRepository.OfferAssignment(analyseRequestId, expert, queue);
        }

        public void UpdateQueue(int id)
        {
            var outOfQueue = _userAnalyseRequestRepository.GetById(id);
            outOfQueue.USER_ANALYSE_REQ_STATUS_CODE = Resources.UserAnalyseRequestStatusCodes.Waiting;
            _userAnalyseRequestRepository.Update(outOfQueue);
            _unitOfWork.Commit();
        }

        public List<ExpertAnsweredRequest> ExpertAnsweredRequests(int expertId)
        {
            var list = _userAnalyseRequestRepository.GetExpertsAnsweredRequests(expertId);
            List<ExpertAnsweredRequest> request = new List<ExpertAnsweredRequest>();
            foreach (USER_ANALYSE_REQUEST expert in list)
            {
                var userRequest = _analyseRequestRepository.GetById((int)expert.ANALYSE_REQUEST_ID);
                request.Add(new ExpertAnsweredRequest()
                {
                    ACTIVE = true,
                    ANALYSE_REQUEST_ID = Convert.ToString(expert.ANALYSE_REQUEST_ID),
                    DELETED = false,
                    EXPERT_USER = _userRepository.GetById((int)expert.USER_ACCOUNT_ID),
                    STATUS = expert.USER_ANALYSE_REQUEST_STATUS,
                    ANALYSE_REQUEST = userRequest,
                    REQUESTER_USER = _userRepository.GetById(userRequest.USER_ACCOUNT_ID),
                    USER_ANALYSE_REQUEST_ID = Encryptor.Encrypt(expert.USER_ANALYSE_REQUEST_ID.ToString())
                });
            }
            return request;
        }
    }
}
