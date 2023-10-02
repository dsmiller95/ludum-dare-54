using Godot;

namespace LudumDare54.Audience;

public partial class RandomAnimationStart : Node
{
    [Export]
    public AnimatedSprite2D AnimatingTarget;

    private ulong startTime = 0;
    public override void _Ready()
    {
        startTime = (ulong)(GD.Randf() * 1000) + Time.GetTicksMsec();
    }

    public override void _Process(double delta)
    {
        if (startTime <= Time.GetTicksMsec())
        {
            AnimatingTarget.Play();
            QueueFree();
        }
    }
}