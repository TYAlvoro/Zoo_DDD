namespace Zoo.Domain.Common;

public abstract record DomainEvent(DateTime OccurredOn = default)
{
    public DateTime OccurredOn { get; init; } =
        OccurredOn == default ? DateTime.UtcNow : OccurredOn;
}