using GroupLibraryManagment.API.Entities;
using GroupLibraryManagment.API.Persistence;
using GroupLibraryManagment.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace GroupLibraryManagment.API.Controllers
{
    [Route("api/punishment")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class PunishmentController : ControllerBase
    {
        private readonly GroupLibraryManagmentDbContext _context;
        public PunishmentController(GroupLibraryManagmentDbContext context)
        {
            _context = context;
        }

        // GET: api/punishment/all
        /// <summary>Get all punishments</summary>
        /// <remarks>Gives you a list of all punishments</remarks>
        /// <response code="200">Returns the list of punishments</response>
        /// <response code="204">No punishments were found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("all")]
        [ProducesResponseType(typeof(List<Punishment>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> GetAllPunishments()
        {
            try
            {
                var punishments = await _context.Punishments
                    .Include(p => p.Request).ThenInclude(r => r.User)
                    .Select(p => new
                    {
                        p.PunishmentId,
                        p.PunishmentReason,
                        p.PunishmentLevel,
                        emittedDate = p.EmittedDate.ToShortDateString(),
                        Request = new
                        {
                            p.RequestId,
                            p.Request.RequestStatus,
                            p.Request.User.UserId,
                            p.Request.User.UserName,
                        },
                    })
                    .ToListAsync();

                if (!punishments.Any())
                {
                    return NoContent();
                }

                return Ok(punishments);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET api/punishment/5
        /// <summary>Get punishment by id</summary>
        /// <remarks>Gives you a specific punishment by id</remarks>
        /// <param name="punishmentId">The id of the punishment you want to get</param>
        /// <response code="200">Returns the punishment</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Punishment was not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{punishmentId}")]
        [ProducesResponseType(typeof(Punishment), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> GetPunishmentById(int punishmentId)
        {
            try
            {
                var punishment = await _context.Punishments
                    .Where(p => p.PunishmentId == punishmentId)
                    .Include(p => p.Request).ThenInclude(r => r.User)
                    .Select(p => new
                    {
                        p.PunishmentId,
                        p.PunishmentReason,
                        p.PunishmentLevel,
                        emittedDate = p.EmittedDate.ToShortDateString(),
                        Request = new
                        {
                            p.RequestId,
                            p.Request.RequestStatus,
                            p.Request.User.UserId,
                            p.Request.User.UserName,
                        },
                    })
                    .SingleOrDefaultAsync();

                if (punishment == null)
                {
                    return NotFound();
                }

                return Ok(punishment);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET: api/punishment/all/user/1
        /// <summary>Get all punishments by user id</summary>
        /// <remarks>Gives you a list of all punishments by user id</remarks>
        /// <param name="userId">The id of the user you want to get punishments for</param>
        /// <response code="200">Returns the list of punishments</response>
        /// <response code="204">No punishments were found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("all/user/{userId}")]
        [ProducesResponseType(typeof(List<Punishment>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian, Reader")]
        public async Task<IActionResult> GetAllPunishmentsByUserId(int userId)
        {
            try
            {
                var punishments = await _context.Punishments
                    .Where(p => p.Request.UserId == userId)
                    .Include(p => p.Request).ThenInclude(r => r.User)
                    .Select(p => new
                    {
                        p.PunishmentId,
                        p.PunishmentReason,
                        p.PunishmentLevel,
                        emittedDate = p.EmittedDate.ToShortDateString(),
                        Request = new
                        {
                            p.RequestId,
                            p.Request.RequestStatus,
                            p.Request.User.UserId,
                            p.Request.User.UserName,
                        },
                    })
                    .ToListAsync();

                if (!punishments.Any())
                {
                    return NoContent();
                }

                return Ok(punishments);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET: api/punishment/all/request/1
        /// <summary>Get punishment by request id</summary>
        /// <remarks>Gives you the punishment by request id</remarks>
        /// <param name="requestId">The id of the request you want to get punishment for</param>
        /// <response code="200">Returns punishment</response>
        /// <response code="204">No punishments were found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("request/{requestId}")]
        [ProducesResponseType(typeof(List<Punishment>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian, Reader")]
        public async Task<IActionResult> GetPunishmentByRequestId(int requestId)
        {
            try
            {
                var punishment = await _context.Punishments
                    .Where(p => p.RequestId == requestId)
                    .Include(p => p.Request).ThenInclude(r => r.User)
                    .Select(p => new
                    {
                        p.PunishmentId,
                        p.PunishmentReason,
                        p.PunishmentLevel,
                        emittedDate = p.EmittedDate.ToShortDateString(),
                        Request = new
                        {
                            p.RequestId,
                            p.Request.RequestStatus,
                            p.Request.User.UserId,
                            p.Request.User.UserName,
                        },
                    })
                    .SingleOrDefaultAsync();

                if (punishment == null)
                {
                    return NoContent();
                }

                return Ok(punishment);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // POST api/punishment/add
        /// <summary>Create a punishment</summary>
        /// <remarks>Create a punishment</remarks>
        /// <response code="201">Returns the created punishment</response>
        /// <response code="400">Punishment already exist for this requet</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("add")]
        [ProducesResponseType(typeof(Punishment), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> CreatePunishment(PunishmentDTO input)
        {
            try
            {
                if(await _context.Punishments.AnyAsync(p => p.RequestId == input.RequestId))
                {
                    return BadRequest();
                }

                var punishment = Punishment.Create(input);
                _context.Punishments.Add(punishment);

                var notification = Notification.CreateNotificationForPunishment(punishment.RequestId, punishment.PunishmentReason);
                _context.Notifications.Add(notification);

                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetPunishmentById), new { punishment.PunishmentId }, punishment);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // PUT api/punishment/edit/5
        /// <summary>Update a punishment</summary>
        /// <remarks>Update a punishment</remarks>
        /// <param name="punishmentId">The id of the punishment you want to update</param>
        /// <response code="201">Returns the updated punishment</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Punishment was not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("edit/{punishmentId}")]
        [ProducesResponseType(typeof(Punishment), 201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> UpdatePunishment(int punishmentId, PunishmentDTO input)
        {
            try
            {
                var punishment = await _context.Punishments.SingleOrDefaultAsync(d => d.PunishmentId == punishmentId);

                if (punishment == null)
                {
                    return NotFound();
                }

                punishment.Update(input);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetPunishmentById), new { punishment.PunishmentId }, punishment);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // PUT api/punishment/edit/5/level/3
        /// <summary>Update a punishment level</summary>
        /// <remarks>Update a punishment level</remarks>
        /// <param name="punishmentId">The id of the punishment you want to update</param>
        /// <param name="level">The level of the punishment you want to update</param>
        /// <response code="201">Returns the updated punishment</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Punishment was not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("edit/{punishmentId}/level/{level}")]
        [ProducesResponseType(typeof(Punishment), 201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> UpdatePunishmentLevel(int punishmentId, PunishmentLevel level)
        {
            try
            {
                var punishment = await _context.Punishments.SingleOrDefaultAsync(d => d.PunishmentId == punishmentId);

                if (punishment == null)
                {
                    return NotFound();
                }

                punishment.UpdateLevel(level);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetPunishmentById), new { punishment.PunishmentId }, punishment);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // DELETE api/punishment/delete/5
        /// <summary>Delete a punishment</summary>
        /// <remarks>Delete a punishment</remarks>
        /// <param name="punishmentId">The id of the punishment you want to delete</param>
        /// <response code="204">Punishment was deleted</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Punishment was not found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("delete/{punishmentId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> DeletePunishment(int punishmentId)
        {
            try
            {
                var punishment = _context.Punishments.SingleOrDefault(d => d.PunishmentId == punishmentId);

                if (punishment == null)
                {
                    return NotFound();
                }

                _context.Punishments.Remove(punishment);
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
