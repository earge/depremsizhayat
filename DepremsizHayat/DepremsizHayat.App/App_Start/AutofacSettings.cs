using Autofac;
using Autofac.Extras.Quartz;
using Autofac.Integration.Mvc;
using DepremsizHayat.Business.Factory;
using DepremsizHayat.Business.UnitOfWork;
using DepremsizHayat.Job.Job;
using DepremsizHayat.Job.Scheduler;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace DepremsizHayat.App.App_Start
{
    public class AutofacSettings
    {
        public static void Run()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<AnalyseRequestJob>().InstancePerLifetimeScope();
            builder.RegisterType<UserAnalyseRequestJob>().InstancePerLifetimeScope();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(Assembly.Load("DepremsizHayat.Business")).Where(p => p.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(Assembly.Load("DepremsizHayat.Business")).Where(p => p.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();
            RegisterSchedulers(builder);
            IContainer container = builder.Build();
            ConfigureSchedulers(container);
            AutofacDependencyResolver resolver = new AutofacDependencyResolver(container);
            DependencyResolver.SetResolver(resolver);
        }

        private static void RegisterSchedulers(ContainerBuilder builder)
        {
            var schedulerConfig = new NameValueCollection {
              {"quartz.threadPool.threadCount", "3"},

             };
            builder.RegisterModule(new QuartzAutofacFactoryModule
            {
                ConfigurationProvider = c => schedulerConfig
            });
            builder.RegisterModule(new QuartzAutofacJobsModule(typeof(AnalyseRequestJob).Assembly));
            builder.RegisterType<AnalyseRequestJobScheduler>().AsSelf();
            builder.RegisterModule(new QuartzAutofacJobsModule(typeof(UserAnalyseRequestJob).Assembly));
            builder.RegisterType<UserAnalyseRequestJobScheduler>().AsSelf();
        }
        private static void ConfigureSchedulers(IContainer container)
        {
            var arjScheduler = container.Resolve<AnalyseRequestJobScheduler>();
            arjScheduler.Start();
            var uarjScheduler = container.Resolve<UserAnalyseRequestJobScheduler>();
            uarjScheduler.Start();
        }
    }
}