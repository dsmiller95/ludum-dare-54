using DotnetLibrary;
using Godot;

namespace LudumDare54;

public partial class CrowdActorDummy : Node2D
{
    private readonly ICrowdActor crowdActorImpl = new DummyCrowdActor();
    public override void _Ready()
    {
        var push = new PushPhysicsEvent(this.Position, Vector2.Up);
        crowdActorImpl.ReceivePushEvent(push);
    }

    public override void _PhysicsProcess(double delta)
    {
        crowdActorImpl.PhysicsUpdate(delta);
        this.Position = crowdActorImpl.Position;
    }
}