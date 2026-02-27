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
        G = 1,

        /// <summary>Parental Guidance Suggested. Some material may not suit young children. MinAge: 0 (parental guidance)</summary>
        PG = 2,

        /// <summary>Parents Strongly Cautioned. Some material may be inappropriate for children under 13.</summary>
        PG13 = 3,

        /// <summary>Restricted. Under 17 requires accompanying parent or guardian. MinAge: 17</summary>
        R = 4,

        /// <summary>Adults Only. No one under 18 admitted. MinAge: 18</summary>
        NC17 = 5,

        // ── TV / streaming ratings ───────────────────────────────────────

        /// <summary>TV-Y: Designed to be appropriate for all children.</summary>
        TV_Y = 10,

        /// <summary>TV-Y7: Suitable for children age 7 and older.</summary>
        TV_Y7 = 11,

        /// <summary>TV-G: General audience. Suitable for all ages.</summary>
        TV_G = 12,

        /// <summary>TV-PG: Parental Guidance Suggested.</summary>
        TV_PG = 13,

        /// <summary>TV-14: Parents Strongly Cautioned. Suitable for ages 14+.</summary>
        TV_14 = 14,

        /// <summary>TV-MA: Mature Audience Only. Suitable for ages 17+.</summary>
        TV_MA = 15
    }
}
