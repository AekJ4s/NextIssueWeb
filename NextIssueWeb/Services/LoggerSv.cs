using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NextIssueWeb.Data;
using NextIssueWeb.Models;
using System.Linq;

namespace NextIssueWeb.Services
{
    public class LoggerSv : Controller
    {
        public NextIssueContext _db;

        public string _config;
        private string encrypword = "NextIssueSystemForAuthenticationKeyAndSendBackToMe";
        readonly string Services = "LoggerSv";
        public LoggerSv(NextIssueContext context)
        {
            _db = context;
        }
        public LoggerSv(string config)
        {
            _config = config;
        }
        #region Create
        public ResponseModel<Nlogger> CreateLog(Nlogger logger)
        {
            var rs = new ResponseModel<Nlogger>();
            try
            {
                _db.Nloggers.Add(logger);
                _db.SaveChanges();
                var systemDeleteLog = _db.SystemOnDates.Where(db => db.Id == 1).FirstOrDefault();
                if (systemDeleteLog != null && systemDeleteLog.SystemStatus == true)
                {
                    DeleteLog(systemDeleteLog.SystemDaydeleted);
                }
                rs.Data = FindLogById(logger.Id).Data;
                rs.IsSuccess = true;
                rs.Message = "Log Create Successfully";
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
        public ResponseModel<Nlogger> FindLogById(int Id)
        {
            var rs = new ResponseModel<Nlogger>();
            try
            {
                rs.Data = _db.Nloggers.Where(db => db.Id == Id).FirstOrDefault();
                rs.IsSuccess = true;
                rs.Message = "Find Log Successfully";
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
        public ResponseModel<List<Nlogger>> LogLists()
        {
            var rs = new ResponseModel<List<Nlogger>>();
            try
            {
                rs.Data = _db.Nloggers.ToList();
                rs.IsSuccess = true;
                rs.Message = "Get All Log Successfully";
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
        public bool DeleteLog(int day)
        {
            try
            {
                // คำนวณวันที่ที่จะลบ
                var datetimeToDelete = DateTime.Now.Date - TimeSpan.FromDays(day);

                // ค้นหาและลบข้อมูล log ที่เก่ากว่าจำนวนวันที่กำหนด
                var logsToDelete = _db.Nloggers.Where(log => log.CreateDate < datetimeToDelete).ToList();

                if (logsToDelete.Any())
                {
                    _db.Nloggers.RemoveRange(logsToDelete);
                    _db.SaveChanges(); // บันทึกการเปลี่ยนแปลงในฐานข้อมูล
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteLogWithParam(int day, int idSystem)
        {
            try
            {
                // คำนวณวันที่ที่จะลบ
                var datetimeToDelete = DateTime.Now.Date - TimeSpan.FromDays(day);

                // ค้นหาและลบข้อมูล log ที่เก่ากว่าจำนวนวันที่กำหนด
                var logsToDelete = _db.Nloggers.Where(log => log.CreateDate < datetimeToDelete && log.SystemId == idSystem).ToList();

                if (logsToDelete.Any())
                {
                    _db.Nloggers.RemoveRange(logsToDelete);
                    _db.SaveChanges(); // บันทึกการเปลี่ยนแปลงในฐานข้อมูล
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

    }
}