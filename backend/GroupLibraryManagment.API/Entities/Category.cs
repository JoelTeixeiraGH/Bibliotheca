using GroupLibraryManagment.API.DTOs;
using System.Text.Json.Serialization;

namespace GroupLibraryManagment.API.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        [JsonIgnore]
        public ICollection<BookCategory> BookCategories { get; set; } = new List<BookCategory>();
        public static Category Create(CategoryDTO input)
        {
            var category = new Category
            {
                CategoryName = input.CategoryName
            };

            return category;
        }
        public void Update(CategoryDTO input)
        {
            CategoryName = input.CategoryName;
        }
    }
}
