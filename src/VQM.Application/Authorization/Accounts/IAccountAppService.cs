using Abp.Application.Services;
using VQM.Authorization.Accounts.Dto;
using System.Threading.Tasks;

namespace VQM.Authorization.Accounts;

public interface IAccountAppService : IApplicationService
{
    Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

    Task<RegisterOutput> Register(RegisterInput input);
}
