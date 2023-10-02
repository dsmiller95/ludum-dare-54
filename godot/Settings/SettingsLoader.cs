using System;
using Godot;
using LudumDare54.Audience;

namespace LudumDare54.Settings;

public static class SettingsSingleton
{
    public static Settings Settings = null;
    public static event Action OptionsReady;

    public static void Ready(Settings options)
    {
        Settings = options;
        OptionsReady?.Invoke();
    }

    public static void InitOrWaitForReady(Action<Settings> initOptions)
    {
        if (Settings == null)
        {
            OptionsReady += () =>
            {
                initOptions(Settings);
            };
        }
        else
        {
            initOptions(Settings);
        }
    }
}

public class Settings
{
    public bool UseNeighborCalculation = true;
    public DebugViewOption DisplayDebugFactors = DebugViewOption.RawFactors;
}

public partial class SettingsLoader : Node
{
    public override void _Ready()
    { 
        var tuningResource = GD.Load<SettingsResource>("res://Settings/settings_store.tres");
        
        SettingsSingleton.Ready(new Settings
        {
            UseNeighborCalculation = tuningResource.UseNeighborCalculation,
            DisplayDebugFactors = tuningResource.DisplayDebugFactors
        });
    }
}