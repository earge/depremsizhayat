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
        AnalyseDetailRequest GetDetailRequest(string id);
        List<MyAnalyseRequest> GetRequestsByUserId(int ID);
        BaseResponse SendNewRequest(DataAccess.ANALYSE_REQUEST request);
        void ConfirmPendingRequest(DataAccess.ANALYSE_REQUEST request);
        bool DenyRequests(List<string> requests);
        bool AllowRequests(List<string> requests);
        BaseResponse UpdateRequestDetail(AnalyseDetailRequest request);
        MyAnalyseRequest GetRequestByUniqueCode(string code);
    }
}
