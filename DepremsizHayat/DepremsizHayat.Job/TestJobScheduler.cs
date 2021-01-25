using Quartz;
using Quartz.Impl;

namespace DepremsizHayat.Job
{
    public class TestJobScheduler
    {
        private readonly IScheduler _scheduler;
        public TestJobScheduler(IScheduler scheduler)
        {
            this._scheduler = scheduler;
        }
        public void Start()
        {
            _scheduler.Start();
            IJobDetail job = JobBuilder.Create<TestJob>().Build();
            ITrigger trigger = TriggerBuilder.Create().WithSimpleSchedule(
                s =>
                s
                .WithIntervalInSeconds(30).WithRepeatCount(3)
                ).StartNow().Build();
            _scheduler.ScheduleJob(job, trigger);
        }
    }
}
