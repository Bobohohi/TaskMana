using LuanVanTotNghiep.Repository;
using Microsoft.AspNetCore.Mvc;

namespace LuanVanTotNghiep.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProjectController : Controller
    {
        private readonly Project_Repo ProjectService = new Project_Repo();
        public async Task<ActionResult> Index()
        {
            return View(await ProjectService.GetAllProject());
        }
    }
}
