using GR50322.Domain.Entities;
using GR50322.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GR50322.Domain.Models;
namespace GR50322.UI.Controllers
{
    public class ProductController(ICategoryService categoryService, IProductService productService) : Controller
    {
        [Route("Catalog")]
        [Route("Catalog/{category}")]
        public async Task<IActionResult> Index(string? category, int pageNo=1)
        {

            // Получить список категорий
            var categoriesResponse = await categoryService.GetCategoryListAsync();

            // Если список не получен, вернуть код 404
            if (!categoriesResponse.Success)
                return NotFound(categoriesResponse.ErrorMessage);

            // Передать список категорий во ViewData
            ViewData["categories"] = categoriesResponse.Data;

            // Передать во ViewData имя текущей категории
            var currentCategory = category == null
                ? "Все"
                : categoriesResponse.Data?.FirstOrDefault(c => c.NormalizedName == category)?.Name;
            ViewData["currentCategory"] = currentCategory;

            // Получить список объектов с фильтрацией по выбранной категории
            var productResponse = await productService.GetProductListAsync(category, pageNo);

            if (!productResponse.Success)
            {
                ViewData["Error"] = productResponse.ErrorMessage;
                return View(new ProductListModel<StudentGroup>());
            }

            return View(productResponse.Data);
        }
    }
}
