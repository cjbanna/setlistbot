using CSharpFunctionalExtensions;
using Newtonsoft.Json;

namespace Setlistbot.Domain.UnitTests
{
    public class ArtistTests
    {
        [Fact]
        public void Test()
        {
            // TODO
            var maybe = Maybe<string>.From("Bar");
            var testType = new TestType(maybe);
            var json = JsonConvert.SerializeObject(
                testType,
                Formatting.Indented,
                new JsonSerializerSettings() { Converters = { new MaybeJsonConverter<string>() } }
            );
        }
    }

    public record TestType(Maybe<string> Foo);

    public class MaybeJsonConverter<T> : JsonConverter<Maybe<T>>
    {
        public override void WriteJson(
            JsonWriter writer,
            Maybe<T> value,
            JsonSerializer serializer
        ) => value.Match(some => serializer.Serialize(writer, some), writer.WriteNull);

        public override Maybe<T> ReadJson(
            JsonReader reader,
            Type objectType,
            Maybe<T> existingValue,
            bool hasExistingValue,
            JsonSerializer serializer
        )
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return Maybe<T>.None;
            }

            var value = serializer.Deserialize<T>(reader);
            return Maybe<T>.From(value);
        }
    }
}
