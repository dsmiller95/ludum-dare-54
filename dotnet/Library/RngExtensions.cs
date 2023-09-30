using Godot;

namespace DotnetLibrary;

public static class RngExtensions
{
    
    public static Vector2 NextVector2(this RandomNumberGenerator rng, float min, float max)
    {
        return new Vector2(rng.RandfRange(min, max), rng.RandfRange(min, max));
    }
}