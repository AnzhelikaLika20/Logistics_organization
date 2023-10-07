using Microsoft.AspNetCore.Mvc;

namespace Workshop.Api.ActionFilters;

public class ResponseTypeAttribute : ProducesResponseTypeAttribute
{

    public ResponseTypeAttribute(int statusCode) : base(typeof(ErrorResponse), statusCode)
    {
    }
}