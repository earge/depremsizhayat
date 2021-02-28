using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using DepremsizHayat.Business.Factory;
using DepremsizHayat.Business.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace DepremsizHayat.Api.App_Start
{
    public class AutofacApiSettings
    {
        public static void Run()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(Assembly.Load("DepremsizHayat.Business")).Where(p => p.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(Assembly.Load("DepremsizHayat.Business")).Where(p => p.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            IContainer container = builder.Build();
            //AutofacDependencyResolver resolver = new AutofacDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            //DependencyResolver.SetResolver(resolver);
        } 
    }
}