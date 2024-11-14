using Microsoft.AspNetCore.Mvc;
using NextIssueWeb.Data;
using NextIssueWeb.Models;
using NextIssueWeb.Services;
using System.Diagnostics;
using System.Security;
using static NextIssueWeb.Models.ViewModel;

namespace NextIssueWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AccountSv _acSv;
        private readonly LoggerSv _lgSv;
        private readonly ProjectSv _pjSv;
        private readonly StatusSv _stSv;
        private readonly TicketSv _tkSv;
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
            TicketSv tkSv,
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
                _tkSv = tkSv ?? throw new ArgumentNullException(nameof(tkSv));
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
                    ViewModel model = new ViewModel();
                    var userData = _acSv.GetUserByName(user);
                    var TicketLists = _tkSv.GetTicketLists().Data;
                    model.ticketCount = TicketLists.Count;
                    model.ticketCloseCount = TicketLists.Where(db => db.StatusId == 5).Count();
                    model.ticketOpenCount = TicketLists.Where(db => db.InformerId == userData.Data.Id).Count();
                    model.YourticketCloseCount = TicketLists.Where(db => db.InformerId == userData.Data.Id && db.StatusId == 5).Count();
                    model.ProjectWithTicketsLists = new List<ProjectWithTickets>();
                    #region Count Issue Of Any project
                    foreach( var i in _pjSv.GetProjectLists().Data)
                    {
                        var projectXticket = new ProjectWithTickets()
                        {
                            ProjectId = i.Id,
                            ProjectName = i.Name,
                            ProjectStatus = i.Status,
                            CreateBy = i.CreateBy,
                            CreateDate = i.CreateDate.Value,
                            UpdateBy = i.UpdateBy,
                            UpdateDate = i.UpdateDate.Value,
                            TicketLists = _tkSv.GetTicketListsByProjectId(i.Id).Data,
                            
                        };
                        projectXticket.TicketsCount = projectXticket.TicketLists.Count();
                        model.ProjectWithTicketsLists.Add(projectXticket);
                    }
                    #endregion

                    return View(model);
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
