using Godot;
using LudumDare54.Audience;

namespace LudumDare54.Settings;

[GlobalClass]
public partial class SettingsResource : Resource
{
    [Export] public bool UseNeighborCalculation = true;
    [Export] public DebugViewOption DisplayDebugFactors = DebugViewOption.RawFactors;
}