using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NextIssueWeb.Data;
using NextIssueWeb.Models;
using System.Linq;

namespace NextIssueWeb.Services
{
    public class StatusSv : Controller
    {
        public NextIssueContext _db;

        public string _config;
        private string encrypword = "NextIssueSystemForAuthenticationKeyAndSendBackToMe";
        readonly string Services = "LoggerSv";
        public StatusSv(NextIssueContext context)
        {
            _db = context;
        }
        public StatusSv(string config)
        {
            _config = config;
        }
        #region Create
        public ResponseModel<Nstatus> CreateProject(Metadata.NstatusCreate record)
        {
            var rs = new ResponseModel<Nstatus>();
            try
            {
                Nstatus nstatus = new Nstatus()
                {
                    Name = record.Name,
                    TaskUse = record.TaskUse,
                    CreateBy = record.UserId,
                    UpdateBy = record.UserId,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                };
                _db.Nstatuses.Add(nstatus);
                _db.SaveChanges();
                rs.IsSuccess = true;
                rs.Message = "Status Create Successfully";
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
        public ResponseModel<List<Nstatus>> GetListsStatus(int taskId)
        {
            var rs = new ResponseModel<List<Nstatus>>();
            try
            {
                rs.Data = _db.Nstatuses.Where(db => db.TaskUse == taskId).ToList();
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