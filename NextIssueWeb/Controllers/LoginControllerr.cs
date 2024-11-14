using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextIssueWeb.Data;
using NextIssueWeb.Models;
using NextIssueWeb.Services;
using System.Diagnostics;
using System;
using System.Security;
using Microsoft.Data.SqlClient;

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
            LoginForm model = new LoginForm();
            return View(model);
        }

        [HttpPost]
        public IActionResult LoginPage(LoginForm form)
        {
            try
            {
                var rs = _acSv.Login(form.Username, form.Password);
                if (rs.IsSuccess)
                {
                    var nlogger = new Nlogger
                    {
                        Name = "Login User @" + form.Username,
                        LogBy = rs.Data.Id,
                        Detail = rs.Message,
                        Controller = Controller,
                        SystemId = 1
                    };
                    _lgSv.CreateLog(nlogger);
                    var token = _acSv.GenerateToken(form.Username, form.Password);
                    var per = _acSv.GetPositionById(rs.Data.PositionId.Value).Data;
                    HttpContext.Session.SetString("Id", rs.Data.Id.ToString());
                    HttpContext.Session.SetString("Username", rs.Data.Username);
                    HttpContext.Session.SetString("Token", token);
                    HttpContext.Session.SetString("Permission", per.Name?.ToString() ?? string.Empty);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    if(rs.IsSuccess == false && rs.Code == 53)
                    {
                        return RedirectToAction("SqlError");
                    }
                    var nlogger = new Nlogger()
                    {
                        Name = "Login User @" + form.Username,
                        LogBy = 0,
                        Detail = rs.Message,
                        Controller = Controller,
                        SystemId = 1
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

        public IActionResult SqlError()
        {
            try
            {
               return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("LoginPage");
            }
        }

    }
}
