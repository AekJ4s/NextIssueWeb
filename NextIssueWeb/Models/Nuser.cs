using System;
using System.Collections.Generic;

namespace NextIssueWeb.Models;

public partial class Nuser
{
    public int Id { get; set; }

    public string? Aka { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int? CreateBy { get; set; }

    public int? UpdateBy { get; set; }

    public int? PositionId { get; set; }

    public Guid? CompanyId { get; set; }

    public virtual ICollection<Nticket> NticketInformers { get; set; } = new List<Nticket>();

    public virtual ICollection<Nticket> NticketResponsibles { get; set; } = new List<Nticket>();

    public virtual Nposition? Position { get; set; }

    public virtual ICollection<SystemCompany> SystemCompanies { get; set; } = new List<SystemCompany>();
}
