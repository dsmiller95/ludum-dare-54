using Godot;
using System;

public partial class SillyScript : Sprite2D
{
    public int health;

    public override void _Ready(){
        health = Health.HealthValue;
    }
}
