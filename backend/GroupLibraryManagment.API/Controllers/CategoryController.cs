using GroupLibraryManagment.API.Entities;
using GroupLibraryManagment.API.Persistence;
using GroupLibraryManagment.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace GroupLibraryManagment.API.Controllers
{
    [Route("api/category")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class CategoryController : ControllerBase
    {
        private readonly GroupLibraryManagmentDbContext _context;
        public CategoryController(GroupLibraryManagmentDbContext context)
        {
            _context = context;
        }

        // GET: api/category/all
        /// <summary>
        /// Gets a list of all Categories.
        /// </summary>
        /// <remarks>
        /// Gets a list of all Categories.
        /// </remarks>
        /// <response code="200">Returns a list of Categories</response>
        /// <response code="204">If the list is empty</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response> 
        [HttpGet("all")]
        [ProducesResponseType(typeof(List<Category>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian, Reader")]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categories = await _context.Categories.ToListAsync();

                if (!categories.Any())
                {
                    return NoContent();
                }

                return Ok(categories);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET api/category/id/5
        /// <summary>
        /// Gets a Category by id.
        /// </summary>
        /// <remarks>
        /// Gets a Category by id.
        /// </remarks>
        /// <param name="categoryId">Category ID</param>
        /// <response code="200">Returns a Category</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">If the Category is not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("id/{categoryId}")]
        [ProducesResponseType(typeof(Category), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian, Reader")]
        public async Task<IActionResult> GetCategoryById(int categoryId)
        {
            try
            {
                var category = await _context.Categories
                    .Where(c => c.CategoryId == categoryId)
                    .SingleOrDefaultAsync();

                if (category == null)
                {
                    return NotFound();
                }

                return Ok(category);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET api/category/name/science
        /// <summary>
        /// Gets a Category by name.
        /// </summary>
        /// <remarks>
        /// Gets a Category by name.
        /// </remarks>
        /// <param name="categoryName">Category name</param>
        /// <response code="200">Returns a Category</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">If the Category is not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("name/{categoryName}")]
        [ProducesResponseType(typeof(Category), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> GetCategoryByName(string categoryName)
        {
            try
            {
                var category = await _context.Categories
                    .Where(c => c.CategoryName == categoryName)
                    .SingleOrDefaultAsync();

                if (category == null)
                {
                    return NotFound();
                }

                return Ok(category);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // POST api/category/add
        /// <summary>
        /// Creates a new Category.
        /// </summary>
        /// <remarks>
        /// Creates a new Category.
        /// </remarks>
        /// <response code="201">Returns the newly created Category</response>
        /// <response code="400">If the Category already exists</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("add")]
        [ProducesResponseType(typeof(Category), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> CreateCategory(CategoryDTO input)
        {
            try
            {
                if (await _context.Categories.AnyAsync(c => c.CategoryName == input.CategoryName))
                {
                    return BadRequest();
                }

                var category = Category.Create(input);

                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCategoryById), new { category.CategoryId }, category);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // PUT api/category/edit/5
        /// <summary>
        /// Edits a Category.
        /// </summary>
        /// <remarks>
        /// Edits a Category.
        /// </remarks>
        /// <param name="categoryId">Category ID</param>
        /// <response code="201">Returns the newly edited Category</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">If the Category is not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("edit/{categoryId}")]
        [ProducesResponseType(typeof(Category), 201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> UpdateCategory(int categoryId, CategoryDTO input)
        {
            try
            {
                var category = await _context.Categories.SingleOrDefaultAsync(c => c.CategoryId == categoryId);

                if (category == null)
                {
                    return NotFound();
                }

                category.Update(input);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCategoryById), new { category.CategoryId }, category);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // DELETE api/category/delete/5
        /// <summary>
        /// Deletes a Category.
        /// </summary>
        /// <remarks>
        /// Deletes a Category.
        /// </remarks>
        /// <param name="categoryId">Category ID</param>
        /// <response code="204">Category deleted</response>
        /// <response code="400">If the Category is in use</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">If the Category is not found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("delete/{categoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            try
            {
                var category = _context.Categories.SingleOrDefault(c => c.CategoryId == categoryId);

                if (category == null)
                {
                    return NotFound();
                }

                var bookCategoriesToRemove = await _context.BookCategories.Where(ba => ba.CategoryId == categoryId).ToListAsync();

                if (bookCategoriesToRemove.Any())
                {
                    return BadRequest();
                }

                _context.BookCategories.RemoveRange(bookCategoriesToRemove);
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
