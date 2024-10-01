using GroupLibraryManagment.API.Entities;
using GroupLibraryManagment.API.Persistence;
using GroupLibraryManagment.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace GroupLibraryManagment.API.Controllers
{
    [Route("api/author")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class AuthorController : ControllerBase
    {
        private readonly GroupLibraryManagmentDbContext _context;
        public AuthorController(GroupLibraryManagmentDbContext context)
        {
            _context = context;
        }

        // GET: api/author/all
        /// <summary>
        /// Gets a list of all Authors.
        /// </summary>
        /// <remarks>
        /// Gets a list of all Authors.
        /// </remarks>
        /// <response code="200">Returns a list of Authors</response>
        /// <response code="204">If the list is empty</response> 
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response> 
        [HttpGet("all")]
        [ProducesResponseType(typeof(List<Author>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> GetAllAuthors()
        {
            try
            {
                var authors = await _context.Authors.ToListAsync();

                if (!authors.Any())
                {
                    return NoContent();
                }

                return Ok(authors);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET api/author/id/5
        /// <summary>
        /// Gets an Author by Id.
        /// </summary>
        /// <remarks>
        /// Gets an Author by Id.
        /// </remarks>
        /// <param name="authorId">Author ID</param>
        /// <response code="200">Returns an Author</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">If the Author is not found</response>
        /// <respondes code="500">Internal server error</respondes>
        [HttpGet("id/{authorId}")]
        [ProducesResponseType(typeof(Author), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian, Reader")]
        public async Task<IActionResult> GetAuthorById(int authorId)
        {
            try
            {
                var author = await _context.Authors
                    .Where(a => a.AuthorId == authorId)
                    .SingleOrDefaultAsync();

                if (author == null)
                {
                    return NotFound();
                }

                return Ok(author);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET api/author/name/george
        /// <summary>
        /// Gets an Author by name.
        /// </summary>
        /// <remarks>
        /// Gets an Author by name.
        /// </remarks>
        /// <param name="authorName">Author Name</param>
        /// <response code="200">Returns an Author</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">If the Author is not found</response>
        /// <respondes code="500">Internal server error</respondes> 
        [HttpGet("name/{authorName}")]
        [ProducesResponseType(typeof(Author), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> GetAuthorByName(string authorName)
        {   
            try
            {
                var author = await _context.Authors
                    .Where(a => a.AuthorName == authorName)
                    .SingleOrDefaultAsync();

                if (author == null)
                {
                    return NotFound();
                }

                return Ok(author);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // POST api/author/add
        /// <summary>
        /// Creates a new Author.
        /// </summary>
        /// <remarks>
        /// Creates a new Author.
        /// </remarks>
        /// <response code="201">Returns the newly created Author</response>
        /// <response code="400">If the Author already exists</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("add")]
        [ProducesResponseType(typeof(Author), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> CreateAuthor(AuthorDTO input)
        {
            try
            {
                if (await _context.Authors.AnyAsync(a => a.AuthorName == input.AuthorName))
                {
                    return BadRequest();
                }

                var author = Author.Create(input);

                _context.Authors.Add(author);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAuthorById), new { author.AuthorId }, author);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // PUT api/author/edit/5
        /// <summary>
        /// Edits an Author.
        /// </summary>
        /// <remarks>
        /// Edits an Author.
        /// </remarks>
        /// <param name="authorId">Author ID</param>
        /// <response code="201">Returns the newly edited Author</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">If the Author is not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("edit/{authorId}")]
        [ProducesResponseType(typeof(Author), 201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> UpdateAuthor(int authorId, AuthorDTO input)
        {
            try
            {
                var author = await _context.Authors.SingleOrDefaultAsync(a => a.AuthorId == authorId);

                if (author == null)
                {
                    return NotFound();
                }

                author.Update(input);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAuthorById), new { author.AuthorId }, author);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // DELETE api/author/delete/5
        /// <summary>
        /// Deletes an Author.
        /// </summary>
        /// <remarks>
        /// Deletes an Author.
        /// </remarks>
        /// <param name="authorId">Author ID</param>
        /// <response code="204">Author deleted</response>
        /// <response code="400">If the Author has books</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">If the Author is not found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("delete/{authorId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAuthor(int authorId)
        {
            try
            {
                var author = await _context.Authors.SingleOrDefaultAsync(a => a.AuthorId == authorId);

                if (author == null)
                {
                    return NotFound();
                }

                var bookAuthorsToRemove = await _context.BookAuthors.Where(ba => ba.AuthorId == authorId).ToListAsync();

                if (bookAuthorsToRemove.Any())
                {
                    return BadRequest();
                }

                _context.BookAuthors.RemoveRange(bookAuthorsToRemove);
                _context.Authors.Remove(author);
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
