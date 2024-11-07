using System;
using System.Collections.Generic;

namespace NextIssueWeb.Models;

public partial class MergeissuePicture
{
    public Guid Id { get; set; }

    public Guid IssueId { get; set; }

    public Guid PictureId { get; set; }
}
