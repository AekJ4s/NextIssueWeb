﻿using System;
using System.Collections.Generic;

namespace NextIssueWeb.Models;

public partial class Npicture
{
    public int Id { get; set; }

    public byte[]? Picture { get; set; }

    public DateTime? UploadDate { get; set; }
}
