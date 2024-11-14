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
        public ResponseModel<Nstatus> CreateStatus(Nstatus record)
        {
            var rs = new ResponseModel<Nstatus>();
            try
            {
                Nstatus nstatus = new Nstatus()
                {
                    Name = record.Name,
                    TaskUse = record.TaskUse,
                    CreateBy = record.CreateBy,
                    UpdateBy = record.UpdateBy,
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
        public ResponseModel<List<Nstatus>> GetListsStatusByTaskId(int taskId)
        {
            var rs = new ResponseModel<List<Nstatus>>();
            try
            {
                rs.Data = _db.Nstatuses.Where(db => db.TaskUse == taskId).ToList();
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
        public ResponseModel<Nstatus> GetStatusById(int taskId)
        {
            var rs = new ResponseModel<Nstatus>();
            try
            {
                rs.Data = _db.Nstatuses.Where(db => db.Id == taskId).FirstOrDefault();
                rs.Code = 200;
                rs.Message = "Get Status Successfully";
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
        public ResponseModel<bool> DeleteStatusById(int id)
        {
            var rs = new ResponseModel<bool>();
            try
            {
                var status = _db.Nstatuses.FirstOrDefault(db => db.Id == id);
                if (status != null)
                {
                    _db.Nstatuses.Remove(status);
                    _db.SaveChanges();
                    rs.IsSuccess = true;
                    rs.Data = true;
                    rs.Message = "Status Deleted Successfully";
                    rs.Code = 200;
                }
                else
                {
                    rs.IsSuccess = false;
                    rs.Data = false;
                    rs.Message = "Status not found";
                    rs.Code = 404;
                }
            }
            catch (Exception ex)
            {
                rs.IsSuccess = false;
                rs.Data = false;
                rs.Message = ex.Message;
                rs.Code = 500;
            }
            return rs;
        }
        #endregion


    }
}