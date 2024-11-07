using System;
using System.Collections.Generic;

namespace NextIssueWeb.Models;

public partial class MergeuserPosition
{
    public int? Id { get; set; }

    public int? PositionId { get; set; }

    public int? UserId { get; set; }

    public virtual Nposition? Position { get; set; }

    public virtual Nuser? User { get; set; }
}
