namespace Lararium.Core
{
    public interface IUser
    {
        public Guid Id { get; set; }
        /// <summary>
        /// Имя
        /// </summary>
        public string? FirstName { get; set; }
        /// <summary>
        /// Фамилия
        /// </summary>
        public string? MiddleName { get; set; }
        /// <summary>
        /// Surname
        /// </summary>
        public string? LastName { get; set; }
        /// <summary>
        /// Login
        /// </summary>
        public string? Login { get; set; }
        /// <summary>
        /// Gets or sets a salted and hashed representation of the password for this user.
        /// </summary>
        public string? PasswordHash { get; set; }
    }
}
