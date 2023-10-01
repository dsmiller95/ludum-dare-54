using Godot;
using System;

public static class Health
{
    public static int HealthValue;

    public static int AdjustHealth(int adjustment)
    {
        return HealthValue += adjustment;
    }
}
