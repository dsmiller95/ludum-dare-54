using Godot;

namespace DotnetLibrary.Audience.Factors;

public interface IFactorAttractorStrategy
{
    float GetAttractionScale(Factors factors);
    float GetAttractionToNeighborFactors(Factors factors);
}

public class ExtrovertControlFactorAttractorStrategy : IFactorAttractorStrategy
{
    public float GetAttractionScale(Factors factors)
    {
        return factors.GetNormalized(FactorType.IntrovertToExtrovert);
    }
    public float GetAttractionToNeighborFactors(Factors factors)
    {
        return factors.GetNormalized(FactorType.Rage);
    }
}
public class HornyControlFactorAttractorStrategy : IFactorAttractorStrategy
{
    public float GetAttractionScale(Factors factors)
    {
        return factors.GetNormalized(FactorType.Horny);
    }
    public float GetAttractionToNeighborFactors(Factors factors)
    {
        return factors.GetNormalized(FactorType.StinkyToAttractive);
    }
}

public class AttractorControlFactorTuning
{
    public float AttractionForceMultiplier { get; set; }
    public float MaximumAttractiveForce { get; set; }
}

public class AttractorControlFactor : IFactorEffect
{
    private readonly AttractorControlFactorTuning tuning;
    private readonly IFactorAttractorStrategy factorAttractor;

    public AttractorControlFactor(
        AttractorControlFactorTuning tuning,
        IFactorAttractorStrategy factorAttractor)
    {
        this.tuning = tuning;
        this.factorAttractor = factorAttractor;
    }
    
    public AiResult GetFactorEffect(AiParams parameters)
    {
        var forceMultiplier = tuning.AttractionForceMultiplier;
        var maxForce = tuning.MaximumAttractiveForce;
        
        if (parameters.Neighbors.Length == 0)
        {// take no action when no neighbors
            return AiResult.Default;
        }

        var attractionScale = factorAttractor.GetAttractionScale(parameters.SelfFactors);
        var attractionMultiplier = forceMultiplier * attractionScale;

        var totalAttractionForce = Vector2.Zero;
        var totalSamples = 0;
        for (int i = 0; i < parameters.Neighbors.Length; i++)
        {
            var neighborNull = parameters.Neighbors[i];
            if(!neighborNull.HasValue) continue;
            var neighbor = neighborNull.Value;
            var neighborPosition = neighbor.Position;
            var neighborDistance = neighborPosition.Length();
            if(neighborDistance <= 0.001f) continue; // avoid divide by zero (or near zero)
            var neighborAttraction = factorAttractor.GetAttractionToNeighborFactors(neighbor.Factors);
            totalAttractionForce += neighborPosition.Normalized() * (1f/neighborDistance) * neighborAttraction;
            totalSamples++;
        }

        var averageAttractionForce = totalAttractionForce / totalSamples;
        var totalAttractiveForce = averageAttractionForce * attractionScale * attractionMultiplier;
        totalAttractiveForce = totalAttractiveForce.Clamped(maxForce);
        
        return new AiResult()
        {
            AdditionalLinearForce = totalAttractiveForce,
            Firmness = 1 - attractionScale
        };
    }
}