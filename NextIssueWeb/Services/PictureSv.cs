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
                Npicture npicture = new Npicture()
                {
                    Picture = record.Picture,
                    UploadDate = DateTime.Now,
                };
                _db.Npictures.Add(npicture);
                _db.SaveChanges();
                rs.Data = npicture;
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

        public ResponseModel<MergeissuePicture> MergeIssueWithPicture(MergeissuePicture record)
        {
            var rs = new ResponseModel<MergeissuePicture>();
            try
            {
                _db.MergeissuePictures.Add(record);
                _db.SaveChanges();
                rs.Data = record;
                rs.IsSuccess = true;
                rs.Message = "Merge Picture Successfully";
                rs.Code = 200;
            }
            catch (Exception ex)
            {
                rs.Data = null;
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