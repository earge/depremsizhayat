using DepremsizHayat.DataAccess;
using DepremsizHayat.DTO;
using DepremsizHayat.DTO.Admin;
using DepremsizHayat.DTO.User;
using System.Collections.Generic;

namespace DepremsizHayat.Business.IService
{
    public interface IAnalyseRequestService
    {
        List<DataAccess.ANALYSE_REQUEST> GetPendingRequests();
        List<AnalyseRequest> GetAllRequests();
        AnalyseDetailRequest GetRequestDetail(string id);
        List<MyAnalyseRequest> GetRequestsByUserId(int ID);
        SendAnalyseResponse SendNewRequest(DataAccess.ANALYSE_REQUEST request,List<string> paths);
        void ConfirmPendingRequest(DataAccess.ANALYSE_REQUEST request);
        bool DenyRequests(List<string> requests);
        bool AllowRequests(List<string> requests);
        BaseResponse UpdateRequestDetail(AnalyseDetailRequest request);
        MyAnalyseRequest GetRequestByUniqueCode(string code);
    }
}
