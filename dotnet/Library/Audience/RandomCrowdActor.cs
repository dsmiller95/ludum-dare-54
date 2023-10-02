using Godot;

namespace DotnetLibrary.Audience;

public class RandomCrowdActor : ICrowdActor
{
    private readonly float randomnessSpeed;
    private Vector2 RandomMoveVector { get; set; }

    private RandomNumberGenerator rng = new RandomNumberGenerator();
    private readonly float timeOffset;
    private readonly float xSpeed;

    private readonly float ySpeed;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="calmness">a value from 0 to 1. 1 is maximally chill, 0 will hold grudges for eternity</param>
    public RandomCrowdActor(float randomnessSpeed = 2)
    {
        this.randomnessSpeed = randomnessSpeed;
        this.timeOffset = rng.Randf();
        this.xSpeed = rng.Randf();
        this.ySpeed = rng.Randf();
    }

    public void Update(double deltaTime, double currentSeconds, Span<NeighborCrowdActor> neighbors)
    {
        currentSeconds += timeOffset;
        currentSeconds *= randomnessSpeed;
        var waveX = (float)Mathf.Sin(currentSeconds * xSpeed);
        var waveY = (float)Mathf.Sin(currentSeconds * ySpeed);

        RandomMoveVector = new Vector2(waveX, waveY);
    }

    public void ReceivePushEvent(PushEvent pushEvent)
    {
    }

    public float GetFirmness()
    {
        return 1;
    }

    public Vector2 GetCurrentSelfMoveForce()
    {
        return RandomMoveVector;
    }

    public CrowdActorEffect GetCrowdEffectLevels()
    {
        return new CrowdActorEffect
        {
            DrunkEffect = 1
        };
    }

    public float GetAlertStrength()
    {
        return 0;
    }
}