namespace NetflixClone.Application.Features.Catalog.Person.Commands.UpdatePerson
{
    public class UpdatePersonRequest
    {
        public string? FullName { get; set; }
        public string? Slug { get; set; }
        public string? Bio { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
