using System;
using System.Collections.Generic;

namespace NextIssueWeb.Models;

public partial class MergeuserPosition
{
    public int Id { get; set; }

    public Guid UserId { get; set; }

    public string PositionId { get; set; } = null!;

    public virtual Nposition Position { get; set; } = null!;

    public virtual Nuser User { get; set; } = null!;
}
