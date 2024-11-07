using System;
using System.Collections.Generic;

namespace NextIssueWeb.Models;

public partial class SystemCompany
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? People { get; set; }

    public string? Year { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int? CreateBy { get; set; }

    public int? UpdateBy { get; set; }

    public int OwnerId { get; set; }

    public Guid LicenseId { get; set; }

    public virtual SystemLicense License { get; set; } = null!;

    public virtual Nuser Owner { get; set; } = null!;
}
