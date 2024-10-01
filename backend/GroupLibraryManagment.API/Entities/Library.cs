using GroupLibraryManagment.API.DTOs;
using System.Text.Json.Serialization;

namespace GroupLibraryManagment.API.Entities
{
    public class Library
    {
        public int LibraryId { get; set; }
        public string? LibraryName { get; set; }
        public string LibraryAlias { get; set; } = string.Empty;
        public string? LibraryAddress { get; set; }
        [JsonIgnore]
        public ICollection<PhysicalBook> PhysicalBooks { get; set; } = new List<PhysicalBook>();
        [JsonIgnore]
        public ICollection<User> Users { get; set; } = new List<User>();
        [JsonIgnore]
        public ICollection<Transfer> Transfers { get; set; } = new List<Transfer>();
        [JsonIgnore]
        public ICollection<Request> Requests { get; set; } = new List<Request>();
        public static Library Create(LibraryDTO input)
        {
            var library = new Library
            {
                LibraryName = input.LibraryName,
                LibraryAlias = input.LibraryAlias,
                LibraryAddress = input.LibraryAddress
            };

            return library;
        }
        public void Update(LibraryDTO input)
        {
            LibraryName = input.LibraryName;
            LibraryAlias = input.LibraryAlias;
            LibraryAddress = input.LibraryAddress;
        }
    }
}
