using DotnetLibrary.Audience.Factors;
using Godot;

namespace DotnetLibrary.Audience;

public class CalmCrowdActor
{
    private Vector2 DecayingPushForceRecord { get; set; }
    private float DecayingPushMagnitudeRecord { get; set; }

    private readonly float calmness;
    private readonly float frictionMultiplier;
    private float PushExponentialDecayConstant => calmness;
    private float PushBackMultiplier => 1 - calmness;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="calmness">a value from 0 to 1. 1 is maximally chill, 0 will hold grudges for eternity</param>
    /// <param name="frictionMultiplier"></param>
    public CalmCrowdActor(float calmness, float frictionMultiplier)
    {
        this.calmness = calmness;
        this.frictionMultiplier = frictionMultiplier;
    }

    public void Update(double deltaTime, double currentSeconds)
    {
        var exponentialDecay = (float)Mathf.Pow(Mathf.E, -PushExponentialDecayConstant * deltaTime);
        DecayingPushForceRecord *= exponentialDecay;
        DecayingPushMagnitudeRecord *= exponentialDecay;
    }

    public void ReceivePushEvent(PushEvent pushEvent)
    {
        DecayingPushForceRecord += pushEvent.PushForce;
        DecayingPushMagnitudeRecord += pushEvent.PushForce.Length();
    }

    public float GetFirmness()
    {
        // smooth ramp down. should stay at 1 for a while, then ramp down towards 0 rapidly
        return Mathf.Pow(DecayingPushMagnitudeRecord, 0.2f) * frictionMultiplier;
    }

    public Vector2 GetCurrentSelfMoveForce()
    {
        return DecayingPushForceRecord * PushBackMultiplier * -1;
    }
    
    public CrowdActorEffect GetCrowdEffectLevels()
    {
        return CrowdActorEffect.Zero;
    }
}