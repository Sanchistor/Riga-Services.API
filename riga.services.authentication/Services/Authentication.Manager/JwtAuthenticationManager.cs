using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Tokens;
using riga.services.DbContext;
using riga.services.Models;

namespace riga.services.riga.services.authentication.Services;

public class JwtAuthenticationManager : IJwtAuthenticationManager
    {
        private readonly string key;
        private readonly ApiDbContext dbContext;

        public JwtAuthenticationManager(string key, ApiDbContext dbContext)
        {
            this.key = key;
            this.dbContext = dbContext;
        }

        public string Authenticate(string email, string password)
        {
            if (!IsValidEmail(email))
            {
                return "Invalid email format";
            }

            // Fetch user from the database using the provided email
            User user = dbContext.Users.FirstOrDefault(u => u.Email == email);

            // Auth failed - user not found
            if (user == null)
            {
                return "User not found";
            }

            // Verify if the provided password matches the user's hashed password
            if (!VerifyPassword(password, user.Password))
            {
                return "Invalid password"; // Incorrect password
            }

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Email, email)
                }),
                //set duration of token here
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature) // sha256 algorithm
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private bool IsValidEmail(string email)
        {
            const string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, emailPattern);
        }
        private bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }