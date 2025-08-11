namespace CookHub.API.Models
{
    public class Recipe
    {
        public int RecipeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int Calories { get; set; }
        public decimal Fat { get; set; }
        public decimal Carbs { get; set; }
        public decimal Protein { get; set; }
        public int CategoryId { get; set; }
        public string Instructions { get; set; } = string.Empty;
        public List<string> Ingredients { get; set; } = new List<string>();
    }
}
