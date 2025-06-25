using LuanVanTotNghiep.Models;
using LuanVanTotNghiep.Repository;
using Microsoft.AspNetCore.Mvc;

namespace LuanVanTotNghiep.Controllers
{
    public class GroupDetailController : Controller
    {
        private readonly GroupDetail_Repo GroupDetailService = new GroupDetail_Repo();
        private readonly Group_Repo GroupService = new Group_Repo();
        public async Task<ActionResult> Index(int id)
        {
            return View(await GroupDetailService.GetAllUserByGroupId(id));
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(GroupItem model)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                ModelState.AddModelError("", "Không xác định được người dùng.");
                return View(model);
            }
            model.UserId = int.Parse(userIdString);
            if (ModelState.IsValid)
            {
                bool result = await GroupService.CreateGroup(model);
                if (result)
                    return RedirectToAction("Index");
                else
                    ModelState.AddModelError("", "Tạo Nhóm thất bại.");
            }

            return View(model);
        }
    }
}
