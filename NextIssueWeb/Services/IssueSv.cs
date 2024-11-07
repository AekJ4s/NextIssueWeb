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
                    ResponsibleId = record.ResponsibleId,
                    StatusId = record.StatusId,
                    CreateBy = record.InformerId,
                    UpdateBy = record.InformerId,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                };
                _db.Nissues.Add(nissue);

                MergeprojectIssue projectWithIssue = new MergeprojectIssue()
                {
                    IssueId = nissue.Id,
                    ProjectId = record.ProjectId
                };
                _db.MergeprojectIssues.Add(projectWithIssue);

                rs.IsSuccess = true;
                rs.Message = "Project Create Successfully";
                rs.Code = 200;
                _db.SaveChanges();
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
        #endregion

        #region Delete

        #endregion

    }
}