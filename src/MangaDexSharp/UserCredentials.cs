using System;

namespace MangaDexSharp
{
    //TODO: Validate username length and password length
    /// <summary>
    /// User credentials for login
    /// </summary>
    public struct UserCredentials
    {
        public string Email { get; }
        public string Password {  get; }
        public string Username { get; }

        public UserCredentials(string username, string email, string password)
        {
            Username = username;
            //TODO: Validate email format
            Email = email;
            Password = password;
        }
    }
}
