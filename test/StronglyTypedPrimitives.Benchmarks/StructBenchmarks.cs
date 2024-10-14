using BenchmarkDotNet.Attributes;
using EnsureThat;
using Setlistbot.Domain;

namespace StronglyTypedPrimitives.Benchmarks
{
    [MemoryDiagnoser]
    public class StructBenchmarks
    {
        [Benchmark]
        public string ArtistName()
        {
            var artistName = new ArtistName("The Beatles");
            return artistName;
        }

        [Benchmark(Baseline = true)]
        public string ArtistName2()
        {
            var artistId = new ArtistId2("The Beatles");
            return artistId;
        }
    }

    public sealed record ArtistId2
    {
        private readonly string _artistId = string.Empty;

        public ArtistId2(NonEmptyString artistId)
        {
            _artistId = EnsureArg.IsNotNullOrWhiteSpace(artistId, nameof(artistId)).ToLower();
        }

        public static implicit operator string(ArtistId2 artistId) => artistId._artistId;
    }
}
