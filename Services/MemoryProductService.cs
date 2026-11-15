using GR50322.Domain.Entities;
using GR50322.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace GR50322.UI.Services
{
    public class MemoryProductService : IProductService
    {
        private readonly ICategoryService _categoryService;
        private readonly IConfiguration _config;
        private List<StudentGroup> _items;
        private List<Category> _categories;

        public MemoryProductService([FromServices] IConfiguration config, ICategoryService categoryService)
        {
            _categories = categoryService.GetCategoryListAsync().Result.Data;
            _config = config;
            SetupData();
        }

        private void SetupData()
        {
            _items = new List<StudentGroup>
            {
                new StudentGroup
                {
                    Id = 1,
                    Name = "Иванов Иван",
                    Description = "Староста группы, специализация веб-разработка",
                    StudentsCount = 9,
                    StartYear = 2024,
                    Image="Images/6.jfif",
                    CategoryId = _categories.Find(c => c.NormalizedName.Equals("40322"))!.Id
                },
                new StudentGroup
                {
                    Id = 2,
                    Name = "Петрова Анна",
                    Description = "Профорг группы, направление ПОИТ",
                    StudentsCount = 10,
                    StartYear = 2024,
                    Image="Images/1.jfif",
                    CategoryId = _categories.Find(c => c.NormalizedName.Equals("40322"))!.Id
                },
                new StudentGroup
                {
                    Id = 3,
                    Name = "Сидоров Алексей",
                    Description = "Разработка мобильных и десктопных приложений",
                    StudentsCount = 8,
                    StartYear = 2025,
                    Image="Images/2.jfif",
                    CategoryId = _categories.Find(c => c.NormalizedName.Equals("50321"))!.Id
                },
                new StudentGroup
                {
                    Id = 4,
                    Name = "Козлова Мария",
                    Description = "Анализ данных и базы данных",
                    StudentsCount = 9,
                    StartYear = 2025,
                    Image="Images/3.jfif",
                    CategoryId = _categories.Find(c => c.NormalizedName.Equals("50321"))!.Id
                },
                new StudentGroup
                {
                    Id = 5,
                    Name = "Смирнова Екатерина",
                    Description = "Веб-технологии и проектирование интерфейсов",
                    StudentsCount = 10,
                    StartYear = 2025,
                    Image="Images/4.jfif",
                    CategoryId = _categories.Find(c => c.NormalizedName.Equals("50322"))!.Id
                },
                new StudentGroup
                {
                    Id = 6,
                    Name = "Новиков Дмитрий",
                    Description = "Архитектура программных систем",
                    StudentsCount = 8,
                    StartYear = 2025,
                    Image="Images/5.jfif",
                    CategoryId = _categories.Find(c => c.NormalizedName.Equals("50322"))!.Id
                }
            };
        }

        public Task<ResponseData<ProductListModel<StudentGroup>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            // Получаем ID категории для фильтрации (если категория выбрана)
            int? categoryId = null;
            if (categoryNormalizedName != null)
            {
                categoryId = _categories
                    .Find(c => c.NormalizedName.Equals(categoryNormalizedName))?.Id;
            }

            // Фильтрация данных по категории
            var data = _items
                .Where(d => categoryId == null || d.CategoryId.Equals(categoryId))
                .ToList();

            // Заполняем свойство Category для каждого студента
            foreach (var item in data)
            {
                item.Category = _categories.FirstOrDefault(c => c.Id == item.CategoryId);
            }


            // Получаем размер страницы из конфигурации (по умолчанию 3)
            int pageSize = _config.GetSection("ItemsPerPage").Get<int>();
            if (pageSize <= 0) pageSize = 10;

            // Вычисляем общее количество страниц
            int totalPages = (int)Math.Ceiling(data.Count / (double)pageSize);

            // Получаем данные для текущей страницы
            var listData = new ProductListModel<StudentGroup>()
            {
                Items = data.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList(),
                CurrentPage = pageNo,
                TotalPages = totalPages
            };

            var result = new ResponseData<ProductListModel<StudentGroup>>
            {
                Data = listData
            };

            if (data.Count == 0)
            {
                result.Success = false;
                result.ErrorMessage = "Нет объектов в выбранной категории";
            }

            return Task.FromResult(result);
        }

        public Task<ResponseData<StudentGroup>> GetProductByIdAsync(int id)
        {
            var item = _items.FirstOrDefault(d => d.Id == id);
            var result = new ResponseData<StudentGroup>();

            if (item == null)
            {
                result.Success = false;
                result.ErrorMessage = "Объект не найден";
            }
            else
            {
                result.Data = item;
            }

            return Task.FromResult(result);
        }

        public async Task<ResponseData<StudentGroup>> CreateProductAsync(StudentGroup product, IFormFile? formFile)
        {
            product.Id = _items.Count > 0 ? _items.Max(d => d.Id) + 1 : 1;

            // Сохранение загруженной картинки в массив байтов (вариант 2)
            if (formFile != null)
            {
                using var memoryStream = new MemoryStream();
                await formFile.CopyToAsync(memoryStream);
                product.ImageData = memoryStream.ToArray();
                product.MimeType = formFile.ContentType;
            }

            _items.Add(product);
            return new ResponseData<StudentGroup> { Data = product };
        }

        public async Task UpdateProductAsync(int id, StudentGroup product, IFormFile? formFile)
        {
            var existingItem = _items.FirstOrDefault(d => d.Id == id);
            if (existingItem != null)
            {
                existingItem.Name = product.Name;
                existingItem.Description = product.Description;
                existingItem.StudentsCount = product.StudentsCount;
                existingItem.StartYear = product.StartYear;
                existingItem.CategoryId = product.CategoryId;

                if (formFile != null)
                {
                    using var memoryStream = new MemoryStream();
                    await formFile.CopyToAsync(memoryStream);
                    existingItem.ImageData = memoryStream.ToArray();
                    existingItem.MimeType = formFile.ContentType;
                }
            }
        }

        public Task DeleteProductAsync(int id)
        {
            var item = _items.FirstOrDefault(d => d.Id == id);
            if (item != null)
            {
                _items.Remove(item);
            }
            return Task.CompletedTask;
        }
    }
}