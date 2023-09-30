using Godot;
using System;

public partial class Player : RigidBody2D
{
	[Export] public int AccelerationForce { get; set; } = 400; // How fast the player will accelerate (pixels/sec^2).
	[Export] public int MaximumVelocity { get; set; } = 400; // the max velocity the player will move (pixels/sec).
	
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

	public override void _IntegrateForces(PhysicsDirectBodyState2D state)
	{
		var input = GetInputVectorNormalized(); 
		
		state.ApplyCentralForce(input * AccelerationForce);

		var velocityMagnitude = state.LinearVelocity.Length();
		if (velocityMagnitude > MaximumVelocity)
		{
			velocityMagnitude = MaximumVelocity;
			state.LinearVelocity = state.LinearVelocity.Normalized() * velocityMagnitude;
		}
		
	}
}
