using GroupLibraryManagment.API.Entities;
using GroupLibraryManagment.API.Persistence;
using GroupLibraryManagment.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;

namespace GroupLibraryManagment.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly GroupLibraryManagmentDbContext _context;
        public UserController(GroupLibraryManagmentDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("Id", user.UserId.ToString()),
                new Claim("Name", user.UserName),
                new Claim("Email", user.UserEmail),
                new Claim("LibraryId", user.LibraryId.ToString()),
                new Claim(ClaimTypes.Role, user.UserRole.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
        private RefreshToken GenerateRefreshToken(User user)
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
            }; 

            return refreshToken;
        }
        private async Task SetRefreshToken(User user, RefreshToken newRefreshToken)
        {

            var cookieOptions = new CookieOptions
            {
                HttpOnly = false,
                Expires = newRefreshToken.Expires,
                SameSite = SameSiteMode.None,
                Secure= true
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;

            await _context.SaveChangesAsync();
        }

        // GET: api/user/all
        /// <summary>Get all users</summary>
        /// <remarks>Gives you a list of all users</remarks>
        /// <response code="200">Returns the list of users</response>
        /// <response code="204">No users found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("all")]
        [ProducesResponseType(typeof(List<User>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _context.Users
                    .Include(u => u.Library)
                    .Include(u => u.Requests).ThenInclude(r => r.Punishment)
                    .Select(u => new
                    {
                        u.UserId,
                        u.UserName,
                        u.UserEmail,
                        Library = new
                        {
                            u.LibraryId,
                            u.Library.LibraryAlias
                        },
                        NumberOfRequests = u.Requests.Count,
                        NumberOfPunishments = u.Requests.Count(r => r.Punishment != null)
                    })
                    .ToListAsync();

                if (!users.Any())
                {
                    return NoContent();
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET: api/user/all/library/1
        /// <summary>Get all users by library id</summary>
        /// <remarks>Gives you a list of all users by library id</remarks>
        /// <param name="libraryId">Library id</param>
        /// <response code="200">Returns the list of users</response>
        /// <response code="204">No users found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("all/library/{libraryId}")]
        [ProducesResponseType(typeof(List<User>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> GetAllUsersByLibraryId(int libraryId)
        {
            try
            {
                var users = await _context.Users
                    .Where(u => u.LibraryId == libraryId && u.UserRole == UserRole.Reader)
                    .Include(u => u.Library)
                    .Include(u => u.Requests).ThenInclude(r => r.Punishment)
                    .Select(u => new
                    {
                        u.UserId,
                        u.UserName,
                        u.UserEmail,
                        Library = new
                        {
                            u.LibraryId,
                            u.Library.LibraryAlias
                        },
                        NumberOfRequests = u.Requests.Count,
                        NumberOfPunishments = u.Requests.Count(r => r.Punishment != null)
                    })
                    .ToListAsync();

                if (!users.Any())
                {
                    return NoContent();
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET api/user/5
        /// <summary>Get user by id</summary>
        /// <remarks>Gives you a user by id</remarks>
        /// <param name="userId">User id</param>
        /// <response code="200">Returns the user</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">User not found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian, Reader")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            try
            {
                var user = await _context.Users
                    .Where(u => u.UserId == userId)
                    .Include(g => g.Library)
                    .Include(g => g.Notifications)
                    .Include(g => g.Evaluations)
                    .Include(g => g.Requests)
                    .Select(g => new
                    {
                        g.UserId,
                        g.UserName,
                        g.UserEmail,
                        Library = new
                        {
                            g.LibraryId,
                            g.Library.LibraryAlias
                        },
                        Notifications = g.Notifications.Select(n => new
                        {
                            n.NotificationId,
                            n.NotificationDescription
                        }).ToList(),
                        Evaluations = g.Evaluations.Select(e => new
                        {
                            e.EvaluationId,
                            e.EvaluationDescription,
                            e.EvaluationScore
                        }).ToList(),
                        NumberOfRequests = g.Requests.Count,
                        Requests = g.Requests.Select(e => new
                        {
                            e.RequestId,
                            e.RequestStatus,
                            Punishment = e.Punishment != null ? new
                            {
                                e.Punishment.PunishmentId,
                                e.Punishment.PunishmentReason
                            } : null
                        }).ToList(),
                        NumberOfPunishments = g.Requests.Count(r => r.Punishment != null)
                    })
                    .SingleOrDefaultAsync();

                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // POST api/user/register
        /// <summary>Register a new user</summary>
        /// <remarks>Register a new user</remarks>
        /// <response code="201">Returns the user</response>
        /// <response code="400">Email already exists</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(User), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Register(UserRegisterDTO input)
        {
            try
            {
                if(await _context.Users.AnyAsync(u => u.UserEmail == input.UserEmail))
                {
                    return BadRequest();
                }

                var user = Entities.User.Create(input);

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetUserById), new { userId = user.UserId }, user);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // POST api/user/login
        /// <summary>Login</summary>
        /// <remarks>Login</remarks>
        /// <response code="200">Returns the token</response>
        /// <response code="400">Wrong password</response>
        /// <response code="404">User not found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Login(UserLoginDTO input)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(d => d.UserEmail == input.UserEmail);

                if (user == null)
                {
                    return NotFound();
                }

                if (!BCrypt.Net.BCrypt.Verify(input.UserPassword, user.UserPassword))
                {
                    return BadRequest();
                }

                string token = CreateToken(user);

                var refreshToken = GenerateRefreshToken(user);
                await SetRefreshToken(user, refreshToken);

                return Ok(token);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // POST api/user/refresh-token
        /// <summary>Refresh token</summary>
        /// <remarks>Refresh token</remarks>
        /// <response code="200">Returns the token</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                var refreshtoken = Request.Cookies["refreshToken"];
                var user = await _context.Users.SingleOrDefaultAsync(u => u.RefreshToken.Equals(refreshtoken));

                if (user == null)
                {
                    return BadRequest();
                }

                if (user.TokenExpires < DateTime.Now)
                {
                    return Unauthorized("Token Expired.");
                }

                string token = CreateToken(user);
                var newRefreshToken = GenerateRefreshToken(user);

                await SetRefreshToken(user, newRefreshToken);

                return Ok(token);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // POST api/user/logout
        /// <summary>Logout</summary>
        /// <remarks>Logout</remarks>
        /// <response code="200">Returns ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost("logout")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian, Reader")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var refreshtoken = Request.Cookies["refreshToken"];

                var user = await _context.Users.SingleOrDefaultAsync(u => u.RefreshToken.Equals(refreshtoken));

                if (user == null)
                {
                    return BadRequest();
                }

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = false,
                    Expires = user.TokenExpires.AddYears(-1),
                    SameSite = SameSiteMode.None,
                    Secure = true
                };
                Response.Cookies.Append("refreshToken", user.RefreshToken, cookieOptions);

                user.TokenExpires = user.TokenExpires.AddYears(-1);

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // PUT api/user/edit/5
        /// <summary>Update user</summary>
        /// <remarks>Update user</remarks>
        /// <param name="userId">User id</param>
        /// <response code="201">Returns the user</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">User not found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("edit/{userId}")]
        [ProducesResponseType(typeof(User), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian, Reader")]
        public async Task<IActionResult> UpdateUser(int userId, UserUpdateDTO userUpdateModel)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(d => d.UserId == userId);

                if (user == null)
                {
                    return NotFound();
                }

                if (await _context.Users.AnyAsync(u => u.UserEmail == userUpdateModel.UserEmail && u.UserId != userId))
                {
                    return BadRequest();
                }

                user.Update(userUpdateModel);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetUserById), new { userId = user.UserId }, user);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
            
        }

        // PUT api/user/5/password
        /// <summary>Update user password</summary>
        /// <remarks>Update user password</remarks>
        /// <param name="userId">User id</param>
        /// <response code="201">Returns the user</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">User not found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("edit/{userId}/password")]
        [ProducesResponseType(typeof(User), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian, Reader")]
        public async Task<IActionResult> UpdatePassword(int userId, UserUpdatePasswordDTO updatePasswordModel)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(d => d.UserId == userId);

                if (user == null)
                {
                    return NotFound();
                }

                if (!BCrypt.Net.BCrypt.Verify(updatePasswordModel.OldPassword, user.UserPassword))
                {
                    return BadRequest();
                }

                user.UpdatePassword(updatePasswordModel.NewPassword);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetUserById), new { userId = user.UserId }, user);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // PUT api/user/5/role
        /// <summary>Update user role to librarian</summary>
        /// <remarks>Update user role to librarian</remarks>
        /// <param name="userId">User id</param>
        /// <response code="201">Returns the user</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">User not found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("edit/{userId}/role")]
        [ProducesResponseType(typeof(User), 201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeRoleToLibrarian(int userId)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(d => d.UserId == userId);

                if (user == null)
                {
                    return NotFound();
                }

                user.ChangeRoleToLibrarian();
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetUserById), new { userId = user.UserId }, user);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // DELETE api/user/delete/5
        /// <summary>Delete user</summary>
        /// <remarks>Delete user</remarks>
        /// <param name="userId">User id</param>
        /// <response code="204">Returns no content</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">User not found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpDelete("delete/{userId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Librarian, Reader")]
        public async Task<IActionResult> DeleteUser(int userId, [FromBody] string password)
        {
            try
            {
                var user = _context.Users.SingleOrDefault(d => d.UserId == userId);

                if (user == null)
                {
                    return NotFound();
                }

                if (!BCrypt.Net.BCrypt.Verify(password, user.UserPassword))
                {
                    return BadRequest();
                }

                if (await _context.Requests.AnyAsync(r => r.UserId == userId && (r.RequestStatus != RequestStatus.Returned && r.RequestStatus != RequestStatus.Canceled)))
                {
                    return BadRequest();
                }

                if (user.Evaluations != null && user.Evaluations.Any())
                {
                    _context.Evaluations.RemoveRange(user.Evaluations);
                }

                if (user.Notifications != null && user.Notifications.Any())
                {
                    _context.Notifications.RemoveRange(user.Notifications);
                }

                if (user.Requests != null && user.Requests.Any())
                {
                    _context.Requests.RemoveRange(user.Requests);
                }

                _context.Users.Remove(user);
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
