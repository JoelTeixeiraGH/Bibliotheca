using GroupLibraryManagment.API.Entities;
using GroupLibraryManagment.API.Persistence;
using GroupLibraryManagment.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;

namespace GroupLibraryManagment.API.Controllers
{
    [Route("api/generic-book")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class GenericBookController : ControllerBase
    {
        private readonly GroupLibraryManagmentDbContext _context;
        public GenericBookController(GroupLibraryManagmentDbContext context)
        {
            _context = context;
        }
        private async Task<List<int>> GetAuthorsIds(string[] authorsName)
        {
            var authorIds = new List<int>();

            foreach (var authorName in authorsName)
            {
                var authorExists = await _context.Authors.FirstOrDefaultAsync(a => a.AuthorName == authorName);

                if (authorExists == null)
                {
                    var newAuthor = new Author { AuthorName = authorName };
                    _context.Authors.Add(newAuthor);
                    await _context.SaveChangesAsync();
                    authorIds.Add(newAuthor.AuthorId);
                }
                else
                {
                    authorIds.Add(authorExists.AuthorId);
                }
            }
            return authorIds;
        }
        private async Task<List<int>> GetCategoriesIds(string[] categoriesName)
        {
            var categoryIds = new List<int>();

            foreach (var categoryName in categoriesName)
            {
                var categoryExists = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryName == categoryName);

                if (categoryExists == null)
                {
                    var newCategory = new Category { CategoryName = categoryName };
                    _context.Categories.Add(newCategory);
                    await _context.SaveChangesAsync();
                    categoryIds.Add(newCategory.CategoryId);
                }
                else
                {
                    categoryIds.Add(categoryExists.CategoryId);
                }
            }
            return categoryIds;
        }
        private async Task<int> GetLanguageId(string languageAlias)
        {
            var languageId = 0;
            var language = await _context.Languages.FirstOrDefaultAsync(l => l.LanguageAlias == languageAlias);

            if (language == null)
            {
                var newLanguage = new Language { LanguageName = "", LanguageAlias = languageAlias };
                _context.Languages.Add(newLanguage);
                await _context.SaveChangesAsync();
                languageId = newLanguage.LanguageId;
            }
            else
            {
                languageId = language.LanguageId;
            }

            return languageId;
        }
        private async Task RemoveBookAuthors(string ISBN)
        {
            var bookAuthorsToRemove = await _context.BookAuthors.Where(ba => ba.ISBN == ISBN).ToListAsync();
            _context.BookAuthors.RemoveRange(bookAuthorsToRemove);
        }
        private async Task RemoveBookCategories(string ISBN)
        {
            var bookCategoriesToRemove = await _context.BookCategories.Where(bc => bc.ISBN == ISBN).ToListAsync();
            _context.BookCategories.RemoveRange(bookCategoriesToRemove);
        }
        private async Task RemoveAllEvaluations(string ISBN)
        {
            var evaluationsToRemove = await _context.Evaluations.Where(e => e.ISBN == ISBN).ToListAsync();
            _context.Evaluations.RemoveRange(evaluationsToRemove);
        }

        // GET: api/generic-book/all
        /// <summary>Get all generic books</summary>
        /// <remarks>Gives you a list of all generic books</remarks>
        /// <response code="200">Returns the list of generic books</response>
        /// <response code="204">No generic books were found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("all")]
        [ProducesResponseType(typeof(List<GenericBook>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian, Reader")]
        public async Task<IActionResult> GetAllGenericBooks()
        {
            try
            {
                var genericBooks = await _context.GenericBooks
                    .Include(g => g.Language)
                    .Include(g => g.PhysicalBooks)
                    .Include(g => g.BookAuthors).ThenInclude(a => a.Author)
                    .Include(g => g.BookCategories).ThenInclude(c => c.Category)
                    .Select(g => new
                    {
                        g.ISBN,
                        g.Title,
                        g.Description,
                        datePublished = g.DatePublished.ToShortDateString(),
                        g.PageNumber,
                        Language = new
                        {
                            g.Language.LanguageId,
                            g.Language.LanguageAlias
                        },
                        Authors = g.BookAuthors.Select(a => new
                        {
                            a.AuthorId,
                            a.Author.AuthorName
                        }).ToList(),
                        Categories = g.BookCategories.Select(c => new
                        {
                            c.CategoryId,
                            c.Category.CategoryName
                        }).ToList(),
                        PhysicalBooks = _context.PhysicalBooks
                            .Where(pb => pb.ISBN == g.ISBN && pb.PhysicalBookStatus == PhysicalBookStatus.AtLibrary)
                            .GroupBy(pb => new { pb.Library.LibraryId, pb.Library.LibraryAlias })
                            .Select(group => new
                            {
                                LibraryId = group.Key.LibraryId,
                                LibraryAlias = group.Key.LibraryAlias,
                                Count = group.Count()
                            })
                            .ToList(),
                        g.Thumbnail,
                        g.SmallThumbnail
                    })
                    .ToListAsync();

                if (!genericBooks.Any())
                {
                    return NoContent();
                }

                return Ok(genericBooks);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET: api/generic-book/all/most-popular
        /// <summary>Get all generic books by most popular</summary>
        /// <remarks>Gives you a list of all generic books by most popular</remarks>
        /// <response code="200">Returns the list of generic books by most popular</response>
        /// <response code="204">No generic books were found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("all/most-popular")]
        [ProducesResponseType(typeof(List<GenericBook>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian, Reader")]
        public async Task<IActionResult> GetAllGenericBooksByMostPopular()
        {
            try
            {
                var popularGenericBooks = await _context.GenericBooks
                    .Include(g => g.Language)
                    .Include(g => g.BookAuthors).ThenInclude(a => a.Author)
                    .Include(g => g.BookCategories).ThenInclude(c => c.Category)
                    .Select(g => new
                    {
                        g.ISBN,
                        g.Title,
                        g.Description,
                        datePublished = g.DatePublished.ToShortDateString(),
                        g.PageNumber,
                        Language = new
                        {
                            g.Language.LanguageId,
                            g.Language.LanguageAlias
                        },
                        Authors = g.BookAuthors.Select(a => new
                        {
                            a.AuthorId,
                            a.Author.AuthorName
                        }).ToList(),
                        Categories = g.BookCategories.Select(c => new
                        {
                            c.CategoryId,
                            c.Category.CategoryName
                        }).ToList(),
                        g.Thumbnail,
                        g.SmallThumbnail,
                        RequestCount = g.PhysicalBooks.SelectMany(pb => pb.Requests).Count()
                    })
                    .OrderByDescending(g => g.RequestCount)
                    .Take(20)
                    .ToListAsync();

                if (!popularGenericBooks.Any())
                {
                    return NoContent();
                }

                return Ok(popularGenericBooks);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET: api/generic-book/9781234121234
        /// <summary>Get a generic book by ISBN</summary>
        /// <remarks>Gives you a generic book by ISBN</remarks>
        /// <param name="ISBN">The ISBN of the generic book</param>
        /// <response code="200">Returns the generic book</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Generic book not found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("{ISBN}")]
        [ProducesResponseType(typeof(GenericBook), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian, Reader")]
        public async Task<IActionResult> GetGenericBookByISBN(string ISBN)
        {
            try
            {
                var genericBook = await _context.GenericBooks
                    .Where(g => g.ISBN == ISBN)
                    .Include(g => g.Evaluations)
                    .Include(g => g.Language)
                    .Include(g => g.PhysicalBooks)
                    .Include(g => g.Requests)
                    .Include(g => g.BookAuthors).ThenInclude(a => a.Author)
                    .Include(g => g.BookCategories).ThenInclude(c => c.Category)
                    .Select(g => new
                    {
                        g.ISBN,
                        g.Title,
                        g.Description,
                        datePublished = g.DatePublished.ToShortDateString(),
                        g.PageNumber,
                        Language = new
                        {
                            g.Language.LanguageId,
                            g.Language.LanguageName,
                            g.Language.LanguageAlias
                        },
                        Authors = g.BookAuthors.Select(a => new
                        {
                            a.AuthorId,
                            a.Author.AuthorName
                        }).ToList(),
                        Categories = g.BookCategories.Select(c => new
                        {
                            c.CategoryId,
                            c.Category.CategoryName
                        }).ToList(),
                        PhysicalBooks = _context.PhysicalBooks
                            .Where(pb => pb.ISBN == ISBN && pb.PhysicalBookStatus == PhysicalBookStatus.AtLibrary)
                            .GroupBy(pb => new { pb.Library.LibraryId, pb.Library.LibraryAlias })
                            .Select(group => new
                            {
                                LibraryId = group.Key.LibraryId,
                                LibraryAlias = group.Key.LibraryAlias,
                                Count = group.Count()
                            })
                            .ToList(),
                        Evaluations = g.Evaluations.Any() ? g.Evaluations.Select(e => new
                        {
                            e.EvaluationId,
                            e.EvaluationDescription,
                            e.EvaluationScore,
                            emittedDate = e.EmittedDate.ToShortDateString(),
                            e.UserId,
                            e.User.UserName
                        }).ToList() : null,
                        NumberOfEvaluations = g.Evaluations.Any() ? g.Evaluations.Count() : 0,
                        AverageEvaluationScore = g.Evaluations.Any() ? g.Evaluations.Average(e => (decimal)e.EvaluationScore) : 0,
                        g.Thumbnail,
                        g.SmallThumbnail
                    })
                    .SingleOrDefaultAsync();

                if (genericBook == null)
                {
                    return NotFound();
                }

                return Ok(genericBook);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, $"An error occurred while processing your request. {ex.Message}");
            }
        }

        // GET: api/generic-book/9781234121234/google
        /// <summary>Get a generic book by ISBN from Google Books API</summary>
        /// <remarks>Gives you a generic book by ISBN from Google Books API</remarks>
        /// <param name="ISBN">The ISBN of the generic book</param>
        /// <response code="200">Returns the generic book</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Generic book not found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("{ISBN}/google")]
        [ProducesResponseType(typeof(GenericBook), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> GetGenericBookByGoogleAPI(string ISBN)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    // Make an HTTP request to the Google Books API
                    var apiUrl = $"https://www.googleapis.com/books/v1/volumes?q=isbn:{ISBN}";
                    var response = await httpClient.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        // Deserialize the response content to obtain book information
                        var content = await response.Content.ReadAsAsync<JObject>();
                        var items = content?["items"] as JArray;
                        var googleBook = items?.FirstOrDefault()?["volumeInfo"] as JObject;

                        if (googleBook != null)
                        {
                            // Extract ISBN from industryIdentifiers
                            var industryIdentifiers = googleBook["industryIdentifiers"] as JArray;
                            var isbnIdentifier = industryIdentifiers?
                                .FirstOrDefault(identifier => identifier?["type"]?.ToString() == "ISBN_13")
                                ?["identifier"]?.ToString();

                            // Extract authors
                            var authorsJArray = googleBook["authors"] as JArray;
                            var authors = new List<string>();
                            if (authorsJArray != null)
                            {
                                authors.AddRange(authorsJArray.Select(a => a.ToString()));
                            }

                            // Extract mainCategory and categories
                            var mainCategory = googleBook["mainCategory"]?.ToString();
                            var additionalCategories = googleBook["categories"] as JArray;

                            // Combine mainCategory and additionalCategories into a single variable named "Categories"
                            var categories = new List<string>();
                            if (!string.IsNullOrEmpty(mainCategory))
                            {
                                categories.Add(mainCategory);
                            }
                            if (additionalCategories != null)
                            {
                                categories.AddRange(additionalCategories.Select(c => c.ToString()));
                            }

                            // Create a GenericBook-like object with the desired properties
                            var genericBook = new
                            {
                                ISBN = isbnIdentifier,
                                Title = googleBook["title"]?.ToString(),
                                Description = googleBook["description"]?.ToString(),
                                PageNumber = googleBook["pageCount"]?.ToObject<int?>(),
                                Language = googleBook["language"]?.ToString(),
                                DatePublished = googleBook["publishedDate"]?.ToObject<DateTime?>(),
                                Authors = authors,
                                Categories = categories,
                                Thumbnail = googleBook["imageLinks"]["thumbnail"]?.ToString(),
                                SmallThumbnail = googleBook["imageLinks"]["smallThumbnail"]?.ToString()
                            };

                            return Ok(genericBook);
                        }
                    }

                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET api/generic-book/category/5
        /// <summary>Get a generic book by category Id</summary>
        /// <remarks>Gives you a generic book by category Id</remarks>
        /// <param name="categoryId">The Id of the category</param>
        /// <response code="200">Returns the list of generic book</response>
        /// <response code="204">No generic book were found</response>  
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("all/category/{categoryId}")]
        [ProducesResponseType(typeof(List<GenericBook>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian, Reader")]
        public async Task<IActionResult> GetAllGenericBooksByCategoryId(int categoryId)
        {
            try
            {
                var booksInCategory = await _context.GenericBooks
                    .Where(g => g.BookCategories.Any(bc => bc.CategoryId == categoryId))
                    .Include(g => g.Language)
                    .Include(g => g.BookAuthors).ThenInclude(a => a.Author)
                    .Include(g => g.BookCategories).ThenInclude(c => c.Category)
                    .Select(g => new
                    {
                        g.ISBN,
                        g.Title,
                        g.Description,
                        datePublished = g.DatePublished.ToShortDateString(),
                        g.PageNumber,
                        Language = new
                        {
                            g.Language.LanguageId,
                            g.Language.LanguageName
                        },
                        Authors = g.BookAuthors.Select(a => new
                        {
                            a.AuthorId,
                            a.Author.AuthorName
                        }).ToList(),
                        Categories = g.BookCategories.Select(c => new
                        {
                            c.CategoryId,
                            c.Category.CategoryName
                        }).ToList(),
                        g.Thumbnail,
                        g.SmallThumbnail
                    })
                    .ToListAsync();

                if (!booksInCategory.Any())
                {
                    return NoContent();
                }

                return Ok(booksInCategory);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET api/generic-book/all/author/5
        /// <summary>Get a generic book by author Id</summary>
        /// <remarks>Gives you a generic book by author Id</remarks>
        /// <param name="authorId">The Id of the author</param>
        /// <response code="200">Returns the list of generic book</response>
        /// <response code="204">No generic book were found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("all/author/{authorId}")]
        [ProducesResponseType(typeof(List<GenericBook>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian, Reader")]
        public async Task<IActionResult> GetAllGenericBooksByAuthorId(int authorId)
        {
            try
            {
                var booksInAuthor = await _context.GenericBooks
                    .Where(g => g.BookAuthors.Any(bc => bc.AuthorId == authorId))
                    .Include(g => g.Language)
                    .Include(g => g.BookAuthors).ThenInclude(a => a.Author)
                    .Include(g => g.BookCategories).ThenInclude(c => c.Category)
                    .Select(g => new
                    {
                        g.ISBN,
                        g.Title,
                        g.Description,
                        datePublished = g.DatePublished.ToShortDateString(),
                        g.PageNumber,
                        Language = new
                        {
                            g.Language.LanguageId,
                            g.Language.LanguageName
                        },
                        Authors = g.BookAuthors.Select(a => new
                        {
                            a.AuthorId,
                            a.Author.AuthorName
                        }).ToList(),
                        Categories = g.BookCategories.Select(c => new
                        {
                            c.CategoryId,
                            c.Category.CategoryName
                        }).ToList(),
                        g.Thumbnail,
                        g.SmallThumbnail
                    })
                    .ToListAsync();

                if (!booksInAuthor.Any())
                {
                    return NoContent();
                }

                return Ok(booksInAuthor);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET api/generic-book/all/language/5
        /// <summary>Get a generic book by language Id</summary>
        /// <remarks>Gives you a generic book by language Id</remarks>
        /// <param name="languageId">The Id of the language</param>
        /// <response code="200">Returns the list of generic book</response>
        /// <response code="204">No generic book were found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("all/language/{languageId}")]
        [ProducesResponseType(typeof(List<GenericBook>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian, Reader")]
        public async Task<IActionResult> GetAllGenericBooksByLanguageId(int languageId)
        {
            try
            {
                var booksInLanguage = await _context.GenericBooks
                    .Where(g => g.LanguageId == languageId)
                    .Include(g => g.Language)
                    .Include(g => g.BookAuthors).ThenInclude(a => a.Author)
                    .Include(g => g.BookCategories).ThenInclude(c => c.Category)
                    .Select(g => new
                    {
                        g.ISBN,
                        g.Title,
                        g.Description,
                        datePublished = g.DatePublished.ToShortDateString(),
                        g.PageNumber,
                        Language = new
                        {
                            g.Language.LanguageId,
                            g.Language.LanguageName
                        },
                        Authors = g.BookAuthors.Select(a => new
                        {
                            a.AuthorId,
                            a.Author.AuthorName
                        }).ToList(),
                        Categories = g.BookCategories.Select(c => new
                        {
                            c.CategoryId,
                            c.Category.CategoryName
                        }).ToList(),
                        g.Thumbnail,
                        g.SmallThumbnail
                    })
                    .ToListAsync();

                if (!booksInLanguage.Any())
                {
                    return NoContent();
                }

                return Ok(booksInLanguage);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET: api/generic-book/search/searchterm
        /// <summary>Search generic books by title, author or category</summary>
        /// <remarks>Gives you a list of generic books by title, author or category</remarks>
        /// <param name="searchTerm">The search term</param>
        /// <response code="200">Returns the list of generic books</response>
        /// <response code="204">No generic books were found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("search/{searchTerm}")]
        [ProducesResponseType(typeof(List<GenericBook>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian, Reader")]
        public async Task<IActionResult> SearchGenericBooks(string searchTerm)
        {
            try
            {
                var genericBooks = await _context.GenericBooks
                    .Where(g => g.Title.Contains(searchTerm) || g.BookAuthors.Any(author => author.Author.AuthorName.Contains(searchTerm)) ||
                        g.BookCategories.Any(category => category.Category.CategoryName.Contains(searchTerm)))
                    .Include(g => g.Language)
                    .Include(g => g.BookAuthors).ThenInclude(a => a.Author)
                    .Include(g => g.BookCategories).ThenInclude(c => c.Category)
                    .Select(g => new
                    {
                        g.ISBN,
                        g.Title,
                        g.Description,
                        datePublished = g.DatePublished.ToShortDateString(),
                        g.PageNumber,
                        Language = new
                        {
                            g.Language.LanguageId,
                            g.Language.LanguageAlias
                        },
                        Authors = g.BookAuthors.Select(a => new
                        {
                            a.AuthorId,
                            a.Author.AuthorName
                        }).ToList(),
                        Categories = g.BookCategories.Select(c => new
                        {
                            c.CategoryId,
                            c.Category.CategoryName
                        }).ToList(),
                        g.Thumbnail,
                        g.SmallThumbnail
                    })
                    .ToListAsync();

                if (!genericBooks.Any())
                {
                    return NoContent();
                }

                return Ok(genericBooks);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // POST: api/generic-book/add
        /// <summary>Create new generic book</summary>
        /// <remarks>Create new generic book</remarks>
        /// <response code="201">Returns the newly created generic book</response>
        /// <response code="400">If the ISBN already exists</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost("add")]
        [ProducesResponseType(typeof(GenericBook), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> CreateGenericBook(GenericBookDTO input)
        {
            try
            {
                if (await _context.GenericBooks.AnyAsync(g => g.ISBN == input.ISBN))
                {
                    return BadRequest();
                }

                var authorIds = await GetAuthorsIds(input.AuthorsNames);
                var categoryIds = await GetCategoriesIds(input.CategoriesNames);
                var languageId = await GetLanguageId(input.LanguageAlias);

                var genericBook = GenericBook.Create(input, authorIds, categoryIds, languageId);

                _context.GenericBooks.Add(genericBook);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetGenericBookByISBN), new { genericBook.ISBN }, genericBook);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
            
        }

        // PUT: api/generic-book/edit
        /// <summary>Edit generic book</summary>
        /// <remarks>Edit generic book</remarks>
        /// <response code="201">Returns the newly edited generic book</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">If the generic book was not found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("edit")]
        [ProducesResponseType(typeof(GenericBook), 201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> UpdateGenericBook(GenericBookDTO input)
        {
            try
            {
                var genericBook = await _context.GenericBooks.SingleOrDefaultAsync(d => d.ISBN == input.ISBN);

                if (genericBook == null)
                {
                    return NotFound();
                }

                await RemoveBookAuthors(input.ISBN);
                await RemoveBookCategories(input.ISBN);
                var authorIds = await GetAuthorsIds(input.AuthorsNames);
                var categoryIds = await GetCategoriesIds(input.CategoriesNames);
                var languageId = await GetLanguageId(input.LanguageAlias);
                genericBook.Update(input);
                genericBook.AddAuthors(authorIds);
                genericBook.AddCategories(categoryIds);
                genericBook.LanguageId = languageId;
                
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetGenericBookByISBN), new { genericBook.ISBN }, genericBook);
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
        /// <summary>Delete generic book</summary>
        /// <remarks>Delete generic book</remarks>
        /// <param name="ISBN">The ISBN of the generic book</param>
        /// <response code="204">Generic book was deleted</response>
        /// <response code="400">If the generic book is used by physical books</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">If the generic book was not found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpDelete("delete/{ISBN}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> DeleteGenericBook(string ISBN)
        {
            try
            {
                var genericBook = await _context.GenericBooks.SingleOrDefaultAsync(d => d.ISBN == ISBN);

                if (genericBook == null)
                {
                    return NotFound();
                }

                if (await _context.PhysicalBooks.Where(e => e.ISBN == ISBN).AnyAsync())
                {
                    return BadRequest();
                }

                await RemoveBookAuthors(genericBook.ISBN);
                await RemoveBookCategories(genericBook.ISBN);
                await RemoveAllEvaluations(genericBook.ISBN);

                _context.GenericBooks.Remove(genericBook);
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
