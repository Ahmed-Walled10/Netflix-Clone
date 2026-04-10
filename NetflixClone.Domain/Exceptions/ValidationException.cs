using System.Collections.Generic;

namespace NetflixClone.Domain.Exceptions
{
    public class ValidationException : AppException
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException(string message) : base(message)
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IDictionary<string, string[]> errors) 
            : base("One or more validation failures have occurred.")
        {
            Errors = errors;
        }
    }
}
