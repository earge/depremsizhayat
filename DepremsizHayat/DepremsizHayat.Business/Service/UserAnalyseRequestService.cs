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
    public class UserAnalyseRequestService : IUserAnalyseRequestService
    {
        private IUnitOfWork _unitOfWork;
        private IUserAnalyseRequestRepository _userAnalyseRequestRepository;
        private IUserRepository _userRepository;
        private IAnalyseRequestRepository _analyseRequestRepository;
        public UserAnalyseRequestService(IUnitOfWork unitOfWork,
            IUserAnalyseRequestRepository userAnalyseRequestRepository,
            IUserRepository userRepository,
            IAnalyseRequestRepository analyseRequestRepository)
        {
            this._unitOfWork = unitOfWork;
            this._userAnalyseRequestRepository = userAnalyseRequestRepository;
            this._userRepository = userRepository;
            this._analyseRequestRepository = analyseRequestRepository;
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
                    REQUESTER_USER = _userRepository.GetById(userRequest.USER_ACCOUNT_ID)
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
                if (assignedRequest.USER_ANALYSE_REQ_STATUS_CODE == Resources.AnalyseRequestStatusCodes.Waiting)
                {
                    assignedRequest.USER_ANALYSE_REQ_STATUS_CODE = type;
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
                p.USER_ANALYSE_REQ_STATUS_CODE == Resources.AnalyseRequestStatusCodes.Waiting
                )
                .ToList();
        }
        public void CancelRequest(int id)
        {
            var waitingRequest = _userAnalyseRequestRepository.GetById(id);
            waitingRequest.USER_ANALYSE_REQ_STATUS_CODE = Resources.AnalyseRequestStatusCodes.Canceled;
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
                 p.USER_ANALYSE_REQ_STATUS_CODE == Resources.AnalyseRequestStatusCodes.Accepted
                 )
                 .ToList();
        }

        public List<USER_ANALYSE_REQUEST> GetAtQueue()
        {
            return _userAnalyseRequestRepository
                .GetAll()
                .Where(p =>
                p.USER_ANALYSE_REQ_STATUS_CODE == Resources.AnalyseRequestStatusCodes.Queue
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
            outOfQueue.USER_ANALYSE_REQ_STATUS_CODE = Resources.AnalyseRequestStatusCodes.Waiting;
            _userAnalyseRequestRepository.Update(outOfQueue);
            _unitOfWork.Commit();
        }
    }
}
