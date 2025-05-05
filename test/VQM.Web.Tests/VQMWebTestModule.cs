using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using VQM.EntityFrameworkCore;
using VQM.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace VQM.Web.Tests;

[DependsOn(
    typeof(VQMWebMvcModule),
    typeof(AbpAspNetCoreTestBaseModule)
)]
public class VQMWebTestModule : AbpModule
{
    public VQMWebTestModule(VQMEntityFrameworkModule abpProjectNameEntityFrameworkModule)
    {
        abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
    }

    public override void PreInitialize()
    {
        Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(VQMWebTestModule).GetAssembly());
    }

    public override void PostInitialize()
    {
        IocManager.Resolve<ApplicationPartManager>()
            .AddApplicationPartsIfNotAddedBefore(typeof(VQMWebMvcModule).Assembly);
    }
}