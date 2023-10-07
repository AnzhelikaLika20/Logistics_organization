using System.Net;

namespace Workshop.Api.ActionFilters;

public record ErrorResponse(HttpStatusCode StatusCode, string Message)
{
    public override string ToString()
    {
        return $"{{ StatusCode = {StatusCode}, Message = {Message} }}";
    }
}