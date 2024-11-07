using Microsoft.AspNetCore.Mvc;
using NextIssueWeb.Data;
using NextIssueWeb.Models;
using NextIssueWeb.Services;
using System.Diagnostics;
using System.Security;

namespace NextIssueWeb.Controllers
{
    public class IssueController : Controller
    {
        private readonly ILogger<IssueController> _logger;
        private readonly AccountSv _acSv;
        private readonly LoggerSv _lgSv;
        private readonly ProjectSv _pjSv;
        private readonly StatusSv _stSv;
        private readonly IssueSv _isSv;

        private readonly NextIssueContext _context;
        public readonly string Controller = "Issue";
        public IssueController(ILogger<IssueController> logger,
            ProjectSv pjSv,
            AccountSv acSv,
            LoggerSv lgSv,
            StatusSv stSv,
            IssueSv isSv,
            NextIssueContext context)
        {
            _logger = logger;
            _acSv = acSv ?? throw new ArgumentNullException(nameof(acSv));
            _lgSv = lgSv ?? throw new ArgumentNullException(nameof(lgSv));
            _pjSv = pjSv ?? throw new ArgumentNullException(nameof(pjSv));
            _stSv = stSv ?? throw new ArgumentNullException(nameof(stSv));
            _isSv = isSv ?? throw new ArgumentNullException(nameof(isSv));

        }

        public IActionResult popup1()
        {
            try
            {
                var user = HttpContext.Session.GetString("Username");
                var token = HttpContext.Session.GetString("Token");
                var permission = HttpContext.Session.GetString("Permission");
                if (user != null && token != null)
                {
                    var model = new Metadata.NissueCreate();
                    model.StatusLists = _stSv.GetListsStatus(2).Data;
                    model.UserLists = _acSv.GetAllUserPermission().Data;
                    model.StatusId = 1;
                    return PartialView("popup1",model);
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
        public IActionResult OpenTicket(Metadata.NissueCreate model)
        {
            try
            {
                var user = HttpContext.Session.GetString("Username");
                var id = HttpContext.Session.GetString("Id");
                var token = HttpContext.Session.GetString("Token");
                var permission = HttpContext.Session.GetString("Permission");

                if (user != null && token != null)
                {
                    var rs = _isSv.CreateIssue(model);
                    if (rs.IsSuccess == true)
                    {
                        var log = new Metadata.NloggerCreate()
                        {
                            Controller = Controller,
                            System_id = 3,
                            Loguser = int.Parse(id),
                            Name = "สร้าง Issue @"+ user,
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
