using LuanVanTotNghiep.Repository;
using Microsoft.AspNetCore.Mvc;

namespace LuanVanTotNghiep.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FileController : Controller
    {
        private readonly TaskFile_Repo TaskFileService = new TaskFile_Repo();
        public async Task<ActionResult> Index()
        {
            return View(await TaskFileService.GetAllFile());
        }
    }
}
