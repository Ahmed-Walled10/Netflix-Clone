using NetflixClone.Domain.Common.Enums;

namespace NetflixClone.Application.ResourceParameters
{
    public class CatalogResourceParameters : BaseResourceParameters
    {

        public List<Guid>? GenreIds { get; set; }
        public List<ContentType>? ContentTypes { get; set; }

        public decimal? MinRating { get; set; }
        public List<MaturityRating>? MaturityRatings { get; set; }

        public PersonRole? PersonRole { get; set; }

        public List<string>? Languages { get; set; }
        public int? ReleaseYear { get; set; }
        public bool? IsOriginal { get; set; }

        public bool? OrderedByRatingDesending { get; set; }

    }
}
