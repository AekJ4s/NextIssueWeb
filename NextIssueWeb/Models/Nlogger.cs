using System;
using System.Collections.Generic;

namespace NextIssueWeb.Models;

public partial class Nlogger
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Detail { get; set; } = null!;

    public Guid Loguser { get; set; }

    public string Controller { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public string SystemId { get; set; } = null!;

    public virtual SystemOnDate System { get; set; } = null!;
}
