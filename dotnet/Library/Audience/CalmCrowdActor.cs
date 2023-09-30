using Godot;

namespace DotnetLibrary.Audience;

public class CalmCrowdActor : ICrowdActor
{
    private Vector2 DecayingPushForceRecord { get; set; }
    private float DecayingPushMagnitudeRecord { get; set; }

    private readonly float calmness;
    private float PushExponentialDecayConstant => calmness;
    private float PushBackMultiplier => 1 - calmness;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="calmness">a value from 0 to 1. 1 is maximally chill, 0 will hold grudges for eternity</param>
    public CalmCrowdActor(float calmness)
    {
        this.calmness = calmness;
    }

    public void Update(double deltaTime)
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
        return Mathf.Pow(DecayingPushMagnitudeRecord, 0.2f);
    }

    public Vector2 GetCurrentSelfMoveForce()
    {
        return DecayingPushForceRecord * PushBackMultiplier * -1;
    }
}