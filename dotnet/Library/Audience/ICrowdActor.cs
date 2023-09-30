using Godot;

namespace DotnetLibrary.Audience;

public interface ICrowdActor
{
    /// <summary>
    /// must be called before interacting with any other methods, with the correct delta time since this function was last called
    /// </summary>
    /// <param name="deltaTime"></param>
    public void Update(double deltaTime);
    public void ReceivePushEvent(PushEvent pushEvent);

    public float GetFirmness();

    public Vector2 GetCurrentSelfMoveForce();
}

public record struct PushEvent(Vector2 PushForce);