using OpenTelemetry.Trace;

namespace Clients.Api.Diagnostics;

public class RateSampler: Sampler
{
    private double _sampleRate;
    private Random _random;
    
    public RateSampler(double sampleRate)
    {
        _sampleRate = sampleRate;
        _random = new Random();
    }
    
    public override SamplingResult ShouldSample(in SamplingParameters samplingParameters)
    {
        var shouldBeSample = _random.NextDouble() < _sampleRate;
        if (shouldBeSample)
            return new SamplingResult(SamplingDecision.RecordAndSample);
        
        
        return new SamplingResult(SamplingDecision.Drop);
    }
}