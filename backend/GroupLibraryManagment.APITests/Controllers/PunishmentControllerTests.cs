using GroupLibraryManagment.API.Controllers;
using GroupLibraryManagment.API.DTOs;
using GroupLibraryManagment.API.Entities;
using GroupLibraryManagment.API.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace GroupLibraryManagment.APITests.Controllers
{
    [TestFixture]
    public class PunishmentControllerTests
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
            var mockPunishments = new List<Punishment>
            {
                new Punishment { PunishmentReason = "Failed to deliver in time", RequestId = 1},
                new Punishment { PunishmentReason = "Failed to deliver in time", RequestId = 2},
            };

            var mockRequests = new List<Request>
            {
                new Request { RequestId = 1, UserId = 1, ISBN = "1", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), RequestStatus = RequestStatus.NotReturned },
                new Request { RequestId = 2, UserId = 2, ISBN = "2", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), RequestStatus = RequestStatus.NotReturned },
                new Request { RequestId = 3, UserId = 1, ISBN = "1", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), RequestStatus = RequestStatus.NotReturned }
            };

            var mockGenericBooks = new List<GenericBook>
            {
                new GenericBook { ISBN = "1", Title = "Title1", Description = "Description1", PageNumber = 100, LanguageId = 1, DatePublished = DateTime.Now, Thumbnail = "Thumbnail1", SmallThumbnail = "SmallThumbnail1" },
                new GenericBook { ISBN = "2", Title = "Title2", Description = "Description2", PageNumber = 200, LanguageId = 1, DatePublished = DateTime.Now, Thumbnail = "Thumbnail2", SmallThumbnail = "SmallThumbnail2" },
            };

            var mockLanguages = new List<Language>
            {
                new Language { LanguageId = 1, LanguageName = "English", LanguageAlias = "en" },
                new Language { LanguageId = 2, LanguageName = "Romanian", LanguageAlias = "ro" },
            };

            var mockUsers = new List<User>
            {
                new User { UserId = 1, UserName = "FirstName1", UserEmail = "Email1", UserPassword = "Password1", UserRole = UserRole.Reader },
                new User { UserId = 2, UserName = "FirstName2", UserEmail = "Email2", UserPassword = "Password2", UserRole = UserRole.Reader }
            };

            var mockLibraries = new List<Library>
            {
                new Library { LibraryId = 1, LibraryName = "LibraryName1", LibraryAlias = "LibraryAlias1", LibraryAddress = "LibraryAddress1" },
                new Library { LibraryId = 2, LibraryName = "LibraryName2", LibraryAlias = "LibraryAlias2", LibraryAddress = "LibraryAddress2" }
            };

            _dbContext.Libraries.AddRange(mockLibraries);
            _dbContext.Users.AddRange(mockUsers);
            _dbContext.Languages.AddRange(mockLanguages);
            _dbContext.GenericBooks.AddRange(mockGenericBooks);
            _dbContext.Punishments.AddRange(mockPunishments);
            _dbContext.Requests.AddRange(mockRequests);
            await _dbContext.SaveChangesAsync();
        }

        [TearDown]
        public void TearDown()
        {
            // Optionally dispose of the DbContext to release resources
            _dbContext.Dispose();
        }

        [Test]
        public async Task GetAllPunishments_ReturnsAllPunishments()
        {
            // Arrange
            PopulateDatabase();
            var controller = new PunishmentController(_dbContext);

            // Act
            var result = await controller.GetAllPunishments();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
        }

        [Test]
        public async Task GetAllPunishments_ReturnsNoContentForEmptyList()
        {
            // Arrange
            // No need to seed the database for an empty list
            var controller = new PunishmentController(_dbContext);

            // Act
            var result = await controller.GetAllPunishments();

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task GetAllPunishmentsByUserId_ReturnsAllPunishments()
        {
            // Arrange
            PopulateDatabase();
            var controller = new PunishmentController(_dbContext);

            // Act
            var result = await controller.GetAllPunishmentsByUserId(1);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
        }

        [Test]
        public async Task GetAllPunishmentsByUserId_ReturnsNoContentForEmptyList()
        {
            // Arrange
            // No need to seed the database for an empty list
            var controller = new PunishmentController(_dbContext);

            // Act
            var result = await controller.GetAllPunishmentsByUserId(1);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task GetPunishmentById_ReturnsPunishment()
        {
            // Arrange
            PopulateDatabase();
            var controller = new PunishmentController(_dbContext);

            // Act
            var result = await controller.GetPunishmentById(1);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
        }

        [Test]
        public async Task GetPunishmentById_ReturnsNotFoundForInvalidId()
        {
            // Arrange
            PopulateDatabase();
            var controller = new PunishmentController(_dbContext);

            // Act
            var result = await controller.GetPunishmentById(3);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task GetPunishmentByRequestId_ReturnsPunishment()
        {
            // Arrange
            PopulateDatabase();
            var controller = new PunishmentController(_dbContext);

            // Act
            var result = await controller.GetPunishmentByRequestId(1);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
        }

        [Test]
        public async Task GetPunishmentByRequestId_ReturnsNoContent()
        {
            // Arrange
            PopulateDatabase();
            var controller = new PunishmentController(_dbContext);

            // Act
            var result = await controller.GetPunishmentByRequestId(3);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task CreatePunishment_ReturnsCreatedPunishment()
        {
            // Arrange
            PopulateDatabase();
            var controller = new PunishmentController(_dbContext);
            var punishment = new PunishmentDTO { PunishmentReason = "Failed to deliver in time", RequestId = 3 };

            // Act
            var result = await controller.CreatePunishment(punishment);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = result as CreatedAtActionResult;
            Assert.That(createdResult, Is.Not.Null);
        }

        [Test]
        public async Task CreatePunishment_ReturnsBadRequest()
        {
            // Arrange
            PopulateDatabase();
            var controller = new PunishmentController(_dbContext);
            var punishment = new PunishmentDTO { PunishmentReason = "Failed to deliver in time", RequestId = 1 };

            // Act
            var result = await controller.CreatePunishment(punishment);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task UpdatePunishment_ReturnsCreatedPunishment()
        {
            // Arrange
            PopulateDatabase();
            var controller = new PunishmentController(_dbContext);
            var punishment = new PunishmentDTO { PunishmentReason = "Failed to deliver in time", RequestId = 1 };

            // Act
            var result = await controller.UpdatePunishment(1, punishment);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
        }

        [Test]
        public async Task UpdatePunishment_ReturnsNotFound()
        {
            // Arrange
            PopulateDatabase();
            var controller = new PunishmentController(_dbContext);
            var punishment = new PunishmentDTO { PunishmentReason = "Failed to deliver in time", RequestId = 1 };

            // Act
            var result = await controller.UpdatePunishment(3, punishment);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task DeletePunishment_ReturnsNoContent()
        {
            // Arrange
            PopulateDatabase();
            var controller = new PunishmentController(_dbContext);

            // Act
            var result = await controller.DeletePunishment(1);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task DeletePunishment_ReturnsNotFoundForInvalidId()
        {
            // Arrange
            PopulateDatabase();
            var controller = new PunishmentController(_dbContext);

            // Act
            var result = await controller.DeletePunishment(3);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

    }
}
