using Abp.Application.Services;
using VQM.MultiTenancy.Dto;

namespace VQM.MultiTenancy;

public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
{
}

