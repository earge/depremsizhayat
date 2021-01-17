using Quartz;
using Quartz.Impl;

namespace DepremsizHayat.Job
{
    public class TestJobScheduler
    {
        private IScheduler Start()
        {
            ISchedulerFactory schedulerFact = new StdSchedulerFactory();
            IScheduler scheduler = schedulerFact.GetScheduler().GetAwaiter().GetResult();
            scheduler.Start();
            return scheduler;
        }
        public void Trig()
        {
            IScheduler scheduler = Start();
            IJobDetail job = JobBuilder.Create<TestJob>().Build();
            ITrigger trigger = TriggerBuilder.Create().WithSimpleSchedule(
                s =>
                s
                .WithIntervalInSeconds(6).WithRepeatCount(5)
                ).StartNow().Build();
            scheduler.ScheduleJob(job, trigger);

        }
    }
}
