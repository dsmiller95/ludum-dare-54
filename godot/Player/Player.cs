using Godot;
using System;
using DotnetLibrary;
using LudumDare54.Audience;

public partial class Player : RigidBody2D, IHavePersonBody
{
	[Signal] public delegate void SoftCollisionEventHandler();
	[Signal] public delegate void HealthDepletedEventHandler();
	[Signal] public delegate void YouDiedEventHandler();

	[Export] public float AccelerationForce { get; set; } = 400; // How fast to accelerate (pixels/sec^2).
	[Export] public float SturdySmoothingMultiplier { get; set; } = 1; // higher is more responsive. lower is more sluggish
	[Export] public float MinimumSturdySpeed { get; set; } = 0.2f;
	[Export] public float MaximumDashSpeed { get; set; } = 2f;
	[Export] public float SturdyImpactResistance { get; set; } = 1.3f;
	[Export] public float DashImpactResistance { get; set; } = 0.9f;
	[Export] public PersonPhysicsDefinition PersonMovement { get; set; } = null!;

	private Vector2? lastTurnInput = null;
	/// <summary>
	/// -1..1 
	/// Positive values cause slower, more durable movement. negative values cause faster, more unsteady movement.
	/// </summary>
	private float currentSturdyFactor = 0f;

	private PersonBody personBody;

	public override void _Ready()
	{
		Health.HealthValue = 100;
		personBody = new PersonBody(this);
	}

	public PersonBody GetBody()
	{
		return personBody;
	}

	public override void _PhysicsProcess(double delta)
	{
		personBody._PhysicsProcess();
		currentSturdyFactor = Mathf.Lerp(currentSturdyFactor, GetSturdyVsDash(), SturdySmoothingMultiplier * (float)delta);
		
		var myPhysics = PersonMovement.GetConfiguredPhysics();

		var input = GetInputVectorNormalized();
		var desiredLinearForce = input * AccelerationForce * GetSturdyMovementMultiplier(currentSturdyFactor);
		var desiredLookDirection = DesiredForwardDirection(input);

		var integrationResult = myPhysics.ComputeIntegrationResult(
			desiredLinearForce,
			desiredLookDirection,
			this.LinearVelocity,
			this.Transform.X);

		integrationResult.ApplyTo(this);
	}

	public void OnBodyEntered(Node bodyGeneric)
	{
		if (bodyGeneric is not RigidBody2D otherBody) return;
		
		var impactVector = otherBody.LinearVelocity - LinearVelocity;
		var impact = impactVector.Length();

		float impactThreshold = 100;
		impactThreshold *= GetSturdyImpactThreshold(currentSturdyFactor);
		
		if (!(impact > impactThreshold))
		{
			EmitSignal("SoftCollision");
			return;
		}

		Health.AdjustHealth(-5);
		EmitSignal("HealthDepleted");
		var spill = GetNode<CpuParticles2D>("SpillParticles");
		spill.Direction = new Vector2(impactVector.Y, -impactVector.X);
		spill.InitialVelocityMin = impact;
		spill.InitialVelocityMax = impact * 5;
		spill.Emitting = true;

		if (Health.HealthValue > 0) return;
		
		EmitSignal("YouDied");
		GetTree().ChangeSceneToFile("res://MenuScenes/GameOverScene.tscn");
	}
	
	private Vector2 GetInputVectorNormalized()
	{
		var velocity = Vector2.Zero; // The player's movement vector.

		if (Input.IsActionPressed("move_right"))
		{
			velocity.X += 1;
		}

		if (Input.IsActionPressed("move_left"))
		{
			velocity.X -= 1;
		}

		if (Input.IsActionPressed("move_down"))
		{
			velocity.Y += 1;
		}

		if (Input.IsActionPressed("move_up"))
		{
			velocity.Y -= 1;
		}

		velocity = velocity.Normalized();
		return velocity;
	}

	private bool IsInputTurnedToSide()
	{
		return Input.IsActionPressed("turn_side");
	}

	private float GetSturdyImpactThreshold(float sturdyFactor)
	{
		return sturdyFactor switch
		{
			< 0 => Mathf.Lerp(1, DashImpactResistance, -sturdyFactor),
			> 0 => Mathf.Lerp(1, SturdyImpactResistance, sturdyFactor),
			_ => 1
		};
	}
	
	private float GetSturdyMovementMultiplier(float sturdyFactor)
	{
		return sturdyFactor switch
		{
			< 0 => Mathf.Lerp(1, MaximumDashSpeed, -sturdyFactor),
			> 0 => Mathf.Lerp(1, MinimumSturdySpeed, sturdyFactor),
			_ => 1
		};
	}
	
	private float GetSturdyVsDash()
	{
		var sturdyFactor = 0;
		if (Input.IsActionPressed("brace"))
		{
			sturdyFactor += 1;
		}

		if (Input.IsActionPressed("dash"))
		{
			sturdyFactor -= 1;
		}

		return sturdyFactor;
	}

	private Vector2? DesiredForwardDirection(Vector2 input)
	{
		if (input.Length() > 0.01 && (!lastTurnInput.HasValue || !IsInputTurnedToSide()))
		{
			lastTurnInput = input;
		}

		if (!lastTurnInput.HasValue) return null;

		var targetForward = lastTurnInput.Value;
		targetForward += Vector2.Up * 2;

		if (IsInputTurnedToSide())
		{
			targetForward = targetForward.Rotated(Mathf.Pi / 2);
		}

		return targetForward;
	}

}
