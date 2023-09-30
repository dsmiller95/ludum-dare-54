using Godot;

namespace DotnetLibrary;

public class SampleGradientPointDensity : ISamplePoints
{
    private readonly float jitterPercentageOfSpawnArea;
    private readonly int iterations;

    public SampleGradientPointDensity(float jitterPercentageOfSpawnArea = 0.05f, int iterations = 100)
    {
        this.jitterPercentageOfSpawnArea = jitterPercentageOfSpawnArea;
        this.iterations = iterations;
    }
    
    public IEnumerable<Vector2> SampleVector2Field(int seed, Rect2 spawnArea, int sampleNum = 100)
    {
        var gradientSampler = new FastNoiseLite();
        gradientSampler.NoiseType = FastNoiseLite.NoiseTypeEnum.Simplex;
        gradientSampler.Seed = seed;
        
        var baseSampler = new SampleHaltonPoints();
        var samples = baseSampler.SampleVector2Field(seed, spawnArea, sampleNum)
            .ToArray();

        var jitterMagnitude = Mathf.Min(spawnArea.Size.X, spawnArea.Size.Y) * jitterPercentageOfSpawnArea;

        var rng = new RandomNumberGenerator();
        rng.Randomize();


        for (int iter = 0; iter < iterations; iter++)
        {
            var jitterMultiplier = Mathf.Pow((1f - (float)iter / iterations), 0.1f) * jitterMagnitude;
            for (int i = 0; i < samples.Length; i++)
            {
                var sample = samples[i];
                var density = gradientSampler.GetNoise2Dv(sample);

                var randVec = rng.NextVector2(-1, 1).Normalized() * density * jitterMultiplier;
                
                var jitteredSample = sample + randVec;
                jitteredSample = spawnArea.RestrictInside(jitteredSample);
                samples[i] = jitteredSample;
            }
        }

        return samples;
    }
}