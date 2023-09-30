using Godot;

namespace LudumDare54.Audience;

public partial class RandomAnimationStart : AnimatedSprite2D
{

    private ulong startTime = 0;
    public override void _Ready()
    {
        startTime = (ulong)(GD.Randf() * 1000) + Time.GetTicksMsec();
    }

    public override void _Process(double delta)
    {
        if (startTime <= Time.GetTicksMsec())
        {
            this.Play();
        }
    }
}