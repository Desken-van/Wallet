using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallet.Application.Models;
using Wallet.Data;
using Wallet.Models.Requests;

namespace Wallet.Controllers.AdminPanel.CRUD
{
    public class UserInfoCRUDController : Controller
    {
        private readonly IUserInfoRepository _repo;
        private readonly IMapper _mapper;

        public UserInfoCRUDController(IUserInfoRepository repo, IMapper mapper)
        {
            _repo = repo;

            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("user/info/crud/list")]
        public async Task<ActionResult<IEnumerable<UserInfo>>> GetUserInfoList()
        {
            try
            {
                var list = await _repo.GetUserInfoListAsync();

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
        [HttpGet("user/info/crud/{id}")]
        public async Task<ActionResult<UserInfo>> GetUserInfoById(int id)
        {
            try
            {
                var info = await _repo.GetUserInfoByIdAsync(id);

                if ((info != null))
                {
                    return Json(Ok(info));
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("user/info/crud/getbyuserid/{userId}")]
        public async Task<ActionResult<UserInfo>> GetUserInfoByUserId(int userId)
        {
            try
            {
                var info = await _repo.GetUserInfoByUserIdAsync(userId);

                if ((info != null))
                {
                    return Json(Ok(info));
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("user/info/crud/add")]
        public async Task<ActionResult> CreateUserInfo(UserInfoRequest request)
        {
            try
            {
                var result = _mapper.Map<UserInfo>(request);

                var check = await _repo.Create(result);

                return Json(check);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("user/info/crud/update")]
        public async Task<ActionResult> UpdateUserInfo(UserInfoUpdateRequest request)
        {
            try
            {
                var result = _mapper.Map<UserInfo>(request);

                await _repo.UpdateAsync(result);

                return Json(Ok());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("user/info/crud/{id}")]
        public async Task<ActionResult> DeleteUserInfoByID(int id)
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
