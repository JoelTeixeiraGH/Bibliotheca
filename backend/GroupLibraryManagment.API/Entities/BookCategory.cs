namespace GroupLibraryManagment.API.Entities
{
    public class BookCategory
    {
        public GenericBook? GenericBook { get; set; }
        public string? ISBN { get; set; }
        public Category? Category { get; set; }
        public int? CategoryId { get; set; }
    }
}