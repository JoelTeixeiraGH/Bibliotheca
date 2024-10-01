using System.Text.Json.Serialization;

namespace GroupLibraryManagment.Webjobs.Entities
{
    public enum TransferStatus
    {
        Pending = 0,
        Accepted = 1,
        Rejected = 2,
        Canceled = 3
    }
    public class Transfer
    {
        public int TransferId { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; }
        public TransferStatus TransferStatus { get; set; } = TransferStatus.Pending;
        [JsonIgnore]
        public Library SourceLibrary { get; set; }
        public int SourceLibraryId { get; set; }
        [JsonIgnore]
        public Library DestinationLibrary { get; set; }
        public int DestinationLibraryId { get; set; }
        [JsonIgnore]
        public PhysicalBook PhysicalBook { get; set; }
        public int PhysicalBookId { get; set; }
    }
}
