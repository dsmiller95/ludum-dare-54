using DotnetLibrary;
using DotnetLibrary.Audience;
using Godot;

namespace LudumDare54.Audience;

public class PersonBody
{
    private RigidBody2D attachedBody;
    private MovingAverage AverageSpeed = new(15);

    public PersonBody(RigidBody2D attachedBody)
    {
        this.attachedBody = attachedBody;
    }

    public float GetAverageSpeed()
    {
        return AverageSpeed.Average;
    }
    
    public void _PhysicsProcess()
    {
        AverageSpeed.ComputeAverage(attachedBody.LinearVelocity.Length());
    }
    
}

public interface IHavePersonBody
{
    public PersonBody GetBody();
}