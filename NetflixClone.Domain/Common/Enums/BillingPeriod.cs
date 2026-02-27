using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetflixClone.Domain.Common.Enums
{
    public enum BillingPeriod
    {
        /// <summary>Charged every month.</summary>
        Monthly = 1,

        /// <summary>Charged once per year (usually at a discount vs monthly).</summary>
        Yearly = 2
    }
}
