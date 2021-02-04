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
    public interface IAnalyseRequestService
    {
        List<DataAccess.ANALYSE_REQUEST> GetPendingRequests();
        List<AnalyseRequest> GetAllRequests();
        List<DataAccess.ANALYSE_REQUEST> GetRequestsByUserId(int ID);
        BaseResponse SendNewRequest(DataAccess.ANALYSE_REQUEST request);
        void ConfirmPendingRequest(DataAccess.ANALYSE_REQUEST request);
        bool DenyRequests(List<ANALYSE_REQUEST> requests);
        bool AllowRequests(List<ANALYSE_REQUEST> requests);
    }
}
