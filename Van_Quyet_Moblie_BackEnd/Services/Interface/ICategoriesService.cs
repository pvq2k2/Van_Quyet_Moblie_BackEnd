using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs.Categories;
using Van_Quyet_Moblie_BackEnd.Handle.Request.CategoriesRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;

namespace Van_Quyet_Moblie_BackEnd.Services.Interface
{
    public interface ICategoriesService
    {
        public Task<PageResult<CategoriesDTO>> GetAllCategories(Pagination pagination);
        public Task<object> GetCategoriesToView();
        public Task<Response> CreateCategories(CreateCategoriesRequest request);
        public Task<ResponseObject<CategoriesDTO>> GetCategoriesByID(int categoriesID);
        public Task<ResponseObject<CategoriesDTO>> GetDetailCategoriesBySlug(string slug);
        public Task<Response> UpdateCategories(int categoriesID, UpdateCategoriesRequest request);

    }
}
