using System;
using System.Collections.Generic;

namespace NextIssueWeb.Models;

public partial class MergeprojectIssue
{
    public int Id { get; set; }

    public int IssueId { get; set; }

    public Guid ProjectId { get; set; }
}
