using Microsoft.AspNetCore.Mvc;
using NextIssueWeb.Data;
using NextIssueWeb.Models;
using NextIssueWeb.Services;
using System.Diagnostics;
using System.Security;
using static NextIssueWeb.Models.Metadata;

namespace NextIssueWeb.Controllers
{
    public class TicketController : Controller
    {
        private readonly ILogger<TicketController> _logger;
        private readonly AccountSv _acSv;
        private readonly LoggerSv _lgSv;
        private readonly ProjectSv _pjSv;
        private readonly StatusSv _stSv;
        private readonly IssueSv _isSv;
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
                    var model = new Metadata.NissueCreate();
                    model.StatusLists = _stSv.GetListsStatus(2).Data;
                    model.UserLists = _acSv.GetAllUserPermission().Data;
                    model.PositionLists = _psSv.GetPositionLists().Data.Where(db => db.Id != 0).ToList();
                    model.ImportanceLists = _imSv.GetListsimportance().Data;
                    model.ProjectLists = _pjSv.GetProjectLists().Data;
                    model.ProjectId = Guid.Empty;
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
            var model = new List<Metadata.NissueLists>();
            try
            {
                var MergeLst = _isSv.GetMergeProjectWithIssueLists();
                var userDetail = _acSv.GetUserByName(user).Data;
                NuserWithPermission userWithPermission = _acSv.GetPermissionUserById2(userDetail.Id).Data;
                if (MergeLst.IsSuccess == true && MergeLst.Data.Count > 0)
                {
                    foreach (var data in MergeLst.Data)
                    {
                        var project = _pjSv.GetProjectById(data.ProjectId).Data;
                        if(project.Status == 1 && permission == "Admin")
                        {
                            var issue = _isSv.GetIssueById(data.IssueId).Data;
                            var informer = _acSv.GetUserById(issue.CreateBy).Data;
                            var role = _acSv.GetPermissionUserById2(informer.Id).Data;
                            var record = new Metadata.NissueLists()
                            {
                                IssueName = issue.Name,
                                IssueId = issue.Id,
                                IssueStatus = issue.StatusId,
                                IssueStatusName = _stSv.GetStatusById(issue.StatusId).Data.Name,
                                IssueCreateDate = issue.CreateDate,
                                ProjectId = project.Id,
                                ProjectName = project.Name,
                                ProjectCreateDate = project.CreateDate,
                                InformerId = issue.CreateBy,
                                InformerName = role.Aka + "(" +role.Permission+ ")",
                            };

                            if (issue.ResponsibleId != 0)
                            {
                                record.ResponsibleId = issue.ResponsibleId;
                                record.ResponsibleName = _acSv.GetUserById(issue.ResponsibleId).Data.Aka;
                                record.ResponsibleGroupId = issue.ResponsibleGroupId;
                                record.ResponsibleGroupName = _psSv.GetPositionById(issue.ResponsibleGroupId).Data.Name;
                            }
                            else
                            {
                                record.ResponsibleId = 0;
                                record.ResponsibleName = "-";
                                record.ResponsibleGroupId = issue.ResponsibleGroupId;
                                record.ResponsibleGroupName = _psSv.GetPositionById(issue.ResponsibleGroupId).Data.Name;
                            }

                            model.Add(record);
                        }
                        else if (project.Status == 1 && permission != "Admin")
                        {
                            var issue = _isSv.GetIssueByIdAndAboutMe(data.IssueId,userWithPermission).Data;
                            var record = new Metadata.NissueLists()
                            {
                                IssueName = issue.Name,
                                IssueId = issue.Id,
                                IssueStatus = issue.StatusId,
                                IssueStatusName = _stSv.GetStatusById(issue.StatusId).Data.Name,
                                IssueCreateDate = issue.CreateDate,
                                ProjectId = project.Id,
                                ProjectName = project.Name,
                                ProjectCreateDate = project.CreateDate,
                                InformerId = issue.CreateBy,
                                InformerName = _acSv.GetUserById(issue.CreateBy).Data.Aka
                            };

                            if (issue.ResponsibleId != 0)
                            {
                                record.ResponsibleId = issue.ResponsibleId;
                                record.ResponsibleName = _acSv.GetUserById(issue.ResponsibleId).Data.Aka;
                                record.ResponsibleGroupId = issue.ResponsibleGroupId;
                                record.ResponsibleGroupName = _psSv.GetPositionById(issue.ResponsibleGroupId).Data.Name;
                            }
                            else
                            {
                                record.ResponsibleId = 0;
                                record.ResponsibleName = "ยังไม่มีผู้รับผิดชอบ";
                                record.ResponsibleGroupId = issue.ResponsibleGroupId;
                                record.ResponsibleGroupName = _psSv.GetPositionById(issue.ResponsibleGroupId).Data.Name;
                            }

                            model.Add(record);
                        }

                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index","Home");
            }
        }

        [HttpGet]
        public IActionResult ListsFilther(Guid ProjectId)
        {
            var model = new List<Metadata.NissueLists>();
            try
            {
                var MergeLst = _isSv.GetIssueListsByProjectId(ProjectId);

                if (MergeLst.IsSuccess == true && MergeLst.Data.Count > 0)
                {
                    foreach (var data in MergeLst.Data)
                    {
                        var project = _pjSv.GetProjectById(data.ProjectId).Data;
                        var issue = _isSv.GetIssueById(data.IssueId).Data;
                        var record = new Metadata.NissueLists()
                        {
                            IssueName = issue.Name,
                            IssueId = issue.Id,
                            IssueStatus = issue.StatusId,
                            IssueCreateDate = issue.CreateDate,
                            ProjectId = project.Id,
                            ProjectName = project.Name,
                            ProjectCreateDate = project.CreateDate,
                            InformerId = issue.CreateBy,
                            InformerName = _acSv.GetUserById(issue.CreateBy).Data.Aka
                        };

                        if (issue.ResponsibleId != 0)
                        {
                            record.ResponsibleId = issue.ResponsibleId;
                            record.ResponsibleName = _acSv.GetUserById(issue.ResponsibleId).Data.Aka;
                            record.ResponsibleGroupId = issue.ResponsibleGroupId;
                            record.ResponsibleGroupName = _psSv.GetPositionById(issue.ResponsibleGroupId).Data.Name;
                        }
                        else
                        {
                            record.ResponsibleId = 0;
                            record.ResponsibleName = "ยังไม่มีผู้รับผิดชอบ";
                            record.ResponsibleGroupId = issue.ResponsibleGroupId;
                            record.ResponsibleGroupName = _psSv.GetPositionById(issue.ResponsibleGroupId).Data.Name;
                        }
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet] 
        public IActionResult Detail(int TicketId)
        {
            var user = HttpContext.Session.GetString("Username");
            var token = HttpContext.Session.GetString("Token");
            var permission = HttpContext.Session.GetString("Permission");
            var model = new Metadata.TicketWithProjectAndPicture();
            try
            {
                if (user != null)
                {
                    var merge = _isSv.GetMergeProjectWithIssueLists();
                    model.Issue = _isSv.GetIssueById(TicketId).Data;
                    model.IssueStatusName = _stSv.GetStatusById(model.Issue.StatusId).Data.Name;
                    model.Project = _isSv.GetProjectByTicket(TicketId).Data;
                    model.ProjectStatusName = _stSv.GetStatusById(model.Project.Status).Data.Name;
                    model.Pictures = _isSv.GetPictureListsByTicketId(TicketId).Data;
                    model.ProjectLst = _pjSv.GetProjectActiveLists().Data;
                    model.StatusTicketLst = _stSv.GetListsStatus(2).Data;
                    model.StatusProjectLst = _stSv.GetListsStatus(1).Data;
                    model.StatusTicketId = model.Issue.StatusId;
                    model.StatusProjectId = model.Project.Status;
                    model.ProjectId = model.Project.Id;

                    foreach (var record in model.Pictures)
                    {
                        if (record?.Picture != null)
                        {
                            string base64Image = Convert.ToBase64String(record.Picture);
                            model.Files.Add($"data:image/jpeg;base64,{base64Image}");
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
                    if (model.StatusId == 0)
                    {
                        TempData["WarningMessage"] = "กรุณาเลือกสถานะของปัญหา";
                        return PartialView("popup1",model);
                    }
                    if (model.ResponsibleGroupId == 0 && model.Checkbox1 == false) {
                        TempData["WarningMessage"] = "กรุณาเลือกกลุ่มผู้รับผิดชอบ";
                        return PartialView("popup1", model);

                    }
                    if (model.ResponsibleGroupId == 0 && model.Checkbox1 == true)
                    {
                        TempData["WarningMessage"] = "กรุณาเลือกกลุ่มผู้รับผิดชอบ";
                        return PartialView("popup1", model);
                    }
                    if (model.ResponsibleGroupId != 0 && model.ResponsibleId == 0 && model.Checkbox1 == true) 
                    { 
                        TempData["WarningMessage"] = "กรุณาเลือกผู้รับผิดชอบ";
                        return PartialView("popup1", model);

                    }

                    model.InformerId = int.Parse(id);
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
                    }
                    if (model.Picture.Count > 0)
                    {
                        var pictureFile = new List<Npicture>();
                        foreach (var picture in model.Picture)
                        {
                            var file = ConvertToBytes(picture);
                            var sendThis = new Npicture();
                            sendThis.Picture = file;
                            var rsFile = _pictureSv.CreatePicture(sendThis);
                            if (rsFile.IsSuccess == true)
                            {
                                var logFile = new Metadata.NloggerCreate()
                                {
                                    Controller = Controller,
                                    System_id = 4,
                                    Loguser = int.Parse(id),
                                    Name = "สร้าง Picture File @" + user,
                                    CreateDate = DateTime.Now
                                };
                                _lgSv.CreateLog(logFile);
                                pictureFile.Add(rsFile.Data);
                                var Merge = new MergeissuePicture();
                                var rsMerge = new ResponseModel<MergeissuePicture>();
                                if (rs.IsSuccess == true && rsFile.IsSuccess == true)
                                {
                                    Merge.PictureId = rsFile.Data.Id;
                                    Merge.IssueId = rs.Data.Id;
                                    rsMerge = _pictureSv.MergeIssueWithPicture(Merge);
                                }
                                if(rsMerge.IsSuccess == true)
                                {
                                    logFile = new Metadata.NloggerCreate()
                                    {
                                        Controller = Controller,
                                        System_id = 4,
                                        Loguser = int.Parse(id),
                                        Name = "อัพโหลด Picture File @" + user,
                                        CreateDate = DateTime.Now
                                    };
                                    _lgSv.CreateLog(logFile);
                                }
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

        [HttpPost]
        public IActionResult UpdateTicket(Metadata.TicketWithProjectAndPicture Model)
        {
            var user = HttpContext.Session.GetString("Username");
            var id = HttpContext.Session.GetString("Id");
            var token = HttpContext.Session.GetString("Token");
            var permission = HttpContext.Session.GetString("Permission");
            try
            {
                var Ticket = _isSv.GetIssueById(Model.Issue.Id);
                Ticket.Data.StatusId = Model.StatusTicketId;
                Ticket.Data.Name = Model.Issue.Name;
                var rs = _isSv.UpdateTicket(Ticket.Data);

                if(rs.IsSuccess == true)
                {
                    TempData["SuccessMessage"] = "Update Ticket Successfully";
                    return RedirectToAction("Detail",new { TicketId = rs.Data.Id});
                }
                else
                {
                    TempData["ErrorMessage"] = rs.Message;
                    return RedirectToAction("Index", "Home");
                }
              
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index", "Home");
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
            var returnThis = _acSv.GetUserPermissionListsByGroupId(groupId).Data;
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
            var returnThis = _stSv.GetListsStatus(type).Data;
            return Json(returnThis);
        }
        #endregion
    }
}
