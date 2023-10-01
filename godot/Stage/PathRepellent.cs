using System;
using System.IO;
using DotnetLibrary;
using Godot;
using LudumDare54.Audience;

namespace LudumDare54.Stage;

public partial class PathRepellent : Area2D
{
    [Export] public float RepellentForce = 100f;

    [Export] public CollisionShape2D RepellentShapeDefinition;
    [Export] public bool OnlyRepelCrowdActors = true;
    
    public override void _Ready()
    {
        if (RepellentShapeDefinition.Shape is not RectangleShape2D rectShape)
        {
            GD.PrintErr($"Using invalid shape for path repellent {Name}");
            throw new InvalidDataException($"Using invalid shape for repellent {Name}");
        }
    }
    
    public Vector2 GetRepellentForce(Node2D other)
    {
        var shape = RepellentShapeDefinition.Shape as RectangleShape2D;
        ArgumentNullException.ThrowIfNull(shape);
        var majorAxis = shape.Size.X > shape.Size.Y ? Vector2.Right : Vector2.Up;
        var majorAxisGlobal = RepellentShapeDefinition.GlobalTransform.ToGlobalDirectionalVector(majorAxis);
        var minorAxisGlobal = majorAxisGlobal.Rotated(Mathf.Pi / 2f);
        
        var delta = other.GlobalPosition - RepellentShapeDefinition.GlobalPosition;
        var deltaDir = delta.Normalized();
        var deltaInMinorAxis = deltaDir.Dot(minorAxisGlobal);
        
        return minorAxisGlobal * Mathf.Sign(deltaInMinorAxis) * RepellentForce;
    }

    public override void _PhysicsProcess(double delta)
    {
        foreach (var overlappingBody in GetOverlappingBodies())
        {
            if (overlappingBody is not RigidBody2D rigid) continue;
            if (OnlyRepelCrowdActors && rigid is not CrowdActor crowdActor) continue;
            var force = GetRepellentForce(rigid);
            rigid.ApplyCentralForce(force);
        }
    }
}