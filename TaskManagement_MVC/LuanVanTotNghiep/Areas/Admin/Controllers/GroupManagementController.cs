using LuanVanTotNghiep.Repository;
using Microsoft.AspNetCore.Mvc;

namespace LuanVanTotNghiep.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GroupManagementController : Controller
    {
        private readonly Group_Repo GroupService = new Group_Repo();
        public async Task<ActionResult> Index()
        {

            return View(await GroupService.GetAllGroups());
        }
    }
}
