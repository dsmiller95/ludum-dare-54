using Godot;

namespace DotnetLibrary.Audience.Factors;

public interface IFactorEffect
{
    public AiResult GetFactorEffect(AiParams parameters);
}
