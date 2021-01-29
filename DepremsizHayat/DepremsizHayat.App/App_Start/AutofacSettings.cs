using Autofac;
using Autofac.Extras.Quartz;
using Autofac.Integration.Mvc;
using Autofac.Integration.Web;
using DepremsizHayat.Business.Factory;
using DepremsizHayat.Business.IService;
using DepremsizHayat.Business.Service;
using DepremsizHayat.Business.UnitOfWork;
using DepremsizHayat.Job;
using DepremsizHayat.Security;
using DepremsizHayat.Utility;
using Quartz;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace DepremsizHayat.App.App_Start
{
    public class AutofacSettings
    {
        public static void Run()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<TestJob>().InstancePerLifetimeScope();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(Assembly.Load("DepremsizHayat.Business")).Where(p => p.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(Assembly.Load("DepremsizHayat.Business")).Where(p => p.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();
            RegisterScheduler(builder);
            IContainer container = builder.Build();
            ConfigureScheduler(container);
            AutofacDependencyResolver resolver = new AutofacDependencyResolver(container);
            DependencyResolver.SetResolver(resolver);
        }

        private static void RegisterScheduler(ContainerBuilder builder)
        {
            var schedulerConfig = new NameValueCollection {
              {"quartz.threadPool.threadCount", "3"},

             };
            builder.RegisterModule(new QuartzAutofacFactoryModule
            {
                ConfigurationProvider = c => schedulerConfig
            });
            builder.RegisterModule(new QuartzAutofacJobsModule(typeof(TestJob).Assembly));
            builder.RegisterType<TestJobScheduler>().AsSelf();
        }
        private static void ConfigureScheduler(IContainer container)
        {
            var scheduler = container.Resolve<TestJobScheduler>();
            scheduler.Start();
        }
    }
}