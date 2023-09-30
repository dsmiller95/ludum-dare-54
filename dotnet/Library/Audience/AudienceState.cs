using Godot;

namespace DotnetLibrary.Audience;

public enum AudienceState
{
    Rage,
    Moshing,
    Chilling
}

/// <summary>
/// all factors vary between 0..1
/// </summary>
public record struct AudienceFactors
{
    /// <summary>
    /// high value encourages entry into the Rage state.
    /// Higher values lead to actively punching, attacking
    /// </summary>
    public float RageThreshold;
    
    /// <summary>
    /// lower factor makes them move erratically, and may not be able to move normally
    /// </summary>
    public float MotorControl;
    
    /// <summary>
    /// how much other crowd members want to move towards this member
    /// </summary>
    public float Attractant;
    
    /// <summary>
    /// when low, actor will actively avoid other actors in very excited states
    /// when mid, actor will tit-for-tat. if pushed, push back
    /// when high, actor will escalate. if pushed, punch back
    /// </summary>
    public float FlightVsFight;
}