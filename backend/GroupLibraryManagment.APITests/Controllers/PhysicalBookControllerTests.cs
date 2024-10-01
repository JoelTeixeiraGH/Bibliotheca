using GroupLibraryManagment.API.Controllers;
using GroupLibraryManagment.API.DTOs;
using GroupLibraryManagment.API.Entities;
using GroupLibraryManagment.API.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupLibraryManagment.APITests.Controllers
{
    [TestFixture]
    public class PhysicalBookControllerTests
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
            var mockPhysicalBooks = new List<PhysicalBook>
            {
                new PhysicalBook { ISBN = "1", LibraryId = 1 },
                new PhysicalBook { ISBN = "1", LibraryId = 2 },
                new PhysicalBook { ISBN = "2", LibraryId = 1, PhysicalBookStatus = PhysicalBookStatus.Requested },
                new PhysicalBook { ISBN = "2", LibraryId = 2, PhysicalBookStatus = PhysicalBookStatus.Transfer },
            };

            var mockGenericBooks = new List<GenericBook>
            {
                new GenericBook { ISBN = "1", Title = "Title 1", Description = "Description 1", PageNumber = 1, LanguageId = 1, DatePublished = DateTime.Now },
                new GenericBook { ISBN = "2", Title = "Title 2", Description = "Description 2", PageNumber = 2, LanguageId = 1, DatePublished = DateTime.Now },
            };

            var mockLibraries = new List<Library>
            {
                new Library { LibraryId = 1, LibraryName = "Library 1", LibraryAlias = "L1", LibraryAddress = "Address 1" },
                new Library { LibraryId = 2, LibraryName = "Library 2", LibraryAlias = "L2", LibraryAddress = "Address 2" },
            };

            var mockLanguages = new List<Language>
            {
                new Language { LanguageId = 1, LanguageName = "Language 1", LanguageAlias = "L1" },
                new Language { LanguageId = 2, LanguageName = "Language 2", LanguageAlias = "L2" },
            };

            var mockTransfers = new List<Transfer>
            { 
                new Transfer { PhysicalBookId = 4, TransferStatus = TransferStatus.Accepted, EndDate = DateTime.Now, DestinationLibraryId = 1, SourceLibraryId = 2 },
            };

            _dbContext.Transfers.AddRange(mockTransfers);
            _dbContext.Languages.AddRange(mockLanguages);
            _dbContext.Libraries.AddRange(mockLibraries);
            _dbContext.PhysicalBooks.AddRange(mockPhysicalBooks);
            _dbContext.GenericBooks.AddRange(mockGenericBooks);
            await _dbContext.SaveChangesAsync();
        }

        [TearDown]
        public void TearDown()
        {
            // Optionally dispose of the DbContext to release resources
            _dbContext.Dispose();
        }

        [Test]
        public async Task GetPhysicalBooks_ReturnsAllPhysicalBooks()
        {
            // Arrange
            PopulateDatabase();
            var physicalBookController = new PhysicalBookController(_dbContext);

            // Act
            var result = await physicalBookController.GetAllPhysicalBooks();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
        }

        [Test]
        public async Task GetPhysicalBooks_ReturnsNoContentForEmptyList()
        {
            // Arrange
            // No need to seed the database for an empty list
            var controller = new PhysicalBookController(_dbContext);

            // Act
            var result = await controller.GetAllPhysicalBooks();

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task GetAllPhysicalBooksByLibraryId_ReturnsAllPhysicalBooks()
        {
            // Arrange
            PopulateDatabase();
            var physicalBookController = new PhysicalBookController(_dbContext);
            var libraryId = 1;

            // Act
            var result = await physicalBookController.GetAllPhysicalBooksByLibraryId(libraryId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
        }

        [Test]
        public async Task GetAllPhysicalBooksByLibraryId_ReturnsNoContentForEmptyList()
        {
            // Arrange
            // No need to seed the database for an empty list
            var controller = new PhysicalBookController(_dbContext);
            var libraryId = 1;

            // Act
            var result = await controller.GetAllPhysicalBooksByLibraryId(libraryId);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task GetAllPhysicalBooksByISBNAtLibrary_ReturnsAllPhysicalBooks()
        {
            // Arrange
            PopulateDatabase();
            var physicalBookController = new PhysicalBookController(_dbContext);
            var bookIsbn = "1";

            // Act
            var result = await physicalBookController.GetAllPhysicalBooksByISBNAtLibrary(bookIsbn);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
        }

        [Test]
        public async Task GetAllPhysicalBooksByISBNAtLibrary_ReturnsNoContentForEmptyList()
        {
            // Arrange
            // No need to seed the database for an empty list
            var controller = new PhysicalBookController(_dbContext);
            var bookIsbn = "1";

            // Act
            var result = await controller.GetAllPhysicalBooksByISBNAtLibrary(bookIsbn);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task GetAllPhysicalBooksByISBNByLibraryId_ReturnsAllPhysicalBooks()
        {
            // Arrange
            PopulateDatabase();
            var physicalBookController = new PhysicalBookController(_dbContext);
            var bookIsbn = "1";
            var libraryId = 1;

            // Act
            var result = await physicalBookController.GetAllPhysicalBooksByISBNByLibraryId(bookIsbn, libraryId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
        }

        [Test]
        public async Task GetAllPhysicalBooksByISBNByLibraryId_ReturnsNoContentForEmptyList()
        {
            // Arrange
            // No need to seed the database for an empty list
            var controller = new PhysicalBookController(_dbContext);
            var bookIsbn = "1";
            var libraryId = 1;

            // Act
            var result = await controller.GetAllPhysicalBooksByISBNByLibraryId(bookIsbn, libraryId);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task GetAllPhysicalBooksByISBNCurrentlyAtLibrary_ReturnsAllPhysicalBooks()
        {
            // Arrange
            PopulateDatabase();
            var physicalBookController = new PhysicalBookController(_dbContext);
            var bookIsbn = "1";
            var libraryId = 1;

            // Act
            var result = await physicalBookController.GetAllPhysicalBooksByISBNCurrentlyAtLibrary(bookIsbn, libraryId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
        }

        [Test]
        public async Task GetAllPhysicalBooksByISBNCurrentlyAtLibrary_ReturnsNoContentForEmptyList()
        {
            // Arrange
            // No need to seed the database for an empty list
            var controller = new PhysicalBookController(_dbContext);
            var bookIsbn = "1";
            var libraryId = 1;

            // Act
            var result = await controller.GetAllPhysicalBooksByISBNCurrentlyAtLibrary(bookIsbn, libraryId);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task GetAllPhysicalBooksWithTransferStatusForLibrary_ReturnsAllPhysicalBooks()
        {
            // Arrange
            PopulateDatabase();
            var physicalBookController = new PhysicalBookController(_dbContext);
            var libraryId = 1;

            // Act
            var result = await physicalBookController.GetAllPhysicalBooksWithTransferStatusForLibrary(libraryId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
        }

        [Test]
        public async Task GetAllPhysicalBooksWithTransferStatusForLibrary_ReturnsNoContentForEmptyList()
        {
            // Arrange
            // No need to seed the database for an empty list
            var controller = new PhysicalBookController(_dbContext);
            var libraryId = 1;

            // Act
            var result = await controller.GetAllPhysicalBooksWithTransferStatusForLibrary(libraryId);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task GetPhysicalBookByPhysicalBookId_ReturnsPhysicalBook()
        {
            // Arrange
            PopulateDatabase();
            var physicalBookController = new PhysicalBookController(_dbContext);
            var physicalBookId = 1;

            // Act
            var result = await physicalBookController.GetPhysicalBookById(physicalBookId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
        }

        [Test]
        public async Task GetPhysicalBookByPhysicalBookId_ReturnsNotFoundForInvalidId()
        {
            // Arrange
            PopulateDatabase();
            var physicalBookController = new PhysicalBookController(_dbContext);
            var physicalBookId = 999;

            // Act
            var result = await physicalBookController.GetPhysicalBookById(physicalBookId);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task CreatePhysicalBook_ReturnsCreatedPhysicalBook()
        {
            // Arrange
            PopulateDatabase();
            var physicalBookController = new PhysicalBookController(_dbContext);
            var physicalBookDTO = new PhysicalBookDTO
            {
                LibraryId = 1,
                ISBN = "2"
            };

            // Act
            var result = await physicalBookController.CreatePhysicalBook(physicalBookDTO);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = result as CreatedAtActionResult;
            Assert.That(createdResult, Is.Not.Null);
        }

        [Test]
        public async Task CreatePhysicalBooks_ReturnsCreatedPhysicalBook()
        {
            // Arrange
            PopulateDatabase();
            var physicalBookController = new PhysicalBookController(_dbContext);
            var physicalBookDTO = new PhysicalBookDTO
            {
                LibraryId = 1,
                ISBN = "2"
            };

            // Act
            var result = await physicalBookController.CreatePhysicalBooks(5, physicalBookDTO);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = result as CreatedAtActionResult;
            Assert.That(createdResult, Is.Not.Null);
        }

        [Test]
        public async Task UpdatePhysicalBook_ReturnsCreatedAtAction()
        {
            // Arrange
            PopulateDatabase();
            var physicalBookController = new PhysicalBookController(_dbContext);
            var physicalBookDTO = new PhysicalBookDTO
            {
                LibraryId = 1,
                ISBN = "2"
            };
            var physicalBookId = 1;

            // Act
            var result = await physicalBookController.UpdatePhysicalBook(physicalBookId, physicalBookDTO);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
        }

        [Test]
        public async Task UpdatePhysicalBookStatus_ReturnsCreatedAtAction()
        {
            // Arrange
            PopulateDatabase();
            var physicalBookController = new PhysicalBookController(_dbContext);
            var physicalBookStatus = 0;
            var physicalBookId = 1;

            // Act
            var result = await physicalBookController.UpdatePhysicalBookStatus(physicalBookId, physicalBookStatus);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
        }

        [Test]
        public async Task DeletePhysicalBook_ReturnsNoContent()
        {
            // Arrange
            PopulateDatabase();
            var physicalBookController = new PhysicalBookController(_dbContext);
            var physicalBookId = 1;

            // Act
            var result = await physicalBookController.DeletePhysicalBook(physicalBookId);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task DeletePhysicalBook_ReturnsNotFound()
        {
            // Arrange
            PopulateDatabase();
            var physicalBookController = new PhysicalBookController(_dbContext);
            var physicalBookId = 99;

            // Act
            var result = await physicalBookController.DeletePhysicalBook(physicalBookId);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task DeletePhysicalBook_ReturnsBadRequest()
        {
            // Arrange
            PopulateDatabase();
            var physicalBookController = new PhysicalBookController(_dbContext);
            var physicalBookId = 3;

            // Act
            var result = await physicalBookController.DeletePhysicalBook(physicalBookId);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }
    }
}
