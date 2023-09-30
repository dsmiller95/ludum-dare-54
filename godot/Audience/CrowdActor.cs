using DotnetLibrary;
using DotnetLibrary.Audience;
using Godot;

namespace LudumDare54.Audience;

public partial class CrowdActor : RigidBody2D, IHavePersonBody
{
    [Export]
    private float calmness = 0.5f;
    
    // [Export]
    // private float frictionMultiplier = 1f;
    //
    // /// <summary>
    // /// global move force identifier. should be set based on the physics system config.
    // /// individual crowd actor impls may have their own scaling for this force
    // /// </summary>
    // [Export]
    // private float moveForceMultiplier = 1f;
    
    [Export]
    private float assumedPushForce = 1f;
    
    private readonly ICrowdActor crowdActorImpl = new CalmCrowdActor(0.5f);
    private PersonBody personBody;
    
    [Export] public PersonPhysicsDefinition PersonMovement { get; set; } = null!;
    public override void _Ready()
    {
        personBody = new PersonBody(this);
    }

    public PersonBody GetBody()
    {
        return personBody;
    }
    public override void _PhysicsProcess(double delta)
    {
        personBody._PhysicsProcess();
        
        crowdActorImpl.Update(delta);
        
        // // calculate friction force manually, rather than instantiating a unique physics material per actor
        // var frictionFactor = crowdActorImpl.GetFirmness() * frictionMultiplier;
        // var firmnessFrictionForce = this.LinearVelocity * -frictionFactor;
        //
        // var selfMoveForce = crowdActorImpl.GetCurrentSelfMoveForce() * moveForceMultiplier;
        //
        // ApplyCentralForce(selfMoveForce + firmnessFrictionForce);
    }

    public void OnBodyEntered(Node bodyGeneric)
    {
        if (bodyGeneric is not Node2D body)
        {
            GD.PrintErr("CrowdActor.OnBodyEntered: colliding body is not a node 2D");
            return;
        }
        if (body is not IHavePersonBody personOwner)
        {
            GD.PrintErr("CrowdActor.OnBodyEntered: colliding body is not a Person Body");
            return;
        }

        var person = personOwner.GetBody();
        var delta = person.GetVelocityDelta();
        
        // assume a certain push force magnitude
        var pushForce = delta;
        var pushDirection = GlobalPosition - body.GlobalPosition;
        var pushVector = pushDirection.Normalized() * pushForce;

        var pushEvent = new PushEvent(pushVector);
        crowdActorImpl.ReceivePushEvent(pushEvent);
        
        GD.Print("CrowdActor.OnBodyEntered: emitted push event: " + pushEvent);
    }
    
    
    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        var frictionFactor = crowdActorImpl.GetFirmness();
        var desiredLinearForce = crowdActorImpl.GetCurrentSelfMoveForce();
        var desiredLookDirection = desiredLinearForce.Normalized();

        var myPhysics = PersonMovement.GetConfiguredPhysics()
            .WithModifiedFriction(frictionFactor);
        
        var integrationResult = myPhysics.GetLinearForce(
            desiredLinearForce,
            desiredLookDirection,
            state.LinearVelocity,
            state.Transform.X);
		  
        integrationResult.ApplyTo(state);
    }
}