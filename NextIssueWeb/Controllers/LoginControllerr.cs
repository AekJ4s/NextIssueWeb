using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextIssueWeb.Data;
using NextIssueWeb.Models;
using NextIssueWeb.Services;
using System.Diagnostics;
using System;
using System.Security;

namespace NextIssueWeb.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly AccountSv _acSv;
        private readonly LoggerSv _lgSv;
        private readonly NextIssueContext _context;
        private static readonly TimeSpan TokenLifetime = TimeSpan.FromMinutes(10);
        public readonly string Controller = "Login";

        // Constructor to include dependency injection for AccountSv and LoggerSv
        public LoginController(ILogger<LoginController> logger, AccountSv acSv, LoggerSv lgSv, NextIssueContext context)
        {
            _logger = logger;
            _acSv = acSv ?? throw new ArgumentNullException(nameof(acSv));
            _lgSv = lgSv ?? throw new ArgumentNullException(nameof(lgSv));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IActionResult LoginPage()
        {
            Metadata.NuserLogin model = new Metadata.NuserLogin();
            return View(model);
        }

        [HttpPost]
        public IActionResult LoginPage(Metadata.NuserCreate form)
        {
            try
            {
                var rs = _acSv.Login(form.Username, form.Password);
                if (rs.IsSuccess)
                {
                    var nlogger = new Metadata.NloggerCreate()
                    {
                        Name = "Login User @" + form.Username,
                        Loguser = rs.Data.Id,
                        Detail = rs.Message,
                        Controller = Controller,
                        System_id = 1
                    };
                    _lgSv.CreateLog(nlogger);
                    var token = _acSv.GenerateToken(form.Username, form.Password);
                    var per = _acSv.GetPemissionUserById(rs.Data.Id).Data;
                    var permission = per.OrderBy(db => db.Name).FirstOrDefault();
                    HttpContext.Session.SetString("Username", rs.Data.Username);
                    HttpContext.Session.SetString("Id", rs.Data.Id.ToString());
                    HttpContext.Session.SetString("Token", token);
                    HttpContext.Session.SetString("Permission", permission.Name?.ToString() ?? string.Empty);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var nlogger = new Metadata.NloggerCreate()
                    {
                        Name = "Login User @" + form.Username,
                        Loguser = 0,
                        Detail = rs.Message,
                        Controller = Controller,
                        System_id = 1
                    };
                    _lgSv.CreateLog(nlogger);
                    TempData["ErrorMessage"] = rs.Message;
                    return RedirectToAction("LoginPage");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("LoginPage");
            }
        }

        public IActionResult Logout()
        {
            try
            {
                HttpContext.Session.Clear();
                return RedirectToAction("LoginPage");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("LoginPage");
            }
        }
    }
}
