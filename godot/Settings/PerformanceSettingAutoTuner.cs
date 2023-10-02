using Godot;

namespace LudumDare54.Settings;

public partial class PerformanceSettingAutoTuner : Node
{
    public override void _EnterTree()
    {
        SettingsSingleton.Overwrite(s => s.IsInTunableScene += 1);
    }

    public override void _ExitTree()
    {
        SettingsSingleton.Overwrite(s => s.IsInTunableScene -= 1);
    }
}
