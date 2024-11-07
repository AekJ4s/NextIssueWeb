using System;
using System.Collections.Generic;

namespace NextIssueWeb.Models;

public partial class Nlogger
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Controller { get; set; }

    public string? Detail { get; set; }

    public int? SystemId { get; set; }

    public DateTime? CreateDate { get; set; }

    public int? LogBy { get; set; }

    public virtual SystemOnDate? System { get; set; }
}
