using LuanVanTotNghiep.Repository;
using Microsoft.AspNetCore.Mvc;

namespace LuanVanTotNghiep.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TaskManagementController : Controller
    {
        private readonly Task_Repo TaskService = new Task_Repo();
        public async Task<ActionResult> Index()
        {
            return View(await TaskService.GetAllTasks());
        }
    }
}
