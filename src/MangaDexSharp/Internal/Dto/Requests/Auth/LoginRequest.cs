#nullable disable
using System;
using System.Text.Json.Serialization;

namespace MangaDexSharp.Internal.Dto.Requests.Auth
{
    internal class LoginRequest
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }

        public LoginRequest(string username, string password)
        {
            if(username == null)
            {
                throw new ArgumentNullException(nameof(username));
            }
            if (password == null)
            {
                throw new ArgumentNullException(nameof(username));
            }
            Username = username;
            Password = password;
        }

        public LoginRequest(string username, string email, string password)
        {
            if (username == null && email == null)
            {
                throw new ArgumentException("Login request must have username or password");
            }
            if (password == null)
            {
                throw new ArgumentNullException(nameof(username));
            }
            Username = username ?? "";
            Email = email ?? "";
            Password = password;
        }
    }
}
