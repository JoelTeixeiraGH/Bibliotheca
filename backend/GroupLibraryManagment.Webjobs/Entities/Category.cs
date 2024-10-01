using System.Text.Json.Serialization;

namespace GroupLibraryManagment.Webjobs.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        [JsonIgnore]
        public ICollection<BookCategory> BookCategories { get; set; } = new List<BookCategory>();
    }
}
