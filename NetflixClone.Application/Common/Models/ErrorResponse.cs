using System.Collections.Generic;

namespace NetflixClone.Application.Common.Models
{
    public record ErrorResponse(
        int Status,
        string Title,
        string Detail,
        IDictionary<string, string[]>? Errors = null
    );
}
