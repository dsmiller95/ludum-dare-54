using Godot;
using System;

public partial class StageLight : DirectionalLight2D
{
	[Export] public float Bpm { get; set; } = 120;
	[Export] public Curve LightIntensity { get; set; }

	public override void _Process(double delta)
	{
		var value = (Time.GetTicksMsec() % (1000 / (Bpm / 60))) / (1000 / (Bpm / 60));
		Energy = LightIntensity.Sample(value);
	}
}
