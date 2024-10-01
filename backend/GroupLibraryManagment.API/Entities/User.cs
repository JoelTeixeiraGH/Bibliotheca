using GroupLibraryManagment.API.DTOs;
using System.Text.Json.Serialization;

namespace GroupLibraryManagment.API.Entities
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
        public static User Create(UserRegisterDTO input)
        {
            var user = new User
            {
                UserName = input.UserName,
                UserEmail = input.UserEmail,
                UserPassword = BCrypt.Net.BCrypt.HashPassword(input.UserPassword),
                LibraryId = input.LibraryId,
            };

            return user;
        }
        public void Update(UserUpdateDTO userUpdateModel)
        {
            UserName = userUpdateModel.UserName;
            UserEmail = userUpdateModel.UserEmail;
            LibraryId = userUpdateModel.LibraryId;
        }
        public void UpdatePassword(string password)
        {
            UserPassword = BCrypt.Net.BCrypt.HashPassword(password);
        }
        public void ChangeRoleToLibrarian()
        {
            UserRole = UserRole.Librarian;
        }
    }
}
