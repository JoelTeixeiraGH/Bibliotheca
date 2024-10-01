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
    public class RequestControllerTests
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
                new PhysicalBook { ISBN = "1", LibraryId = 1, PhysicalBookStatus = PhysicalBookStatus.Requested },
                new PhysicalBook { ISBN = "1", LibraryId = 1, PhysicalBookStatus = PhysicalBookStatus.AtLibrary },
                new PhysicalBook { ISBN = "1", LibraryId = 1, PhysicalBookStatus = PhysicalBookStatus.Requested },
                new PhysicalBook { ISBN = "1", LibraryId = 1, PhysicalBookStatus = PhysicalBookStatus.AtLibrary },
                new PhysicalBook { ISBN = "1", LibraryId = 1, PhysicalBookStatus = PhysicalBookStatus.Requested },
                new PhysicalBook { ISBN = "1", LibraryId = 2, PhysicalBookStatus = PhysicalBookStatus.Requested }
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

            var mockUsers = new List<User>
            {
                new User { UserId = 1, UserName = "User 1", UserEmail = "user1", LibraryId = 1 },
                new User { UserId = 2, UserName = "User 2", UserEmail = "user2", LibraryId = 2 },
                new User { UserId = 3, UserName = "User 3", UserEmail = "user3", LibraryId = 1 }
            };

            var mockRequests = new List<Request>
            {
                new Request { RequestId = 1, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), RequestStatus = RequestStatus.Pending, UserId = 1, PhysicalBookId = 1, ISBN = "1", LibraryId = 1 },
                new Request { RequestId = 2, StartDate = DateTime.Now, RequestStatus = RequestStatus.Waiting, UserId = 1, ISBN = "1", LibraryId = 1 },
                new Request { RequestId = 3, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), RequestStatus = RequestStatus.Requested, UserId = 1, PhysicalBookId = 3, ISBN = "1", LibraryId = 1 },
                new Request { RequestId = 4, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), RequestStatus = RequestStatus.Returned, UserId = 1, PhysicalBookId = 4, ISBN = "1", LibraryId = 1 },
                new Request { RequestId = 5, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), RequestStatus = RequestStatus.NotReturned, UserId = 1, PhysicalBookId = 5, ISBN = "1", LibraryId = 1 },
            };

            _dbContext.Requests.AddRange(mockRequests);
            _dbContext.Users.AddRange(mockUsers);
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
        public async Task GetAllRequests_ReturnsAllRequests()
        {
            // Arrange
            PopulateDatabase();
            var controller = new RequestController(_dbContext);

            // Act
            var result = await controller.GetAllRequests();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
        }

        [Test]
        public async Task GetAllRequests_ReturnsNoContentForEmptyList()
        {
            // Arrange
            // No need to seed the database for an empty list
            var controller = new RequestController(_dbContext);

            // Act
            var result = await controller.GetAllRequests();

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task GetAllRequestsByISBNForWaitingListByLibraryId_ReturnsAllRequests()
        {
            // Arrange
            PopulateDatabase();
            var controller = new RequestController(_dbContext);

            // Act
            var result = await controller.GetAllRequestsByISBNForWaitingListByLibraryId("1", 1);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
        }

        [Test]
        public async Task GetAllRequestsByISBNForWaitingListByLibraryId_ReturnsNoContentForEmptyList()
        {
            // Arrange
            // No need to seed the database for an empty list
            var controller = new RequestController(_dbContext);

            // Act
            var result = await controller.GetAllRequestsByISBNForWaitingListByLibraryId("2", 1);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task GetAllRequestsByUserId_ReturnsAllRequests()
        {
            // Arrange
            PopulateDatabase();
            var controller = new RequestController(_dbContext);

            // Act
            var result = await controller.GetAllRequestsByUserId(1);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
        }

        [Test]
        public async Task GetAllRequestsByUserId_ReturnsNoContentForEmptyList()
        {
            // Arrange
            // No need to seed the database for an empty list
            var controller = new RequestController(_dbContext);

            // Act
            var result = await controller.GetAllRequestsByUserId(99);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task GetAllRequestsByLibraryId_ReturnsAllRequests()
        {
            // Arrange
            PopulateDatabase();
            var controller = new RequestController(_dbContext);

            // Act
            var result = await controller.GetAllRequestsByLibraryId(1);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
        }

        [Test]
        public async Task GetAllRequestsByLibraryId_ReturnsNoContentForEmptyList()
        {
            // Arrange
            // No need to seed the database for an empty list
            var controller = new RequestController(_dbContext);

            // Act
            var result = await controller.GetAllRequestsByLibraryId(999);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task GetRequestById_ReturnsRequest()
        {
            // Arrange
            PopulateDatabase();
            var controller = new RequestController(_dbContext);

            // Act
            var result = await controller.GetRequestById(1);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
        }

        [Test]
        public async Task GetRequestById_ReturnsNotFoundForInvalidId()
        {
            // Arrange
            PopulateDatabase();
            var controller = new RequestController(_dbContext);

            // Act
            var result = await controller.GetRequestById(99);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task CreateRequestWithStatusRequested_ReturnsCreatedRequest()
        {
            // Arrange
            PopulateDatabase();
            var controller = new RequestController(_dbContext);
            var request = new RequestDTO { EndDate = DateTime.Now.AddDays(1), UserId = 3, PhysicalBookId = 2, ISBN = "1", LibraryId = 1 };

            // Act
            var result = await controller.CreateRequestWithStatusRequested(request);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = result as CreatedAtActionResult;
            Assert.That(createdResult, Is.Not.Null);
        }

        [Test]
        public async Task CreateRequestWithStatusRequested_ReturnsBadRequest()
        {
            // Arrange
            PopulateDatabase();
            var controller = new RequestController(_dbContext);
            var request = new RequestDTO { EndDate = DateTime.Now.AddDays(1), UserId = 1, PhysicalBookId = 1, ISBN = "1", LibraryId = 1 };

            // Act
            var result = await controller.CreateRequestWithStatusRequested(request);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task CreateRequestWithStatusPending_ReturnsCreatedRequest()
        {
            // Arrange
            PopulateDatabase();
            var controller = new RequestController(_dbContext);
            var request = new RequestDTO { EndDate = DateTime.Now.AddDays(1), UserId = 3, PhysicalBookId = 2, ISBN = "1", LibraryId = 1 };

            // Act
            var result = await controller.CreateRequestWithStatusPending(request);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = result as CreatedAtActionResult;
            Assert.That(createdResult, Is.Not.Null);
        }

        [Test]
        public async Task CreateRequestWithStatusPending_ReturnsBadRequest()
        {
            // Arrange
            PopulateDatabase();
            var controller = new RequestController(_dbContext);
            var request = new RequestDTO { EndDate = DateTime.Now.AddDays(1), UserId = 1, PhysicalBookId = 1, ISBN = "1", LibraryId = 1 };

            // Act
            var result = await controller.CreateRequestWithStatusPending(request);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task CreateRequestWithStatusWaiting_ReturnsCreatedRequest()
        {
            // Arrange
            PopulateDatabase();
            var controller = new RequestController(_dbContext);
            var request = new RequestDTO { UserId = 2, ISBN = "1", LibraryId = 2 };

            // Act
            var result = await controller.CreateRequestWithStatusWaiting(request);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = result as CreatedAtActionResult;
            Assert.That(createdResult, Is.Not.Null);
        }

        [Test]
        public async Task CreateRequestWithStatusWaiting_ReturnsBadRequest()
        {
            // Arrange
            PopulateDatabase();
            var controller = new RequestController(_dbContext);
            var request = new RequestDTO { UserId = 1, ISBN = "1", LibraryId = 1 };

            // Act
            var result = await controller.CreateRequestWithStatusWaiting(request);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task ChangeRequestStatusToCanceled_ReturnsCreatedRequest()
        {
            // Arrange
            PopulateDatabase();
            var controller = new RequestController(_dbContext);

            // Act
            var result = await controller.ChangeRequestStatusToCanceled(1);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = result as CreatedAtActionResult;
            Assert.That(createdResult, Is.Not.Null);
        }

        [Test]
        public async Task ChangeRequestStatusToCanceled_ReturnsBadRequest()
        {
            // Arrange
            PopulateDatabase();
            var controller = new RequestController(_dbContext);

            // Act
            var result = await controller.ChangeRequestStatusToCanceled(3);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task ChangeRequestStatusToCanceled_ReturnsNotFound()
        {
            // Arrange
            PopulateDatabase();
            var controller = new RequestController(_dbContext);

            // Act
            var result = await controller.ChangeRequestStatusToCanceled(999);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task ChangeRequestStatusToRequested_ReturnsCreatedRequest()
        {
            // Arrange
            PopulateDatabase();
            var controller = new RequestController(_dbContext);

            // Act
            var result = await controller.ChangeRequestStatusToRequested(1, DateTime.Now);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = result as CreatedAtActionResult;
            Assert.That(createdResult, Is.Not.Null);
        }

        [Test]
        public async Task ChangeRequestStatusToRequested_ReturnsBadRequest()
        {
            // Arrange
            PopulateDatabase();
            var controller = new RequestController(_dbContext);

            // Act
            var result = await controller.ChangeRequestStatusToRequested(3, DateTime.Now);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task ChangeRequestStatusToRequested_ReturnsNotFound()
        {
            // Arrange
            PopulateDatabase();
            var controller = new RequestController(_dbContext);

            // Act
            var result = await controller.ChangeRequestStatusToRequested(999, DateTime.Now);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task ChangeRequestStatusToPending_ReturnsCreatedRequest()
        {
            // Arrange
            PopulateDatabase();
            var controller = new RequestController(_dbContext);

            // Act
            var result = await controller.ChangeRequestStatusToPending(2, DateTime.Now);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = result as CreatedAtActionResult;
            Assert.That(createdResult, Is.Not.Null);
        }

        [Test]
        public async Task ChangeRequestStatusToPending_ReturnsBadRequest()
        {
            // Arrange
            PopulateDatabase();
            var controller = new RequestController(_dbContext);

            // Act
            var result = await controller.ChangeRequestStatusToPending(3, DateTime.Now);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task ChangeRequestStatusToPending_ReturnsNotFound()
        {
            // Arrange
            PopulateDatabase();
            var controller = new RequestController(_dbContext);

            // Act
            var result = await controller.ChangeRequestStatusToPending(999, DateTime.Now);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task ChangeRequestStatusToReturned_ReturnsCreatedRequest()
        {
            // Arrange
            PopulateDatabase();
            var controller = new RequestController(_dbContext);

            // Act
            var result = await controller.ChangeRequestStatusToReturned(3);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = result as CreatedAtActionResult;
            Assert.That(createdResult, Is.Not.Null);
        }

        [Test]
        public async Task ChangeRequestStatusToReturned_ReturnsBadRequest()
        {
            // Arrange
            PopulateDatabase();
            var controller = new RequestController(_dbContext);

            // Act
            var result = await controller.ChangeRequestStatusToReturned(1);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task ChangeRequestStatusToReturned_ReturnsNotFound()
        {
            // Arrange
            PopulateDatabase();
            var controller = new RequestController(_dbContext);

            // Act
            var result = await controller.ChangeRequestStatusToReturned(999);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task ChangeRequestStatusToNotReturned_ReturnsCreatedRequest()
        {
            // Arrange
            PopulateDatabase();
            var controller = new RequestController(_dbContext);

            // Act
            var result = await controller.ChangeRequestStatusToNotReturned(3);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = result as CreatedAtActionResult;
            Assert.That(createdResult, Is.Not.Null);
        }

        [Test]
        public async Task ChangeRequestStatusToNotReturned_ReturnsBadRequest()
        {
            // Arrange
            PopulateDatabase();
            var controller = new RequestController(_dbContext);

            // Act
            var result = await controller.ChangeRequestStatusToNotReturned(1);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task ChangeRequestStatusToNotReturned_ReturnsNotFound()
        {
            // Arrange
            PopulateDatabase();
            var controller = new RequestController(_dbContext);

            // Act
            var result = await controller.ChangeRequestStatusToNotReturned(999);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task ExtendTime_ReturnsCreatedRequest()
        {
            // Arrange
            PopulateDatabase();
            var controller = new RequestController(_dbContext);

            // Act
            var result = await controller.ExtendTime(3, DateTime.Now);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = result as CreatedAtActionResult;
            Assert.That(createdResult, Is.Not.Null);
        }

        [Test]
        public async Task ExtendTime_ReturnsBadRequest()
        {
            // Arrange
            PopulateDatabase();
            var controller = new RequestController(_dbContext);

            // Act
            var result = await controller.ExtendTime(1, DateTime.Now);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task ExtendTime_ReturnsNotFound()
        {
            // Arrange
            PopulateDatabase();
            var controller = new RequestController(_dbContext);

            // Act
            var result = await controller.ExtendTime(999, DateTime.Now);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task DeleteRequest_ReturnsNoContent()
        {
            // Arrange
            PopulateDatabase();
            var controller = new RequestController(_dbContext);

            // Act
            var result = await controller.DeleteRequest(4);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task DeleteRequest_ReturnsBadRequest()
        {
            // Arrange
            PopulateDatabase();
            var controller = new RequestController(_dbContext);

            // Act
            var result = await controller.DeleteRequest(1);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task DeleteRequest_ReturnsNotFound()
        {
            // Arrange
            PopulateDatabase();
            var controller = new RequestController(_dbContext);

            // Act
            var result = await controller.DeleteRequest(999);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}
