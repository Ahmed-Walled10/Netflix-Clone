using NetflixClone.Domain.Common.Enums;
using System;

namespace NetflixClone.Application.Features.Catalog.Person.Queries.GetPersonById
{
    public class PersonWorkDto
    {
        public Guid ContentId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? ThumbnailUrl { get; set; }
        public decimal AverageRating { get; set; }
        public PersonRole Role { get; set; }
        public string? CharacterName { get; set; }
    }
}
