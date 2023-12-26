using Microsoft.EntityFrameworkCore;
using QuanLyTrungTam_API.Helper;
using Van_Quyet_Moblie_BackEnd.DataContext;
using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Handle.Converter;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs.Categories;
using Van_Quyet_Moblie_BackEnd.Handle.Request.CategoriesRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;
using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Middleware;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Services.Implement
{
    public class CategoriesService: ICategoriesService
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

        public async Task<Response> CreateCategories(CreateCategoriesRequest request)
        {
            _tokenHelper.IsToken();
            string role = _tokenHelper.GetRole();
            if (role != "Admin")
            {
                throw new CustomException(StatusCodes.Status401Unauthorized, "Không có quyền !");
            }
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Tên danh mục không được để trống !");
            }

            var category = new Categories
            {
                Icon = request.Icon,
                Name = request.Name,
                Slug = InputHelper.CreateSlug(request.Name),
            };
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            return _response.ResponseSuccess("Thêm danh mục thành công !");
        }

        public async Task<PageResult<CategoriesDTO>> GetAllCategories(Pagination pagination)
        {
            _tokenHelper.IsToken();
            string role = _tokenHelper.GetRole();

            if (role != "Admin")
            {
                throw new CustomException(StatusCodes.Status401Unauthorized, "Không có quyền !");
            }

            var query = _dbContext.Categories.OrderByDescending(x => x.ID).AsQueryable();

            pagination.TotalCount = await query.CountAsync();
            var result = PageResult<Categories>.ToPageResult(pagination, query);

            var list = result.ToList();

            return new PageResult<CategoriesDTO>(pagination, _categoriesConverter.ListEntityCategoriesToDTO(result.ToList()));
        }

        public async Task<ResponseObject<CategoriesDTO>> GetCategoriesByID(int categoriesID)
        {
            _tokenHelper.IsToken();
            string role = _tokenHelper.GetRole();
            if (role != "Admin")
            {
                throw new CustomException(StatusCodes.Status401Unauthorized, "Không có quyền !");
            }
            var categories = await _dbContext.Categories.FirstOrDefaultAsync(x => x.ID == categoriesID) ?? throw new CustomException(StatusCodes.Status404NotFound, "Danh mục không tồn tại !");

            return _responseCategories.ResponseSuccess("Thành công !", _categoriesConverter.EntityCategoriesToDTO(categories));
        }

        public async Task<ResponseObject<CategoriesDTO>> GetDetailCategoriesBySlug(string slug)
        {
            _tokenHelper.IsToken();
            string role = _tokenHelper.GetRole();
            if (role != "Admin")
            {
                throw new CustomException(StatusCodes.Status401Unauthorized, "Không có quyền !");
            }
            var categories = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Slug == slug) ?? throw new CustomException(StatusCodes.Status404NotFound, "Danh mục không tồn tại !");

            return _responseCategories.ResponseSuccess("Thành công !", _categoriesConverter.EntityCategoriesToDTO(categories));
        }

        public async Task<Response> UpdateCategories(int categoriesID, UpdateCategoriesRequest request)
        {
            _tokenHelper.IsToken();
            string role = _tokenHelper.GetRole();
            if (role != "Admin")
            {
                throw new CustomException(StatusCodes.Status401Unauthorized, "Không có quyền !");
            }
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Tên danh mục không được để trống !");
            }
            var categories = await _dbContext.Categories.FirstOrDefaultAsync(x => x.ID == categoriesID) ?? throw new CustomException(StatusCodes.Status404NotFound, "Danh mục không tồn tại !");

            categories.Icon = request.Icon;
            categories.Name = request.Name;
            categories.Slug = InputHelper.CreateSlug(request.Name);
            categories.UpdatedAt = DateTime.Now;

            _dbContext.Categories.Update(categories);
            await _dbContext.SaveChangesAsync();
            return _response.ResponseSuccess("Cập nhật danh mục thành công !");
        }
    }
}
