﻿// <auto-generated />
using System;
using GroupLibraryManagment.API.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GroupLibraryManagment.API.Persistence.Migrations
{
    [DbContext(typeof(GroupLibraryManagmentDbContext))]
    [Migration("20231121152821_UpdateRequestStatusEnum")]
    partial class UpdateRequestStatusEnum
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GroupLibraryManagment.API.Entities.Author", b =>
                {
                    b.Property<int>("AuthorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AuthorId"));

                    b.Property<string>("AuthorName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("AuthorName");

                    b.HasKey("AuthorId");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("GroupLibraryManagment.API.Entities.BookAuthor", b =>
                {
                    b.Property<string>("ISBN")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("AuthorId")
                        .HasColumnType("int");

                    b.HasKey("ISBN", "AuthorId");

                    b.HasIndex("AuthorId");

                    b.ToTable("BookAuthors");
                });

            modelBuilder.Entity("GroupLibraryManagment.API.Entities.BookCategory", b =>
                {
                    b.Property<string>("ISBN")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int");

                    b.HasKey("ISBN", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("BookCategories");
                });

            modelBuilder.Entity("GroupLibraryManagment.API.Entities.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"));

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("CategoryName");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("GroupLibraryManagment.API.Entities.Evaluation", b =>
                {
                    b.Property<int>("EvaluationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EvaluationId"));

                    b.Property<DateTime>("EmittedDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("EmittedDate");

                    b.Property<string>("EvaluationDescription")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("EvaluationDescription");

                    b.Property<int>("EvaluationScore")
                        .HasColumnType("int")
                        .HasColumnName("EvaluationScore");

                    b.Property<string>("ISBN")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("EvaluationId");

                    b.HasIndex("ISBN");

                    b.HasIndex("UserId");

                    b.ToTable("Evaluations");
                });

            modelBuilder.Entity("GroupLibraryManagment.API.Entities.GenericBook", b =>
                {
                    b.Property<string>("ISBN")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DatePublished")
                        .HasColumnType("datetime2")
                        .HasColumnName("DatePublished");

                    b.Property<string>("Description")
                        .HasMaxLength(2000)
                        .HasColumnType("varchar(2000)")
                        .HasColumnName("Description");

                    b.Property<int>("LanguageId")
                        .HasColumnType("int");

                    b.Property<int>("PageNumber")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0)
                        .HasColumnName("PageNumber");

                    b.Property<string>("SmallThumbnail")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("SmallThumbnail");

                    b.Property<string>("Thumbnail")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("Thumbnail");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("Title");

                    b.HasKey("ISBN")
                        .HasName("ISBN");

                    b.HasIndex("LanguageId");

                    b.ToTable("GenericBooks");
                });

            modelBuilder.Entity("GroupLibraryManagment.API.Entities.Language", b =>
                {
                    b.Property<int>("LanguageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LanguageId"));

                    b.Property<string>("LanguageAlias")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("LanguageAlias");

                    b.Property<string>("LanguageName")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("LanguageName");

                    b.HasKey("LanguageId");

                    b.ToTable("Languages");
                });

            modelBuilder.Entity("GroupLibraryManagment.API.Entities.Library", b =>
                {
                    b.Property<int>("LibraryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LibraryId"));

                    b.Property<string>("LibraryAddress")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("LibraryAddress");

                    b.Property<string>("LibraryAlias")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("LibraryAlias");

                    b.Property<string>("LibraryName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("LibraryName");

                    b.HasKey("LibraryId");

                    b.ToTable("Libraries");
                });

            modelBuilder.Entity("GroupLibraryManagment.API.Entities.Notification", b =>
                {
                    b.Property<int>("NotificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NotificationId"));

                    b.Property<DateTime>("EmittedDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("EmittedDate");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("EndDate");

                    b.Property<bool>("ForAll")
                        .HasColumnType("bit")
                        .HasColumnName("ForAll");

                    b.Property<int?>("LibraryId")
                        .HasColumnType("int");

                    b.Property<string>("NotificationDescription")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("NotificationDescription");

                    b.Property<string>("NotificationTitle")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("NotificationTitle");

                    b.Property<int?>("RequestId")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("NotificationId");

                    b.HasIndex("LibraryId");

                    b.HasIndex("RequestId");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("GroupLibraryManagment.API.Entities.PhysicalBook", b =>
                {
                    b.Property<int>("PhysicalBookId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PhysicalBookId"));

                    b.Property<bool>("Available")
                        .HasColumnType("bit")
                        .HasColumnName("Available");

                    b.Property<string>("ISBN")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("LibraryId")
                        .HasColumnType("int");

                    b.HasKey("PhysicalBookId");

                    b.HasIndex("ISBN");

                    b.HasIndex("LibraryId");

                    b.ToTable("PhysicalBooks");
                });

            modelBuilder.Entity("GroupLibraryManagment.API.Entities.Punishment", b =>
                {
                    b.Property<int>("PunishmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PunishmentId"));

                    b.Property<DateTime>("EmittedDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("EmittedDate");

                    b.Property<int>("PunishmentLevel")
                        .HasColumnType("int")
                        .HasColumnName("PunishmentLevel");

                    b.Property<string>("PunishmentReason")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("PunishmentReason");

                    b.Property<int>("RequestId")
                        .HasColumnType("int");

                    b.HasKey("PunishmentId");

                    b.HasIndex("RequestId")
                        .IsUnique();

                    b.ToTable("Punishments");
                });

            modelBuilder.Entity("GroupLibraryManagment.API.Entities.Request", b =>
                {
                    b.Property<int>("RequestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RequestId"));

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("EndDate");

                    b.Property<int>("PhysicalBookId")
                        .HasColumnType("int");

                    b.Property<int>("RequestStatus")
                        .HasColumnType("int")
                        .HasColumnName("RequestStatus");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("StartDate");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("RequestId");

                    b.HasIndex("PhysicalBookId");

                    b.HasIndex("UserId");

                    b.ToTable("Requests");
                });

            modelBuilder.Entity("GroupLibraryManagment.API.Entities.Transfer", b =>
                {
                    b.Property<int>("TransferId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TransferId"));

                    b.Property<int>("DestinationLibraryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("EndDate");

                    b.Property<int>("PhysicalBookId")
                        .HasColumnType("int");

                    b.Property<int>("SourceLibraryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("StartDate");

                    b.Property<int>("TransferStatus")
                        .HasColumnType("int")
                        .HasColumnName("TransferStatus");

                    b.HasKey("TransferId");

                    b.HasIndex("DestinationLibraryId");

                    b.HasIndex("PhysicalBookId");

                    b.HasIndex("SourceLibraryId");

                    b.ToTable("Transfers");
                });

            modelBuilder.Entity("GroupLibraryManagment.API.Entities.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<int?>("LibraryId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<string>("UserEmail")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("UserEmail");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("UserName");

                    b.Property<string>("UserPassword")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)")
                        .HasColumnName("UserPassword");

                    b.Property<int>("UserRole")
                        .HasColumnType("int")
                        .HasColumnName("UserRole");

                    b.HasKey("UserId");

                    b.HasIndex("LibraryId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("GroupLibraryManagment.API.Entities.BookAuthor", b =>
                {
                    b.HasOne("GroupLibraryManagment.API.Entities.Author", "Author")
                        .WithMany("BookAuthors")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("GroupLibraryManagment.API.Entities.GenericBook", "GenericBook")
                        .WithMany("BookAuthors")
                        .HasForeignKey("ISBN")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("GenericBook");
                });

            modelBuilder.Entity("GroupLibraryManagment.API.Entities.BookCategory", b =>
                {
                    b.HasOne("GroupLibraryManagment.API.Entities.Category", "Category")
                        .WithMany("BookCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("GroupLibraryManagment.API.Entities.GenericBook", "GenericBook")
                        .WithMany("BookCategories")
                        .HasForeignKey("ISBN")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("GenericBook");
                });

            modelBuilder.Entity("GroupLibraryManagment.API.Entities.Evaluation", b =>
                {
                    b.HasOne("GroupLibraryManagment.API.Entities.GenericBook", "GenericBook")
                        .WithMany("Evaluations")
                        .HasForeignKey("ISBN")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("GroupLibraryManagment.API.Entities.User", "User")
                        .WithMany("Evaluations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("GenericBook");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GroupLibraryManagment.API.Entities.GenericBook", b =>
                {
                    b.HasOne("GroupLibraryManagment.API.Entities.Language", "Language")
                        .WithMany("GenericBooks")
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Language");
                });

            modelBuilder.Entity("GroupLibraryManagment.API.Entities.Notification", b =>
                {
                    b.HasOne("GroupLibraryManagment.API.Entities.Library", "Library")
                        .WithMany()
                        .HasForeignKey("LibraryId");

                    b.HasOne("GroupLibraryManagment.API.Entities.Request", "Request")
                        .WithMany("Notifications")
                        .HasForeignKey("RequestId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("GroupLibraryManagment.API.Entities.User", "User")
                        .WithMany("Notifications")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Library");

                    b.Navigation("Request");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GroupLibraryManagment.API.Entities.PhysicalBook", b =>
                {
                    b.HasOne("GroupLibraryManagment.API.Entities.GenericBook", "GenericBook")
                        .WithMany("PhysicalBooks")
                        .HasForeignKey("ISBN")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("GroupLibraryManagment.API.Entities.Library", "Library")
                        .WithMany("PhysicalBooks")
                        .HasForeignKey("LibraryId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("GenericBook");

                    b.Navigation("Library");
                });

            modelBuilder.Entity("GroupLibraryManagment.API.Entities.Punishment", b =>
                {
                    b.HasOne("GroupLibraryManagment.API.Entities.Request", "Request")
                        .WithOne("Punishment")
                        .HasForeignKey("GroupLibraryManagment.API.Entities.Punishment", "RequestId");

                    b.Navigation("Request");
                });

            modelBuilder.Entity("GroupLibraryManagment.API.Entities.Request", b =>
                {
                    b.HasOne("GroupLibraryManagment.API.Entities.PhysicalBook", "PhysicalBook")
                        .WithMany("Requests")
                        .HasForeignKey("PhysicalBookId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("GroupLibraryManagment.API.Entities.User", "User")
                        .WithMany("Requests")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("PhysicalBook");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GroupLibraryManagment.API.Entities.Transfer", b =>
                {
                    b.HasOne("GroupLibraryManagment.API.Entities.Library", "DestinationLibrary")
                        .WithMany()
                        .HasForeignKey("DestinationLibraryId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("GroupLibraryManagment.API.Entities.PhysicalBook", "PhysicalBook")
                        .WithMany("Transfers")
                        .HasForeignKey("PhysicalBookId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("GroupLibraryManagment.API.Entities.Library", "SourceLibrary")
                        .WithMany("Transfers")
                        .HasForeignKey("SourceLibraryId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("DestinationLibrary");

                    b.Navigation("PhysicalBook");

                    b.Navigation("SourceLibrary");
                });

            modelBuilder.Entity("GroupLibraryManagment.API.Entities.User", b =>
                {
                    b.HasOne("GroupLibraryManagment.API.Entities.Library", "Library")
                        .WithMany("Users")
                        .HasForeignKey("LibraryId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Library");
                });

            modelBuilder.Entity("GroupLibraryManagment.API.Entities.Author", b =>
                {
                    b.Navigation("BookAuthors");
                });

            modelBuilder.Entity("GroupLibraryManagment.API.Entities.Category", b =>
                {
                    b.Navigation("BookCategories");
                });

            modelBuilder.Entity("GroupLibraryManagment.API.Entities.GenericBook", b =>
                {
                    b.Navigation("BookAuthors");

                    b.Navigation("BookCategories");

                    b.Navigation("Evaluations");

                    b.Navigation("PhysicalBooks");
                });

            modelBuilder.Entity("GroupLibraryManagment.API.Entities.Language", b =>
                {
                    b.Navigation("GenericBooks");
                });

            modelBuilder.Entity("GroupLibraryManagment.API.Entities.Library", b =>
                {
                    b.Navigation("PhysicalBooks");

                    b.Navigation("Transfers");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("GroupLibraryManagment.API.Entities.PhysicalBook", b =>
                {
                    b.Navigation("Requests");

                    b.Navigation("Transfers");
                });

            modelBuilder.Entity("GroupLibraryManagment.API.Entities.Request", b =>
                {
                    b.Navigation("Notifications");

                    b.Navigation("Punishment");
                });

            modelBuilder.Entity("GroupLibraryManagment.API.Entities.User", b =>
                {
                    b.Navigation("Evaluations");

                    b.Navigation("Notifications");

                    b.Navigation("Requests");
                });
#pragma warning restore 612, 618
        }
    }
}
