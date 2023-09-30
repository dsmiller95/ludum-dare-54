using DotnetLibrary.Audience;
using Godot;

namespace LudumDare54.Audience;

public partial class CrowdActor : RigidBody2D
{
    [Export]
    private float calmness = 0.5f;
    
    [Export]
    private float frictionMultiplier = 1f;
    
    /// <summary>
    /// global move force identifier. should be set based on the physics system config.
    /// individual crowd actor impls may have their own scaling for this force
    /// </summary>
    [Export]
    private float moveForceMultiplier = 1f;
    
    [Export]
    private float assumedPushForce = 1f;
    
    private readonly ICrowdActor crowdActorImpl = new CalmCrowdActor(0.5f);
    
    public override void _Ready()
    {
    }

    public override void _PhysicsProcess(double delta)
    {
        crowdActorImpl.Update(delta);
        
        // calculate friction force manually, rather than instantiating a unique physics material per actor
        var frictionFactor = crowdActorImpl.GetFirmness() * frictionMultiplier;
        var firmnessFrictionForce = this.LinearVelocity * -frictionFactor;

        var selfMoveForce = crowdActorImpl.GetCurrentSelfMoveForce() * moveForceMultiplier;
        
        ApplyCentralForce(selfMoveForce + firmnessFrictionForce);
    }

    public void OnBodyEntered(Node bodyGeneric)
    {
        if (bodyGeneric is not Node2D body)
        {
            GD.PrintErr("CrowdActor.OnBodyEntered: colliding body is not a node 2D");
            return;
        }
        
        // assume a certain push force magnitude
        var pushForce = assumedPushForce;
        var pushDirection = GlobalPosition - body.GlobalPosition;
        var pushVector = pushDirection.Normalized() * pushForce;

        var pushEvent = new PushEvent(pushVector);
        crowdActorImpl.ReceivePushEvent(pushEvent);
        
        GD.Print("CrowdActor.OnBodyEntered: emitted push event: " + pushEvent);
    }
}