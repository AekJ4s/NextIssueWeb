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
                    var model = new ViewModel();
                    model.statusLists = _stSv.GetListsStatusByTaskId(1).Data;
                    model.statusId = 1;
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
        public IActionResult CreateProject(ViewModel model)
        {

            try
            {
                var user = HttpContext.Session.GetString("Username");
                var id = HttpContext.Session.GetString("Id");
                var token = HttpContext.Session.GetString("Token");
                var permission = HttpContext.Session.GetString("Permission");
                if (user != null && token != null)
                {
                    model.Checkbox1 = false;
                    model.Checkbox2 = false;
                    model.Nproject.Id = Guid.NewGuid();
                    model.Nproject.CreateBy = int.Parse(id);
                    model.Nproject.UpdateBy = int.Parse(id);
                    var rs = _pjSv.CreateProject(model.Nproject);
                    if (rs.IsSuccess == true)
                    {
                        var log = new Nlogger()
                        {
                            Controller = Controller,
                            SystemId = 2,
                            LogBy = int.Parse(id),
                            Name = "สร้าง Project @"+ user,
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
