using GroupLibraryManagment.API.Entities;
using GroupLibraryManagment.API.Persistence;
using GroupLibraryManagment.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace GroupLibraryManagment.API.Controllers
{
    [Route("api/physical-book")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class PhysicalBookController : ControllerBase
    {
        private readonly GroupLibraryManagmentDbContext _context;
        public PhysicalBookController(GroupLibraryManagmentDbContext context)
        {
            _context = context;
        }

        // GET: api/physical-book/all
        /// <summary>Gets all physical books.</summary>
        /// <remarks>Gives you a list of all physical books.</remarks>
        /// <response code="200">Returns the list of physical books.</response>
        /// <response code="204">No physical books were found.</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("all")]
        [ProducesResponseType(typeof(List<PhysicalBook>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> GetAllPhysicalBooks()
        {
            try
            {
                var physicalBooks = await _context.PhysicalBooks
                .Include(p => p.Library)
                .Include(p => p.GenericBook)
                .Select(p => new
                {
                    p.PhysicalBookId,
                    p.PhysicalBookStatus,
                    Library = new
                    {
                        p.LibraryId,
                        p.Library.LibraryAlias
                    },
                    GenericBook = new
                    {
                        p.ISBN,
                        p.GenericBook.Title
                    }
                })
                .ToListAsync();

                if (!physicalBooks.Any())
                {
                    return NoContent();
                }

                return Ok(physicalBooks);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET: api/physical-book/9
        /// <summary>Gets a physical book by id.</summary>
        /// <remarks>Gives you a physical book by id.</remarks>
        /// <param name="physicalBookId">The id of the physical book.</param>
        /// <response code="200">Returns the physical book.</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Physical book not found.</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{physicalBookId}")]
        [ProducesResponseType(typeof(PhysicalBook), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> GetPhysicalBookById(int physicalBookId)
        {
            try
            {
                var physicalBook = await _context.PhysicalBooks.Where(p => p.PhysicalBookId == physicalBookId)
                    .Include(p => p.Requests)
                    .Include(p => p.Transfers)
                    .Include(p => p.Library)
                    .Include(p => p.GenericBook)
                    .Select(p => new
                    {
                        p.PhysicalBookId,
                        p.PhysicalBookStatus,
                        Library = new
                        {
                            p.LibraryId,
                            p.Library.LibraryAlias
                        },
                        GenericBook = new
                        {
                            p.ISBN,
                            p.GenericBook.Title
                        },
                        Requests = p.Requests.Select(r => new
                        {
                            r.RequestId,
                            r.RequestStatus,
                        }).ToList(),
                        Transfers = p.Transfers.Select(t => new
                        {
                            t.TransferId,
                            t.TransferStatus,
                        }).ToList(),
                    })
                    .SingleOrDefaultAsync();

                if (physicalBook == null)
                {
                    return NotFound();
                }

                return Ok(physicalBook);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET: api/physical-book/all/library/1
        /// <summary>Gets all physical books by library id.</summary>
        /// <remarks>Gives you a list of all physical books by library id.</remarks>
        /// <param name="libraryId">The id of the library.</param>
        /// <response code="200">Returns the list of physical books.</response>
        /// <response code="204">No physical books were found.</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("all/library/{libraryId}")]
        [ProducesResponseType(typeof(List<PhysicalBook>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> GetAllPhysicalBooksByLibraryId(int libraryId)
        {
            try
            {
                var physicalBooks = await _context.PhysicalBooks
                    .Where(p => p.LibraryId == libraryId)
                    .Include(p => p.GenericBook)
                    .Select(p => new
                    {
                        p.PhysicalBookId,
                        p.PhysicalBookStatus,
                        GenericBook = new
                        {
                            p.ISBN,
                            p.GenericBook.Title
                        }
                    })
                    .ToListAsync();

                if (!physicalBooks.Any())
                {
                    return NoContent();
                }

                return Ok(physicalBooks);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET: api/physical-book/all/generic-book/1
        /// <summary>Gets all physical books by generic book id.</summary>
        /// <remarks>Gives you a list of all physical books by generic book id.</remarks>
        /// <param name="isbn">The isbn of the generic book.</param>
        /// <response code="200">Returns the list of physical books.</response>
        /// <response code="204">No physical books were found.</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("all/generic-book/{isbn}")]
        [ProducesResponseType(typeof(List<PhysicalBook>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> GetAllPhysicalBooksByISBNAtLibrary(string isbn)
        {
            try
            {
                var physicalBooks = await _context.PhysicalBooks
                    .Where(p => p.ISBN == isbn && p.PhysicalBookStatus == PhysicalBookStatus.AtLibrary)
                    .Include(p => p.GenericBook)
                    .Include(p => p.Library)
                    .Select(p => new
                    {
                        p.PhysicalBookId,
                        p.PhysicalBookStatus,
                        GenericBook = new
                        {
                            p.ISBN,
                            p.GenericBook.Title
                        },
                        Library = new
                        {
                            p.LibraryId,
                            p.Library.LibraryAlias
                        }
                    })
                    .ToListAsync();

                if (!physicalBooks.Any())
                {
                    return NoContent();
                }

                return Ok(physicalBooks);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET: api/physical-book/all/generic-book/11421412412/1
        /// <summary>Gets all physical books by generic book id and library id.</summary>
        /// <remarks>Gives you a list of all physical books by generic book id and library id.</remarks>
        /// <param name="isbn">The isbn of the generic book.</param>
        /// <param name="libraryId">The id of the library.</param>
        /// <response code="200">Returns the list of physical books.</response>
        /// <response code="204">No physical books were found.</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("all/generic-book/{isbn}/{libraryId}")]
        [ProducesResponseType(typeof(List<PhysicalBook>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> GetAllPhysicalBooksByISBNByLibraryId(string isbn, int libraryId)
        {
            try
            {
                var physicalBooks = await _context.PhysicalBooks
                    .Where(p => p.ISBN == isbn && p.LibraryId == libraryId)
                    .Include(p => p.GenericBook)
                    .Include(p => p.Library)
                    .Select(p => new
                    {
                        p.PhysicalBookId,
                        p.PhysicalBookStatus,
                        GenericBook = new
                        {
                            p.ISBN,
                            p.GenericBook.Title
                        },
                        Library = new
                        {
                            p.LibraryId,
                            p.Library.LibraryAlias
                        }
                    })
                    .ToListAsync();

                if (!physicalBooks.Any())
                {
                    return NoContent();
                }

                return Ok(physicalBooks);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET: api/physical-book/all/generic-book/9781231231233/library/1
        /// <summary>Gets all physical books by generic book id and library id with status atLibrary.</summary>
        /// <remarks>Gives you a list of all physical books by generic book id and library id with status atLibrary.</remarks>
        /// <param name="isbn">The isbn of the generic book.</param>
        /// <param name="libraryId">The id of the library.</param>
        /// <response code="200">Returns the list of physical books.</response>
        /// <response code="204">No physical books were found.</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("all/generic-book/{isbn}/library/{libraryId}")]
        [ProducesResponseType(typeof(List<PhysicalBook>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> GetAllPhysicalBooksByISBNCurrentlyAtLibrary(string isbn, int libraryId)
        {
            try
            {
                var physicalBooks = await _context.PhysicalBooks
                    .Where(p => p.ISBN == isbn && p.PhysicalBookStatus == PhysicalBookStatus.AtLibrary && p.LibraryId == libraryId)
                    .Include(p => p.GenericBook)
                    .Include(p => p.Library)
                    .Select(p => new
                    {
                        p.PhysicalBookId,
                        p.PhysicalBookStatus,
                        GenericBook = new
                        {
                            p.ISBN,
                            p.GenericBook.Title
                        },
                        Library = new
                        {
                            p.LibraryId,
                            p.Library.LibraryAlias
                        }
                    })
                    .ToListAsync();

                if (!physicalBooks.Any())
                {
                    return NoContent();
                }

                return Ok(physicalBooks);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET: api/physical-book/all/transfer/library/1
        /// <summary>Gets all physical book by library id with status transfer.</summary>
        /// <remarks>Gives you a list of all physical books by library id with status transfer.</remarks>
        /// <param name="libraryId">The id of the library.</param>
        /// <response code="200">Returns the list of physical books.</response>
        /// <response code="204">No physical books were found.</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("all/transfer/library/{libraryId}")]
        [ProducesResponseType(typeof(List<PhysicalBook>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> GetAllPhysicalBooksWithTransferStatusForLibrary(int libraryId)
        {
            try
            {
                var physicalBooks = await _context.Transfers
                    .Where(t => t.TransferStatus == TransferStatus.Accepted && t.DestinationLibraryId == libraryId)
                    .Include(t => t.PhysicalBook)
                    .Include(t => t.PhysicalBook.GenericBook)
                    .Include(t => t.PhysicalBook.Library)
                    .Select(t => new
                    {
                        t.PhysicalBook.PhysicalBookId,
                        t.PhysicalBook.PhysicalBookStatus,
                        t.PhysicalBook.LibraryId,
                        GenericBook = new
                        {
                            t.PhysicalBook.ISBN,
                            t.PhysicalBook.GenericBook.Title
                        },
                        Library = new
                        {
                            t.PhysicalBook.LibraryId,
                            t.PhysicalBook.Library.LibraryAlias
                        }
                    })
                    .Where(p => p.PhysicalBookStatus == PhysicalBookStatus.Transfer)
                    .ToListAsync();

                if (!physicalBooks.Any())
                {
                    return NoContent();
                }

                return Ok(physicalBooks);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // POST: api/physical-book/add/5
        /// <summary> Creates a physical book.</summary>
        /// <remarks>Creates a physical book.</remarks>
        /// <response code="201">Returns the newly created physical book.</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("add")]
        [ProducesResponseType(typeof(PhysicalBook), 201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> CreatePhysicalBook(PhysicalBookDTO input)
        {
            try
            {
                var physicalBook = PhysicalBook.Create(input);
                _context.PhysicalBooks.Add(physicalBook);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetPhysicalBookById), new { physicalBook.PhysicalBookId }, physicalBook);

            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // POST: api/physical-book/add/5
        /// <summary> Creates various physical book.</summary>
        /// <remarks>Creates various physical book.</remarks>
        /// <param name="numOfBooks">The number of physical books to create.</param>
        /// <response code="201">Returns the newly created physical book.</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("add/{numOfBooks}")]
        [ProducesResponseType(typeof(PhysicalBook), 201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> CreatePhysicalBooks(int numOfBooks, PhysicalBookDTO input)
        {
            try
            {
                var physicalBooks = new List<PhysicalBook>();

                for (int i = 0; i < numOfBooks; i++)
                {
                    var physicalBook = PhysicalBook.Create(input);
                    physicalBooks.Add(physicalBook);
                }

                _context.PhysicalBooks.AddRange(physicalBooks);
                await _context.SaveChangesAsync();

                // For simplicity, assuming the first physical book in the list represents the result
                var resultPhysicalBook = physicalBooks.FirstOrDefault();

                return CreatedAtAction(nameof(GetPhysicalBookById), new { resultPhysicalBook.PhysicalBookId }, resultPhysicalBook);

            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // PUT: api/physical-book/edit/9
        /// <summary>Updates a physical book.</summary>
        /// <remarks>Updates a physical book.</remarks>
        /// <param name="physicalBookId">The id of the physical book.</param>
        /// <response code="201">Returns the newly created physical book.</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">If the physical book is not found.</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("edit/{id}")]
        [ProducesResponseType(typeof(PhysicalBook), 201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> UpdatePhysicalBook(int physicalBookId, PhysicalBookDTO input)
        {
            try
            {
                var physicalBook = await _context.PhysicalBooks.SingleOrDefaultAsync(p => p.PhysicalBookId == physicalBookId);

                if (physicalBook == null)
                {
                    return NotFound();
                }

                physicalBook.Update(input);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetPhysicalBookById), new { physicalBook.PhysicalBookId }, physicalBook);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // PUT: api/physical-book/edit/status/9/1
        /// <summary>Updates a physical book status.</summary>
        /// <remarks>Updates a physical book status.</remarks>
        /// <param name="physicalBookId">The id of the physical book.</param>
        /// <param name="libraryId">The id of the library.</param>
        /// <response code="201">Returns the newly created physical book.</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">If the physical book is not found.</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("edit/status/{physicalBookId}/{libraryId}")]
        [ProducesResponseType(typeof(PhysicalBook), 201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> UpdatePhysicalBookStatus(int physicalBookId, int libraryId)
        {
            try
            {
                var physicalBook = await _context.PhysicalBooks.SingleOrDefaultAsync(p => p.PhysicalBookId == physicalBookId);

                if (physicalBook == null)
                {
                    return NotFound();
                }

                physicalBook.LibraryId = libraryId;
                physicalBook.PhysicalBookStatus = PhysicalBookStatus.AtLibrary;
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetPhysicalBookById), new { physicalBook.PhysicalBookId }, physicalBook);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // DELETE: api/generic-book/delete/9781234121234
        /// <summary>Deletes a physical book.</summary>
        /// <remarks>Deletes a physical book.</remarks>
        /// <param name="physicalBookId">The id of the physical book.</param>
        /// <response code="204">Physical book deleted.</response>
        /// <response code="400">If the physical book is not at library.</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">If the physical book is not found.</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("delete/{physicalBookId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> DeletePhysicalBook(int physicalBookId)
        {
            try
            {
                var physicalBook = await _context.PhysicalBooks.SingleOrDefaultAsync(p => p.PhysicalBookId == physicalBookId);

                if (physicalBook == null)
                {
                    return NotFound();
                }

                if (physicalBook.PhysicalBookStatus != PhysicalBookStatus.AtLibrary)
                {
                    return BadRequest();
                }

                _context.PhysicalBooks.Remove(physicalBook);
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
