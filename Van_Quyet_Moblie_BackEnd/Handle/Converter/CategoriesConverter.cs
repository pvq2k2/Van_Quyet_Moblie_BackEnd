using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs.Categories;

namespace Van_Quyet_Moblie_BackEnd.Handle.Converter
{
    public class CategoriesConverter
    {
        public CategoriesDTO EntityCategoriesToDTO(Categories categories)
        {
            return new CategoriesDTO
            {
                ID = categories.ID,
                Icon = categories.Icon,
                Name = categories.Name,
                Slug = categories.Slug,
            };
        }

        public List<CategoriesDTO> ListEntityCategoriesToDTO(List<Categories> listCategories)
        {
            var listCategoriesDTO = new List<CategoriesDTO>();
            foreach (var categories in listCategories)
            {
                listCategoriesDTO.Add(EntityCategoriesToDTO(categories));
            }

            return listCategoriesDTO;
        }
    }
}
