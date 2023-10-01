using Godot;
using System;
using System.Diagnostics;

public partial class LongLight2D : PointLight2D
{
	[Export] public float CycleTimeMs { get; set; }
	[Export] public float SkewRangeDegrees { get; set; }
	[Export] public Curve LightPath { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var value = (Time.GetTicksMsec() % CycleTimeMs) / CycleTimeMs;
		Skew = Mathf.DegToRad(LightPath.Sample(value));
		Debug.Print(Skew.ToString());
	}
}
