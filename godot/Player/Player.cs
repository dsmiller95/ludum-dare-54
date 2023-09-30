using Godot;
using System;
using DotnetLibrary;

public partial class Player : RigidBody2D
{
	[Export] public int AccelerationForce { get; set; } = 400; // How fast the player will accelerate (pixels/sec^2).
	[Export] public int RotationalAcceleration { get; set; } = 400; // How much force will apply to keep the player facing forward (kg pixels^2 / sec^2 radians) aka (Torque / radian)
	[Export] public int MaximumVelocity { get; set; } = 400; // the max velocity the player will move (pixels/sec).
	[Export] public int ActiveFrictionCoefficient { get; set; } = 10; // Resistance to movement. force / velocity (kg/s)

	private Vector2? lastTurnInput = null;
	
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
		var myPhysics = new PersonPhysics
		{
			AccelerationForce = AccelerationForce,
			RotationalAcceleration = RotationalAcceleration,
			MaximumVelocity = MaximumVelocity,
			ActiveFrictionCoefficient = ActiveFrictionCoefficient
		};
		
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
