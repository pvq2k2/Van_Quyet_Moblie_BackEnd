using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using VQM.Authorization;

namespace VQM;

[DependsOn(
    typeof(VQMCoreModule),
    typeof(AbpAutoMapperModule))]
public class VQMApplicationModule : AbpModule
{
    public override void PreInitialize()
    {
        Configuration.Authorization.Providers.Add<VQMAuthorizationProvider>();
    }

    public override void Initialize()
    {
        var thisAssembly = typeof(VQMApplicationModule).GetAssembly();

        IocManager.RegisterAssemblyByConvention(thisAssembly);

        Configuration.Modules.AbpAutoMapper().Configurators.Add(
            // Scan the assembly for classes which inherit from AutoMapper.Profile
            cfg => cfg.AddMaps(thisAssembly)
        );
    }
}
