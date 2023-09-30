using System.Linq;
using DotnetLibrary;
using Godot;

namespace LudumDare54;

public partial class RandomSpawner : Node2D
{
    [Export] private Node2D ParentNode { get; set; }
    [Export] private PackedScene SceneToSpawn { get; set; }

    [Export] private Rect2 SpawnArea { get; set; } = new Rect2(10, 10, 500, 500);
    [Export] private int SpawnNum { get; set; } = 100;
    
    public override void _Ready()
    {
        var rng = new RandomNumberGenerator();
        rng.Randomize();
        ISamplePoints pointSampler = new SampleHaltonPoints();
        foreach (var nextPoint in pointSampler
                     .SampleVector2Field(rng.RandiRange(1, int.MaxValue), SpawnArea, SpawnNum))
        {
            var instance = SceneToSpawn.Instantiate<Node2D>();
            instance.GlobalPosition = this.ToGlobal(nextPoint);
            ParentNode.AddChild(instance);
        }
    }
}