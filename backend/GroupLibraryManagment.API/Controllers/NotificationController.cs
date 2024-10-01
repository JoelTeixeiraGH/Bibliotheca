using GroupLibraryManagment.API.Entities;
using GroupLibraryManagment.API.Persistence;
using GroupLibraryManagment.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace GroupLibraryManagment.API.Controllers
{
    [Route("api/notification")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class NotificationController : ControllerBase
    {
        private readonly GroupLibraryManagmentDbContext _context;
        public NotificationController(GroupLibraryManagmentDbContext context)
        {
            _context = context;
        }

        // GET: api/notification/all
        /// <summary>Get all notifications</summary>
        /// <remarks>Gives you a list of all notifications</remarks>
        /// <response code="200">Returns the list of notifications</response>
        /// <response code="204">If there are no notifications</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("all")]
        [ProducesResponseType(typeof(List<Notification>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllNotifications()
        {
            try
            {
                var notifications = await _context.Notifications
                    .Include(g => g.User)
                    .Include(g => g.Request)
                    .Include(g => g.Library)
                    .Select(g => new
                    {
                        g.NotificationId,
                        g.NotificationTitle,
                        g.NotificationDescription,
                        emittedDate = g.EmittedDate.ToShortDateString(),
                        endDate = g.EndDate.HasValue ? g.EndDate.Value.ToShortDateString() : null,
                        g.ForAll,
                        User = g.User != null ? new
                        {
                            g.User.UserId,
                            g.User.UserName
                        } : null,
                        Request = g.Request != null ? new
                        {
                            g.Request.RequestId,
                            g.Request.RequestStatus
                        } : null,
                        Library = g.Library != null ? new
                        {
                            g.Library.LibraryId,
                            g.Library.LibraryAlias
                        } : null,
                    })
                    .ToListAsync();

                if (!notifications.Any())
                {
                    return NoContent();
                }

                return Ok(notifications);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET api/notification/5
        /// <summary>Get notification by id</summary>
        /// <remarks>Gives you a notification by id</remarks>
        /// <param name="notificationId">Notification id</param>
        /// <response code="200">Returns the notification</response>
        /// <response code="404">If the notification is not found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{notificationId}")]
        [ProducesResponseType(typeof(Notification), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> GetNotificationById(int notificationId)
        {
            try
            {
                var notification = await _context.Notifications
                    .Where(n => n.NotificationId == notificationId)
                    .Include(g => g.User)
                    .Include(g => g.Request)
                    .Include(g => g.Library)
                    .Select(g => new
                    {
                        g.NotificationId,
                        g.NotificationTitle,
                        g.NotificationDescription,
                        emittedDate = g.EmittedDate.ToShortDateString(),
                        endDate = g.EndDate.HasValue ? g.EndDate.Value.ToShortDateString() : null,
                        g.ForAll,
                        User = g.User != null ? new
                        {
                            g.User.UserId,
                            g.User.UserName
                        } : null,
                        Request = g.Request != null ? new
                        {
                            g.Request.RequestId,
                            g.Request.RequestStatus
                        } : null,
                        Library = g.Library != null ? new
                        {
                            g.Library.LibraryId,
                            g.Library.LibraryAlias
                        } : null,
                    })
                    .SingleOrDefaultAsync();

                if (notification == null)
                {
                    return NotFound();
                }

                return Ok(notification);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET api/notification/all/user/1
        /// <summary>Get all notifications by user id</summary>
        /// <remarks>Gives you a list of all notifications by user id</remarks>
        /// <param name="userId">User id</param>
        /// <response code="200">Returns the list of notifications</response>
        /// <response code="204">If there are no notifications</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("all/user/{userId}")]
        [ProducesResponseType(typeof(List<Notification>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian, Reader")]
        public async Task<IActionResult> GetAllNotificationsByUserId(int userId)
        {
            try
            {
                DateTime currentDate = DateTime.Now;

                var notificationsInUser = await _context.Notifications
                    .Where(n => n.UserId == userId && n.EndDate >= currentDate)
                    .Select(g => new
                    {
                        g.NotificationId,
                        g.NotificationTitle,
                        g.NotificationDescription,
                        emittedDate = g.EmittedDate.ToShortDateString(),
                        endDate = g.EndDate.HasValue ? g.EndDate.Value.ToShortDateString() : null,
                        g.ForAll,
                        User = g.User != null ? new
                        {
                            g.User.UserId,
                            g.User.UserName
                        } : null,
                        Request = g.Request != null ? new
                        {
                            g.Request.RequestId,
                            g.Request.RequestStatus
                        } : null,
                        Library = g.Library != null ? new
                        {
                            g.Library.LibraryId,
                            g.Library.LibraryAlias
                        } : null,
                    })
                    .ToListAsync();

                    if (!notificationsInUser.Any())
                    {
                        return NoContent();
                    }

                    return Ok(notificationsInUser);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
            
        }

        // GET api/notification/all/library/1/user
        /// <summary>Get all notifications by library id for user</summary>
        /// <remarks>Gives you a list of all notifications by library id for user</remarks>
        /// <param name="libraryId">Library id</param>
        /// <response code="200">Returns the list of notifications</response>
        /// <response code="204">If there are no notifications</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("all/library/{libraryId}/user")]
        [ProducesResponseType(typeof(List<Notification>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian, Reader")]
        public async Task<IActionResult> GetAllNotificationsByLibraryIdForUser(int libraryId)
        {
            try
            {
                DateTime currentDate = DateTime.Now;

                var notificationsInLibrary = await _context.Notifications
                    .Where(n => n.LibraryId == libraryId && n.EndDate >= currentDate && n.ForAll)
                    .Select(g => new
                    {
                        g.NotificationId,
                        g.NotificationTitle,
                        g.NotificationDescription,
                        emittedDate = g.EmittedDate.ToShortDateString(),
                        endDate = g.EndDate.HasValue ? g.EndDate.Value.ToShortDateString() : null,
                        g.ForAll,
                        User = g.User != null ? new
                        {
                            g.User.UserId,
                            g.User.UserName
                        } : null,
                        Request = g.Request != null ? new
                        {
                            g.Request.RequestId,
                            g.Request.RequestStatus
                        } : null,
                        Library = g.Library != null ? new
                        {
                            g.Library.LibraryId,
                            g.Library.LibraryAlias
                        } : null,
                    })
                    .ToListAsync();

                if (!notificationsInLibrary.Any())
                {
                    return NoContent();
                }

                return Ok(notificationsInLibrary);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }

        }

        // GET api/notification/all/library/1/library
        /// <summary>Get all notifications by library id for library</summary>
        /// <remarks>Gives you a list of all notifications by library id for library</remarks>
        /// <param name="libraryId">Library id</param>
        /// <response code="200">Returns the list of notifications</response>
        /// <response code="204">If there are no notifications</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("all/library/{libraryId}/library")]
        [ProducesResponseType(typeof(List<Notification>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> GetAllNotificationsByLibraryIdForLibrary(int libraryId)
        {
            try
            {
                DateTime currentDate = DateTime.Now;

                var notificationsInLibrary = await _context.Notifications
                    .Where(n => n.LibraryId == libraryId && n.EndDate >= currentDate)
                    .Select(n => new
                    {
                        n.NotificationId,
                        n.NotificationTitle,
                        n.NotificationDescription,
                        emittedDate = n.EmittedDate.ToShortDateString(),
                        endDate = n.EndDate.HasValue ? n.EndDate.Value.ToShortDateString() : null,
                        n.ForAll,
                        n.LibraryId,
                    })
                    .ToListAsync();

                if (!notificationsInLibrary.Any())
                {
                    return NoContent();
                }

                return Ok(notificationsInLibrary);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }

        }

        // GET api/notification/all/request/1
        /// <summary>Get all notifications by user id from requests</summary>
        /// <remarks>Gives you a list of all notifications by user id from requests</remarks>
        /// <param name="userId">User id</param>
        /// <response code="200">Returns the list of notifications</response>
        /// <response code="204">If there are no notifications</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("all/request/{userId}")]
        [ProducesResponseType(typeof(List<Notification>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian, Reader")]
        public async Task<IActionResult> GetAllNotificationsFromRequestsByUserId(int userId)
        {
            try
            {
                DateTime currentDate = DateTime.Now;

                var requestsInUser = await _context.Requests
                    .Where(r => r.UserId == userId)
                    .Select(r => r.RequestId)
                    .ToListAsync();

                var notificationsInUser = await _context.Notifications
                    .Where(n => requestsInUser.Contains(n.RequestId.HasValue ? n.RequestId.Value : 0) && n.EndDate >= currentDate)
                    .Select(g => new
                    {
                        g.NotificationId,
                        g.NotificationTitle,
                        g.NotificationDescription,
                        emittedDate = g.EmittedDate.ToShortDateString(),
                        endDate = g.EndDate.HasValue ? g.EndDate.Value.ToShortDateString() : null,
                        g.ForAll,
                        User = g.User != null ? new
                        {
                            g.User.UserId,
                            g.User.UserName
                        } : null,
                        Request = g.Request != null ? new
                        {
                            g.Request.RequestId,
                            g.Request.RequestStatus
                        } : null,
                        Library = g.Library != null ? new
                        {
                            g.Library.LibraryId,
                            g.Library.LibraryAlias
                        } : null,
                    })
                    .ToListAsync();

                if (!notificationsInUser.Any())
                {
                    return NoContent();
                }

                return Ok(notificationsInUser);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // POST api/notification/add
        /// <summary>Create notification</summary>
        /// <remarks>Create a new notification</remarks>
        /// <response code="201">Returns the newly created notification</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("add")]
        [ProducesResponseType(typeof(Notification), 201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> CreateNotification(NotificationDTO input)
        {
            try
            {
                var notification = Notification.CreateGlobalNotification(input);

                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetNotificationById), new { notification.NotificationId }, notification);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // PUT api/notification/edit/5
        /// <summary>Update notification</summary>
        /// <remarks>Update an existing notification</remarks>
        /// <param name="notificationId">Notification id</param>
        /// <response code="201">Returns the newly updated notification</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">If the notification is not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("edit/{notificationId}")]
        [ProducesResponseType(typeof(Notification), 201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateNotification(int notificationId, NotificationDTO input)
        {
            try
            {
                var notification = await _context.Notifications.SingleOrDefaultAsync(d => d.NotificationId == notificationId);

                if (notification == null)
                {
                    return NotFound();
                }

                notification.Update(input);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetNotificationById), new { notification.NotificationId }, notification);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // DELETE api/notification/delete/5
        /// <summary>Delete notification</summary>
        /// <remarks>Delete an existing notification</remarks>
        /// <param name="notificationId">Notification id</param>
        /// <response code="204">Returns no content</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">If the notification is not found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("delte/{notificationId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteNotification(int notificationId)
        {
            try
            {
                var notification = _context.Notifications.SingleOrDefault(d => d.NotificationId == notificationId);

                if (notification == null)
                {
                    return NotFound();
                }

                _context.Notifications.Remove(notification);
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
