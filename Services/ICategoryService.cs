using GR50322.Domain.Models;
using GR50322.Domain.Entities;
namespace GR50322.UI.Services
{
    public interface ICategoryService
    {
        public Task<ResponseData<List<Category>>> GetCategoryListAsync();
    }
}
