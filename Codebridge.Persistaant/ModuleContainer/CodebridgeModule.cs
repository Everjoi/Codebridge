using AspNetCoreRateLimit;
using Autofac;
using Codebridge.Application.Interfaces.Repository;
using Codebridge.Persistant.Data.Contexts;
using Codebridge.Persistant.Repository;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebridge.Persistant.ModuleContainer
{
    public class CodebridgeModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CodebridgeContext>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
            builder.RegisterType<MemoryCache>().As<IMemoryCache>().InstancePerLifetimeScope();        
        }

    }
}
