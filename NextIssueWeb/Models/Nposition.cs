using System;
using System.Collections.Generic;

namespace NextIssueWeb.Models;

public partial class Nposition
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public Guid CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public Guid UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }
}
