using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task_API.Models;
using Task_API.MyModels;

namespace Task_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        private readonly TaskManagementContext _context;
        public BoardController(TaskManagementContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult getBoard()
        {
            try
            {
                TaskManagementContext db = new TaskManagementContext();
                var ds = db.Boards.Select(t => new
                {
                    BoardId = t.BoardId,
                    ProjectId = t.ProjectId,
                    BoardName = t.BoardName,
                    CreatedAt = t.CreatedAt,

                }).ToList();
                return Ok(ds);
        }
            catch
            {
                return BadRequest();// đúng thì ok, sai thì badrequest()
                                    //NotFound -> không tìm thấy!!!
            }

        }
        [HttpGet("GetListBoardByProjectId/{projectId}")]
        public ActionResult<IEnumerable<Board>> GetListBoardByProjectId(int projectId)
        {
            var boards = (from p in _context.Boards
                            where p.ProjectId == projectId
                            select p).ToList();
            return Ok(boards);
        }
        [HttpPost]
        public IActionResult addBoard(Add_Board x)
        {
            try
            {
                TaskManagementContext db = new TaskManagementContext();
                Board newG = new Board
                {
                    BoardName = x.BoardName,
                    ProjectId = x.ProjectId,
                    CreatedAt = x.CreatedAt,
                };
                db.Boards.Add(newG);
                db.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
