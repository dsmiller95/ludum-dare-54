using Godot;
using System;

public partial class barrier_vertical : Node2D
{

	private Sprite2D sprite;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sprite = (Sprite2D)GetNode("Sprite2D");
		sprite.ZIndex = (int)Position.Y/10;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
