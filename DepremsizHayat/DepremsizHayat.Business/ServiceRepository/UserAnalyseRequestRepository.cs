using DepremsizHayat.Business.BaseRepository;
using DepremsizHayat.Business.Factory;
using DepremsizHayat.Business.IServiceRepository;
using DepremsizHayat.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using DepremsizHayat.Resources;
namespace DepremsizHayat.Business.ServiceRepository
{
    public class UserAnalyseRequestRepository : Repository<USER_ANALYSE_REQUEST>, IUserAnalyseRequestRepository
    {
        public UserAnalyseRequestRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
        public List<USER_ANALYSE_REQUEST> GetExpertsActiveRequests(int expertId)
        {
            return _dbContext.USER_ANALYSE_REQUEST
                .Where(p =>
                p.USER_ACCOUNT_ID == expertId &&
                (
                p.USER_ANALYSE_REQ_STATUS_CODE == AnalyseRequestStatusCodes.Accepted ||
                p.USER_ANALYSE_REQ_STATUS_CODE == AnalyseRequestStatusCodes.Waiting)
                )
                .ToList();
        }
        public List<USER_ANALYSE_REQUEST> GetExpertsWaitingRequests(int expertId)
        {
            return _dbContext.USER_ANALYSE_REQUEST
                .Where(p =>
                p.USER_ACCOUNT_ID == expertId &&
                (p.USER_ANALYSE_REQ_STATUS_CODE == AnalyseRequestStatusCodes.Waiting))
                .ToList();
        }
        public List<USER_ANALYSE_REQUEST> GetExpertsNotAnsweredRequests(int expertId)
        {
            return _dbContext.USER_ANALYSE_REQUEST
                .Where(p =>
                p.USER_ACCOUNT_ID == expertId &&
                (p.USER_ANALYSE_REQ_STATUS_CODE == AnalyseRequestStatusCodes.Accepted))
                .ToList();
        }

        public List<USER_ANALYSE_REQUEST> GetByAnalyseRequestId(int analyseRequestId)
        {
            return _dbContext.USER_ANALYSE_REQUEST
                .Where(p => p.ANALYSE_REQUEST_ID == analyseRequestId)
                .ToList();
        }
    }
}
