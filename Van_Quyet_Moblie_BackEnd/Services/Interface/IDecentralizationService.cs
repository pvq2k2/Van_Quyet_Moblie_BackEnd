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
        public Task<ResponseObject<DecentralizationDTO>> CreateDecentralization(CreateDecentralizationRequest request);
        public Task<ResponseObject<DecentralizationDTO>> UpdateDecentralization(int decentralizationID, UpdateDecentralizationRequest request);
        public Task<ResponseObject<string>> RemoveDecentralization(int decentralizationID);
    }
}
