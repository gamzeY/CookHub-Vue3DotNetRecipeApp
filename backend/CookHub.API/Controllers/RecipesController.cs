using Microsoft.AspNetCore.Mvc;
using CookHub.API.Models;

namespace CookHub.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecipesController : ControllerBase
    {
        private static List<Recipe> _recipes = new List<Recipe>
        {
           
            new Recipe
            {
                RecipeId = 2,
                Name = "Chicken Caesar Salad",
                Description = "Fresh salad with grilled chicken and Caesar dressing",
                ImageUrl = "https://images.unsplash.com/photo-1546793665-c74683f339c1?w=400&h=300&fit=crop&v=food2",
                Calories = 350,
                Fat = 15,
                Carbs = 10,
                Protein = 35,
                CategoryId = 1,
                Instructions = "1. Grill chicken breast\n2. Wash and chop lettuce\n3. Make Caesar dressing\n4. Combine all ingredients",
                Ingredients = new List<string> { "Romaine lettuce", "Chicken breast", "Parmesan cheese", "Croutons", "Caesar dressing" }
            },
            new Recipe
            {
                RecipeId = 3,
                Name = "Chocolate Chip Cookies",
                Description = "Soft and chewy chocolate chip cookies with melted chocolate chunks",
                ImageUrl = "https://images.unsplash.com/photo-1499636136210-6f4ee915583e?w=400&h=300&fit=crop&v=food3",
                Calories = 150,
                Fat = 8,
                Carbs = 18,
                Protein = 2,
                CategoryId = 2,
                Instructions = "1. Cream butter and sugar\n2. Add eggs and vanilla\n3. Mix in flour and chocolate chips\n4. Bake at 350°F for 10-12 minutes",
                Ingredients = new List<string> { "Flour", "Butter", "Sugar", "Eggs", "Chocolate chips", "Vanilla extract" }
            },
            new Recipe
            {
                RecipeId = 4,
                Name = "Beef Tacos",
                Description = "Delicious Mexican beef tacos with fresh toppings and lime",
                ImageUrl = "https://images.unsplash.com/photo-1565299585323-38d6b0865b47?w=400&h=300&fit=crop&v=food4",
                Calories = 450,
                Fat = 20,
                Carbs = 35,
                Protein = 30,
                CategoryId = 3,
                Instructions = "1. Cook ground beef with spices\n2. Warm tortillas\n3. Add beef to tortillas\n4. Top with vegetables and salsa",
                Ingredients = new List<string> { "Ground beef", "Tortillas", "Onion", "Tomatoes", "Lettuce", "Cheese", "Salsa" }
            },
        
            new Recipe
            {
                RecipeId = 7,
                Name = "Greek Salad",
                Description = "Fresh Mediterranean salad with feta cheese and olives",
                ImageUrl = "https://images.unsplash.com/photo-1540420773420-3366772f4999?w=400&h=300&fit=crop&v=food7",
                Calories = 280,
                Fat = 12,
                Carbs = 8,
                Protein = 8,
                CategoryId = 1,
                Instructions = "1. Chop vegetables\n2. Mix with feta cheese\n3. Add olives and dressing\n4. Serve chilled",
                Ingredients = new List<string> { "Cucumber", "Tomatoes", "Red onion", "Feta cheese", "Kalamata olives", "Olive oil" }
            },
            new Recipe
            {
                RecipeId = 8,
                Name = "Tiramisu",
                Description = "Classic Italian dessert with coffee and mascarpone cream",
                ImageUrl = "https://images.unsplash.com/photo-1571877227200-a0d98ea607e9?w=400&h=300&fit=crop&v=food8",
                Calories = 320,
                Fat = 18,
                Carbs = 28,
                Protein = 6,
                CategoryId = 2,
                Instructions = "1. Make coffee mixture\n2. Layer ladyfingers\n3. Add mascarpone cream\n4. Chill for 4 hours",
                Ingredients = new List<string> { "Ladyfingers", "Mascarpone cheese", "Coffee", "Eggs", "Sugar", "Cocoa powder" }
            },
            new Recipe
            {
                RecipeId = 9,
                Name = "Grilled Salmon",
                Description = "Perfectly grilled salmon with herbs and lemon",
                ImageUrl = "https://images.unsplash.com/photo-1467003909585-2f8a72700288?w=400&h=300&fit=crop&v=food9",
                Calories = 380,
                Fat = 22,
                Carbs = 2,
                Protein = 42,
                CategoryId = 3,
                Instructions = "1. Season salmon with herbs\n2. Grill for 4-5 minutes per side\n3. Serve with lemon wedges\n4. Garnish with fresh herbs",
                Ingredients = new List<string> { "Salmon fillet", "Lemon", "Fresh herbs", "Olive oil", "Salt", "Black pepper" }
            },
            new Recipe
            {
                RecipeId = 10,
                Name = "Test Recipe for Deletion",
                Description = "This is a test recipe to verify delete functionality",
                ImageUrl = "https://images.unsplash.com/photo-1565299585323-38d6b0865b47?w=400&h=300&fit=crop&v=test",
                Calories = 300,
                Fat = 15,
                Carbs = 25,
                Protein = 20,
                CategoryId = 1,
                Instructions = "1. Test step one\n2. Test step two\n3. Test step three",
                Ingredients = new List<string> { "Test ingredient 1", "Test ingredient 2", "Test ingredient 3" }
            },
        };

        [HttpGet]
        public ActionResult<IEnumerable<Recipe>> GetRecipes()
        {
            return Ok(_recipes);
        }

        [HttpGet("{id}")]
        public ActionResult<Recipe> GetRecipe(int id)
        {
            var recipe = _recipes.FirstOrDefault(r => r.RecipeId == id);
            if (recipe == null)
                return NotFound();
            return Ok(recipe);
        }

        [HttpGet("byCategory/{categoryId}")]
        public ActionResult<IEnumerable<Recipe>> GetRecipesByCategory(int categoryId)
        {
            var recipes = _recipes.Where(r => r.CategoryId == categoryId).ToList();
            return Ok(recipes);
        }

        [HttpPost]
        public ActionResult<Recipe> CreateRecipe(Recipe recipe)
        {
            recipe.RecipeId = _recipes.Max(r => r.RecipeId) + 1;
            _recipes.Add(recipe);
            return CreatedAtAction(nameof(GetRecipe), new { id = recipe.RecipeId }, recipe);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateRecipe(int id, Recipe recipe)
        {
            try
            {
                var existingRecipe = _recipes.FirstOrDefault(r => r.RecipeId == id);
                if (existingRecipe == null)
                    return NotFound();

                existingRecipe.Name = recipe.Name;
                existingRecipe.Description = recipe.Description;
                existingRecipe.ImageUrl = recipe.ImageUrl;
                existingRecipe.VideoUrl = recipe.VideoUrl;
                existingRecipe.Calories = recipe.Calories;
                existingRecipe.Fat = recipe.Fat;
                existingRecipe.Carbs = recipe.Carbs;
                existingRecipe.Protein = recipe.Protein;
                existingRecipe.CategoryId = recipe.CategoryId;
                existingRecipe.Instructions = recipe.Instructions;
                existingRecipe.Ingredients = recipe.Ingredients;

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating recipe: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRecipe(int id)
        {
            try
            {
                Console.WriteLine($"Attempting to delete recipe with ID: {id}");
                Console.WriteLine($"Current recipes count: {_recipes.Count}");
                Console.WriteLine($"Available recipe IDs: {string.Join(", ", _recipes.Select(r => r.RecipeId))}");
                
                var recipe = _recipes.FirstOrDefault(r => r.RecipeId == id);
                if (recipe == null)
                {
                    Console.WriteLine($"Recipe with ID {id} not found");
                    return NotFound();
                }

                Console.WriteLine($"Found recipe: {recipe.Name}");
                _recipes.Remove(recipe);
                Console.WriteLine($"Recipe deleted. New count: {_recipes.Count}");
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in DeleteRecipe: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return BadRequest($"Error deleting recipe: {ex.Message}");
            }
        }
    }
}
