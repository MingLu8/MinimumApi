    using System.Globalization;

namespace MinimumApi.Exceptions
{
    public class ValidationFailedException : Exception
    {
        public IDictionary<string, string[]>? Errors { get; }

        public ValidationFailedException() : base() { }

        public ValidationFailedException(string message, IDictionary<string, string[]> errors) : base(message)
        {
            Errors = errors;
        }
    }
}
