using GroupLibraryManagment.API.DTOs;
using System.Text.Json.Serialization;

namespace GroupLibraryManagment.API.Entities
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        [JsonIgnore]
        public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
        public static Author Create(AuthorDTO input)
        {
            var author = new Author
            {
                AuthorName = input.AuthorName
            };

            return author;
        }
        public void Update(AuthorDTO input)
        {
            AuthorName = input.AuthorName;
        }
    }
}