namespace Zoo.Domain.ValueObjects;

public readonly record struct AnimalId(Guid Value)
{
    public static AnimalId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}

public readonly record struct EnclosureId(Guid Value)
{
    public static EnclosureId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}

public readonly record struct FeedingId(Guid Value)
{
    public static FeedingId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}