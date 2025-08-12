using Microsoft.AspNetCore.Mvc;
using CookHub.API.Models;

namespace CookHub.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private static List<Category> _categories = new List<Category>
        {
            new Category { 
                CategoryId = 1, 
                Name = "Salads", 
                Description = "Fresh and healthy salads", 
                Color = "green",
                Icon = "mdi-food-salad",
                Letter = "S",
                Abbreviation = "Sal"
            },
            new Category { 
                CategoryId = 2, 
                Name = "Desserts", 
                Description = "Sweet treats and desserts", 
                Color = "pink",
                Icon = "mdi-cake-variant",
                Letter = "D",
                Abbreviation = "Des"
            },
            new Category { 
                CategoryId = 3, 
                Name = "Main Dishes", 
                Description = "Main course dishes", 
                Color = "blue",
                Icon = "mdi-food-steak",
                Letter = "M",
                Abbreviation = "Main"
            },
        };

        [HttpGet]
        public ActionResult<IEnumerable<Category>> GetCategories()
        {
            return Ok(_categories);
        }

        [HttpGet("{id}")]
        public ActionResult<Category> GetCategory(int id)
        {
            var category = _categories.FirstOrDefault(c => c.CategoryId == id);
            if (category == null)
                return NotFound();
            return Ok(category);
        }
    }
}
