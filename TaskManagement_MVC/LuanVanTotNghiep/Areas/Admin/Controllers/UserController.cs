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
    }
}
