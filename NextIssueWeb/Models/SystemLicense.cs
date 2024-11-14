using System;
using System.Collections.Generic;

namespace NextIssueWeb.Models;

public partial class SystemLicense
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string License { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime? ActiveDate { get; set; }

    public DateTime? ExpireDate { get; set; }

    public virtual ICollection<SystemCompany> SystemCompanies { get; set; } = new List<SystemCompany>();
}
