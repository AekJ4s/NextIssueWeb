using Microsoft.AspNetCore.Mvc;
using NextIssueWeb.Data;
using NextIssueWeb.Models;
using NextIssueWeb.Services;
using System.Diagnostics;
using System.Security;
using static NextIssueWeb.Models.Metadata;

namespace NextIssueWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AccountSv _acSv;
        private readonly LoggerSv _lgSv;
        private readonly ProjectSv _pjSv;
        private readonly StatusSv _stSv;
        private readonly IssueSv _isSv;
        private readonly PositionSv _psSv;
        private readonly ImportanceSv _imSv;
        private readonly PictureSv _pictureSv;
        private readonly NextIssueContext _context;
        public readonly string Controller = "Home";
        public readonly string Version = "1.0.0";


        public HomeController(
            ILogger<HomeController> logger,
            ProjectSv pjSv,
            AccountSv acSv,
            LoggerSv lgSv,
            StatusSv stSv,
            IssueSv isSv,
            PositionSv psSv,
            ImportanceSv imSv,
            PictureSv pcSv,
            NextIssueContext context)
        {
                _logger = logger;
                _acSv = acSv ?? throw new ArgumentNullException(nameof(acSv));
                _lgSv = lgSv ?? throw new ArgumentNullException(nameof(lgSv));
                _pjSv = pjSv ?? throw new ArgumentNullException(nameof(pjSv));
                _stSv = stSv ?? throw new ArgumentNullException(nameof(stSv));
                _isSv = isSv ?? throw new ArgumentNullException(nameof(isSv));
                _psSv = psSv ?? throw new ArgumentNullException(nameof(psSv));
                _imSv = imSv ?? throw new ArgumentNullException(nameof(imSv));
                _pictureSv = pcSv ?? throw new ArgumentNullException(nameof(pcSv));
        }

        public IActionResult Index()
        {
            try
            {
                var user = HttpContext.Session.GetString("Username");
                var token = HttpContext.Session.GetString("Token");
                var permission = HttpContext.Session.GetString("Permission");
                if (user != null && token != null)
                {
                    Metadata.InterfaceHome interfaceHome = new Metadata.InterfaceHome();
                    var userData = _acSv.GetUserByName(user);
                    var issueList = _isSv.GetIssueLists().Data;
                    interfaceHome.TicketCount = issueList.Count;
                    interfaceHome.TicketCloseCount = issueList.Where(db => db.StatusId == 5).Count();
                    interfaceHome.OpenTicketCount = issueList.Where(db => db.InformerId == userData.Data.Id).Count();
                    interfaceHome.CloseTicketCount = issueList.Where(db => db.InformerId == userData.Data.Id && db.StatusId == 5).Count();
                    interfaceHome.Version = Version;

                    #region Count Issue Of Any project
                    var projectDetailLists = new List<Metadata.NprojectIssueCount>();
                    var projectLists = _pjSv.GetProjectActiveLists().Data;
                    foreach(var pjLst in projectLists) 
                    {
                        var projectIssueCount = new NprojectIssueCount()
                        {
                            ProjectName = pjLst.Name,
                            IssueCount = _isSv.GetIssueListsByProjectId(pjLst.Id).Data.Count(),
                            UserCreating = pjLst.CreateBy,
                            Status = pjLst.Status
                        };
                        projectDetailLists.Add(projectIssueCount);
                    }
                    #endregion
                    interfaceHome.projectLists = projectDetailLists.OrderByDescending(x=>x.IssueCount).ToList();
                    return View(interfaceHome);
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult NOTFOUND()
        {
            return View();
        }
    }
}
