using LuanVanTotNghiep.Repository;
using Microsoft.AspNetCore.Mvc;

namespace LuanVanTotNghiep.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CommentController : Controller
    {
        private readonly Comment_Repo CommentService = new Comment_Repo();
        public async Task<ActionResult> Index()
        {
            return View();
        }
    }
}
