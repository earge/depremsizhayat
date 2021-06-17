using DepremsizHayat.Business.IService;
using DepremsizHayat.Business.IServiceRepository;
using DepremsizHayat.Business.UnitOfWork;
using DepremsizHayat.DataAccess;
using DepremsizHayat.DTO;
using DepremsizHayat.DTO.Admin;
using DepremsizHayat.Resources;
using DepremsizHayat.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using DepremsizHayat.DTO.User;

namespace DepremsizHayat.Business.Service
{
    public class AnalyseRequestService : IAnalyseRequestService
    {
        private IAnalyseRequestRepository _analyseRequestRepository;
        private IUserRepository _userRepository;
        private IStatusRepository _statusRepository;
        private IAnalyseRequestAnswerRepository _analyseRequestAnswerRepository;
        private IUserAnalyseRequestRepository _userAnalyseRequestRepository;
        private IUnitOfWork _unitOfWork;
        private IMailRepository _mailRepository;
        private IFileRepository _fileRepository;
        public AnalyseRequestService(IAnalyseRequestRepository analyseRequestRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IStatusRepository statusRepository,
            IAnalyseRequestAnswerRepository analyseRequestAnswerRepository,
            IMailRepository mailRepository,
            IUserAnalyseRequestRepository userAnalyseRequestRepository,
            IFileRepository fileRepository)
        {
            this._analyseRequestRepository = analyseRequestRepository;
            this._userRepository = userRepository;
            this._statusRepository = statusRepository;
            this._analyseRequestAnswerRepository = analyseRequestAnswerRepository;
            this._unitOfWork = unitOfWork;
            this._mailRepository = mailRepository;
            this._userAnalyseRequestRepository = userAnalyseRequestRepository;
            this._fileRepository = fileRepository;
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
                    YEAR_OF_CONSTRUCTION = analyse.YEAR_OF_CONSTRUCTION,
                    CREATED_DATE = analyse.CREATED_DATE
                    
                };
                var userCache = _userRepository.GetById(analyse.USER_ACCOUNT_ID);
                analyseCache.FIRST_NAME = userCache.FIRST_NAME;
                analyseCache.LAST_NAME = userCache.LAST_NAME;
                list.Add(analyseCache);
            }
            return list;
        }
        public List<MyAnalyseRequest> GetRequestsByUserId(int ID)
        {
            var request = new List<MyAnalyseRequest>();
            foreach (ANALYSE_REQUEST analyse in _analyseRequestRepository.GetByUserIdDescendingDate(ID))
            {
                var answer =
                    (analyse.STATUS.STATUS_CODE == AnalyseRequestStatusCodes.Completed)
                    ? _analyseRequestAnswerRepository.GetByRequestId(analyse.ANALYSIS_REQUEST_ID)
                    : null;
                request.Add(new MyAnalyseRequest()
                {
                    ANALYSIS_REQUEST_ID = analyse.ANALYSIS_REQUEST_ID.ToString(),
                    COUNTRY = analyse.COUNTRY,
                    DISTRICT = analyse.DISTRICT,
                    NUMBER_OF_FLOORS = analyse.NUMBER_OF_FLOORS,
                    STATUS_ID = Convert.ToString(analyse.STATUS_ID),
                    STATUS_NAME = _statusRepository.GetById(analyse.STATUS.STATUS_ID).NAME,
                    USER_ACCOUNT_ID = analyse.USER_ACCOUNT_ID.ToString(),
                    YEAR_OF_CONSTRUCTION = analyse.YEAR_OF_CONSTRUCTION,
                    ADDRESS = analyse.ADDRESS,
                    STATUS_CODE = analyse.STATUS.STATUS_CODE,
                    COMPLETED = (answer != null) ? answer.COMPLETED : false,
                    RISK_SCORE = answer?.RISK_SCORE
                });
            }
            return request;
        }

        private static Random random = new Random();
        public string GetUniqueCode()
        {
        TryAgain:
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var result = new string(Enumerable.Repeat(chars, 6)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            if (_analyseRequestRepository.IsUniqueCodeExist(result))
                goto TryAgain;
            else
                return result;
        }

        public SendAnalyseResponse SendNewRequest(ANALYSE_REQUEST request, List<string> paths)
        {
            SendAnalyseResponse response = new SendAnalyseResponse();
            request.UNIQUE_KEY = GetUniqueCode();
            var added = _analyseRequestRepository.Add(request);
            if (added != null)
            {
                _unitOfWork.Commit();
                if (paths != null)
                {
                    foreach (var path in paths)
                    {
                        _fileRepository.Add(new FILE()
                        {
                            CREATED_DATE = DateTime.Now,
                            DELETED = false,
                            ENTITY_ID = added.ANALYSIS_REQUEST_ID,
                            ENTITY_TYPE_CODE = "IMAGE",
                            FILE_TYPE_ID = 1,
                            URL = path
                        });
                    }
                }
                var requester = _userRepository.GetById(request.USER_ACCOUNT_ID);
                var template = _mailRepository.GetTemplate(2);
                var body = template.Replace("%BODY%", request.CREATED_DATE.ToShortDateString() + " tarihli yeni bir analiz talebi var. Talep sahibi: " + requester.FIRST_NAME + " " + requester.LAST_NAME + "(" + requester.E_MAIL + ") Talep ID: " + request.UNIQUE_KEY);
                foreach (USER_ACCOUNT admin in _userRepository.GetAdmins())
                {
                    _mailRepository.SendMail("app", admin.E_MAIL, "Bir yeni analiz talebi var", body);
                }
                body = template.Replace("%BODY%", "Analiz talebiniz gönderildi. Talep takip kodu: " + request.UNIQUE_KEY);
                _mailRepository.SendMail("app", requester.E_MAIL, "Analiz talebiniz gönderildi", body);
                response.Status = true;
                response.Message.Add("Analiz talebi gönderildi.");
                response.Code = request.UNIQUE_KEY;
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
            return _analyseRequestRepository
                .GetAll()
                .Where(p => p.STATUS.STATUS_CODE == AnalyseRequestStatusCodes.WaitingAdminConfirmation)
                .ToList();
        }
        public void ConfirmPendingRequest(ANALYSE_REQUEST request)
        {
            ANALYSE_REQUEST current = _analyseRequestRepository.GetById(request.ANALYSIS_REQUEST_ID);
            current.STATUS_ID = _statusRepository.GetByCode(AnalyseRequestStatusCodes.WaitingExpertConfirmation).STATUS_ID;
            _analyseRequestRepository.Update(current);
            var expert = _userAnalyseRequestRepository.GetRandomExpertForAnalyse(null);
            if (expert != null)
            {
                _userAnalyseRequestRepository.OfferAssignment(request.ANALYSIS_REQUEST_ID, expert, null);
                var requester = _userRepository.GetById(request.USER_ACCOUNT_ID);
                string body = requester.FIRST_NAME + " " + requester.LAST_NAME + " tarafından gönderilmiş yeni bir talebiniz var. Onaylamak ya da reddetmek için tıklayın:" + "Bir yanıt vermemeniz durumunda talep 24 saat içinde otomatik olarak başka bir uzmana atanacaktır.";
                _mailRepository.SendMail("app", expert.E_MAIL, "Bir yeni analiz talebiniz var.", body);
            }
            else
            {
                _userAnalyseRequestRepository.AddToQueue(request);
            }
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
                    analyse.STATUS_ID = _statusRepository.GetByCode(AnalyseRequestStatusCodes.Denied).STATUS_ID;
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
                    ConfirmPendingRequest(analyse);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public AnalyseDetailRequest GetRequestDetail(string id)
        {
            int decryptedId = Decryptor.DecryptInt(id);
            ANALYSE_REQUEST thatRequest = _analyseRequestRepository.GetById(decryptedId);
            var answer = _analyseRequestAnswerRepository.GetByRequestId(thatRequest.ANALYSIS_REQUEST_ID) != null
                ? _analyseRequestAnswerRepository.GetByRequestId(thatRequest.ANALYSIS_REQUEST_ID)
                : null;
            AnalyseDetailRequest detail = new AnalyseDetailRequest()
            {
                ANALYSIS_REQUEST_ID = Convert.ToString(thatRequest.ANALYSIS_REQUEST_ID),
                ADDRESS = thatRequest.ADDRESS,
                CREATED_DATE = thatRequest.CREATED_DATE,
                PHONE_NUMBER_1 = thatRequest.PHONE_NUMBER_1,
                PHONE_NUMBER_2 = thatRequest.PHONE_NUMBER_2,
                USER_NOTE = thatRequest.USER_NOTE,
                ANSWER = answer?.DETAIL,
                RISK_SCORE = answer?.RISK_SCORE,
                IMAGES =  _fileRepository.GetByEntityId(thatRequest.ANALYSIS_REQUEST_ID).Select(p=>p.URL).ToList()
            };
            return detail;
        }
        public BaseResponse UpdateRequestDetail(AnalyseDetailRequest request)
        {
            BaseResponse response = new BaseResponse();
            ANALYSE_REQUEST analyse = _analyseRequestRepository.GetById(Decryptor.DecryptInt(request.ANALYSIS_REQUEST_ID));
            if (request.ADDRESS == analyse.ADDRESS)
            {
                response.Message.Add("Adresler aynı.");
            }
            else
            {
                response.Message.Add("Adres güncellendi.");
                analyse.ADDRESS = request.ADDRESS;
            }
            if (request.PHONE_NUMBER_1 == analyse.PHONE_NUMBER_1)
            {
                response.Message.Add("Birincil telefon numaraları aynı.");
            }
            else
            {
                response.Message.Add("Birincil telefon numarası güncellendi.");
                analyse.PHONE_NUMBER_1 = request.PHONE_NUMBER_1;
            }
            if (request.PHONE_NUMBER_2 == analyse.PHONE_NUMBER_2)
            {
                response.Message.Add("İkincil telefon numaraları aynı.");
            }
            else
            {
                response.Message.Add("İkincil telefon numarası güncellendi.");
                analyse.PHONE_NUMBER_2 = request.PHONE_NUMBER_2;
            }
            if (request.USER_NOTE == analyse.USER_NOTE)
            {
                response.Message.Add("Açıklamalar aynı.");
            }
            else
            {
                response.Message.Add("Açıklama güncellendi.");
                analyse.USER_NOTE = request.USER_NOTE;
            }
            response.Status = (response.Message.Count == 0) ? true : false;
            _unitOfWork.Commit();
            return response;
        }
        public MyAnalyseRequest GetRequestByUniqueCode(string code)
        {
            var dbRecord = _analyseRequestRepository.GetByUniqueCode(code);
            var answer = _analyseRequestAnswerRepository.GetByRequestId(dbRecord.ANALYSIS_REQUEST_ID);
            MyAnalyseRequest request = new MyAnalyseRequest()
            {
                ADDRESS = dbRecord.ADDRESS,
                ANALYSIS_REQUEST_ID = Convert.ToString(dbRecord.ANALYSIS_REQUEST_ID),
                COUNTRY = dbRecord.COUNTRY,
                DISTRICT = dbRecord.DISTRICT,
                NUMBER_OF_FLOORS = dbRecord.NUMBER_OF_FLOORS,
                STATUS_CODE = dbRecord.STATUS.STATUS_CODE,
                USER_ACCOUNT_ID = Convert.ToString(dbRecord.USER_ACCOUNT_ID),
                YEAR_OF_CONSTRUCTION = dbRecord.YEAR_OF_CONSTRUCTION,
                ANSWER = (answer != null) ? answer.DETAIL : null,
                RISK_SCORE = (answer != null) ? (int?)answer.RISK_SCORE : null,
                STATUS_ID = (dbRecord != null && dbRecord.STATUS != null) ? dbRecord.STATUS_ID.ToString() : ""
            };
            return request;
        }
    }
}
