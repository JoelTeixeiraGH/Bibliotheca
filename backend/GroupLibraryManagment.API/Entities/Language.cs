using GroupLibraryManagment.API.DTOs;
using System.Text.Json.Serialization;

namespace GroupLibraryManagment.API.Entities
{
    public class Language
    {
        public int LanguageId { get; set; }
        public string? LanguageName { get; set; }
        public string LanguageAlias { get; set; } = string.Empty;
        [JsonIgnore]
        public ICollection<GenericBook> GenericBooks { get; set; } = new List<GenericBook>();
        public static Language Create(LanguageDTO input)
        {
            var language = new Language
            {
                LanguageName = input.LanguageName,
                LanguageAlias = input.LanguageAlias
            };

            return language;
        }
        public void Update(LanguageDTO input)
        {
            LanguageName = input.LanguageName;
            LanguageAlias = input.LanguageAlias;
        }
    }
}