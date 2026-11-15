using GR50322.Domain.Entities;
using GR50322.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GR50322.UI.Controllers
{
    public class ProductController(ICategoryService categoryService, IProductService productService) : Controller
    {
        public async Task<IActionResult> Index(string? category)
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
            var productResponse = await productService.GetProductListAsync(category);

            if (!productResponse.Success)
            {
                ViewData["Error"] = productResponse.ErrorMessage;
                return View(new List<StudentGroup>());
            }

            return View(productResponse.Data.Items);
        }
    }
}
