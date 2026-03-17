using NetflixClone.Domain.Common.Enums;
using NetflixClone.Domain.Entities.Catalog;
using NetflixClone.Domain.Entities.Engagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetflixClone.Application.ResourceParameters
{
    public class CatalogResourceParameters : BaseResourceParameters
    {
        // ── Text Search ──────────────────────────────────────────────────
        // Note: You can use inherited "SearchQuery" for general text search across title/description
        public string? CatalogName { get; set; }

        public List<Guid>? GenreIds { get; set; }
        public List<ContentType>? ContentTypes { get; set; }

        public decimal? MinRating { get; set; }
        public List<MaturityRating>? MaturityRatings { get; set; }

        public PersonRole? PersonRole { get; set; }

        public List<string>? Languages { get; set; }
        public int? ReleaseYear { get; set; }
        public bool? IsOriginal { get; set; }

    }
}
