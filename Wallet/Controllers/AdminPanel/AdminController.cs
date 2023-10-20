using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallet.Application.Models;
using Wallet.Data;

namespace Wallet.Controllers.AdminPanel
{
    public class AdminController : Controller
    {
        private readonly IUserRepository _userRepository;
        public AdminController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("/admin/makerole/admin/username/{username}")]
        public async Task<IActionResult> MakeAdminByName(string username)
        {
            try
            {
                var user = await _userRepository.GetUserByNameAsync(username);

                var result = await ChangeRoleToAdmin(user);

                return result;
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("/admin/makerole/admin/{id}")]
        public async Task<IActionResult> MakeAdminById(int id)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);

                var result = await ChangeRoleToAdmin(user);

                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("/admin/makerole/user/username/{username}")]
        public async Task<IActionResult> MakeUserByName(string username)
        {
            try
            {
                var user = await _userRepository.GetUserByNameAsync(username);

                var result = await ChangeRoleToUser(user);

                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("/admin/makerole/user/{id}")]
        public async Task<IActionResult> MakeUserById(int id)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);

                var result = await ChangeRoleToUser(user);

                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task<IActionResult> ChangeRoleToAdmin(User user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            else
            {
                if (user.Role == "User")
                {
                    user.Role = "Admin";

                    await _userRepository.UpdateAsync(user);

                    return Json(Ok());
                }
                else
                {
                    return Json(Ok("Already Admin"));
                }
            }
        }

        private async Task<IActionResult> ChangeRoleToUser(User user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            else
            {
                if (user.Role == "Admin")
                {
                    user.Role = "User";

                    await _userRepository.UpdateAsync(user);

                    return Json(Ok());
                }
                else
                {
                    return Json(Ok("Already User"));
                }
            }
        }
    }
}
