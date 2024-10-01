using System.Text.Json.Serialization;

namespace GroupLibraryManagment.Webjobs.Entities
{
    public class Language
    {
        public int LanguageId { get; set; }
        public string? LanguageName { get; set; }
        public string LanguageAlias { get; set; } = string.Empty;
        [JsonIgnore]
        public ICollection<GenericBook> GenericBooks { get; set; } = new List<GenericBook>();
       
    }
}