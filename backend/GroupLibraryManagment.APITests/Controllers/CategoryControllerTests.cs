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
    public class CategoryControllerTests
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
            var mockCategories = new List<Category>
            {
                new Category { CategoryId = 1, CategoryName = "Category1" },
                new Category { CategoryId = 2, CategoryName = "Category2" },
            };

            var associatedBookCategory = new BookCategory { CategoryId = 2, ISBN = "2" };

            _dbContext.Categories.AddRange(mockCategories);
            _dbContext.BookCategories.Add(associatedBookCategory);
            await _dbContext.SaveChangesAsync();
        }

        [TearDown]
        public void TearDown()
        {
            // Optionally dispose of the DbContext to release resources
            _dbContext.Dispose();
        }

        [Test]
        public async Task GetAllCategories_ReturnsListOfCategories()
        {
            // Arrange
            PopulateDatabase();
            var controller = new CategoryController(_dbContext);

            // Act
            var result = await controller.GetAllCategories();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var categories = okResult.Value as List<Category>;
            Assert.That(categories, Is.Not.Null);
            Assert.That(categories.Count, Is.EqualTo(_dbContext.Categories.Count()));
        }

        [Test]
        public async Task GetAllCategories_ReturnsNoContentForEmptyList()
        {
            // Arrange
            // No need to seed the database for an empty list
            var controller = new CategoryController(_dbContext);

            // Act
            var result = await controller.GetAllCategories();

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task GetCategoryById_ExistingCategory_ReturnsOkResult()
        {
            // Arrange
            PopulateDatabase();
            var controller = new CategoryController(_dbContext);
            var existingCategoryId = 1;

            // Act
            var result = await controller.GetCategoryById(existingCategoryId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.InstanceOf<Category>());
            var category = (Category)okResult.Value;
            Assert.That(category.CategoryId, Is.EqualTo(existingCategoryId));
        }

        [Test]
        public async Task GetCategoryById_NonExistingCategory_ReturnsNotFoundResult()
        {
            // Arrange
            var controller = new CategoryController(_dbContext);
            var nonExistingCategoryId = 999;

            // Act
            var result = await controller.GetCategoryById(nonExistingCategoryId);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task GetCategoryByName_ExistingCategory_ReturnsOkResult()
        {
            // Arrange
            PopulateDatabase();
            var controller = new CategoryController(_dbContext);
            var existingCategoryName = "Category1";

            // Act
            var result = await controller.GetCategoryByName(existingCategoryName);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.InstanceOf<Category>());
            var category = (Category)okResult.Value;
            Assert.That(category.CategoryName, Is.EqualTo(existingCategoryName));
        }

        [Test]
        public async Task GetCategoryByName_NonExistingCategory_ReturnsNotFoundResult()
        {
            // Arrange
            var controller = new CategoryController(_dbContext);
            var nonExistingCategoryName = "NonExistingCategoryName";

            // Act
            var result = await controller.GetCategoryByName(nonExistingCategoryName);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task CreateCategory_ValidInput_ReturnsCreatedAtAction()
        {
            // Arrange
            var controller = new CategoryController(_dbContext);
            var validCategoryDTO = new CategoryDTO { CategoryName = "NewCategory" };

            // Act
            var result = await controller.CreateCategory(validCategoryDTO);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var createdAtActionResult = (CreatedAtActionResult)result;

            Assert.That(createdAtActionResult.ActionName, Is.EqualTo(nameof(CategoryController.GetCategoryById)));

            Assert.That(createdAtActionResult.RouteValues, Is.Not.Null);
            Assert.That(createdAtActionResult.RouteValues.ContainsKey("categoryId"));
        }

        [Test]
        public async Task CreateCategory_DuplicateCategoryName_ReturnsBadRequest()
        {
            // Arrange
            PopulateDatabase();
            var controller = new CategoryController(_dbContext);
            var duplicateCategoryDTO = new CategoryDTO { CategoryName = "Category1" };

            // Act
            var result = await controller.CreateCategory(duplicateCategoryDTO);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task UpdateCategory_ExistingCategory_ReturnsCreatedAtAction()
        {
            // Arrange
            PopulateDatabase();
            var controller = new CategoryController(_dbContext);
            var existingCategory = _dbContext.Categories.First();
            var updatedCategoryDTO = new CategoryDTO { CategoryName = "UpdatedCategory" };

            // Act
            var result = await controller.UpdateCategory(existingCategory.CategoryId, updatedCategoryDTO);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var createdAtActionResult = (CreatedAtActionResult)result;

            Assert.That(createdAtActionResult.ActionName, Is.EqualTo(nameof(CategoryController.GetCategoryById)));

            Assert.That(createdAtActionResult.RouteValues, Is.Not.Null);
            Assert.That(createdAtActionResult.RouteValues.ContainsKey("categoryId"));
            Assert.That(createdAtActionResult.RouteValues["categoryId"], Is.EqualTo(existingCategory.CategoryId));

        }

        [Test]
        public async Task UpdateCategory_NonExistingCategory_ReturnsNotFound()
        {
            // Arrange
            var controller = new CategoryController(_dbContext);
            var nonExistingCategoryId = 999;
            var updatedCategoryDTO = new CategoryDTO { CategoryName = "UpdatedCategory" };

            // Act
            var result = await controller.UpdateCategory(nonExistingCategoryId, updatedCategoryDTO);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task DeleteCategory_ExistingCategory_NoAssociatedBooks_ReturnsNoContent()
        {
            // Arrange
            PopulateDatabase();
            var existingCategory = _dbContext.Categories.First();
            var controller = new CategoryController(_dbContext);

            // Act
            var result = await controller.DeleteCategory(existingCategory.CategoryId);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task DeleteCategory_ExistingCategory_WithAssociatedBooks_ReturnsBadRequest()
        {
            // Arrange
            PopulateDatabase();
            var existingCategory = _dbContext.Categories.Last();
            var controller = new CategoryController(_dbContext);

            // Act
            var result = await controller.DeleteCategory(existingCategory.CategoryId);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task DeleteCategory_NonExistingCategory_ReturnsNotFound()
        {
            // Arrange
            var controller = new CategoryController(_dbContext);
            var nonExistingCategoryId = 999;

            // Act
            var result = await controller.DeleteCategory(nonExistingCategoryId);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}

