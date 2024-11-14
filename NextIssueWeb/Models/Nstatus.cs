using System;
using System.Collections.Generic;

namespace NextIssueWeb.Models;

public partial class Nstatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? CreateDate { get; set; }

    public int CreateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int UpdateBy { get; set; }

    public int TaskUse { get; set; }

    public virtual ICollection<Nticket> Ntickets { get; set; } = new List<Nticket>();
}
