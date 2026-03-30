namespace NetflixClone.Domain.Common.Enums
{
    public enum ContentType
    {
        /// <summary>A single stand-alone film. Has DurationMinutes. No Seasons/Episodes.</summary>
        Movie = 1,

        /// <summary>An episodic show. Has Seasons → Episodes. DurationMinutes is null on the Content itself.</summary>
        Series = 2,

        /// <summary>A documentary film or series. Follows the same Movie/Series split but tagged separately for browse filters.</summary>
        Documentary = 3,
    }
}
