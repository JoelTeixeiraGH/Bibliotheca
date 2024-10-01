using GroupLibraryManagment.API.Entities;
using GroupLibraryManagment.API.Persistence;
using GroupLibraryManagment.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace GroupLibraryManagment.API.Controllers
{
    [Route("api/evaluation")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class EvaluationController : ControllerBase
    {
        private readonly GroupLibraryManagmentDbContext _context;
        public EvaluationController(GroupLibraryManagmentDbContext context)
        {
            _context = context;
        }

        // GET: api/evaluation/all
        /// <summary>Get all evaluations</summary>
        /// <remarks>Get all evaluations</remarks>
        /// <response code="200">Returns all evaluations</response>
        /// <response code="204">No evaluations</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("all")]
        [ProducesResponseType(typeof(List<Evaluation>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> GetAllEvaluations()
        {
            try
            {
                var evaluations = await _context.Evaluations
                    .Include(e => e.GenericBook)
                    .Include(e => e.User)
                    .Select(e => new
                    {
                        e.EvaluationId,
                        e.EvaluationDescription,
                        e.EvaluationScore,
                        emittedDate = e.EmittedDate.ToShortDateString(),
                        GenericBook = new
                        {
                            e.ISBN,
                            e.GenericBook.Title
                        },
                        User = new
                        {
                            e.UserId,
                            e.User.UserName,
                            e.User.UserEmail
                        },
                    })
                    .ToListAsync();

                if (!evaluations.Any())
                {
                    return NoContent();
                }

                return Ok(evaluations);
            }
            catch (Exception ex) 
                {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET api/evaluation/id/5
        /// <summary>Get evaluation by id</summary>
        /// <remarks>Get evaluation by id</remarks>
        /// <param name="evaluationId">Evaluation id</param>
        /// <response code="200">Returns evaluation</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Evaluation not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("id/{evaluationId}")]
        [ProducesResponseType(typeof(Evaluation), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian, Reader")]
        public async Task<IActionResult> GetEvaluationById(int evaluationId)
        {
            try
            {
                var evaluation = await _context.Evaluations.Where(e => e.EvaluationId == evaluationId)
                .Include(e => e.GenericBook)
                .Include(e => e.User)
                .Select(e => new
                {
                    e.EvaluationId,
                    e.EvaluationDescription,
                    e.EvaluationScore,
                    emittedDate = e.EmittedDate.ToShortDateString(),
                    GenericBook = new
                    {
                        e.ISBN,
                        e.GenericBook.Title
                    },
                    User = new
                    {
                        e.UserId,
                        e.User.UserName,
                        e.User.UserEmail
                    },
                })
                .SingleOrDefaultAsync();

                if (evaluation == null)
                {
                    return NotFound();
                }

                return Ok(evaluation);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
            
        }

        // GET api/evaluation/all/user/1
        /// <summary>Get all evaluations by user id</summary>
        /// <remarks>Get all evaluations by user id</remarks>
        /// <param name="userId">User id</param>
        /// <response code="200">Returns all evaluations by user id</response>
        /// <response code="204">No evaluations by user id</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("all/user/{userId}")]
        [ProducesResponseType(typeof(List<Evaluation>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian, Reader")]
        public async Task<IActionResult> GetAllEvaluationsByUserId(int userId)
        {
            try
            {
                var evaluationsInUser = await _context.Evaluations.Where(p => p.UserId == userId)
                    .Include(e => e.GenericBook)
                    .Select(p => new
                    {
                        p.EvaluationId,
                        p.EvaluationDescription,
                        p.EvaluationScore,
                        emittedDate = p.EmittedDate.ToShortDateString(),
                        p.GenericBook.ISBN,
                        p.GenericBook.Title
                    })
                    .ToListAsync();

                if (!evaluationsInUser.Any())
                {
                    return NoContent();
                }

                return Ok(evaluationsInUser);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET api/evaluation/all/book/112414121241
        /// <summary>Get all evaluations by book isbn</summary>
        /// <remarks>Get all evaluations by book isbn</remarks>
        /// <param name="isbn">Book isbn</param>
        /// <response code="200">Returns all evaluations by user id</response>
        /// <response code="204">No evaluations by user id</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("all/book/{isbn}")]
        [ProducesResponseType(typeof(List<Evaluation>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian, Reader")]
        public async Task<IActionResult> GetAllEvaluationsByISBN(string isbn)
        {
            try
            {
                var evaluationsInBook = await _context.Evaluations.Where(p => p.ISBN == isbn)
                    .Include(e => e.User)
                    .Select(p => new
                    {
                        p.EvaluationId,
                        p.EvaluationDescription,
                        p.EvaluationScore,
                        emittedDate = p.EmittedDate.ToShortDateString(),
                        p.UserId,
                        p.User.UserName
                    })
                    .ToListAsync();

                if (!evaluationsInBook.Any())
                {
                    return NoContent();
                }

                return Ok(evaluationsInBook);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        // POST api/evaluation/add
        /// <summary>Create evaluation</summary>
        /// <remarks>Create evaluation</remarks>
        /// <response code="201">Returns created evaluation</response>
        /// <response code="400">User already evaluated this book</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("add")]
        [ProducesResponseType(typeof(Evaluation), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian, Reader")]
        public async Task<IActionResult> CreateEvaluation(EvaluationDTO input)
        {
            try
            {
                if(!await _context.Users.AnyAsync(u => u.UserId == input.UserId))
                {
                    return BadRequest();
                }
                
                if (await _context.Evaluations.AnyAsync(e => e.ISBN == input.ISBN && e.UserId == input.UserId))
                {
                    return BadRequest();
                }
                var evaluation = Evaluation.Create(input);

                _context.Evaluations.Add(evaluation);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetEvaluationById), new { evaluation.EvaluationId }, evaluation);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // PUT api/evaluation/edit/5
        /// <summary>Edit evaluation</summary>
        /// <remarks>Edit evaluation</remarks>
        /// <param name="evaluationId">Evaluation id</param>
        /// <response code="201">Returns edited evaluation</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Evaluation not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("edit/{evaluationId}")]
        [ProducesResponseType(typeof(Evaluation), 201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian, Reader")]
        public async Task<IActionResult> UpdateEvaluation(int evaluationId, EvaluationDTO input)
        {
            try
            {
                var evaluation = await _context.Evaluations.SingleOrDefaultAsync(d => d.EvaluationId == evaluationId);

                if (evaluation == null)
                {
                    return NotFound();
                }

                evaluation.Update(input);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetEvaluationById), new { evaluation.EvaluationId }, evaluation);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // DELETE api/evaluation/delete/5
        /// <summary>Delete evaluation</summary>
        /// <remarks>Delete evaluation</remarks>
        /// <param name="evaluationId">Evaluation id</param>
        /// <response code="204">Evaluation deleted</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Evaluation not found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("delete/{evaluationId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian, Reader")]
        public async Task<IActionResult> DeleteEvaluation(int evaluationId)
        {
            try
            {
                var evaluation = _context.Evaluations.SingleOrDefault(d => d.EvaluationId == evaluationId);

                if (evaluation == null)
                {
                    return NotFound();
                }

                _context.Evaluations.Remove(evaluation);
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
