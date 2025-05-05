using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace VQM.Controllers
{
    public abstract class VQMControllerBase : AbpController
    {
        protected VQMControllerBase()
        {
            LocalizationSourceName = VQMConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
