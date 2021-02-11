using DepremsizHayat.Business.IService;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.Job.Job
{
    public class AnalyseRequestJob : IJob
    {
        private IAnalyseRequestService _analyseRequestService;
        public AnalyseRequestJob(IAnalyseRequestService analyseRequestService)
        {
            this._analyseRequestService = analyseRequestService;
        }
        public Task Execute(IJobExecutionContext context)
        {
            foreach (DataAccess.ANALYSE_REQUEST request in _analyseRequestService.GetPendingRequests())
            {
                if (DateTime.Now>request.CREATED_DATE.AddDays(1))
                {
                    _analyseRequestService.ConfirmPendingRequest(request);
                }
            }
            return Task.CompletedTask;
        }
    }
}
