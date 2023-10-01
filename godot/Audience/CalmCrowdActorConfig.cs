using DotnetLibrary.Audience;
using Godot;

namespace LudumDare54.Audience;

[GlobalClass]
public partial class CalmCrowdActorConfig : Resource, ICrowdActorPreset
{
    [Export]
    private float calmLevel = 0.5f;
    public ICrowdActor ConstructConfiguredActor()
    {
        return new CalmCrowdActor(calmLevel);
    }
}