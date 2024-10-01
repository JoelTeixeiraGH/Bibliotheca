using System.Text.Json.Serialization;

namespace GroupLibraryManagment.Webjobs.Entities
{
    public enum UserRole
    {
        Admin = 873,
        Librarian = 687,
        Reader = 190,
    }
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public UserRole UserRole { get; set; } = UserRole.Reader;
        [JsonIgnore]
        public string UserPassword { get; set; } = string.Empty;
        [JsonIgnore]
        public string RefreshToken { get; set; } = string.Empty;
        [JsonIgnore]
        public DateTime TokenCreated { get; set; }
        [JsonIgnore]
        public DateTime TokenExpires { get; set; }
        [JsonIgnore]
        public Library Library { get; set; }
        public int LibraryId { get; set; }
        [JsonIgnore]
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        [JsonIgnore]
        public ICollection<Evaluation> Evaluations { get; set; } = new List<Evaluation>();
        [JsonIgnore]
        public ICollection<Request> Requests { get; set; } = new List<Request>();
    }
}
