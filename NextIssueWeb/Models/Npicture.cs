using System;
using System.Collections.Generic;

namespace NextIssueWeb.Models;

public partial class Npicture
{
    public Guid Id { get; set; }

    public byte[] Picture { get; set; } = null!;

    public DateTime UploadDate { get; set; }

    public Guid UploadBy { get; set; }
}
