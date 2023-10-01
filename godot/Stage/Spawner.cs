using System;
using System.IO;
using DotnetLibrary;
using Godot;
using LudumDare54;
using LudumDare54.Audience;

public partial class Spawner : CollisionShape2D
{
	[Export] private CrowdCorral CrowdCorral { get; set; }
	
	[Export] private PackedScene SceneToSpawn { get; set; }
	
	[Export] private int SpawnNum { get; set; } = 100;
	
	[Export] private SampleType SampleType { get; set; } = SampleType.Halton;

	[Export] private int FacingSkew { get; set; } = 15;
	
	public override void _Ready()
	{
		Disabled = true;

		if (CrowdCorral is null)
		{
			GD.PrintErr($"Spawner {Name} has no CrowdCorral in scene {GetTree().CurrentScene.Name}");
		}
		
		if (Shape is not RectangleShape2D rectShape)
		{
			GD.PrintErr($"Using invalid shape for spawner {Name}");
			throw new InvalidDataException($"Using invalid shape for spawner {Name}");
		}
		
		var rng = new RandomNumberGenerator();
		rng.Randomize();
		
		ISamplePoints pointSampler = SampleType switch {
			SampleType.Halton => new SampleHaltonPoints(),
			SampleType.Gradient => new SampleGradientPointDensity(),
			_ => throw new ArgumentOutOfRangeException()
		};
		
		foreach (var nextPoint in pointSampler
					 .SampleVector2Field(rng.RandiRange(1, int.MaxValue), rectShape.GetRect(), SpawnNum))
		{
			var instance = SceneToSpawn.Instantiate<Node2D>();
			instance.Position = nextPoint + GlobalPosition;

			var rotation = rng.RandiRange(-1 * FacingSkew, FacingSkew);
			instance.RotationDegrees += rotation;
			CrowdCorral.AddChild(instance);

			if (instance is CrowdActor crowdActor)
			{
				crowdActor.CrowdCorral = CrowdCorral;
			}
		}
	}
}
