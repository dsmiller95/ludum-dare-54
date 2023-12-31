using DotnetLibrary.Audience.Factors;
using Godot;

namespace DotnetLibrary.Audience;

/// <summary>
/// may be a:
/// identities:
/// stoner: toked out single actor
/// speed: enraged, moshing
/// alcohol: wobbling, erratic
/// ecstacy: looking to contact, engage
///
/// factors:
/// rage vs calm aka threat value
///     highly enraged starts moshing very easily
///     bumping into people increases this
/// motor control factor
///     lower factor makes them move erratically, and may not be able to move normally towards their goal
/// repellent vs attractant
///     how much other crowd members want to move towards this member
/// avoidance of drama
///     actively moves away from excited states
///
/// needs:
///     info about direct collisions with other people
///     info about proximity to other people
///         either a list every frame or enter/exit events
///         details about the state of the other people
///     constant reactive push? or a event
///
/// states:
///     rage
///     moshing
///     chilling
///
/// 
/// 
/// stretch goals:
///     a large group acting as one identity, once enraged
///     queueing actors. if you interrupt their queue, they get real mad
/// 
/// </summary>

public record struct PushEvent(Vector2 PushForce);


public record struct CrowdActorEffect
{
    public static readonly CrowdActorEffect Zero = new CrowdActorEffect();
    
    public float EnragedEffect; // red eyes, red light maybe
    public float DrunkEffect; // drunk lines
    public float AlertEffect; // an alert bubble
}