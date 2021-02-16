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
        private IUserService _userService;
        public UserAnalyseRequestJob(IUserAnalyseRequestService userAnalyseRequestService,
            IUserService userService)
        {
            this._userAnalyseRequestService = userAnalyseRequestService;
            this._userService = userService;
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
            foreach (USER_ANALYSE_REQUEST accepted in _userAnalyseRequestService.GetAcceptedRequests())
            {
                if (DateTime.Now > ((DateTime)accepted.CREATED_DATE).AddDays(2))
                {
                    _userAnalyseRequestService.CancelRequest(accepted.USER_ANALYSE_REQUEST_ID);
                }
            }
            foreach (USER_ANALYSE_REQUEST queue in _userAnalyseRequestService.GetAtQueue())
            {
                var expert = _userService.GetRandomExpertForAnalyse(null);
                if (expert != null)
                {
                    _userAnalyseRequestService.OfferAssignment((int)queue.ANALYSE_REQUEST_ID, expert, queue);
                    //_userAnalyseRequestService.UpdateQueue(queue.USER_ANALYSE_REQUEST_ID);
                }
            }
            return Task.CompletedTask;
        }
    }
}
