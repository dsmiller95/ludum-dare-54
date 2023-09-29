using Godot;
using System;

public partial class FirstScript : Node
{
	private AnotherScript proxy = new AnotherScript();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		proxy._Ready();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		proxy._Process(delta);
		GD.Print(proxy.Time);
	}
}
