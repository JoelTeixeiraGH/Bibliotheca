using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GroupLibraryManagment.Webjobs.Entities
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
    }
}
