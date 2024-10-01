using System.Text.Json.Serialization;

namespace GroupLibraryManagment.Webjobs.Entities
{
    public enum PhysicalBookStatus
    {
        AtLibrary = 0,
        Requested = 1,
        Transfer = 2,
    }
    public class PhysicalBook
    {
        public int PhysicalBookId { get; set; }
        public PhysicalBookStatus PhysicalBookStatus { get; set; } = PhysicalBookStatus.AtLibrary;
        [JsonIgnore]
        public Library Library { get; set; }
        public int LibraryId { get; set; }
        [JsonIgnore]
        public GenericBook GenericBook { get; set; }
        public string ISBN { get; set; } = string.Empty;
        [JsonIgnore]
        public ICollection<Request> Requests { get; set; } = new List<Request>();
        [JsonIgnore]
        public ICollection<Transfer> Transfers { get; set; } = new List<Transfer>();
    }
}
