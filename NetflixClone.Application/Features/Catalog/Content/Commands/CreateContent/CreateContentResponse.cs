using NetflixClone.Domain.Common.Enums;

namespace NetflixClone.Application.Features.Content.Commands.CreateContent;

public class CreateContentResponse
{
    /// <summary>The generated primary key of the newly created content.</summary>
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    /// <summary>The URL-safe slug (auto-generated if not supplied in the request).</summary>
    public string Slug { get; set; } = string.Empty;

    public ContentType ContentType { get; set; }
}

