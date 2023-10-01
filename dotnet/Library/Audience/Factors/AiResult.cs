using Godot;

namespace DotnetLibrary.Audience.Factors;

public struct AiResult
{
    public Vector2 AdditionalLinearForce;
    public float Firmness;
    

    public static AiResult operator +(AiResult a, AiResult b) => new AiResult
    {
        AdditionalLinearForce = a.AdditionalLinearForce + b.AdditionalLinearForce
    };

    public static AiResult operator *(AiResult a, float b) => new AiResult
    {
        AdditionalLinearForce = a.AdditionalLinearForce / b
    };
    public static AiResult operator *(AiResult a, int b) => new AiResult
    {
        AdditionalLinearForce = a.AdditionalLinearForce / (float)b
    };
}