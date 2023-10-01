namespace DotnetLibrary.Audience;

public class CrowdActorFactory
{
    public static ICrowdActor GetActor(CrowdActorType type)
    {
        return type switch
        {
            CrowdActorType.Calm => new CalmCrowdActor(0.5f),
            CrowdActorType.Random => new RandomCrowdActor(),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
    
}

public enum CrowdActorType
{
    Calm,
    Random
}