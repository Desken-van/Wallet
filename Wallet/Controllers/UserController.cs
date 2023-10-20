using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Wallet.Application.Models;
using Wallet.Data;
using Wallet.Models.Requests;
using Wallet.Settinngs;

namespace Wallet.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private IConfiguration Configuration;
        private AuthorizationOption authoption;
        private IMapper _mapper;

        public UserController(IConfiguration configuration, IUserRepository userRepository, IMapper mapper)
        {
            Configuration = configuration;
            _userRepository = userRepository;
            authoption = Configuration.GetSection("Option").Get<AuthorizationOption>();
            _mapper = mapper;
        }

        [HttpPost("/user/token")]
        public async Task<IActionResult> Token(UserAuthRequest request)
        {
            try
            {
                var user = await _userRepository.GetUserByNameAsync(request.Name);

                if (user == null)
                {
                    return NotFound("Current User not found");
                }

                var confirm = JWTModule.VerifyHashedPassword(user.HashCode, request.Password);

                if (confirm == false)
                {
                    return BadRequest("Password not matches");
                }
                else
                {
                    var identity = GetIdentity(user);

                    var response = new
                    {
                        access_token = JWTModule.CreateJWT(authoption, identity),
                        username = identity.Name,
                    };

                    return Json(response);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("/user/register")]
        public async Task<IActionResult> Register(UserAuthRequest request)
        {
            if ((request.Password.Length > 12 && request.Password.Length < 4) || request.Name == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }
            else
            {
                try
                {
                    var user = _mapper.Map<User>(request);

                    user.HashCode = JWTModule.HashPassword(request.Password);
                    user.CreatedDate = DateTime.UtcNow;
                    //user.Role = "User";
                    user.Role = "Admin";

                    var check = await _userRepository.Create(user);

                    return Json(check);
                }
                catch(Exception ex) 
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        private ClaimsIdentity GetIdentity(User user)
        {
            var login = user;
            if (login != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, login.Name),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, login.Role)
                };

                var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                                                        ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            return null;
        }
    }
}
