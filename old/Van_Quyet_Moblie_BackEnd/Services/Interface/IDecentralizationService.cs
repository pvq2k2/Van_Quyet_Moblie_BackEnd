using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;
using Van_Quyet_Moblie_BackEnd.Handle.Request.DecentralizationRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;

namespace Van_Quyet_Moblie_BackEnd.Services.Interface
{
    public interface IDecentralizationService
    {
        public Task<PageResult<DecentralizationDTO>> GetAllDecentralization(Pagination pagination);
        public Task<ResponseObject<DecentralizationDTO>> GetDecentralizationByID(int decentralizationID);
        public Task<Response> CreateDecentralization(CreateDecentralizationRequest request);
        public Task<Response> UpdateDecentralization(int decentralizationID, UpdateDecentralizationRequest request);
        public Task<Response> RemoveDecentralization(int decentralizationID);
    }
}
