using System;
using System.Collections.Generic;

namespace NextIssueWeb.Models;

public partial class Nissue
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int ImportantId { get; set; }

    public int InformerId { get; set; }

    public int ResponsibleId { get; set; }

    public int ResponsibleGroupId { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int CreateBy { get; set; }

    public int UpdateBy { get; set; }

    public int StatusId { get; set; }

    public virtual Nimportant Important { get; set; } = null!;

    public virtual ICollection<MergeprojectIssue> MergeprojectIssues { get; set; } = new List<MergeprojectIssue>();
}
