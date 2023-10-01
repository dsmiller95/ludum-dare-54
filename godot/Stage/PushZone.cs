using Godot;

namespace LudumDare54.Stage;

public partial class PushZone: Area2D
{
    [Export] private float PushForce = 5000f;

    [Export] private PushDirection PushDirection = PushDirection.Up;

    public override void _PhysicsProcess(double delta)
    {
        foreach (var overlappingBody in GetOverlappingBodies())
        {
            if (overlappingBody is not RigidBody2D rigid) continue;

            var pushIdentity = PushDirection switch
            {
                PushDirection.Up => Vector2.Up,
                PushDirection.Right => Vector2.Right,
                PushDirection.Left => Vector2.Left,
                PushDirection.Down => Vector2.Down
            };
            
            rigid.ApplyCentralForce(pushIdentity * PushForce);
        }
    }
}

public enum PushDirection
{
    Up, 
    Down, 
    Left, 
    Right
}