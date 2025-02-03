using Vogen;

namespace Setlistbot.Domain
{
    [ValueObject<int>]
    public readonly partial struct PositiveInt
    {
        private static Validation Validate(int value) =>
            value > 0 ? Validation.Ok : Validation.Invalid("Value must be greater than 0.");
    }
}
