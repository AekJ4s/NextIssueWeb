using System;
using System.Collections.Generic;

namespace NextIssueWeb.Models;

public partial class Nuser
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Aka { get; set; } = null!;

    public Guid CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public Guid UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }
}
