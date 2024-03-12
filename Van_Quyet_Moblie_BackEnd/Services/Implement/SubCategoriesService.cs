using Van_Quyet_Moblie_BackEnd.DataContext;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs.SubCategories;
using Van_Quyet_Moblie_BackEnd.Handle.Request.SubCategoriesRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;
using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Services.Interface;
using Van_Quyet_Moblie_BackEnd.Handle.Converter;
using Microsoft.EntityFrameworkCore;
using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Middleware;

namespace Van_Quyet_Moblie_BackEnd.Services.Implement
{
    public class SubCategoriesService : ISubCategoriesService
    {
        private readonly SubCategoriesConverter _subCategoriesConverter;
        private readonly AppDbContext _dbContext;
        private readonly ResponseObject<SubCategoriesDTO> _responseSubCategories;
        private readonly CloudinaryHelper _cloudinaryHelper;
        private readonly TokenHelper _tokenHelper;
        private readonly Response _response;

        public SubCategoriesService(AppDbContext dbContext,
            ResponseObject<SubCategoriesDTO> responseSubCategories,
            CloudinaryHelper cloudinaryHelper,
            TokenHelper tokenHelper,
            Response response)
        {
            _subCategoriesConverter = new SubCategoriesConverter();
            _dbContext = dbContext;
            _responseSubCategories = responseSubCategories;
            _cloudinaryHelper = cloudinaryHelper;
            _tokenHelper = tokenHelper;
            _response = response;
        }
        public async Task<Response> CreateSubCategories(CreateSubCategoriesRequest request)
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
            if (!await _dbContext.Categories.AnyAsync(x => x.ID == request.CategoriesID))
            {
                throw new CustomException(StatusCodes.Status404NotFound, "Danh mục cha không tồn tại !");
            }
            InputHelper.IsImage(request.Image!);

            string image = await _cloudinaryHelper.UploadImage(request.Image!, $"van-quyet-mobile/sub-categories", "sub-ct");
            var subCategories = new SubCategories
            {
                Name = request.Name,
                Image = image,
                CategoriesID = request.CategoriesID,
                Slug = InputHelper.CreateSlug(request.Name)
            };

            await _dbContext.SubCategories.AddAsync(subCategories);
            await _dbContext.SaveChangesAsync();

            return _response.ResponseSuccess("Thêm danh mục thành công !");
        }

        public async Task<PageResult<SubCategoriesDTO>> GetAllSubCategories(Pagination pagination, int categoriesID)
        {
            _tokenHelper.IsToken();
            string role = _tokenHelper.GetRole();

            if (role != "Admin")
            {
                throw new CustomException(StatusCodes.Status401Unauthorized, "Không có quyền !");
            }

            var query = _dbContext.SubCategories.Where(x => x.CategoriesID == categoriesID).AsQueryable();

            pagination.TotalCount = await query.CountAsync();
            var result = PageResult<SubCategories>.ToPageResult(pagination, query);

            var list = result.ToList();

            return new PageResult<SubCategoriesDTO>(pagination, _subCategoriesConverter.ListEntitySubCategoriesToDTO(result.ToList()));
        }

        public async Task<ResponseObject<SubCategoriesDTO>> GetSubCategoriesByID(int subCategoriesID)
        {
            _tokenHelper.IsToken();
            string role = _tokenHelper.GetRole();
            if (role != "Admin")
            {
                throw new CustomException(StatusCodes.Status401Unauthorized, "Không có quyền !");
            }
            var subCategories = await _dbContext.SubCategories.FirstOrDefaultAsync(x => x.ID == subCategoriesID) ?? throw new CustomException(StatusCodes.Status404NotFound, "Danh mục không tồn tại !");

            return _responseSubCategories.ResponseSuccess("Thành công !", _subCategoriesConverter.EntitySubCategoriesToDTO(subCategories));
        }

        public async Task<Response> UpdateSubCategories(int subCategoriesID, UpdateSubCategoriesRequest request)
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
            if (!await _dbContext.Categories.AnyAsync(x => x.ID == request.CategoriesID))
            {
                throw new CustomException(StatusCodes.Status404NotFound, "Danh mục cha không tồn tại !");
            }
            var subCategories = await _dbContext.SubCategories.FirstOrDefaultAsync(x => x.ID == subCategoriesID) ?? throw new CustomException(StatusCodes.Status404NotFound, "Danh mục không tồn tại !");

            string img;
            if (request.Image == null || request.Image.Length == 0)
            {
                img = subCategories.Image!;
            }
            else
            {
                InputHelper.IsImage(request.Image!);
                img = await _cloudinaryHelper.UploadImage(request.Image!, $"van-quyet-mobile/sub-categories", "sub-ct");
                await _cloudinaryHelper.DeleteImageByUrl(subCategories.Image!);
            }


            subCategories.Name = request.Name;
            subCategories.Image = img;
            subCategories.CategoriesID = request.CategoriesID;
            subCategories.Slug = InputHelper.CreateSlug(request.Name);
            subCategories.UpdatedAt = DateTime.Now;

            _dbContext.SubCategories.Update(subCategories);
            await _dbContext.SaveChangesAsync();

            return _response.ResponseSuccess("Cập nhật danh mục thành công !");
        }
    }
}
