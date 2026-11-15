
using GR50322.Domain.Entities;
using GR50322.Domain.Models;

namespace GR50322.UI.Services
{
    public class MemoryCategoryService : ICategoryService
    {
        public Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Группа 40322", NormalizedName = "40322" },
                new Category { Id = 2, Name = "Группа 50321", NormalizedName = "50321" },
                new Category { Id = 3, Name = "Группа 50322", NormalizedName = "50322" }
            };

            var result = new ResponseData<List<Category>>
            {
                Data = categories
            };

            return Task.FromResult(result);
        }
    }
}