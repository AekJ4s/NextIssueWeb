using System;
using System.Collections.Generic;

namespace NextIssueWeb.Models;

public partial class Nposition
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int? CreateBy { get; set; }

    public int? UpdateBy { get; set; }

    public virtual ICollection<Nticket> Ntickets { get; set; } = new List<Nticket>();

    public virtual ICollection<Nuser> Nusers { get; set; } = new List<Nuser>();
}
