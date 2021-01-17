using DepremsizHayat.Business.IService;
using DepremsizHayat.Business.IServiceRepository;
using DepremsizHayat.DataAccess;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DepremsizHayat.Job
{
    public class TestJob : IJob
    {
        public IUserService userService { get; set; }
        Task IJob.Execute(IJobExecutionContext context)
        {
            System.Diagnostics.Debug.WriteLine("Triggered at "+DateTime.Now.ToString("HH : mm : ss"));
            return Task.CompletedTask;
        }
    }
}
