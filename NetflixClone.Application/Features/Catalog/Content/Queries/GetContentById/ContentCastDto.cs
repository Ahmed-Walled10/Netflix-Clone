using NetflixClone.Domain.Common.Enums;
using System;

namespace NetflixClone.Application.Features.Catalog.Content.Queries.GetContentById
{
    public class ContentCastDto
    {
        public Guid PersonId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
        public PersonRole Role { get; set; }
        public string? CharacterName { get; set; }
    }
}
