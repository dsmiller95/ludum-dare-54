using Godot;

namespace DotnetLibrary;

public interface ICrowdActor
{
    public Vector2 Position { get; }
    public void ReceivePushEvent(PushPhysicsEvent pushEvent);
    public void PhysicsUpdate(double deltaTime);
}

public record struct PushPhysicsEvent(Vector2 PushedFromPosition, Vector2 PushingDirection);

public class DummyCrowdActor : ICrowdActor
{
    private readonly RandomNumberGenerator rng;
    public Vector2 Position { get; private set; }

    public DummyCrowdActor()
    {
        this.rng = new RandomNumberGenerator();
    }
    
    public void ReceivePushEvent(PushPhysicsEvent pushEvent)
    {
        var dir = pushEvent.PushingDirection.Normalized();
        var newPos = Position + dir * 10;
        Position = newPos;
    }

    public void PhysicsUpdate(double deltaTime)
    {
        var randMove = new Vector2(
            rng.RandfRange(-1, 1),
            rng.RandfRange(-1, 1));
        var newPos = Position + randMove * 10 * (float)deltaTime;
        Position = newPos;
    }
}
