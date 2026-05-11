namespace AppCore.Domains.Entities.Base;

public abstract class BaseEntityWithDeleteOnly : BaseEntity
{
    public bool IsDeleted { get; private set; }

    public DateTime? DeletedAtUtc { get; private set; }
    public long? DeletedBy { get; private set; }

    public void Delete(long userId)
    {
        if (IsDeleted) return;

        IsDeleted = true;
        DeletedAtUtc = DateTime.UtcNow;
        DeletedBy = userId;
    }
}
