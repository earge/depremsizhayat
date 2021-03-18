using DepremsizHayat.DataAccess;
using DepremsizHayat.DTO;
using DepremsizHayat.DTO.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.Business.IService
{
    public interface IUserAnalyseRequestService
    {
        List<ExpertWaitingAnalyseRequest> ExpertNotConfirmedRequests(int expertId);
        List<ExpertNotAnsweredRequest> ExpertNotAnsweredRequests(int expertId);
        List<ExpertAnsweredRequest> ExpertAnsweredRequests(int expertId);
        BaseResponse ProcessTheRequest(int requestId, string type);
        List<USER_ANALYSE_REQUEST> GetWaitingRequests();
        void CancelRequest(int id);
        List<USER_ANALYSE_REQUEST> GetAcceptedRequests();
        List<USER_ANALYSE_REQUEST> GetAtQueue();
        void OfferAssignment(int analyseRequestId, USER_ACCOUNT expert,USER_ANALYSE_REQUEST queue);
        void UpdateQueue(int id);
    }
}
