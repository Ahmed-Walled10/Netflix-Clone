using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetflixClone.Domain.Common.Primitives
{
    public abstract class BaseEntity
    {
        public Guid Id { get; init; } = Guid.NewGuid();
    }
}
