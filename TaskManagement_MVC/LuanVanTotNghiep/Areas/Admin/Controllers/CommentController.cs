using LuanVanTotNghiep.Models;
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
            return View(await CommentService.GetAllComments());
        }
        [HttpGet]
        public async Task<ActionResult> CommentDetail(int id)
        {
            return View(CommentService.GetCommentById(id));
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Thêm mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CommentItemInput model)
        {
            if (ModelState.IsValid)
            {
                var result = await CommentService.CreateComment(model);
                if (result) return RedirectToAction("Index");
                ModelState.AddModelError("", "Thêm bình luận thất bại.");
            }
            return View(model);
        }

        // GET: Cập nhật
        [HttpGet]
        public async Task<ActionResult> Update(int id)
        {
            var comment = await CommentService.GetCommentById(id);
            return View(comment);
        }

        // POST: Cập nhật
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(CommentItem model)
        {
            if (ModelState.IsValid)
            {
                var result = await CommentService.UpdateComment(model.CommentId, model);
                if (result) return RedirectToAction("Index");
                ModelState.AddModelError("", "Cập nhật bình luận thất bại.");
            }
            return View(model);
        }

        // GET: Xác nhận xóa
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var comment = await CommentService.GetCommentById(id);
            return View(comment);
        }

        // POST: Thực hiện xóa
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var result = await CommentService.DeleteComment(id);
            if (result) return RedirectToAction("Index");
            ModelState.AddModelError("", "Xóa bình luận thất bại.");
            var comment = await CommentService.GetCommentById(id);
            return View("Delete", comment);
        }
    }
}
