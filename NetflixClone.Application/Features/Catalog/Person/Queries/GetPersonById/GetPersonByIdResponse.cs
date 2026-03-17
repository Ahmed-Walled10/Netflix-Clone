using System;

namespace NetflixClone.Application.Features.Catalog.Person.Queries.GetPersonById
{
    public class GetPersonByIdResponse
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? PhotoUrl { get; set; }

        public List<PersonWorkDto> Work { get; set; } = new();
    }
}
