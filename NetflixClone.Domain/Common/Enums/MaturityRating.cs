using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetflixClone.Domain.Common.Enums
{
    public enum MaturityRating
    {
        /// <summary>General Audiences. No age restriction. Suitable for all ages.</summary>
        G = 0,

        /// <summary>TV-PG: Parental Guidance Suggested.</summary>
        TV_PG = 1,

        /// <summary>TV-Y7: Suitable for children age 7 and older.</summary>
        TV_Y7 = 7,

        /// <summary>Parents Strongly Cautioned. Some material may be inappropriate for children under 13.</summary>
        PG13 = 13,

        /// <summary>TV-14: Parents Strongly Cautioned. Suitable for ages 14+.</summary>
        TV_14 = 14,

        /// <summary>TV-MA: Mature Audience Only. Suitable for ages 17+.</summary>
        TV_MA = 17,

        /// <summary>Adults Only. No one under 18 admitted. MinAge: 18</summary>
        NC17 = 18,

    }
}
