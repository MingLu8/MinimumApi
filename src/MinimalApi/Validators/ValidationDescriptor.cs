using FluentValidation;

namespace MinimumApi.Validators
{

    public static partial class ValidationFilter
    {
        private class ValidationDescriptor
        {
            public required int ArgumentIndex { get; init; }
            public required Type ArgumentType { get; init; }
            public required IValidator Validator { get; init; }
        }
    }
}
