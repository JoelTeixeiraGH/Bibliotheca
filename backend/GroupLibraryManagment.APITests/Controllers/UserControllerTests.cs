using GroupLibraryManagment.API.Controllers;
using GroupLibraryManagment.API.DTOs;
using GroupLibraryManagment.API.Entities;
using GroupLibraryManagment.API.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GroupLibraryManagment.APITests.Controllers
{
    [TestFixture]
    public class UserControllerTests
    {
        private GroupLibraryManagmentDbContext _dbContext;
        private readonly IConfiguration _configuration;

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
            var mockUsers = new List<User>
            {
                new User { UserId = 1, UserName = "User 1", UserEmail = "user1", LibraryId = 1, UserPassword = BCrypt.Net.BCrypt.HashPassword("test"), RefreshToken = "token", TokenExpires= DateTime.Now.AddDays(1) },
                new User { UserId = 2, UserName = "User 2", UserEmail = "user2", LibraryId = 2, UserPassword = BCrypt.Net.BCrypt.HashPassword("test"), RefreshToken = "token", TokenExpires= DateTime.Now.AddDays(1) },
                new User { UserId = 3, UserName = "User 3", UserEmail = "user3", LibraryId = 1, UserPassword = BCrypt.Net.BCrypt.HashPassword("test"), RefreshToken = "token", TokenExpires= DateTime.Now.AddDays(1) }
            };

            var mockLibraries = new List<Library>
            {
                new Library { LibraryId = 1, LibraryName = "Library 1", LibraryAlias = "lib1", LibraryAddress = "Address 1" },
                new Library { LibraryId = 2, LibraryName = "Library 2", LibraryAlias = "lib2", LibraryAddress = "Address 2" }
            };  

            _dbContext.Libraries.AddRange(mockLibraries);
            _dbContext.Users.AddRange(mockUsers);
            await _dbContext.SaveChangesAsync();
        }

        [TearDown]
        public void TearDown()
        {
            // Optionally dispose of the DbContext to release resources
            _dbContext.Dispose();
        }

        [Test]
        public async Task GetAllUsers_ReturnsAllUsers()
        {
            // Arrange
            PopulateDatabase();
            var controller = new UserController(_dbContext, _configuration);

            // Act
            var result = await controller.GetAllUsers();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
        }

        [Test]
        public async Task GetAllUsers_ReturnsNoContentForEmptyList()
        {
            // Arrange
            // No need to seed the database for an empty list
            var controller = new UserController(_dbContext, _configuration);

            // Act
            var result = await controller.GetAllUsers();

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task GetAllUsersByLibraryId_ReturnsAllUsers()
        {
            // Arrange
            PopulateDatabase();
            var controller = new UserController(_dbContext, _configuration);

            // Act
            var result = await controller.GetAllUsersByLibraryId(1);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
        }

        [Test]
        public async Task GetAllUsersByLibraryId_ReturnsNoContentForEmptyList()
        {
            // Arrange
            // No need to seed the database for an empty list
            var controller = new UserController(_dbContext, _configuration);

            // Act
            var result = await controller.GetAllUsersByLibraryId(3);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task GetUserById_ReturnsUser()
        {
            // Arrange
            PopulateDatabase();
            var controller = new UserController(_dbContext, _configuration);

            // Act
            var result = await controller.GetUserById(1);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
        }

        [Test]
        public async Task GetUserById_ReturnsNotFound()
        {
            // Arrange
            PopulateDatabase();
            var controller = new UserController(_dbContext, _configuration);

            // Act
            var result = await controller.GetUserById(99);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task Register_ValidInput_ReturnsCreatedAtAction()
        {
            // Arrange
            var controller = new UserController(_dbContext, _configuration);
            var validUserRegisterDTO = new UserRegisterDTO { UserEmail = "test@example.com", UserName = "TestUser", UserPassword = "TestPassword" };

            // Act
            var result = await controller.Register(validUserRegisterDTO);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = result as CreatedAtActionResult;
            Assert.That(createdResult, Is.Not.Null);
        }

        [Test]
        public async Task Register_DuplicateUserEmail_ReturnsBadRequest()
        {
            // Arrange
            PopulateDatabase();
            var controller = new UserController(_dbContext, _configuration);
            var duplicateUserRegisterDTO = new UserRegisterDTO { UserEmail = "user1", UserName = "NewUser", UserPassword = "NewPassword" };

            // Act
            var result = await controller.Register(duplicateUserRegisterDTO);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task Login_ValidCredentials_ReturnsOkResult()
        {
            // Arrange
            PopulateDatabase();
            var controller = new UserController(_dbContext, _configuration);
            var validUserLoginDTO = new UserLoginDTO { UserEmail = "user1", UserPassword = "test" };

            // Act
            var result = await controller.Login(validUserLoginDTO);

            // Assert
            Assert.That(result, Is.InstanceOf<ObjectResult>());
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);
        }

        [Test]
        public async Task Login_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            var controller = new UserController(_dbContext, _configuration);
            var nonExistingUserLoginDTO = new UserLoginDTO { UserEmail = "nonexisting@example.com", UserPassword = "NonExistingPassword" };

            // Act
            var result = await controller.Login(nonExistingUserLoginDTO);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task Login_IncorrectPassword_ReturnsBadRequest()
        {
            // Arrange
            PopulateDatabase();
            var controller = new UserController(_dbContext, _configuration);
            var incorrectPasswordLoginDTO = new UserLoginDTO { UserEmail = "user1", UserPassword = "IncorrectPassword" };

            // Act
            var result = await controller.Login(incorrectPasswordLoginDTO);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task UpdateUser_ReturnsCreatedUser()
        {
            // Arrange
            PopulateDatabase();
            var controller = new UserController(_dbContext, _configuration);
            var user = new UserUpdateDTO { UserName = "NewName", UserEmail = "NewEmail", LibraryId = 2 };

            // Act
            var result = await controller.UpdateUser(1, user);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = result as CreatedAtActionResult;
            Assert.That(createdResult, Is.Not.Null);
        }

        [Test]
        public async Task UpdateUser_ReturnsBadRequest()
        {
            // Arrange
            PopulateDatabase();
            var controller = new UserController(_dbContext, _configuration);
            var user = new UserUpdateDTO { UserName = "NewName", UserEmail = "user2", LibraryId = 2 };

            // Act
            var result = await controller.UpdateUser(1, user);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task UpdateUser_ReturnsNotFound()
        {
            // Arrange
            PopulateDatabase();
            var controller = new UserController(_dbContext, _configuration);
            var user = new UserUpdateDTO { UserName = "NewName", UserEmail = "NewEmail", LibraryId = 2 };

            // Act
            var result = await controller.UpdateUser(999, user);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task UpdatePassword_ReturnsCreatedUser()
        {
            // Arrange
            PopulateDatabase();
            var controller = new UserController(_dbContext, _configuration);
            var user = new UserUpdatePasswordDTO { OldPassword = "test", NewPassword = "newPassword" };

            // Act
            var result = await controller.UpdatePassword(1, user);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = result as CreatedAtActionResult;
            Assert.That(createdResult, Is.Not.Null);
        }

        [Test]
        public async Task UpdatePassword_ReturnsBadRequest()
        {
            // Arrange
            PopulateDatabase();
            var controller = new UserController(_dbContext, _configuration);
            var user = new UserUpdatePasswordDTO { OldPassword = "IncorrectPassword", NewPassword = "newPassword" };

            // Act
            var result = await controller.UpdatePassword(1, user);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task UpdatePassword_ReturnsNotFound()
        {
            // Arrange
            PopulateDatabase();
            var controller = new UserController(_dbContext, _configuration);
            var user = new UserUpdatePasswordDTO { OldPassword = "test", NewPassword = "newPassword" };

            // Act
            var result = await controller.UpdatePassword(999, user);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task DeleteUser_ReturnsNoContent()
        {
            // Arrange
            PopulateDatabase();
            var controller = new UserController(_dbContext, _configuration);

            // Act
            var result = await controller.DeleteUser(1, "test");

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task DeleteUser_ReturnsBadRequest()
        {
            // Arrange
            PopulateDatabase();
            var controller = new UserController(_dbContext, _configuration);

            // Act
            var result = await controller.DeleteUser(1, "IncorrectPassword");

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task DeleteUser_ReturnsNotFound()
        {
            // Arrange
            PopulateDatabase();
            var controller = new UserController(_dbContext, _configuration);

            // Act
            var result = await controller.DeleteUser(999, "test");

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}

