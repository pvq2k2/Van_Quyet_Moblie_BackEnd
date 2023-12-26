using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs.Categories;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs.SubCategories;

namespace Van_Quyet_Moblie_BackEnd.Handle.Converter
{
    public class SubCategoriesConverter
    {
        public SubCategoriesDTO EntitySubCategoriesToDTO(SubCategories subCategories)
        {
            return new SubCategoriesDTO
            {
                ID = subCategories.ID,
                Image = subCategories.Image,
                Name = subCategories.Name,
                Slug = subCategories.Slug,
            };
        }

        public List<SubCategoriesDTO> ListEntitySubCategoriesToDTO(List<SubCategories> listSubCategories)
        {
            var listSubCategoriesDTO = new List<SubCategoriesDTO>();
            foreach (var subCategories in listSubCategories)
            {
                listSubCategoriesDTO.Add(EntitySubCategoriesToDTO(subCategories));
            }

            return listSubCategoriesDTO;
        }
    }
}
