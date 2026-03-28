namespace NetflixClone.Application.Features.Profiles.Queries.GetProfiles;

public class GetProfilesResponse
{
    public Guid   Id                { get; set; }
    public string Name              { get; set; } = string.Empty;
    public string? AvatarUrl        { get; set; }
    public int    Age               { get; set; }
    public bool   IsKidsMode        { get; set; }
    public string PreferredLanguage { get; set; } = "en";
    public bool   HasPin            { get; set; }
}
