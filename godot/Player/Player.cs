using Godot;
using System;
using DotnetLibrary;
using LudumDare54.Audience;

public partial class Player : RigidBody2D, IHavePersonBody
{
	[Export] public float AccelerationForce { get; set; } = 400; // How fast to accelerate (pixels/sec^2).
    [Export] public PersonPhysicsDefinition PersonMovement { get; set; } = null!;
	
	private Vector2? lastTurnInput = null;
	
	private PersonBody personBody;
	
	
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

	private Vector2? DesiredForwardDirection(Vector2 input)
	{
		if (input.Length() > 0.01 && (!lastTurnInput.HasValue || !IsInputTurnedToSide()))
		{
			lastTurnInput = input;
		}

		if (!lastTurnInput.HasValue) return null;
		
		var targetForward = lastTurnInput.Value;
		if (IsInputTurnedToSide())
		{
			targetForward = targetForward.Rotated(Mathf.Pi / 2);
		}

		return targetForward;
	}

	public override void _IntegrateForces(PhysicsDirectBodyState2D state)
	{
		var myPhysics = PersonMovement.GetConfiguredPhysics();
		
		var input = GetInputVectorNormalized();
		var desiredLinearForce = input * AccelerationForce;
		var desiredLookDirection = DesiredForwardDirection(input);

		var integrationResult = myPhysics.GetLinearForce(
			desiredLinearForce,
			desiredLookDirection,
			state.LinearVelocity,
			state.Transform.X);
		
		integrationResult.ApplyTo(state);
	}
}
