using Godot;

namespace DotnetLibrary.Audience.Factors;

public class RageFactor : IFactorEffect
{
    private readonly FactorTuningParams tuning;
    private readonly RandomNumberGenerator rng;

    private Punch? currentPunch = null;

    private struct Punch
    {
        public float PunchStartTime;
        public Vector2 PunchDirection;
        public float PunchMagnitude;

        public Punch(float punchStartTime, Vector2 punchDirection, float punchMagnitude)
        {
            PunchStartTime = punchStartTime;
            PunchDirection = punchDirection;
            PunchMagnitude = punchMagnitude;
        }

        public AiResult? ContinuePunch(float currentTime, float punchDuration)
        {
            var t = (currentTime - PunchStartTime) / punchDuration;
            if(t > 1) return null;
            // double integral of sin(x) is -sin(x), sin(Pi) = 0
            var punchLerpForce = Mathf.Sin(t * Mathf.Pi);
            var punchForce = PunchDirection * punchLerpForce * PunchMagnitude;
            return new AiResult
            {
                AdditionalLinearForce = punchForce,
                Firmness = 1
            };
        }
    }
    
    public RageFactor(FactorTuningParams tuning, RandomNumberGenerator rng)
    {
        this.tuning = tuning;
        this.rng = rng;
    }

    public AiResult GetFactorEffect(AiParams parameters)
    {
        if (currentPunch.HasValue)
        {
            var punchResult = this.currentPunch.Value.ContinuePunch(parameters.currentTime, tuning.RagePunchDuration);
            if (punchResult.HasValue) return punchResult.Value;
            currentPunch = null;
        }
        
        var punchChance = tuning.RagePunchChancePerSecond * parameters.deltaTime;
        var willPunchThisFrame = rng.Randf() < punchChance;
        if (!willPunchThisFrame) return AiResult.Default;
        
        var factorScale = parameters.SelfFactors.GetNormalized(FactorType.Rage);
        this.currentPunch = GetPunch(parameters.currentTime, factorScale);

        return AiResult.Default;
    }
    
    
    private Punch GetPunch(float currentTime, float punchForce)
    {
        var punchDirection = new Vector2(rng.RandfRange(-1, 1), rng.RandfRange(-1, 1)).Normalized();
        var punchMagnitude = rng.RandfRange(tuning.RagePunchMinMagnitude, tuning.RagePunchMaxMagnitude);
        return new Punch(currentTime, punchDirection, punchMagnitude * punchForce);
    }
}