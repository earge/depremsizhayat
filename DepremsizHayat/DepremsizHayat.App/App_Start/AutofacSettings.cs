using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.Web;
using DepremsizHayat.Business.Factory;
using DepremsizHayat.Business.IService;
using DepremsizHayat.Business.UnitOfWork;
using DepremsizHayat.Security;
using System;
using System.Collections.Generic;
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
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerRequest();
            builder.RegisterAssemblyTypes(Assembly.Load("DepremsizHayat.Business")).Where(p => p.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(Assembly.Load("DepremsizHayat.Business")).Where(p => p.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerRequest();
            IContainer container = builder.Build();
            builder.Register(c => new UserRoleProvider { _userService = c.Resolve<IUserService>() });
            builder.Register(c => new UserRoleProvider { _roleService = c.Resolve<IRoleService>() });
            AutofacDependencyResolver resolver = new AutofacDependencyResolver(container);
            DependencyResolver.SetResolver(resolver);
        }
    }
}