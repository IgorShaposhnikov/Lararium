namespace Lararium.Authorization.Jwt.Models.Requests;

public sealed class RegisterRequest
{
    /// <summary>
    /// The user's email address.
    /// </summary>
    public string Login { get; set; } = string.Empty;
    /// <summary>
    /// The password.
    /// </summary>
    public string Password { get; set; } = string.Empty;
    /// <summary>
    /// The password confirmation.
    /// </summary>
    public string PasswordConfirmation { get; set; } = string.Empty;
    /// <summary>
    /// The user's first name.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;
    /// <summary>
    /// The user's middle name or patronymic.
    /// </summary>
    public string? MiddleName { get; set; } = string.Empty;
    /// <summary>
    /// The user's last name.
    /// </summary>
    public string LastName { get; set; } = string.Empty;
}
