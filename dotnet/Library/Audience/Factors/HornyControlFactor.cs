using Godot;

namespace DotnetLibrary.Audience.Factors;

public class HornyControlFactor : IFactorEffect
{
    private readonly FactorTuningParams tuning;

    public HornyControlFactor(FactorTuningParams tuning)
    {
        this.tuning = tuning;
    }

    public AiResult GetFactorEffect(AiParams parameters)
    {
        if (parameters.Neighbors.Length == 0)
        {// take no action when no neighbors
            return AiResult.Default;
        }

        var hornyScale = parameters.SelfFactors.GetNormalized(FactorType.Horny);
        var attractionMultiplier = Mathf.Lerp(
            tuning.MinAttractionForceMultiplier,
            tuning.MaxAttractionForceMultiplier,
            hornyScale);

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
            var neighborAttraction = neighbor.Factors.GetNormalized(FactorType.StinkyToAttractive) + tuning.GlobalAttractionOffset;
            totalAttractionForce += neighborPosition.Normalized() * (1f/neighborDistance) * neighborAttraction;
            totalSamples++;
        }

        var averageAttractionForce = totalAttractionForce / totalSamples;
        var totalAttractiveForce = averageAttractionForce * hornyScale * attractionMultiplier;
        totalAttractiveForce = totalAttractiveForce.Clamped(tuning.MaximumAttractiveForce);
        
        return new AiResult()
        {
            AdditionalLinearForce = totalAttractiveForce,
            Firmness = 1 - hornyScale
        };
    }
}