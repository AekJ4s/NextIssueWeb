using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NextIssueWeb.Data;
using NextIssueWeb.Models;
using System.Linq;

namespace NextIssueWeb.Services
{
    public class ProjectSv : Controller
    {
        public NextIssueContext _db;

        public string _config;
        private string encrypword = "NextIssueSystemForAuthenticationKeyAndSendBackToMe";
        readonly string Services = "LoggerSv";
        public ProjectSv(NextIssueContext context)
        {
            _db = context;
        }
        public ProjectSv(string config)
        {
            _config = config;
        }
        #region Create
        public ResponseModel<Nproject> CreateProject(Metadata.NprojectCreate record)
        {
            var rs = new ResponseModel<Nproject>();
            try
            {
                Nproject nproject = new Nproject()
                {
                    Id = Guid.NewGuid(),
                    Name = record.Name,
                    Status = record.Status,
                    CreateBy = record.UserId,
                    UpdateBy = record.UserId,
                    CreateDate = record.CreateDate,
                    UpdateDate = DateTime.Now,
                };
                _db.Nprojects.Add(nproject);
                _db.SaveChanges();
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
        public ResponseModel<Nproject> GetProjectById(Guid id)
        {
            var rs = new ResponseModel<Nproject>();
            try
            {
                rs.Data = _db.Nprojects.Where(db=>db.Id == id).FirstOrDefault();
                rs.IsSuccess = true;
                rs.Message = "Get Project Successfully";
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

        public ResponseModel<List<Nproject>> GetProjectLists()
        {
            var rs = new ResponseModel<List<Nproject>>();
            try
            {
                rs.Data = _db.Nprojects.ToList();
                rs.IsSuccess = true;
                rs.Message = "Get Project Lists Successfully";
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
        public ResponseModel<List<Nproject>> GetProjectActiveLists()
        {
            var rs = new ResponseModel<List<Nproject>>();
            try
            {
                rs.Data = _db.Nprojects.Where(db=>db.Status == 1).ToList();
                rs.IsSuccess = true;
                rs.Message = "Get Project Lists Successfully";
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