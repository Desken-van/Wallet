using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallet.Application.Models;
using Wallet.Data;
using Wallet.Data.Repository;
using Wallet.Models.Requests;
using Wallet.Settinngs;

namespace Wallet.Controllers.AdminPanel.CRUD
{
    public class UserCRUDController : Controller
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;

        public UserCRUDController(IUserRepository repo, IMapper mapper)
        {
            _repo = repo;

            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("user/crud/list")]
        public async Task<ActionResult<IEnumerable<User>>> GetUserList()
        {
            try
            {
                var list = await _repo.GetUserListAsync();

                if ((list != null) && list.Any())
                {
                    return Json(Ok(list));
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("user/crud/{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            try
            {
                var user = await _repo.GetUserByIdAsync(id);

                if ((user != null))
                {
                    return Json(Ok(user));
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("user/crud/getbyname/{name}")]
        public async Task<ActionResult<User>> GetUserByName(string name)
        {
            try
            {
                var user = await _repo.GetUserByNameAsync(name);

                if ((user != null))
                {
                    return Json(Ok(user));
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("user/crud/add")]
        public async Task<ActionResult> CreateUser(UserRequest request)
        {
            try
            {
                var result = _mapper.Map<User>(request);

                result.HashCode = JWTModule.HashPassword(request.Password);
                result.CreatedDate = DateTime.UtcNow;

                var check = await _repo.Create(result);

                return Json(check);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("user/crud/update")]
        public async Task<ActionResult> UpdateUser(UserUpdateRequest request)
        {
            try
            {
                var result = _mapper.Map<User>(request);

                result.HashCode = JWTModule.HashPassword(request.Password);

                await _repo.UpdateAsync(result);

                return Json(Ok());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("user/crud/{id}")]
        public async Task<ActionResult> DeleteUserByID(int id)
        {
            try
            {
                await _repo.DeleteAsync(id);

                return Json(Ok());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
