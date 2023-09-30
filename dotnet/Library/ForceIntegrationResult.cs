using Godot;

namespace DotnetLibrary;

public record struct ForceIntegrationResult
{
    public Vector2 LinearAcceleration;
    public Vector2? LinearVelocityOverride;
    public float AppliedTorque;

    public void ApplyTo(PhysicsDirectBodyState2D state)
    {
        state.ApplyCentralForce(LinearAcceleration);
        if (LinearVelocityOverride.HasValue)
        {
            state.LinearVelocity = LinearVelocityOverride.Value;
        }
        state.ApplyTorque(AppliedTorque);
    }
}