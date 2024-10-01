using System.Text.Json.Serialization;

namespace GroupLibraryManagment.Webjobs.Entities
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public string NotificationTitle { get; set; } = string.Empty;
        public string NotificationDescription { get; set; } = string.Empty;
        public DateTime EmittedDate { get; set; } = DateTime.Now;
        public DateTime? EndDate { get; set; }
        public bool ForAll { get; set; } = false;
        [JsonIgnore]
        public User? User { get; set; }
        public int? UserId { get; set; }
        [JsonIgnore]
        public Request? Request { get; set; }
        public int? RequestId { get; set; }
        [JsonIgnore]
        public Library? Library { get; set; }
        public int? LibraryId { get; set; }
        public static Notification CreateNotificationForEndRequestEminent(int requestId, string isbn, DateTime endDate, int daysLeft)
        {
            var notification = new Notification
            {
                NotificationTitle = "Return Notice",
                NotificationDescription = "End date for request of book " + isbn + " eminent. Days until it ends: " + daysLeft,
                EndDate = endDate,
                ForAll = false,
                RequestId = requestId,
            };

            return notification;
        }
        public static Notification CreateNotificationForPunishment(int requestId, string isbn, string punishmentReason)
        {
            var notification = new Notification
            {
                NotificationTitle = "Punishment",
                NotificationDescription = "Punishment issued for request of book " + isbn + " for reason: " + punishmentReason,
                EndDate = DateTime.Now,
                ForAll = false,
                RequestId = requestId,
            };

            return notification;
        }
        public static Notification CreateNotificationForIncreasePunishmentLevel(int requestId, string isbn, PunishmentLevel punishmentLevel)
        {
            var notification = new Notification
            {
                NotificationTitle = "Punishment",
                NotificationDescription = "Punishment level increased to " + punishmentLevel + " for request of book " + isbn + " .",
                EndDate = DateTime.Now,
                ForAll = false,
                RequestId = requestId,
            };

            return notification;
        }
        public static Notification CreateNotificationForCanceledRequest(int requestId, string isbn)
        {
            var notification = new Notification
            {
                NotificationTitle = "Request",
                NotificationDescription = "Request for book " + isbn + " has been canceled.",
                EndDate = DateTime.Now.AddDays(1),
                ForAll = false,
                RequestId = requestId,
            };

            return notification;
        }
        public static Notification CreateNotificationForAvailableBook(int userId, string isbn)
        {
            var notification = new Notification
            {
                NotificationTitle = "Request for book " + isbn,
                NotificationDescription = "Book available to pickup. Will be available for 3 days",
                EndDate = DateTime.Now.AddDays(3),
                ForAll = false,
                UserId = userId,
            };

            return notification;
        }
        public static Notification CreateNotificationForCanceledTransfer(int libraryId, string libraryAlias, int physicalBookId)
        {
            var notification = new Notification
            {
                NotificationTitle = "Book Transfer",
                NotificationDescription = "Transfer of copie " + physicalBookId + " from library " + libraryAlias + " has been canceled.",
                EndDate = DateTime.Now.AddDays(1),
                ForAll = false,
                LibraryId = libraryId,
            };

            return notification;
        }
        
    }
}
