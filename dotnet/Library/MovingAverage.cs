namespace DotnetLibrary;

public class MovingAverage  
{
    private readonly Queue<float> samples = new();
    private readonly int _windowSize = 16;
    private float sampleAccumulator;
    public float Average { get; private set; }

    public MovingAverage(int windowSize)
    {
        _windowSize = windowSize;
    }

    /// <summary>
    /// Computes a new windowed average each time a new sample arrives
    /// </summary>
    /// <param name="newSample"></param>
    public void ComputeAverage(float newSample)
    {
        sampleAccumulator += newSample;
        samples.Enqueue(newSample);

        if (samples.Count > _windowSize)
        {
            sampleAccumulator -= samples.Dequeue();
        }

        Average = sampleAccumulator / samples.Count;
    }
}