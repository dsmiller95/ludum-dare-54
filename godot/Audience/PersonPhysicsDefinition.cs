using DotnetLibrary;
using DotnetLibrary.Audience;
using Godot;

namespace LudumDare54.Audience;

[GlobalClass]
public partial class PersonPhysicsDefinition : Resource
{
    [Export] public int RotationalAcceleration { get; set; } = 400; // How much force will apply to keep the player facing forward (kg pixels^2 / sec^2 radians) aka (Torque / radian)
    [Export] public int MaximumVelocity { get; set; } = 400; // the max velocity the player will move (pixels/sec).
    [Export] public int ActiveFrictionCoefficient { get; set; } = 10; // Resistance to movement. force / velocity (kg/s)

    [Export] public int MaximumAcceleration { get; set; } = 10000;
    [Export] public int MaximumTorque { get; set; } = 100000;
    

    public PersonPhysics GetConfiguredPhysics()
    {
        return new PersonPhysics
        {
            RotationalAcceleration = RotationalAcceleration,
            MaximumVelocity = MaximumVelocity,
            ActiveFrictionCoefficient = ActiveFrictionCoefficient,
            MaximumAcceleration = MaximumAcceleration,
            MaximumTorque = MaximumTorque,
        };
    }
}