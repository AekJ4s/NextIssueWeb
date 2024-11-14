using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NextIssueWeb.Data;
using NextIssueWeb.Models;
using System.Linq;

namespace NextIssueWeb.Services
{
    public class PictureSv : Controller
    {
        public NextIssueContext _db;

        public string _config;
        private string encrypword = "NextIssueSystemForAuthenticationKeyAndSendBackToMe";
        readonly string Services = "PictureSv";
        public PictureSv(NextIssueContext context)
        {
            _db = context;
        }
        public PictureSv(string config)
        {
            _config = config;
        }

        #region Create
        public ResponseModel<Npicture> CreatePicture(Npicture record)
        {
            var rs = new ResponseModel<Npicture>();
            try
            {
                _db.Npictures.Add(record);
                _db.SaveChanges();
                rs.Data = record;
                rs.IsSuccess = true;
                rs.Message = "Picture Create Successfully";
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

        public ResponseModel<List<Npicture>> GetPictureListByTicketId(int ticketId)
        {
            var rs = new ResponseModel<List<Npicture>>();
            try
            {
                rs.Data = _db.Npictures.Where(x => x.TicketId == ticketId).ToList();
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

        #region Delete

        #endregion

    }
}