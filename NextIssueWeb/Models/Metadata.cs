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
        public Guid guid { get; set; } = Guid.Empty;
    }

    public partial class NuserUpdate
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Aka { get; set; }
        public string Controller { get; set; } = "?";
        public Guid guid { get; set; } = Guid.Empty;

    }

    public partial class NuserChangPassword
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string Controller { get; set; } = "?";
        public Guid guid { get; set; } = Guid.Empty;
    }

    public partial class NloggerCreate
    {
        public required string Name { get; set; }
        public string? Detail { get; set; }
        public required Guid Loguser { get; set; }
        public required string Controller { get; set; }
        public DateTime CreateDate { get; set; }
        public required string Type { get; set; }
    }
}
