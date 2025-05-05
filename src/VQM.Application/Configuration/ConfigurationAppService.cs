using Abp.Authorization;
using Abp.Runtime.Session;
using VQM.Configuration.Dto;
using System.Threading.Tasks;

namespace VQM.Configuration;

[AbpAuthorize]
public class ConfigurationAppService : VQMAppServiceBase, IConfigurationAppService
{
    public async Task ChangeUiTheme(ChangeUiThemeInput input)
    {
        await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
    }
}
