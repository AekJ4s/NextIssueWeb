using System;
using System.Collections.Generic;

namespace NextIssueWeb.Models;

public partial class MergeissuePicture
{
    public int Id { get; set; }

    public int? IssueId { get; set; }

    public int? PictureId { get; set; }
}
