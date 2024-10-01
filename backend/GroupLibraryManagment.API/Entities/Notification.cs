using GroupLibraryManagment.API.DTOs;
using System.Text.Json.Serialization;

namespace GroupLibraryManagment.API.Entities
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
        public static Notification CreateGlobalNotification(NotificationDTO input)
        {
            var notification = new Notification
            {
                NotificationTitle = input.NotificationTitle,
                NotificationDescription = input.NotificationDescription,
                EndDate = input.EndDate,
                ForAll = true,
                LibraryId = input.LibraryId,
            };

            return notification;
        }
        public static Notification CreateNotificationForEnteringWaitingList(int userId, int requestId, int libraryId, string isbn)
        {
            var notification = new Notification
            {
                NotificationTitle = "Book Transfer",
                NotificationDescription = "User of ID " + userId + " entered waiting list for book " + isbn + " . Transfer copie from another library?",
                EndDate = DateTime.Now.AddDays(3),
                ForAll = false,
                RequestId = requestId,
                LibraryId = libraryId,
            };

            return notification;
        }
        public static Notification CreateNotificationForLibraryForTransfer(int libraryId, string libraryAlias, DateTime endDate)
        {
            var notification = new Notification
            {
                NotificationTitle = "Book transfer",
                NotificationDescription = "Library " + libraryAlias + " requested a transfer.",
                EndDate = endDate,
                LibraryId = libraryId,
                UserId = null,
                RequestId = null,
            };

            return notification;
        }
        public static Notification CreateNotificationForBookArrival(int libraryId)
        {
            var notification = new Notification
            {
                NotificationTitle = "Book transfer",
                NotificationDescription = "Book is arriving from a transfer. Don't forget to change the status of the book when it arrives.",
                EndDate = DateTime.Now.AddDays(3),
                ForAll = false,
                LibraryId = libraryId,
            };

            return notification;
        }
        public static Notification CreateNotificationForEndRequestEminent(int requestId, string isbn, DateTime endDate)
        {
            var notification = new Notification
            {
                NotificationTitle = "Return Notice",
                NotificationDescription = "End date for request of book " + isbn + " eminent.",
                EndDate = endDate,
                ForAll = false,
                RequestId = requestId,
            };

            return notification;
        }
        public static Notification CreateNotificationForPunishment(int requestId, string punishmentReason)
        {
            var notification = new Notification
            {
                NotificationTitle = "Punishment",
                NotificationDescription = "Punishment issued for request " + requestId + " for reason: " + punishmentReason,
                EndDate = DateTime.Now.AddDays(1),
                ForAll = false,
                RequestId = requestId,
            };

            return notification;
        }
        public static Notification CreateNotificationForRequest(int requestId, string isbn)
        {
            var notification = new Notification
            {
                NotificationTitle = "Request",
                NotificationDescription = "Request for book " + isbn + " has been issued.",
                EndDate = DateTime.Now.AddDays(3),
                ForAll = false,
                RequestId = requestId,
            };

            return notification;
        }
        public void Update(NotificationDTO input)
        {
            NotificationTitle = input.NotificationTitle;
            NotificationDescription = input.NotificationDescription;
        }

    }
}
