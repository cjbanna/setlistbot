using System;

namespace StronglyTypedPrimitives.Attributes
{
    [AttributeUsage(AttributeTargets.Struct)]
    public class StronglyTypedAttribute : Attribute
    {
        public StronglyTypedAttribute(Template template)
        {
            Template = template;
        }

        public Template Template { get; }
    }
}
