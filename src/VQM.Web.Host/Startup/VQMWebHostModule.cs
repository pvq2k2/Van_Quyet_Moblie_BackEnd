using Abp.Modules;
using Abp.Reflection.Extensions;
using VQM.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace VQM.Web.Host.Startup
{
    [DependsOn(
       typeof(VQMWebCoreModule))]
    public class VQMWebHostModule : AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public VQMWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(VQMWebHostModule).GetAssembly());
        }
    }
}
