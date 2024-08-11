using Microsoft.EntityFrameworkCore;
using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.DataContext;
using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Handle.Converter;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs.Categories;
using Van_Quyet_Moblie_BackEnd.Handle.Request.CategoriesRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;
using Van_Quyet_Moblie_BackEnd.Middleware;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Services.Implement
{
    public class CategoriesService : ICategoriesService
    {
        private readonly CategoriesConverter _categoriesConverter;
        private readonly AppDbContext _dbContext;
        private readonly TokenHelper _tokenHelper;
        private readonly Response _response;
        private readonly ResponseObject<CategoriesDTO> _responseCategories;

        public CategoriesService(AppDbContext dbContext,
            TokenHelper tokenHelper,
            Response response,
            ResponseObject<CategoriesDTO> responseCategoties)
        {
            _categoriesConverter = new CategoriesConverter();
            _dbContext = dbContext;
            _tokenHelper = tokenHelper;
            _response = response;
            _responseCategories = responseCategoties;
        }

        #region Validate
        private void IsAdmin()
        {
            _tokenHelper.IsToken();
            string role = _tokenHelper.GetRole();
            if (role != "Admin")
            {
                throw new CustomException(StatusCodes.Status403Forbidden, "Không có quyền !");
            }
        }
        private async Task<Categories> IsCategoryExist(int categoryID)
        {
            var categories = await _dbContext.Categories.FirstOrDefaultAsync(x => x.ID == categoryID);
            return categories ?? throw new CustomException(StatusCodes.Status404NotFound, "Danh mục không tồn tại !");
        }
        private static void ValidateCategory(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Tên danh mục không được để trống !");
            }
        }
        #endregion

        public async Task<Response> CreateCategories(CreateCategoriesRequest request)
        {
            IsAdmin();
            ValidateCategory(request.Name!);

            var category = new Categories
            {
                Icon = request.Icon,
                Name = request.Name,
                Slug = InputHelper.CreateSlug(request.Name!),
            };
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            return _response.ResponseSuccess("Thêm danh mục thành công !");
        }

        public async Task<PageResult<CategoriesDTO>> GetAllCategories(Pagination pagination)
        {
            IsAdmin();
            var query = _dbContext.Categories.OrderByDescending(x => x.ID).AsQueryable();

            pagination.TotalCount = await query.CountAsync();
            var result = PageResult<Categories>.ToPageResult(pagination, query);

            var list = result.ToList();

            return new PageResult<CategoriesDTO>(pagination, _categoriesConverter.ListEntityCategoriesToDTO(result.ToList()));
        }

        public async Task<object> GetCategoriesToView()
        {
            var query = await _dbContext.Categories.Include(c => c.ListSubCategories).ToListAsync();
            return query;
        }

        public async Task<ResponseObject<CategoriesDTO>> GetCategoriesByID(int categoriesID)
        {
            IsAdmin();
            var categories = await IsCategoryExist(categoriesID);
            return _responseCategories.ResponseSuccess("Thành công !", _categoriesConverter.EntityCategoriesToDTO(categories));
        }

        public async Task<ResponseObject<CategoriesDTO>> GetDetailCategoriesBySlug(string slug)
        {
            IsAdmin();
            var categories = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Slug == slug) ?? throw new CustomException(StatusCodes.Status404NotFound, "Danh mục không tồn tại !");

            return _responseCategories.ResponseSuccess("Thành công !", _categoriesConverter.EntityCategoriesToDTO(categories));
        }

        public async Task<Response> UpdateCategories(int categoriesID, UpdateCategoriesRequest request)
        {
            IsAdmin();
            ValidateCategory(request.Name!);
            var categories = await IsCategoryExist(categoriesID);

            categories.Icon = request.Icon;
            categories.Name = request.Name;
            categories.Slug = InputHelper.CreateSlug(request.Name!);
            categories.UpdatedAt = DateTime.Now;

            _dbContext.Categories.Update(categories);
            await _dbContext.SaveChangesAsync();
            return _response.ResponseSuccess("Cập nhật danh mục thành công !");
        }
    }
}
