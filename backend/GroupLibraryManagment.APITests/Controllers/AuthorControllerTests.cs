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
    public class AuthorControllerTests
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
            var mockAuthors = new List<Author>
            {
                new Author { AuthorId = 1, AuthorName = "Author1" },
                new Author { AuthorId = 2, AuthorName = "Author2" },
            };

            var associatedBookAuthor = new BookAuthor { AuthorId = 2, ISBN = "2" };

            _dbContext.Authors.AddRange(mockAuthors);
            _dbContext.BookAuthors.Add(associatedBookAuthor);
            await _dbContext.SaveChangesAsync();
        }   

        [TearDown]
        public void TearDown()
        {
            // Optionally dispose of the DbContext to release resources
            _dbContext.Dispose();
        }

        [Test]
        public async Task GetAllAuthors_ReturnsListOfAuthors()
        {
            // Arrange
            PopulateDatabase();
            var controller = new AuthorController(_dbContext);

            // Act
            var result = await controller.GetAllAuthors();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var authors = okResult.Value as List<Author>;
            Assert.That(authors, Is.Not.Null);
            Assert.That(authors.Count, Is.EqualTo(_dbContext.Authors.Count()));
        }

        [Test]
        public async Task GetAllAuthors_ReturnsNoContentForEmptyList()
        {
            // Arrange
            // No need to seed the database for an empty list
            var controller = new AuthorController(_dbContext);

            // Act
            var result = await controller.GetAllAuthors();

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task GetAuthorById_ExistingAuthor_ReturnsOkResult()
        {
            // Arrange
            PopulateDatabase();
            var controller = new AuthorController(_dbContext);
            var existingAuthorId = 1;

            // Act
            var result = await controller.GetAuthorById(existingAuthorId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.InstanceOf<Author>());
            var author = (Author)okResult.Value;
            Assert.That(author.AuthorId, Is.EqualTo(existingAuthorId));
        }

        [Test]
        public async Task GetAuthorById_NonExistingAuthor_ReturnsNotFoundResult()
        {
            // Arrange
            var controller = new AuthorController(_dbContext);
            var nonExistingAuthorId = 999;

            // Act
            var result = await controller.GetAuthorById(nonExistingAuthorId);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task GetAuthorByName_ExistingAuthor_ReturnsOkResult()
        {
            // Arrange
            PopulateDatabase();
            var controller = new AuthorController(_dbContext);
            var existingAuthorName = "Author1";

            // Act
            var result = await controller.GetAuthorByName(existingAuthorName);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.InstanceOf<Author>());
            var author = (Author)okResult.Value;
            Assert.That(author.AuthorName, Is.EqualTo(existingAuthorName));
        }

        [Test]
        public async Task GetAuthorByName_NonExistingAuthor_ReturnsNotFoundResult()
        {
            // Arrange
            var controller = new AuthorController(_dbContext);
            var nonExistingAuthorName = "NonExistingAuthorName";

            // Act
            var result = await controller.GetAuthorByName(nonExistingAuthorName);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task CreateAuthor_ValidInput_ReturnsCreatedAtAction()
        {
            // Arrange
            var controller = new AuthorController(_dbContext);
            var validAuthorDTO = new AuthorDTO { AuthorName = "NewAuthor" };

            // Act
            var result = await controller.CreateAuthor(validAuthorDTO);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var createdAtActionResult = (CreatedAtActionResult)result;

            Assert.That(createdAtActionResult.ActionName, Is.EqualTo(nameof(AuthorController.GetAuthorById)));

            Assert.That(createdAtActionResult.RouteValues, Is.Not.Null);
            Assert.That(createdAtActionResult.RouteValues.ContainsKey("authorId"));
        }

        [Test]
        public async Task CreateAuthor_DuplicateAuthorName_ReturnsBadRequest()
        {
            // Arrange
            PopulateDatabase();
            var controller = new AuthorController(_dbContext);
            var duplicateAuthorDTO = new AuthorDTO { AuthorName = "Author1" };

            // Act
            var result = await controller.CreateAuthor(duplicateAuthorDTO);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task UpdateAuthor_ExistingAuthor_ReturnsCreatedAtAction()
        {
            // Arrange
            PopulateDatabase();
            var controller = new AuthorController(_dbContext);
            var existingAuthor = _dbContext.Authors.First();
            var updatedAuthorDTO = new AuthorDTO { AuthorName = "UpdatedAuthor" };

            // Act
            var result = await controller.UpdateAuthor(existingAuthor.AuthorId, updatedAuthorDTO);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var createdAtActionResult = (CreatedAtActionResult)result;

            Assert.That(createdAtActionResult.ActionName, Is.EqualTo(nameof(AuthorController.GetAuthorById)));

            Assert.That(createdAtActionResult.RouteValues, Is.Not.Null);
            Assert.That(createdAtActionResult.RouteValues.ContainsKey("authorId"));
            Assert.That(createdAtActionResult.RouteValues["authorId"], Is.EqualTo(existingAuthor.AuthorId));

        }

        [Test]
        public async Task UpdateAuthor_NonExistingAuthor_ReturnsNotFound()
        {
            // Arrange
            var controller = new AuthorController(_dbContext);
            var nonExistingAuthorId = 999;
            var updatedAuthorDTO = new AuthorDTO { AuthorName = "UpdatedAuthor" };

            // Act
            var result = await controller.UpdateAuthor(nonExistingAuthorId, updatedAuthorDTO);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task DeleteAuthor_ExistingAuthor_NoAssociatedBooks_ReturnsNoContent()
        {
            // Arrange
            PopulateDatabase();
            var existingAuthor = _dbContext.Authors.First();
            var controller = new AuthorController(_dbContext);

            // Act
            var result = await controller.DeleteAuthor(existingAuthor.AuthorId);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task DeleteAuthor_ExistingAuthor_WithAssociatedBooks_ReturnsBadRequest()
        {
            // Arrange
            PopulateDatabase();
            var existingAuthor = _dbContext.Authors.Last();
            var controller = new AuthorController(_dbContext);

            // Act
            var result = await controller.DeleteAuthor(existingAuthor.AuthorId);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task DeleteAuthor_NonExistingAuthor_ReturnsNotFound()
        {
            // Arrange
            var controller = new AuthorController(_dbContext);
            var nonExistingAuthorId = 999;

            // Act
            var result = await controller.DeleteAuthor(nonExistingAuthorId);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}
