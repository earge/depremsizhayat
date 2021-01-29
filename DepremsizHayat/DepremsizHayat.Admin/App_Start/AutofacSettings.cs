using Autofac;
using Autofac.Integration.Mvc;
using DepremsizHayat.Business.Factory;
using DepremsizHayat.Business.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace DepremsizHayat.Admin.App_Start
{
    public class AutofacSettings
    {
        public static void Run()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(Assembly.Load("DepremsizHayat.Business")).Where(p => p.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(Assembly.Load("DepremsizHayat.Business")).Where(p => p.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();
            IContainer container = builder.Build();
            AutofacDependencyResolver resolver = new AutofacDependencyResolver(container);
            DependencyResolver.SetResolver(resolver);
        }
    }
}