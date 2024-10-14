using Vogen;

namespace Setlistbot.Domain
{
    [ValueObject<int>(conversions: Conversions.TypeConverter | Conversions.NewtonsoftJson)]
    public readonly partial struct PositiveInt
    {
        private static Validation Validate(int value) =>
            value > 0 ? Validation.Ok : Validation.Invalid("Value must be greater than 0.");
    }
}
