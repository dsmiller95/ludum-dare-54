using DotnetLibrary.Audience;
using Godot;

namespace LudumDare54.Audience;

public interface ICrowdActorPreset
{
    public ICrowdActor ConstructConfiguredActor();
}