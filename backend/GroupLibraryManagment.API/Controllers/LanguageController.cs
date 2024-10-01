using GroupLibraryManagment.API.Entities;
using GroupLibraryManagment.API.Persistence;
using GroupLibraryManagment.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace GroupLibraryManagment.API.Controllers
{
    [Route("api/language")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class LanguageController : ControllerBase
    {
        private readonly GroupLibraryManagmentDbContext _context;
        public LanguageController(GroupLibraryManagmentDbContext context)
        {
            _context = context;
        }

        // GET: api/language/all
        /// <summary>Get all languages</summary>
        /// <remarks>Gives you a list of all languages</remarks>
        /// <response code="200">Returns the list of all languages</response>
        /// <response code="204">No languages found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("all")]
        [ProducesResponseType(typeof(List<Language>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> GetAllLanguages()
        {
            try
            {
                var languages = await _context.Languages.ToListAsync();

                if (!languages.Any())
                {
                    return NoContent();
                }

                return Ok(languages);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET api/language/id/5
        /// <summary>Get language by id</summary>
        /// <remarks>Gives you a language by id</remarks>
        /// <param name="languageId">Language id</param>
        /// <response code="200">Returns the language</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Language not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("id/{languageId}")]
        [ProducesResponseType(typeof(Language), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> GetLanguageById(int languageId)
        {
            try
            {
                var language = await _context.Languages
                    .Where(l => l.LanguageId == languageId)
                    .SingleOrDefaultAsync();

                if (language == null)
                {
                    return NotFound();
                }

                return Ok(language);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET api/language/alias/en
        /// <summary>Get language by alias</summary>
        /// <remarks>Gives you a language by alias</remarks>
        /// <param name="languageAlias">Language alias</param>
        /// <response code="200">Returns the language</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Language not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("alias/{languageAlias}")]
        [ProducesResponseType(typeof(Language), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> GetLanguageByAlias(string languageAlias)
        {
            try
            {
                var language = await _context.Languages
                    .Where(l => l.LanguageAlias == languageAlias)
                    .SingleOrDefaultAsync();

                if (language == null)
                {
                    return NotFound();
                }

                return Ok(language);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // POST api/language/add
        /// <summary>Create language</summary>
        /// <remarks>Creates a new language</remarks>
        /// <response code="201">Returns the newly created language</response>
        /// <response code="400">Language alias already exists</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("add")]
        [ProducesResponseType(typeof(Language), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> CreateLanguage(LanguageDTO input)
        {
            try
            {
                if (await _context.Languages.AnyAsync(g => g.LanguageAlias == input.LanguageAlias))
                {
                    return BadRequest();
                }

                var language = Language.Create(input);

                _context.Languages.Add(language);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetLanguageById), new { language.LanguageId }, language);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // PUT api/language/edit/5
        /// <summary>Edit language</summary>
        /// <remarks>Edits a language</remarks>
        /// <param name="languageId">Language id</param>
        /// <response code="201">Returns the newly edited language</response>
        /// <response code="400">Language alias already exists</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Language not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("edit/{languageId}")]
        [ProducesResponseType(typeof(Language), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> UpdateLanguage(int languageId, LanguageDTO input)
        {
            try
            {
                var language = await _context.Languages.SingleOrDefaultAsync(l => l.LanguageId == languageId);

                if (language == null)
                {
                    return NotFound();
                }

                if (await _context.Languages.AnyAsync(g => g.LanguageAlias == input.LanguageAlias))
                {
                    return BadRequest();
                }

                language.Update(input);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetLanguageById), new { language.LanguageId }, language);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // DELETE api/language/delete/5
        /// <summary>Delete language</summary>
        /// <remarks>Deletes a language</remarks>
        /// <param name="languageId">Language id</param>
        /// <response code="204">Language deleted</response>
        /// <response code="400">Books contains this language</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Language not found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("delete/{languageId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> DeleteLanguage(int languageId)
        {
            try
            {
                var language = _context.Languages.SingleOrDefault(l => l.LanguageId == languageId);

                if (language == null)
                {
                    return NotFound();
                }

                if (await _context.GenericBooks.Where(g => g.LanguageId == languageId).AnyAsync())
                {
                    return BadRequest();
                }

                _context.Languages.Remove(language);
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

