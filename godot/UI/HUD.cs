using Godot;
using System;

public partial class HUD : CanvasLayer
{
	
	[Export] public Node2D Goal;
	[Export] public Node2D player;
	public Sprite2D sprite;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sprite = (Sprite2D)GetNode("Pointer").GetNode("PointerSprite");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 lookVector = player.Position.DirectionTo(Goal.Position);
		
		
		double det = (lookVector.X * (-1)) - (lookVector.Y * 0);
		
		double dot = lookVector.Dot(Vector2.Up);
		
		double ang = (180 / Math.PI) * Math.Atan2(dot,det);
		sprite.RotationDegrees = (float)ang - 90;
	}
}
