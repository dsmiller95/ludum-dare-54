using DotnetLibrary;
using DotnetLibrary.Audience;
using Godot;

namespace LudumDare54.Audience;

public class PersonBody
{
    private RigidBody2D attachedBody;
    private Vector2 lastLinearVelocity = Vector2.Zero;

    public PersonBody(RigidBody2D attachedBody)
    {
        this.attachedBody = attachedBody;
    }

    public Vector2 GetVelocityDelta()
    {
        return attachedBody.LinearVelocity - lastLinearVelocity;
    }
    
    public void _PhysicsProcess()
    {
        this.lastLinearVelocity = attachedBody.LinearVelocity;
    }
    
}

public interface IHavePersonBody
{
    public PersonBody GetBody();
}