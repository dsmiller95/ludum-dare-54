using Godot;

namespace DotnetLibrary;

public interface ISamplePoints
{
    public IEnumerable<Vector2> SampleVector2Field(
        int seed,
        Rect2 spawnArea,
        int sampleNum = 100);
}