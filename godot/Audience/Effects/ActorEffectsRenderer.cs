using DotnetLibrary.Audience;
using Godot;

namespace LudumDare54.Audience;

public partial class ActorEffectsRenderer : Node2D
{
    [Export] private AnimatedSprite2D bodySprite;

    public void RenderEffects(CrowdActorEffect effects)
    {
        bodySprite.SelfModulate = Colors.White
            .Lerp(Colors.Red, effects.EnragedEffect);
    }
}