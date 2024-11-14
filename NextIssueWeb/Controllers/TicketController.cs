using Microsoft.AspNetCore.Mvc;
using NextIssueWeb.Data;
using NextIssueWeb.Models;
using NextIssueWeb.Services;
using System.Diagnostics;
using System.Security;
using static NextIssueWeb.Models.ViewModel;

namespace NextIssueWeb.Controllers
{
    public class TicketController : Controller
    {
        private readonly ILogger<TicketController> _logger;
        private readonly AccountSv _acSv;
        private readonly LoggerSv _lgSv;
        private readonly ProjectSv _pjSv;
        private readonly StatusSv _stSv;
        private readonly TicketSv _tkSv;
        private readonly PositionSv _psSv;
        private readonly ImportanceSv _imSv;
        private readonly PictureSv _pictureSv;
        private readonly NextIssueContext _context;
        public readonly string Controller = "Issue";
        public TicketController(ILogger<TicketController> logger,
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


        #region View
        public IActionResult popup1()
        {
            try
            {
                var user = HttpContext.Session.GetString("Username");
                var token = HttpContext.Session.GetString("Token");
                var permission = HttpContext.Session.GetString("Permission");
                if (user != null && token != null)
                {
                    var model = new ViewModel();
                    model.statusLists = _stSv.GetListsStatusByTaskId(2).Data;
                    model.UserLists = _acSv.GetAllUser().Data;
                    model.PositionList = _psSv.GetPositionLists().Data.Where(db => db.Id != 0).ToList();
                    model.importantsLists = _imSv.GetListsimportance().Data;
                    model.projectsLists = _pjSv.GetProjectLists().Data;
                    model.formFilesLists = new List<IFormFile>();
                    return PartialView("popup1", model);
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

        [HttpGet]
        public IActionResult Lists()
        {
            var user = HttpContext.Session.GetString("Username");
            var token = HttpContext.Session.GetString("Token");
            var permission = HttpContext.Session.GetString("Permission");
            var model = new ViewModel();
            try
            {
                model.TicketsList = _tkSv.GetTicketLists().Data;
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        //public IActionResult ListsFilther(Guid ProjectId)
        //{
        //    var model = new List<Metadata.NissueLists>();
        //    try
        //    {
        //        var MergeLst = _isSv.GetIssueListsByProjectId(ProjectId);

        //        if (MergeLst.IsSuccess == true && MergeLst.Data.Count > 0)
        //        {
        //            foreach (var data in MergeLst.Data)
        //            {
        //                var project = _pjSv.GetProjectById(data.ProjectId).Data;
        //                var issue = _isSv.GetIssueById(data.IssueId).Data;
        //                var record = new Metadata.NissueLists()
        //                {
        //                    IssueName = issue.Name,
        //                    IssueId = issue.Id,
        //                    IssueStatus = issue.StatusId,
        //                    IssueCreateDate = issue.CreateDate,
        //                    ProjectId = project.Id,
        //                    ProjectName = project.Name,
        //                    ProjectCreateDate = project.CreateDate,
        //                    InformerId = issue.CreateBy,
        //                    InformerName = _acSv.GetUserById(issue.CreateBy).Data.Aka
        //                };

        //                if (issue.ResponsibleId != 0)
        //                {
        //                    record.ResponsibleId = issue.ResponsibleId;
        //                    record.ResponsibleName = _acSv.GetUserById(issue.ResponsibleId).Data.Aka;
        //                    record.ResponsibleGroupId = issue.ResponsibleGroupId;
        //                    record.ResponsibleGroupName = _psSv.GetPositionById(issue.ResponsibleGroupId).Data.Name;
        //                }
        //                else
        //                {
        //                    record.ResponsibleId = 0;
        //                    record.ResponsibleName = "ยังไม่มีผู้รับผิดชอบ";
        //                    record.ResponsibleGroupId = issue.ResponsibleGroupId;
        //                    record.ResponsibleGroupName = _psSv.GetPositionById(issue.ResponsibleGroupId).Data.Name;
        //                }
        //            }
        //        }
        //        return View(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["ErrorMessage"] = ex.Message;
        //        return RedirectToAction("Index", "Home");
        //    }
        //}

        [HttpGet]
        public IActionResult Detail(int TicketId)
        {
            var user = HttpContext.Session.GetString("Username");
            var token = HttpContext.Session.GetString("Token");
            var permission = HttpContext.Session.GetString("Permission");
            var model = new ViewModel();
            try
            {
                if (user != null)
                {

                    model.projectsLists = _pjSv.GetProjectLists().Data;
                    model.Nticket = _tkSv.GetTicketById(TicketId).Data;
                    model.NpictureLists = _pictureSv.GetPictureListByTicketId(TicketId).Data;
                    model.statusLists = _stSv.GetListsStatusByTaskId(2).Data;
                    model.statusprojectsLists = _stSv.GetListsStatusByTaskId(1).Data;
                    model.projectId = model.Nticket.ProjectId;
                    model.statusId = model.Nticket.StatusId;
                    model.stringsLists = new List<string>();
                    foreach (var record in model.NpictureLists)
                    {
                        if (record?.Picture != null)
                        {
                            string base64Image = Convert.ToBase64String(record.Picture);
                            model.stringsLists.Add($"data:image/jpeg;base64,{base64Image}");
                        }
                    }

                    return View(model);
                }
                return RedirectToAction("Login", "Login");


            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion

        #region Create
        [HttpPost]
        public IActionResult OpenTicket(ViewModel model)
        {
            try
            {

                var user = HttpContext.Session.GetString("Username");
                var id = HttpContext.Session.GetString("Id");
                var token = HttpContext.Session.GetString("Token");
                var permission = HttpContext.Session.GetString("Permission");

                if (user != null && token != null)
                {
                    model.Nticket.InformerId = int.Parse(id);
                    model.Nticket.CreateBy = int.Parse(id);
                    model.Nticket.UpdateBy = int.Parse(id);
                    var rs = _tkSv.CreateTicket(model.Nticket);
                    if (rs.IsSuccess == true)
                    {
                        var log = new Nlogger()
                        {
                            Controller = Controller,
                            SystemId = 3,
                            LogBy = int.Parse(id),
                            Name = "สร้าง Issue @" + user,
                            CreateDate = DateTime.Now
                        };
                        _lgSv.CreateLog(log);
                    }
                    if (model.formFilesLists != null && model.formFilesLists.Count > 0 && model.Checkbox2 == true)
                    {
                        var pictureFile = new List<Npicture>();
                        foreach (var record in model.formFilesLists)
                        {
                            var file = ConvertToBytes(record);
                            var sendThis = new Npicture();
                            sendThis.Picture = file;
                            sendThis.TicketId = rs.Data.Id;
                            sendThis.UploadDate = DateTime.Now;
                            sendThis.TicketId = rs.Data.Id;
                            var rsFile = _pictureSv.CreatePicture(sendThis);
                            if (rsFile.IsSuccess == true)
                            {
                                var logFile = new Nlogger()
                                {
                                    Controller = Controller,
                                    SystemId = 4,
                                    LogBy = int.Parse(id),
                                    Name = "สร้าง Picture File @" + user,
                                    CreateDate = DateTime.Now
                                };
                                _lgSv.CreateLog(logFile);     
                            }
                        }
                    }
                    TempData["SuccessMessage"] = "Open Ticket Success";
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
                return RedirectToAction("Index", "Home");
            }

        }
        [HttpDelete]
        public JsonResult DeleteTicket(int TicketId)
        {
            try
            {
                _tkSv.DeleteTicket(TicketId);
                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }

        //[HttpPost]
        //public IActionResult UpdateTicket(Metadata.TicketWithProjectAndPicture Model)
        //{
        //    var user = HttpContext.Session.GetString("Username");
        //    var id = HttpContext.Session.GetString("Id");
        //    var token = HttpContext.Session.GetString("Token");
        //    var permission = HttpContext.Session.GetString("Permission");
        //    try
        //    {
        //        var Ticket = _isSv.GetIssueById(Model.Issue.Id);
        //        Ticket.Data.StatusId = Model.StatusTicketId;
        //        Ticket.Data.Name = Model.Issue.Name;
        //        var rs = _isSv.UpdateTicket(Ticket.Data);

        //        if(rs.IsSuccess == true)
        //        {
        //            TempData["SuccessMessage"] = "Update Ticket Successfully";
        //            return RedirectToAction("Detail",new { TicketId = rs.Data.Id});
        //        }
        //        else
        //        {
        //            TempData["ErrorMessage"] = rs.Message;
        //            return RedirectToAction("Index", "Home");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["ErrorMessage"] = ex.Message;
        //        return RedirectToAction("Index", "Home");
        //    }
        //}
        #endregion

        #region Update Ticket
        [HttpPost]
        public IActionResult UpdateTicket(ViewModel model)
        {
            var user = HttpContext.Session.GetString("Username");
            var id = HttpContext.Session.GetString("Id");
            var token = HttpContext.Session.GetString("Token");
            var permission = HttpContext.Session.GetString("Permission");
            try
            {
                model.Nticket.UpdateDate = DateTime.Now;
                model.Nticket.UpdateBy = int.Parse(id);
                var rs = _tkSv.UpdateTicket(model.Nticket);
                if(rs.IsSuccess == true)
                {
                    TempData["SuccessMessage"] = rs.Message;
                }
                return RedirectToAction("Detail",new { TicketId = model.Nticket.Id } );
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Detail", new { TicketId = model.Nticket.Id });
            }
        }
        #endregion
        #region function
        public byte[] ConvertToBytes(IFormFile file)
        {
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                return ms.ToArray();
            }
        }

        [HttpGet]
        public JsonResult jrGetUserByGroup(int groupId)
        {
            var returnThis = _acSv.GetListUserByGroupId(groupId).Data;
            return Json(returnThis);
        }

        [HttpGet]
        public JsonResult jrGetProjectLists()
        {
            var returnThis = _pjSv.GetProjectLists().Data;
            return Json(returnThis);
        }

        [HttpGet]
        public JsonResult jrGetStatusLists(int type)
        {
            var returnThis = _stSv.GetListsStatusByTaskId(type).Data;
            return Json(returnThis);
        }
        #endregion
    }
}
