using DepremsizHayat.Business.IService;
using DepremsizHayat.Business.IServiceRepository;
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
        public AnalyseRequestService(IAnalyseRequestRepository analyseRequestRepository)
        {
            this._analyseRequestRepository = analyseRequestRepository;
        }
        public List<DataAccess.ANALYSE_REQUEST> GetAllRequests()
        {
            return _analyseRequestRepository.GetAll();
            
        }
    }
}
