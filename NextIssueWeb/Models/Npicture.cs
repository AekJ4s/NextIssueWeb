using System;
using System.Collections.Generic;

namespace NextIssueWeb.Models;

public partial class Npicture
{
    public int Id { get; set; }

    public byte[] Picture { get; set; } = null!;

    public DateTime UploadDate { get; set; }

    public int TicketId { get; set; }

    public virtual Nticket Ticket { get; set; } = null!;
}
