using System;
using Godot;
using LudumDare54.Audience;

namespace LudumDare54.Settings;

public static class SettingsSingleton
{
    public static Settings Settings = null;
    public static event Action OptionsUpdate;

    public static void Overwrite(Action<Settings> options)
    {
        if (Settings == null)
        {
            bool once = false;
            OptionsUpdate += () =>
            {
                if (once) return;
                once = true;
                options(Settings);
                OptionsUpdate?.Invoke();
            };
        }
        else
        {
            options(Settings);
            OptionsUpdate?.Invoke();
        }
    }
    
    public static void Ready(Settings options)
    {
        Settings = options;
        OptionsUpdate?.Invoke();
    }

    public static void InitAndListen(Action<Settings> initOptions)
    {
        if (Settings == null)
        {
            bool once = false;
            OptionsUpdate += () =>
            {
                if (once) return;
                once = true;
                initOptions(Settings);
            };
        }
        else
        {
            initOptions(Settings);
        }
    }

    public static void InitOrWaitForReady(Action<Settings> initOptions)
    {
        if (Settings == null)
        {
            bool once = false;
            OptionsUpdate += () =>
            {
                if (once) return;
                once = true;
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
    public bool EnableDynamicTuning = true;

    public int IsInTunableScene = 0;
}

public partial class SettingsLoader : Node
{
    private float fpsSmoothingConstant = 10f;
    private float targetFps = 60;
    private int currentQuality = 1;

    private float lastQualityTransition = 0f;
    private float timePerTransition = 3f;
    
    private float[] smoothedFpsPerQuality = new float[3];

    private float logDebounce = 0;
    private float maxDeltaPerLog = 0.25f;
    
    public override void _Ready()
    { 
        var tuningResource = GD.Load<SettingsResource>("res://Settings/settings_store.tres");

        SettingsSingleton.Ready(tuningResource.GetSettings());
        var currentTime = Time.GetTicksMsec() / 1000f;
        lastQualityTransition = currentTime;

        smoothedFpsPerQuality = new float[3];
        Array.Fill(smoothedFpsPerQuality, targetFps);
    }
    
    public override void _Process(double delta)
    {
        var floatDelta = (float)delta;
        
        smoothedFpsPerQuality[currentQuality] = Mathf.Lerp(
            smoothedFpsPerQuality[currentQuality],
            (float)Engine.GetFramesPerSecond(),
            fpsSmoothingConstant * floatDelta);
        
        var currentTime = Time.GetTicksMsec() / 1000f;
        if (currentTime - lastQualityTransition < timePerTransition) return;
        if (SettingsSingleton.Settings.EnableDynamicTuning == false) return;
        if (SettingsSingleton.Settings.IsInTunableScene <= 0)
        {
            Array.Fill(smoothedFpsPerQuality, targetFps);
            return;
        }
        if (SettingsSingleton.Settings.IsInTunableScene > 1)
        {
            GD.PrintErr("SETTINGS: multiple tunable scenes detected, this is not supported");
        }

        logDebounce += floatDelta;
        if (logDebounce > maxDeltaPerLog)
        {
            logDebounce = 0;
            GD.Print($"SETTINGS: smoothed fps: {smoothedFpsPerQuality[currentQuality]}");
        }

        var nextQuality = currentQuality;
        if (smoothedFpsPerQuality[currentQuality] <= targetFps / 2)
        {
            nextQuality = Mathf.Max(nextQuality - 1, 0);
        }
        if (smoothedFpsPerQuality[currentQuality] >= (targetFps * 2) - 0.01f)
        {
            nextQuality = Mathf.Min(nextQuality + 1, 2);
            if (smoothedFpsPerQuality[nextQuality] <= targetFps / 2)
            {
                // lock us out of going back up to next quality if it definite won't work
                return;
            }
        }
        if (nextQuality == currentQuality) return;
        
        GD.Print($"SETTINGS: tuning to quality level {nextQuality}");
        lastQualityTransition = currentTime;
        currentQuality = nextQuality;
        
        SettingsSingleton.Overwrite( s =>
        {
            s.UseNeighborCalculation = nextQuality switch
            {
                0 => false,
                1 => true,
                2 => true,
                _ => throw new ArgumentOutOfRangeException()
            };
        });
    }
}