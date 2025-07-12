using LuanVanTotNghiep.Models;
using LuanVanTotNghiep.Repository;
using Microsoft.AspNetCore.Mvc;

namespace LuanVanTotNghiep.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly User_Repo UserService = new User_Repo();
        public async Task<ActionResult> Index()
        {
            return View(await UserService.GetAllUser());
        }
        [HttpGet]
        public async Task<ActionResult> UserDetail(int id)
        {
            var user = await UserService.GetUserById(id);
            return View(user);
        }

        // GET: Tạo người dùng
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tạo người dùng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserItem model)
        {

            return View(model);
        }

        // GET: Sửa thông tin người dùng
        [HttpGet]
        public async Task<ActionResult> Update(int id)
        {
            var user = await UserService.GetUserById(id);
            return View(user);
        }

        // POST: Sửa thông tin người dùng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(UserItem model)
        {

            return View(model);
        }

        // GET: Xác nhận xóa
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var user = await UserService.GetUserById(id);
            return View(user);
        }

        // POST: Xác nhận xóa
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {

            return View("Delete");
        }

    }
}
