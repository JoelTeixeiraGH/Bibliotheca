namespace GroupLibraryManagment.API.Entities
{
    public class BookAuthor
    {
        public GenericBook? GenericBook { get; set; }
        public string? ISBN { get; set; }
        public Author? Author { get; set; }
        public int? AuthorId { get; set; }
    }
}
