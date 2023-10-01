using Godot;

namespace DotnetLibrary.Audience.Factors;

public struct AiParams
{
    public float deltaTime;
    public float currentTime;
    
    public Factors SelfFactors;
    public AiNeighbor?[] Neighbors;
}

public struct AiNeighbor
{
    /// <summary>
    /// relative to self 
    /// </summary>
    public Vector2 Position;

    /// <summary>
    /// maybe this isn't the full factors, but an abbreviated version
    /// </summary>
    public Factors Factors;
}