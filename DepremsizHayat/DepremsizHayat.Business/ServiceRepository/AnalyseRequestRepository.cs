using DepremsizHayat.Business.BaseRepository;
using DepremsizHayat.Business.Factory;
using DepremsizHayat.Business.IServiceRepository;
using DepremsizHayat.DataAccess;
using DepremsizHayat.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.Business.ServiceRepository
{
    public class AnalyseRequestRepository : Repository<ANALYSE_REQUEST>, IAnalyseRequestRepository
    {
        private IUserAnalyseRequestRepository _userAnalyseRequestRepository;
        private IUserRepository _userRepository;
        public AnalyseRequestRepository(IDbFactory dbFactory,
            IUserAnalyseRequestRepository userAnalyseRequestRepository,
            IUserRepository userRepository) : base(dbFactory)
        {
            this._userAnalyseRequestRepository = userAnalyseRequestRepository;
            this._userRepository = userRepository;
        }
        public void OfferAssignment(int analyseRequestId, USER_ACCOUNT expert)
        {
            var checkList = _userAnalyseRequestRepository.GetByAnalyseRequestId(analyseRequestId);
            bool checkReOffer = false;
            foreach (USER_ANALYSE_REQUEST analyse in checkList)
            {
                if (analyse.USER_ACCOUNT_ID == expert.USER_ACCOUNT_ID)
                {
                    checkReOffer = true;
                }
            }
            if (checkReOffer)
            {
                expert = _userRepository.GetRandomExpertForAnalyse(expert.USER_ACCOUNT_ID);
            }
            USER_ANALYSE_REQUEST requestAssignmentOffer = new USER_ANALYSE_REQUEST()
            {
                ANALYSE_REQUEST_ID = analyseRequestId,
                CREATED_DATE = DateTime.Now,
                DELETED = false,
                ACTIVE = true,
                USER_ANALYSE_REQ_STATUS_CODE = AnalyseRequestStatusCodes.Waiting,
                USER_ACCOUNT_ID = expert.USER_ACCOUNT_ID
            };
            _userAnalyseRequestRepository.Add(requestAssignmentOffer);
        }
    }
}
