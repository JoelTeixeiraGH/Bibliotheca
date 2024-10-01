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
    public class LibraryControllerTests
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
            var mockLibraries = new List<Library>
            {
                new Library { LibraryId = 1, LibraryAlias = "ESTG", LibraryName = "Escola de tecnologia e gestao", LibraryAddress = "Felgueiras" },
                new Library { LibraryId = 2, LibraryAlias = "ISEP", LibraryName = "DONT KNOW", LibraryAddress = "Porto" },
            };

            var mockUsers = new List<User>
            {
                new User { UserId = 1, UserName = "João", LibraryId = 2 },
                new User { UserId = 2, UserName = "Pedro", LibraryId = 2 },
            };

            _dbContext.Users.AddRange(mockUsers);
            _dbContext.Libraries.AddRange(mockLibraries);
            await _dbContext.SaveChangesAsync();
        }

        [TearDown]
        public void TearDown()
        {
            // Optionally dispose of the DbContext to release resources
            _dbContext.Dispose();
        }

        [Test]
        public async Task GetLibraries_ReturnsAllLibraries()
        {
            // Arrange
            PopulateDatabase();
            var controller = new LibraryController(_dbContext);

            // Act
            var result = await controller.GetAllLibraries();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var libraries = okResult.Value as List<Library>;
            Assert.That(libraries, Is.Not.Null);
            Assert.That(libraries.Count, Is.EqualTo(_dbContext.Libraries.Count()));
        }

        [Test]
        public async Task GetLibraries_ReturnsNoContent()
        {
            // Arrange
            var controller = new LibraryController(_dbContext);

            // Act
            var result = await controller.GetAllLibraries();

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task GetLibraryById_ExistingLibrary_ReturnsOkResult()
        {
            // Arrange
            PopulateDatabase();
            var controller = new LibraryController(_dbContext);
            var existingLibraryId = 1;

            // Act
            var result = await controller.GetLibraryById(existingLibraryId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.InstanceOf<Library>());
            var library = (Library)okResult.Value;
            Assert.That(library.LibraryId, Is.EqualTo(existingLibraryId));
        }

        [Test]
        public async Task GetLibraryById_NonExistingLibrary_ReturnsNotFoundResult()
        {
            // Arrange
            var controller = new LibraryController(_dbContext);
            var nonExistingLibraryId = 999;

            // Act
            var result = await controller.GetLibraryById(nonExistingLibraryId);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task GetLibraryByAlias_ExistingLibrary_ReturnsOkResult()
        {
            // Arrange
            PopulateDatabase();
            var controller = new LibraryController(_dbContext);
            var existingLibraryAlias = "ESTG";

            // Act
            var result = await controller.GetLibraryByAlias(existingLibraryAlias);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.InstanceOf<Library>());
            var library = (Library)okResult.Value;
            Assert.That(library.LibraryAlias, Is.EqualTo(existingLibraryAlias));
        }

        [Test]
        public async Task GetLibraryByAlias_NonExistingLibrary_ReturnsNotFoundResult()
        {
            // Arrange
            var controller = new LibraryController(_dbContext);
            var nonExistingLibraryAlias = "WWWWW";

            // Act
            var result = await controller.GetLibraryByAlias(nonExistingLibraryAlias);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task CreateLibrary_ValidInput_ReturnsCreatedResult()
        {
            // Arrange
            var controller = new LibraryController(_dbContext);
            var library = new LibraryDTO
            {
                LibraryName = "Escola Superior de Tecnologia e Gestão",
                LibraryAlias = "ESTG",
                LibraryAddress = "Felgueiras"
            };

            // Act
            var result = await controller.CreateLibrary(library);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var createdAtActionResult = (CreatedAtActionResult)result;
            Assert.That(createdAtActionResult.Value, Is.InstanceOf<Library>());
            var createdLibrary = (Library)createdAtActionResult.Value;
            Assert.That(createdLibrary.LibraryName, Is.EqualTo(library.LibraryName));
            Assert.That(createdLibrary.LibraryAlias, Is.EqualTo(library.LibraryAlias));
            Assert.That(createdLibrary.LibraryAddress, Is.EqualTo(library.LibraryAddress));
        }

        [Test]
        public async Task CreateLibrary_InvalidInput_ReturnsBadRequestResult()
        {
            // Arrange
            PopulateDatabase();
            var controller = new LibraryController(_dbContext);
            var library = new LibraryDTO
            {
                LibraryName = "Escola Superior de Tecnologia e Gestão",
                LibraryAlias = "ESTG",
                LibraryAddress = "Felgueiras"
            };

            // Act
            var result = await controller.CreateLibrary(library);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task UpdateLibrary_ValidInput_ReturnsCreatedResult()
        {
            // Arrange
            PopulateDatabase();
            var controller = new LibraryController(_dbContext);
            var existingLibraryId = 1;
            var library = new LibraryDTO
            {
                LibraryName = "Escola Superior de Tecnologia e Gestão",
                LibraryAlias = "ESTG",
                LibraryAddress = "Felgueiras"
            };

            // Act
            var result = await controller.UpdateLibrary(existingLibraryId, library);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
        }

        [Test]
        public async Task UpdateLibrary_NonExistingLibrary_ReturnsNotFoundResult()
        {
            // Arrange
            PopulateDatabase();
            var controller = new LibraryController(_dbContext);
            var nonExistingLibraryId = 999;
            var library = new LibraryDTO
            {
                LibraryName = "Escola Superior de Tecnologia e Gestão",
                LibraryAlias = "ESTG",
                LibraryAddress = "Felgueiras"
            };

            // Act
            var result = await controller.UpdateLibrary(nonExistingLibraryId, library);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task DeleteLibrary_ExistingLibrary_ReturnsNoContentResult()
        {
            // Arrange
            PopulateDatabase();
            var controller = new LibraryController(_dbContext);
            var existingLibraryId = 1;

            // Act
            var result = await controller.DeleteLibrary(existingLibraryId);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task DeleteLibrary_NonExistingLibrary_ReturnsNotFoundResult()
        {
            // Arrange
            PopulateDatabase();
            var controller = new LibraryController(_dbContext);
            var nonExistingLibraryId = 999;

            // Act
            var result = await controller.DeleteLibrary(nonExistingLibraryId);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task DeleteLibrary_ExistingLibrary_ReturnsBadRequestResult()
        {
            // Arrange
            PopulateDatabase();
            var controller = new LibraryController(_dbContext);
            var existingLibraryId = 2;

            // Act
            var result = await controller.DeleteLibrary(existingLibraryId);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

    }
}
