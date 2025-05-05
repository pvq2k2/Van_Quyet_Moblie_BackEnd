using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using VQM.Configuration;
using VQM.EntityFrameworkCore;
using VQM.Migrator.DependencyInjection;
using Castle.MicroKernel.Registration;
using Microsoft.Extensions.Configuration;

namespace VQM.Migrator;

[DependsOn(typeof(VQMEntityFrameworkModule))]
public class VQMMigratorModule : AbpModule
{
    private readonly IConfigurationRoot _appConfiguration;

    public VQMMigratorModule(VQMEntityFrameworkModule abpProjectNameEntityFrameworkModule)
    {
        abpProjectNameEntityFrameworkModule.SkipDbSeed = true;

        _appConfiguration = AppConfigurations.Get(
            typeof(VQMMigratorModule).GetAssembly().GetDirectoryPathOrNull()
        );
    }

    public override void PreInitialize()
    {
        Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
            VQMConsts.ConnectionStringName
        );

        Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        Configuration.ReplaceService(
            typeof(IEventBus),
            () => IocManager.IocContainer.Register(
                Component.For<IEventBus>().Instance(NullEventBus.Instance)
            )
        );
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(VQMMigratorModule).GetAssembly());
        ServiceCollectionRegistrar.Register(IocManager);
    }
}
