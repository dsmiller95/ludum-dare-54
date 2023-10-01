using Godot;

namespace DotnetLibrary;

public static class TransformExtensions
{
    public static Vector2 ToGlobalDirectionalVector(this Transform2D transform, Vector2 vector)
    {
        return transform * vector - transform.Origin;
    }
}