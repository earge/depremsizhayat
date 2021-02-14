using DepremsizHayat.Job.Job;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.Job.Scheduler
{
    public class UserAnalyseRequestJobScheduler
    {
        private readonly IScheduler _scheduler;
        public UserAnalyseRequestJobScheduler(IScheduler scheduler)
        {
            this._scheduler = scheduler;
        }
        public void Start()
        {
            _scheduler.Start();
            IJobDetail job = JobBuilder.Create<UserAnalyseRequestJob>().Build();
            ITrigger trigger = TriggerBuilder.Create().WithSimpleSchedule(
                s =>
                s
                .WithIntervalInSeconds(5).RepeatForever()
                ).StartNow().Build();
            _scheduler.ScheduleJob(job, trigger);
        }
    }
}
