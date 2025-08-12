namespace CookHub.API.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Letter { get; set; } = string.Empty;
        public string Abbreviation { get; set; } = string.Empty;
    }
}
