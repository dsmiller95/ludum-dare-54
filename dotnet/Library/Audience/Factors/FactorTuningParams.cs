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

    /// <summary>
    /// how rapidly the factors decay. A high value means ai will be more reactive to any change in their factors,
    /// low value will increase their memory.
    /// </summary>
    public float FactorDecayRate = 0.1f;

    /// <summary>
    /// average number of punches per second when at maximum rage factor.
    /// </summary>
    public float RagePunchChancePerSecond = 1f;

    public float RagePunchMinMagnitude { get; set; }
    public float RagePunchMaxMagnitude { get; set; }
    public float RagePunchDuration { get; set; }
    
    
    public float GlobalAttractionOffset { get; set; }
    public float MinAttractionForceMultiplier { get; set; }
    public float MaxAttractionForceMultiplier { get; set; }
    public float MaximumAttractiveForce { get; set; }
}