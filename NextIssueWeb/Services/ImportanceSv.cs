using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NextIssueWeb.Data;
using NextIssueWeb.Models;
using System.Linq;

namespace NextIssueWeb.Services
{
    public class ImportanceSv : Controller
    {
        public NextIssueContext _db;

        public string _config;
        private string encrypword = "NextIssueSystemForAuthenticationKeyAndSendBackToMe";
        readonly string Services = "LoggerSv";
        public ImportanceSv(NextIssueContext context)
        {
            _db = context;
        }
        public ImportanceSv(string config)
        {
            _config = config;
        }
        #region Create
        public ResponseModel<Nimportant> CreateImportance(Nimportant record)
        {
            var rs = new ResponseModel<Nimportant>();
            try
            {
                Nimportant nimportant = new Nimportant()
                {
                    Name = record.Name,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                };
                _db.Nimportants.Add(nimportant);
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
        public ResponseModel<List<Nimportant>> GetListsimportance()
        {
            var rs = new ResponseModel<List<Nimportant>>();
            try
            {
                rs.Data = _db.Nimportants.ToList();
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
        public ResponseModel<List<Nimportant>> GetListsimportanceById(int imId)
        {
            var rs = new ResponseModel<List<Nimportant>>();
            try
            {
                rs.Data = _db.Nimportants.Where(db => db.Id == imId).ToList();
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