using GroupLibraryManagment.API.Controllers;
using GroupLibraryManagment.API.DTOs;
using GroupLibraryManagment.API.Entities;
using GroupLibraryManagment.API.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace GroupLibraryManagment.APITests.Controllers
{
    public class GenericBookControllerTests
    {
        private GroupLibraryManagmentDbContext _dbContext;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<GroupLibraryManagmentDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new GroupLibraryManagmentDbContext(options);

            // Ensure the database is deleted and recreated to start with an empty state
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();
        }

        public async void PopulateDatabase()
        {
            var mockGenericBooks = new List<GenericBook>
            {
                new GenericBook { ISBN = "1", Title = "Title1", Description = "Description1", PageNumber = 1, LanguageId = 1, DatePublished = DateTime.Now, Thumbnail = "Thumbnail1", SmallThumbnail = "SmallThumbnail1" },
            };

            var mockLanguages = new List<Language>
            {
                new Language { LanguageName = "English", LanguageAlias = "en" },
                new Language { LanguageName = "French", LanguageAlias = "fr" }
            };

            var mockAuthors = new List<Author>
            {
                new Author { AuthorName = "Author1"},
                new Author { AuthorName = "Author2"},
            };

            var mockCategories = new List<Category>
            {
                new Category { CategoryName = "Category1"},
                new Category { CategoryName = "Category2"},
            };

            var associatedBookAuthor = new BookAuthor { AuthorId = 1, ISBN = "1" };
            var associatedBookCategory = new BookCategory { CategoryId = 1, ISBN = "1" };

            _dbContext.GenericBooks.AddRange(mockGenericBooks);
            _dbContext.Languages.AddRange(mockLanguages);
            _dbContext.Authors.AddRange(mockAuthors);
            _dbContext.Categories.AddRange(mockCategories);
            _dbContext.BookAuthors.Add(associatedBookAuthor);
            _dbContext.BookCategories.Add(associatedBookCategory);
            await _dbContext.SaveChangesAsync();
        }

        [TearDown]
        public void TearDown()
        {
            // Optionally dispose of the DbContext to release resources
            _dbContext.Dispose();
        }

        [Test]
        public async Task GetAllGenericBooks_ReturnsListOfGenericBooks()
        {
            // Arrange
            PopulateDatabase();
            var controller = new GenericBookController(_dbContext);

            // Act
            var result = await controller.GetAllGenericBooks();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
        }

        [Test]
        public async Task GetAllGenericBooks_ReturnsNoContentForEmptyList()
        {
            // Arrange
            // No need to seed the database for an empty list
            var controller = new GenericBookController(_dbContext);

            // Act
            var result = await controller.GetAllGenericBooks();

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task GetGenericBookByISBN_ExistingGenericBook_ReturnsOkResult()
        {
            // Arrange
            PopulateDatabase();
            var controller = new GenericBookController(_dbContext);
            var existingGenericBookISBN = "1";

            // Act
            var result = await controller.GetGenericBookByISBN(existingGenericBookISBN);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
        }

        [Test]
        public async Task GetGenericBookByISBN_NonExistingGenericBook_ReturnsNotFoundResult()
        {
            // Arrange
            PopulateDatabase();
            var controller = new GenericBookController(_dbContext);
            var nonExistingGenericBookISBN = "2";

            // Act
            var result = await controller.GetGenericBookByISBN(nonExistingGenericBookISBN);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task GetAllGenericBooksByMostPopular_ReturnsListOfGenericBooks()
        {
            // Arrange
            PopulateDatabase();
            var controller = new GenericBookController(_dbContext);

            // Act
            var result = await controller.GetAllGenericBooksByMostPopular();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
        }

        [Test]
        public async Task GetAllGenericBooksByMostPopular_ReturnsNoContentForEmptyList()
        {
            // Arrange
            // No need to seed the database for an empty list
            var controller = new GenericBookController(_dbContext);

            // Act
            var result = await controller.GetAllGenericBooksByMostPopular();

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task GetGenericBookByGoogleAPI_ReturnsOkResult()
        {
            // Arrange
            PopulateDatabase();
            var controller = new GenericBookController(_dbContext);
            var googleISBN = "9781529018592";

            // Act
            var result = await controller.GetGenericBookByGoogleAPI(googleISBN);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            
        }

        [Test]
        public async Task GetGenericBookByGoogleAPI_ReturnsNotFoundResult()
        {
            // Arrange
            PopulateDatabase();
            var controller = new GenericBookController(_dbContext);
            var googleISBN = "1";

            // Act
            var result = await controller.GetGenericBookByGoogleAPI(googleISBN);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task GetAllGenericBooksByCategoryId_ReturnsListOfGenericBooks()
        {
            // Arrange
            PopulateDatabase();
            var controller = new GenericBookController(_dbContext);
            var categoryId = 1;

            // Act
            var result = await controller.GetAllGenericBooksByCategoryId(categoryId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
        }

        [Test]
        public async Task GetAllGenericBooksByCategoryId_ReturnsNoContentForEmptyList()
        {
            // Arrange
            // No need to seed the database for an empty list
            var controller = new GenericBookController(_dbContext);
            var categoryId = 1;

            // Act
            var result = await controller.GetAllGenericBooksByCategoryId(categoryId);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task GetAllGenericBooksByAuthorId_ReturnsListOfGenericBooks()
        {
            // Arrange
            PopulateDatabase();
            var controller = new GenericBookController(_dbContext);
            var authorId = 1;

            // Act
            var result = await controller.GetAllGenericBooksByAuthorId(authorId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
        }

        [Test]
        public async Task GetAllGenericBooksByAuthorId_ReturnsNoContentForEmptyList()
        {
            // Arrange
            // No need to seed the database for an empty list
            var controller = new GenericBookController(_dbContext);
            var authorId = 1;

            // Act
            var result = await controller.GetAllGenericBooksByAuthorId(authorId);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task GetAllGenericBooksByLanguageId_ReturnsListOfGenericBooks()
        {
            // Arrange
            PopulateDatabase();
            var controller = new GenericBookController(_dbContext);
            var languageId = 1;

            // Act
            var result = await controller.GetAllGenericBooksByLanguageId(languageId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

        }

        [Test]
        public async Task GetAllGenericBooksByLanguageId_ReturnsNoContentForEmptyList()
        {
            // Arrange
            // No need to seed the database for an empty list
            var controller = new GenericBookController(_dbContext);
            var languageId = 1;

            // Act
            var result = await controller.GetAllGenericBooksByLanguageId(languageId);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task GetAllGenericBooksBySearch_ReturnsListOfGenericBooks()
        {
            // Arrange
            PopulateDatabase();
            var controller = new GenericBookController(_dbContext);
            var search = "Title1";

            // Act
            var result = await controller.SearchGenericBooks(search);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

        }

        [Test]
        public async Task GetAllGenericBooksBySearch_ReturnsNoContentForEmptyList()
        {
            // Arrange
            // No need to seed the database for an empty list
            var controller = new GenericBookController(_dbContext);
            var search = "Title1";

            // Act
            var result = await controller.SearchGenericBooks(search);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task CreateGenericBook_ReturnsCreatedResult()
        {
            // Arrange
            PopulateDatabase();
            var controller = new GenericBookController(_dbContext);
            var genericBook = new GenericBookDTO { 
                ISBN = "2",
                Title = "Title2",
                Description = "Description2",
                PageNumber = 2,
                LanguageAlias = "en",
                DatePublished = DateTime.Now,
                Thumbnail = "Thumbnail2",
                SmallThumbnail = "SmallThumbnail2",
                AuthorsNames = new string[] { "author1" }, 
                CategoriesNames = new string[] { "category1" },
            };

            // Act
            var result = await controller.CreateGenericBook(genericBook);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = result as CreatedAtActionResult;
            Assert.That(createdResult, Is.Not.Null);

            var createdGenericBook = createdResult.Value as GenericBook;
            Assert.That(createdGenericBook, Is.Not.Null);
            Assert.That(createdGenericBook.ISBN, Is.EqualTo(genericBook.ISBN));
        }

        [Test]
        public async Task CreateGenericBook_ReturnsBadRequestResult()
        {
            // Arrange
            PopulateDatabase();
            var controller = new GenericBookController(_dbContext);
            var genericBook = new GenericBookDTO { ISBN = "1", Title = "Title1", Description = "Description1", PageNumber = 1, LanguageAlias = "en", DatePublished = DateTime.Now, Thumbnail = "Thumbnail1", SmallThumbnail = "SmallThumbnail1" };

            // Act
            var result = await controller.CreateGenericBook(genericBook);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task UpdateGenericBook_ReturnsNotFoundResult()
        {
            // Arrange
            PopulateDatabase();
            var controller = new GenericBookController(_dbContext);
            var genericBook = new GenericBookDTO { ISBN = "999", Title = "Title1", Description = "Description1", PageNumber = 1, LanguageAlias = "en", DatePublished = DateTime.Now, Thumbnail = "Thumbnail1", SmallThumbnail = "SmallThumbnail1" };

            // Act
            var result = await controller.UpdateGenericBook(genericBook);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task UpdateGenericBook_ReturnsCreatedResult()
        {
            // Arrange
            PopulateDatabase();
            var controller = new GenericBookController(_dbContext);
            var genericBook = new GenericBookDTO
            {
                ISBN = "1",
                Title = "Title1",
                Description = "Description1",
                PageNumber = 45,
                LanguageAlias = "en",
                DatePublished = DateTime.Now,
                Thumbnail = "Thumbnail1",
                SmallThumbnail = "SmallThumbnail1",
                AuthorsNames = new string[] { "author1" },
                CategoriesNames = new string[] { "category1" },
            };

            // Act
            var result = await controller.UpdateGenericBook(genericBook);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = result as CreatedAtActionResult;
            Assert.That(createdResult, Is.Not.Null);

        }

        [Test]
        public async Task DeleteGenericBook_ReturnsNotFoundResult()
        {
            // Arrange
            PopulateDatabase();
            var controller = new GenericBookController(_dbContext);
            var nonExistingGenericBookISBN = "2";

            // Act
            var result = await controller.DeleteGenericBook(nonExistingGenericBookISBN);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task DeleteGenericBook_ReturnsNoContentResult()
        {
            // Arrange
            PopulateDatabase();
            var controller = new GenericBookController(_dbContext);
            var existingGenericBookISBN = "1";

            // Act
            var result = await controller.DeleteGenericBook(existingGenericBookISBN);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }
    }
}