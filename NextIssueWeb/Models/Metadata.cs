using System;
using System.Collections.Generic;

namespace NextIssueWeb.Models;

public partial class Metadata
{

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

    public partial class NstatusCreate
    {
        public string Name { get; set; } = null!;
        public int TaskUse { get; set; }
        public int UserId { get; set; } = 0;
    }

    public partial class NissueCreate
    {
        public string Name { get; set; } = null!;
        public int ImportantId { get; set; }
        public int InformerId { get; set; }
        public int ResponsibleId { get; set; }
        public int StatusId { get; set; }
        public List<Nstatus> StatusLists { get; set; } = new List<Nstatus>();
        public Guid ProjectId { get; set; }
    }

}
