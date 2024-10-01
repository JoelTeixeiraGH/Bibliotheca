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
    public class NotificationControllerTests
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
                new Library { LibraryId = 1, LibraryName = "Library 1", LibraryAlias = "L1", LibraryAddress = "Address 1" },
                new Library { LibraryId = 2, LibraryName = "Library 2", LibraryAlias = "L2", LibraryAddress = "Address 2" },
            };

            var mockNotifications = new List<Notification>
            {
                new Notification { NotificationTitle = "Title 1", NotificationDescription = "Description 1", EmittedDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), ForAll = true, UserId = 1, RequestId = 1, LibraryId = 1 },
                new Notification { NotificationTitle = "Title 2", NotificationDescription = "Description 2", EmittedDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), ForAll = false, UserId = 1, RequestId = 1, LibraryId = 1 },
            };

            var mockUsers = new List<User>
            {
                new User { UserName = "User 1", UserEmail = "Email 1", UserPassword = "Password 1", UserRole = UserRole.Reader, LibraryId = 1 },
                new User { UserName = "User 2", UserEmail = "Email 2", UserPassword = "Password 2", UserRole = UserRole.Reader, LibraryId = 1 },
            };

            var mockRequests = new List<Request>
            {
                new Request { RequestId = 1, UserId = 1, ISBN = "ISBN 1", EndDate = DateTime.Now, RequestStatus = RequestStatus.Waiting, LibraryId = 1 },
                new Request { RequestId = 2, UserId = 1, ISBN = "ISBN 2", EndDate = DateTime.Now, RequestStatus = RequestStatus.Waiting, LibraryId = 1 },
            };

            _dbContext.Notifications.AddRange(mockNotifications);
            _dbContext.Users.AddRange(mockUsers);
            _dbContext.Requests.AddRange(mockRequests);
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
        public async Task GetAllNotifications_ReturnsAllNotifications()
        {
            // Arrange
            PopulateDatabase();
            var controller = new NotificationController(_dbContext);

            // Act
            var result = await controller.GetAllNotifications();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
        }

        [Test]
        public async Task GetAllNotifications_ReturnsNoContentForEmptyList()
        {
            // Arrange
            // No need to seed the database for an empty list
            var controller = new NotificationController(_dbContext);

            // Act
            var result = await controller.GetAllNotifications();

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task GetAllNotificationsByUserId_ReturnsAllNotifications()
        {
            // Arrange
            PopulateDatabase();
            var controller = new NotificationController(_dbContext);

            // Act
            var result = await controller.GetAllNotificationsByUserId(1);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
        }

        [Test]
        public async Task GetAllNotificationsByUserId_ReturnsNoContentForEmptyList()
        {
            // Arrange
            // No need to seed the database for an empty list
            var controller = new NotificationController(_dbContext);

            // Act
            var result = await controller.GetAllNotificationsByUserId(2);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task GetAllNotificationsByLibraryIdForUser_ReturnsAllNotifications()
        {
            // Arrange
            PopulateDatabase();
            var controller = new NotificationController(_dbContext);

            // Act
            var result = await controller.GetAllNotificationsByLibraryIdForUser(1);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
        }

        [Test]
        public async Task GetAllNotificationsByLibraryIdForUser_ReturnsNoContentForEmptyList()
        {
            // Arrange
            // No need to seed the database for an empty list
            var controller = new NotificationController(_dbContext);

            // Act
            var result = await controller.GetAllNotificationsByLibraryIdForUser(2);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task GetAllNotificationsByLibraryIdForLibrary_ReturnsAllNotifications()
        {
            // Arrange
            PopulateDatabase();
            var controller = new NotificationController(_dbContext);

            // Act
            var result = await controller.GetAllNotificationsByLibraryIdForLibrary(1);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
        }

        [Test]
        public async Task GetAllNotificationsByLibraryIdForLibrary_ReturnsNoContentForEmptyList()
        {
            // Arrange
            // No need to seed the database for an empty list
            var controller = new NotificationController(_dbContext);

            // Act
            var result = await controller.GetAllNotificationsByLibraryIdForLibrary(2);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task GetAllNotificationsFromRequestsByUserId_ReturnsAllNotifications()
        {
            // Arrange
            PopulateDatabase();
            var controller = new NotificationController(_dbContext);

            // Act
            var result = await controller.GetAllNotificationsFromRequestsByUserId(1);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
        }

        [Test]
        public async Task GetAllNotificationsFromRequestsByUserId_ReturnsNoContentForEmptyList()
        {
            // Arrange
            // No need to seed the database for an empty list
            var controller = new NotificationController(_dbContext);

            // Act
            var result = await controller.GetAllNotificationsFromRequestsByUserId(2);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task GetNotificationById_ReturnsNotification()
        {
            // Arrange
            PopulateDatabase();
            var controller = new NotificationController(_dbContext);

            // Act
            var result = await controller.GetNotificationById(1);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
        }

        [Test]
        public async Task GetNotificationById_ReturnsNotFound()
        {
            // Arrange
            PopulateDatabase();
            var controller = new NotificationController(_dbContext);

            // Act
            var result = await controller.GetNotificationById(99);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task CreateNotification_ReturnsCreatedNotification()
        {
            // Arrange
            PopulateDatabase();
            var controller = new NotificationController(_dbContext);
            var Notification = new NotificationDTO
            {
                NotificationTitle = "Title 1",
                NotificationDescription = "Description 1",
                EndDate = DateTime.Now.AddDays(3),
                LibraryId = 1,
            };

            // Act
            var result = await controller.CreateNotification(Notification);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = result as CreatedAtActionResult;
            Assert.That(createdResult, Is.Not.Null);
        }

        [Test]
        public async Task UpdateNotification_ReturnsCreatedNotification()
        {
            // Arrange
            PopulateDatabase();
            var controller = new NotificationController(_dbContext);
            var notification = new NotificationDTO
            {
                NotificationTitle = "Title 1",
                NotificationDescription = "Description 1",
                EndDate = DateTime.Now.AddDays(3),
                LibraryId = 1,
            };

            // Act
            var result = await controller.UpdateNotification(1, notification);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = result as CreatedAtActionResult;
            Assert.That(createdResult, Is.Not.Null);
        }

        [Test]
        public async Task UpdateNotification_ReturnsNotFound()
        {
            // Arrange
            PopulateDatabase();
            var controller = new NotificationController(_dbContext);
            var notification = new NotificationDTO
            {
                NotificationTitle = "Title 1",
                NotificationDescription = "Description 1",
                EndDate = DateTime.Now.AddDays(3),
                LibraryId = 1,
            };

            // Act
            var result = await controller.UpdateNotification(999, notification);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task DeleteNotification_ReturnsNoContent()
        {
            // Arrange
            PopulateDatabase();
            var controller = new NotificationController(_dbContext);

            // Act
            var result = await controller.DeleteNotification(1);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task DeleteNotification_ReturnsNotFound()
        {
            // Arrange
            PopulateDatabase();
            var controller = new NotificationController(_dbContext);

            // Act
            var result = await controller.DeleteNotification(999);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

    }
}
