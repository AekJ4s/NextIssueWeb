using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NextIssueWeb.Data;
using NextIssueWeb.Models;
using System.Linq;

namespace NextIssueWeb.Services
{
    public class PositionSv : Controller
    {
        public NextIssueContext _db;

        public string _config;
        private string encrypword = "NextIssueSystemForAuthenticationKeyAndSendBackToMe";
        readonly string Services = "LoggerSv";
        public PositionSv(NextIssueContext context)
        {
            _db = context;
        }
        public PositionSv(string config)
        {
            _config = config;
        }
        #region Create
        public ResponseModel<Nposition> CreatePosition(Nposition record)
        {
            var rs = new ResponseModel<Nposition>();
            try
            {
                Nposition nposition = new Nposition()
                {
                    Name = record.Name,
                    CreateBy = record.CreateBy,
                    UpdateBy = record.UpdateBy,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                };
                _db.Npositions.Add(nposition);
                _db.SaveChanges();
                rs.IsSuccess = true;
                rs.Message = "Position Create Successfully";
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
        public ResponseModel<Nposition> GetPositionById(int id)
        {
            var rs = new ResponseModel<Nposition>();
            try
            {
                rs.Data = _db.Npositions.Where(db => db.Id == id).FirstOrDefault();
                rs.Message = "Get Postion Successfully";
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
        public ResponseModel<List<Nposition>> GetPositionLists()
        {
            var rs = new ResponseModel<List<Nposition>>();
            try
            {
                rs.Data = _db.Npositions.ToList();
                rs.Message = "Get Postion Lists Successfully";
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
        public ResponseModel<Nposition> DeletePositionById(int id)
        {
            var rs = new ResponseModel<Nposition>();
            try
            {
                var record = _db.Npositions.Where(db => db.Id == id).FirstOrDefault();
                _db.Npositions.Remove(record);
                rs.Data = null;
                rs.Message = "Delete Postion Successfully";
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

    }
}