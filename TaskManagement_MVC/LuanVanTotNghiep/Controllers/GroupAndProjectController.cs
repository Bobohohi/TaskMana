using System.Text.RegularExpressions;
using LuanVanTotNghiep.Models;
using LuanVanTotNghiep.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace LuanVanTotNghiep.Controllers
{
    public class GroupAndProjectController : Controller
    {
        private readonly TaskFile_Repo TaskFileService = new TaskFile_Repo();
        private readonly Comment_Repo CommentService = new Comment_Repo();
        private readonly Group_Repo GroupService = new Group_Repo();
        private readonly Project_Repo ProjectService = new Project_Repo();
        private readonly Board_Repo BoardService = new Board_Repo();
        private readonly Task_Repo TaskService = new Task_Repo();
        private readonly User_Repo UserService = new User_Repo();
        private readonly GroupDetail_Repo GroupDetailService = new GroupDetail_Repo();
        private readonly Report_Repo ReportService = new Report_Repo();
        private readonly UserSub_Repo UserSubService = new UserSub_Repo();
        private readonly TaskDetail_Repo TaskDetailService = new TaskDetail_Repo();
        private readonly Notification_Repo NoticeService = new Notification_Repo();
        public async Task<ActionResult> Index()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Sign_in", "Index");
            }
            
            return View(await GroupDetailService.GetAllGroupByUserId(userId));
        }
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            int userId = int.Parse(userIdString);
            bool isValid = await UserSubService.CheckValidSubscription(userId);

            if (!isValid)
            {
                return RedirectToAction("Index", "Membership");
            }
            else 
            {
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> PaymentSuccess(int planId)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                TempData["Message"] = "Không xác định được người dùng.";
                return RedirectToAction("Index", "GroupAndProject");
            }

            int userId = int.Parse(userIdString);

            // (Tùy chọn) bạn có thể xác minh trạng thái thanh toán từ PayOS tại đây
            var newSub = new UserSubItem
            {
                UserId = userId,
                PlanId = planId,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1),
                IsActive = true
            };

            await UserSubService.CreateUserSub(newSub);
            TempData["Message"] = "Đăng ký gói thành công!";
            return RedirectToAction("Index", "GroupAndProject");
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

        public async Task<ActionResult> ListProject(int id)
        {
            try {
                var group = await GroupService.GetGroupById(id);
                if (group != null)
                {
                    ViewBag.GroupName = group.GroupName;
                }
                else
                {
                    ViewBag.GroupName = "Không tìm thấy nhóm";
                }
                ViewBag.GroupId=id;
                return View(await ProjectService.GetAllProjectByGroupId(id)); 
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi khi lấy danh sách : " + ex.Message;
                return View(new List<ProjectItem>());
            }
            
        }
        public async Task<ActionResult> ListBoard(int id, int groupId)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            int UserId = int.Parse(userIdString);
            String getrole=await GroupDetailService.GetRoleByGroupAndUser(groupId, UserId);
            try
            {
                var boards = await BoardService.GetAllBoardByProjectId(id);
                List<TaskItem> tasks;
                try
                {
                    if (getrole == "Member")
                    {
                        tasks = await TaskDetailService.GetAllTaskByUserId(UserId);
                        ViewBag.Tasks = tasks;
                    }
                    else
                    {
                        tasks = await TaskService.GetAllTaskByProjectId(id);
                        ViewBag.Tasks = tasks;
                    }
                }
                catch (Exception taskEx)
                {
                    ViewBag.Tasks = new List<TaskItem>();
                    ViewBag.TaskError = "Không lấy được danh sách task: " + taskEx.Message;
                }
                ViewBag.ProjectId=id;
                ViewBag.GroupId = groupId;
                return View(boards);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi khi lấy danh sách Board: " + ex.ToString();
                return View(new List<BoardItem>());
            }
        }
        [HttpGet]
        public ActionResult CreateProject(int id)
        {
            ViewBag.Gid = id;
            return View(); 
        }
        [HttpPost]
        public async Task<ActionResult> CreateProject(ProjectItem model)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                ModelState.AddModelError("", "Không xác định được người dùng.");
                return View(model);
            }

            model.UserId = int.Parse(userIdString);
            model.CreatedAt = DateTime.Now;

            // Kiểm tra dữ liệu rỗng
            if (string.IsNullOrWhiteSpace(model.ProjectName))
            {
                ModelState.AddModelError("ProjectName", "Vui lòng nhập tên dự án.");
            }

            if (string.IsNullOrWhiteSpace(model.Descript))
            {
                ModelState.AddModelError("Descript", "Vui lòng nhập mô tả dự án.");
            }
            // Kiểm tra ngày bắt đầu và ngày kết thúc
            if (model.StartDate == default(DateTime))
            {
                ModelState.AddModelError("StartDate", "Vui lòng chọn ngày bắt đầu.");
            }

            if (model.EndDate == default(DateTime))
            {
                ModelState.AddModelError("EndDate", "Vui lòng chọn ngày kết thúc.");
            }

            if (model.EndDate < model.StartDate && model.EndDate != default(DateTime) && model.StartDate != default(DateTime))
            {
                ModelState.AddModelError("EndDate", "Ngày kết thúc phải sau ngày bắt đầu.");
            }

            if (ModelState.IsValid)
            {
                bool result = await ProjectService.CreateProject(model);
                if (result)
                    return RedirectToAction("ListProject", new { id = model.GroupId });
                else
                    ModelState.AddModelError("", "Tạo Project thất bại.");
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult CreateBoard(int id, int groupId)
        {
            ViewBag.ProjectId = id;
            ViewBag.GroupId = groupId;
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CreateBoard(BoardItem model,int groupId)
        {
            ViewBag.GroupId = groupId;
            model.CreatedAt = DateTime.Now;
            if (ModelState.IsValid)
            {
                bool result = await BoardService.CreateBoard(model);
                if (result)
                    return RedirectToAction("ListBoard", new { id = model.ProjectId , groupId = groupId });
                else
                    ModelState.AddModelError("", "Tạo Board thất bại.");
            }

            return View(model);
        }
        [HttpGet]
        public ActionResult AddMember(int id)
        {
            TempData["ShowModal"] = true;
            ViewBag.GroupId = id;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddMember(AddMemberViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Dữ liệu không hợp lệ.");
            var userid = await UserService.GetUserIdByEmail(model.Email);
            if (userid == null)
            {
                ModelState.AddModelError("", "Không tìm thấy người dùng.");
                return View(model);
            }
            var newMember = new GroupDetailItem
            {
                GroupId = model.GroupId,
                UserId = userid.UserId,
                RoleInGroup = model.RoleInGroup
            };

            bool result = await GroupDetailService.CreateGroupDetail(newMember);
            if (result)
                return RedirectToAction("Index");
            else
                ModelState.AddModelError("", "Add Member thất bại.");

            return RedirectToAction("ListProject", new { id = model.GroupId });
        }
        [HttpGet]
        public async Task<IActionResult> CreateTask(int projectId, int boardId,int groupId)
        {
            ViewBag.GroupId = groupId;
            ViewBag.ProjectId = projectId;
            ViewBag.BoardId = boardId;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateTask(TaskItem model, int groupId)
        {
           
            ViewBag.GroupId = groupId;

            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                ModelState.AddModelError("", "Không xác định được người dùng.");
                return View(model);
            }
            model.UserId = int.Parse(userIdString);
            model.CreatedDate = DateTime.Now;
            model.Status = "Doing";
            bool result = await TaskService.CreateTasks(model);
            if (result)
                return RedirectToAction("ListBoard", new { id = model.ProjectId, groupId = groupId });
            else
                ModelState.AddModelError("", "Add task thất bại.");

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> TaskDetail(int id,int groupId,int projectId)
        {
            ViewBag.ReturnUrl = Request.Headers["Referer"].ToString();
            ViewBag.GroupId = groupId;
            ViewBag.ProjectId = projectId;
            var UsersInput = await GroupDetailService.GetAllUserByGroupId(groupId);
            ViewBag.ListMember = UsersInput;
            return View(await TaskService.GetTaskByTaskId(id));
        }
        [HttpPost]
        public async Task<IActionResult> TaskDetail(TaskItem model)
        {
            return View(model);
        }
        public async Task<IActionResult> LoadReportList(int projectId)
        {
            var reports = await ReportService.GetAllReportByProjectId(projectId);
            return PartialView("LoadReportList", reports);
        }
        public async Task<IActionResult> LoadMemberList(int groupId)
        {
            var Users = await GroupDetailService.GetAllUserByGroupId(groupId);
            return PartialView("LoadMemberList", Users);
        }
        [HttpPost]
        public async Task<IActionResult> AddMemberToTask(TaskDetailItemInput model,int groupId,int projectId)
        {
            ViewBag.GroupId = groupId;
            ViewBag.ProjectId = projectId;
            if (model.TaskId == null || model.UserId == null)
            {
                TempData["Error"] = "Vui lòng chọn người dùng.";
                return RedirectToAction("TaskDetail", new { id = model.TaskId, groupId = ViewBag.GroupId ,projectId= projectId });
            }
            var result = await TaskDetailService.AddUserToTask(model);
            return RedirectToAction("TaskDetail", new { id = model.TaskId, groupId = ViewBag.GroupId, projectId= projectId });
        }
        [HttpPost]
        public async Task<IActionResult> CreateComment(CommentItemInput model, int groupId,int projectId)
        {
            ViewBag.GroupId = groupId;
            ViewBag.ProjectId = projectId;
            if (!ModelState.IsValid)
                return BadRequest("Dữ liệu không hợp lệ.");

            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                TempData["Error"] = "Không xác định người dùng.";
                return RedirectToAction("TaskDetail", new { id = model.TaskId, groupId= groupId, projectId = projectId }); // hoặc return View
            }

            model.UserId = int.Parse(userIdString);
            model.CreatedAt = DateTime.Now;

            var result = await CommentService.CreateComment(model);

            return RedirectToAction("TaskDetail", new { id = model.TaskId , groupId = groupId, projectId = projectId });
        }
        [HttpGet]
        public async Task<IActionResult> AddFile(int id, int groupId,int projectId)
        {
            ViewBag.GroupId = groupId;
            ViewBag.ProjectId = projectId;
            ViewBag.TaskId = id;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddFile(TaskFileInput model, int groupId, int projectId)
        {
            ViewBag.ProjectId = projectId;
            ViewBag.GroupId = groupId;
            if (!ModelState.IsValid)
                return BadRequest("Dữ liệu không hợp lệ.");
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                ModelState.AddModelError("", "Không xác định được người dùng.");
                return View(model);
            }
            model.UserId = int.Parse(userIdString);
            var TaskFileUrl = await TaskFileService.GetTaskFileUrl(model.FileInput);
            model.FilePath = TaskFileUrl.FileUrl;
            model.UploadedAt = DateTime.Now;
            var newFile = new TaskFileItem
            {
                TaskId = model.TaskId,
                FileName = model.FileName ?? model.FileInput.FileName,
                FilePath = model.FilePath,
                UploadedAt = model.UploadedAt,
                UserId = model.UserId
            };

            bool result = await TaskFileService.CreateTaskFile(newFile);

            if (result)
                return RedirectToAction("TaskDetail", new { id = model.TaskId, groupId= groupId, projectId = projectId });
            else
            {
                ModelState.AddModelError("", "Add thất bại.");
                return View(model);
            }
        }
        [HttpGet]
        public async Task<IActionResult> AddReport(int id, int groupId)
        {
            ViewBag.GroupId = groupId;
            ViewBag.ProjectId = id;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddReport(ReportItem model, int groupId)
        {
            ViewBag.GroupId = groupId;
            if (!ModelState.IsValid)
                return BadRequest("Dữ liệu không hợp lệ.");
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                ModelState.AddModelError("", "Không xác định được người dùng.");
                return View(model);
            }
            model.UserId = int.Parse(userIdString);
            model.CreatedAt = DateTime.Now;

            bool result = await ReportService.CreateReport(model);

            return RedirectToAction("ListBoard", new { id = model.ProjectId, groupId= groupId });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int taskId, string currentStatus, int ProjectId,int GroupId)
        {
            ViewBag.GroupId = GroupId;
            var UsersInput = await GroupDetailService.GetAllUserByGroupId(GroupId);
            var userIds = UsersInput.Select(u => u.UserId).ToList();
            var notifications = userIds.Select(id => new NotificationItem
            {
                UserId = id,
                Message = "Trạng thái công việc đã được cập nhật.",
                IsRead = false,
                SentDate = DateTime.Now
            }).ToList();
            await NoticeService.CreateNoticeList(notifications);



            string newStatus = currentStatus == "Delete"
                ? "Delete"
                : (currentStatus == "Done" ? "Doing" : "Done");

            bool result = await TaskService.UpdateTaskStatus_Query(taskId, newStatus);
            return RedirectToAction("ListBoard", new { id = ProjectId , groupId = GroupId });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            var result = await GroupService.UpdateGroupStatus(id, "Deleted");
            if (result)
            {
                TempData["Message"] = "Group deleted successfully.";
            }
            else
            {
                TempData["Message"] = "Failed to delete group.";
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> RenameGroup(int groupId, string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                TempData["Message"] = "Tên nhóm không hợp lệ.";
                return RedirectToAction("Index");
            }

            var result = await GroupService.UpdateGroupName(groupId, newName);

            TempData["Message"] = result ? "Đổi tên nhóm thành công!" : "Đổi tên thất bại.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProject(int id)
        {
            // Gọi API xóa
            using (var client = new HttpClient())
            {
                var response = await ProjectService.UpdateProjectStatus(id, "Delete");
                if (response)
                {
                    TempData["Message"] = "Xóa dự án thành công!";
                }
                else
                {
                    TempData["Error"] = "Không thể xóa dự án.";
                }
            }

            return RedirectToAction("Index", new { id = ViewBag.GroupId });
        }

        [HttpPost]
        public async Task<IActionResult> RenameProject(int projectId, string newName)
        {
            var success = await ProjectService.UpdateProjectName(projectId, newName);
            if (success)
                TempData["Message"] = "Đổi tên thành công!";
            else
                TempData["Error"] = "Lỗi khi đổi tên.";

            return RedirectToAction("Index", new { id = ViewBag.GroupId });
        }
    }

}
