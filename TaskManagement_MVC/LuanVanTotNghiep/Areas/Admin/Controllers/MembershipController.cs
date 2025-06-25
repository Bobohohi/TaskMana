using LuanVanTotNghiep.Repository;
using Microsoft.AspNetCore.Mvc;

namespace LuanVanTotNghiep.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MembershipController : Controller
    {
        private readonly Membership_Repo MembershipService = new Membership_Repo();
        public async Task<ActionResult> Index()
        {
            return View(await MembershipService.GetAllMembership());
        }
    }
}
