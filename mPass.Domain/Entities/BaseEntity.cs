using NodaTime;

namespace mPass.Persistence.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Instant CreatedAt { get; set; } = SystemClock.Instance.GetCurrentInstant();
    public Instant UpdatedAt { get; set; } = SystemClock.Instance.GetCurrentInstant();
}