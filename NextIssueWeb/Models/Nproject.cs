using System;
using System.Collections.Generic;

namespace NextIssueWeb.Models;

public partial class Nproject
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public int Status { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int CreateBy { get; set; }

    public int UpdateBy { get; set; }
}
