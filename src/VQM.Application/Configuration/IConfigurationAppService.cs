using VQM.Configuration.Dto;
using System.Threading.Tasks;

namespace VQM.Configuration;

public interface IConfigurationAppService
{
    Task ChangeUiTheme(ChangeUiThemeInput input);
}
