namespace DotnetLibrary.Audience.Factors;

public interface IProvideFactorOverride
{
    public Factors GetOverrideFactor();
    /// <summary>
    /// when true, will overwrite all factors with this provider's factors every frame.
    /// When false, overrides will only be used as a default.
    /// </summary>
    public bool UseLiveOverride { get; }
}