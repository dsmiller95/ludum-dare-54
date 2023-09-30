﻿using Godot;

namespace DotnetLibrary;

public static class RectExtensions
{
    
    public static Vector2 RestrictInside(this Rect2 rect, Vector2 vector)
    {
        return new Vector2(
            Mathf.Clamp(vector.X, rect.Position.X, rect.Position.X + rect.Size.X),
            Mathf.Clamp(vector.Y, rect.Position.Y, rect.Position.Y + rect.Size.Y)
        );
    }
}