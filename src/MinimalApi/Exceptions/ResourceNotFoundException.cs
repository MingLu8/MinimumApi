using System.Globalization;

namespace MinimumApi.Exceptions
{
    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException() : base() { }

        public ResourceNotFoundException(string message) : base(message) { }

        public ResourceNotFoundException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
