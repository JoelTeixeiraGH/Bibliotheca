using GroupLibraryManagment.API.Entities;
using GroupLibraryManagment.API.Persistence;
using GroupLibraryManagment.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace GroupLibraryManagment.API.Controllers
{
    [Route("api/library")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class LibraryController : ControllerBase
    {
        private readonly GroupLibraryManagmentDbContext _context;
        public LibraryController(GroupLibraryManagmentDbContext context)
        {
            _context = context;
        }

        // GET: api/library/all
        /// <summary>Get all libraries</summary>
        /// <remarks>Gives you a list of all libraries</remarks>
        /// <response code="200">Returns the list of libraries</response>
        /// <response code="204">No libraries were found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("all")]
        [ProducesResponseType(typeof(List<Library>),200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        // [Authorize(Roles = "Admin, Librarian, Reader")]
        public async Task<IActionResult> GetAllLibraries()
        {
            try
            {
                var libraries = await _context.Libraries.ToListAsync();

                if (!libraries.Any())
                {
                    return NoContent();
                }

                return Ok(libraries);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET api/library/5
        /// <summary>Get library by id</summary>
        /// <remarks>Gives you a library by id</remarks>
        /// <param name="libraryId">Library id</param>
        /// <response code="200">Returns the library</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Library not found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("id/{libraryId}")]
        [ProducesResponseType(typeof(Library),200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian, Reader")]
        public async Task<IActionResult> GetLibraryById(int libraryId)
        {
            try
            {
                var library = await _context.Libraries
                    .Where(l => l.LibraryId == libraryId)
                    .SingleOrDefaultAsync();

                if (library == null)
                {
                    return NotFound();
                }

                return Ok(library);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET api/library/alias/estg
        /// <summary>Get library by alias</summary>
        /// <remarks>Gives you a library by alias</remarks>
        /// <response code="200">Returns the library</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Library not found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("alias/{libraryAlias}")]
        [ProducesResponseType(typeof(Library),200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> GetLibraryByAlias(string libraryAlias)
        {
            try
            {
                var library = await _context.Libraries
                    .Where(l => l.LibraryAlias == libraryAlias)
                    .SingleOrDefaultAsync();

                if (library == null)
                {
                    return NotFound();
                }

                return Ok(library);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // POST api/library/add
        /// <summary>Create a new library</summary>
        /// <remarks>Create a new library</remarks>
        /// <response code="201">Returns the created library</response>
        /// <response code="400">Library alias already exists</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost("add")]
        [ProducesResponseType(typeof(Library),201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateLibrary(LibraryDTO input)
        {
            try
            {
                if (await _context.Libraries.AnyAsync(g => g.LibraryAlias == input.LibraryAlias))
                {
                    return BadRequest();
                }

                var library = Library.Create(input);

                _context.Libraries.Add(library);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetLibraryById), new { library.LibraryId }, library);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // PUT api/library/edit/5
        /// <summary>Edit library</summary>
        /// <remarks>Edit library</remarks>
        /// <param name="libraryId">Library id</param>
        /// <response code="201">Returns the edited library</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Library not found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("edit/{libraryId}")]
        [ProducesResponseType(typeof(Library),201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateLibrary(int libraryId, LibraryDTO input)
        {
            try
            {
                var library = await _context.Libraries.SingleOrDefaultAsync(l => l.LibraryId == libraryId);

                if (library == null)
                {
                    return NotFound();
                }

                library.Update(input);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetLibraryById), new { library.LibraryId }, library);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // DELETE api/library/delete/5
        /// <summary>Delete library</summary>
        /// <remarks>Delete library</remarks>
        /// <param name="libraryId">Library id</param>
        /// <response code="204">Library deleted</response>
        /// <response code="400">Library contains users</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Library not found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpDelete("delete/{libraryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteLibrary(int libraryId)
        {
            try
            {
                var library = _context.Libraries.SingleOrDefault(d => d.LibraryId == libraryId);

                if (library == null)
                {
                    return NotFound();
                }

                if (await _context.Users.Where(u => u.LibraryId == libraryId).AnyAsync())
                {
                    return BadRequest();
                }

                _context.Libraries.Remove(library);
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
