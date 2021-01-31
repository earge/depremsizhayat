using DepremsizHayat.Business.IService;
using DepremsizHayat.Business.IServiceRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.Business.Service
{
    public class StatusService : IStatusService
    {
        private IStatusRepository _statusRepository;
        public StatusService(IStatusRepository statusRepository)
        {
            this._statusRepository = statusRepository;
        }
        public int GetIdByName(string name)
        {
            return _statusRepository.GetByName(name).STATUS_ID;
        }
    }
}
