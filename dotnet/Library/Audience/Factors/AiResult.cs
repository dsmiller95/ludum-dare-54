using Godot;

namespace DotnetLibrary.Audience.Factors;

public struct AiResult
{
    public Vector2 AdditionalLinearForce;
    public float Firmness;
    
    public static AiResult operator +(AiResult a, AiResult b) => new AiResult
    {
        AdditionalLinearForce = a.AdditionalLinearForce + b.AdditionalLinearForce,
        Firmness = a.Firmness + b.Firmness
    };

    public static AiResult operator *(AiResult a, float b) => new AiResult
    {
        AdditionalLinearForce = a.AdditionalLinearForce / b,
        Firmness = a.Firmness / b
    };

    public static AiResult operator *(AiResult a, int b) => a * (float)b;
}