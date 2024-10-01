using GroupLibraryManagment.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace GroupLibraryManagment.API.Persistence
{
    public class GroupLibraryManagmentDbContext: DbContext
    {
        public GroupLibraryManagmentDbContext(DbContextOptions<GroupLibraryManagmentDbContext> options) : base(options) { }
        public DbSet<GenericBook> GenericBooks { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Library> Libraries { get; set; }
        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Punishment> Punishments { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<PhysicalBook> PhysicalBooks { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<GenericBook>(e =>
            {
                e.HasKey(e => e.ISBN).HasName("ISBN");

                e.Property(e => e.Title).IsRequired().HasMaxLength(255);

                e.Property(e => e.Description).IsRequired().HasMaxLength(2000);

                e.Property(e => e.PageNumber).HasColumnType("int").HasDefaultValue(0);

                e.HasOne(e => e.Language).WithMany(e => e.GenericBooks).HasForeignKey(e => e.LanguageId).OnDelete(DeleteBehavior.NoAction);

                e.Property(e => e.DatePublished).HasColumnType("date");

                e.Property(e => e.Thumbnail).IsRequired().HasMaxLength(255);

                e.Property(e => e.SmallThumbnail).IsRequired().HasMaxLength(255);
            });

            builder.Entity<Author>(e =>
            {
                e.HasKey(e => e.AuthorId);

                e.Property(e => e.AuthorName).IsRequired().HasMaxLength(100);
            });

            builder.Entity<BookAuthor>(e =>
            {
                e.HasKey(e => new { e.ISBN, e.AuthorId});

                e.HasOne(e => e.GenericBook)
                   .WithMany(e => e.BookAuthors)
                   .HasForeignKey(e => e.ISBN)
                   .OnDelete(DeleteBehavior.NoAction);

                e.HasOne(e => e.Author)
                   .WithMany(e => e.BookAuthors)
                   .HasForeignKey(e => e.AuthorId)
                   .OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<Category>(e =>
            {
                e.HasKey(e => e.CategoryId);

                e.Property(e => e.CategoryName).IsRequired().HasMaxLength(100);
            });

            builder.Entity<BookCategory>(e =>
            {
                e.HasKey(e => new { e.ISBN, e.CategoryId });

                e.HasOne(e => e.GenericBook)
                   .WithMany(e => e.BookCategories)
                   .HasForeignKey(e => e.ISBN)
                   .OnDelete(DeleteBehavior.NoAction);

                e.HasOne(e => e.Category)
                   .WithMany(e => e.BookCategories)
                   .HasForeignKey(e => e.CategoryId)
                   .OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<Language>(e =>
            {
                e.HasKey(e => e.LanguageId);

                e.Property(e => e.LanguageName).IsRequired(false).HasMaxLength(50);

                e.Property(e => e.LanguageAlias).IsRequired().HasMaxLength(10);
            });

            builder.Entity<Library>(e =>
            {
                e.HasKey(e => e.LibraryId);

                e.Property(e => e.LibraryName).IsRequired().HasMaxLength(100);

                e.Property(e => e.LibraryAlias).IsRequired().HasMaxLength(10);

                e.Property(e => e.LibraryAddress).IsRequired(false).HasMaxLength(100);
            });

            builder.Entity<Evaluation>(e =>
            {
                e.HasKey(e => e.EvaluationId);

                e.Property(e => e.EvaluationDescription).IsRequired().HasMaxLength(255);

                e.Property(e => e.EvaluationScore).HasColumnType("int");

                e.Property(e => e.EmittedDate).HasColumnType("date");

                e.HasOne(e => e.User).WithMany(e => e.Evaluations).HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.NoAction);

                e.HasOne(e => e.GenericBook).WithMany(e => e.Evaluations).HasForeignKey(e => e.ISBN).OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<User>(e =>
            {
                e.HasKey(e => e.UserId);

                e.Property(e => e.UserName).IsRequired().HasMaxLength(25);

                e.Property(e => e.UserEmail).IsRequired().HasMaxLength(100);

                e.Property(e => e.UserPassword).IsRequired().HasMaxLength(500);

                e.Property(e => e.RefreshToken).IsRequired().HasMaxLength(500);

                e.Property(e => e.TokenCreated).HasColumnType("date");
                
                e.Property(e => e.TokenExpires).HasColumnType("date");

                e.HasOne(e => e.Library).WithMany(e => e.Users).HasForeignKey(e => e.LibraryId).OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<Request>(e =>
            {
                e.HasKey(e => e.RequestId);

                e.Property(e => e.RequestStatus).HasColumnType("int");

                e.Property(e => e.StartDate).HasColumnType("date");

                e.Property(e => e.EndDate).HasColumnType("date");

                e.HasOne(e => e.User).WithMany(e => e.Requests).HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.NoAction);

                e.HasOne(e => e.GenericBook).WithMany(e => e.Requests).HasForeignKey(e => e.ISBN).OnDelete(DeleteBehavior.NoAction);

                e.HasOne(e => e.Punishment).WithOne(e => e.Request).HasForeignKey<Punishment>(e => e.RequestId).IsRequired(false);

                e.HasOne(e => e.PhysicalBook).WithMany(e => e.Requests).HasForeignKey(e => e.PhysicalBookId).OnDelete(DeleteBehavior.NoAction);

                e.HasOne(e => e.Library).WithMany(e => e.Requests).HasForeignKey(e => e.LibraryId).OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<Punishment>(e =>
            {
                e.HasKey(e => e.PunishmentId);

                e.Property(e => e.PunishmentReason).IsRequired().HasMaxLength(255);

                e.Property(e => e.PunishmentLevel).HasColumnType("int");

                e.Property(e => e.EmittedDate).HasColumnType("date");
            });

            builder.Entity<Transfer>(e =>
            {
                e.HasKey(e => e.TransferId);

                e.Property(e => e.TransferStatus).HasColumnType("int");

                e.Property(e => e.StartDate).HasColumnType("date");

                e.Property(e => e.EndDate).HasColumnType("date");

                e.HasOne(e => e.SourceLibrary).WithMany(e => e.Transfers).HasForeignKey(e => e.SourceLibraryId).OnDelete(DeleteBehavior.NoAction);

                e.HasOne(e => e.DestinationLibrary).WithMany().HasForeignKey(e => e.DestinationLibraryId).OnDelete(DeleteBehavior.NoAction);

                e.HasOne(e => e.PhysicalBook).WithMany(e => e.Transfers).HasForeignKey(e => e.PhysicalBookId).OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<Notification>(e =>
            {
                e.HasKey(e => e.NotificationId);

                e.Property(e => e.NotificationTitle).IsRequired().HasMaxLength(100);

                e.Property(e => e.NotificationDescription).IsRequired().HasMaxLength(255);

                e.Property(e => e.EmittedDate).HasColumnType("date");

                e.Property(e => e.EndDate).HasColumnType("date");

                e.Property(e => e.ForAll);

                e.HasOne(e => e.User).WithMany(e => e.Notifications).HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.NoAction);

                e.HasOne(e => e.Request).WithMany(e => e.Notifications).HasForeignKey(e => e.RequestId).OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<PhysicalBook>(e =>
            {
                e.HasKey(e => e.PhysicalBookId);

                e.Property(e => e.PhysicalBookStatus).HasColumnType("int");

                e.HasOne(e => e.Library).WithMany(e => e.PhysicalBooks).HasForeignKey(e => e.LibraryId).OnDelete(DeleteBehavior.NoAction);

                e.HasOne(e => e.GenericBook).WithMany(e => e.PhysicalBooks).HasForeignKey(e => e.ISBN).OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}
