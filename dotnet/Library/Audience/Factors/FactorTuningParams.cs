namespace DotnetLibrary.Audience.Factors;

public record FactorTuningParams
{
    /// <summary>
    /// how rapidly the directional vect on random walks changes
    /// </summary>
    public float RandomWalkJitter = 2;

    /// <summary>
    /// how many units of push force are required to produce a single unit of rage factor
    /// </summary>
    public float PushToRageRatio = 400;
}