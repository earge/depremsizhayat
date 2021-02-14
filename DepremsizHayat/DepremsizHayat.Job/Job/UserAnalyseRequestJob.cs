using DepremsizHayat.Business.IService;
using DepremsizHayat.DataAccess;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.Job.Job
{
    public class UserAnalyseRequestJob : IJob
    {
        private IUserAnalyseRequestService _userAnalyseRequestService;
        public UserAnalyseRequestJob(IUserAnalyseRequestService userAnalyseRequestService)
        {
            this._userAnalyseRequestService = userAnalyseRequestService;
        }
        public Task Execute(IJobExecutionContext context)
        {
            foreach (USER_ANALYSE_REQUEST waiting in _userAnalyseRequestService.GetWaitingRequests())
            {
                if (DateTime.Now > ((DateTime)waiting.CREATED_DATE).AddDays(1))
                {
                    _userAnalyseRequestService.CancelRequest(waiting.USER_ANALYSE_REQUEST_ID);
                }
            }
            return Task.CompletedTask;
        }
    }
}
