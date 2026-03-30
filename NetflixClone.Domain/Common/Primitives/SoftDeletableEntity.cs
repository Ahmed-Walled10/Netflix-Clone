namespace NetflixClone.Domain.Common.Primitives
{
    public abstract class SoftDeletableEntity : AuditableEntity, ISoftDeletable
    {
        public bool IsDeleted { get; private set; } = false;
        public DateTime? DeletedAt { get; private set; }

        public void SoftDelete()
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
        }

        public void Restore()
        {
            IsDeleted = false;
            DeletedAt = null;
        }
    }
}
