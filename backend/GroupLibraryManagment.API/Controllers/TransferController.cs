using GroupLibraryManagment.API.Entities;
using GroupLibraryManagment.API.Persistence;
using GroupLibraryManagment.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace GroupLibraryManagment.API.Controllers
{
    [Route("api/transfer")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class TransferController : ControllerBase
    {
        private readonly GroupLibraryManagmentDbContext _context;
        public TransferController(GroupLibraryManagmentDbContext context)
        {
            _context = context;
        }

        // GET: api/transfer/all
        /// <summary>Get all transfers</summary>
        /// <remarks>Gives you a list of all transfers</remarks>
        /// <response code="200">Returns the list of transfers</response>
        /// <response code="204">No transfers were found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("all")]
        [ProducesResponseType(typeof(List<Transfer>),200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> GetAllTransfers()
        {
            try
            {
                var transfers = await _context.Transfers
                    .Include(t => t.PhysicalBook)
                    .Include(t => t.SourceLibrary)
                    .Include(t => t.DestinationLibrary)
                    .Select(t => new
                    {
                        t.TransferId,
                        startDate = t.StartDate.ToShortDateString(),
                        endDate = t.EndDate.ToShortDateString(),
                        t.TransferStatus,
                        SourceLibrary = new
                        {
                            t.SourceLibraryId,
                            t.SourceLibrary.LibraryAlias
                        },
                        DestinationLibrary = new
                        {
                            t.DestinationLibraryId,
                            t.DestinationLibrary.LibraryAlias
                        },
                        PhysicalBook = new
                        {
                            t.PhysicalBookId,
                            t.PhysicalBook.PhysicalBookStatus,
                            t.PhysicalBook.Library.LibraryAlias
                        }
                    })
                    .ToListAsync();

                if (!transfers.Any())
                {
                    return NoContent();
                }

                return Ok(transfers);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET api/transfer/5
        /// <summary>Get transfer by id</summary>
        /// <remarks>Gives you a transfer by id</remarks>
        /// <param name="transferId">Transfer id</param>
        /// <response code="200">Returns the transfer</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Transfer not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{transferId}")]
        [ProducesResponseType(typeof(Transfer),200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> GetTransferByTransferId(int transferId)
        {
            try
            {
                var transfer = await _context.Transfers
                    .Where(t => t.TransferId == transferId)
                    .Include(t => t.PhysicalBook)
                    .Include(t => t.SourceLibrary)
                    .Include(t => t.DestinationLibrary)
                    .Select(t => new
                    {
                        t.TransferId,
                        startDate = t.StartDate.ToShortDateString(),
                        endDate = t.EndDate.ToShortDateString(),
                        t.TransferStatus,
                        SourceLibrary = new
                        {
                            t.SourceLibraryId,
                            t.SourceLibrary.LibraryAlias
                        },
                        DestinationLibrary = new
                        {
                            t.DestinationLibraryId,
                            t.DestinationLibrary.LibraryAlias
                        },
                        PhysicalBook = new
                        {
                            t.PhysicalBookId,
                            t.PhysicalBook.PhysicalBookStatus,
                            t.PhysicalBook.Library.LibraryAlias
                        }
                    })
                    .SingleOrDefaultAsync();

                if (transfer == null)
                {
                    return NotFound();
                }

                return Ok(transfer);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET api/transfer/all/library/1
        /// <summary>Get all transfers by library id</summary>
        /// <remarks>Gives you a list of all transfers by library id</remarks>
        /// <param name="libraryId">Library id</param>
        /// <response code="200">Returns the list of transfers</response>
        /// <response code="204">No transfers were found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("all/library/{libraryId}")]
        [ProducesResponseType(typeof(List<Transfer>),200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> GetAllTransfersByLibraryId(int libraryId)
        {
            try
            {
                var transfers = await _context.Transfers
                   .Where(t => t.SourceLibraryId == libraryId || t.DestinationLibraryId == libraryId)
                   .Include(t => t.PhysicalBook)
                   .Include(t => t.SourceLibrary)
                   .Include(t => t.DestinationLibrary)
                   .Select(t => new
                   {
                       t.TransferId,
                       startDate = t.StartDate.ToShortDateString(),
                       endDate = t.EndDate.ToShortDateString(),
                       t.TransferStatus,
                       SourceLibrary = new
                       {
                           t.SourceLibraryId,
                           t.SourceLibrary.LibraryAlias
                       },
                       DestinationLibrary = new
                       {
                           t.DestinationLibraryId,
                           t.DestinationLibrary.LibraryAlias
                       },
                       PhysicalBook = new
                       {
                           t.PhysicalBookId,
                           t.PhysicalBook.PhysicalBookStatus,
                           t.PhysicalBook.Library.LibraryAlias
                       }
                   })
                   .ToListAsync();

                if (!transfers.Any())
                {
                    return NoContent();
                }

                return Ok(transfers);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // POST api/transfer/add
        /// <summary>Create a transfer</summary>
        /// <remarks>Create a transfer</remarks>
        /// <response code="201">Returns the created transfer</response>
        /// <response code="400">Invalid transfer data</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Physical book not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("add")]
        [ProducesResponseType(typeof(Transfer),201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> CreateTransfer(TransferDTO input)
        {
            try
            {
                var physicalBook = await _context.PhysicalBooks.SingleOrDefaultAsync(p => p.PhysicalBookId == input.PhysicalBookId);

                if (physicalBook == null)
                {
                    return NotFound();
                }

                if (physicalBook.PhysicalBookStatus != PhysicalBookStatus.AtLibrary)
                {
                    return BadRequest();
                }

                if (physicalBook.LibraryId != input.SourceLibraryId)
                {
                    return BadRequest();
                }

                var transfer = Transfer.Create(input);
                _context.Transfers.Add(transfer);

                var library = await _context.Libraries.SingleOrDefaultAsync(l => l.LibraryId == input.SourceLibraryId);

                var notification = Notification.CreateNotificationForLibraryForTransfer(input.SourceLibraryId, library.LibraryAlias, input.EndDate);
                _context.Notifications.Add(notification);

                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetTransferByTransferId), new { transfer.TransferId }, transfer);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // PUT api/transfer/edit/change-to-accepted/id
        /// <summary>Change transfer status to accepted</summary>
        /// <remarks>Change transfer status to accepted</remarks>
        /// <param name="transferId">Transfer id</param>
        /// <response code="201">Returns the updated transfer</response>
        /// <response code="400">Invalid transfer data</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Transfer not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("edit/change-to-accepted/{transferId}")]
        [ProducesResponseType(typeof(Transfer),201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> ChangeTransferStatusToAccepted(int transferId)
        {
            try
            {
                var transfer = await _context.Transfers.SingleOrDefaultAsync(t => t.TransferId == transferId);
                
                if (transfer == null)
                {
                    return NotFound();
                }

                if(transfer.TransferStatus != TransferStatus.Pending)
                {
                    return BadRequest();
                }

                var physicalBook = await _context.PhysicalBooks.SingleOrDefaultAsync(p => p.PhysicalBookId == transfer.PhysicalBookId);

                if (physicalBook == null)
                {
                    return BadRequest();
                }

                physicalBook.PhysicalBookStatus = PhysicalBookStatus.Transfer;
                transfer.TransferStatus = TransferStatus.Accepted;

                var notification = Notification.CreateNotificationForBookArrival(transfer.DestinationLibraryId);
                _context.Notifications.Add(notification);

                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetTransferByTransferId), new { transfer.TransferId }, transfer);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // PUT api/transfer/edit/change-to-rejected/id
        /// <summary>Change transfer status to rejected</summary>
        /// <remarks>Change transfer status to rejected</remarks>
        /// <param name="transferId">Transfer id</param>
        /// <response code="201">Returns the updated transfer</response>
        /// <response code="400">Invalid transfer data</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Transfer not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("edit/change-to-rejected/{transferId}")]
        [ProducesResponseType(typeof(Transfer),201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> ChangeTransferStatusToRejected(int transferId)
        {
            try
            {
                var transfer = await _context.Transfers.SingleOrDefaultAsync(t => t.TransferId == transferId);

                if (transfer == null)
                {
                    return NotFound();
                }

                if (transfer.TransferStatus != TransferStatus.Pending)
                {
                    return BadRequest();
                }

                transfer.TransferStatus = TransferStatus.Rejected;
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetTransferByTransferId), new { transfer.TransferId }, transfer);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // PUT api/transfer/edit/change-to-canceled/id
        /// <summary>Change transfer status to canceled</summary>
        /// <remarks>Change transfer status to canceled</remarks>
        /// <param name="transferId">Transfer id</param>
        /// <response code="201">Returns the updated transfer</response>
        /// <response code="400">Invalid transfer data</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Transfer not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("edit/change-to-canceled/{transferId}")]
        [ProducesResponseType(typeof(Transfer),201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> ChangeTransferStatusToCanceled(int transferId)
        {
            try
            {
                var transfer = await _context.Transfers.SingleOrDefaultAsync(t => t.TransferId == transferId);

                if (transfer == null)
                {
                    return NotFound();
                }

                if (transfer.TransferStatus != TransferStatus.Pending)
                {
                    return BadRequest();
                }

                transfer.TransferStatus = TransferStatus.Canceled;
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetTransferByTransferId), new { transfer.TransferId }, transfer);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // DELETE api/transfer/delete/5
        /// <summary>Delete a transfer</summary>
        /// <remarks>Delete a transfer</remarks>
        /// <param name="transferId">Transfer id</param>
        /// <response code="204">Transfer deleted</response>
        /// <response code="400">Invalid transfer data</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Transfer not found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("delete/{transferId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> DeleteTransfer(int transferId)
        {
            try
            {
                var transfer = _context.Transfers.SingleOrDefault(d => d.TransferId == transferId);

                if (transfer == null)
                {
                    return NotFound();
                }

                if (transfer.TransferStatus == TransferStatus.Pending)
                {
                    return BadRequest();
                }

                _context.Transfers.Remove(transfer);
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
