using System;
using System.Collections.Generic;

namespace NextIssueWeb.Models;

public partial class Metadata
{

    public partial class InterfaceHome
    {
        public int TicketCount { get; set; } = 0;
        public int BugCount { get; set; } = 0;
        public int TicketCloseCount { get; set; } = 0;
        public int OpenTicketCount { get; set; } = 0;
        public int CloseTicketCount { get; set; } = 0;
        public string Version { get; set; } = " ? ";
        public List<NprojectIssueCount> projectLists { get; set; } = new List<NprojectIssueCount>();


    }
    public partial class NuserLogin
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public partial class NuserCreate
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Aka { get; set; }
        public required string NameContent { get; set; }
        public string Details { get; set; }
        public string Controller { get; set; } = "?";
        public int UserId { get; set; } = 0;
    }
    public partial class NuserUpdate
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Aka { get; set; }
        public string Controller { get; set; } = "?";
        public int UserId { get; set; } = 0;

    }
    public partial class NuserChangPassword
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string Controller { get; set; } = "?";
        public int UserId { get; set; } = 0;
    }
    public partial class NloggerCreate
    {
        public required string Name { get; set; }
        public string? Detail { get; set; }
        public required int Loguser { get; set; }
        public required string Controller { get; set; }
        public DateTime CreateDate { get; set; }
        public required int System_id { get; set; }
    }
    public partial class NprojectCreate
    {
        public string Name { get; set; } = null!;
        public int Status { get; set; }
        public List<Nstatus> StatusLists { get; set; } = new List<Nstatus>();
        public DateTime CreateDate { get; set; }
        public int UserId { get; set; } = 0;

    }
    public partial class NprojectIssueCount
    {
        public string ProjectName { get; set; } = null!;
        public int Status { get; set; } = 0;
        public int IssueCount { get; set; } = 0;
        public int UserCreating { get; set; } = 0;

    }
    public partial class NstatusCreate
    {
        public string Name { get; set; } = null!;
        public int TaskUse { get; set; }
        public int UserId { get; set; } = 0;
    }
    public partial class NissueLists 
    {
        public string IssueName { get; set; } = "Not Found Data";
        public int IssueId { get; set; } = 0;
        public int IssueStatus { get; set; } = 0;
        public string IssueStatusName { get; set; } = "Not Found Data";

        public string ResponsibleName = "Not Found Data";
        public int ResponsibleId { get; set; } = 0;
        public string ResponsibleGroupName = "Not Found Data";
        public int ResponsibleGroupId { get; set; } = 0;
        public string InformerName { get; set; } = "Not Found Data"; 
        public int InformerId { get; set; } = 0;
        public string ProjectName { get; set; } = "Not Found Data";
        public Guid ProjectId { get; set; } = Guid.Empty;
        public DateTime? ProjectCreateDate { get; set; }
        public DateTime? IssueCreateDate { get; set; }

    }
    public partial class NissueCreate
    {
        public string Name { get; set; } = null!;
        public int ImportantId { get; set; }
        public int InformerId { get; set; }
        public int ResponsibleId { get; set; }
        public int ResponsibleGroupId { get; set; }
        public int StatusId { get; set; }
        public int ImportanceId { get; set; }
        public Guid ProjectId { get; set; }
        public List<Nstatus>? StatusLists { get; set; } = new List<Nstatus>();
        public List<Nposition>? PositionLists { get; set; } = new List<Nposition>();
        public List<NuserWithPermission>? UserLists { get; set; } = new List<NuserWithPermission>();
        public List<Nimportant>? ImportanceLists { get; set; } = new List<Nimportant>();
        public List<Nproject>? ProjectLists { get; set; } = new List<Nproject>();
        public bool Checkbox1 { get; set; } = false;
        public bool Checkbox2 { get; set; } = false;
        public List<IFormFile> Picture { get; set; } = new List<IFormFile>();
    }
    public partial class NuserWithPermission
    {
        public int Id { get; set; }
        public string? Aka { get; set; }

        public string? Username { get; set; }

        public string? Permission { get; set; }
        public int? PermissionId { get; set; }
    }
    public partial class NpositionCreate
    {
        public string? Name { get; set; }
        public int? CreateBy { get; set; }
        public int? UpdateBy { get; set; }
    }
    public partial class TicketWithProjectAndPicture
    {
        public Nissue Issue { get; set; } = new Nissue { };
        public string IssueStatusName { get; set; } = " ? ";
        public string ProjectStatusName { get; set; } = " ? ";
        public Nproject Project { get; set; } = new Nproject { };
        public List<Npicture> Pictures { get; set; } = new List<Npicture> { };
        public List<string> Files { get; set; } = new List<string>();
        public List<Nstatus> StatusProjectLst = new List<Nstatus>();
        public List<Nstatus> StatusTicketLst = new List<Nstatus>();
        public int StatusProjectId = 0;
        public int StatusTicketId = 0;
        public List<Nproject> ProjectLst = new List<Nproject>();
        public Guid ProjectId = Guid.Empty;

    }

}
