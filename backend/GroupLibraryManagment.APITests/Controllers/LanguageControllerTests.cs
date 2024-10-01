using GroupLibraryManagment.API.Controllers;
using GroupLibraryManagment.API.DTOs;
using GroupLibraryManagment.API.Entities;
using GroupLibraryManagment.API.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace GroupLibraryManagment.APITests.Controllers
{
    [TestFixture]
    public class LanguageControllerTests
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
            var mockLanguages = new List<Language>
            {
                new Language { LanguageId = 1, LanguageAlias = "en", LanguageName = "English" },
                new Language { LanguageId = 2, LanguageAlias = "pt", LanguageName = "Portuguese" },
            };

            var mockGenericBooks = new List<GenericBook>
            {
                new GenericBook { ISBN = "1", LanguageId = 2 },
                new GenericBook { ISBN = "2", LanguageId = 2 },
            };

            _dbContext.GenericBooks.AddRange(mockGenericBooks);
            _dbContext.Languages.AddRange(mockLanguages);
            await _dbContext.SaveChangesAsync();
        }

        [TearDown]
        public void TearDown()
        {
            // Optionally dispose of the DbContext to release resources
            _dbContext.Dispose();
        }

        [Test]
        public async Task GetAllLanguages_ReturnsListOfLanguages()
        {
            // Arrange
            PopulateDatabase();
            var controller = new LanguageController(_dbContext);

            // Act
            var result = await controller.GetAllLanguages();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.InstanceOf<List<Language>>());

            var languages = okResult.Value as List<Language>;
            Assert.That(languages, Is.Not.Null);
            Assert.That(languages.Count, Is.EqualTo(_dbContext.Languages.Count()));
        }

        [Test]
        public async Task GetAllLanguages_ReturnsNoContentForEmptyList()
        {
            // Arrange
            // No need to seed the database for an empty list
            var controller = new LanguageController(_dbContext);

            // Act
            var result = await controller.GetAllLanguages();

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task GetLanguageById_ReturnsLanguage()
        {
            // Arrange
            PopulateDatabase();
            var controller = new LanguageController(_dbContext);

            // Act
            var result = await controller.GetLanguageById(1);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.InstanceOf<Language>());

            var language = okResult.Value as Language;
            Assert.That(language, Is.Not.Null);
            Assert.That(language.LanguageId, Is.EqualTo(1));
        }

        [Test]
        public async Task GetLanguageById_ReturnsNotFoundForNonExistingLanguage()
        {
            // Arrange
            PopulateDatabase();
            var controller = new LanguageController(_dbContext);

            // Act
            var result = await controller.GetLanguageById(3);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task GetLanguageByAlias_ReturnsLanguage()
        {
            // Arrange
            PopulateDatabase();
            var controller = new LanguageController(_dbContext);

            // Act
            var result = await controller.GetLanguageByAlias("en");

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.InstanceOf<Language>());

            var language = okResult.Value as Language;
            Assert.That(language, Is.Not.Null);
            Assert.That(language.LanguageAlias, Is.EqualTo("en"));
        }

        [Test]
        public async Task GetLanguageByAlias_ReturnsNotFoundForNonExistingLanguage()
        {
            // Arrange
            PopulateDatabase();
            var controller = new LanguageController(_dbContext);

            // Act
            var result = await controller.GetLanguageByAlias("fr");

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task CreateLanguage_ReturnsCreatedLanguage()
        {
            // Arrange
            var controller = new LanguageController(_dbContext);
            var languageDTO = new LanguageDTO
            {
                LanguageName = "French",
                LanguageAlias = "fr"
            };

            // Act
            var result = await controller.CreateLanguage(languageDTO);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = result as CreatedAtActionResult;
            Assert.That(createdResult.Value, Is.InstanceOf<Language>());

            var language = createdResult.Value as Language;
            Assert.That(language, Is.Not.Null);
            Assert.That(language.LanguageName, Is.EqualTo("French"));
            Assert.That(language.LanguageAlias, Is.EqualTo("fr"));
        }

        [Test]
        public async Task CreateLanguage_ReturnsBadRequestForInvalidInput()
        {
            // Arrange
            PopulateDatabase();
            var controller = new LanguageController(_dbContext);
            var languageDTO = new LanguageDTO
            {
                LanguageName = "English",
                LanguageAlias = "en"
            };

            // Act
            var result = await controller.CreateLanguage(languageDTO);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task UpdateLanguage_ReturnsUpdatedLanguage()
        {
            // Arrange
            PopulateDatabase();
            var controller = new LanguageController(_dbContext);
            var languageDTO = new LanguageDTO
            {
                LanguageName = "French",
                LanguageAlias = "fr"
            };

            // Act
            var result = await controller.UpdateLanguage(1, languageDTO);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var okResult = result as CreatedAtActionResult;
            Assert.That(okResult.Value, Is.InstanceOf<Language>());

            var language = okResult.Value as Language;
            Assert.That(language, Is.Not.Null);
            Assert.That(language.LanguageName, Is.EqualTo("French"));
            Assert.That(language.LanguageAlias, Is.EqualTo("fr"));
        }

        [Test]
        public async Task UpdateLanguage_ReturnsBadRequestForInvalidInput()
        {
            // Arrange
            PopulateDatabase();
            var controller = new LanguageController(_dbContext);
            var languageDTO = new LanguageDTO
            {
                LanguageName = "English",
                LanguageAlias = "en"
            };

            // Act
            var result = await controller.UpdateLanguage(2, languageDTO);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task UpdateLanguage_ReturnsNotFoundForNonExistingLanguage()
        {
            // Arrange
            PopulateDatabase();
            var controller = new LanguageController(_dbContext);
            var languageDTO = new LanguageDTO
            {
                LanguageName = "English",
                LanguageAlias = "en"
            };

            // Act
            var result = await controller.UpdateLanguage(3, languageDTO);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task DeleteLanguage_ReturnsNoContent()
        {
            // Arrange
            PopulateDatabase();
            var controller = new LanguageController(_dbContext);

            // Act
            var result = await controller.DeleteLanguage(1);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task DeleteLanguage_ReturnsNotFoundForNonExistingLanguage()
        {
            // Arrange
            PopulateDatabase();
            var controller = new LanguageController(_dbContext);

            // Act
            var result = await controller.DeleteLanguage(3);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task DeleteLanguage_ReturnsBadRequestForLanguageWithAssociatedBooks()
        {
            // Arrange
            PopulateDatabase();
            var controller = new LanguageController(_dbContext);

            // Act
            var result = await controller.DeleteLanguage(2);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

    }
}
