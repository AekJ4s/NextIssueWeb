using System;
using System.Collections.Generic;

namespace NextIssueWeb.Models;

public partial class SystemOnDate
{
    public int Id { get; set; }

    public int SystemDaydeleted { get; set; }

    public string SystemNameEn { get; set; } = null!;

    public string SystemNameTh { get; set; } = null!;

    public bool SystemStatus { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int UpdateBy { get; set; }

    public virtual ICollection<Nlogger> Nloggers { get; set; } = new List<Nlogger>();
}
