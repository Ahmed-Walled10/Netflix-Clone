
namespace NetflixClone.Domain.Common.Primitives
{
    public abstract class BaseEntity
    {
        public Guid Id { get; init; } = Guid.NewGuid();
    }
}
