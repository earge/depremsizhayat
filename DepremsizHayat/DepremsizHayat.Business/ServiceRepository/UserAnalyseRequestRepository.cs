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
                p.USER_ANALYSE_REQ_STATUS_CODE == UserAnalyseRequestStatusCodes.Accepted ||
                p.USER_ANALYSE_REQ_STATUS_CODE == UserAnalyseRequestStatusCodes.Waiting)
                )
                .ToList();
        }
        public List<USER_ANALYSE_REQUEST> GetExpertsWaitingRequests(int expertId)
        {
            return _dbContext.USER_ANALYSE_REQUEST
                .Where(p =>
                p.USER_ACCOUNT_ID == expertId &&
                (p.USER_ANALYSE_REQ_STATUS_CODE == UserAnalyseRequestStatusCodes.Waiting))
                .ToList();
        }
        public List<USER_ANALYSE_REQUEST> GetExpertsNotAnsweredRequests(int expertId)
        {
            return _dbContext.USER_ANALYSE_REQUEST
                .Where(p =>
                p.USER_ACCOUNT_ID == expertId &&
                (p.USER_ANALYSE_REQ_STATUS_CODE == UserAnalyseRequestStatusCodes.Accepted))
                .ToList();
        }
        public List<USER_ANALYSE_REQUEST> GetByAnalyseRequestId(int analyseRequestId)
        {
            return _dbContext.USER_ANALYSE_REQUEST
                .Where(p => p.ANALYSE_REQUEST_ID == analyseRequestId)
                .ToList();
        }
        public void AddToQueue(ANALYSE_REQUEST analyse)
        {
            USER_ANALYSE_REQUEST queue = new USER_ANALYSE_REQUEST()
            {
                ANALYSE_REQUEST_ID = analyse.ANALYSIS_REQUEST_ID,
                CREATED_DATE = DateTime.Now,
                DELETED = false,
                ACTIVE = true,
                USER_ANALYSE_REQ_STATUS_CODE = UserAnalyseRequestStatusCodes.Queue
            };
            Add(queue);
        }
        public void OfferAssignment(int analyseRequestId, USER_ACCOUNT expert, USER_ANALYSE_REQUEST queue)
        {
            var oldExpertRecord = GetByAnalyseRequestId(analyseRequestId)
                .FirstOrDefault(p => p.USER_ACCOUNT_ID == expert.USER_ACCOUNT_ID);
            var oldExpert = (oldExpertRecord != null) ? _dbContext.USER_ACCOUNT.Find(expert.USER_ACCOUNT_ID) : null;
            if (oldExpert!=null)
            {
                do
                {
                    expert = GetRandomExpertForAnalyse(expert.USER_ACCOUNT_ID);
                } while (oldExpert == expert);
            }
            if (queue == null)
            {
                USER_ANALYSE_REQUEST requestAssignmentOffer = new USER_ANALYSE_REQUEST()
                {
                    ANALYSE_REQUEST_ID = analyseRequestId,
                    CREATED_DATE = DateTime.Now,
                    DELETED = false,
                    ACTIVE = true,
                    USER_ANALYSE_REQ_STATUS_CODE = UserAnalyseRequestStatusCodes.Waiting,
                    USER_ACCOUNT_ID = expert.USER_ACCOUNT_ID
                };
                Add(requestAssignmentOffer);
            }
            else
            {
                var updating = GetById(queue.USER_ANALYSE_REQUEST_ID);
                updating.USER_ANALYSE_REQ_STATUS_CODE = UserAnalyseRequestStatusCodes.Waiting;
                updating.USER_ACCOUNT_ID = expert.USER_ACCOUNT_ID;
                Update(updating);
            }
            _dbContext.SaveChanges();
        }
        public USER_ACCOUNT GetRandomExpertForAnalyse(int? alreadyAssigned)
        {
            Random random = new Random();
            int id;
            int roleId = _dbContext.ROLE.FirstOrDefault(p => p.NAME == "Expert").ROLE_ID;
            List<USER_ACCOUNT> list = _dbContext.USER_ACCOUNT
                .Where(p => p.ROLE_ID == roleId)
                //.OrderBy(p=>p.LAST_ANSWER_DATE)
                .ToList();

            var availableCount = list.Sum(p => p.MAX_COUNT_REQUEST);
            var busyCount =
                GetAll()
                .Where(p =>
                p.USER_ANALYSE_REQ_STATUS_CODE == Resources.UserAnalyseRequestStatusCodes.Accepted ||
                p.USER_ANALYSE_REQ_STATUS_CODE == Resources.UserAnalyseRequestStatusCodes.Waiting)
                .Count();
        FindAvailable:
            USER_ACCOUNT expert = null;
            do
            {
                id = random.Next(0, list.Count);
                expert = _dbContext.USER_ACCOUNT.Find(list[id].USER_ACCOUNT_ID);
            } while (expert == null);

            var maxAnswerCount = expert.MAX_COUNT_REQUEST;
            int totalAssignmentsCount = GetExpertsActiveRequests(expert.USER_ACCOUNT_ID).Count();

            if (busyCount < availableCount)
            {
                if (totalAssignmentsCount < expert.MAX_COUNT_REQUEST)
                {
                    return expert;
                }
                else
                {
                    goto FindAvailable;
                }
            }
            return null;
        }
    }
}
