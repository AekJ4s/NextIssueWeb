using System;
using System.Collections.Generic;

namespace NextIssueWeb.Models;

public partial class ViewModel
{
    #region Login

    #endregion
    #region ID
    public Guid projectId { get; set; } =Guid.Empty;
    public int ticketId { get; set; } = 0;
    public int userId { get; set; } = 0;
    public int statusId { get; set; } = 0;
    public int importaceId { get; set; } = 0;
    #endregion
    #region project
    public Nproject Nproject { get; set; }
    public Nticket Nticket { get; set; }
    public Npicture Npicture { get; set; }
    public Nuser Nuser { get; set; }
    
    public int projectCount { get; set; } = 0;
    public int projectOpenCount { get; set; } = 0;
    public int projectCloseCount { get; set; } = 0;
    public int projectSucessCount { get; set; } = 0;
    public int userCount { get; set; } = 0;
    public int statusCount { get; set; } = 0;
    public int importaceCount { get; set; } = 0;

    #endregion
    #region tickets
    public int ticketCount { get; set; } = 0;
    public int ticketCloseCount { get; set; } = 0;
    public int ticketOpenCount { get; set; } = 0;
    public int ticketSucessCount { get; set; } = 0;
    public int YourticketCount { get; set; } = 0;
    public int YourticketCloseCount { get; set; } = 0;
    public int YourticketOpenCount { get; set; } = 0;
    public int YourticketSucessCount { get; set; } = 0;
    public bool Checkbox1 { get; set; } = false;
    public bool Checkbox2 { get; set; } = false;

    #endregion
    #region Lists
    public List<Nimportant> importantsLists { get; set; }
    public List<Nuser> UserLists { get; set; }
    public List<Nposition> PositionList { get; set; }
    public List<Nticket> TicketsList { get; set; }
    public List<Nproject> projectsLists { get; set; }
    public List<Nstatus> statusprojectsLists { get; set; }
    public List<Nstatus> statusLists { get; set; }
    public List<ProjectWithTickets> ProjectWithTicketsLists { get; set; }
    public List<Npicture> NpictureLists { get; set; }
    public List<IFormFile> formFilesLists { get; set; }
    public List<byte[]> bytesLists { get; set; }
    public List<string> stringsLists { get; set; }
    #endregion
    #region extension
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public int CreateBy { get; set; }
    public int UpdateBy { get; set; }

    #endregion
}
public partial class ProjectWithTickets {
    public Guid ProjectId { get; set; }
    public string ProjectName { get; set; }
    public int ProjectStatus { get; set; }
    public int TicketsCount { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public int CreateBy { get; set; }
    public int UpdateBy { get; set; }
    public List<Nticket> TicketLists { get; set; } = new List<Nticket>();

}

public partial class LoginForm
{
    public string Username { get; set; }
    public string Password { get; set; }
}