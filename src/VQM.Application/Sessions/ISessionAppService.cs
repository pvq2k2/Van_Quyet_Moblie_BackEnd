using Abp.Application.Services;
using VQM.Sessions.Dto;
using System.Threading.Tasks;

namespace VQM.Sessions;

public interface ISessionAppService : IApplicationService
{
    Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
}
