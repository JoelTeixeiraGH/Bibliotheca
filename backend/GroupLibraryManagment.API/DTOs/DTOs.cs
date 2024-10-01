using GroupLibraryManagment.API.Entities;
using System.ComponentModel.DataAnnotations;

namespace GroupLibraryManagment.API.DTOs
{
    public class AuthorDTO
    {
        public string AuthorName { get; set; } = string.Empty;
    }
    public class CategoryDTO
    {
        public string CategoryName { get; set; } = string.Empty;
    }
    public class EvaluationDTO
    {
        public string EvaluationDescription { get; set; } = string.Empty;
        public EvaluationScore EvaluationScore { get; set; }
        public int UserId { get; set; }
        public string ISBN { get; set; } = string.Empty;
    }
    public class GenericBookDTO
    {
        public string ISBN { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int PageNumber { get; set; }
        public string LanguageAlias { get; set; } = string.Empty;
        public DateTime DatePublished { get; set; }
        public string[] AuthorsNames { get; set; }
        public string[] CategoriesNames { get; set; }
        public string Thumbnail { get; set; } = string.Empty;
        public string SmallThumbnail { get; set; } = string.Empty;
    }
    public class LanguageDTO
    {
        public string? LanguageName { get; set; }
        public string LanguageAlias { get; set; } = string.Empty;
    }
    public class LibraryDTO
    {
        public string? LibraryName { get; set; }
        public string LibraryAlias { get; set; } = string.Empty;
        public string? LibraryAddress { get; set; }
    }
    public class NotificationDTO
    {
        public string NotificationTitle { get; set; } = string.Empty;
        public string NotificationDescription { get; set; } = string.Empty;
        public DateTime? EndDate { get; set; }
        public int? LibraryId { get; set; }
    }
    public class PhysicalBookDTO
    {
        public int LibraryId { get; set; }
        public string ISBN { get; set; } = string.Empty;

    }
    public class PunishmentDTO
    {
        public string PunishmentReason { get; set; } = string.Empty;
        public int RequestId { get; set; }
    }
    public class RequestDTO
    {
        public DateTime? EndDate { get; set; }
        public int UserId { get; set; }
        public int? PhysicalBookId { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public int LibraryId { get; set; }
    }
    public class TransferDTO
    {
        public DateTime EndDate { get; set; }
        public int SourceLibraryId { get; set; }
        public int DestinationLibraryId { get; set; }
        public int PhysicalBookId { get; set; }
    }
    public class UserRegisterDTO
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string UserEmail { get; set; } = string.Empty;
        [Required]
        public string UserPassword { get; set; } = string.Empty;
        [Required]
        public int LibraryId { get; set; }
    }
    public class UserLoginDTO
    {
        [Required]
        public string UserEmail { get; set; } = string.Empty;
        [Required]
        public string UserPassword { get; set; } = string.Empty;
    }
    public class UserUpdateDTO
    {
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public int LibraryId { get; set; }
    }
    public class UserUpdatePasswordDTO
    {
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
