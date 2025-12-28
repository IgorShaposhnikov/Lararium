using System.Diagnostics.CodeAnalysis;
using System.Security.Authentication;

namespace Lararium.Authorization.Jwt.Exceptions
{
    public class UserNotFoundException(string identifier) : AuthenticationException($"User with login: '{identifier}' not found")
    {
        public static void ThrowIfNull([NotNull] object? argument, string login)
        {
            if (argument is null)
            {
                Throw(login);
            }
        }

        [DoesNotReturn]
        internal static void Throw(string? paramName) => throw new ArgumentNullException(paramName);
    }
}
