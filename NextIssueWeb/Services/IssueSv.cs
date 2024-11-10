using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NextIssueWeb.Data;
using NextIssueWeb.Models;
using System.Linq;

namespace NextIssueWeb.Services
{
    public class IssueSv : Controller
    {
        public NextIssueContext _db;

        public string _config;
        private string encrypword = "NextIssueSystemForAuthenticationKeyAndSendBackToMe";
        readonly string Services = "LoggerSv";
        public IssueSv(NextIssueContext context)
        {
            _db = context;
        }
        public IssueSv(string config)
        {
            _config = config;
        }
        #region Create
        public ResponseModel<Nissue> CreateIssue(Metadata.NissueCreate record)
        {
            var rs = new ResponseModel<Nissue>();
            try
            {
                Nissue nissue = new Nissue()
                {
                    Name = record.Name,
                    InformerId = record.InformerId,
                    ResponsibleGroupId = record.ResponsibleGroupId,
                    ResponsibleId = record.ResponsibleId,
                    ImportantId = record.ImportantId,
                    StatusId = record.StatusId,
                    CreateBy = record.InformerId,
                    UpdateBy = record.InformerId,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                };
                _db.Nissues.Add(nissue);
                _db.SaveChanges();

                var project = _db.Nprojects.Where(db=>db.Id == record.ProjectId).FirstOrDefault();
                MergeprojectIssue projectWithIssue = new MergeprojectIssue()
                {
                    IssueId = nissue.Id,
                    ProjectId = project.Id
                };
                _db.MergeprojectIssues.Add(projectWithIssue);
                _db.SaveChanges();

                rs.Data = nissue;
                rs.IsSuccess = true;
                rs.Message = "Project Create Successfully";
                rs.Code = 200;
            }
            catch (Exception ex)
            {
                rs.IsSuccess = false;
                rs.Message = ex.Message;
                rs.Code = 500;
            }
            return rs;
        }
        #endregion

        #region Get
        public ResponseModel<Nissue> GetIssueById(int id)
        {
            var rs = new ResponseModel<Nissue>();
            try
            {
                rs.Data = _db.Nissues.Where(db=>db.Id == id).FirstOrDefault();
                rs.IsSuccess = true;
                rs.Message = "Get Issue Successfully";
                rs.Code = 200;
            }
            catch (Exception ex)
            {
                rs.IsSuccess = false;
                rs.Message = ex.Message;
                rs.Code = 500;
            }
            return rs;
        }
        public ResponseModel<Nissue> GetIssueByIdAndAboutMe(int id,Metadata.NuserWithPermission user)
        {
            var rs = new ResponseModel<Nissue>();
            try
            {
                rs.Data = _db.Nissues
                    .Where(db => db.Id == id && 
                    (db.ResponsibleId == user.Id 
                    || db.ResponsibleGroupId 
                    == user.PermissionId ))
                    .FirstOrDefault();
                rs.IsSuccess = true;
                rs.Message = "Get Issue Successfully";
                rs.Code = 200;
            }
            catch (Exception ex)
            {
                rs.IsSuccess = false;
                rs.Message = ex.Message;
                rs.Code = 500;
            }
            return rs;
        }
        public ResponseModel<List<Nissue>> GetIssueLists()
        {
            var rs = new ResponseModel<List<Nissue>>();
            try
            {
                rs.Data = _db.Nissues.ToList();
                rs.IsSuccess = true;
                rs.Message = "Get Issue Successfully";
                rs.Code = 200;
            }
            catch (Exception ex)
            {
                rs.IsSuccess = false;
                rs.Message = ex.Message;
                rs.Code = 500;
            }
            return rs;
        }
        public ResponseModel<List<MergeprojectIssue>> GetIssueListsByProjectId(Guid id)
        {
            var rs = new ResponseModel<List<MergeprojectIssue>>();
            try
            {
                rs.Data = _db.MergeprojectIssues.Where(db=>db.ProjectId == id).ToList();
                rs.IsSuccess = true;
                rs.Message = "Get Issue Successfully";
                rs.Code = 200;
            }
            catch (Exception ex)
            {
                rs.IsSuccess = false;
                rs.Message = ex.Message;
                rs.Code = 500;
            }
            return rs;
        }
        public ResponseModel<List<MergeprojectIssue>> GetMergeProjectWithIssueLists()
        {
            var rs = new ResponseModel<List<MergeprojectIssue>>();
            try
            {
                rs.Data = _db.MergeprojectIssues.ToList();
                rs.IsSuccess = true;
                rs.Message = "Get Issue Successfully";
                rs.Code = 200;
            }
            catch (Exception ex)
            {
                rs.IsSuccess = false;
                rs.Message = ex.Message;
                rs.Code = 500;
            }
            return rs;
        }
        public ResponseModel<Nproject> GetProjectByTicket(int TicketId)
        {
            var rs = new ResponseModel<Nproject>();
            try
            {
                var findMerge = _db.MergeprojectIssues.Where(db => db.IssueId == TicketId).FirstOrDefault();
                rs.Data = _db.Nprojects.Where(db => db.Id == findMerge.ProjectId).FirstOrDefault();
                rs.IsSuccess = true;
                rs.Message = "Get Issue Successfully";
                rs.Code = 200;
            }
            catch (Exception ex)
            {
                rs.IsSuccess = false;
                rs.Message = ex.Message;
                rs.Code = 500;
            }
            return rs;
        }
        public ResponseModel<List<Npicture>> GetPictureListsByTicketId(int TicketId)
        {
            var rs = new ResponseModel<List<Npicture>>();
            try
            {
                var PictureLst = new List<Npicture>();
                var findMerge = _db.MergeissuePictures.Where(db => db.IssueId == TicketId).ToList();
                foreach(var item in findMerge)
                {
                    var data = _db.Npictures.Where(db => db.Id == item.PictureId).FirstOrDefault();
                    PictureLst.Add(data);
                }
                rs.Data = PictureLst;
                rs.IsSuccess = true;
                rs.Message = "Get Picture Lists Successfully";
                rs.Code = 200;
            }
            catch (Exception ex)
            {
                rs.IsSuccess = false;
                rs.Message = ex.Message;
                rs.Code = 500;
            }
            return rs;
        }

        #endregion

        #region Update
        public ResponseModel<Nissue> UpdateTicket(Nissue Ticket)
        {
            var rs = new ResponseModel<Nissue>();
            try
            {
                _db.Update(Ticket);
                _db.SaveChanges();
                rs.Data = Ticket;
                rs.IsSuccess = true;
                rs.Message = "Get Issue Successfully";
                rs.Code = 200;
            }
            catch (Exception ex)
            {
                rs.IsSuccess = false;
                rs.Message = ex.Message;
                rs.Code = 500;
            }
            return rs;
        }

        #endregion
        #region Delete

        #endregion

    }
}