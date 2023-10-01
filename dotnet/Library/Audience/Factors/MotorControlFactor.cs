using Godot;

namespace DotnetLibrary.Audience.Factors;

public class MotorControlFactor : IFactorEffect
{
    private readonly FactorTuningParams tuning;
    private readonly float timeOffset;
    private readonly float xSpeed;
    private readonly float ySpeed;

    public MotorControlFactor(FactorTuningParams tuning, RandomNumberGenerator rng)
    {
        this.tuning = tuning;
        this.timeOffset = rng.Randf();
        this.xSpeed = rng.Randf();
        this.ySpeed = rng.Randf();
    }

    public AiResult GetFactorEffect(AiParams parameters)
    {
        var currentSeconds = parameters.currentTime + timeOffset;
        currentSeconds *= tuning.RandomWalkJitter;
        var waveX = (float)Mathf.Sin(currentSeconds * xSpeed);
        var waveY = (float)Mathf.Sin(currentSeconds * ySpeed);

        var factorScale = parameters.SelfFactors.GetNormalized(FactorType.MotorControl);
        return new AiResult
        {
            AdditionalLinearForce = new Vector2(waveX, waveY) * factorScale
        };
    }
}