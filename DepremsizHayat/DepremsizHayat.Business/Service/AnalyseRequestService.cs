using DepremsizHayat.Business.IService;
using DepremsizHayat.Business.IServiceRepository;
using DepremsizHayat.DataAccess;
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
        public AnalyseRequestService(IAnalyseRequestRepository analyseRequestRepository,
            IUserRepository userRepository)
        {
            this._analyseRequestRepository = analyseRequestRepository;
            this._userRepository = userRepository;
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
                    STATUS=analyse.STATUS.NAME,
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
    }
}
