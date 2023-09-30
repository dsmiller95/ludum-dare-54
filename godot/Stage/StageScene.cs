using System;
using DotnetLibrary;
using Godot;
using LudumDare54;

public partial class StageScene : Node2D
{
	[Export] private PackedScene SceneToSpawn { get; set; }

	[Export] private Rect2 SpawnArea { get; set; } = new(10, 10, 500, 500);
	
	[Export] private int SpawnNum { get; set; } = 100;
	
	[Export] private SampleType SampleType { get; set; } = SampleType.Halton;

	[Export] private int FacingSkew { get; set; } = 15;
	
	public override void _Ready()
	{
		var rng = new RandomNumberGenerator();
		rng.Randomize();
		
		ISamplePoints pointSampler = SampleType switch {
			SampleType.Halton => new SampleHaltonPoints(),
			SampleType.Gradient => new SampleGradientPointDensity(),
			_ => throw new ArgumentOutOfRangeException()
		};
		
		foreach (var nextPoint in pointSampler
					 .SampleVector2Field(rng.RandiRange(1, int.MaxValue), SpawnArea, SpawnNum))
		{
			var instance = SceneToSpawn.Instantiate<Node2D>();
			instance.Position = nextPoint;

			var rotation = rng.RandiRange(-1 * FacingSkew, FacingSkew);
			instance.RotationDegrees += rotation;
			
			GD.Print(nextPoint);
			AddChild(instance);
		}
	}
}
