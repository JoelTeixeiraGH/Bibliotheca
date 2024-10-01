using System.Text.Json.Serialization;

namespace GroupLibraryManagment.Webjobs.Entities
{
    public enum RequestStatus
    {
        Pending = 0,
        Requested = 1,
        NotReturned = 2,
        Returned = 3,
        Waiting = 4,
        Canceled = 5,
    }
    public class Request
    {
        public int RequestId { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime? EndDate { get; set; }
        public RequestStatus RequestStatus { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        public int UserId { get; set; }
        [JsonIgnore]
        public Punishment? Punishment { get; set; }
        [JsonIgnore]
        public PhysicalBook? PhysicalBook { get; set; }
        public int? PhysicalBookId { get; set; }
        [JsonIgnore]
        public GenericBook GenericBook { get; set; }
        public string ISBN { get; set; } = string.Empty;
        [JsonIgnore]
        public Library Library { get; set; }
        public int LibraryId { get; set; }
        [JsonIgnore]
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
