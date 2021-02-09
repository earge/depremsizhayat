using Autofac;
using DepremsizHayat.Job.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DepremsizHayat.App.App_Start
{
    public class JobSettings
    {
        public static void StartAnalyseRequestJob()
        {
            var scheduler = new ContainerBuilder().Build().Resolve<AnalyseRequestJobScheduler>();
            scheduler.Start();
        }
    }
}