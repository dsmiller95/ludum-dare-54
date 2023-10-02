using System.Collections.Generic;
using System.Linq;
using DotnetLibrary;
using DotnetLibrary.Audience;
using DotnetLibrary.Audience.Factors;
using Godot;
using Godot.Collections;

namespace LudumDare54.Audience;

public partial class CrowdActor : RigidBody2D, IHavePersonBody
{
    /// <summary>
    /// global move force identifier. should be set based on the physics system config.
    /// individual crowd actor impls may have their own scaling for this force
    /// </summary>
    [Export]
    private float moveForceMultiplier = 5f;
    
    [Export]
    private float assumedPushForce = 100f;
    [Export]
    private ActorEffectsRenderer effectsRenderer;
    [Export]
    private Array<Resource> crowdActorPresetOptions;
    
    private ICrowdActor crowdActorImpl;
    private PersonBody personBody;
    private PersonPhysics myPhysics;

    public CrowdCorral CrowdCorral;

    [Export] public PersonPhysicsDefinition PersonMovement { get; set; } = null!;
    public override void _Ready()
    {
        ProcessPriority = 11;
        var options = crowdActorPresetOptions.Cast<ICrowdActorPreset>().Where(x => x != null).ToArray();
        if (options.Length <= 0)
        {
            GD.PrintErr("no crowd actor presets provided");
            return;
        }

        var rng = new RandomNumberGenerator();
        rng.Randomize();
        crowdActorImpl = rng.PickRandom(options).ConstructConfiguredActor();
        personBody = new PersonBody(this);
        myPhysics = PersonMovement.GetConfiguredPhysics();
    }

    public PersonBody GetBody()
    {
        return personBody;
    }
    public override void _PhysicsProcess(double delta)
    {
        var neighborCrowdActors = NeighborCrowdActors();

        crowdActorImpl.Update(delta, Time.GetTicksMsec() / 1000f, neighborCrowdActors);
        
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

        personBody._PhysicsProcess();
        
        var crowdEffectLevels = crowdActorImpl.GetCrowdEffectLevels();
        effectsRenderer.RenderEffects(crowdEffectLevels);
        if (crowdActorImpl is FactorBasedCrowdActor factorBased)
        {
            effectsRenderer.RenderDebugRawFactors(factorBased.GetRawFactorsUnnormalized());
        }
    }

    private NeighborCrowdActor[] NeighborCrowdActors()
    {
        var neighbors = CrowdCorral.GetNeighbors(this);
        if (neighbors == null) return null;
        var neighborCrowdActors = new List<NeighborCrowdActor>();

        foreach (var neighbor in neighbors)
        {
            neighborCrowdActors.Add(new NeighborCrowdActor
            {
                actor = neighbor.crowdActorImpl,
                relativePosition = neighbor.GlobalPosition
            });
        }

        return neighborCrowdActors.ToArray();
    }

    public void OnBodyEntered(Node bodyGeneric)
    {
        if (bodyGeneric is not Node2D body)
        {
            GD.PrintErr("CrowdActor.OnBodyEntered: colliding body is not a node 2D");
            return;
        }
        
        if (body is not IHavePersonBody otherPerson)
        {
            //GD.Print($"CrowdActor.OnBodyEntered: colliding body {body.Name} is not a person");
            return;
        }
        
        var person = otherPerson.GetBody();
        var pushDirection = GlobalPosition - body.GlobalPosition;

        var pushVector = pushDirection.Normalized() * person.GetAverageSpeed() / PersonMovement.MaximumVelocity;

        var pushEvent = new PushEvent(pushVector);
        crowdActorImpl.ReceivePushEvent(pushEvent);
    }
    
    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        return;
    }
}
