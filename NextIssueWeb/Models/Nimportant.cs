using System;
using System.Collections.Generic;

namespace NextIssueWeb.Models;

public partial class Nimportant
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public Guid CreateBy { get; set; }

    public DateTime CreateDate { get; set; }

    public Guid UpdateBy { get; set; }

    public DateTime UpdateDate { get; set; }

    public virtual ICollection<Nissue> Nissues { get; set; } = new List<Nissue>();
}
