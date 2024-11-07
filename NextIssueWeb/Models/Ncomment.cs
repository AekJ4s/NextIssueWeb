using System;
using System.Collections.Generic;

namespace NextIssueWeb.Models;

public partial class Ncomment
{
    public int Id { get; set; }

    public string Comment { get; set; } = null!;

    public int CreateBy { get; set; }

    public int IssueId { get; set; }
}
