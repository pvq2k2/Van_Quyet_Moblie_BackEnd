﻿using QuanLyTrungTam_API.Helper;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs.SubCategories;
using Van_Quyet_Moblie_BackEnd.Handle.Request.CategoriesRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Request.SubCategoriesRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;

namespace Van_Quyet_Moblie_BackEnd.Services.Interface
{
    public interface ISubCategoriesService
    {
        public Task<PageResult<SubCategoriesDTO>> GetAllSubCategories(Pagination pagination, int categoriesID);
        public Task<Response> CreateSubCategories(CreateSubCategoriesRequest request);
        public Task<ResponseObject<SubCategoriesDTO>> GetSubCategoriesByID(int subCategoriesID);
        //public Task<ResponseObject<SubCategoriesDTO>> GetCategoriesBySlug(string slug);
        public Task<Response> UpdateSubCategories(int subCategoriesID, UpdateSubCategoriesRequest request);
    }
}