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
    public class TransferControllerTests
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
                new PhysicalBook { ISBN = "1", LibraryId = 1, PhysicalBookStatus = PhysicalBookStatus.AtLibrary },
                new PhysicalBook { ISBN = "1", LibraryId = 1, PhysicalBookStatus = PhysicalBookStatus.Requested },
            };

            var mockGenericBooks = new List<GenericBook>
            {
                new GenericBook { ISBN = "1", Title = "Title 1", Description = "Description 1", PageNumber = 1, LanguageId = 1, DatePublished = DateTime.Now },
            };

            var mockLibraries = new List<Library>
            {
                new Library { LibraryId = 1, LibraryName = "Library 1", LibraryAlias = "L1", LibraryAddress = "Address 1" },
                new Library { LibraryId = 2, LibraryName = "Library 2", LibraryAlias = "L2", LibraryAddress = "Address 2" },
            };

            var mockLanguages = new List<Language>
            {
                new Language { LanguageId = 1, LanguageName = "Language 1", LanguageAlias = "L1" },
            };

            var mockTransfers = new List<Transfer>
            {
                new Transfer { TransferId = 1, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), TransferStatus = TransferStatus.Pending, SourceLibraryId = 1, DestinationLibraryId = 2, PhysicalBookId = 1 },
                new Transfer { TransferId = 2, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), TransferStatus = TransferStatus.Accepted, SourceLibraryId = 1, DestinationLibraryId = 2, PhysicalBookId = 1 },
                new Transfer { TransferId = 3, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), TransferStatus = TransferStatus.Rejected, SourceLibraryId = 1, DestinationLibraryId = 2, PhysicalBookId = 1 },
                new Transfer { TransferId = 4, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), TransferStatus = TransferStatus.Canceled, SourceLibraryId = 1, DestinationLibraryId = 2, PhysicalBookId = 1 },
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
        public async Task GetAllTransfers_ReturnsAllTransfers()
        {
            // Arrange
            PopulateDatabase();
            var controller = new TransferController(_dbContext);

            // Act
            var result = await controller.GetAllTransfers();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
        }

        [Test]
        public async Task GetAllTransfers_ReturnsNoContentForEmptyList()
        {
            // Arrange
            // No need to seed the database for an empty list
            var controller = new TransferController(_dbContext);

            // Act
            var result = await controller.GetAllTransfers();

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task GetAllTransfersByLibraryId_ReturnsAllTransfers()
        {
            // Arrange
            PopulateDatabase();
            var controller = new TransferController(_dbContext);

            // Act
            var result = await controller.GetAllTransfersByLibraryId(1);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
        }

        [Test]
        public async Task GetAllTransfersByLibraryId_ReturnsNoContentForEmptyList()
        {
            // Arrange
            // No need to seed the database for an empty list
            var controller = new TransferController(_dbContext);

            // Act
            var result = await controller.GetAllTransfersByLibraryId(99);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task GetTransferById_ReturnsTransfer()
        {
            // Arrange
            PopulateDatabase();
            var controller = new TransferController(_dbContext);

            // Act
            var result = await controller.GetTransferByTransferId(1);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
        }

        [Test]
        public async Task GetTransferById_ReturnsNotFound()
        {
            // Arrange
            PopulateDatabase();
            var controller = new TransferController(_dbContext);

            // Act
            var result = await controller.GetTransferByTransferId(99);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task CreateTransfer_ReturnsCreatedTransfer()
        {
            // Arrange
            PopulateDatabase();
            var controller = new TransferController(_dbContext);
            var transfer = new TransferDTO
            {
                EndDate = DateTime.Now.AddDays(1),
                SourceLibraryId = 1,
                DestinationLibraryId = 2,
                PhysicalBookId = 1,
            };

            // Act
            var result = await controller.CreateTransfer(transfer);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = result as CreatedAtActionResult;
            Assert.That(createdResult, Is.Not.Null);
        }

        [Test]
        public async Task CreateTransfer_ReturnsBadRequest()
        {
            // Arrange
            PopulateDatabase();
            var controller = new TransferController(_dbContext);
            var transfer = new TransferDTO
            {
                EndDate = DateTime.Now.AddDays(1),
                SourceLibraryId = 2,
                DestinationLibraryId = 1,
                PhysicalBookId = 1,
            };

            // Act
            var result = await controller.CreateTransfer(transfer);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task ChangeTransferStatusToAccepted_ReturnsCreatedTransfer()
        {
            // Arrange
            PopulateDatabase();
            var controller = new TransferController(_dbContext);

            // Act
            var result = await controller.ChangeTransferStatusToAccepted(1);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = result as CreatedAtActionResult;
            Assert.That(createdResult, Is.Not.Null);
        }

        [Test]
        public async Task ChangeTransferStatusToAccepted_ReturnsBadRequest()
        {
            // Arrange
            PopulateDatabase();
            var controller = new TransferController(_dbContext);

            // Act
            var result = await controller.ChangeTransferStatusToAccepted(2);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task ChangeTransferStatusToAccepted_ReturnsNotFound()
        {
            // Arrange
            PopulateDatabase();
            var controller = new TransferController(_dbContext);

            // Act
            var result = await controller.ChangeTransferStatusToAccepted(999);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task ChangeTransferStatusToRejected_ReturnsCreatedTransfer()
        {
            // Arrange
            PopulateDatabase();
            var controller = new TransferController(_dbContext);

            // Act
            var result = await controller.ChangeTransferStatusToRejected(1);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = result as CreatedAtActionResult;
            Assert.That(createdResult, Is.Not.Null);
        }

        [Test]
        public async Task ChangeTransferStatusToRejected_ReturnsBadRequest()
        {
            // Arrange
            PopulateDatabase();
            var controller = new TransferController(_dbContext);

            // Act
            var result = await controller.ChangeTransferStatusToRejected(2);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task ChangeTransferStatusToRejected_ReturnsNotFound()
        {
            // Arrange
            PopulateDatabase();
            var controller = new TransferController(_dbContext);

            // Act
            var result = await controller.ChangeTransferStatusToRejected(999);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task ChangeTransferStatusToCanceled_ReturnsCreatedTransfer()
        {
            // Arrange
            PopulateDatabase();
            var controller = new TransferController(_dbContext);

            // Act
            var result = await controller.ChangeTransferStatusToCanceled(1);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = result as CreatedAtActionResult;
            Assert.That(createdResult, Is.Not.Null);
        }

        [Test]
        public async Task ChangeTransferStatusToCanceled_ReturnsBadRequest()
        {
            // Arrange
            PopulateDatabase();
            var controller = new TransferController(_dbContext);

            // Act
            var result = await controller.ChangeTransferStatusToCanceled(2);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task ChangeTransferStatusToCanceled_ReturnsNotFound()
        {
            // Arrange
            PopulateDatabase();
            var controller = new TransferController(_dbContext);

            // Act
            var result = await controller.ChangeTransferStatusToCanceled(999);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task DeleteTransfer_ReturnsNoContent()
        {
            // Arrange
            PopulateDatabase();
            var controller = new TransferController(_dbContext);

            // Act
            var result = await controller.DeleteTransfer(2);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task DeleteTransfer_ReturnsBadRequest()
        {
            // Arrange
            PopulateDatabase();
            var controller = new TransferController(_dbContext);

            // Act
            var result = await controller.DeleteTransfer(1);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task DeleteTransfer_ReturnsNotFound()
        {
            // Arrange
            PopulateDatabase();
            var controller = new TransferController(_dbContext);

            // Act
            var result = await controller.DeleteTransfer(999);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}
