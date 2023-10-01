using DotnetLibrary;
using DotnetLibrary.Audience;
using Godot;

namespace LudumDare54.Audience;

public partial class CrowdActor : RigidBody2D, IHavePersonBody
{
    [Export] private CrowdActorType actorType;
    
    /// <summary>
    /// global move force identifier. should be set based on the physics system config.
    /// individual crowd actor impls may have their own scaling for this force
    /// </summary>
    [Export]
    private float moveForceMultiplier = 5f;
    
    [Export]
    private float assumedPushForce = 100f;
    [Export]
    private Resource crowdActorPreset;
    
    private ICrowdActor crowdActorImpl;
    private PersonBody personBody;
    private PersonPhysics myPhysics;

    [Export] public PersonPhysicsDefinition PersonMovement { get; set; } = null!;
    public override void _Ready()
    {
        if (crowdActorPreset is not ICrowdActorPreset preset)
        {
            GD.PrintErr("Resource is not a crowd actor preset");
            return;
        }
        crowdActorImpl = preset.ConstructConfiguredActor();// CrowdActorFactory.GetActor(actorType);
        personBody = new PersonBody(this);
        myPhysics = PersonMovement.GetConfiguredPhysics();
    }

    public PersonBody GetBody()
    {
        return personBody;
    }
    public override void _PhysicsProcess(double delta)
    {
        personBody._PhysicsProcess();
        
        crowdActorImpl.Update(delta, Time.GetTicksMsec() / 1000f);
        
        // calculate friction force manually, rather than instantiating a unique physics material per actor
        // var frictionFactor = myPhysics.ActiveFrictionCoefficient;
        // var firmnessFrictionForce = this.LinearVelocity * -frictionFactor;

        var selfMoveForce = crowdActorImpl.GetCurrentSelfMoveForce() * moveForceMultiplier;
        
        var integrationResult = myPhysics.ComputeIntegrationResult(
            selfMoveForce,
            Vector2.Up,
            this.LinearVelocity,
            this.Transform.X,
            crowdActorImpl.GetFirmness());
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
    }
    
    
    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        return;
    }
}