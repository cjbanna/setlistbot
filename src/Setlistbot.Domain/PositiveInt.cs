using EnsureThat;
using StronglyTypedPrimitives;
using StronglyTypedPrimitives.Attributes;

namespace Setlistbot.Domain
{
    [StronglyTyped(Template.Int)]
    public readonly partial struct PositiveInt
    {
        public PositiveInt(int value) => _value = EnsureArg.IsGt(value, 0, nameof(value));
    }
}
