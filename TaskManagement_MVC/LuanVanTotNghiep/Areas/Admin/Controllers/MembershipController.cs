using LuanVanTotNghiep.Models;
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
        [HttpGet]
        public async Task<ActionResult> MemDetail(int id)
        {
            var plan = await MembershipService.GetMembershipById(id);
            return View(plan);
        }

        // GET: Tạo mới
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tạo mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(MembershipItem model)
        {
            if (ModelState.IsValid)
            {
                var result = await MembershipService.CreateMembership(model);
                if (result) return RedirectToAction("Index");
                ModelState.AddModelError("", "Thêm gói membership thất bại.");
            }
            return View(model);
        }

        // GET: Cập nhật
        [HttpGet]
        public async Task<ActionResult> Update(int id)
        {
            var plan = await MembershipService.GetMembershipById(id);
            return View(plan);
        }

        // POST: Cập nhật
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(MembershipItem model)
        {
            if (ModelState.IsValid)
            {
                var result = await MembershipService.UpdateMembership(model.PlanId, model);
                if (result) return RedirectToAction("Index");
                ModelState.AddModelError("", "Cập nhật thất bại.");
            }
            return View(model);
        }

        // GET: Xác nhận xóa
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var plan = await MembershipService.GetMembershipById(id);
            return View(plan);
        }

        // POST: Thực hiện xóa
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var result = await MembershipService.DeleteMembership(id);
            if (result) return RedirectToAction("Index");

            ModelState.AddModelError("", "Xóa thất bại.");
            var plan = await MembershipService.GetMembershipById(id);
            return View("Delete", plan);
        }
    }
}
