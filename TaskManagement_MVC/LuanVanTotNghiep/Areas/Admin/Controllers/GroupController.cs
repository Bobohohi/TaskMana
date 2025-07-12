using LuanVanTotNghiep.Models;
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
        [HttpGet]
        public async Task<ActionResult> GroupDetail(int id)
        {
            var group = await GroupService.GetGroupById(id);
            return View(group);
        }

        // GET: Thêm nhóm
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Thêm nhóm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(GroupItem newGroup)
        {
            if (ModelState.IsValid)
            {
                var result = await GroupService.CreateGroup(newGroup);
                if (result) return RedirectToAction("Index");
                ModelState.AddModelError("", "Thêm nhóm thất bại.");
            }
            return View(newGroup);
        }

        // GET: Cập nhật nhóm
        [HttpGet]
        public async Task<ActionResult> Update(int id)
        {
            var group = await GroupService.GetGroupById(id);
            return View(group);
        }

        // POST: Cập nhật nhóm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(GroupItem model)
        {
            if (ModelState.IsValid)
            {
                var result = await GroupService.UpdateGroup(model.GroupId, model);
                if (result) return RedirectToAction("Index");
                ModelState.AddModelError("", "Cập nhật nhóm thất bại.");
            }
            return View(model);
        }

        // GET: Xác nhận xóa nhóm
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var group = await GroupService.GetGroupById(id);
            return View(group);
        }

        // POST: Xóa nhóm
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var result = await GroupService.DeleteGroup(id);

            if (result)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Không thể xóa nhóm. Nhóm có thể đang được sử dụng hoặc đã xảy ra lỗi trong quá trình xóa.");
                var group = await GroupService.GetGroupById(id);
                return View("Delete", group);
            }
        }

    }
}
