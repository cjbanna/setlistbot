using EnsureThat;
using StronglyTypedPrimitives;
using StronglyTypedPrimitives.Attributes;

namespace Setlistbot.Domain
{
    [StronglyTyped(Template.String)]
    public readonly partial struct NonEmptyString
    {
        public NonEmptyString(string value) =>
            _value = EnsureArg.IsNotEmptyOrWhiteSpace(value, nameof(value));
    }
}
