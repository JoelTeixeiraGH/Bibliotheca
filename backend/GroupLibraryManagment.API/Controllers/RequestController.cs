using GroupLibraryManagment.API.Entities;
using GroupLibraryManagment.API.Persistence;
using GroupLibraryManagment.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace GroupLibraryManagment.API.Controllers
{
    [Route("api/request")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class RequestController : ControllerBase
    {
        private readonly GroupLibraryManagmentDbContext _context;
        public RequestController(GroupLibraryManagmentDbContext context)
        {
            _context = context;
        }
        private async Task<bool> CheckIfAlreadyHasActiveRequestForBook(string isbn, int userId)
        {
            var request = await _context.Requests.FirstOrDefaultAsync(r => r.ISBN == isbn && r.UserId == userId && r.RequestStatus != RequestStatus.Canceled && r.RequestStatus != RequestStatus.Returned);

            if (request == null)
            {
                return false;
            }

            return true;
        }
        private async Task<bool> CheckIfHasMoreThanThreeActiveRequests(int userId)
        {
            var requests = await _context.Requests.Where(r => r.UserId == userId && r.RequestStatus != RequestStatus.Canceled && r.RequestStatus != RequestStatus.Returned).ToListAsync();

            if (requests.Count >= 3)
            {
                return true;
            }

            return false;
        }

        // GET: api/request/all
        /// <summary>Get all requests</summary>
        /// <remarks>Gives you a list of all requests</remarks>
        /// <response code="200">Returns the list of requests</response>
        /// <response code="204">No requests were found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("all")]
        [ProducesResponseType(typeof(List<Request>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> GetAllRequests()
        {
            try
            {
                var requests = await _context.Requests
                    .Include(r => r.User)
                    .Include(r => r.PhysicalBook)
                    .Include(r => r.Library)
                    .Select(r => new
                    {
                        r.RequestId,
                        r.RequestStatus,
                        startDate = r.StartDate.ToShortDateString(),
                        EndDate = r.EndDate.HasValue ? r.EndDate.Value.ToShortDateString() : null,
                        User = new 
                        {
                            r.User.UserId,
                            r.User.UserEmail
                        },
                        r.ISBN,
                        PhysicalBook = r.PhysicalBook != null ? new
                        {
                            r.PhysicalBookId,
                            r.PhysicalBook.PhysicalBookStatus,
                        }: null,
                        Library = new
                        {
                            r.LibraryId,
                            r.Library.LibraryAlias
                        }
                    })
                    .ToListAsync();

                if (!requests.Any())
                {
                    return NoContent();
                }

                return Ok(requests);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET api/request/5
        /// <summary>Get request by id</summary>
        /// <remarks>Gives you a request by id</remarks>
        /// <param name="requestId">Request id</param>
        /// <response code="200">Returns the request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Request not found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("{requestId}")]
        [ProducesResponseType(typeof(Request), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian, Reader")]
        public async Task<IActionResult> GetRequestById(int requestId)
        {
            try
            {
                var request = await _context.Requests
                    .Where(r => r.RequestId == requestId)
                    .Include(r => r.User)
                    .Include(r => r.PhysicalBook)
                    .Include(r => r.Notifications)
                    .Include(r => r.Punishment)
                    .Include(r => r.Library)
                    .Select(r => new
                    {
                        r.RequestId,
                        r.RequestStatus,
                        startDate = r.StartDate.ToShortDateString(),
                        EndDate = r.EndDate.HasValue ? r.EndDate.Value.ToShortDateString() : null,
                        User = new
                        {
                            r.User.UserId,
                            r.User.UserName
                        },
                        r.ISBN,
                        PhysicalBook = new
                        {
                            r.PhysicalBookId,
                            r.PhysicalBook.PhysicalBookStatus,
                        },
                        Punishment = r.Punishment != null ? new
                        {
                            r.Punishment.PunishmentId,
                            r.Punishment.PunishmentReason,
                            r.Punishment.PunishmentLevel,
                            r.Punishment.EmittedDate
                        } : null,
                        Notifications = r.Notifications.Select(n => new
                        {
                            n.NotificationId,
                            n.NotificationTitle,
                            n.NotificationDescription,
                        }).ToList(),
                        Library = new
                        {
                            r.LibraryId,
                            r.Library.LibraryAlias
                        }
                    })
                    .SingleOrDefaultAsync();

                if (request == null)
                {
                    return NotFound();
                }

                return Ok(request);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET api/request/all/waiting/9781231231233/1
        /// <summary>Get all requests by ISBN for waiting list by library id</summary>
        /// <remarks>Gives you a list of all requests by ISBN for waiting list by library id</remarks>
        /// <param name="isbn">Book ISBN</param>
        /// <param name="libraryId">Library id</param>
        /// <response code="200">Returns the list of requests</response>
        /// <response code="204">No requests were found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("all/waiting/{isbn}/{libraryId}")]
        [ProducesResponseType(typeof(List<Request>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian, Reader")]
        public async Task<IActionResult> GetAllRequestsByISBNForWaitingListByLibraryId(string isbn, int libraryId)
        {
            try
            {
                var requests = await _context.Requests
                    .Where(r => r.ISBN == isbn && r.RequestStatus == RequestStatus.Waiting && r.LibraryId == libraryId)
                    .Include(r => r.User)
                    .Select(r => new
                    {
                        startDate = r.StartDate.ToShortDateString(),
                        User = new
                        {
                            r.User.UserId,
                            r.User.UserName
                        },
                        r.LibraryId
                    })
                    .ToListAsync();

                if (!requests.Any())
                {
                    return NoContent();
                }

                return Ok(requests);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET api/request/all/user/1
        /// <summary>Get all requests by user id</summary>
        /// <remarks>Gives you a list of all requests by user id</remarks>
        /// <param name="userId">User id</param>
        /// <response code="200">Returns the list of requests</response>
        /// <response code="204">No requests were found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("all/user/{userId}")]
        [ProducesResponseType(typeof(List<Request>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian, Reader")]
        public async Task<IActionResult> GetAllRequestsByUserId(int userId)
        {
            try
            {
                var requestsInUser = await _context.Requests
                    .Where(r => r.UserId == userId)
                    .Include(r => r.User)
                    .Include(r => r.PhysicalBook)
                    .Include(r => r.Library)
                    .Select(r => new
                    {
                        r.RequestId,
                        r.RequestStatus,
                        startDate = r.StartDate.ToShortDateString(),
                        EndDate = r.EndDate.HasValue ? r.EndDate.Value.ToShortDateString() : null,
                        User = new
                        {
                            r.User.UserId,
                            r.User.UserEmail
                        },
                        r.ISBN,
                        PhysicalBook = r.PhysicalBook != null ? new
                        {
                            r.PhysicalBookId,
                            r.PhysicalBook.PhysicalBookStatus,
                        } : null,
                        Library = new
                        {
                            r.LibraryId,
                            r.Library.LibraryAlias
                        }
                    })
                    .ToListAsync();

                if (!requestsInUser.Any())
                {
                    return NoContent();
                }

                return Ok(requestsInUser);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET api/request/all/library/1
        /// <summary>Get all requests by library id</summary>
        /// <remarks>Gives you a list of all requests by library id</remarks>
        /// <param name="libraryId">Library id</param>
        /// <response code="200">Returns the list of requests</response>
        /// <response code="204">No requests were found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("all/library/{libraryId}")]
        [ProducesResponseType(typeof(List<Request>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> GetAllRequestsByLibraryId(int libraryId)
        {
            try
            {
                var requestsInLibrary = await _context.Requests
                    .Where(r => r.LibraryId == libraryId)
                    .Include(r => r.User)
                    .Include(r => r.PhysicalBook)
                    .Include(r => r.Library)
                    .Select(r => new
                    {
                        r.RequestId,
                        r.RequestStatus,
                        startDate = r.StartDate.ToShortDateString(),
                        EndDate = r.EndDate.HasValue ? r.EndDate.Value.ToShortDateString() : null,
                        User = new
                        {
                            r.User.UserId,
                            r.User.UserEmail
                        },
                        r.ISBN,
                        PhysicalBook = r.PhysicalBook != null ? new
                        {
                            r.PhysicalBookId,
                            r.PhysicalBook.PhysicalBookStatus,
                        } : null,
                        Library = new
                        {
                            r.LibraryId,
                            r.Library.LibraryAlias
                        }
                    })
                    .ToListAsync();

                if (!requestsInLibrary.Any())
                {
                    return NoContent();
                }

                return Ok(requestsInLibrary);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // POST api/request/add/requested
        /// <summary>Create request with status Requested</summary>
        /// <remarks>Create request with status Requested</remarks>
        /// <response code="201">Returns the created request</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost("add/requested")]
        [ProducesResponseType(typeof(Request), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> CreateRequestWithStatusRequested(RequestDTO input)
        {
            try
            {
                var physicalBook = await _context.PhysicalBooks.FirstOrDefaultAsync(p => p.PhysicalBookId == input.PhysicalBookId && p.PhysicalBookStatus == PhysicalBookStatus.AtLibrary && p.LibraryId == input.LibraryId);

                if (physicalBook == null)
                {
                    return BadRequest();
                }

                if(await CheckIfAlreadyHasActiveRequestForBook(input.ISBN, input.UserId))
                {
                    return BadRequest();
                }

                if(await CheckIfHasMoreThanThreeActiveRequests(input.UserId))
                {
                    return BadRequest();
                }

                physicalBook.UpdatePhysicalBookStatus(PhysicalBookStatus.Requested);

                var request = Entities.Request.Create(input, RequestStatus.Requested);
                _context.Requests.Add(request);

                await _context.SaveChangesAsync();

                var notification = Notification.CreateNotificationForRequest(request.RequestId, request.ISBN);
                _context.Notifications.Add(notification);

                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetRequestById), new { request.RequestId }, request);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // POST api/request/add/pending
        /// <summary>Create request with status Pending</summary>
        /// <remarks>Create request with status Pending</remarks>
        /// <response code="201">Returns the created request</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost("add/pending")]
        [ProducesResponseType(typeof(Request), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian, Reader")]
        public async Task<IActionResult> CreateRequestWithStatusPending(RequestDTO input)
        {
            try
            {
                var physicalBook = await _context.PhysicalBooks.FirstOrDefaultAsync(p => p.ISBN == input.ISBN && p.PhysicalBookStatus == PhysicalBookStatus.AtLibrary && p.LibraryId == input.LibraryId);

                if (physicalBook == null)
                {
                    return BadRequest();
                }

                if (await CheckIfAlreadyHasActiveRequestForBook(input.ISBN, input.UserId))
                {
                    return BadRequest();
                }

                if (await CheckIfHasMoreThanThreeActiveRequests(input.UserId))
                {
                    return BadRequest();
                }

                input.PhysicalBookId = physicalBook.PhysicalBookId;
                physicalBook.UpdatePhysicalBookStatus(PhysicalBookStatus.Requested);

                var request = Entities.Request.Create(input, RequestStatus.Pending);

                _context.Requests.Add(request);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetRequestById), new { request.RequestId }, request);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // POST api/request/add/waiting
        /// <summary>Create request with status Waiting</summary>
        /// <remarks>Create request with status Waiting</remarks>
        /// <response code="201">Returns the created request</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <rresponse code="500">Internal Server Error</rresponse>
        [HttpPost("add/waiting")]
        [ProducesResponseType(typeof(Request), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian, Reader")]
        public async Task<IActionResult> CreateRequestWithStatusWaiting(RequestDTO input)
        {
            try
            {
                var physicalBook = await _context.PhysicalBooks.AnyAsync(p => p.ISBN == input.ISBN && p.PhysicalBookStatus == PhysicalBookStatus.AtLibrary && p.LibraryId == input.LibraryId);

                if (physicalBook)
                {
                    return BadRequest();
                }

                if (await CheckIfAlreadyHasActiveRequestForBook(input.ISBN, input.UserId))
                {
                    return BadRequest();
                }

                if (await CheckIfHasMoreThanThreeActiveRequests(input.UserId))
                {
                    return BadRequest();
                }

                var request = Entities.Request.Create(input, RequestStatus.Waiting);
                _context.Requests.Add(request);

                await _context.SaveChangesAsync();

                var notification = Notification.CreateNotificationForEnteringWaitingList(input.UserId, request.RequestId, input.LibraryId, input.ISBN);
                _context.Notifications.Add(notification);

                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetRequestById), new { request.RequestId }, request);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // PUT api/request/edit/change-to-canceled/5
        /// <summary>Change request status to Canceled</summary>
        /// <remarks>Change request status to Canceled</remarks>
        /// <param name="requestId">Request id</param>
        /// <response code="201">Returns the updated request</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Request not found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("edit/change-to-canceled/{requestId}")]
        [ProducesResponseType(typeof(Request), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian, Reader")]
        public async Task<IActionResult> ChangeRequestStatusToCanceled(int requestId)
        {
            try
            {
                var request = await _context.Requests.SingleOrDefaultAsync(r => r.RequestId == requestId);

                if (request == null)
                {
                    return NotFound();
                }

                if (request.RequestStatus != RequestStatus.Pending && request.RequestStatus != RequestStatus.Waiting)
                {
                    return BadRequest();
                }

                // In case the request has a physicalBook attached change status to AtLibrary
                if (request.PhysicalBookId != null)
                {
                    var physicalBook = await _context.PhysicalBooks.SingleOrDefaultAsync(p => p.PhysicalBookId == request.PhysicalBookId);

                    if (physicalBook == null)
                    {
                        return BadRequest();
                    }

                    physicalBook.UpdatePhysicalBookStatus(PhysicalBookStatus.AtLibrary);
                }

                request.RequestStatus = RequestStatus.Canceled;
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetRequestById), new { request.RequestId }, request);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // PUT api/request/edit/change-to-requested/5
        /// <summary>Change request status to Requested</summary>
        /// <remarks>Change request status to Requested</remarks>
        /// <param name="requestId">Request id</param>
        /// <param name="endDate">Request end date</param>
        /// <response code="201">Returns the updated request</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Request not found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("edit/change-to-requested/{requestId}")]
        [ProducesResponseType(typeof(Request), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> ChangeRequestStatusToRequested(int requestId, [FromBody] DateTime endDate)
        {
            try
            {
                var request = await _context.Requests.SingleOrDefaultAsync(r => r.RequestId == requestId);

                if (request == null)
                {
                    return NotFound();
                }

                if(request.RequestStatus != RequestStatus.Pending && request.RequestStatus != RequestStatus.Waiting)
                {
                    return BadRequest();
                }

                // Add physicalBook to request in case it comes from status Waiting
                if (request.PhysicalBookId == null && request.RequestStatus == RequestStatus.Waiting)
                {
                    var physicalBook = await _context.PhysicalBooks.FirstOrDefaultAsync(p => p.ISBN == request.ISBN && p.PhysicalBookStatus == PhysicalBookStatus.AtLibrary && p.LibraryId == request.LibraryId);

                    if (physicalBook == null)
                    {
                        return BadRequest();
                    }

                    request.PhysicalBookId = physicalBook.PhysicalBookId;
                    physicalBook.UpdatePhysicalBookStatus(PhysicalBookStatus.Requested);
                }

                request.EndDate = endDate;
                request.RequestStatus = RequestStatus.Requested;

                var notification = Notification.CreateNotificationForRequest(request.RequestId, request.ISBN);
                _context.Notifications.Add(notification);

                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetRequestById), new { request.RequestId }, request);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // PUT api/request/edit/change-to-pending/5
        /// <summary>Change request status to Pending</summary>
        /// <remarks>Change request status to Pending</remarks>
        /// <param name="requestId">Request id</param>
        /// <param name="endDate">Request end date</param>
        /// <response code="201">Returns the updated request</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Request not found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("edit/change-to-pending/{requestId}")]
        [ProducesResponseType(typeof(Request), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian, Reader")]
        public async Task<IActionResult> ChangeRequestStatusToPending(int requestId, [FromBody] DateTime endDate)
        {
            try
            {
                var request = await _context.Requests.SingleOrDefaultAsync(r => r.RequestId == requestId);

                if (request == null)
                {
                    return NotFound();
                }

                if (request.RequestStatus != RequestStatus.Waiting)
                {
                    return BadRequest();
                }

                var physicalBook = await _context.PhysicalBooks.FirstOrDefaultAsync(p => p.ISBN == request.ISBN && p.PhysicalBookStatus == PhysicalBookStatus.AtLibrary && p.LibraryId == request.LibraryId);

                if (physicalBook == null)
                {
                    return BadRequest();
                }

                request.PhysicalBookId = physicalBook.PhysicalBookId;
                physicalBook.UpdatePhysicalBookStatus(PhysicalBookStatus.Requested);
                request.EndDate = endDate;
                request.RequestStatus = RequestStatus.Pending;

                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetRequestById), new { request.RequestId }, request);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // PUT api/request/edit/change-to-returned/5
        /// <summary>Change request status to Returned</summary>
        /// <remarks>Change request status to Returned</remarks>
        /// <param name="requestId">Request id</param>
        /// <response code="201">Returns the updated request</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Request not found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("edit/change-to-returned/{requestId}")]
        [ProducesResponseType(typeof(Request), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> ChangeRequestStatusToReturned(int requestId)
        {
            try
            {
                var request = await _context.Requests.SingleOrDefaultAsync(r => r.RequestId == requestId);

                if (request == null)
                {
                    return NotFound();
                }

                if (request.RequestStatus != RequestStatus.Requested && request.RequestStatus != RequestStatus.NotReturned)
                {
                    return BadRequest();
                }

                // Change physicalBook availability to true
                var physicalBook = await _context.PhysicalBooks.SingleOrDefaultAsync(p => p.PhysicalBookId == request.PhysicalBookId);

                if (physicalBook == null)
                {
                    return BadRequest();
                }

                physicalBook.UpdatePhysicalBookStatus(PhysicalBookStatus.AtLibrary);
                request.RequestStatus = RequestStatus.Returned;
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetRequestById), new { request.RequestId }, request);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // PUT api/request/edit/change-to-not-returned/5
        /// <summary>Change request status to NotReturned</summary>
        /// <remarks>Change request status to NotReturned</remarks>
        /// <param name="requestId">Request id</param>
        /// <response code="201">Returns the updated request</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Request not found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("edit/change-to-not-returned/{requestId}")]
        [ProducesResponseType(typeof(Request), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> ChangeRequestStatusToNotReturned(int requestId)
        {
            try
            {
                var request = await _context.Requests.SingleOrDefaultAsync(r => r.RequestId == requestId);

                if (request == null)
                {
                    return NotFound();
                }

                if (request.RequestStatus != RequestStatus.Requested)
                {
                    return BadRequest();
                }

                request.RequestStatus = RequestStatus.NotReturned;
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetRequestById), new { request.RequestId }, request);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // PUT api/request/edit/extend-time/5
        /// <summary>Extend request time</summary>
        /// <remarks>Extend request time</remarks>
        /// <param name="requestId">Request id</param>
        /// <param name="endDate">Request end date</param>
        /// <response code="201">Returns the updated request</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Request not found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("edit/extend-time/{requestId}")]
        [ProducesResponseType(typeof(Request), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> ExtendTime(int requestId, [FromBody] DateTime endDate)
        {
            try
            {
                var request = await _context.Requests.SingleOrDefaultAsync(r => r.RequestId == requestId);

                if (request == null)
                {
                    return NotFound();
                }

                if (request.RequestStatus != RequestStatus.Requested)
                {
                    return BadRequest();
                }

                request.EndDate = endDate;
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetRequestById), new { request.RequestId }, request);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // DELETE api/request/5
        /// <summary>Delete request</summary>
        /// <remarks>Delete request</remarks>
        /// <param name="requestId">Request id</param>
        /// <response code="204">Request deleted</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Request not found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpDelete("delete/{requestId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> DeleteRequest(int requestId)
        {
            try
            {
                var request = _context.Requests.SingleOrDefault(r => r.RequestId == requestId);

                if (request == null)
                {
                    return NotFound();
                }

                if (request.RequestStatus != RequestStatus.Returned && request.RequestStatus != RequestStatus.Canceled)
                {
                    return BadRequest();
                }

                if (request.Punishment != null)
                {
                    _context.Punishments.Remove(request.Punishment);
                }

                if (request.Notifications != null && request.Notifications.Any())
                {
                    _context.Notifications.RemoveRange(request.Notifications);
                }

                _context.Requests.Remove(request);
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
