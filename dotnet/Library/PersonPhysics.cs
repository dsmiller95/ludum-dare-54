//using Godot; Godot is not available in a class library project

using Godot;

namespace DotnetLibrary;

public record PersonPhysics
{
	public float RotationalAcceleration { get; init; } = 400; // How much force will apply to keep facing forward (kg pixels^2 / sec^2 radians) aka (Torque / radian)
	public float MaximumVelocity { get; init; } = 400; // the max velocity of movement. (pixels/sec).
	public float ActiveFrictionCoefficient { get; init; } = 10; // Resistance to movement. force / velocity (kg/s)
	
	public ForceIntegrationResult ComputeIntegrationResult(
		Vector2 desiredLinearForce,
		Vector2? turnTowardsTarget,
		Vector2 currentLinearVelocity,
		Vector2 currentForward,
		float frictionModifier = 1
	)
	{
		var integrationResult = new ForceIntegrationResult
		{
			LinearAcceleration = Vector2.Zero,
			AppliedTorque = 0
		};
		
		var activeFriction = currentLinearVelocity * -(ActiveFrictionCoefficient * frictionModifier);
		integrationResult.LinearAcceleration = desiredLinearForce + activeFriction;

		var velocityMagnitude = currentLinearVelocity.Length();
		
		if (velocityMagnitude > MaximumVelocity)
		{
			velocityMagnitude = MaximumVelocity;
			integrationResult.LinearVelocityOverride = currentLinearVelocity.Normalized() * velocityMagnitude;
		}

		if (turnTowardsTarget.HasValue)
		{
			var targetForward = turnTowardsTarget.Value;
			var turnAngleDeltaRadians = currentForward.AngleTo(targetForward);
			var turnForce = turnAngleDeltaRadians * RotationalAcceleration;
			integrationResult.AppliedTorque = turnForce;
		}
		
		return integrationResult;
	}
}
