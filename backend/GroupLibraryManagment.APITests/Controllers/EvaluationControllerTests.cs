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
    public class EvaluationControllerTests
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
            var mockEvaluations = new List<Evaluation>
            {
                new Evaluation { EvaluationId = 1, EvaluationDescription = "Description1", EvaluationScore = EvaluationScore.One, ISBN = "1", UserId = 1 },
                new Evaluation { EvaluationId = 2, EvaluationDescription = "Description2", EvaluationScore = EvaluationScore.Five, ISBN = "2", UserId = 2 }
            };

            var mockGenericBooks = new List<GenericBook>
            {
                new GenericBook { ISBN = "1", Title = "Title1", Description = "Description1", PageNumber = 1, LanguageId = 1, DatePublished = DateTime.Now, Thumbnail = "Thumbnail1", SmallThumbnail = "SmallThumbnail1" },
                new GenericBook { ISBN = "2", Title = "Title2", Description = "Description2", PageNumber = 2, LanguageId = 2, DatePublished = DateTime.Now, Thumbnail = "Thumbnail2", SmallThumbnail = "SmallThumbnail2" }
            };

            var mockUsers = new List<User>
            {
                new User { UserId = 1, UserName = "FirstName1", UserEmail = "Email1", UserPassword = "Password1", UserRole = UserRole.Reader },
                new User { UserId = 2, UserName = "FirstName2", UserEmail = "Email2", UserPassword = "Password2", UserRole = UserRole.Reader }
            };
            
            _dbContext.GenericBooks.AddRange(mockGenericBooks);
            _dbContext.Users.AddRange(mockUsers);
            _dbContext.Evaluations.AddRange(mockEvaluations);
            await _dbContext.SaveChangesAsync();
        }

        [TearDown]
        public void TearDown()
        {
            // Optionally dispose of the DbContext to release resources
            _dbContext.Dispose();
        }

        [Test]
        public async Task GetAllEvaluations_EvaluationsExist_ReturnsOkResult()
        {
            // Arrange
            PopulateDatabase();
            var controller = new EvaluationController(_dbContext);

            // Act
            var result = await controller.GetAllEvaluations();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
        }

        [Test]
        public async Task GetAllEvaluations_NoEvaluations_ReturnsNoContent()
        {
            // Arrange
            var controller = new EvaluationController(_dbContext);

            // Act
            var result = await controller.GetAllEvaluations();

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task GetEvaluationById_ExistingEvaluation_ReturnsOkResult()
        {
            // Arrange
            PopulateDatabase();
            var controller = new EvaluationController(_dbContext);
            var existingEvaluationId = 1;

            // Act
            var result = await controller.GetEvaluationById(existingEvaluationId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
        }

        [Test]
        public async Task GetEvaluationById_NonExistingEvaluation_ReturnsNotFound()
        {
            // Arrange
            PopulateDatabase();
            var controller = new EvaluationController(_dbContext);
            var nonExistingEvaluationId = 999;

            // Act
            var result = await controller.GetEvaluationById(nonExistingEvaluationId);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task GetAllEvaluationByISBN_ExistingEvaluation_ReturnsOkResult()
        {
            // Arrange
            PopulateDatabase();
            var controller = new EvaluationController(_dbContext);
            var existingEvaluationISBN = "1";

            // Act
            var result = await controller.GetAllEvaluationsByISBN(existingEvaluationISBN);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
        }

        [Test]
        public async Task GetAllEvaluationByISBN_NonExistingEvaluation_ReturnsNoContent()
        {
            // Arrange
            PopulateDatabase();
            var controller = new EvaluationController(_dbContext);
            var nonExistingEvaluationISBN = "999";

            // Act
            var result = await controller.GetAllEvaluationsByISBN(nonExistingEvaluationISBN);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task GetAllEvaluationsByUserId_ExistingEvaluation_ReturnsOkResult()
        {
            // Arrange
            PopulateDatabase();
            var controller = new EvaluationController(_dbContext);
            var existingEvaluationUserId = 1;

            // Act
            var result = await controller.GetAllEvaluationsByUserId(existingEvaluationUserId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
        }

        [Test]
        public async Task GetAllEvaluationsByUserId_NonExistingEvaluation_ReturnsNoContent()
        {
            // Arrange
            PopulateDatabase();
            var controller = new EvaluationController(_dbContext);
            var nonExistingEvaluationUserId = 999;

            // Act
            var result = await controller.GetAllEvaluationsByUserId(nonExistingEvaluationUserId);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task CreateEvaluation_ValidInput_ReturnsCreatedAtActionResult()
        {
            // Arrange
            PopulateDatabase();
            var controller = new EvaluationController(_dbContext);
            var validEvaluation = new EvaluationDTO { EvaluationDescription = "Description3", EvaluationScore = EvaluationScore.Five, ISBN = "1", UserId = 2 };

            // Act
            var result = await controller.CreateEvaluation(validEvaluation);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.That(createdAtActionResult, Is.Not.Null);
        }

        [Test]
        public async Task CreateEvaluation_InvalidInput_ReturnsBadRequest()
        {
            // Arrange
            PopulateDatabase();
            var controller = new EvaluationController(_dbContext);
            var invalidEvaluation = new EvaluationDTO { EvaluationDescription = "Description3", EvaluationScore = EvaluationScore.Five, ISBN = "1", UserId = 999 };

            // Act
            var result = await controller.CreateEvaluation(invalidEvaluation);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task UpdateEvaluation_ExistingEvaluation_ReturnsCreatedAtActionResult()
        {
            // Arrange
            PopulateDatabase();
            var controller = new EvaluationController(_dbContext);
            var existingEvaluationId = 1;
            var updatedEvaluation = new EvaluationDTO { EvaluationDescription = "Description3", EvaluationScore = EvaluationScore.Five, ISBN = "1", UserId = 1 };

            // Act
            var result = await controller.UpdateEvaluation(existingEvaluationId, updatedEvaluation);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
        }

        [Test]
        public async Task UpdateEvaluation_NonExistingEvaluation_ReturnsNotFound()
        {
            // Arrange
            PopulateDatabase();
            var controller = new EvaluationController(_dbContext);
            var nonExistingEvaluationId = 999;
            var updatedEvaluation = new EvaluationDTO { EvaluationDescription = "Description3", EvaluationScore = EvaluationScore.Five, ISBN = "1", UserId = 1 };

            // Act
            var result = await controller.UpdateEvaluation(nonExistingEvaluationId, updatedEvaluation);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task DeleteEvaluation_ExistingEvaluation_ReturnsNoContent()
        {
            // Arrange
            PopulateDatabase();
            var controller = new EvaluationController(_dbContext);
            var existingEvaluationId = 1;

            // Act
            var result = await controller.DeleteEvaluation(existingEvaluationId);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task DeleteEvaluation_NonExistingEvaluation_ReturnsNotFound()
        {
            // Arrange
            PopulateDatabase();
            var controller = new EvaluationController(_dbContext);
            var nonExistingEvaluationId = 999;

            // Act
            var result = await controller.DeleteEvaluation(nonExistingEvaluationId);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}
