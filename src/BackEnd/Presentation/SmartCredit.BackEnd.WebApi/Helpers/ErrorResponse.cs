using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCredit.BackEnd.WebApi.Helpers
{
    public class ErrorResponse
{
    public bool Success { get; } = false;
    public int StatusCode { get; }
    public List<string> Errors { get; private set; } = new List<string>();
    public ErrorResponse(int statuscode, List<string> errors)
    {
        StatusCode = statuscode;
        Errors = errors;
    }

    public ErrorResponse(int statuscode, string error)
    {
        StatusCode = statuscode;
        Errors.Add(error);
    }
}
}