using System;
using System.Collections.Generic;

namespace NextIssueWeb.Models;

public partial class Nimportant
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual ICollection<Nticket> Ntickets { get; set; } = new List<Nticket>();
}
