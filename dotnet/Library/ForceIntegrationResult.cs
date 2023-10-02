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

    private bool IsValidNumber(float num)
    {
        return !float.IsNaN(num) && !float.IsInfinity(num);
    }
    
    public bool ClampAndCheckValid(float maximumTorque, float maximumAcceleration)
    {
        if(!IsValidNumber(AppliedTorque) || 
           !IsValidNumber(LinearAcceleration.X) || !IsValidNumber(LinearAcceleration.Y) ||
           !IsValidNumber(LinearVelocityOverride?.X ?? 0) || !IsValidNumber(LinearVelocityOverride?.Y ?? 0))
        {
            return false;
        }
        var clampedT =Mathf.Clamp(AppliedTorque, -maximumTorque, maximumTorque);
        var clampedAcc = LinearAcceleration.Clamped(maximumAcceleration);
        AppliedTorque = clampedT;
        LinearAcceleration = clampedAcc;
        return true;
    }
    
    public void ApplyTo(RigidBody2D state)
    {
        state.ApplyCentralForce(LinearAcceleration);
        if (LinearVelocityOverride.HasValue)
        {
            state.LinearVelocity = LinearVelocityOverride.Value;
        }
        state.ApplyTorque(AppliedTorque);
    }
}