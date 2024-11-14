using System;
using System.Collections.Generic;

namespace NextIssueWeb.Models;

public partial class Nticket
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int ImportantId { get; set; }

    public int InformerId { get; set; }

    public int? ResponsibleId { get; set; }

    public int ResponsibleGroupId { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime UpdateDate { get; set; }

    public int CreateBy { get; set; }

    public int UpdateBy { get; set; }

    public int StatusId { get; set; }

    public Guid ProjectId { get; set; }

    public DateTime? DeadlineDate { get; set; }

    public DateTime? CloseDate { get; set; }

    public virtual Nimportant Important { get; set; } = null!;

    public virtual Nuser Informer { get; set; } = null!;

    public virtual ICollection<Ncomment> Ncomments { get; set; } = new List<Ncomment>();

    public virtual ICollection<Npicture> Npictures { get; set; } = new List<Npicture>();

    public virtual Nproject Project { get; set; } = null!;

    public virtual Nuser? Responsible { get; set; }

    public virtual Nposition ResponsibleGroup { get; set; } = null!;

    public virtual Nstatus Status { get; set; } = null!;
}
