using DepremsizHayat.Business.IService;
using DepremsizHayat.Business.IServiceRepository;
using DepremsizHayat.Business.UnitOfWork;
using DepremsizHayat.DataAccess;
using DepremsizHayat.DTO;
using DepremsizHayat.Security;
using System;

namespace DepremsizHayat.Business.Service
{
    public class AnalyseRequestAnswerService : IAnalyseRequestAnswerService
    {
        IAnalyseRequestAnswerRepository _analyseRequestAnswerRepository;
        IAnalyseRequestRepository _analyseRequestRepository;
        IUserAnalyseRequestRepository _userAnalyseRequestRepository;
        IStatusRepository _statusRepository;
        IMailRepository _mailRepository;
        IUserRepository _userRepository;
        IUnitOfWork _unitOfWork;
        public AnalyseRequestAnswerService(IAnalyseRequestAnswerRepository analyseRequestAnswerRepository,
            IAnalyseRequestRepository analyseRequestRepository,
            IUserAnalyseRequestRepository userAnalyseRequestRepository,
            IStatusRepository statusRepository,
            IMailRepository mailRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            this._analyseRequestAnswerRepository = analyseRequestAnswerRepository;
            this._analyseRequestRepository = analyseRequestRepository;
            this._userAnalyseRequestRepository = userAnalyseRequestRepository;
            this._statusRepository = statusRepository;
            this._mailRepository = mailRepository;
            this._userRepository = userRepository;
            this._unitOfWork = unitOfWork;
        }
        public BaseResponse ReplyRequest(string requestId, string answer, int score)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                int reqId = Decryptor.DecryptInt(requestId);
                USER_ANALYSE_REQUEST expertsRequest = _userAnalyseRequestRepository.GetById(reqId);
                ANALYSE_REQUEST request = _analyseRequestRepository.GetById((int)expertsRequest.ANALYSE_REQUEST_ID);
                USER_ACCOUNT requester = _userRepository.GetById(request.USER_ACCOUNT_ID);
                ANALYSE_REQUEST_ANSWER reply = new ANALYSE_REQUEST_ANSWER()
                {
                    ANALYSIS_REQUEST_ID = request.ANALYSIS_REQUEST_ID,
                    COMPLETED = true,
                    CREATED_DATE = DateTime.Now,
                    DELETED = false,
                    DETAIL = answer,
                    RISK_SCORE = score,
                    USER_ACCOUNT_ID = (int)expertsRequest.USER_ACCOUNT_ID
                };
                _analyseRequestAnswerRepository.Add(reply);
                request.STATUS_ID = _statusRepository.GetByCode(Resources.AnalyseRequestStatusCodes.Completed).STATUS_ID;
                expertsRequest.USER_ANALYSE_REQ_STATUS_CODE = Resources.UserAnalyseRequestStatusCodes.Completed;
                _unitOfWork.Commit();
                response.Status = true;
                response.Message.Add("Talep yanıtı başarıyla gönderildi.");
                var subject = "Analiz talebiniz yanıtlandı."; //DÜZENLENECEK
                var body = "Analiz talebiniz yanıtlandı."; //DÜZENLENECEK
                _mailRepository.SendMail("app",requester.E_MAIL,subject,body);
            }
            catch (Exception)
            {
                response.Message.Add("Talep yanıtı gönderilemedi!");
            }
            return response;
        }
    }
}
