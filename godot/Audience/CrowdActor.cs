using DotnetLibrary;
using DotnetLibrary.Audience;
using Godot;

namespace LudumDare54.Audience;

public partial class CrowdActor : RigidBody2D, IHavePersonBody
{
    [Export]
    private float calmness = 0.5f;
    
    /// <summary>
    /// global move force identifier. should be set based on the physics system config.
    /// individual crowd actor impls may have their own scaling for this force
    /// </summary>
    [Export]
    private float moveForceMultiplier = 5f;
    
    [Export]
    private float assumedPushForce = 100f;
    
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
        
        var myPhysics = PersonMovement.GetConfiguredPhysics()
            .WithModifiedFriction(crowdActorImpl.GetFirmness());
        
        // calculate friction force manually, rather than instantiating a unique physics material per actor
        // var frictionFactor = myPhysics.ActiveFrictionCoefficient;
        // var firmnessFrictionForce = this.LinearVelocity * -frictionFactor;

        var selfMoveForce = crowdActorImpl.GetCurrentSelfMoveForce() * moveForceMultiplier;
        
        var integrationResult = myPhysics.ComputeIntegrationResult(
            selfMoveForce,
            Vector2.Up,
            this.LinearVelocity,
            this.Transform.X);
        integrationResult.ApplyTo(this);
    }

    public void OnBodyEntered(Node bodyGeneric)
    {
        if (bodyGeneric is not Node2D body)
        {
            GD.PrintErr("CrowdActor.OnBodyEntered: colliding body is not a node 2D");
            return;
        }
        // if (body is not IHavePersonBody personOwner)
        // {
        //     GD.PrintErr("CrowdActor.OnBodyEntered: colliding body is not a Person Body");
        //     return;
        // }
        //
        // var person = personOwner.GetBody();
        // var delta = person.GetVelocityDelta();
        //
        // assume a certain push force magnitude
        var pushForce = assumedPushForce;
        var pushDirection = GlobalPosition - body.GlobalPosition;
        var pushVector = pushDirection.Normalized() * pushForce;

        var pushEvent = new PushEvent(pushVector);
        crowdActorImpl.ReceivePushEvent(pushEvent);
        
        GD.Print("CrowdActor.OnBodyEntered: emitted push event: " + pushEvent);

    }
    
    
    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        return;
        var frictionFactor = crowdActorImpl.GetFirmness();
        var desiredLinearForce = crowdActorImpl.GetCurrentSelfMoveForce() * moveForceMultiplier;
        var desiredLookDirection = desiredLinearForce.Normalized();

        var myPhysics = PersonMovement.GetConfiguredPhysics()
            .WithModifiedFriction(frictionFactor);
        
        var integrationResult = myPhysics.ComputeIntegrationResult(
            desiredLinearForce,
            desiredLookDirection,
            state.LinearVelocity,
            state.Transform.X);
		  
        integrationResult.ApplyTo(state);
    }
}