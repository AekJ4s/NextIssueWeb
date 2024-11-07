using Microsoft.AspNetCore.Mvc;
using NextIssueWeb.Data;
using NextIssueWeb.Models;
using NextIssueWeb.Services;
using System.Diagnostics;
using System.Security;

namespace NextIssueWeb.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ILogger<ProjectController> _logger;
        private readonly AccountSv _acSv;
        private readonly LoggerSv _lgSv;
        private readonly ProjectSv _pjSv;
        private readonly StatusSv _stSv;
        private readonly NextIssueContext _context;
        public readonly string Controller = "Project";
        public ProjectController(ILogger<ProjectController> logger,
            ProjectSv pjSv,
            AccountSv acSv,
            LoggerSv lgSv,
            StatusSv stSv,
            NextIssueContext context)
        {
            _logger = logger;
            _acSv = acSv ?? throw new ArgumentNullException(nameof(acSv));
            _lgSv = lgSv ?? throw new ArgumentNullException(nameof(lgSv));
            _pjSv = pjSv ?? throw new ArgumentNullException(nameof(pjSv));
            _stSv = stSv ?? throw new ArgumentNullException(nameof(stSv));
        }

        [HttpGet]
        public IActionResult popup()
        {
            try
            {
                var user = HttpContext.Session.GetString("Username");
                var token = HttpContext.Session.GetString("Token");
                var permission = HttpContext.Session.GetString("Permission");
                if (user != null && token != null)
                {
                    var model = new Metadata.NprojectCreate();
                    model.StatusLists = _stSv.GetListsStatus(1).Data;
                    model.Status = 1;
                    model.CreateDate = DateTime.Now;
                    return PartialView(nameof(popup), model);

                }
                else
                {
                    return RedirectToAction("LoginPage", "Login");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("LoginPage", "Login");
            }

        }

        [HttpPost]
        public IActionResult CreateProject(Metadata.NprojectCreate model)
        {

            try
            {
                var user = HttpContext.Session.GetString("Username");
                var id = HttpContext.Session.GetString("Id");
                var token = HttpContext.Session.GetString("Token");
                var permission = HttpContext.Session.GetString("Permission");
                if (user != null && token != null)
                {
                    var rs = _pjSv.CreateProject(model);
                    if (rs.IsSuccess == true)
                    {
                        var log = new Metadata.NloggerCreate()
                        {
                            Controller = Controller,
                            System_id = 2,
                            Loguser = int.Parse(id),
                            Name = "สร้าง Project",
                            CreateDate = DateTime.Now
                        };
                        _lgSv.CreateLog(log);

                        TempData["SuccessMessage"] = rs.Message;
                        return RedirectToAction("Index", "Home");
                    }
                    TempData["ErrorMessage"] = rs.Message;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("LoginPage", "Login");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("LoginPage", "Login");
            }

        }

    }
}
