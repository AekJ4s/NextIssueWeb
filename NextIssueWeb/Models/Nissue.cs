using System;
using System.Collections.Generic;

namespace NextIssueWeb.Models;

public partial class Nissue
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public Guid CreateBy { get; set; }

    public DateTime UpdateDate { get; set; }

    public Guid UpdateBy { get; set; }

    public int ImportantId { get; set; }

    public Guid InformerId { get; set; }

    public Guid ResponsibleId { get; set; }

    public virtual Nimportant Important { get; set; } = null!;
}
