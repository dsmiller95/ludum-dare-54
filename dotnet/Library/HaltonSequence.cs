using Godot;

namespace DotnetLibrary;

/// <summary>
/// An utility class to compute samples on the Halton sequence.
/// https://en.wikipedia.org/wiki/Halton_sequence
/// </summary>
public static class HaltonSequence
{
    /// <summary>
    /// Gets a deterministic sample in the Halton sequence.
    /// </summary>
    /// <param name="index">The index in the sequence.</param>
    /// <param name="radix">The radix of the sequence.</param>
    /// <returns>A sample from the Halton sequence.</returns>
    public static float Get(int index, int radix)
    {
        float result = 0f;
        float fraction = 1f / radix;

        while (index > 0)
        {
            result += (index % radix) * fraction;

            index /= radix;
            fraction /= radix;
        }

        return result;
    }

    public static IEnumerable<Vector2> SampleVector2Field(
        int horizontalRadix,
        int verticalRadix,
        int seed, 
        Vector2 maxVector,
        Vector2 minVector,
        int maxSamples = 10000)
    {
        var indexInSequence = seed;
        var scale                = maxVector - minVector;

        var i = 0;
        while (i++ < maxSamples)
        {
            var sample = new Vector2(
                Get(indexInSequence, horizontalRadix),
                Get(indexInSequence, verticalRadix)
            );
            indexInSequence++;

            yield return sample * scale + minVector;
        }
    }
}

public class SampleHaltonPoints : ISamplePoints
{
    public IEnumerable<Vector2> SampleVector2Field(
        int seed,
        Rect2 spawnArea,
        int sampleNum = 100)
    {
        return HaltonSequence.SampleVector2Field(
            2,
            3,
            seed,
            spawnArea.End,
            spawnArea.Position,
            sampleNum);
    }
}