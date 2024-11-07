using System;
using System.Collections.Generic;

namespace NextIssueWeb.Models;

public partial class ResponseModel<obj>
{
    public int Code { get; set; } = 000;
    public string Message { get; set; } = null;
    public obj? Data { get; set; }
    public bool IsSuccess { get; set; } = false;

}
