using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
    

    private AnimatedSprite2D sprite;

    public ICrowdActor CrowdActorImpl;
    private PersonBody personBody;
    private PersonPhysics myPhysics;

    public CrowdCorral CrowdCorral;

    [Export] public PersonPhysicsDefinition PersonMovement { get; set; } = null!;
    public override void _Ready()
    {
        sprite = (AnimatedSprite2D)GetNode("AnimatedSprite2D");
        ProcessPriority = 11;
        var options = crowdActorPresetOptions.Cast<ICrowdActorPreset>().Where(x => x != null).ToArray();
        if (options.Length <= 0)
        {
            GD.PrintErr("no crowd actor presets provided");
            return;
        }

        var rng = new RandomNumberGenerator();
        rng.Randomize();
        CrowdActorImpl = rng.PickRandom(options).ConstructConfiguredActor();
        personBody = new PersonBody(this);
        myPhysics = PersonMovement.GetConfiguredPhysics();
    }

    public PersonBody GetBody()
    {
        return personBody;
    }

    private int GetZIndex()
    {
        long index = (int)Position.Y/10;
        index = Math.Clamp(index, Godot.RenderingServer.CanvasItemZMin + 10, Godot.RenderingServer.CanvasItemZMax - 10);
        return (int)index;
    }
    public override void _Process(double delta)
    {
        sprite.ZIndex = GetZIndex();
        
        var crowdEffectLevels = CrowdActorImpl.GetCrowdEffectLevels();
        effectsRenderer.RenderEffects(crowdEffectLevels);
        if (CrowdActorImpl is FactorBasedCrowdActor factorBased)
        {
            effectsRenderer.RenderDebugRawFactors(factorBased.GetRawFactorsUnnormalized());
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        // var neighborCrowdActors = NeighborCrowdActors();
        //
        // integrationResult =ManagedPhysicsProcess(delta, neighborCrowdActors);
        //
        
        // integrationResult?.ApplyTo(this);
        // integrationResult = null;
        // personBody._PhysicsProcess();
    }

    private List<NeighborCrowdActor> neighborCrowdActorCache = new List<NeighborCrowdActor>();
    public void OwnedPhysicsProcess(double delta, Span<NeighborCrowdActor> neighbors)
    {
        ManagedPhysicsProcess(delta, neighbors);
        personBody._PhysicsProcess();
    }
    
    private void ManagedPhysicsProcess(double delta, Span<NeighborCrowdActor> neighbors)
    {
        CrowdActorImpl.Update(delta, Time.GetTicksMsec() / 1000f, neighbors);
        
        var selfMoveForce = CrowdActorImpl.GetCurrentSelfMoveForce() * moveForceMultiplier;
        
        var integrationResult = myPhysics.ComputeIntegrationResult(
            selfMoveForce,
            Vector2.Up,
            this.LinearVelocity,
            this.Transform.X,
            CrowdActorImpl.GetFirmness());
        integrationResult.ApplyTo(this);
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
        CrowdActorImpl.ReceivePushEvent(pushEvent);
    }
    
    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        return;
    }
}