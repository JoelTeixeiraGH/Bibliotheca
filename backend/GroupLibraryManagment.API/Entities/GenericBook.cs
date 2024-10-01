using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using GroupLibraryManagment.API.DTOs;
using System.Text.Json.Serialization;

namespace GroupLibraryManagment.API.Entities
{
    public class GenericBook
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ISBN { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int PageNumber { get; set; }
        [JsonIgnore]
        public Language Language { get; set; }
        public int LanguageId { get; set; }
        public DateTime DatePublished { get; set; }
        [JsonIgnore]
        public ICollection<PhysicalBook> PhysicalBooks { get; set; } = new List<PhysicalBook>();
        [JsonIgnore]
        public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
        [JsonIgnore]
        public ICollection<BookCategory> BookCategories { get; set; } = new List<BookCategory>();
        [JsonIgnore]
        public ICollection<Evaluation> Evaluations { get; set; } = new List<Evaluation>();
        [JsonIgnore]
        public ICollection<Request> Requests { get; set; } = new List<Request>();
        public string Thumbnail { get; set; } = string.Empty;
        public string SmallThumbnail { get; set; } = string.Empty;
        public static GenericBook Create(GenericBookDTO input, List<int> authorIds, List<int> categoryIds, int languageId)
        {
            var genericBook = new GenericBook
            {
                ISBN = input.ISBN,
                Title = input.Title,
                Description = input.Description,
                PageNumber = input.PageNumber,
                LanguageId = languageId,
                DatePublished = input.DatePublished,
                Thumbnail = input.Thumbnail,
                SmallThumbnail = input.SmallThumbnail
            };

            genericBook.AddAuthors(authorIds);
            genericBook.AddCategories(categoryIds);

            return genericBook;
        }
        public void Update(GenericBookDTO input)
        {
            Title = input.Title;
            Description = input.Description;
            PageNumber = input.PageNumber;
            DatePublished = input.DatePublished;
            Thumbnail = input.Thumbnail;
            SmallThumbnail = input.SmallThumbnail;

            BookAuthors.Clear();
            BookCategories.Clear();
        }

        public void AddAuthors(IEnumerable<int> authorIds)
        {
            foreach (var authorId in authorIds)
            {
                BookAuthors.Add(new BookAuthor
                {
                    AuthorId = authorId
                });
            }
        }
        public void AddCategories(IEnumerable<int> categoryIds)
        {
            foreach (var categoryId in categoryIds)
            {
                BookCategories.Add(new BookCategory
                {
                    CategoryId = categoryId
                });
            }
        }

    }
}
