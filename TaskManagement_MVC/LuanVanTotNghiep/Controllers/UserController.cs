using System.Text.RegularExpressions;
using LuanVanTotNghiep.Models;
using LuanVanTotNghiep.Repository;
using Microsoft.AspNetCore.Mvc;

namespace LuanVanTotNghiep.Controllers
{
    public class UserController : Controller
    {
        private readonly User_Repo UserService = new User_Repo();
        public async Task<ActionResult> Index()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Sign_in", "Index");
            }
            return View(await UserService.GetUserById(userId));
        }
        [HttpPost]
        public async Task<IActionResult> UploadAvatar(IFormFile PictureFile)
        {
            var imageUrl = await UserService.UploadAvatar(PictureFile);
            var userIdString = HttpContext.Session.GetString("UserId");
            int userId = int.Parse(userIdString);
            await UserService.UpdatePictureUrlByUserIdAndPictureUrl(userId, imageUrl);
            if (string.IsNullOrEmpty(imageUrl))
                return BadRequest("Upload failed.");


            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateUserInfor(UserItemUpdate model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Dữ liệu không hợp lệ.");
            var userIdString = HttpContext.Session.GetString("UserId");
            int userId = int.Parse(userIdString);
            model.UserId = userId;
            bool result = await UserService.UpdateUser(model);

            return RedirectToAction("Index");
        }

    }
}
