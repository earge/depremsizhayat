using DepremsizHayat.Business.BaseRepository;
using DepremsizHayat.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.Business.IServiceRepository
{
    public interface IUserAnalyseRequestRepository:IRepository<USER_ANALYSE_REQUEST>
    {
        List<USER_ANALYSE_REQUEST> GetExpertsActiveRequests(int expertId);
        List<USER_ANALYSE_REQUEST> GetExpertsWaitingRequests(int expertId);
        List<USER_ANALYSE_REQUEST> GetExpertsNotAnsweredRequests(int expertId);
        List<USER_ANALYSE_REQUEST> GetExpertsAnsweredRequests(int expertId);
        List<USER_ANALYSE_REQUEST> GetByAnalyseRequestId(int analyseRequestId);
        void AddToQueue(ANALYSE_REQUEST analyse);
        void OfferAssignment(int analyseRequestId, USER_ACCOUNT expert, USER_ANALYSE_REQUEST queue);
        USER_ACCOUNT GetRandomExpertForAnalyse(int? alreadyAssigned);
    }
}
