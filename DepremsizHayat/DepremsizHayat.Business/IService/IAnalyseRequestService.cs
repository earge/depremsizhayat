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
        List<DataAccess.ANALYSE_REQUEST> GetAllRequests();
    }
}
